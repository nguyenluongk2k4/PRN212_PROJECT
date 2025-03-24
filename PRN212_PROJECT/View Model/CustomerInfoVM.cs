using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_PROJECT.View_Model
{
    public class CustomerInfoVM:BaseViewModel
    {
        

        private ObservableCollection<OrderItem> _orderItems;

        public ObservableCollection<OrderItem> OrderItems
        {
            get => _orderItems;
            set
            {
                _orderItems = value;
                OnPropertyChanged();
            }
        }
        
    }
}
