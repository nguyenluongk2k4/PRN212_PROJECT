using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PRN212_PROJECT.Models;
using PRN212_PROJECT.View;
using System.Timers;

namespace PRN212_PROJECT.View_Model
{
    public class CustomerOrderVM : BaseViewModel
    {
        private ObservableCollection<Food> _foodItems;
        public ObservableCollection<Food> FoodItems
        {
            get => _foodItems;
            set
            {
                _foodItems = value;
                OnPropertyChanged(nameof(FoodItems));
            }
        }

        private ObservableCollection<TypeOfFood> _foodTypes;
        public ObservableCollection<TypeOfFood> FoodTypes
        {
            get => _foodTypes;
            set
            {
                _foodTypes = value;
                OnPropertyChanged(nameof(FoodTypes));
            }
        }

        private TypeOfFood _selectedType;
        public TypeOfFood SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertyChanged(nameof(SelectedType));
                if (IsShowingFoods)
                {
                    LoadFoodList();
                }
            }
        }

        private ObservableCollection<Combo> _combos;
        public ObservableCollection<Combo> Combos
        {
            get => _combos;
            set
            {
                _combos = value;
                OnPropertyChanged(nameof(Combos));
            }
        }

        private ObservableCollection<object> _displayItems;
        public ObservableCollection<object> DisplayItems
        {
            get => _displayItems;
            set
            {
                _displayItems = value;
                OnPropertyChanged(nameof(DisplayItems));
            }
        }

        private bool _isShowingFoods = true;
        public bool IsShowingFoods
        {
            get => _isShowingFoods;
            set
            {
                _isShowingFoods = value;
                OnPropertyChanged(nameof(IsShowingFoods));
                UpdateDisplayItems();
            }
        }

        private ObservableCollection<OrderDetailFood> _orderDetailFoods;
        public ObservableCollection<OrderDetailFood> OrderDetailFoods
        {
            get => _orderDetailFoods;
            set
            {
                if (_orderDetailFoods != null)
                {
                    _orderDetailFoods.CollectionChanged -= OrderDetailFoods_CollectionChanged;
                }

                _orderDetailFoods = value;

                if (_orderDetailFoods != null)
                {
                    _orderDetailFoods.CollectionChanged += OrderDetailFoods_CollectionChanged;
                }

                OnPropertyChanged(nameof(OrderDetailFoods));
                UpdateTotalPrice();
            }
        }

        private ObservableCollection<OrderDetailCombo> _orderDetailCombos;
        public ObservableCollection<OrderDetailCombo> OrderDetailCombos
        {
            get => _orderDetailCombos;
            set
            {
                if (_orderDetailCombos != null)
                {
                    _orderDetailCombos.CollectionChanged -= OrderDetailCombos_CollectionChanged;
                }

                _orderDetailCombos = value;

                if (_orderDetailCombos != null)
                {
                    _orderDetailCombos.CollectionChanged += OrderDetailCombos_CollectionChanged;
                }

                OnPropertyChanged(nameof(OrderDetailCombos));
                UpdateTotalPrice();
            }
        }

        private ObservableCollection<object> _cartItems;
        public ObservableCollection<object> CartItems
        {
            get => _cartItems;
            set
            {
                _cartItems = value;
                OnPropertyChanged(nameof(CartItems));
            }
        }

        private double _totalPrice;
        public double TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        // Properties for CheckoutScreen
        private string _customerName;
        public string CustomerName
        {
            get => _customerName;
            set
            {
                _customerName = value;
                OnPropertyChanged(nameof(CustomerName));
            }
        }

        private bool _isOrderConfirmed;
        public bool IsOrderConfirmed
        {
            get => _isOrderConfirmed;
            set
            {
                _isOrderConfirmed = value;
                OnPropertyChanged(nameof(IsOrderConfirmed));
            }
        }

        private string _qrCodeImage;
        public string QRCodeImage
        {
            get => _qrCodeImage;
            set
            {
                _qrCodeImage = value;
                OnPropertyChanged(nameof(QRCodeImage));
            }
        }

        private string _paymentStatus;
        public string PaymentStatus
        {
            get => _paymentStatus;
            set
            {
                _paymentStatus = value;
                OnPropertyChanged(nameof(PaymentStatus));
            }
        }

        private int _pendingOrderId;
        private System.Timers.Timer _paymentCheckTimer;

        public RelayCommand SelectFoodTypeCommand { get; set; }
        public RelayCommand ToggleDisplayCommand { get; set; }
        public RelayCommand AddFoodToCartCommand { get; set; }
        public RelayCommand AddComboToCartCommand { get; set; }
        public RelayCommand IncrementFoodQuantityCommand { get; set; }
        public RelayCommand DecrementFoodQuantityCommand { get; set; }
        public RelayCommand IncrementComboQuantityCommand { get; set; }
        public RelayCommand DecrementComboQuantityCommand { get; set; }
        public RelayCommand CheckoutCommand { get; set; }
        public RelayCommand ConfirmOrderCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public CustomerOrderVM()
        {
            if (ChickenPrnContext.Ins == null)
            {
                throw new InvalidOperationException("ChickenPrnContext.Ins is not initialized.");
            }

            FoodTypes = new ObservableCollection<TypeOfFood>();
            Combos = new ObservableCollection<Combo>();
            DisplayItems = new ObservableCollection<object>();
            OrderDetailFoods = new ObservableCollection<OrderDetailFood>();
            OrderDetailCombos = new ObservableCollection<OrderDetailCombo>();
            CartItems = new ObservableCollection<object>();
            PaymentStatus = "Chưa thanh toán";

            SelectFoodTypeCommand = new RelayCommand(
                parameter =>
                {
                    if (parameter is TypeOfFood type)
                    {
                        SelectedType = type;
                    }
                });

            ToggleDisplayCommand = new RelayCommand(
                _ =>
                {
                    IsShowingFoods = !IsShowingFoods;
                });

            AddFoodToCartCommand = new RelayCommand(
                parameter =>
                {
                    if (parameter is Food food)
                    {
                        AddFoodToCart(food);
                    }
                });

            AddComboToCartCommand = new RelayCommand(
                parameter =>
                {
                    if (parameter is Combo combo)
                    {
                        AddComboToCart(combo);
                    }
                });

            IncrementFoodQuantityCommand = new RelayCommand(
                parameter =>
                {
                    if (parameter is OrderDetailFood orderDetail)
                    {
                        orderDetail.Amount = (orderDetail.Amount ?? 0) + 1;
                        UpdateTotalPrice();
                        UpdateCartItems();
                    }
                });

            DecrementFoodQuantityCommand = new RelayCommand(
                parameter =>
                {
                    if (parameter is OrderDetailFood orderDetail)
                    {
                        orderDetail.Amount = (orderDetail.Amount ?? 0) - 1;
                        if (orderDetail.Amount <= 0)
                        {
                            OrderDetailFoods.Remove(orderDetail);
                        }
                        UpdateTotalPrice();
                        UpdateCartItems();
                    }
                });

            IncrementComboQuantityCommand = new RelayCommand(
                parameter =>
                {
                    if (parameter is OrderDetailCombo orderDetail)
                    {
                        orderDetail.Amount = (orderDetail.Amount ?? 0) + 1;
                        UpdateTotalPrice();
                        UpdateCartItems();
                    }
                });

            DecrementComboQuantityCommand = new RelayCommand(
                parameter =>
                {
                    if (parameter is OrderDetailCombo orderDetail)
                    {
                        orderDetail.Amount = (orderDetail.Amount ?? 0) - 1;
                        if (orderDetail.Amount <= 0)
                        {
                            OrderDetailCombos.Remove(orderDetail);
                        }
                        UpdateTotalPrice();
                        UpdateCartItems();
                    }
                });

            CheckoutCommand = new RelayCommand(
                _ =>
                {
                    if (OrderDetailFoods.Any() || OrderDetailCombos.Any())
                    {
                        // Reset checkout state
                        IsOrderConfirmed = false;
                        CustomerName = string.Empty;
                        QRCodeImage = null;
                        PaymentStatus = "Chưa thanh toán";

                        var checkoutScreen = new CheckoutScreen();
                        checkoutScreen.DataContext = this;
                        checkoutScreen.Show();
                        Application.Current.Windows.OfType<View.CustomerOrderScreen>().FirstOrDefault()?.Close();
                    }
                    else
                    {
                        MessageBox.Show("Giỏ hàng trống. Vui lòng thêm món trước khi thanh toán.");
                    }
                });

            ConfirmOrderCommand = new RelayCommand(
                async _ =>
                {
                    // Generate a temporary order ID for the QR code
                    _pendingOrderId = new Random().Next(1000, 9999); // Simulate a temporary order ID

                    // Generate QR code URL for payment
                    string qrCodeUrl = GeneratePaymentQRCode(_pendingOrderId, TotalPrice);
                    if (string.IsNullOrEmpty(qrCodeUrl))
                    {
                        MessageBox.Show("Không thể tạo mã QR. Vui lòng thử lại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    QRCodeImage = qrCodeUrl;

                    // Update UI state to show QR code
                    IsOrderConfirmed = true;
                    PaymentStatus = "Đang chờ thanh toán...";

                    // Start polling for payment confirmation
                    await StartPaymentPolling();
                },
                _ => !string.IsNullOrWhiteSpace(CustomerName)
            );

            CancelCommand = new RelayCommand(
                _ =>
                {
                    // Stop the payment polling timer if it's running
                    _paymentCheckTimer?.Stop();
                    _paymentCheckTimer?.Dispose();
                    Application.Current.Windows.OfType<CheckoutScreen>().FirstOrDefault()?.Close();
                });

            LoadFoodTypes();
            LoadCombos();
            LoadFoodList();
            UpdateDisplayItems();
        }

        private async Task StartPaymentPolling()
        {
            // Simulate payment polling (replace with actual payment gateway API call)
            _paymentCheckTimer = new System.Timers.Timer(5000); // Check every 5 seconds
            int maxAttempts = 12; // 60 seconds timeout (5s * 12)
            int attempts = 0;

            _paymentCheckTimer.Elapsed += async (sender, e) =>
            {
                attempts++;
                bool paymentConfirmed = await CheckPaymentStatus(_pendingOrderId);

                if (paymentConfirmed)
                {
                    _paymentCheckTimer.Stop();
                    _paymentCheckTimer.Dispose();

                    // Payment confirmed, now save the order to the database
                    await SaveOrderToDatabase();
                    PaymentStatus = "Thanh toán thành công!";
                    MessageBox.Show("Thanh toán thành công! Đơn hàng đã được lưu.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (attempts >= maxAttempts)
                {
                    _paymentCheckTimer.Stop();
                    _paymentCheckTimer.Dispose();
                    PaymentStatus = "Thanh toán thất bại hoặc hết thời gian.";
                    MessageBox.Show("Thanh toán thất bại hoặc hết thời gian. Vui lòng thử lại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };

            _paymentCheckTimer.Start();
        }

        private async Task<bool> CheckPaymentStatus(int orderId)
        {
            // Simulate payment confirmation (replace with actual API call to payment gateway)
            // For simulation, assume payment is confirmed after 10 seconds
            await Task.Delay(10000); // Simulate network delay
            return true; // Simulate successful payment
        }

        private async Task SaveOrderToDatabase()
        {
            await Task.Run(() =>
            {
                try
                {
                    var order = new OrderTable
                    {
                        Date = DateTime.Now,
                        Total = TotalPrice,
                        CustomerName = CustomerName,
                        Address = "In-store delivery",
                        Shipping = false,
                        IsPaid = true, // Payment confirmed
                        Done = false,
                        OrderDetailFoods = new List<OrderDetailFood>(),
                        OrderDetailCombos = new List<OrderDetailCombo>()
                    };

                    // Add the order to the database
                    ChickenPrnContext.Ins.OrderTables.Add(order);
                    ChickenPrnContext.Ins.SaveChanges();

                    // Add order details for foods
                    foreach (var item in OrderDetailFoods)
                    {
                        var orderDetail = new OrderDetailFood
                        {
                            OrderId = order.OrderId,
                            FoodId = item.FoodId,
                            Amount = item.Amount,
                            Price = item.Price
                        };
                        ChickenPrnContext.Ins.OrderDetailFoods.Add(orderDetail);
                    }

                    // Add order details for combos
                    foreach (var item in OrderDetailCombos)
                    {
                        var orderDetail = new OrderDetailCombo
                        {
                            OrderId = order.OrderId,
                            ComboId = item.ComboId,
                            Amount = item.Amount,
                            Price = item.Price
                        };
                        ChickenPrnContext.Ins.OrderDetailCombos.Add(orderDetail);
                    }

                    ChickenPrnContext.Ins.SaveChanges();
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Error saving order: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            });
        }

        private string GeneratePaymentQRCode(int orderId, double totalPrice)
        {
            try
            {
                double totalAmount = totalPrice != 0 ? totalPrice : 0;
                string qrCodeUrl = $"https://img.vietqr.io/image/MB-0936971273-compact2.jpg?amount={totalAmount}&addInfo={Uri.EscapeDataString($"Thanh toan don hang {orderId}")}";
                return qrCodeUrl;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating QR code: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private void OrderDetailFoods_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateCartItems();
        }

        private void OrderDetailCombos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateCartItems();
        }

        private void AddFoodToCart(Food food)
        {
            if (food == null)
            {
                return;
            }

            var existingItem = OrderDetailFoods.FirstOrDefault(od => od.FoodId == food.FoodId);
            if (existingItem != null)
            {
                existingItem.Amount = (existingItem.Amount ?? 0) + 1;
            }
            else
            {
                var newItem = new OrderDetailFood
                {
                    FoodId = food.FoodId,
                    Food = food,
                    Amount = 1,
                    Price = food.Price ?? 0
                };
                OrderDetailFoods.Add(newItem);
            }
            UpdateTotalPrice();
            UpdateCartItems();
        }

        private void AddComboToCart(Combo combo)
        {
            if (combo == null)
            {
                return;
            }

            var existingItem = OrderDetailCombos.FirstOrDefault(od => od.ComboId == combo.ComboId);
            if (existingItem != null)
            {
                existingItem.Amount = (existingItem.Amount ?? 0) + 1;
            }
            else
            {
                var newItem = new OrderDetailCombo
                {
                    ComboId = combo.ComboId,
                    Combo = combo,
                    Amount = 1,
                    Price = combo.Price ?? 0
                };
                OrderDetailCombos.Add(newItem);
            }
            UpdateTotalPrice();
            UpdateCartItems();
        }

        private void UpdateTotalPrice()
        {
            double total = 0;

            total += OrderDetailFoods.Sum(od => (od.Price ?? 0) * (od.Amount ?? 0));
            if (OrderDetailCombos != null)
            {
                total += OrderDetailCombos.Sum(od => (od.Price ?? 0) * (od.Amount ?? 0));
            }

            TotalPrice = total;
        }

        private void UpdateCartItems()
        {
            CartItems.Clear();
            foreach (var food in OrderDetailFoods)
            {
                CartItems.Add(food);
            }
            foreach (var combo in OrderDetailCombos)
            {
                CartItems.Add(combo);
            }
        }

        private void LoadFoodTypes()
        {
            var types = ChickenPrnContext.Ins.TypeOfFoods.ToList();
            FoodTypes = new ObservableCollection<TypeOfFood>(types);
        }

        private void LoadFoodList()
        {
            var listFood = ChickenPrnContext.Ins.Foods
                .Include(f => f.FoodTypeNavigation)
                .ToList();
            if (SelectedType != null)
            {
                listFood = listFood.Where(f => f.FoodTypeNavigation != null && f.FoodTypeNavigation.TypeId == SelectedType.TypeId).ToList();
            }
            FoodItems = new ObservableCollection<Food>(listFood);
            UpdateDisplayItems();
        }

        private void LoadCombos()
        {
            var combos = ChickenPrnContext.Ins.Combos
                .Include(c => c.ComboDetails)
                .ThenInclude(cd => cd.Food)
                .ToList();
            Combos = new ObservableCollection<Combo>(combos);
        }

        private void UpdateDisplayItems()
        {
            DisplayItems.Clear();
            if (IsShowingFoods)
            {
                foreach (var food in FoodItems)
                {
                    if (food != null)
                    {
                        DisplayItems.Add(food);
                    }
                }
            }
            else
            {
                foreach (var combo in Combos)
                {
                    if (combo != null)
                    {
                        DisplayItems.Add(combo);
                    }
                }
            }
        }
    }

    public class WidthConverterForFourItems : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double scrollViewerWidth)
            {
                double itemWidth = (scrollViewerWidth - (4 * 25)) / 4;
                return Math.Max(itemWidth, 0);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DisplayItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FoodTemplate { get; set; }
        public DataTemplate ComboTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is PRN212_PROJECT.Models.Food)
                return FoodTemplate;
            if (item is PRN212_PROJECT.Models.Combo)
                return ComboTemplate;
            return null;
        }
    }

    public class CartItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FoodTemplate { get; set; }
        public DataTemplate ComboTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is OrderDetailFood)
                return FoodTemplate;
            if (item is OrderDetailCombo)
                return ComboTemplate;
            return null;
        }
    }

    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isVisible)
            {
                bool invert = parameter as string == "False";
                return (isVisible != invert) ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                bool invert = parameter as string == "False";
                return (visibility == Visibility.Visible) != invert;
            }
            return false;
        }
    }
    public class PaymentStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                if (status.Contains("Thành công"))
                    return "Green";
                if (status.Contains("Thất bại"))
                    return "Red";
                return "Orange"; // For "Đang chờ thanh toán..."
            }
            return "Black";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}