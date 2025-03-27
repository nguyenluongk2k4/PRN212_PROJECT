using PRN212_PROJECT.Models;
using PRN212_PROJECT.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics; 
using System.Linq;
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
        private List<string> _permissions;

        public List<string> Permissions
        {
            get => _permissions;
            set
            {
                _permissions = value;
                OnPropertyChanged();
                
                InitializeHasPermission();
                OnPropertyChanged(nameof(HasPermission));
            }
        }

        private Dictionary<string, bool> _hasPermission;
        public Dictionary<string, bool> HasPermission
        {
            get => _hasPermission;
            private set
            {
                _hasPermission = value;
                OnPropertyChanged();
            }
        }

        private void InitializeHasPermission()
        {
            _hasPermission = new Dictionary<string, bool>();
            try
            {
                
                var permissionsToCheck = ChickenPrnContext.Ins?.Permissions?.Select(x => x.PermissionName).ToList() ?? new List<string>();

                if (!permissionsToCheck.Any())
                {
                    Debug.WriteLine("Không tìm thấy permissions trong ChickenPrnContext.Ins.Permissions.");
                }
                else
                {
                    Debug.WriteLine("Permissions từ ChickenPrnContext: " + string.Join(", ", permissionsToCheck));
                }

                

                foreach (var permission in permissionsToCheck)
                {
                    try
                    {
                        bool hasPermission = false;
                        if (permissionsToCheck.Contains(permission))
                        {
                            hasPermission = AccountLogin.HasPermission(permission);
                        }
                        _hasPermission[permission] = hasPermission;
                        Debug.WriteLine($"Permission {permission}: {hasPermission}");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Lỗi khi kiểm tra permission {permission}: {ex.Message}");
                        _hasPermission[permission] = false; // Nếu có lỗi, mặc định là false
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi load permissions: {ex.Message}");
                
            }
        }

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
        public ICommand GoOrderCommand { get; }
        public ICommand GoCategoryCommand { get; }
        public ICommand GoRevenueCommand { get; }
        public ICommand GoStaffCommand { get; }
        public ICommand GoRoleManagementCommand { get; }
        public ICommand GoFeedbackManagementCommand { get; }
        public ICommand GoImportGoodsCommand { get; }
        public ICommand GoCreateOrderCommand { get; }

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

        private int _todayOrders;
        private string _todayOrdersPercentage;
        private string _revenue;
        private string _revenuePercentage;
        private TopFoodItem _topSellingFood;
        private ObservableCollection<OrderTable> _recentOrders;
        private ObservableCollection<Feedback> _recentFeeBacks;

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
        public ObservableCollection<Feedback> RecentFeedBack { 
            get => _recentFeeBacks;
            set
            {
                _recentFeeBacks = value;
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

            if (AccountLogin.Permissions == null)
            {
                MessageBox.Show("AccountLogin.Permissions is null!");
                Application.Current.Windows.OfType<AdminDashBoard>().FirstOrDefault()?.Close();
                new Login().Show();
                return;
            }

            

            AdminName = AccountLogin.full_name;

            // Khởi tạo HasPermission ngay trong constructor
            InitializeHasPermission();

            LoadTopSellingFoods();
            LoadStats();
            LoadRecentOrders();
            LoadRecentFeedBack();

            LogoutCommand = new RelayCommand(ExecuteLogout);
            GoOrderCommand = new RelayCommand(ExecuteGoOrder);
            GoCategoryCommand = new RelayCommand(ExecuteGoCategory);
            GoRevenueCommand = new RelayCommand(ExecuteGoRevenue);
            GoStaffCommand = new RelayCommand(ExecuteGoStaff);
            GoRoleManagementCommand = new RelayCommand(ExecuteGoRoleManagement);
            GoFeedbackManagementCommand = new RelayCommand(ExecuteGoFeedbackManagement);
            GoImportGoodsCommand = new RelayCommand(ExecuteGoImportGoods);
            GoCreateOrderCommand = new RelayCommand(ExecuteGoCreateOrder);
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
                    : new ObservableCollection<TopFoodItem>
                    {
                        new TopFoodItem { FoodId = 0, FoodName = "Không có dữ liệu", TypeName = "N/A", OrderCount = 0 }
                    };
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
                var today = DateTime.Today;
                TodayOrders = ChickenPrnContext.Ins.OrderTables
                    .Count(o => o.Date.HasValue && o.Date.Value.Date == today);

                var yesterday = today.AddDays(-1);
                var yesterdayOrders = ChickenPrnContext.Ins.OrderTables
                    .Count(o => o.Date.HasValue && o.Date.Value.Date == yesterday);
                TodayOrdersPercentage = yesterdayOrders > 0
                    ? $"{((TodayOrders - yesterdayOrders) * 100.0 / yesterdayOrders):F0}% so với HQ"
                    : "N/A";

                var todayRevenue = ChickenPrnContext.Ins.OrderTables
                    .Where(o => o.Date.HasValue && o.Date.Value.Date == today)
                    .Sum(o => o.Total ?? 0);
                Revenue = $"{todayRevenue / 1000000:F1}M";

                var yesterdayRevenue = ChickenPrnContext.Ins.OrderTables
                    .Where(o => o.Date.HasValue && o.Date.Value.Date == yesterday)
                    .Sum(o => o.Total ?? 0);
                RevenuePercentage = yesterdayRevenue > 0
                    ? $"{((todayRevenue - yesterdayRevenue) * 100.0 / yesterdayRevenue):F0}% so với HQ"
                    : "N/A";

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
                    .FirstOrDefault();

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
                TopSellingFood = new TopFoodItem { FoodId = 0, FoodName = "Không có dữ liệu", TypeName = "N/A", OrderCount = 0 };
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
        private void LoadRecentFeedBack()
        {
            try
            {
                RecentFeedBack = new ObservableCollection<Feedback>(
                    ChickenPrnContext.Ins.Feedbacks
                        .OrderByDescending(o => o.TimeFeedback)
                        .Take(5)
                        .ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load đơn hàng gần đây: {ex.Message}");
                RecentFeedBack = new ObservableCollection<Feedback>();
            }
        }

        private void ExecuteLogout(object parameter)
        {
            AccountLogin.Clear();
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
            Application.Current.Windows.OfType<AdminDashBoard>().FirstOrDefault()?.Close();
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

        private void ExecuteGoRoleManagement(object parameter)
        {
            RoleManagement fb = new RoleManagement();
            fb.Show();
            Application.Current.Windows.OfType<AdminDashBoard>().FirstOrDefault()?.Close();
        }

        private void ExecuteGoFeedbackManagement(object parameter)
        {
            FeedbackList fb = new FeedbackList();
            fb.Show();
            Application.Current.Windows.OfType<AdminDashBoard>().FirstOrDefault()?.Close();
        }

        private void ExecuteGoImportGoods(object parameter)
        {
            MessageBox.Show("Admin đang làm");
        }

        private void ExecuteGoCreateOrder(object parameter)
        {
            OrderedFood fb = new OrderedFood();
            fb.Show();
            Application.Current.Windows.OfType<AdminDashBoard>().FirstOrDefault()?.Close();
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