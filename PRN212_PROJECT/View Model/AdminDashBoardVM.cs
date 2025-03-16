using PRN212_PROJECT.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PRN212_PROJECT.View_Model
{
    public class AdminDashBoardVM: BaseViewModel
    {
        public ICommand GoOrder => new GoOrderImpl(this);
        public class GoOrderImpl : ICommand
        {
            private readonly AdminDashBoardVM _viewModel;

            public GoOrderImpl(AdminDashBoardVM viewModel)
            {
                _viewModel = viewModel;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter)
            {
                // Mở cửa sổ quản lý đơn hàng
                ManageOrder od = new ManageOrder();
                od.Show();
                Application.Current.Windows[0].Close();
                

            }
        }


    }
}
