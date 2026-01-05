using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DormitoryManagementSystem.Models;
using DormitoryManagementSystem.Services;
using DormitoryManagementSystem.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace DormitoryManagementSystem.ViewModels
{
    public partial class AdminContactsViewModel : ObservableObject
    {
        private readonly ContactService _service;
        private readonly MainWindow _mainWindow;
        private readonly UserInfo _currentUser;

        public ObservableCollection<ContactMessage> Messages { get; } = new();

        public IAsyncRelayCommand LoadMessagesCommand { get; }
        public IRelayCommand BackCommand { get; }
        public IRelayCommand<int> MarkAsReadCommand { get; }
        public IRelayCommand<int> DeleteCommand { get; }

        public AdminContactsViewModel(ContactService service, MainWindow mainWindow, UserInfo currentUser)
        {
            _service = service;
            _mainWindow = mainWindow;
            _currentUser = currentUser;

            LoadMessagesCommand = new AsyncRelayCommand(LoadMessagesAsync);
            BackCommand = new RelayCommand(OnBack);
            MarkAsReadCommand = new RelayCommand<int>(OnMarkAsRead);
            DeleteCommand = new RelayCommand<int>(OnDelete);
        }

        private async Task LoadMessagesAsync()
        {
            Messages.Clear();
            var all = await _service.GetAllAsync();
            foreach (var m in all)
                Messages.Add(m);
        }

        private void OnBack()
        {
            _mainWindow.NavigateTo(new AdminView(_mainWindow, _currentUser));
        }

        private async void OnMarkAsRead(int messageId)
        {
            await _service.MarkAsReadAsync(messageId);
            await LoadMessagesAsync();
        }

        private async void OnDelete(int messageId)
        {
            var result = MessageBox.Show(
                "Ви дійсно хочете видалити це повідомлення?",
                "Підтвердження видалення",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _service.DeleteMessageAsync(messageId);
                await LoadMessagesAsync();
            }
        }
    }
}
