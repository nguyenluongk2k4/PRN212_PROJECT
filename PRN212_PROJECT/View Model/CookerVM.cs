using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PRN212_PROJECT.Models;
using System.Windows.Threading;
using System.Windows.Data;
using System.Globalization;
using Microsoft.EntityFrameworkCore; // Add this for DispatcherTimer

namespace PRN212_PROJECT.View_Model
{
    public class CookerVM : BaseViewModel
    {
        private ObservableCollection<OrderTable> _commingOrder;
        public ObservableCollection<OrderTable> CommingOrder
        {
            get => _commingOrder;
            set
            {
                _commingOrder = value;
                OnPropertyChanged(nameof(CommingOrder));
            }
        }

        private ObservableCollection<OrderTable> _inProgressOrders;
        public ObservableCollection<OrderTable> InProgressOrders
        {
            get => _inProgressOrders;
            set
            {
                _inProgressOrders = value;
                OnPropertyChanged(nameof(InProgressOrders));
            }
        }

        private ObservableCollection<OrderDetailCombo> _commingDetailCombo;
        public ObservableCollection<OrderDetailCombo> CommingDetailCombo
        {
            get => _commingDetailCombo;
            set
            {
                _commingDetailCombo = value;
                OnPropertyChanged(nameof(CommingDetailCombo));
            }
        }

        private ObservableCollection<OrderDetailFood> _commingDetailFood;
        public ObservableCollection<OrderDetailFood> CommingDetailFood
        {
            get => _commingDetailFood;
            set
            {
                _commingDetailFood = value;
                OnPropertyChanged(nameof(CommingDetailFood));
            }
        }

        private OrderTable _selectedPendingOrder;
        public OrderTable SelectedPendingOrder
        {
            get => _selectedPendingOrder;
            set
            {
                _selectedPendingOrder = value;
                OnPropertyChanged(nameof(SelectedPendingOrder));
            }
        }

        private OrderTable _selectedInProgressOrder;
        public OrderTable SelectedInProgressOrder
        {
            get => _selectedInProgressOrder;
            set
            {
                _selectedInProgressOrder = value;
                OnPropertyChanged(nameof(SelectedInProgressOrder));
            }
        }

        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
                LoadIncomingOrder(); // Reload orders based on the selected status
            }
        }

        // To persist InProgressOrders across refreshes
        private List<int> _inProgressOrderIds;

        public RelayCommand StartPreparingCommand { get; set; }
        public RelayCommand MarkAsDoneCommand { get; set; }
        public RelayCommand RefreshCommand { get; set; }

        private DispatcherTimer _refreshTimer; // Use DispatcherTimer instead of System.Timers.Timer

        public CookerVM()
        {
            CommingOrder = new ObservableCollection<OrderTable>();
            InProgressOrders = new ObservableCollection<OrderTable>();
            CommingDetailCombo = new ObservableCollection<OrderDetailCombo>();
            CommingDetailFood = new ObservableCollection<OrderDetailFood>();
            _inProgressOrderIds = new List<int>(); // Initialize the list to track in-progress order IDs
            SelectedStatus = "Tất cả"; // Default status

            StartPreparingCommand = new RelayCommand(
                _ =>
                {
                    if (SelectedPendingOrder != null)
                    {
                        StartPreparingOrder(SelectedPendingOrder);
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng chọn một đơn hàng để bắt đầu chuẩn bị!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });

            MarkAsDoneCommand = new RelayCommand(
                _ =>
                {
                    if (SelectedInProgressOrder != null)
                    {
                        MarkOrderAsDone(SelectedInProgressOrder);
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng chọn một đơn hàng để đánh dấu là hoàn thành!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });

            RefreshCommand = new RelayCommand(
                _ =>
                {
                    LoadIncomingOrder();
                });

            LoadIncomingOrder();

            // Start a DispatcherTimer to refresh every 30 seconds
            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(30)
            };
            _refreshTimer.Tick += (sender, e) => LoadIncomingOrder();
            _refreshTimer.Start();
        }

        private void LoadIncomingOrder()
        {
            try
            {
                var orders = ChickenPrnContext.Ins.OrderTables
                    .Include(o => o.OrderDetailFoods)
                        .ThenInclude(odf => odf.Food)
                    .Include(o => o.OrderDetailCombos)
                        .ThenInclude(odc => odc.Combo)
                    .Where(x => x.Done == false)
                    .OrderBy(o => o.Date) // Sort by Date
                    .ToList();

                // Filter based on SelectedStatus
                if (SelectedStatus == "Đang chờ")
                {
                    orders = orders.Where(o => !_inProgressOrderIds.Contains(o.OrderId)).ToList();
                }
                else if (SelectedStatus == "Đang làm")
                {
                    orders = orders.Where(o => _inProgressOrderIds.Contains(o.OrderId)).ToList();
                }
                // "Tất cả" will show all orders with Done == false

                // Update collections on the UI thread
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CommingOrder.Clear();
                    foreach (var order in orders)
                    {
                        CommingOrder.Add(order);
                    }

                    // Update InProgressOrders based on _inProgressOrderIds
                    InProgressOrders.Clear();
                    foreach (var order in orders.Where(o => _inProgressOrderIds.Contains(o.OrderId)))
                    {
                        InProgressOrders.Add(order);
                    }
                });
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Error loading incoming orders: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }

        private void StartPreparingOrder(OrderTable order)
        {
            if (order == null) return;

            // Move the order from CommingOrder to InProgressOrders
            CommingOrder.Remove(order);
            if (!InProgressOrders.Any(o => o.OrderId == order.OrderId))
            {
                InProgressOrders.Add(order);
                _inProgressOrderIds.Add(order.OrderId); // Track the order ID
            }

            // Clear the selection
            SelectedPendingOrder = null;

            // Update the UI based on the selected status
            LoadIncomingOrder();
        }

        private void MarkOrderAsDone(OrderTable order)
        {
            if (order == null) return;

            try
            {
                // Update the order in the database
                var orderInDb = ChickenPrnContext.Ins.OrderTables.FirstOrDefault(o => o.OrderId == order.OrderId);
                if (orderInDb != null)
                {
                    orderInDb.Done = true;
                    ChickenPrnContext.Ins.SaveChanges();
                }

                // Remove the order from both collections
                CommingOrder.Remove(order);
                InProgressOrders.Remove(order);
                _inProgressOrderIds.Remove(order.OrderId); // Remove from tracking list

                // Clear the selection
                SelectedInProgressOrder = null;

                // Update the UI based on the selected status
                LoadIncomingOrder();

                MessageBox.Show($"Đơn hàng {order.OrderId} đã được đánh dấu là hoàn thành!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error marking order as done: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

   

    public class TimeElapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime orderDate)
            {
                var elapsed = DateTime.Now - orderDate;
                return $"{orderDate:HH:mm} ({(int)elapsed.TotalMinutes}')";
            }
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}