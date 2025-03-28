using Microsoft.EntityFrameworkCore;
using PRN212_PROJECT.Models;
using PRN212_PROJECT.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PRN212_PROJECT.View_Model
{
    public class RoleManagementVM : BaseViewModel
    {
        private ObservableCollection<Role> _roles;
        public ObservableCollection<Role> Roles
        {
            get => _roles;
            set
            {
                _roles = value;
                OnPropertyChanged();
            }
        }

        private Role _selectedRole;
        public Role SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged();
                LoadRolePermissions();
            }
        }

        private ObservableCollection<RolePermissionViewModel> _rolePermissions;
        public ObservableCollection<RolePermissionViewModel> RolePermissions
        {
            get => _rolePermissions;
            set
            {
                _rolePermissions = value;
                OnPropertyChanged();
            }
        }

        public ICommand GoBackCommand { get; }
        public ICommand SavePermissionsCommand { get; }

        

        public RoleManagementVM()
        {
            
            LoadRoles();

            GoBackCommand = new RelayCommand(ExecuteGoBack);
            SavePermissionsCommand = new RelayCommand(ExecuteSavePermissions);
        }

        private void LoadRoles()
        {
            // Load tất cả roles từ database bằng LINQ
            var roles = ChickenPrnContext.Ins.Roles
                .ToList();
            Roles = new ObservableCollection<Role>(roles);
        }

        private void LoadRolePermissions()
        {
            if (SelectedRole == null)
            {
                RolePermissions = null;
                return;
            }

            // Load tất cả permissions từ database
            var allPermissions = ChickenPrnContext.Ins.Permissions
                .ToList();

            // Load permissions của role được chọn
            var rolePermissions = ChickenPrnContext.Ins.Roles
                .Include(r => r.Permissions)
                .Where(r => r.RoleId == SelectedRole.RoleId)
                .SelectMany(r => r.Permissions)
                .Select(p => p.PermissionId)
                .ToList();

            // Tạo danh sách RolePermissionViewModel để hiển thị trên giao diện
            RolePermissions = new ObservableCollection<RolePermissionViewModel>(
                allPermissions.Select(p => new RolePermissionViewModel
                {
                    PermissionId = p.PermissionId,
                    PermissionName = p.PermissionName,
                    HasPermission = rolePermissions.Contains(p.PermissionId)
                }));
        }

        private void ExecuteGoBack(object parameter)
        {
            AdminDashBoard previousWindow = new AdminDashBoard();
            previousWindow.Show();
            Application.Current.Windows.OfType<RoleManagement>().FirstOrDefault()?.Close();
        }

        private void ExecuteSavePermissions(object parameter)
        {
            if (SelectedRole == null)
            {
                MessageBox.Show("Vui lòng chọn một role để lưu quyền!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Load role cùng với permissions hiện tại
            var role = ChickenPrnContext.Ins.Roles
                .Include(r => r.Permissions)
                .FirstOrDefault(r => r.RoleId == SelectedRole.RoleId);

            if (role == null)
            {
                MessageBox.Show("Không tìm thấy role!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Xóa tất cả quyền hiện tại của role
            role.Permissions.Clear();

            // Thêm lại các quyền được chọn
            var selectedPermissionIds = RolePermissions
                .Where(rp => rp.HasPermission)
                .Select(rp => rp.PermissionId)
                .ToList();

            var permissionsToAdd = ChickenPrnContext.Ins.Permissions
                .Where(p => selectedPermissionIds.Contains(p.PermissionId))
                .ToList();

            foreach (var permission in permissionsToAdd)
            {
                role.Permissions.Add(permission);
            }

            ChickenPrnContext.Ins.SaveChanges();
            MessageBox.Show($"Đã lưu quyền cho role {SelectedRole.RoleName}!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            AccountLogin.Clear();
            Login previousWindow = new Login();
            previousWindow.Show();
            Application.Current.Windows.OfType<RoleManagement>().FirstOrDefault()?.Close();

            
        }
    }

    // Class phụ để hiển thị quyền trên giao diện
    public class RolePermissionViewModel : BaseViewModel
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }

        private bool _hasPermission;
        public bool HasPermission
        {
            get => _hasPermission;
            set
            {
                _hasPermission = value;
                OnPropertyChanged();
            }
        }
    }
}