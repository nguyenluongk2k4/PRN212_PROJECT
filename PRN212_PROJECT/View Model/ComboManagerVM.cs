using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Xaml.Behaviors.Media;
using PRN212_PROJECT.Models;

namespace PRN212_PROJECT.View_Model
{
    public class ComboManagerVM : BaseViewModel
    {
        // Declare variables
        private ObservableCollection<ComboDetail> _listComboFood;
        public ObservableCollection<ComboDetail> ListComboFood
        {
            get => _listComboFood;
            set
            {
                _listComboFood = value;
                OnPropertyChanged(nameof(ListComboFood));
            }
        }

        private ObservableCollection<Combo> _combos;
        public ObservableCollection<Combo> Combo
        {
            get => _combos;
            set
            {
                _combos = value;
                OnPropertyChanged(nameof(Combo));
            }
        }

        private Combo _selectedCombo;
        public Combo SelectedCombo
        {
            get => _selectedCombo;
            set
            {
                _selectedCombo = value;
                OnPropertyChanged(nameof(SelectedCombo));
                if (_selectedCombo != null)
                {
                    LoadComboDetailsWithFood(_selectedCombo.ComboId);
                    EditComboName = _selectedCombo.ComboName;
                    EditComboPrice = String.Format("{0:N0}", _selectedCombo.Price); // Use String.Format to avoid overload issue
                    LockButtonText = _selectedCombo.Status == 1 ? "Khóa Combo" : "Mở Khóa Combo";
                    LockButtonToolTip = _selectedCombo.Status == 1 ? "Khóa combo đã chọn" : "Mở khóa combo đã chọn";
                }
                else
                {
                    ListComboFood = new ObservableCollection<ComboDetail>();
                    EditComboName = string.Empty;
                    EditComboPrice = string.Empty;
                    LockButtonText = "Khóa Combo";
                    LockButtonToolTip = "Khóa combo đã chọn";
                }
            }
        }

        private ComboDetail _selectedComboDetail;
        public ComboDetail SelectedComboDetail
        {
            get => _selectedComboDetail;
            set
            {
                _selectedComboDetail = value;
                OnPropertyChanged(nameof(SelectedComboDetail));
            }
        }

        private string _newComboName;
        public string NewComboName
        {
            get => _newComboName;
            set
            {
                _newComboName = value;
                OnPropertyChanged(nameof(NewComboName));
            }
        }

        private string _newComboPrice;
        public string NewComboPrice
        {
            get => _newComboPrice;
            set
            {
                _newComboPrice = value;
                OnPropertyChanged(nameof(NewComboPrice));
            }
        }

        private string _editComboName;
        public string EditComboName
        {
            get => _editComboName;
            set
            {
                _editComboName = value;
                OnPropertyChanged(nameof(EditComboName));
            }
        }

        private string _editComboPrice;
        public string EditComboPrice
        {
            get => _editComboPrice;
            set
            {
                _editComboPrice = value;
                OnPropertyChanged(nameof(EditComboPrice));
            }
        }

        private string _lockButtonText;
        public string LockButtonText
        {
            get => _lockButtonText;
            set
            {
                _lockButtonText = value;
                OnPropertyChanged(nameof(LockButtonText));
            }
        }

        private string _lockButtonToolTip;
        public string LockButtonToolTip
        {
            get => _lockButtonToolTip;
            set
            {
                _lockButtonToolTip = value;
                OnPropertyChanged(nameof(LockButtonToolTip));
            }
        }

        // Select food screen
        private ObservableCollection<Food> _foodList;
        public ObservableCollection<Food> AllFoods
        {
            get => _foodList;
            set
            {
                _foodList = value;
                OnPropertyChanged(nameof(AllFoods));
            }
        }

        private Food _selectedFood;
        public Food SelectedFood
        {
            get => _selectedFood;
            set
            {
                _selectedFood = value;
                OnPropertyChanged(nameof(SelectedFood));
            }
        }

        public List<string> FoodType { get; set; }

        private string _searchFoodName;
        public string SearchFoodName
        {
            get => _searchFoodName;
            set
            {
                _searchFoodName = value;
                OnPropertyChanged(nameof(SearchFoodName));
                SearchFoodByName();
            }
        }

        // Command buttons
        public ICommand AddNewCombo { get; set; }
        public ICommand DeleteCombo { get; set; }
        public ICommand UpdateCombo { get; set; }
        public ICommand DecreaseAmountCommand { get; set; }
        public ICommand ChangeComboNameCommand { get; set; }
        public ICommand UpdateComboPriceCommand { get; set; }
        public ICommand LockComboCommand { get; set; }

        // Constructor
        public ComboManagerVM()
        {
            LoadCombo();
            GetAllFood();
            AddNewCombo = new RelayCommand(ExecuteAddCombo, CanAddCombo);
            GetFoodType();
            UpdateCombo = new RelayCommand(ExecuteAddNewFoodToCombo, CanUpdateCombo);
            DecreaseAmountCommand = new RelayCommand(ExecuteDecreaseAmount, CanDecreaseAmount);
            ChangeComboNameCommand = new RelayCommand(ExecuteChangeComboName, CanChangeComboName);
            DeleteCombo = new RelayCommand(ExecuteDeleteCombo, CanDeleteCombo);
            UpdateComboPriceCommand = new RelayCommand(ExecuteUpdateComboPrice, CanUpdateComboPrice);
            LockComboCommand = new RelayCommand(ExecuteLockCombo, CanLockCombo);
            LockButtonText = "Khóa Combo"; // Default value
            LockButtonToolTip = "Khóa combo đã chọn";
        }

        private void GetFoodType()
        {
            FoodType = ChickenPrnContext.Ins.TypeOfFoods.Select(x => x.TypeName).ToList();
        }

        private void GetAllFood()
        {
            var foods = ChickenPrnContext.Ins.Foods.ToList();
            AllFoods = new ObservableCollection<Food>(foods);
        }

        private void LoadCombo()
        {
            var listCombo = ChickenPrnContext.Ins.Combos
                .Include(x => x.ComboDetails)
                .ThenInclude(cd => cd.Food)
                .ToList();

            if (listCombo != null && listCombo.Any())
            {
                Combo = new ObservableCollection<Combo>(listCombo);
            }
            else
            {
                Combo = new ObservableCollection<Combo>();
            }
        }

        private void LoadComboDetailsWithFood(int comboId)
        {
            var comboDetails = ChickenPrnContext.Ins.ComboDetails
                .Include(cd => cd.Food)
                .Where(cd => cd.ComboId == comboId)
                .ToList();

            if (comboDetails != null && comboDetails.Any())
            {
                ListComboFood = new ObservableCollection<ComboDetail>(comboDetails);
            }
            else
            {
                ListComboFood = new ObservableCollection<ComboDetail>();
            }
        }

        // Add New Combo
        private bool CanAddCombo(object parameter)
        {
            if (string.IsNullOrWhiteSpace(NewComboName) || !Regex.IsMatch(NewComboName, @"^[\p{L}0-9\s]+$"))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(NewComboPrice))
            {
                return false;
            }

            string priceText = NewComboPrice.Replace(",", "");
            return float.TryParse(priceText, out float price) && price >= 0;
        }

        public void ExecuteAddCombo(object parameter)
        {
            bool existedCombo = ChickenPrnContext.Ins.Combos.Any(x => x.ComboName.Trim().ToLower() == NewComboName.Trim().ToLower());
            if (existedCombo)
            {
                MessageBox.Show("Tên combo đã tồn tại");
            }
            else
            {
                string priceText = NewComboPrice.Replace(",", "");
                if (!float.TryParse(priceText, out float price))
                {
                    MessageBox.Show("Vui lòng nhập giá hợp lệ!");
                }
                else
                {
                    Combo newCB = new Combo { ComboName = NewComboName.Trim(), Status = 1, Price = price };
                    ChickenPrnContext.Ins.Combos.Add(newCB);
                    ChickenPrnContext.Ins.SaveChanges();
                    LoadCombo();
                    NewComboName = string.Empty;
                    NewComboPrice = string.Empty;
                    MessageBox.Show("Thêm thành công");
                }
            }
        }

        // Delete Combo
        private bool CanDeleteCombo(object parameter)
        {
            return SelectedCombo != null;
        }

        private void ExecuteDeleteCombo(object parameter)
        {
            if (SelectedCombo == null)
            {
                MessageBox.Show("Vui lòng chọn một combo để xóa!");
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa combo '{SelectedCombo.ComboName}'?", "Xác nhận xóa", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var comboDetails = ChickenPrnContext.Ins.ComboDetails.Where(cd => cd.ComboId == SelectedCombo.ComboId).ToList();
                    ChickenPrnContext.Ins.ComboDetails.RemoveRange(comboDetails);
                    ChickenPrnContext.Ins.Combos.Remove(SelectedCombo);
                    ChickenPrnContext.Ins.SaveChanges();
                    LoadCombo();
                    SelectedCombo = null;
                    MessageBox.Show("Xóa combo thành công!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa combo: {ex.Message}");
                }
            }
        }

        // Toggle Combo Status (Lock/Unlock)
        private bool CanLockCombo(object parameter)
        {
            return SelectedCombo != null;
        }

        private void ExecuteLockCombo(object parameter)
        {
            if (SelectedCombo == null)
            {
                MessageBox.Show("Vui lòng chọn một combo để thay đổi trạng thái!");
                return;
            }

            try
            {
                // Toggle the status
                SelectedCombo.Status = SelectedCombo.Status == 1 ? 0 : 1;
                ChickenPrnContext.Ins.Combos.Update(SelectedCombo);
                ChickenPrnContext.Ins.SaveChanges();
                LoadCombo();
                var currentComboId = SelectedCombo.ComboId;
                SelectedCombo = Combo.FirstOrDefault(c => c.ComboId == currentComboId);
                MessageBox.Show(SelectedCombo.Status == 1 ? "Mở khóa combo thành công!" : "Khóa combo thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thay đổi trạng thái combo: {ex.Message}");
            }
        }

        // Search Food by Name
        private void SearchFoodByName()
        {
            if (!string.IsNullOrWhiteSpace(SearchFoodName))
            {
                var result = ChickenPrnContext.Ins.Foods
                    .Where(x => x.FoodName.ToLower().Contains(SearchFoodName.ToLower().Trim()))
                    .ToList();
                AllFoods = new ObservableCollection<Food>(result);
            }
            else
            {
                GetAllFood();
            }
        }

        // Add New Food to Combo
        private bool CanUpdateCombo(object parameter)
        {
            return SelectedFood != null && SelectedCombo != null;
        }

        private void ExecuteAddNewFoodToCombo(object parameter)
        {
            if (SelectedCombo == null || SelectedFood == null)
            {
                MessageBox.Show("Vui lòng chọn một combo và một món ăn!");
                return;
            }

            List<ComboDetail> selectedComboDetails = SelectedCombo.ComboDetails.ToList();
            var existedFoodInCombo = selectedComboDetails.FirstOrDefault(x => x.FoodId == SelectedFood.FoodId);

            try
            {
                if (existedFoodInCombo == null)
                {
                    ComboDetail newComboDetail = new ComboDetail { ComboId = SelectedCombo.ComboId, FoodId = SelectedFood.FoodId, Amount = 1 };
                    ChickenPrnContext.Ins.ComboDetails.Add(newComboDetail);
                    ChickenPrnContext.Ins.SaveChanges();
                    MessageBox.Show("Thêm vào combo thành công");
                }
                else
                {
                    existedFoodInCombo.Amount += 1;
                    ChickenPrnContext.Ins.ComboDetails.Update(existedFoodInCombo);
                    ChickenPrnContext.Ins.SaveChanges();
                    MessageBox.Show("Tăng số lượng thành công");
                }

                LoadCombo();
                var currentComboId = SelectedCombo.ComboId;
                SelectedCombo = Combo.FirstOrDefault(c => c.ComboId == currentComboId);
                SelectedFood = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm món vào combo: {ex.Message}");
            }
        }

        // Decrease Amount
        private bool CanDecreaseAmount(object parameter)
        {
            return SelectedCombo != null && SelectedComboDetail != null;
        }

        private void ExecuteDecreaseAmount(object parameter)
        {
            if (SelectedCombo == null || SelectedComboDetail == null)
            {
                MessageBox.Show("Vui lòng chọn một combo và một thành phần!");
                return;
            }

            try
            {
                SelectedComboDetail.Amount -= 1;

                if (SelectedComboDetail.Amount <= 0)
                {
                    ChickenPrnContext.Ins.ComboDetails.Remove(SelectedComboDetail);
                    MessageBox.Show("Đã xóa thành phần khỏi combo!");
                }
                else
                {
                    ChickenPrnContext.Ins.ComboDetails.Update(SelectedComboDetail);
                    MessageBox.Show("Giảm số lượng thành công!");
                }

                ChickenPrnContext.Ins.SaveChanges();
                LoadCombo();
                var currentComboId = SelectedCombo.ComboId;
                SelectedCombo = Combo.FirstOrDefault(c => c.ComboId == currentComboId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi giảm số lượng: {ex.Message}");
            }
        }

        // Change Combo Name
        private bool CanChangeComboName(object parameter)
        {
            if (SelectedCombo == null || string.IsNullOrWhiteSpace(EditComboName))
            {
                return false;
            }

            if (!Regex.IsMatch(EditComboName, @"^[\p{L}0-9\s]+$"))
            {
                return false;
            }

            bool exists = ChickenPrnContext.Ins.Combos.Any(x => x.ComboName.Trim().ToLower() == EditComboName.Trim().ToLower() && x.ComboId != SelectedCombo.ComboId);
            return !exists;
        }

        private void ExecuteChangeComboName(object parameter)
        {
            if (SelectedCombo == null)
            {
                MessageBox.Show("Vui lòng chọn một combo để sửa tên!");
                return;
            }

            try
            {
                SelectedCombo.ComboName = EditComboName.Trim();
                ChickenPrnContext.Ins.Combos.Update(SelectedCombo);
                ChickenPrnContext.Ins.SaveChanges();
                LoadCombo();
                var currentComboId = SelectedCombo.ComboId;
                SelectedCombo = Combo.FirstOrDefault(c => c.ComboId == currentComboId);
                MessageBox.Show("Đổi tên combo thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đổi tên combo: {ex.Message}");
            }
        }

        // Update Combo Price
        private bool CanUpdateComboPrice(object parameter)
        {
            if (SelectedCombo == null || string.IsNullOrWhiteSpace(EditComboPrice))
            {
                return false;
            }

            string priceText = EditComboPrice.Replace(",", "");
            return float.TryParse(priceText, out float price) && price >= 0;
        }

        private void ExecuteUpdateComboPrice(object parameter)
        {
            if (SelectedCombo == null)
            {
                MessageBox.Show("Vui lòng chọn một combo để cập nhật giá!");
                return;
            }

            string priceText = EditComboPrice.Replace(",", "");
            if (!float.TryParse(priceText, out float price))
            {
                MessageBox.Show("Vui lòng nhập giá hợp lệ!");
                return;
            }

            try
            {
                SelectedCombo.Price = price;
                ChickenPrnContext.Ins.Combos.Update(SelectedCombo);
                ChickenPrnContext.Ins.SaveChanges();
                LoadCombo();
                var currentComboId = SelectedCombo.ComboId;
                SelectedCombo = Combo.FirstOrDefault(c => c.ComboId == currentComboId);
                MessageBox.Show("Cập nhật giá thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật giá: {ex.Message}");
            }
        }
    }
}