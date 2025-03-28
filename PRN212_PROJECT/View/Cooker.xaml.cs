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
using PRN212_PROJECT.View_Model;

namespace PRN212_PROJECT.View
{
    /// <summary>
    /// Interaction logic for Cooker.xaml
    /// </summary>
    public partial class Cooker : Window
    {
        public Cooker()
        {
            InitializeComponent();
            DataContext = new CookerVM();
            WindowState = WindowState.Maximized;

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            AccountLogin.Clear();
            this.Close();

        }
    }
}
