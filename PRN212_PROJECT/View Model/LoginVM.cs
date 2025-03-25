using PRN212_PROJECT.Models;
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
        public static Account Account { get; private set; }

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
                var account = ChickenPrnContext.Ins.Accounts
                    .Where(x => x.Username == _vm.Email && x.Password == _vm.Password)
                    .FirstOrDefault();

                if (account != null)
                {
                    MessageBox.Show("Login Successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    Window dashboard = null;

                    switch (account.RoleId)
                    {
                        case 1:
                            dashboard = new AdminDashBoard();
                            break;
                        case 2:
                            dashboard = new AdminDashBoard();
                            break;
                        case 3:
                            dashboard = new Cooker();
                            break;
                        case 4:
                            dashboard = new CustomerOrderScreen();
                            break;
                        default:
                            MessageBox.Show("Unknown Role", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                    }

                    if (dashboard != null)
                    {
                        dashboard.Show();

                        
                        Application.Current.Windows[0]?.Close();
                    }
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
