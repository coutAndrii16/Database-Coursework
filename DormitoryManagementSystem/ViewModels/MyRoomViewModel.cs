using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DormitoryManagementSystem.Models;
using DormitoryManagementSystem.Models.DTO; // або де ти зберігаєш
using DormitoryManagementSystem.Services;
using DormitoryManagementSystem.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DormitoryManagementSystem.ViewModels
{
    public partial class MyRoomViewModel : ObservableObject
    {
        private readonly RoomService _roomService;
        private readonly int _userId;
        private readonly MainWindow _mainWindow;
        private readonly UserInfo _user;

        public ObservableCollection<RoomMateInfo> RoomMates { get; } = new();

        public IAsyncRelayCommand LoadRoomMatesCommand { get; }

        public MyRoomViewModel(RoomService roomService, int userId, MainWindow mainWindow, UserInfo user)
        {
            _roomService = roomService;
            _userId = userId;
            _user = user;
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));

            LoadRoomMatesCommand = new AsyncRelayCommand(LoadAsync);
        }

        private async Task LoadAsync()
        {
            RoomMates.Clear();
            var users = await _roomService.GetRoomMatesAsync(_userId);
            foreach (var u in users)
            {
                RoomMates.Add(new RoomMateInfo
                {
                    FullName = u.FullName,
                    Gender = u.Gender,
                    Course = u.Course ?? 0,
                    Faculty = u.Faculty?.Name ?? "",
                    Phone = u.PhoneNumber ?? ""
                });
            }
        }
        [RelayCommand]
        private void Back()
        {
            _mainWindow.NavigateTo(new ResidentView(_mainWindow, _user));
        }
    }
}
