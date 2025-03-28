using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PRN212_PROJECT.Models;

namespace PRN212_PROJECT.View_Model
{
    public class ImportIngredientVM : BaseViewModel
    {
        private ObservableCollection<SupplierOrder> _supplierOrders;
        public ObservableCollection<SupplierOrder> SupplierOrders
        {
            get => _supplierOrders;
            set
            {
                _supplierOrders = value;
                OnPropertyChanged(nameof(SupplierOrders));
            }
        }

        public ICommand ExportOrderDetailData { get; set; }

        public ImportIngredientVM()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            SupplierOrders = new ObservableCollection<SupplierOrder>();
            LoadSupplierOrder();
            ExportOrderDetailData = new RelayCommand(ExportFile);
        }

        private void LoadSupplierOrder()
        {
            var orders = ChickenPrnContext.Ins.SupplierOrders
                .Include(x => x.Supplier)
                .OrderByDescending(o => o.Id)
                .ToList();
            SupplierOrders = new ObservableCollection<SupplierOrder>(orders);
        }

        private void ExportFile(object parameter)
        {
            // Get selected orders; if none are selected, export all
            var selectedOrders = SupplierOrders.Where(o => o.IsSelected).ToList();
            if (!selectedOrders.Any())
            {
                selectedOrders = SupplierOrders.ToList(); // Export all if none selected
    
            }
      

       

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Lưu file Excel",
                FileName = "SupplierOrderDetails.xlsx"
            };

            if (saveFileDialog.ShowDialog() != true) return;

            try
            {
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("SupplierOrderDetails");

                    // Add headers
                    worksheet.Cells[1, 1].Value = "Mã Đơn Hàng";
                    worksheet.Cells[1, 2].Value = "Nhà Cung Cấp";
                    worksheet.Cells[1, 3].Value = "Ngày Đặt Hàng";
                    worksheet.Cells[1, 4].Value = "Ngày Giao Hàng";
                    worksheet.Cells[1, 5].Value = "Tổng Tiền";
                    worksheet.Cells[1, 6].Value = "Đã Thanh Toán";
                    worksheet.Cells[1, 7].Value = "Tên Sản Phẩm";
                    worksheet.Cells[1, 8].Value = "Số Lượng";
                    worksheet.Cells[1, 9].Value = "Đơn Vị Đo";
                    worksheet.Cells[1, 10].Value = "Giá Tiền";
                    worksheet.Cells[1, 11].Value = "Tổng Sản Phẩm";

                    System.Diagnostics.Debug.WriteLine("Headers added to Excel file.");

                    int row = 2;
                    int totalDetailsProcessed = 0;

                    foreach (var order in selectedOrders)
                    {
                        var details = ChickenPrnContext.Ins.SupplierOrderDetails
                            .Where(d => d.SupplierOrderId == order.Id)
                            .ToList();

                   
                        System.Diagnostics.Debug.WriteLine($"Order ID: {order.Id} has {details.Count} details.");

                        if (!details.Any())
                        {
                           
                          
                            worksheet.Cells[row, 1].Value = order.Id;
                            worksheet.Cells[row, 2].Value = order.Supplier?.Name ?? "N/A";
                            worksheet.Cells[row, 3].Value = order.OrderDate?.ToString("dd/MM/yyyy") ?? "N/A";
                            worksheet.Cells[row, 4].Value = order.DeliverDate?.ToString("dd/MM/yyyy") ?? "Chưa xác định";
                            worksheet.Cells[row, 5].Value = order.Total;
                            worksheet.Cells[row, 5].Style.Numberformat.Format = "#,##0 VND";
                            worksheet.Cells[row, 6].Value = order.IsPaid.HasValue && order.IsPaid.Value ? "Đã thanh toán" : "Chưa thanh toán";
                            worksheet.Cells[row, 7].Value = "Không có chi tiết";
                            row++;
                            continue;
                        }

                        foreach (var detail in details)
                        {
                            
                          

                            worksheet.Cells[row, 1].Value = order.Id;
                            worksheet.Cells[row, 2].Value = order.Supplier?.Name ?? "N/A";
                            worksheet.Cells[row, 3].Value = order.OrderDate?.ToString("dd/MM/yyyy") ?? "N/A";
                            worksheet.Cells[row, 4].Value = order.DeliverDate?.ToString("dd/MM/yyyy") ?? "Chưa xác định";
                            worksheet.Cells[row, 5].Value = order.Total;
                            worksheet.Cells[row, 5].Style.Numberformat.Format = "#,##0 VND";
                            worksheet.Cells[row, 6].Value = order.IsPaid.HasValue && order.IsPaid.Value ? "Đã thanh toán" : "Chưa thanh toán";

                            worksheet.Cells[row, 7].Value = detail.ProductName ?? "N/A";
                            worksheet.Cells[row, 8].Value = detail.Amount ?? 0;
                            worksheet.Cells[row, 9].Value = detail.CalculationUnit ?? "N/A";
                            worksheet.Cells[row, 10].Value = detail.UnitPrice ?? 0;
                            worksheet.Cells[row, 10].Style.Numberformat.Format = "#,##0 VND";
                            worksheet.Cells[row, 11].Value = (detail.Amount ?? 0) * (detail.UnitPrice ?? 0);
                            worksheet.Cells[row, 11].Style.Numberformat.Format = "#,##0 VND";

                            row++;
                            totalDetailsProcessed++;
                        }
                    }

                    // Log the total number of rows inserted
                    System.Diagnostics.Debug.WriteLine($"Total details processed: {totalDetailsProcessed}");
                    System.Diagnostics.Debug.WriteLine($"Total rows written (including headers): {row}");

                

                    // Auto-fit columns
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                 

                 
                    var file = new FileInfo(saveFileDialog.FileName);
                    package.SaveAs(file);
                   

             

                    MessageBox.Show("Xuất file Excel thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during export: {ex.Message}\nStack Trace: {ex.StackTrace}");
                MessageBox.Show($"Lỗi khi xuất file Excel: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
