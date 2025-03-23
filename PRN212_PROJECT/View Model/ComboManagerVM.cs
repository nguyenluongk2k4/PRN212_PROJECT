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
        // declare variable
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

                }
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
        public string Error { get; set; }

        public ICommand AddNewCombo { get; set; }
        public ICommand DeleteCombo { get; set; }
        public ICommand UpdateCombo { get; set; }



        //end declare
        public ComboManagerVM()
        {
            LoadCombo();
            AddNewCombo = new RelayCommand(ExecuteAddCombo, CanAddCombo);

        }
        private void LoadCombo()
        {
            var listCombo = ChickenPrnContext.Ins.Combos.Include(x => x.ComboDetails).ToList();
            if (!listCombo.IsNullOrEmpty())
            {
                Combo = new ObservableCollection<Combo>(listCombo);

            }
        }
        private void LoadComboDetailsWithFood(int comboId)
        {
            // Load ComboDetails including the virtual Food property for the selected Combo
            var comboDetails = ChickenPrnContext.Ins.ComboDetails
                .Include(cd => cd.Food) // Include the virtual Food property
                .Where(cd => cd.ComboId == comboId)
                .ToList();

            if (!comboDetails.IsNullOrEmpty())
            {
                ListComboFood = new ObservableCollection<ComboDetail>(comboDetails);
            }
            else
            {
                ListComboFood = new ObservableCollection<ComboDetail>();
            }
        }

        private bool CanAddCombo(object parameter)
        {
            return !NewComboName.IsNullOrEmpty() && Regex.IsMatch(NewComboName, @"^[a-zA-Z0-9\s]+$")&& ChickenPrnContext.Ins.Combos.Any(x => x.ComboName.ToLower().Contains(NewComboName.ToLower().Trim()));
        }
        public void ExecuteAddCombo(object parameter)
        {
            bool existedCombo = ChickenPrnContext.Ins.Combos.Any(x => x.ComboName.ToLower().Contains(NewComboName.ToLower().Trim()));
            if (existedCombo)
            {
                Error = "Tên đã tồn tại";
            }
            else
            {
                Error = "";
                Combo newCB = new Combo() { ComboName = NewComboName, Status = 1 };
                ChickenPrnContext.Ins.Combos.Add(newCB);
                ChickenPrnContext.Ins.SaveChanges();
                LoadCombo();
                MessageBox.Show("Thêm thành công");
            }
        }

    }
}
