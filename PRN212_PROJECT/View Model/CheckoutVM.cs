using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using PRN212_PROJECT.Models;
using QRCoder;
using System.Drawing;
using System.IO;
using PRN212_PROJECT.Models;
using PRN212_PROJECT.View;


namespace PRN212_PROJECT.View_Model
{
    public class CheckoutVM : BaseViewModel
    {
        private string _customerName;
        public string CustomerName
        {
            get => _customerName;
            set
            {
                _customerName = value;
                OnPropertyChanged(nameof(CustomerName));
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        private bool _shipping;
        public bool Shipping
        {
            get => _shipping;
            set
            {
                _shipping = value;
                OnPropertyChanged(nameof(Shipping));
            }
        }

        private ObservableCollection<OrderDetailFood> _orderDetailFoods;
        public ObservableCollection<OrderDetailFood> OrderDetailFoods
        {
            get => _orderDetailFoods;
            set
            {
                _orderDetailFoods = value;
                OnPropertyChanged(nameof(OrderDetailFoods));
            }
        }

        private ObservableCollection<OrderDetailCombo> _orderDetailCombos;
        public ObservableCollection<OrderDetailCombo> OrderDetailCombos
        {
            get => _orderDetailCombos;
            set
            {
                _orderDetailCombos = value;
                OnPropertyChanged(nameof(OrderDetailCombos));
            }
        }

        private double _totalPrice;
        public double TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public RelayCommand ConfirmOrderCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public CheckoutVM(ObservableCollection<OrderDetailFood> orderDetailFoods, ObservableCollection<OrderDetailCombo> orderDetailCombos, double totalPrice)
        {
            OrderDetailFoods = orderDetailFoods;
            OrderDetailCombos = orderDetailCombos;
            TotalPrice = totalPrice;

            ConfirmOrderCommand = new RelayCommand(
                _ =>
                {
                    // Create and save the order
                    var order = new OrderTable
                    {
                         Date= DateTime.Now,
                        Total = TotalPrice,
                        CustomerName = CustomerName,
                        Address = Address,
                        Shipping = Shipping,
                        OrderDetailFoods = OrderDetailFoods.ToList(),
                        OrderDetailCombos = OrderDetailCombos.ToList()
                    };

                    try
                    {
                        ChickenPrnContext.Ins.OrderTables.Add(order);
                        ChickenPrnContext.Ins.SaveChanges();

                        // Generate QR code for payment
                        var qrCodeImage = GeneratePaymentQRCode(order.OrderId, TotalPrice);

                        // Show the QR code window
                        var qrCodeWindow = new PRN212_PROJECT.View.QRCodeWindow(qrCodeImage);
                        qrCodeWindow.ShowDialog();

                        // Close the checkout screen
                        Application.Current.Windows.OfType<CheckoutScreen>().FirstOrDefault()?.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving order: {ex.Message}");
                    }
                },
                _ => !string.IsNullOrWhiteSpace(CustomerName) && !string.IsNullOrWhiteSpace(Address)
            );

            CancelCommand = new RelayCommand(
                _ =>
                {
                    Application.Current.Windows.OfType<CheckoutScreen>().FirstOrDefault()?.Close();
                });
        }

        private BitmapSource GeneratePaymentQRCode(int orderId, double totalPrice)
        {
            // VietQR format: https://vietqr.io/standards/
            // We'll use a simplified version for this example
            // Replace these with your actual bank account details
            string bankId = "MBBANK"; // Example: MB Bank
            string accountNumber = "1234567890"; // Your bank account number
            string accountName = "Food Store Name"; // Your account name
            string amount = totalPrice.ToString("F0"); // Total price without decimals
            string description = $"Payment for Order {orderId}";

            // VietQR URL format
            var qrContent = $"https://img.vietqr.io/image/{bankId}-{accountNumber}-compact2.png?amount={amount}&addInfo={Uri.EscapeDataString(description)}&accountName={Uri.EscapeDataString(accountName)}";

            // Alternatively, you can use a custom format if your bank provides a specific QR code format
            // For example, a raw string format:
            // var qrContent = $"BANKID:{bankId}|ACCOUNT:{accountNumber}|AMOUNT:{amount}|DESC:{description}";

            // Generate QR code
            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
                using (var qrCode = new QRCode(qrCodeData))
                {
                    using (var qrCodeImage = qrCode.GetGraphic(20))
                    {
                        // Convert Bitmap to BitmapSource for WPF
                        using (var memory = new MemoryStream())
                        {
                            qrCodeImage.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                            memory.Position = 0;

                            var bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = memory;
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.EndInit();
                            bitmapImage.Freeze();

                            return bitmapImage;
                        }
                    }
                }
            }
        }
    }
}