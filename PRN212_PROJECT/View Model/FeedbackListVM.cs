using PRN212_PROJECT.Models;
using PRN212_PROJECT.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PRN212_PROJECT.View_Model
{
    public class FeedbackListVM : BaseViewModel
    {
        private DateTime? _fromDate;
        public DateTime? FromDate
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _toDate;
        public DateTime? ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
                OnPropertyChanged();
            }
        }

        private int? _selectedRating;
        public int? SelectedRating
        {
            get => _selectedRating;
            set
            {
                _selectedRating = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Feedback> _feedbackList;
        public ObservableCollection<Feedback> FeedbackList
        {
            get => _feedbackList;
            set
            {
                _feedbackList = value;
                OnPropertyChanged();
            }
        }

        public List<int?> RatingOptions { get; }

        public ICommand GoBackCommand { get; }
        public ICommand SearchFeedbackCommand { get; }

        private List<Feedback> _allFeedbacks; // Danh sách tất cả feedback (giả lập database)

        public FeedbackListVM()
        {
            // Khởi tạo danh sách rating (1-5 sao, và null để chọn tất cả)
            RatingOptions = new List<int?> { null, 1, 2, 3, 4, 5 };

            // Khởi tạo danh sách feedback (giả lập)
            _allFeedbacks = ChickenPrnContext.Ins.Feedbacks.ToList();

            FeedbackList = new ObservableCollection<Feedback>(_allFeedbacks);

            GoBackCommand = new RelayCommand(ExecuteGoBack);
            SearchFeedbackCommand = new RelayCommand(ExecuteSearchFeedback);
        }

        private void ExecuteGoBack(object parameter)
        {
            AdminDashBoard previousWindow = new AdminDashBoard();
            previousWindow.Show();
            Application.Current.Windows.OfType<FeedbackList>().FirstOrDefault()?.Close();
        }

        private void ExecuteSearchFeedback(object parameter)
        {
            var filteredFeedbacks = _allFeedbacks.AsEnumerable();

            // Lọc theo khoảng ngày
            if (FromDate.HasValue)
            {
                filteredFeedbacks = filteredFeedbacks.Where(f => f.TimeFeedback >= FromDate.Value);
            }

            if (ToDate.HasValue)
            {
                filteredFeedbacks = filteredFeedbacks.Where(f => f.TimeFeedback <= ToDate.Value.AddDays(1));
            }

            // Lọc theo rating
            if (SelectedRating.HasValue)
            {
                filteredFeedbacks = filteredFeedbacks.Where(f => f.Rate == SelectedRating.Value);
            }

            FeedbackList = new ObservableCollection<Feedback>(filteredFeedbacks);
        }
    }
}