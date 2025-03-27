using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using PRN212_PROJECT.Models;

namespace PRN212_PROJECT.View_Model
{
    public class ManageSupplierVM : BaseViewModel
    {
        private ObservableCollection<Supplier> _suppliers;
        public ObservableCollection<Supplier> Suppliers
        {
            get => _suppliers;
            set
            {
                _suppliers = value;
                OnPropertyChanged(nameof(Suppliers)); // Fixed property name
            }
        }

        private string _newSupplierName;
        public string NewSupplierName
        {
            get => _newSupplierName;
            set
            {
                _newSupplierName = value;
                OnPropertyChanged(nameof(NewSupplierName));
            }
        }

        private string _newSupplierPhone;
        public string NewSupplierPhone
        {
            get => _newSupplierPhone;
            set
            {
                _newSupplierPhone = value;
                OnPropertyChanged(nameof(NewSupplierPhone));
            }
        }

        private string _newSupplierAddress;
        public string NewSupplierAddress
        {
            get => _newSupplierAddress;
            set
            {
                _newSupplierAddress = value;
                OnPropertyChanged(nameof(NewSupplierAddress));
            }
        }

        private string _newSupplierEmail;
        public string NewSupplierEmail
        {
            get => _newSupplierEmail;
            set
            {
                _newSupplierEmail = value;
                OnPropertyChanged(nameof(NewSupplierEmail));
            }
        }

        private Supplier _selectedSupplier;
        public Supplier SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged(nameof(SelectedSupplier));
                LoadSelectedToForm();
            }
        }

        public ICommand AddNewSupplier { get; set; }
        public ICommand UpdateSupplier { get; set; }
        public ICommand DeleteSupplier { get; set; }
        public ICommand Cancel { get; set; }

        public ManageSupplierVM()
        {
            LoadSupplier();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            AddNewSupplier = new RelayCommand(AddNewSupplierToDB,CanAddNewSupplier);
            UpdateSupplier = new RelayCommand(UpdateSupplierInDB,CanUpdateSupplier);
            DeleteSupplier = new RelayCommand(DeleteSupplierFromDB,CanDeleteSupplier);
            Cancel = new RelayCommand(ClearForm,_ => true);
        }

        private void LoadSupplier()
        {
            var list = ChickenPrnContext.Ins.Suppliers.ToList();
            Suppliers = new ObservableCollection<Supplier>(list);
        }

        private void LoadSelectedToForm()
        {
            if (SelectedSupplier != null)
            {
                NewSupplierName = SelectedSupplier.Name;
                NewSupplierAddress = SelectedSupplier.Address;
                NewSupplierEmail = SelectedSupplier.Email;
                NewSupplierPhone = SelectedSupplier.PhoneNumber;
            }
        }

        private void ClearForm(object parameter)
        {
            NewSupplierName = string.Empty;
            NewSupplierAddress = string.Empty;
            NewSupplierEmail = string.Empty;
            NewSupplierPhone = string.Empty;
            SelectedSupplier = null;
        }

        // Validation methods
        private bool ValidateFields(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(NewSupplierName))
            {
                errorMessage = "Tên nhà cung cấp không được để trống!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(NewSupplierPhone))
            {
                errorMessage = "Số điện thoại không được để trống!";
                return false;
            }

            if (!Regex.IsMatch(NewSupplierPhone, @"^\d{10}$"))
            {
                errorMessage = "Số điện thoại phải gồm 10 chữ số!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(NewSupplierAddress))
            {
                errorMessage = "Địa chỉ không được để trống!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(NewSupplierEmail))
            {
                errorMessage = "Email không được để trống!";
                return false;
            }

            if (!Regex.IsMatch(NewSupplierEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                errorMessage = "Email không đúng định dạng!";
                return false;
            }

            return true;
        }

        // Add Supplier
        private bool CanAddNewSupplier(object parameter)
        {
            return !string.IsNullOrEmpty(NewSupplierPhone) &&
                   !ChickenPrnContext.Ins.Suppliers.Any(x => x.PhoneNumber.Equals(NewSupplierPhone));
        }

        private void AddNewSupplierToDB(object parameter)
        {
            if (!ValidateFields(out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newSupplier = new Supplier
            {
                Name = NewSupplierName,
                PhoneNumber = NewSupplierPhone,
                Email = NewSupplierEmail,
                Address = NewSupplierAddress
            };

            ChickenPrnContext.Ins.Suppliers.Add(newSupplier);
            ChickenPrnContext.Ins.SaveChanges();
            Suppliers.Add(newSupplier);
            ClearForm(parameter);
            MessageBox.Show("Thêm nhà cung cấp thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Update Supplier
        private bool CanUpdateSupplier(object parameter)
        {
            return SelectedSupplier != null;
        }

        private void UpdateSupplierInDB(object parameter)
        {
            if (!ValidateFields(out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var supplierToUpdate = ChickenPrnContext.Ins.Suppliers
                .FirstOrDefault(s => s.Id == SelectedSupplier.Id);

            if (supplierToUpdate != null)
            {
                supplierToUpdate.Name = NewSupplierName;
                supplierToUpdate.PhoneNumber = NewSupplierPhone;
                supplierToUpdate.Email = NewSupplierEmail;
                supplierToUpdate.Address = NewSupplierAddress;

                ChickenPrnContext.Ins.SaveChanges();
                LoadSupplier(); // Refresh the list
                ClearForm(parameter);
                MessageBox.Show("Cập nhật nhà cung cấp thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Delete Supplier
        private bool CanDeleteSupplier(object parameter)
        {
            return SelectedSupplier != null;
        }

        private void DeleteSupplierFromDB(object parameter)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa nhà cung cấp này?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var supplierToDelete = ChickenPrnContext.Ins.Suppliers
                    .FirstOrDefault(s => s.Id == SelectedSupplier.Id);

                if (supplierToDelete != null)
                {
                    ChickenPrnContext.Ins.Suppliers.Remove(supplierToDelete);
                    ChickenPrnContext.Ins.SaveChanges();
                    Suppliers.Remove(SelectedSupplier);
                    ClearForm(parameter);
                    MessageBox.Show("Xóa nhà cung cấp thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }

}