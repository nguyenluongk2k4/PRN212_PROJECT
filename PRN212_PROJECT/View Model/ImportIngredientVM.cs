﻿using System;
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
        private ObservableCollection<SupplierOrder> _filteredSupplierOrders;
        private string _supplierNameFilter;
        private DateOnly? _fromDateFilter;
        private DateOnly? _toDateFilter;

        public ObservableCollection<SupplierOrder> SupplierOrders
        {
            get => _supplierOrders;
            set
            {
                _supplierOrders = value ?? new ObservableCollection<SupplierOrder>();
                OnPropertyChanged(nameof(SupplierOrders));
                ApplyFilters();
            }
        }

        public ObservableCollection<SupplierOrder> FilteredSupplierOrders
        {
            get => _filteredSupplierOrders ?? (_filteredSupplierOrders = new ObservableCollection<SupplierOrder>());
            set
            {
                _filteredSupplierOrders = value ?? new ObservableCollection<SupplierOrder>();
                OnPropertyChanged(nameof(FilteredSupplierOrders));
            }
        }

        public string SupplierNameFilter
        {
            get => _supplierNameFilter;
            set
            {
                _supplierNameFilter = value;
                OnPropertyChanged(nameof(SupplierNameFilter));
                ApplyFilters();
            }
        }

        public DateOnly? FromDateFilter
        {
            get => _fromDateFilter;
            set
            {
                _fromDateFilter = value;
                OnPropertyChanged(nameof(FromDateFilter));
                ApplyFilters();
            }
        }

        public DateOnly? ToDateFilter
        {
            get => _toDateFilter;
            set
            {
                _toDateFilter = value;
                OnPropertyChanged(nameof(ToDateFilter));
                ApplyFilters();
            }
        }

        public ICommand ExportOrderDetailData { get; set; }
        public ICommand ClearFilterCommand { get; set; }

        public ImportIngredientVM()
        {
            // Ensure collections are initialized even if LoadSupplierOrder fails
            _supplierOrders = new ObservableCollection<SupplierOrder>();
            _filteredSupplierOrders = new ObservableCollection<SupplierOrder>();

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            LoadSupplierOrder();
            ExportOrderDetailData = new RelayCommand(ExportFile);
            ClearFilterCommand = new RelayCommand(ClearFilters);
        }

        private void LoadSupplierOrder()
        {
            try
            {
                var orders = ChickenPrnContext.Ins.SupplierOrders
                    .Include(x => x.Supplier)
                    .OrderByDescending(o => o.Id)
                    .ToList();
                SupplierOrders = new ObservableCollection<SupplierOrder>(orders); // This triggers ApplyFilters
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách đơn hàng: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SupplierOrders = new ObservableCollection<SupplierOrder>();
            }
        }

        private void ApplyFilters()
        {
            if (SupplierOrders == null)
            {
                FilteredSupplierOrders = new ObservableCollection<SupplierOrder>();
                return;
            }

            IEnumerable<SupplierOrder> filtered = SupplierOrders;

            // Filter by Supplier Name
            if (!string.IsNullOrWhiteSpace(SupplierNameFilter))
            {
                filtered = filtered.Where(x => x.Supplier?.Name != null &&
                    x.Supplier.Name.Contains(SupplierNameFilter, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by From Date
            if (FromDateFilter.HasValue)
            {
                filtered = filtered.Where(x => x.OrderDate.HasValue &&
                    x.OrderDate.Value >= FromDateFilter.Value);
            }

            // Filter by To Date
            if (ToDateFilter.HasValue)
            {
                filtered = filtered.Where(x => x.OrderDate.HasValue &&
                    x.OrderDate.Value <= ToDateFilter.Value);
            }

            // Update FilteredSupplierOrders safely
            FilteredSupplierOrders.Clear();
            foreach (var order in filtered)
            {
                FilteredSupplierOrders.Add(order);
            }
        }

        private void ClearFilters(object parameter)
        {
            SupplierNameFilter = null;
            FromDateFilter = null;
            ToDateFilter = null;
        }

        private void ExportFile(object parameter)
        {
            if (FilteredSupplierOrders == null || !FilteredSupplierOrders.Any())
            {
                MessageBox.Show("Không có đơn hàng nào để xuất!", "Cảnh báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedOrders = FilteredSupplierOrders.Where(o => o.IsSelected).ToList();
            if (!selectedOrders.Any())
            {
                selectedOrders = FilteredSupplierOrders.ToList();
            }

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Save Excel File",
                FileName = "SupplierOrdersExport.xlsx"
            };

            if (saveFileDialog.ShowDialog() != true) return;

            try
            {
                using (var package = new ExcelPackage())
                {
                    foreach (var order in selectedOrders)
                    {
                        string sheetName = $"{order.Supplier.Name} Ngày {order.OrderDate}";
                        var worksheet = package.Workbook.Worksheets.Add(sheetName);

                        worksheet.Cells[1, 1].Value = "Tên Sản Phẩm";
                        worksheet.Cells[1, 2].Value = "Số Lượng";
                        worksheet.Cells[1, 3].Value = "Đơn Vị Đo";
                        worksheet.Cells[1, 4].Value = "Giá Tiền";
                        worksheet.Cells[1, 5].Value = "Tổng Sản Phẩm";

                        worksheet.HeaderFooter.FirstHeader.CenteredText =
                            $"Mã Đơn: {order.Id} | Nhà Cung Cấp: {order.Supplier?.Name ?? "N/A"} | " +
                            $"Ngày Đặt: {order.OrderDate?.ToString("dd/MM/yyyy") ?? "N/A"} | " +
                            $"Ngày Giao: {order.DeliverDate?.ToString("dd/MM/yyyy") ?? "Chưa xác định"} | " +
                            $"Tổng Tiền: {order.Total:#,##0 VND} | " +
                            $"Trạng Thái: {(order.IsPaid.HasValue && order.IsPaid.Value ? "Đã thanh toán" : "Chưa thanh toán")}";

                        var details = ChickenPrnContext.Ins.SupplierOrderDetails
                            .Where(d => d.SupplierOrderId == order.Id)
                            .ToList();

                        int row = 2;

                        if (!details.Any())
                        {
                            worksheet.Cells[row, 1].Value = "Không có chi tiết";
                            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                            continue;
                        }

                        foreach (var detail in details)
                        {
                            worksheet.Cells[row, 1].Value = detail.ProductName ?? "N/A";
                            worksheet.Cells[row, 2].Value = detail.Amount ?? 0;
                            worksheet.Cells[row, 3].Value = detail.CalculationUnit ?? "N/A";
                            worksheet.Cells[row, 4].Value = detail.UnitPrice ?? 0;
                            worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0 VND";
                            worksheet.Cells[row, 5].Value = (detail.Amount ?? 0) * (detail.UnitPrice ?? 0);
                            worksheet.Cells[row, 5].Style.Numberformat.Format = "#,##0 VND";
                            row++;
                        }

                        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                    }

                    var file = new FileInfo(saveFileDialog.FileName);
                    package.SaveAs(file);

                    MessageBox.Show("Xuất file Excel thành công!", "Thành công",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất file Excel: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}