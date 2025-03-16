using PRN212_PROJECT.View;
using System;
using System.Windows;
using System.Windows.Input;

namespace PRN212_PROJECT.View_Model
{
    public class LoginVM : BaseViewModel
    {
        private string _email;
        private string _password;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand => new LoginCommandImpl(this);

        private class LoginCommandImpl : ICommand
        {
            private readonly LoginVM _vm;

            public LoginCommandImpl(LoginVM vm)
            {
                _vm = vm;
            }

            public bool CanExecute(object parameter)
            {
                return !string.IsNullOrEmpty(_vm.Email) && !string.IsNullOrEmpty(_vm.Password);
            }

            public void Execute(object parameter)
            {
                if (_vm.Email == "admin" && _vm.Password == "123456")
                {
                    MessageBox.Show("Login Successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    AdminDashBoard a = new AdminDashBoard();

                    a.Show();

                    Application.Current.Windows[0]?.Close();

                }
                else
                {
                    MessageBox.Show("Invalid Email or Password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
        }
    }
}
