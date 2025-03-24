using PRN212_PROJECT.Models;
using PRN212_PROJECT.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace PRN212_PROJECT.View_Model
{
    public class ManageOrderVM : BaseViewModel
    {
        private ObservableCollection<OrderTable> _odertable;

        private ObservableCollection<OrderDetailFood> _oderfood;

        private ObservableCollection<OrderDetailCombo> _odercombo;

        private ObservableCollection<OrderTable> _filteredOrderTable;

        private OrderTable _selectedOrder;

        private DateTime? _filterDate;

        private string _searchText;

        private string _selectedPaymentStatus;

        public ICommand FilterCommand { get; }
        public ICommand ClearFilterCommand { get; }

        public ICommand GoBack { get; }


        public ObservableCollection<OrderTable> FilteredOrderTable
        {
            get => _filteredOrderTable;
            set
            {
                _filteredOrderTable = value;
                OnPropertyChanged();
            }
        }
        public DateTime? FilterDate
        {
            get => _filterDate;
            set
            {
                _filterDate = value;
                OnPropertyChanged();
                
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public string SelectedPaymentStatus
        {
            get => _selectedPaymentStatus;
            set
            {
                _selectedPaymentStatus = value;
                OnPropertyChanged();
            }
        }


        public OrderTable selectedOrder
        {
            get => _selectedOrder;
            set 
            { 

                _selectedOrder = value;
                
                OnPropertyChanged();
                LoadOrderDetails();
            }
        }

        public ObservableCollection<OrderTable> orderTable
        {
            get => _odertable;
            set
            {
                _odertable = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<OrderDetailFood> orderFood
        {
            get => _oderfood;
            set
            {
                _oderfood = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<OrderDetailCombo> orderCombo
        {
            get => _odercombo;
            set
            {
                _odercombo = value;
                OnPropertyChanged();
                
            }
        }

        
        private void LoadOrderDetails()
        {
            try
            {
                
                if (selectedOrder != null)
                {
                    
                    getFoodFromId(); 
                    getComboFromId(); 
                }
                else
                {
                    
                    orderFood.Clear(); 
                    orderCombo.Clear(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in LoadOrderDetails: {ex.Message}");
            }
        }

        private void ExecuteFilter(object parameter)
        {
            var filtered = orderTable.AsEnumerable();
            
            // Lọc theo ngày
            if (FilterDate.HasValue)
            {
                // Lấy phần ngày của FilterDate (bỏ qua giờ, phút, giây)
                DateTime filterDateOnly = new DateTime(FilterDate.Value.Year, FilterDate.Value.Month, FilterDate.Value.Day);
                // So sánh với o.Date (kiểm tra null trước)
                filtered = filtered.Where(o => o.Date.HasValue &&
                                              new DateTime(o.Date.Value.Year, o.Date.Value.Month, o.Date.Value.Day) == filterDateOnly);
            }

            // Lọc theo tìm kiếm (OrderId hoặc CustomerName)
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                string searchLower = SearchText.ToLower();
                filtered = filtered.Where(o =>
                    o.OrderId.ToString().Contains(searchLower) ||
                    (o.CustomerName?.ToLower().Contains(searchLower) ?? false));
            }

            //// Lọc theo trạng thái thanh toán
            //if (SelectedPaymentStatus != "Tất cả")
            //{
            //    MessageBox.Show(SelectedPaymentStatus);
            //    bool isPaid = SelectedPaymentStatus.Equals("System.Windows.Controls.ComboBoxltem: Đã thanh toán");
            //    MessageBox.Show($"dcu {isPaid}");
            //    filtered = filtered.Where(o => o.IsPaid == isPaid);
            //}

            FilteredOrderTable = new ObservableCollection<OrderTable>(filtered);
            if (FilteredOrderTable.Any())
            {
                selectedOrder = FilteredOrderTable.First();
            }
            else
            {
                selectedOrder = null;
            }
        }

        private void ExecuteClearFilter(object parameter)
        {
            FilterDate = null;
            SearchText = string.Empty;
            SelectedPaymentStatus = "Tất cả";
            FilteredOrderTable = new ObservableCollection<OrderTable>(orderTable);
            if (FilteredOrderTable.Any())
            {
                selectedOrder = FilteredOrderTable.First();
            }
        }

        private void ExecuteGoBack(object parameter) {
            AdminDashBoard ad = new AdminDashBoard();
            ad.Show();
            Application.Current.Windows[0].Close();


        }

        public ManageOrderVM() {

            getAllOrderDetail();
            orderFood = new ObservableCollection<OrderDetailFood>();
            orderCombo = new ObservableCollection<OrderDetailCombo>();
            FilteredOrderTable = new ObservableCollection<OrderTable>(orderTable);

            SelectedPaymentStatus = "Tất cả"; // Mặc định

            if (orderTable.Any())
            {
                selectedOrder = orderTable.First();
            }
            else
            {
                MessageBox.Show("No orders found in OrderTable");
            }

            FilterCommand = new RelayCommand(ExecuteFilter);
            ClearFilterCommand = new RelayCommand(ExecuteClearFilter);
            //GoBack = new GoBackImpl(this);
            GoBack = new RelayCommand(ExecuteGoBack);
        }

        public void getFoodFromId()
        {
            
            try
            {
                orderFood = new ObservableCollection<OrderDetailFood>(
                    ChickenPrnContext.Ins.OrderDetailFoods
                        .Include(x => x.Food) 
                        .Where(x => x.OrderId == _selectedOrder.OrderId)
                        .ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load Food: {ex.Message}");
                orderFood = new ObservableCollection<OrderDetailFood>();
            }
        }
        
        public void getComboFromId()
        {
            try
            {
                orderCombo = new ObservableCollection<OrderDetailCombo>(
                    ChickenPrnContext.Ins.OrderDetailCombos
                        .Include(x => x.Combo) 
                        .Where(x => x.OrderId == _selectedOrder.OrderId)
                        .ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load Combo: {ex.Message}");
                orderCombo = new ObservableCollection<OrderDetailCombo>();
            }
        }

        public void getAllOrderDetail()
        {
            orderTable = new ObservableCollection<OrderTable>(ChickenPrnContext.Ins.OrderTables.ToList());

        }

        
           


    }

}
