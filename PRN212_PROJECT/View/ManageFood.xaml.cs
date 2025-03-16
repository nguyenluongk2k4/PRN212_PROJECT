using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for ManageFood.xaml
    /// </summary>
    public partial class ManageFood : Window
    {
        public ManageFood()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ManageFoodVM;
            if (vm != null)
            {
                // Retrieve values from form fields
                String Id_String=lbID.Content.ToString();
                int id=int.Parse(Id_String);
                string name = txtName.Text;
                string typeName = cbxType.SelectedItem as string;
                int type=ChickenPrnContext.Ins.TypeOfFoods.FirstOrDefault(x=>x.TypeName.Equals(typeName)).TypeId;
                double price;
                bool isPriceValid = double.TryParse(txtPrice.Text.Replace(",", ""), out price);
                int status = cbStatus.IsChecked==true ? 1:0;
                string image = img.Source.ToString();

                
                Food f=new Food() { FoodId=id,FoodName=name,FoodType=type,Price=price,Status=status,Image=image};

                if (!isPriceValid)
                {
                    MessageBox.Show("Giá không hợp lệ. Vui lòng nhập số hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Pass the values to the view model
                vm.UpdateFood(f);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
