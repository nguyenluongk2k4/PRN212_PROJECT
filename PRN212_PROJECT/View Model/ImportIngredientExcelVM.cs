using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using OfficeOpenXml;
using System.IO;
using PRN212_PROJECT.Models;
using System.Collections.Generic;
using System.Windows;

namespace PRN212_PROJECT.View_Model
{
    public class ImportIngredientExcelVM : BaseViewModel
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

        private List<string> _listSupplierName;
        public List<string> ListSupplierName
        {
            get => _listSupplierName;
            set
            {
                _listSupplierName = value;
                OnPropertyChanged(nameof(ListSupplierName));
            }
        }

        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
                LoadExcelData();
            }
        }

        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }

        private int _rowCount;
        public int RowCount
        {
            get => _rowCount;
            set
            {
                _rowCount = value;
                OnPropertyChanged(nameof(RowCount));
            }
        }

        private ObservableCollection<SupplierOrderDetail> _previewData;
        public ObservableCollection<SupplierOrderDetail> PreviewData
        {
            get => _previewData;
            set
            {
                _previewData = value;
                OnPropertyChanged(nameof(PreviewData));
                CalculateTotalAmount(); 
                UpdateCanImport();
            }
        }

        private string _selectedSupplierName;
        public string SelectedSupplierName
        {
            get => _selectedSupplierName;
            set
            {
                _selectedSupplierName = value;
                OnPropertyChanged(nameof(SelectedSupplierName));
                UpdateCanImport();
            }
        }

        private DateTime? _orderDate;
        public DateTime? OrderDate
        {
            get => _orderDate;
            set
            {
                _orderDate = value;
                OnPropertyChanged(nameof(OrderDate));
                UpdateCanImport();
            }
        }

        private DateTime? _deliveryDate;
        public DateTime? DeliveryDate
        {
            get => _deliveryDate;
            set
            {
                _deliveryDate = value;
                OnPropertyChanged(nameof(DeliveryDate));
         
            }
        }

        private bool _isPaid;
        public bool IsPaid
        {
            get => _isPaid;
            set
            {
                _isPaid = value;
                OnPropertyChanged(nameof(IsPaid));
            }
        }

        private decimal _totalAmount;
        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                _totalAmount = value;
                OnPropertyChanged(nameof(TotalAmount));
                UpdateCanImport();
            }
        }

        private bool _canImport;
        public bool CanImport
        {
            get => _canImport;
            set
            {
                _canImport = value;
                OnPropertyChanged(nameof(CanImport));
            }
        }

        public ICommand ImportCommand { get; }

        public ImportIngredientExcelVM()
        {
            LoadListSupplier();
            PreviewData = new ObservableCollection<SupplierOrderDetail>();
            ImportCommand = new RelayCommand(ExecuteImport, CanExecuteImport);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private void LoadListSupplier()
        {
            var list = ChickenPrnContext.Ins.Suppliers.ToList();
            Suppliers = new ObservableCollection<Supplier>(list);
            ListSupplierName = Suppliers.Select(x => x.Name).ToList();
        }

        private void LoadExcelData()
        {
            if (string.IsNullOrEmpty(FilePath) || !File.Exists(FilePath))
            {
                ClearPreview();
                return;
            }

            FileName = Path.GetFileName(FilePath);
            PreviewData.Clear();

            try
            {
                using (var package = new ExcelPackage(new FileInfo(FilePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension?.Rows ?? 0;
                    if (rowCount < 2) // At least header and one data row
                    {
                        throw new Exception("File Excel không có dữ liệu hoặc không đúng định dạng!");
                    }

                    // Validate headers
                    if (worksheet.Cells[1, 1].Text != "Tên" ||
                        worksheet.Cells[1, 2].Text != "Số Lượng" ||
                        worksheet.Cells[1, 3].Text != "Đơn Vị Đo" ||
                        worksheet.Cells[1, 4].Text != "Giá Tiền" ||
                        worksheet.Cells[1, 5].Text != "Tổng")
                    {
                        throw new Exception("Định dạng file Excel không đúng! Cần các cột: Tên, Số Lượng, Đơn Vị Đo, Giá Tiền, Tổng.");
                    }

                    RowCount = rowCount - 1; // Exclude header row

                    for (int row = 2; row <= rowCount; row++)
                    {
            
                        if (string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Text))
                        {
                            throw new Exception($"Tên sản phẩm không được để trống tại dòng {row}.");
                        }

                        if (!double.TryParse(worksheet.Cells[row, 2].Text.Replace(" kg", "").Trim(), out double amount))
                        {
                            throw new Exception($"Số lượng không hợp lệ tại dòng {row}.");
                        }

                        if (string.IsNullOrWhiteSpace(worksheet.Cells[row, 3].Text))
                        {
                            throw new Exception($"Đơn vị đo không được để trống tại dòng {row}.");
                        }

                        if (!double.TryParse(worksheet.Cells[row, 4].Text.Replace("VND ", "").Replace(",", "").Trim(), out double unitPrice))
                        {
                            throw new Exception($"Giá tiền không hợp lệ tại dòng {row}.");
                        }

                        double calculatedTotal = amount * unitPrice;
                        if (!double.TryParse(worksheet.Cells[row, 5].Text.Replace("VND ", "").Replace(",", "").Trim(), out double total) ||
                            Math.Abs(calculatedTotal - total) > 0.01)
                        {
                            throw new Exception($"Tổng tiền tại dòng {row} không khớp (Số Lượng * Giá Tiền phải bằng Tổng).");
                        }

                        var detail = new SupplierOrderDetail
                        {
                            ProductName = worksheet.Cells[row, 1].Text,
                            Amount = amount,
                            CalculationUnit = worksheet.Cells[row, 3].Text,
                            UnitPrice = unitPrice
                        };
                        PreviewData.Add(detail);
                        TotalAmount += (decimal)(amount * unitPrice);
                    }
                }
        
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đọc file Excel: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearPreview();
            }
        }

        private void CalculateTotalAmount()
        {
            if (PreviewData == null || !PreviewData.Any())
            {
                TotalAmount = 0;
                return;
            }

            decimal total = PreviewData
                .Where(d => d.Amount.HasValue && d.UnitPrice.HasValue)
                .Sum(d => (decimal)(d.Amount.Value * d.UnitPrice.Value));
            TotalAmount = total;
        }

        private void ClearPreview()
        {
            FileName = "Chưa chọn file";
            RowCount = 0;
            PreviewData.Clear();
            // CalculateTotalAmount and UpdateCanImport are called via PreviewData setter
        }

        private void UpdateCanImport()
        {
            CanImport = !string.IsNullOrEmpty(FilePath) &&
                        !string.IsNullOrEmpty(SelectedSupplierName) &&
                        OrderDate.HasValue &&
                        TotalAmount > 0 &&
                        PreviewData.Any();
        }

        private bool CanExecuteImport(object parameter)
        {
            return CanImport;
        }

        private void ExecuteImport(object parameter)
        {
            try
            {
                var supplier = Suppliers.FirstOrDefault(s => s.Name == SelectedSupplierName);
                if (supplier == null)
                {
                    MessageBox.Show("Nhà cung cấp không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Create SupplierOrder
                var supplierOrder = new SupplierOrder
                {
                    SupplierId = supplier.Id,
                    OrderDate = DateOnly.FromDateTime(OrderDate.Value),
                    DeliverDate = DeliveryDate.HasValue ? DateOnly.FromDateTime(DeliveryDate.Value) : null, // Allow null
                    IsPaid = IsPaid,
                    Total = TotalAmount
                };

                ChickenPrnContext.Ins.SupplierOrders.Add(supplierOrder);
                ChickenPrnContext.Ins.SaveChanges();

                // Create SupplierOrderDetails
                foreach (var detail in PreviewData)
                {
                    detail.SupplierOrderId = supplierOrder.Id;
                    ChickenPrnContext.Ins.SupplierOrderDetails.Add(detail);
                }

                ChickenPrnContext.Ins.SaveChanges();

                MessageBox.Show("Import thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi import dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            FilePath = string.Empty;
            FileName = "Chưa chọn file";
            RowCount = 0;
            PreviewData.Clear();
            SelectedSupplierName = null;
            OrderDate = null;
            DeliveryDate = null;
            IsPaid = false;
            TotalAmount = 0;
        }
    }

   
}