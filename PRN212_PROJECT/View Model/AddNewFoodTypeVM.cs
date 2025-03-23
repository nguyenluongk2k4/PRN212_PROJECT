using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using PRN212_PROJECT.Models;

namespace PRN212_PROJECT.View_Model
{
    public class AddNewFoodTypeVM : BaseViewModel
    {
        private string _name = string.Empty;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));

            }
        }
        private ObservableCollection<TypeOfFood> _FoodTypes;
        public ObservableCollection<TypeOfFood> FoodTypes
        {
            get => _FoodTypes;
            set
            {
                _FoodTypes = value;
                OnPropertyChanged(nameof(FoodTypes));
            }
        }
        private TypeOfFood _selectedType;
        public TypeOfFood SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertyChanged(nameof(SelectedType));
            }
        }

        public ICommand AddFoodType { get; set; }
        public ICommand DeleteFoodType { get; set; }
        public ICommand UpdateFoodType { get; set; }
        public AddNewFoodTypeVM()
        {
            AddFoodType = new RelayCommand(ExecuteAddToDB, CanAddFood);
            DeleteFoodType = new RelayCommand(ExecuteDeleteFoodType, CanDeleteFood);
            UpdateFoodType = new RelayCommand(ExecuteUpdateFoodType, CanUpdateFoodType);
            GetList();
        }

        private void ExecuteAddToDB(object parameter)
        {
            TypeOfFood existType = ChickenPrnContext.Ins.TypeOfFoods
                .FirstOrDefault(x => x.TypeName == Name.Trim());
            if (existType == null)
            {
                ChickenPrnContext.Ins.TypeOfFoods.Add(new TypeOfFood { TypeName = Name.Trim() });
                ChickenPrnContext.Ins.SaveChanges();
                MessageBox.Show("Successful");
                Name = string.Empty;
                GetList();
            }
        }
        private void GetList()
        {
            var listFromDB = ChickenPrnContext.Ins.TypeOfFoods.ToList();
            FoodTypes = new ObservableCollection<TypeOfFood>(listFromDB);
        }

        private bool CanAddFood(object parameter)
        {
            return !string.IsNullOrEmpty(Name) &&
                   Name.Any(c => char.IsLetter(c)) &&
                   Name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
        }
        private bool CanDeleteFood(object parameter)
        {
            return SelectedType != null && !ChickenPrnContext.Ins.Foods.Any(x => x.FoodTypeNavigation.TypeId.Equals(SelectedType.TypeId));
        }
        private void ExecuteDeleteFoodType(object parameter)
        {
            if (CanDeleteFood(parameter))
            {
                ChickenPrnContext.Ins.TypeOfFoods.Remove(SelectedType);
                ChickenPrnContext.Ins.SaveChanges();
                MessageBox.Show($"Xóa {SelectedType.TypeName} Thành công!!");
                GetList();
            }
        }
        private bool CanUpdateFoodType(object parameter)
        {
            return SelectedType != null && ChickenPrnContext.Ins.TypeOfFoods.Any(x => x.TypeId.Equals(SelectedType.TypeId)) &&
                !String.IsNullOrEmpty(SelectedType.TypeName) &&
                SelectedType.TypeName.Any(c => Char.IsLetter(c)) &&
                SelectedType.TypeName.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c));
        }
        public void ExecuteUpdateFoodType(object parameter)
        {
            if (CanUpdateFoodType(parameter))
            {
                ChickenPrnContext.Ins.TypeOfFoods.Update(SelectedType);
                ChickenPrnContext.Ins.SaveChanges();
                GetList();
                MessageBox.Show("Đổi Tên Thành Công!!");
            }
        }
    }
}