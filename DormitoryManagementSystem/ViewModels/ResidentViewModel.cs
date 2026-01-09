using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DormitoryManagementSystem.Models;
using DormitoryManagementSystem.Views;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using DormitoryManagementSystem.Services;
using System.Diagnostics;

namespace DormitoryManagementSystem.ViewModels
{
    public partial class ResidentViewModel : ObservableObject
    {
        [ObservableProperty]
        private string welcomeText;
        private readonly MainWindow _mainWindow;
        private readonly int _userId;
        private readonly UserInfo _user;
        private readonly RoomService _roomService;
        private readonly AdminMessageService _adminMessageService;
        private readonly EvictionService _evictionService;
//        private readonly DatabaseContext _db;

        //eviction
        [ObservableProperty]
        private string _evictionMessage = string.Empty;
        
        [ObservableProperty]
        private Visibility _evictionMessageVisibility = Visibility.Collapsed;


        public ResidentViewModel (MainWindow mainwindow, UserInfo user, RoomService roomService, AdminMessageService adminMessageService, EvictionService evictionService)
        {
            _mainWindow = mainwindow;
            _roomService = roomService;
            _userId = user.Id;
            _adminMessageService = adminMessageService;
            _evictionService = evictionService;
            welcomeText = $"Вітаємо, {user.FullName}!";
            _user = user ?? throw new ArgumentNullException(nameof(user));
            _ = RefreshAdminMessagesAsync();
            _ = CheckEvictionNotificationAsync();
        }
        private async Task RefreshAdminMessagesAsync()
        {
            var activeMessages = await _adminMessageService.GetAllActiveAsync();
            LoadAdminMessages(activeMessages);
        }

        [RelayCommand]
        private void OpenContact()
        {
            var service = new ContactService(new DatabaseContext());
            var vm = new ContactViewModel(service, App.CurrentUser?.Id, _mainWindow, _user);
            var view = new ContactView(_mainWindow, _userId, _user);
            _mainWindow.NavigateTo(view);
        }

        [RelayCommand]
        private void OpenPayment()
        {
            const string paymentUrl = "https://next.privat24.ua/payments/dashboard";

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = paymentUrl,
                    UseShellExecute = true  // зовнішній браузер
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вдалося відкрити платіжну сторінку:\n{ex.Message}");
            }
        }

        [RelayCommand]
        private async Task OpenMyRoomAsync()
        {
            var view = new MyRoomView(_mainWindow, _user);
            _mainWindow.NavigateTo(view);
        }

        // Команда для виходу
        [RelayCommand]
        private void Logout()
        {
            // Очистка поточного користувача
            App.CurrentUser = null;

            // Повернення до LoginView
            _mainWindow.NavigateTo(new LoginView(_mainWindow));
        }

        [RelayCommand]
        private void OpenPersonalInfo()
        {
            using var context = new DatabaseContext();

            var loadedUser = context.Users
                .Include(u => u.RoomPlace)
                .ThenInclude(rp => rp.Room)
                .FirstOrDefault(u => u.Id == _userId);

            if (loadedUser != null)
            {
                _mainWindow.NavigateTo(new ResidentInfoView(_mainWindow, loadedUser));
            }
        }
        [RelayCommand]
        private void OpenShowerReservation()
        {
            using var context = new DatabaseContext();
            var user = context.Users.FirstOrDefault(u => u.Id == _userId);

            if (user is null)
            {
                MessageBox.Show("Користувача не знайдено.");
                return;
            }

            var db = new DatabaseContext();
            var service = new ShowerReservationService(db);
            var viewModel = new ShowerReservationViewModel(service, user.Id, user.Gender, _mainWindow, user);
            var view = new ShowerReservationView(user.Id, user.Gender, _mainWindow, user);

            // Навігація в головне вікно
            _mainWindow.NavigateTo(view);

        }


        public ObservableCollection<AdminMessage> AdminMessages { get; } = new();

        public AdminMessage? ActiveAdminMessage => AdminMessages.FirstOrDefault(m => m.IsActive);

        public Visibility AdminMessageVisibility => ActiveAdminMessage != null ? Visibility.Visible : Visibility.Collapsed;

        public string AdminMessageContent => ActiveAdminMessage?.Content ?? "";

        public void LoadAdminMessages(IEnumerable<AdminMessage> messages)
        {
            AdminMessages.Clear();
            foreach (var m in messages)
                AdminMessages.Add(m);

            OnPropertyChanged(nameof(ActiveAdminMessage));
            OnPropertyChanged(nameof(AdminMessageVisibility));
            OnPropertyChanged(nameof(AdminMessageContent));
        }
        private async Task CheckEvictionNotificationAsync()
        {
            var db = new DatabaseContext();
            var evictionService = new EvictionService(db);
            var notification = await evictionService.GetEvictionNotificationAsync(_userId);

            if (notification != null)
            {
                var daysLeft = (notification.BlockDate - DateTime.Now).Days;
                EvictionMessage = $"⚠️ Ви виселені! Причина: {notification.Reason}\n" +
                                  $"Обліковий запис буде заблоковано через {daysLeft} днів " +
                                  $"({notification.BlockDate:dd.MM.yyyy})";
                EvictionMessageVisibility = Visibility.Visible;

                // Позначити як прочитане
                await evictionService.MarkAsReadAsync(notification.Id);
            }
        }
    }
}
