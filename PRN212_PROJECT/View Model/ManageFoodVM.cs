using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;
using Microsoft.EntityFrameworkCore;
using PRN212_PROJECT.Models;

namespace PRN212_PROJECT.View_Model
{
    public class ManageFoodVM : BaseViewModel
    {
        private ObservableCollection<object> _foodList;
        public ObservableCollection<object> FoodList
        {
            get => _foodList;
            set
            {
                _foodList = value;
                OnPropertyChanged(nameof(FoodList));
            }
        }
        private ObservableCollection<string> _typeList;
        public ObservableCollection<string> TypeList
        {
            get => _typeList;
            set
            {
                _typeList = value;
                OnPropertyChanged(nameof(TypeList));
            }
        }
        private object _selectedFood;
        public object SelectedFood
        {
            get => _selectedFood;
            set
            {
                _selectedFood = value;
                OnPropertyChanged(nameof(SelectedFood));
            }
        }

        public ManageFoodVM()
        {
            LoadFoodList();
            LoadTypeList();
        }

        private void LoadFoodList()
        {
            string imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

            var list = ChickenPrnContext.Ins.Foods
                .Include(x => x.FoodTypeNavigation)
                .Select(x => new
                {
                    ID = x.FoodId,
                    Name = x.FoodName,
                    Type = x.FoodTypeNavigation.TypeName,
                    Price = x.Price,
                    Status = x.Status == 1 ? true : false,
                    Image = string.IsNullOrEmpty(x.Image) ? null : Path.Combine(imageFolder, x.Image)
                })
                .ToList();

            FoodList = new ObservableCollection<object>(list);
        }

        private void LoadTypeList()
        {
            var list = ChickenPrnContext.Ins.TypeOfFoods.Select(x => x.TypeName).ToList();
            TypeList = new ObservableCollection<string>(list);
        }
        public void UpdateFood(Food food)
        {
            var selectedFood = ChickenPrnContext.Ins.Foods.FirstOrDefault(x => x.FoodId == food.FoodId);
            if (selectedFood != null)
            {

                ChickenPrnContext.Ins.Foods.Update(food);
                ChickenPrnContext.Ins.SaveChanges();
                LoadFoodList();
            }
        }



        public class StatusToBoolConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value is string Status && Status == "Active";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value is bool isChecked && isChecked ? "Active" : "Inactive";
            }
        }
    }
}
