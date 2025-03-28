using LiveCharts;
using LiveCharts.Wpf;
using PRN212_PROJECT.Models;
using PRN212_PROJECT.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PRN212_PROJECT.View_Model
{
    public class PeriodOption
    {
        public string DisplayName { get; set; }
        public string Value { get; set; }
    }

    public class TimeOption
    {
        public string DisplayName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class ManageProfitVM : BaseViewModel
    {
        #region Summary Statistics Properties
        private string _todayRevenue;
        private string _todayRevenuePercentage;
        private string _thisWeekRevenue;
        private string _thisWeekRevenuePercentage;
        private string _incomeOutcomeRatio;
        private string _monthlyGrowthRate;
        private string _monthlyProfitMargin;

        public string TodayRevenue { get => _todayRevenue; set { _todayRevenue = value; OnPropertyChanged(); } }
        public string TodayRevenuePercentage { get => _todayRevenuePercentage; set { _todayRevenuePercentage = value; OnPropertyChanged(); } }
        public string ThisWeekRevenue { get => _thisWeekRevenue; set { _thisWeekRevenue = value; OnPropertyChanged(); } }
        public string ThisWeekRevenuePercentage { get => _thisWeekRevenuePercentage; set { _thisWeekRevenuePercentage = value; OnPropertyChanged(); } }
        public string IncomeOutcomeRatio { get => _incomeOutcomeRatio; set { _incomeOutcomeRatio = value; OnPropertyChanged(); } }
        public string MonthlyGrowthRate { get => _monthlyGrowthRate; set { _monthlyGrowthRate = value; OnPropertyChanged(); } }
        public string MonthlyProfitMargin { get => _monthlyProfitMargin; set { _monthlyProfitMargin = value; OnPropertyChanged(); } }
        #endregion

        #region Filter Properties
        private ObservableCollection<PeriodOption> _periodOptions;
        private PeriodOption _selectedPeriod;
        private ObservableCollection<TimeOption> _timeOptions;
        private TimeOption _selectedTimeOption;

        public ObservableCollection<PeriodOption> PeriodOptions { get => _periodOptions; set { _periodOptions = value; OnPropertyChanged(); } }
        public PeriodOption SelectedPeriod
        {
            get => _selectedPeriod;
            set { _selectedPeriod = value; OnPropertyChanged(); UpdateTimeOptions(); }
        }
        public ObservableCollection<TimeOption> TimeOptions { get => _timeOptions; set { _timeOptions = value; OnPropertyChanged(); } }
        public TimeOption SelectedTimeOption
        {
            get => _selectedTimeOption;
            set { _selectedTimeOption = value; OnPropertyChanged(); LoadChartData(); }
        }
        #endregion

        #region Chart Visibility Properties
        private bool _isGrossProfitVisible;
        private bool _isRevenueVisible;
        private bool _isOrderCountVisible;
        private bool _isExpenditureVisible;
        private bool _isFoodComboOrdersVisible;

        public bool IsGrossProfitVisible { get => _isGrossProfitVisible; set { UpdateChartVisibility(nameof(IsGrossProfitVisible), value); OnPropertyChanged(); } }
        public bool IsRevenueVisible { get => _isRevenueVisible; set { UpdateChartVisibility(nameof(IsRevenueVisible), value); OnPropertyChanged(); } }
        public bool IsOrderCountVisible { get => _isOrderCountVisible; set { UpdateChartVisibility(nameof(IsOrderCountVisible), value); OnPropertyChanged(); } }
        public bool IsExpenditureVisible { get => _isExpenditureVisible; set { UpdateChartVisibility(nameof(IsExpenditureVisible), value); OnPropertyChanged(); } }
        public bool IsFoodComboOrdersVisible { get => _isFoodComboOrdersVisible; set { UpdateChartVisibility(nameof(IsFoodComboOrdersVisible), value); OnPropertyChanged(); } }
        #endregion

        #region Chart Data Properties
        private ChartValues<double> _grossProfitValues;
        private ChartValues<double> _revenueValues;
        private ChartValues<double> _orderCountValues;
        private ChartValues<double> _expenditureValues;
        private ChartValues<double> _foodComboOrderValues;
        private string[] _labels;
        private string[] _foodComboLabels;
        private Func<double, string> _formatter;
        private Func<double, string> _orderCountFormatter;

        public ChartValues<double> GrossProfitValues { get => _grossProfitValues ??= new ChartValues<double>(); set { _grossProfitValues = value; OnPropertyChanged(); } }
        public ChartValues<double> RevenueValues { get => _revenueValues ??= new ChartValues<double>(); set { _revenueValues = value; OnPropertyChanged(); } }
        public ChartValues<double> OrderCountValues { get => _orderCountValues ??= new ChartValues<double>(); set { _orderCountValues = value; OnPropertyChanged(); } }
        public ChartValues<double> ExpenditureValues { get => _expenditureValues ??= new ChartValues<double>(); set { _expenditureValues = value; OnPropertyChanged(); } }
        public ChartValues<double> FoodComboOrderValues { get => _foodComboOrderValues ??= new ChartValues<double>(); set { _foodComboOrderValues = value; OnPropertyChanged(); } }
        public string[] Labels { get => _labels; set { _labels = value; OnPropertyChanged(); } }
        public string[] FoodComboLabels { get => _foodComboLabels; set { _foodComboLabels = value; OnPropertyChanged(); } }
        public Func<double, string> Formatter { get => _formatter; set { _formatter = value; OnPropertyChanged(); } }
        public Func<double, string> OrderCountFormatter { get => _orderCountFormatter; set { _orderCountFormatter = value; OnPropertyChanged(); } }
        #endregion

        #region Commands
        public ICommand BackCommand { get; }
        public ICommand ToggleGrossProfitCommand { get; }
        public ICommand ToggleRevenueCommand { get; }
        public ICommand ToggleOrderCountCommand { get; }
        public ICommand ToggleExpenditureCommand { get; }
        public ICommand ToggleFoodComboOrdersCommand { get; }
        #endregion

        #region Constructor
        public ManageProfitVM()
        {
            if (ChickenPrnContext.Ins == null)
            {
                MessageBox.Show("ChickenPrnContext.Ins is null");
                return;
            }

            PeriodOptions = new ObservableCollection<PeriodOption>
            {
                new PeriodOption { DisplayName = "Theo ngày", Value = "Day" },
                new PeriodOption { DisplayName = "Theo tháng", Value = "Month" },
                new PeriodOption { DisplayName = "Theo năm", Value = "Year" }
            };
            SelectedPeriod = PeriodOptions.First();

            Formatter = value => $"{value:F1}M";
            OrderCountFormatter = value => $"{value:N0}";

            BackCommand = new RelayCommand(ExecuteBack);
            ToggleGrossProfitCommand = new RelayCommand(ExecuteToggleGrossProfit);
            ToggleRevenueCommand = new RelayCommand(ExecuteToggleRevenue);
            ToggleOrderCountCommand = new RelayCommand(ExecuteToggleOrderCount);
            ToggleExpenditureCommand = new RelayCommand(ExecuteToggleExpenditure);
            ToggleFoodComboOrdersCommand = new RelayCommand(ExecuteToggleFoodComboOrders);

            IsGrossProfitVisible = true;
            IsRevenueVisible = false;
            IsOrderCountVisible = false;
            IsExpenditureVisible = false;
            IsFoodComboOrdersVisible = false;

            LoadProfitStats();
            UpdateTimeOptions();
            LoadFoodComboOrderData();
        }
        #endregion

        #region Command Handlers
        private void ExecuteBack(object parameter)
        {
            new AdminDashBoard().Show();
            Application.Current.Windows.OfType<ManageProfit>().FirstOrDefault()?.Close();
        }

        private void ExecuteToggleGrossProfit(object parameter) => IsGrossProfitVisible = true;
        private void ExecuteToggleRevenue(object parameter) => IsRevenueVisible = true;
        private void ExecuteToggleOrderCount(object parameter) => IsOrderCountVisible = true;
        private void ExecuteToggleExpenditure(object parameter) => IsExpenditureVisible = true;
        private void ExecuteToggleFoodComboOrders(object parameter) => IsFoodComboOrdersVisible = true;
        #endregion

        #region Helper Methods
        private void UpdateChartVisibility(string propertyName, bool value)
        {
            if (!value) return;

            _isGrossProfitVisible = false;
            _isRevenueVisible = false;
            _isOrderCountVisible = false;
            _isExpenditureVisible = false;
            _isFoodComboOrdersVisible = false;

            switch (propertyName)
            {
                case nameof(IsGrossProfitVisible): _isGrossProfitVisible = true; break;
                case nameof(IsRevenueVisible): _isRevenueVisible = true; break;
                case nameof(IsOrderCountVisible): _isOrderCountVisible = true; break;
                case nameof(IsExpenditureVisible): _isExpenditureVisible = true; break;
                case nameof(IsFoodComboOrdersVisible): _isFoodComboOrdersVisible = true; break;
            }

            OnPropertyChanged(nameof(IsGrossProfitVisible));
            OnPropertyChanged(nameof(IsRevenueVisible));
            OnPropertyChanged(nameof(IsOrderCountVisible));
            OnPropertyChanged(nameof(IsExpenditureVisible));
            OnPropertyChanged(nameof(IsFoodComboOrdersVisible));
        }

        private bool IsValidDateOnly(DateOnly date)
        {
            try
            {
                var dateTime = date.ToDateTime(TimeOnly.MinValue);
                return dateTime >= DateTime.MinValue && dateTime <= DateTime.MaxValue;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Data Loading Methods
        private void LoadProfitStats()
        {
            try
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var yesterday = today.AddDays(-1);

                var todayRevenue = ChickenPrnContext.Ins.OrderTables
                    .Where(o => o.Date.HasValue && o.Date.Value == today.ToDateTime(TimeOnly.MinValue))
                    .Sum(o => o.Total) ?? 0;
                TodayRevenue = $"{todayRevenue / 1000000:F1}M";
                var yesterdayRevenue = ChickenPrnContext.Ins.OrderTables
                    .Where(o => o.Date.HasValue && o.Date.Value == yesterday.ToDateTime(TimeOnly.MinValue))
                    .Sum(o => o.Total) ?? 0;
                TodayRevenuePercentage = yesterdayRevenue > 0
                    ? $"{((todayRevenue - yesterdayRevenue) * 100.0 / yesterdayRevenue):F0}% so với hôm qua"
                    : "N/A";

                var startOfThisWeek = today.AddDays(-6);
                var thisWeekRevenue = ChickenPrnContext.Ins.OrderTables
                    .Where(o => o.Date.HasValue && o.Date.Value >= startOfThisWeek.ToDateTime(TimeOnly.MinValue) && o.Date.Value <= today.ToDateTime(TimeOnly.MinValue))
                    .Sum(o => o.Total) ?? 0;
                ThisWeekRevenue = $"{thisWeekRevenue / 1000000:F1}M";
                var startOfLastWeek = startOfThisWeek.AddDays(-7);
                var endOfLastWeek = startOfThisWeek.AddDays(-1);
                var lastWeekRevenue = ChickenPrnContext.Ins.OrderTables
                    .Where(o => o.Date.HasValue && o.Date.Value >= startOfLastWeek.ToDateTime(TimeOnly.MinValue) && o.Date.Value <= endOfLastWeek.ToDateTime(TimeOnly.MinValue))
                    .Sum(o => o.Total) ?? 0;
                ThisWeekRevenuePercentage = lastWeekRevenue > 0
                    ? $"{((thisWeekRevenue - lastWeekRevenue) * 100.0 / lastWeekRevenue):F0}% so với tuần trước"
                    : "N/A";

                var startOfThisMonth = new DateOnly(today.Year, today.Month, 1);
                var thisMonthRevenue = ChickenPrnContext.Ins.OrderTables
                    .Where(o => o.Date.HasValue && o.Date.Value >= startOfThisMonth.ToDateTime(TimeOnly.MinValue) && o.Date.Value <= today.ToDateTime(TimeOnly.MinValue))
                    .Sum(o => o.Total) ?? 0;
                var thisMonthExpenditure = ChickenPrnContext.Ins.Expenditures
                    .Where(e => e.Date.HasValue && e.Date.Value >= startOfThisMonth && e.Date.Value <= today)
                    .Sum(e => e.Cost) ?? 0;
                IncomeOutcomeRatio = thisMonthExpenditure > 0
                    ? $"{(thisMonthRevenue / (double)thisMonthExpenditure):F2} : 1"
                    : "N/A";

                var thisMonthRevenueForGrowth = thisMonthRevenue;
                var startOfLastMonth = startOfThisMonth.AddMonths(-1);
                var endOfLastMonth = startOfLastMonth.AddMonths(1).AddDays(-1);
                var lastMonthRevenue = ChickenPrnContext.Ins.OrderTables
                    .Where(o => o.Date.HasValue && o.Date.Value >= startOfLastMonth.ToDateTime(TimeOnly.MinValue) && o.Date.Value <= endOfLastMonth.ToDateTime(TimeOnly.MinValue))
                    .Sum(o => o.Total) ?? 0;
                MonthlyGrowthRate = lastMonthRevenue > 0
                    ? $"{((thisMonthRevenueForGrowth - lastMonthRevenue) * 100.0 / lastMonthRevenue):F1}%"
                    : "N/A";

                var thisMonthProfit = thisMonthRevenue - (double)thisMonthExpenditure;
                MonthlyProfitMargin = thisMonthRevenue > 0
                    ? $"{(thisMonthProfit * 100.0 / thisMonthRevenue):F1}%"
                    : "N/A";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load thống kê: {ex.Message}");
                TodayRevenue = "0M";
                TodayRevenuePercentage = "N/A";
                ThisWeekRevenue = "0M";
                ThisWeekRevenuePercentage = "N/A";
                IncomeOutcomeRatio = "N/A";
                MonthlyGrowthRate = "N/A";
                MonthlyProfitMargin = "N/A";
            }
        }

        private void UpdateTimeOptions()
        {
            TimeOptions = new ObservableCollection<TimeOption>();
            var now = DateTime.Now;

            switch (SelectedPeriod?.Value)
            {
                case "Day":
                    for (int i = 0; i < 30; i++)
                    {
                        var date = now.AddDays(-i);
                        TimeOptions.Add(new TimeOption
                        {
                            DisplayName = date.ToString("dd/MM/yyyy"),
                            StartDate = date.Date,
                            EndDate = date.Date.AddDays(1).AddTicks(-1)
                        });
                    }
                    break;

                case "Month":
                    for (int i = 0; i < 12; i++)
                    {
                        var date = now.AddMonths(-i);
                        var start = new DateTime(date.Year, date.Month, 1);
                        TimeOptions.Add(new TimeOption
                        {
                            DisplayName = date.ToString("MM/yyyy"),
                            StartDate = start,
                            EndDate = start.AddMonths(1).AddTicks(-1)
                        });
                    }
                    break;

                case "Year":
                    for (int i = 0; i < 5; i++)
                    {
                        var year = now.AddYears(-i);
                        TimeOptions.Add(new TimeOption
                        {
                            DisplayName = year.ToString("yyyy"),
                            StartDate = new DateTime(year.Year, 1, 1),
                            EndDate = new DateTime(year.Year, 12, 31, 23, 59, 59)
                        });
                    }
                    break;
            }

            SelectedTimeOption = TimeOptions.FirstOrDefault();
        }

        private void LoadChartData()
        {
            if (SelectedTimeOption == null)
            {
                GrossProfitValues.Clear();
                RevenueValues.Clear();
                OrderCountValues.Clear();
                ExpenditureValues.Clear();
                Labels = new[] { "N/A" };
                GrossProfitValues.Add(0);
                RevenueValues.Add(0);
                OrderCountValues.Add(0);
                ExpenditureValues.Add(0);
                return;
            }

            try
            {
                var startDate = SelectedTimeOption.StartDate;
                var endDate = SelectedTimeOption.EndDate;

                GrossProfitValues.Clear();
                RevenueValues.Clear();
                OrderCountValues.Clear();
                ExpenditureValues.Clear();
                Labels = Array.Empty<string>();

                var expenditures = ChickenPrnContext.Ins.Expenditures
                    .Where(e => e.Date.HasValue)
                    .AsEnumerable()
                    .Select(e => new { e.Cost, Date = IsValidDateOnly(e.Date.Value) ? e.Date.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null })
                    .Where(e => e.Date.HasValue)
                    .ToList();

                switch (SelectedPeriod?.Value)
                {
                    case "Day":
                        Labels = Enumerable.Range(0, 24).Select(h => $"{startDate:dd/MM/yyyy} {h}h").ToArray();
                        for (int h = 0; h < 24; h++)
                        {
                            var hourStart = startDate.AddHours(h);
                            var hourEnd = hourStart.AddHours(1).AddTicks(-1);

                            var revenue = ChickenPrnContext.Ins.OrderTables
                                .Where(o => o.Date.HasValue && o.Date >= hourStart && o.Date <= hourEnd)
                                .Sum(o => o.Total) ?? 0;
                            var expenditure = expenditures
                                .Where(e => e.Date >= hourStart && e.Date <= hourEnd)
                                .Sum(e => e.Cost) ?? 0;
                            var orderCount = ChickenPrnContext.Ins.OrderTables
                                .Count(o => o.Date.HasValue && o.Date >= hourStart && o.Date <= hourEnd);

                            RevenueValues.Add(revenue / 1000000.0);
                            ExpenditureValues.Add((double)expenditure / 1000000.0);
                            GrossProfitValues.Add((revenue - (double)expenditure) / 1000000.0);
                            OrderCountValues.Add(orderCount);
                        }
                        break;

                    case "Month":
                        var daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
                        Labels = Enumerable.Range(1, daysInMonth).Select(d => startDate.AddDays(d - 1).ToString("dd/MM/yyyy")).ToArray();
                        for (int d = 1; d <= daysInMonth; d++)
                        {
                            var dayStart = startDate.AddDays(d - 1);
                            var dayEnd = dayStart.AddDays(1).AddTicks(-1);

                            var revenue = ChickenPrnContext.Ins.OrderTables
                                .Where(o => o.Date.HasValue && o.Date >= dayStart && o.Date <= dayEnd)
                                .Sum(o => o.Total) ?? 0;
                            var expenditure = expenditures
                                .Where(e => e.Date >= dayStart && e.Date <= dayEnd)
                                .Sum(e => e.Cost) ?? 0;
                            var orderCount = ChickenPrnContext.Ins.OrderTables
                                .Count(o => o.Date.HasValue && o.Date >= dayStart && o.Date <= dayEnd);

                            RevenueValues.Add(revenue / 1000000.0);
                            ExpenditureValues.Add((double)expenditure / 1000000.0);
                            GrossProfitValues.Add((revenue - (double)expenditure) / 1000000.0);
                            OrderCountValues.Add(orderCount);
                        }
                        break;

                    case "Year":
                        Labels = Enumerable.Range(1, 12).Select(m => new DateTime(startDate.Year, m, 1).ToString("MM/yyyy")).ToArray();
                        for (int m = 1; m <= 12; m++)
                        {
                            var monthStart = new DateTime(startDate.Year, m, 1);
                            var monthEnd = monthStart.AddMonths(1).AddTicks(-1);

                            var revenue = ChickenPrnContext.Ins.OrderTables
                                .Where(o => o.Date.HasValue && o.Date >= monthStart && o.Date <= monthEnd)
                                .Sum(o => o.Total) ?? 0;
                            var expenditure = expenditures
                                .Where(e => e.Date >= monthStart && e.Date <= monthEnd)
                                .Sum(e => e.Cost) ?? 0;
                            var orderCount = ChickenPrnContext.Ins.OrderTables
                                .Count(o => o.Date.HasValue && o.Date >= monthStart && o.Date <= monthEnd);

                            RevenueValues.Add(revenue / 1000000.0);
                            ExpenditureValues.Add((double)expenditure / 1000000.0);
                            GrossProfitValues.Add((revenue - (double)expenditure) / 1000000.0);
                            OrderCountValues.Add(orderCount);
                        }
                        break;
                }

                if (GrossProfitValues.Count == 0) GrossProfitValues.Add(0);
                if (RevenueValues.Count == 0) RevenueValues.Add(0);
                if (OrderCountValues.Count == 0) OrderCountValues.Add(0);
                if (ExpenditureValues.Count == 0) ExpenditureValues.Add(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu biểu đồ: {ex.Message}");
                GrossProfitValues.Clear();
                RevenueValues.Clear();
                OrderCountValues.Clear();
                ExpenditureValues.Clear();
                Labels = new[] { "N/A" };
                GrossProfitValues.Add(0);
                RevenueValues.Add(0);
                OrderCountValues.Add(0);
                ExpenditureValues.Add(0);
            }
        }

        private void LoadFoodComboOrderData()
        {
            try
            {
                const int TOP_N = 20; // Show top 20 items

                var foodOrders = ChickenPrnContext.Ins.OrderDetailFoods
                    .GroupBy(odf => odf.FoodId)
                    .Select(g => new
                    {
                        Name = ChickenPrnContext.Ins.Foods
                            .Where(f => f.FoodId == g.Key)
                            .Select(f => f.FoodName)
                            .FirstOrDefault() ?? $"Food {g.Key}",
                        Count = g.Sum(odf => odf.Amount ?? 0)
                    })
                    .ToList();

                var comboOrders = ChickenPrnContext.Ins.OrderDetailCombos
                    .GroupBy(odc => odc.ComboId)
                    .Select(g => new
                    {
                        Name = ChickenPrnContext.Ins.Combos
                            .Where(c => c.ComboId == g.Key)
                            .Select(c => c.ComboName)
                            .FirstOrDefault() ?? $"Combo {g.Key}",
                        Count = g.Sum(odc => odc.Amount ?? 0)
                    })
                    .ToList();

                var allOrders = foodOrders.Concat(comboOrders)
                    .OrderByDescending(x => x.Count) // Sort by count descending
                    .ToList();

                // Take top N and aggregate the rest into "Others"
                var topOrders = allOrders.Take(TOP_N).ToList();
                var othersCount = allOrders.Skip(TOP_N).Sum(x => x.Count);

                FoodComboOrderValues.Clear();
                FoodComboLabels = topOrders.Select(x => x.Name).ToArray();

                if (othersCount > 0)
                {
                    topOrders.Add(new { Name = "Others", Count = othersCount });
                    FoodComboLabels = topOrders.Select(x => x.Name).ToArray();
                }

                FoodComboOrderValues.AddRange(topOrders.Select(x => (double)x.Count));

                if (FoodComboOrderValues.Count == 0)
                {
                    FoodComboOrderValues.Add(0);
                    FoodComboLabels = new[] { "No Data" };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu đơn hàng thức ăn/combo: {ex.Message}");
                FoodComboOrderValues.Clear();
                FoodComboOrderValues.Add(0);
                FoodComboLabels = new[] { "Error" };
            }
        }
        #endregion
    }
}