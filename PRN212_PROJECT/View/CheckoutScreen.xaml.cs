using System.Collections.ObjectModel;
using System.Windows;
using PRN212_PROJECT.Models;
using PRN212_PROJECT.View_Model;

namespace PRN212_PROJECT.View
{
    public partial class CheckoutScreen : Window
    {
        private readonly CheckoutVM _viewModel;

        public CheckoutScreen(ObservableCollection<OrderDetailFood> orderDetailFoods, ObservableCollection<OrderDetailCombo> orderDetailCombos, double totalPrice)
        {
            InitializeComponent();
            _viewModel = new CheckoutVM(orderDetailFoods, orderDetailCombos, totalPrice);
            DataContext = _viewModel;
        }
    }
}