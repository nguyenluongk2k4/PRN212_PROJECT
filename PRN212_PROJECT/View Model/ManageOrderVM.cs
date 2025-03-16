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

namespace PRN212_PROJECT.View_Model
{
    public class ManageOrderVM : BaseViewModel
    {
        private ObservableCollection<OrderDetail> _oderdetails;
        private ObservableCollection<Order> _orders;

        public ObservableCollection<Order> orders
        {
            get => _orders;
            set {
                _orders = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<OrderDetail> orderDetails
        {
            get => _oderdetails;
            set
            {
                _oderdetails = value;
                OnPropertyChanged();
            }
        }
        public ManageOrderVM() { 
        
        getAllOrder();
        }

        public void getAllOrder()
        {
           
            orders = new ObservableCollection<Order>(ChickenPrnContext.Ins.Orders.ToList()); 
        }


        public void getAllOrderDetail()
        {
            orderDetails = new ObservableCollection<OrderDetail>(ChickenPrnContext.Ins.OrderDetails.ToList());

        }


        public ICommand GoBack => new GoBackImpl(this);
            public class GoBackImpl : ICommand
        {
            private readonly ManageOrderVM _viewModel;

            public GoBackImpl(ManageOrderVM viewModel)
            {
                _viewModel = viewModel;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter)
            {
                AdminDashBoard ad = new AdminDashBoard();
                ad.Show();
                Application.Current.Windows[0].Close();
               
            }
        }


    }

}
