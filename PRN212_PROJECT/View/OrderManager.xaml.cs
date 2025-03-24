using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PRN212_PROJECT.View
{
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
<<<<<<<< HEAD:PRN212_PROJECT/View/OrderFood.xaml.cs
    public partial class OrderFood : Window
    {
        public OrderFood()
========
    public partial class OrderManager : Window
    {
        public OrderManager()
>>>>>>>> thanh:PRN212_PROJECT/View/OrderManager.xaml.cs
        {
            InitializeComponent();
            DataContext = new PRN212_PROJECT.View_Model.FoodOrder();
        }
    }
}
