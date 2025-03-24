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

        public RelayCommand SelectFoodTypeCommand { get; set; }
        public RelayCommand ToggleDisplayCommand { get; set; }
        public RelayCommand AddFoodToCartCommand { get; set; }
        public RelayCommand AddComboToCartCommand { get; set; }
        public RelayCommand IncrementFoodQuantityCommand { get; set; }
        public RelayCommand DecrementFoodQuantityCommand { get; set; }
        public RelayCommand IncrementComboQuantityCommand { get; set; }
        public RelayCommand DecrementComboQuantityCommand { get; set; }
        public RelayCommand CheckoutCommand { get; set; }

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
                        UpdateCartItems(); // Manually refresh CartItems
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
                        UpdateCartItems(); // Manually refresh CartItems
                    }
                });

            IncrementComboQuantityCommand = new RelayCommand(
                parameter =>
                {
                    if (parameter is OrderDetailCombo orderDetail)
                    {
                        orderDetail.Amount = (orderDetail.Amount ?? 0) + 1;
                        UpdateTotalPrice();
                        UpdateCartItems(); // Manually refresh CartItems
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
                        UpdateCartItems(); // Manually refresh CartItems
                    }
                });

            CheckoutCommand = new RelayCommand(
                _ =>
                {
                    if (OrderDetailFoods.Any() || OrderDetailCombos.Any())
                    {
                        var checkoutScreen = new PRN212_PROJECT.View.CheckoutScreen(OrderDetailFoods, OrderDetailCombos, TotalPrice);
                        checkoutScreen.Show();
                        Application.Current.Windows.OfType<View.CustomerOrderScreen>().FirstOrDefault()?.Close();
                    }
                    else
                    {
                        MessageBox.Show("Giỏ hàng trống. Vui lòng thêm món trước khi thanh toán.");
                    }
                });

            LoadFoodTypes();
            LoadCombos();
            LoadFoodList();
            UpdateDisplayItems();
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
            UpdateCartItems(); // Manually refresh CartItems
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
            UpdateCartItems(); // Manually refresh CartItems
        }

        private void UpdateTotalPrice()
        {
            double total = 0;

            // Calculate total for OrderDetailFoods based on Price and Amount
            total += OrderDetailFoods.Sum(od => (od.Price ?? 0) * (od.Amount ?? 0));

            // Calculate total for OrderDetailCombos based on Price and Amount
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

    public class BooleanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isTrue && parameter is string options)
            {
                var parts = options.Split('|');
                if (parts.Length == 2)
                {
                    return isTrue ? parts[0] : parts[1];
                }
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isVisible)
            {
                return isVisible ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }
    }
}