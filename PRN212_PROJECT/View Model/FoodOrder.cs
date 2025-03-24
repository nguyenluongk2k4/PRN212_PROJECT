using PRN212_PROJECT.Models;
using PRN212_PROJECT.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PRN212_PROJECT.View_Model
{
    public class OrderItem : BaseViewModel
    {
        private Food _food;
        private int _quantity;

        public Food Food
        {
            get => _food;
            set
            {
                _food = value;
                OnPropertyChanged();
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public double TotalPrice => Food.Price * Quantity ?? 0.0; 

        public OrderItem(Food food, int quantity = 1)
        {
            Food = food;
            Quantity = quantity;
        }
    }

    public class FoodOrder: BaseViewModel
    {
        private string _foodType4;
        private ObservableCollection<Food> _foodListType4;

        private string _foodType1;
        private ObservableCollection<Food> _foodListType1;

        private string _foodType3;
        private ObservableCollection<Food> _foodListType3;

        private string _foodType6;
        private ObservableCollection<Food> _foodListType6;

        public ObservableCollection<Food> FoodListType1
        {
            get => _foodListType1;
            set
            {
                _foodListType1 = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Food> FoodListType3
        {
            get => _foodListType3;
            set
            {
                _foodListType3 = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Food> FoodListType4
        {
            get => _foodListType4;
            set
            {
                _foodListType4 = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Food> FoodListType6
        {
            get => _foodListType6;
            set {
                _foodListType6 = value;
                OnPropertyChanged();
            }
        }
        public string FoodType1
        {
            get => _foodType1;
            set {
                _foodType1 = value;
                OnPropertyChanged();
            }
        }public string FoodType3
        {
            get => _foodType3;
            set {
                _foodType3 = value;
                OnPropertyChanged();
            }
        }public string FoodType4
        {
            get => _foodType4;
            set {
                _foodType4 = value;
                OnPropertyChanged();
            }
        }public string FoodType6
        {
            get => _foodType6;
            set {
                _foodType6 = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddFoodCommand { get; }
        public ICommand RemoveFoodCommand { get; }

        public ICommand GoCash { get; }

        private static ObservableCollection<OrderItem> _orderItems = new ObservableCollection<OrderItem>();
        public static ObservableCollection<OrderItem> OrderItems
        {
            get => _orderItems;
            set => _orderItems = value;
        }
        private double _subtotal;
        private double _tax;
        private double _total;
        private bool _useCustomValues;
        private double _customSubtotal;
        private double _customTax;
        private double _customTotal;
        public double CustomSubtotal
        {
            get => _customSubtotal;
            set
            {
                _customSubtotal = value;
                _useCustomValues = true;
                loadQr();
            }
        }

        public double CustomTax
        {
            get => _customTax;
            set
            {
                _customTax = value;
                _useCustomValues = true;
                loadQr();
            }
        }

        public double CustomTotal
        {
            get => _customTotal;
            set
            {
                _customTotal = value;
                _useCustomValues = true;
                loadQr();
            }
        }

        public double Subtotal
        {
            get => _useCustomValues ? _customSubtotal : _subtotal;
            private set
            {
                _subtotal = value;
                OnPropertyChanged();
            }
        }

        public double Tax
        {
            get => _useCustomValues ? _customTax : _tax;
            private set
            {
                _tax = value;
                OnPropertyChanged();
            }
        }

        public double Total
        {
            get => _useCustomValues ? _customTotal : _total;
            private set
            {
                _total = value;
                OnPropertyChanged();
            }
        }

        public FoodOrder()
        {
            loadFoodType4();
            loadNameType4();

            loadFoodType1();
            loadNameType1();
            
            loadFoodType3();
            loadNameType3();
            
            loadFoodType6();
            loadNameType6();
            UpdateOrderCalculations();

            AddFoodCommand = new RelayCommand(ExecuteAddFood);
            RemoveFoodCommand = new RelayCommand(ExecuteRemoveFood);
            GoCash = new RelayCommand(ExecuteGoCash);

            CancelOrder = new RelayCommand(ResetOrderValues);

            AddOrder = new RelayCommand(AddOrdered);
            loadQr();
        }
        private void ExecuteGoCash(object parameter)
        {
            CustomerInfoForm customerInfoForm = new CustomerInfoForm();
            customerInfoForm.Show();
            Application.Current.Windows.OfType<OrderedFood>().FirstOrDefault()?.Close();
        }

        private void ExecuteAddFood(object parameter)
        {
            if (parameter is Food food)
            {
                var existingItem = OrderItems.FirstOrDefault(item => item.Food.FoodId == food.FoodId);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    OrderItems.Add(new OrderItem(food));
                }

                UpdateOrderCalculations();
            }
        }

        private void ExecuteRemoveFood(object parameter)
        {
            if (parameter is OrderItem item)
            {
                OrderItems.Remove(item);
                UpdateOrderCalculations() ;
            }
        }

        public void loadFoodType4()
        {
            _foodListType4 = new ObservableCollection<Food>(ChickenPrnContext.Ins.Foods.Where(x=> x.FoodType == 4));
        }

        public void loadNameType4()
        {
            var type4 = ChickenPrnContext.Ins.TypeOfFoods.FirstOrDefault(t => t.TypeId == 4);
            _foodType4 = type4 != null ? type4.TypeName : "Mèn mén";
            
        }public void loadFoodType1()
        {
            _foodListType1 = new ObservableCollection<Food>(ChickenPrnContext.Ins.Foods.Where(x=> x.FoodType == 1));
        }

        public void loadNameType1()
        {
            var type4 = ChickenPrnContext.Ins.TypeOfFoods.FirstOrDefault(t => t.TypeId == 1);
            _foodType1 = type4 != null ? type4.TypeName : "Mèn mén";
            
        }public void loadFoodType3()
        {
            _foodListType3 = new ObservableCollection<Food>(ChickenPrnContext.Ins.Foods.Where(x=> x.FoodType == 3));
        }

        public void loadNameType3()
        {
            var type4 = ChickenPrnContext.Ins.TypeOfFoods.FirstOrDefault(t => t.TypeId == 3);
            _foodType3 = type4 != null ? type4.TypeName : "Mèn mén";
            
        }public void loadFoodType6()
        {
            _foodListType6 = new ObservableCollection<Food>(ChickenPrnContext.Ins.Foods.Where(x=> x.FoodType == 6));
        }

        public void loadNameType6()
        {
            var type4 = ChickenPrnContext.Ins.TypeOfFoods.FirstOrDefault(t => t.TypeId == 6);
            _foodType6 = type4 != null ? type4.TypeName : "Mèn mén";
            
        }

        
        
        
        
        
        
        
        // customer Info
        private string _qrCodeImage;
        public ICommand CancelOrder { get; }

        public ICommand AddOrder { get; }
        private string _customerName;
        private string _customerAddress;
        private string _selectedOrderStatus;
        private void UpdateCanSave()
        {
            OnPropertyChanged(nameof(CanSave));
        }

        public string CustomerNameInput
        {
            get => _customerName;
            set
            {
                _customerName = value;
                OnPropertyChanged();
                UpdateCanSave();
                loadQr();
            }
        }

        public string CustomerAddressInput
        {
            get => _customerAddress;
            set
            {
                _customerAddress = value;
                OnPropertyChanged();
                UpdateCanSave();
                loadQr();
            }
        }

        public ObservableCollection<string> OrderStatusOptions { get; } = new ObservableCollection<string>
        {
            "Ăn tại quầy",
            "Đem về"
        };

        public string SelectedOrderStatus
        {
            get => _selectedOrderStatus;
            set
            {
                _selectedOrderStatus = value;
                OnPropertyChanged();
                UpdateCanSave();
                loadQr();
            }
        }

        public bool CanSave => !string.IsNullOrWhiteSpace(CustomerNameInput) &&
                              !string.IsNullOrWhiteSpace(CustomerAddressInput) &&
                              !string.IsNullOrWhiteSpace(SelectedOrderStatus);

        public string QrCodeImage
        {
            get => _qrCodeImage;
            set
            {
                _qrCodeImage = value;
                OnPropertyChanged();
            }
        }

        private void UpdateOrderCalculations()
        {
            if (!_useCustomValues)
            {
                _subtotal = OrderItems.Sum(item => item.TotalPrice);
                _tax = _subtotal * 0.1;
                _total = _subtotal + _tax;
            }
            else
            {
                _subtotal = _customSubtotal;
                _tax = _customTax;
                _total = _customTotal;
            }

            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(Tax));
            OnPropertyChanged(nameof(Total));
            loadQr();
        }

        public void ResetOrderValues(object para)
        {
            Reset();
        }

        public void Reset()
        {
            _useCustomValues = true;
            _customSubtotal = 0;
            _customTax = 0;
            _customTotal = 0;
            OrderItems.Clear();
            UpdateOrderCalculations();
            Thread.Sleep(1000);
            OrderedFood od = new OrderedFood();
            od.Show();
            Application.Current.Windows.OfType<CustomerInfoForm>().FirstOrDefault()?.Close();
        }

        private void AddOrdered(object parameter)
        {
 
            var order = new OrderTable
            {
                
                CustomerName = CustomerNameInput,
                Address = CustomerAddressInput,
                Date = DateTime.Now,
                IsPaid = true,
                Shipping = true,
                Done = true,
                Total = _total,
            };
            ChickenPrnContext.Ins.OrderTables.Add(order);
            ChickenPrnContext.Ins.SaveChanges(); // Lưu để lấy OrderId

            // 3. Lấy OrderId và chèn các bản ghi vào bảng OrderDetailFood
            int orderId = order.OrderId; // OrderId được cập nhật tự động sau SaveChanges()
            foreach (var item in OrderItems)
            {
                var orderDetail = new OrderDetailFood
                {
                    OrderId = orderId,
                    FoodId = item.Food.FoodId,
                    Amount = item.Quantity,
                    Price = item.Food.Price
                };
                ChickenPrnContext.Ins.OrderDetailFoods.Add(orderDetail);
            }

            // 4. Lưu tất cả các bản ghi OrderDetail vào cơ sở dữ liệu
            ChickenPrnContext.Ins.SaveChanges();

            // 5. Hiển thị thông báo thành công và reset đơn hàng
            MessageBox.Show("Đã lưu thông tin khách hàng và hóa đơn thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            Reset();
            // Tự động mở OrderFood và đóng CustomerInfoForm
            OrderedFood od = new OrderedFood();
            od.Show();
            Application.Current.Windows.OfType<CustomerInfoForm>().FirstOrDefault()?.Close();

        }


        public void loadQr()
        {
            double totalAmount = Total != 0 ? Total : 0;
            _qrCodeImage = "https://img.vietqr.io/image/MB-0936971273-compact2.jpg?amount=" + totalAmount + "&addInfo=thanh toan hoa don";
        }


    }
}
