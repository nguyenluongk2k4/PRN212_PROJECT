using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using PRN212_PROJECT.Models;

namespace PRN212_PROJECT.View_Model
{
    public class AddNewExpentitureVM : BaseViewModel
    {
        private string? _expentitureName;
        private bool _isExpentitureNameTouched;
        public string? ExpentitureName
        {
            get => _expentitureName;
            set
            {
                _expentitureName = value;
                _isExpentitureNameTouched = true;
                OnPropertyChanged(nameof(ExpentitureName));
                ValidateInputs();
            }
        }

        private string? _executorName;
        private bool _isExecutorNameTouched;
        public string? ExecutorName
        {
            get => _executorName;
            set
            {
                _executorName = value;
                _isExecutorNameTouched = true;
                OnPropertyChanged(nameof(ExecutorName));
                ValidateInputs();
            }
        }

        private string _amountText;
        private bool _isAmountTouched;
        public string AmountText
        {
            get => _amountText;
            set
            {
                _isAmountTouched = true;
                _amountText = FormatNumber(value);
                OnPropertyChanged(nameof(AmountText));
                ValidateInputs();
            }
        }

        private decimal? _amount;
        public decimal? Amount
        {
            get => _amount;
            private set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

        private string _nameError;
        public string NameError
        {
            get => _nameError;
            set
            {
                _nameError = value;
                OnPropertyChanged(nameof(NameError));
            }
        }

        private string _executorError;
        public string ExecutorError
        {
            get => _executorError;
            set
            {
                _executorError = value;
                OnPropertyChanged(nameof(ExecutorError));
            }
        }

        private string _amountError;
        public string AmountError
        {
            get => _amountError;
            set
            {
                _amountError = value;
                OnPropertyChanged(nameof(AmountError));
            }
        }

        public ICommand AddExpend { get; set; }

        public AddNewExpentitureVM()
        {
            AddExpend = new RelayCommand(ExecuteAdd, CanCreate);
        }

        private bool CanCreate(object parameter)
        {
            return string.IsNullOrEmpty(NameError) &&
                   string.IsNullOrEmpty(ExecutorError) &&
                   string.IsNullOrEmpty(AmountError) &&
                   Amount.HasValue && Amount > 0;
        }

        private void ValidateInputs()
        {
            NameError = _isExpentitureNameTouched && !ValidateName(ExpentitureName) ? "Chỉ sử dụng chữ cái!" : null;
            ExecutorError = _isExecutorNameTouched && !ValidateName(ExecutorName) ? "Chỉ sử dụng chữ cái!" : null;
            AmountError = _isAmountTouched && !ValidateAmount(AmountText) ? "Số tiền không hợp lệ!" : null;
        }

        private bool ValidateName(string? name)
        {
            return !string.IsNullOrEmpty(name) && Regex.IsMatch(name, @"^[\p{L}\p{M}\s]+$");
        }

        private bool ValidateAmount(string? amountText)
        {
            if (decimal.TryParse(RemoveCommas(amountText), out decimal result) && result > 0)
            {
                Amount = result;
                return true;
            }
            Amount = null;
            return false;
        }

        private string FormatNumber(string input)
        {
            if (decimal.TryParse(RemoveCommas(input), out decimal number))
            {
                return number.ToString("#,##0", CultureInfo.InvariantCulture);
            }
            return input;
        }

        private string RemoveCommas(string input)
        {
            return input.Replace(",", "").Trim();
        }

        private void ExecuteAdd(object parameter)
        {
            if (CanCreate(parameter))
            {
                Expenditure newE = new Expenditure()
                {
                    Name = ExpentitureName,
                    Executor = ExecutorName,
                    Cost = Amount,
                    Date = DateOnly.FromDateTime(DateTime.Today)
                };
                ChickenPrnContext.Ins.Expenditures.Add(newE);
                ChickenPrnContext.Ins.SaveChanges();
                MessageBox.Show("Thêm thành công!");
                ExpentitureName = null;
                ExecutorName = null;
                Amount=null;
            }
        }
    }
}
