using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using DormitoryManagementSystem.Models;
using CommunityToolkit.Mvvm.Input;
using DormitoryManagementSystem.Services;
using DormitoryManagementSystem.Views;

namespace DormitoryManagementSystem.ViewModels
{
    public partial class AdminViewModel : ObservableObject 
    {
        [ObservableProperty]
        private string welcomeText;

        private readonly MainWindow _mainWindow;
        private readonly UserInfo _currentUser;
        public AdminViewModel(MainWindow mainWindow, UserInfo user)
        {
            welcomeText = $"Вітаємо, {user.FullName}!";
            _mainWindow = mainWindow;
            _currentUser = user;
        }
        [RelayCommand]
        private void OpenSearch()
        {
            _mainWindow.NavigateTo(new ResidentSearchView(_mainWindow, _currentUser));
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
        private async Task OpenAdminContactsAsync()
        {
            // Припустимо, що App.CurrentUser.IsAdmin == true
            var service = new ContactService(new DatabaseContext());
            var vm = new AdminContactsViewModel(service, _mainWindow, _currentUser);
            var view = new AdminContactsView(vm); // побудуємо View далі
            _mainWindow.NavigateTo(view);

            // Завантажимо повідомлення відразу:
            await vm.LoadMessagesCommand.ExecuteAsync(null);
        }
        [RelayCommand]
        private void OpenSettlement()
        {
            _mainWindow.NavigateTo(new SettlementView(_mainWindow, _currentUser));
        }
    }
}
