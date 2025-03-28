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
using PRN212_PROJECT.Models;
using PRN212_PROJECT.View_Model;

namespace PRN212_PROJECT.View
{
    /// <summary>
    /// Interaction logic for ImportIngredient.xaml
    /// </summary>
    public partial class ImportIngredient : Window
    {
        public ImportIngredient()
        {
            InitializeComponent();
        }

        

        private void BtnExcelImport_Click(object sender, RoutedEventArgs e)
        {
            InputIngredientExcel excelWindow = new InputIngredientExcel();
            excelWindow.Show();
            this.Close();
        }
        private void DgSupplierOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is ImportIngredientVM vm)
            {
                vm.SelectedSupplierOrder = dgSupplierOrders.SelectedItem as SupplierOrder;
            }
        }
    }
}

