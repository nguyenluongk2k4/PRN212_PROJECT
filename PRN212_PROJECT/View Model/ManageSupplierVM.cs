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
                OnPropertyChanged(nameof(Suppliers));
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
                ValidatePhone(); // Validate on change
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
                ValidateEmail(); // Validate on change
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

        private string _phoneErrorMessage;
        public string PhoneErrorMessage
        {
            get => _phoneErrorMessage;
            set
            {
                _phoneErrorMessage = value;
                OnPropertyChanged(nameof(PhoneErrorMessage));
            }
        }

        private string _emailErrorMessage;
        public string EmailErrorMessage
        {
            get => _emailErrorMessage;
            set
            {
                _emailErrorMessage = value;
                OnPropertyChanged(nameof(EmailErrorMessage));
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
            AddNewSupplier = new RelayCommand(AddNewSupplierToDB, CanAddNewSupplier);
            UpdateSupplier = new RelayCommand(UpdateSupplierInDB, CanUpdateSupplier);
            DeleteSupplier = new RelayCommand(DeleteSupplierFromDB, CanDeleteSupplier);
            Cancel = new RelayCommand(ClearForm, _ => true);
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
                PhoneErrorMessage = string.Empty; // Clear errors when loading
                EmailErrorMessage = string.Empty;
            }
        }

        private void ClearForm(object parameter)
        {
            NewSupplierName = string.Empty;
            NewSupplierAddress = string.Empty;
            NewSupplierEmail = string.Empty;
            NewSupplierPhone = string.Empty;
            SelectedSupplier = null;
            PhoneErrorMessage = string.Empty;
            EmailErrorMessage = string.Empty;
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
                PhoneErrorMessage = "Số điện thoại không được để trống!";
                return false;
            }

            if (!Regex.IsMatch(NewSupplierPhone, @"^\d{10}$"))
            {
                PhoneErrorMessage = "Số điện thoại phải gồm 10 chữ số!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(NewSupplierAddress))
            {
                errorMessage = "Địa chỉ không được để trống!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(NewSupplierEmail))
            {
                EmailErrorMessage = "Email không được để trống!";
                return false;
            }

            if (!Regex.IsMatch(NewSupplierEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                EmailErrorMessage = "Email không đúng định dạng!";
                return false;
            }

            return true;
        }

        private void ValidatePhone()
        {
            if (!string.IsNullOrEmpty(NewSupplierPhone) &&
                ChickenPrnContext.Ins.Suppliers.Any(x => x.PhoneNumber.Equals(NewSupplierPhone) &&
                (SelectedSupplier == null || x.Id != SelectedSupplier.Id)))
            {
                PhoneErrorMessage = "Số điện thoại đã tồn tại!";
            }
            else
            {
                PhoneErrorMessage = string.Empty;
            }
        }

        private void ValidateEmail()
        {
            if (!string.IsNullOrEmpty(NewSupplierEmail) &&
                ChickenPrnContext.Ins.Suppliers.Any(x => x.Email.Equals(NewSupplierEmail) &&
                (SelectedSupplier == null || x.Id != SelectedSupplier.Id)))
            {
                EmailErrorMessage = "Email đã tồn tại!";
            }
            else
            {
                EmailErrorMessage = string.Empty;
            }
        }

        // Add Supplier
        private bool CanAddNewSupplier(object parameter)
        {
            return !string.IsNullOrEmpty(NewSupplierPhone) &&
                   !ChickenPrnContext.Ins.Suppliers.Any(x => x.PhoneNumber.Equals(NewSupplierPhone) || x.Email.Equals(NewSupplierEmail));
        }

        private void AddNewSupplierToDB(object parameter)
        {
            if (!ValidateFields(out string errorMessage))
            {
                if (string.IsNullOrEmpty(PhoneErrorMessage) && string.IsNullOrEmpty(EmailErrorMessage))
                    MessageBox.Show(errorMessage, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!string.IsNullOrEmpty(PhoneErrorMessage) || !string.IsNullOrEmpty(EmailErrorMessage))
                return;

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
            return SelectedSupplier != null &&
                   (!ChickenPrnContext.Ins.Suppliers.Any(x => (x.PhoneNumber.Equals(NewSupplierPhone) || x.Email.Equals(NewSupplierEmail)) && x.Id != SelectedSupplier.Id));
        }

        private void UpdateSupplierInDB(object parameter)
        {
            if (!ValidateFields(out string errorMessage))
            {
                if (string.IsNullOrEmpty(PhoneErrorMessage) && string.IsNullOrEmpty(EmailErrorMessage))
                    MessageBox.Show(errorMessage, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!string.IsNullOrEmpty(PhoneErrorMessage) || !string.IsNullOrEmpty(EmailErrorMessage))
                return;

            var supplierToUpdate = ChickenPrnContext.Ins.Suppliers
                .FirstOrDefault(s => s.Id == SelectedSupplier.Id);

            if (supplierToUpdate != null)
            {
                supplierToUpdate.Name = NewSupplierName;
                supplierToUpdate.PhoneNumber = NewSupplierPhone;
                supplierToUpdate.Email = NewSupplierEmail;
                supplierToUpdate.Address = NewSupplierAddress;

                ChickenPrnContext.Ins.SaveChanges();
                LoadSupplier();
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