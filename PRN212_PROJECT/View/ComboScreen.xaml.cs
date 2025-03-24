using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Combo.xaml
    /// </summary>
    public partial class Combo : Window
    {
        private bool _isUpdating;

        public Combo()
        {
            InitializeComponent();
        }

        // Restrict input to digits only
        private void PriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]+$");
        }

        // Format the price as the user types
        private void PriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdating) return;

            _isUpdating = true;

            var textBox = sender as TextBox;
            if (textBox == null) return;

            var caretIndex = textBox.CaretIndex;
            var text = textBox.Text.Replace(",", ""); // Remove existing commas

            if (float.TryParse(text, out float value))
            {
                // Format the number with commas
                textBox.Text = String.Format("{0:N0}", value);
                // Adjust caret position
                textBox.CaretIndex = caretIndex + (textBox.Text.Length - text.Length);
            }
            else if (string.IsNullOrEmpty(text))
            {
                textBox.Text = "";
            }

            _isUpdating = false;
        }
    
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectFoodScreen selectFoodScreen = new SelectFoodScreen();
            selectFoodScreen.Show();
        }
    }
}
