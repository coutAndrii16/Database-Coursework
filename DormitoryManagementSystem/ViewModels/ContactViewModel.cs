using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DormitoryManagementSystem.Services;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DormitoryManagementSystem.Models;
using DormitoryManagementSystem.Views;

namespace DormitoryManagementSystem.ViewModels
{
    public partial class ContactViewModel : ObservableObject
    {
        private readonly ContactService _service;
        private readonly int? _userId;
        private readonly MainWindow _mainWindow;
        private readonly UserInfo _user;

        [ObservableProperty] private bool _isAnonymous;
        [ObservableProperty] private string _content = string.Empty;

        public ContactViewModel(ContactService service, int? userId, MainWindow mainWindow, UserInfo user)
        {
            _service = service;
            _userId = userId;
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            _user = user ?? throw new ArgumentNullException(nameof(user));
        }
        [RelayCommand]
        private async Task SubmitAsync()
        {
            if (string.IsNullOrWhiteSpace(Content))
                return;

            await _service.AddMessageAsync(IsAnonymous ? null : _userId, Content);

            Content = string.Empty;
            MessageBox.Show("Ваше повідомлення надіслано.");
        }
        [RelayCommand]
        private void Back()
        {
            _mainWindow.NavigateTo(new ResidentView(_mainWindow, _user));
        }
    }

}
