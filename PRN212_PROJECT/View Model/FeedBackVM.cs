using Microsoft.EntityFrameworkCore;
using PRN212_PROJECT.Models;
using PRN212_PROJECT.View;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace PRN212_PROJECT.View_Model
{
    public class FeedBackVM : BaseViewModel
    {
        
        public ICommand GoFeedBack { get; }

        public ICommand GoOrderFood {  get; }
        public ICommand LogoutBtn { get; set; }

        public ICommand GoBackOrderCommand {  get; }

        private void LogOutBtn(object parameter)
        {
            AccountLogin.Clear();
            Login lo = new Login();
            lo.Show();
            Application.Current.Windows.OfType<FeedBackOderder>().FirstOrDefault()?.Close();
        }
        private void ExecuteGoFeedBack(object parameter)
        {
            FeedbackForm fb = new FeedbackForm();
            fb.Show();
            Application.Current.Windows.OfType<FeedBackOderder>().FirstOrDefault()?.Close();
        }
        private void ExecuteGoMenuOrder(object parameter)
        {
            FeedBackOderder fo = new FeedBackOderder();
            fo.Show();
            Application.Current.Windows.OfType<FeedbackForm>().FirstOrDefault()?.Close();
        }

        private void ExecuteGoOrder(object parameter) { 
            CustomerOrderScreen om = new CustomerOrderScreen();
            om.Show();
            Application.Current.Windows.OfType<FeedBackOderder>().FirstOrDefault()?.Close();

        }
        public FeedBackVM()
        {
            GoFeedBack = new RelayCommand(ExecuteGoFeedBack);
            
            GoBackOrderCommand = new RelayCommand(ExecuteGoMenuOrder);
            GoOrderFood = new RelayCommand(ExecuteGoOrder);
            SubmitFeedbackCommand = new RelayCommand(ExecuteSubmitFeedback);

        }

        private string _feedbackContent;
        public string FeedbackContent
        {
            get => _feedbackContent;
            set
            {
                _feedbackContent = value;
                OnPropertyChanged();
            }
        }

        private int _feedbackRating;
        public int FeedbackRating
        {
            get => _feedbackRating;
            set
            {
                _feedbackRating = value;
                
                OnPropertyChanged();
            }
        }

        public ICommand GoBackCommand { get; }
        public ICommand SubmitFeedbackCommand { get; }

       

        private void ExecuteGoBack(object parameter)
        {
            // Quay lại màn hình trước (FeedBackOderder)
            FeedBackOderder previousWindow = new FeedBackOderder();
            previousWindow.Show();
            Application.Current.Windows.OfType<FeedbackForm>().FirstOrDefault()?.Close();
        }

        private void ExecuteSubmitFeedback(object parameter)
        {
            if (string.IsNullOrWhiteSpace(FeedbackContent))
            {
                MessageBox.Show("Vui lòng nhập nội dung phản hồi!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (FeedbackRating == 0)
            {
                MessageBox.Show("Vui lòng chọn số sao đánh giá!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Feedback f = new Feedback()
            {
                Content = FeedbackContent,
                Rate = FeedbackRating,
                TimeFeedback = DateTime.Now,
            };

            ChickenPrnContext.Ins.Feedbacks.Add(f);
            ChickenPrnContext.Ins.SaveChanges();

            // Sau khi gửi, quay lại màn hình trước
            ExecuteGoBack(null);
        }
    



    }
}