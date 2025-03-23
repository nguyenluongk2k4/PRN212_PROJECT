using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using PRN212_PROJECT.View_Model;

namespace PRN212_PROJECT.View
{
    public partial class ManageFood : Window
    {
        public ManageFood()
        {
            InitializeComponent();
        }

        private void UpLoadImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string sourcePath = openFileDialog.FileName;
                string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", ".."));
                string imageFolder = Path.Combine(projectRoot, "images");
                string fileName = Path.GetFileNameWithoutExtension(sourcePath);
                string extension = Path.GetExtension(sourcePath);
                string uniqueFileName = $"{fileName}_{Guid.NewGuid().ToString("N")}{extension}"; // Unique filename
                string destPath = Path.Combine(imageFolder, uniqueFileName);

                try
                {
                    Directory.CreateDirectory(imageFolder);
                    File.Copy(sourcePath, destPath, false); // Don’t overwrite, use unique name
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Error uploading file: {ex.Message}");
                    return;
                }

                if (DataContext is ManageFoodVM vm)
                {
                    vm.FormFoodImagePath = destPath;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddNewFoodType addNewFoodTypeScreen=new AddNewFoodType();
            addNewFoodTypeScreen.Show();
        }
    }
}