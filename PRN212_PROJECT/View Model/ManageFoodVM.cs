using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PRN212_PROJECT.Models;

namespace PRN212_PROJECT.View_Model
{
    public class ManageFoodVM : BaseViewModel
    {
        private ObservableCollection<Food> _foodList = new();
        public ObservableCollection<Food> FoodList
        {
            get => _foodList;
            set
            {
                _foodList = value;
                OnPropertyChanged(nameof(FoodList));
            }
        }

        private ObservableCollection<Food> _allFoods = new(); // Store the full list

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                SearchByName();
            }
        }

        private ObservableCollection<string> _typeList = new();
        public ObservableCollection<string> TypeList
        {
            get => _typeList;
            set
            {
                _typeList = value;
                OnPropertyChanged(nameof(TypeList));
            }
        }

        private Food _selectedFoodItem;
        public Food SelectedFoodItem
        {
            get => _selectedFoodItem;
            set
            {
                if (_selectedFoodItem != value)
                {
                    _selectedFoodItem = value;
                    OnPropertyChanged(nameof(SelectedFoodItem));
                    if (_selectedFoodItem != null)
                    {
                        FormFoodName = _selectedFoodItem.FoodName;
                        FormFoodTypeName = _selectedFoodItem.FoodTypeNavigation?.TypeName;
                        FormFoodPrice = (double)_selectedFoodItem.Price;
                        FormFoodStatus = _selectedFoodItem.Status == 1;
                        FormFoodImagePath = _selectedFoodItem.Image;
                    }
                }
            }
        }

        // Form properties for both Add and Update
        private string _formFoodName;
        public string FormFoodName
        {
            get => _formFoodName;
            set
            {
                if (_formFoodName != value)
                {
                    _formFoodName = value;
                    OnPropertyChanged(nameof(FormFoodName));
                }
            }
        }

        private string _formFoodTypeName;
        public string FormFoodTypeName
        {
            get => _formFoodTypeName;
            set
            {
                if (_formFoodTypeName != value)
                {
                    _formFoodTypeName = value;
                    OnPropertyChanged(nameof(FormFoodTypeName));
                }
            }
        }

        private double _formFoodPrice;
        public double FormFoodPrice
        {
            get => _formFoodPrice;
            set
            {
                if (_formFoodPrice != value)
                {
                    _formFoodPrice = value;
                    OnPropertyChanged(nameof(FormFoodPrice));
                }
            }
        }

        private bool _formFoodStatus;
        public bool FormFoodStatus
        {
            get => _formFoodStatus;
            set
            {
                if (_formFoodStatus != value)
                {
                    _formFoodStatus = value;
                    OnPropertyChanged(nameof(FormFoodStatus));
                }
            }
        }

        private string _formFoodImagePath;
        public string FormFoodImagePath
        {
            get => _formFoodImagePath;
            set
            {
                if (_formFoodImagePath != value)
                {
                    _formFoodImagePath = value;
                    OnPropertyChanged(nameof(FormFoodImagePath));
                }
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand ClearFormCommand { get; set; }

        public ManageFoodVM()
        {
            LoadFoodList();
            LoadTypeList();
            AddCommand = new RelayCommand(AddFoodExecute, CanAddFood);
            UpdateCommand = new RelayCommand(UpdateFoodExecute, CanUpdateFood);
            ClearFormCommand = new RelayCommand(ClearFormExecute, _ => true);
            ClearForm();
        }

        private void LoadFoodList()
        {
            var list = ChickenPrnContext.Ins.Foods
                .Include(x => x.FoodTypeNavigation)
                .ToList();

            string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", ".."));
            string imageFolder = Path.Combine(projectRoot, "images");

            foreach (var food in list)
            {
                if (!string.IsNullOrEmpty(food.Image))
                {
                    food.Image = Path.Combine(imageFolder, food.Image);
                }
            }

            _allFoods = new ObservableCollection<Food>(list);
            FoodList = new ObservableCollection<Food>(list);
        }

        private void LoadTypeList()
        {
            var list = ChickenPrnContext.Ins.TypeOfFoods.Select(x => x.TypeName).ToList();
            TypeList = new ObservableCollection<string>(list);
        }

        private void SearchByName()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FoodList = new ObservableCollection<Food>(_allFoods);
            }
            else
            {
                var filteredList = _allFoods
                    .Where(food => food.FoodName != null &&
                                   food.FoodName.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                FoodList = new ObservableCollection<Food>(filteredList);
            }
        }

        // Validation for special characters
        private bool IsValidFoodName(string foodName)
        {
            if (string.IsNullOrEmpty(foodName))
                return false;

            // Allow only letters, numbers, and spaces
            return Regex.IsMatch(foodName, @"^[a-zA-Z0-9\s]+$");
        }

        // Add Part
        private void AddFoodExecute(object parameter)
        {
            if (CanAddFood(parameter))
            {
                if (!IsValidFoodName(FormFoodName))
                {
                    MessageBox.Show("Food name must not contain special characters. Only letters, numbers, and spaces are allowed.");
                    return;
                }

                var newFood = new Food
                {
                    FoodName = FormFoodName,
                    FoodType = ChickenPrnContext.Ins.TypeOfFoods
                        .FirstOrDefault(t => t.TypeName == FormFoodTypeName)?.TypeId,
                    Price = FormFoodPrice,
                    Status = FormFoodStatus ? 1 : 0,
                    Image = Path.GetFileName(FormFoodImagePath)
                };

                ChickenPrnContext.Ins.Foods.Add(newFood);
                ChickenPrnContext.Ins.SaveChanges();
                MessageBox.Show("Added Successfully");
                LoadFoodList();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Please fill in all fields with valid values before adding.");
            }
        }

        private bool CanAddFood(object parameter)
        {
            return !string.IsNullOrEmpty(FormFoodName) &&
                   !string.IsNullOrEmpty(FormFoodTypeName) &&
                   FormFoodPrice > 0 &&
                   !string.IsNullOrEmpty(FormFoodImagePath);
        }

        // Update Part
        private void UpdateFoodExecute(object parameter)
        {
            if (CanUpdateFood(parameter))
            {
                if (!IsValidFoodName(FormFoodName))
                {
                    MessageBox.Show("Food name must not contain special characters. Only letters, numbers, and spaces are allowed.");
                    return;
                }

                var existingFood = ChickenPrnContext.Ins.Foods
                    .FirstOrDefault(x => x.FoodId == SelectedFoodItem.FoodId);

                if (existingFood != null)
                {
                    existingFood.FoodName = FormFoodName;
                    existingFood.FoodType = ChickenPrnContext.Ins.TypeOfFoods
                        .FirstOrDefault(t => t.TypeName == FormFoodTypeName)?.TypeId;
                    existingFood.Price = FormFoodPrice;
                    existingFood.Status = FormFoodStatus ? 1 : 0;
                    existingFood.Image = Path.GetFileName(FormFoodImagePath);

                    ChickenPrnContext.Ins.Foods.Update(existingFood);
                    ChickenPrnContext.Ins.SaveChanges();
                    MessageBox.Show("Update successfully");
                    LoadFoodList();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("No food with this id exists to update.");
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields before updating.");
            }
        }

        private bool CanUpdateFood(object parameter)
        {
            return !string.IsNullOrEmpty(FormFoodName) &&
                   ChickenPrnContext.Ins.Foods.Any(x => x.FoodName == FormFoodName) &&
                   !string.IsNullOrEmpty(FormFoodTypeName) &&
                   FormFoodPrice > 0 &&
                   !string.IsNullOrEmpty(FormFoodImagePath);
        }

        private void ClearFormExecute(object parameter)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            FormFoodName = null;
            FormFoodTypeName = null;
            FormFoodPrice = 0;
            FormFoodStatus = false;
            FormFoodImagePath = null;
            SelectedFoodItem = null;
        }
    }
}