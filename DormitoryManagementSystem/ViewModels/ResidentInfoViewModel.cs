using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DormitoryManagementSystem.Models;
using DormitoryManagementSystem.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormitoryManagementSystem.ViewModels
{
    public partial class ResidentInfoViewModel : ObservableObject
    {
        private readonly MainWindow _mainWindow;
        private readonly UserInfo _user;

        public string FullName => $"{_user.FullName}";
        public string? RoomInfo =>
            _user.RoomPlace?.Room != null
                ? $" {_user.RoomPlace.Room.Name}, Поверх: {_user.RoomPlace.Room.Floor}"
                : "Не заселено";
        public string Email => $" {_user.Email}";
        public string PhoneNumber => $" {_user.PhoneNumber}";

        public ResidentInfoViewModel(MainWindow mainWindow, UserInfo user)
        {
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            _user = user ?? throw new ArgumentNullException(nameof(user));
        }

        [RelayCommand]
        private void Back()
        {
            _mainWindow.NavigateTo(new ResidentView(_mainWindow, _user));
        }
    }
}
