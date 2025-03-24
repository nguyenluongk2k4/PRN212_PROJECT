using PRN212_PROJECT.Models;
using PRN212_PROJECT.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using LiveCharts;

namespace PRN212_PROJECT.View_Model
{
    public class ManageProfitVM : BaseViewModel
    {
        // Properties cho thống kê doanh thu
        private string _todayRevenue;
        private string _todayRevenuePercentage;
        private string _thisWeekRevenue;
        private string _thisWeekRevenuePercentage;

        public string TodayRevenue
        {
            get => _todayRevenue;
            set
            {
                _todayRevenue = value;
                OnPropertyChanged();
            }
        }

        public string TodayRevenuePercentage
        {
            get => _todayRevenuePercentage;
            set
            {
                _todayRevenuePercentage = value;
                OnPropertyChanged();
            }
        }

        public string ThisWeekRevenue
        {
            get => _thisWeekRevenue;
            set
            {
                _thisWeekRevenue = value;
                OnPropertyChanged();
            }
        }

        public string ThisWeekRevenuePercentage
        {
            get => _thisWeekRevenuePercentage;
            set
            {
                _thisWeekRevenuePercentage = value;
                OnPropertyChanged();
            }
        }

        // Dữ liệu cho biểu đồ
        private ChartValues<double> _revenueValues;
        private string[] _labels;
        private Func<double, string> _formatter;

        public ChartValues<double> RevenueValues
        {
            get => _revenueValues;
            set
            {
                _revenueValues = value;
                OnPropertyChanged();
            }
        }

        public string[] Labels
        {
            get => _labels;
            set
            {
                _labels = value;
                OnPropertyChanged();
            }
        }

        public Func<double, string> Formatter
        {
            get => _formatter;
            set
            {
                _formatter = value;
                OnPropertyChanged();
            }
        }

        // Command cho nút "Quay lại"
        public ICommand BackCommand { get; }

        public ManageProfitVM()
        {
            if (ChickenPrnContext.Ins == null)
            {
                MessageBox.Show("ChickenPrnContext.Ins is null");
                return;
            }

            // Tính toán thống kê doanh thu
            LoadProfitStats();

            // Tải dữ liệu cho biểu đồ
            LoadChartData();

            // Khởi tạo Command
            BackCommand = new RelayCommand(ExecuteBack);
        }

        private void LoadProfitStats()
        {
            try
            {
                // Lấy ngày hiện tại
                var today = DateTime.Today;
                var yesterday = today.AddDays(-1);

                // Doanh thu hôm nay
                var todayRevenue = ChickenPrnContext.Ins.OrderTables
                    .Where(o => o.Date.HasValue && o.Date.Value.Date == today)
                    .Sum(o => o.Total);
                TodayRevenue = $"{todayRevenue / 1000000:F1}M";

                // Doanh thu hôm qua
                var yesterdayRevenue = ChickenPrnContext.Ins.OrderTables
                    .Where(o => o.Date.HasValue && o.Date.Value.Date == yesterday)
                    .Sum(o => o.Total);
                TodayRevenuePercentage = yesterdayRevenue > 0
                    ? $"{((todayRevenue - yesterdayRevenue) * 100.0 / yesterdayRevenue):F0}% so với hôm qua"
                    : "N/A";

                // Doanh thu tuần này (7 ngày gần nhất)
                var startOfThisWeek = today.AddDays(-6); // Từ hôm nay về 6 ngày trước
                var thisWeekRevenue = ChickenPrnContext.Ins.OrderTables
                    .Where(o => o.Date.HasValue && o.Date.Value.Date >= startOfThisWeek && o.Date.Value.Date <= today)
                    .Sum(o => o.Total);
                ThisWeekRevenue = $"{thisWeekRevenue / 1000000:F1}M";

                // Doanh thu tuần trước (7 ngày trước tuần này)
                var startOfLastWeek = startOfThisWeek.AddDays(-7);
                var endOfLastWeek = startOfThisWeek.AddDays(-1);
                var lastWeekRevenue = ChickenPrnContext.Ins.OrderTables
                    .Where(o => o.Date.HasValue && o.Date.Value.Date >= startOfLastWeek && o.Date.Value.Date <= endOfLastWeek)
                    .Sum(o => o.Total);
                ThisWeekRevenuePercentage = lastWeekRevenue > 0
                    ? $"{((thisWeekRevenue - lastWeekRevenue) * 100.0 / lastWeekRevenue):F0}% so với tuần trước"
                    : "N/A";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load thống kê doanh thu: {ex.Message}");
                TodayRevenue = "0M";
                TodayRevenuePercentage = "N/A";
                ThisWeekRevenue = "0M";
                ThisWeekRevenuePercentage = "N/A";
            }
        }

        private void LoadChartData()
        {
            try
            {
                // Lấy ngày hiện tại
                var today = DateTime.Today;
                var startDate = today.AddMonths(-11); // 12 tháng gần nhất (bao gồm tháng hiện tại)

                // Tạo mảng doanh thu và nhãn cho 12 tháng
                var revenueValues = new ChartValues<double>();
                var labels = new string[12];

                for (int i = 0; i < 12; i++)
                {
                    var monthDate = startDate.AddMonths(i);
                    var monthStart = new DateTime(monthDate.Year, monthDate.Month, 1);
                    var monthEnd = monthStart.AddMonths(1).AddDays(-1); // Ngày cuối của tháng

                    var monthlyRevenue = ChickenPrnContext.Ins.OrderTables
                        .Where(o => o.Date.HasValue && o.Date.Value.Date >= monthStart && o.Date.Value.Date <= monthEnd)
                        .Sum(o => o.Total) / 1000000.0 ?? 0; // Chuyển sang đơn vị triệu (M)

                    revenueValues.Add(monthlyRevenue);
                    labels[i] = monthDate.ToString("MM/yyyy");
                }

                // Gán dữ liệu cho biểu đồ
                RevenueValues = revenueValues;
                Labels = labels;

                // Định dạng trục Y
                Formatter = value => $"{value:F1}M";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu biểu đồ: {ex.Message}");
                RevenueValues = new ChartValues<double> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                Labels = new string[] { "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A", "N/A" };
                Formatter = value => $"{value:F1}M";
            }
        }

        private void ExecuteBack(object parameter)
        {
            AdminDashBoard adminDashBoard = new AdminDashBoard();
            adminDashBoard.Show();
            Application.Current.Windows.OfType<ManageProfit>().FirstOrDefault()?.Close();
        }
    }

}
