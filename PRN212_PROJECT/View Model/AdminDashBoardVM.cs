using PRN212_PROJECT.Models;
using PRN212_PROJECT.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PRN212_PROJECT.View_Model
{
    public class TopFoodItem
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public string TypeName { get; set; }
        public int OrderCount { get; set; }
    }

    public class AdminDashBoardVM : BaseViewModel
    {
        // Properties cho Header
        private string _adminName;
        public string AdminName
        {
            get => _adminName;
            set
            {
                _adminName = value;
                OnPropertyChanged();
            }
        }

        public ICommand LogoutCommand { get; }

        // Properties và Commands cho Quick Access Panel
        public ICommand GoOrderCommand { get; }
        public ICommand GoCategoryCommand { get; }
        public ICommand GoRevenueCommand { get; }
        public ICommand GoStaffCommand { get; }

        // Properties cho Top 5 món ăn bán chạy
        private ObservableCollection<TopFoodItem> _topSellingFoods;
        public ObservableCollection<TopFoodItem> TopSellingFoods
        {
            get => _topSellingFoods;
            set
            {
                _topSellingFoods = value;
                OnPropertyChanged();
            }
        }

        // Properties cho Dashboard/Stats
        private int _todayOrders;
        private string _todayOrdersPercentage;
        private string _revenue;
        private string _revenuePercentage;
        private TopFoodItem _topSellingFood;
        private ObservableCollection<OrderTable> _recentOrders;

        public int TodayOrders
        {
            get => _todayOrders;
            set
            {
                _todayOrders = value;
                OnPropertyChanged();
            }
        }

        public string TodayOrdersPercentage
        {
            get => _todayOrdersPercentage;
            set
            {
                _todayOrdersPercentage = value;
                OnPropertyChanged();
            }
        }

        public string Revenue
        {
            get => _revenue;
            set
            {
                _revenue = value;
                OnPropertyChanged();
            }
        }

        public string RevenuePercentage
        {
            get => _revenuePercentage;
            set
            {
                _revenuePercentage = value;
                OnPropertyChanged();
            }
        }

        public TopFoodItem TopSellingFood
        {
            get => _topSellingFood;
            set
            {
                _topSellingFood = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<OrderTable> RecentOrders
        {
            get => _recentOrders;
            set
            {
                _recentOrders = value;
                OnPropertyChanged();
            }
        }

        public AdminDashBoardVM()
        {
            if (ChickenPrnContext.Ins == null)
            {
                MessageBox.Show("ChickenPrnContext.Ins is null");
                return;
            }

            // Khởi tạo dữ liệu cho Header
            AdminName = "Admin";

            // Khởi tạo dữ liệu cho Top 5 món ăn bán chạy
            LoadTopSellingFoods();

            // Khởi tạo dữ liệu cho Dashboard/Stats
            LoadStats();
            LoadRecentOrders();

            // Khởi tạo Commands
            LogoutCommand = new RelayCommand(ExecuteLogout);
            GoOrderCommand = new RelayCommand(ExecuteGoOrder);
            GoCategoryCommand = new RelayCommand(ExecuteGoCategory);
            GoRevenueCommand = new RelayCommand(ExecuteGoRevenue);
            GoStaffCommand = new RelayCommand(ExecuteGoStaff);
        }

        private void LoadTopSellingFoods()
        {
            try
            {
                var topFoods = ChickenPrnContext.Ins.OrderTables
                    .Join(ChickenPrnContext.Ins.OrderDetailFoods,
                          ot => ot.OrderId,
                          od => od.OrderId,
                          (ot, od) => new { ot, od })
                    .Join(ChickenPrnContext.Ins.Foods,
                          x => x.od.FoodId,
                          f => f.FoodId,
                          (x, f) => new { x.ot, x.od, f })
                    .Join(ChickenPrnContext.Ins.TypeOfFoods,
                          x => x.f.FoodType,
                          tf => tf.TypeId,
                          (x, tf) => new { x.ot, x.od, x.f, tf })
                    .GroupBy(x => new { x.f.FoodId, x.f.FoodName, x.tf.TypeName })
                    .Select(g => new TopFoodItem
                    {
                        FoodId = g.Key.FoodId,
                        FoodName = g.Key.FoodName,
                        TypeName = g.Key.TypeName,
                        OrderCount = g.Count()
                    })
                    .OrderByDescending(x => x.OrderCount)
                    .Take(5)
                    .ToList();

                TopSellingFoods = topFoods.Any()
                    ? new ObservableCollection<TopFoodItem>(topFoods)
                    : new ObservableCollection<TopFoodItem> { new TopFoodItem
                    {
                        FoodId = 0,
                        FoodName = "Không có dữ liệu",
                        TypeName = "N/A",
                        OrderCount = 0
                    }};
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load top 5 món ăn bán chạy: {ex.Message}");
                TopSellingFoods = new ObservableCollection<TopFoodItem>();
            }
        }

        private void LoadStats()
        {
            try
            {
                // Đơn hôm nay
                var today = DateTime.Today;
                TodayOrders = ChickenPrnContext.Ins.OrderTables
                    .Count(o => o.Date.HasValue && o.Date.Value.Date == today);

                // Phần trăm so với hôm qua
                var yesterday = today.AddDays(-1);
                var yesterdayOrders = ChickenPrnContext.Ins.OrderTables
                    .Count(o => o.Date.HasValue && o.Date.Value.Date == yesterday);
                TodayOrdersPercentage = yesterdayOrders > 0
                    ? $"{((TodayOrders - yesterdayOrders) * 100.0 / yesterdayOrders):F0}% so với HQ"
                    : "N/A";

                // Doanh thu hôm nay
                var todayRevenue = ChickenPrnContext.Ins.OrderTables
                    .Where(o => o.Date.HasValue && o.Date.Value.Date == today)
                    .Sum(o => o.Total);
                Revenue = $"{todayRevenue / 1000000:F1}M";

                // Phần trăm doanh thu so với hôm qua
                var yesterdayRevenue = ChickenPrnContext.Ins.OrderTables
                    .Where(o => o.Date.HasValue && o.Date.Value.Date == yesterday)
                    .Sum(o => o.Total);
                RevenuePercentage = yesterdayRevenue > 0
                    ? $"{((todayRevenue - yesterdayRevenue) * 100.0 / yesterdayRevenue):F0}% so với HQ"
                    : "N/A";

                // Top 5 món bán chạy
                var topSelling = ChickenPrnContext.Ins.OrderTables
                    .Join(ChickenPrnContext.Ins.OrderDetailFoods,
                          ot => ot.OrderId,
                          od => od.OrderId,
                          (ot, od) => new { ot, od })
                    .Join(ChickenPrnContext.Ins.Foods,
                          x => x.od.FoodId,
                          f => f.FoodId,
                          (x, f) => new { x.ot, x.od, f })
                    .Join(ChickenPrnContext.Ins.TypeOfFoods,
                          x => x.f.FoodType,
                          tf => tf.TypeId,
                          (x, tf) => new { x.ot, x.od, x.f, tf })
                    .GroupBy(x => new { x.f.FoodId, x.f.FoodName, x.tf.TypeName })
                    .Select(g => new TopFoodItem
                    {
                        FoodId = g.Key.FoodId,
                        FoodName = g.Key.FoodName,
                        TypeName = g.Key.TypeName,
                        OrderCount = g.Count()
                    })
                    .OrderByDescending(x => x.OrderCount)
                    .Take(1).FirstOrDefault();
                    ;

                TopSellingFood = topSelling ?? new TopFoodItem
                {
                    FoodId = 0,
                    FoodName = "Không có dữ liệu",
                    TypeName = "N/A",
                    OrderCount = 0
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load thống kê: {ex.Message}");
                TodayOrders = 0;
                TodayOrdersPercentage = "N/A";
                Revenue = "0M";
                RevenuePercentage = "N/A";
                TopSellingFood = new TopFoodItem
                {
                    FoodId = 0,
                    FoodName = "Không có dữ liệu",
                    TypeName = "N/A",
                    OrderCount = 0
                };
            }
        }

        private void LoadRecentOrders()
        {
            try
            {
                RecentOrders = new ObservableCollection<OrderTable>(
                    ChickenPrnContext.Ins.OrderTables
                        .OrderByDescending(o => o.Date)
                        .Take(5)
                        .ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load đơn hàng gần đây: {ex.Message}");
                RecentOrders = new ObservableCollection<OrderTable>();
            }
        }

        private void ExecuteLogout(object parameter)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            Application.Current.Windows.OfType<AdminDashBoard>().FirstOrDefault()?.Close();
        }

        private void ExecuteGoOrder(object parameter)
        {
            ManageOrder manageOrderWindow = new ManageOrder();
            manageOrderWindow.Show();
            Application.Current.Windows.OfType<AdminDashBoard>().FirstOrDefault()?.Close();
        }

        private void ExecuteGoCategory(object parameter)
        {
            ManageFood manageFood = new ManageFood();
            manageFood.Show();
            Application.Current.Windows.OfType<AdminDashBoard>().FirstOrDefault().Close();
        }

        private void ExecuteGoRevenue(object parameter)
        {
            ManageProfit profit = new ManageProfit();
            profit.Show();
            Application.Current.Windows.OfType<AdminDashBoard>().FirstOrDefault()?.Close();
        }

        private void ExecuteGoStaff(object parameter)
        {
            MessageBox.Show("Chức năng Nhân viên chưa được triển khai.");
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        public RelayCommand(Action<object> execute) => _execute = execute;
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _execute(parameter);
    }
}
