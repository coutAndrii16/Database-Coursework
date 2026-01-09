using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DormitoryManagementSystem.Models;
using DormitoryManagementSystem.DTO;
using DormitoryManagementSystem.Services;
using DormitoryManagementSystem.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace DormitoryManagementSystem.ViewModels
{
    public partial class SettlementViewModel : ObservableObject
    {
        private readonly MainWindow _mainWindow;
        private readonly UserInfo _currentUser;
        private readonly PasswordBox _passwordBox;
        private readonly DatabaseContext _db;

        [ObservableProperty] private string _fullName = string.Empty;
        [ObservableProperty] private string _email = string.Empty;
        [ObservableProperty] private string _password = string.Empty;
        [ObservableProperty] private string _phoneNumber = string.Empty;
        [ObservableProperty] private string _gender = string.Empty;
        [ObservableProperty] private string _group = string.Empty;
        [ObservableProperty] private string _formOfEducation = string.Empty;
        [ObservableProperty] private int? _course;

        [ObservableProperty] private Faculty? _selectedFaculty;
        [ObservableProperty] private RoomDto? _selectedRoom;
        [ObservableProperty] private RoomPlaceDto? _selectedPlace;

        public ObservableCollection<Faculty> Faculties { get; } = new();
        public ObservableCollection<RoomDto> AvailableRooms { get; } = new();
        public ObservableCollection<RoomPlaceDto> AvailablePlaces { get; } = new();

        public SettlementViewModel(MainWindow mainWindow, UserInfo currentUser, PasswordBox passwordBox)
        {
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _passwordBox = passwordBox ?? throw new ArgumentNullException(nameof(passwordBox));
            _db = new DatabaseContext();

            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            // Завантажити факультети
            var faculties = await _db.Faculties.ToListAsync();
            foreach (var f in faculties)
                Faculties.Add(f);

            // Завантажити кімнати з вільними місцями
            await LoadAvailableRoomsAsync();
        }

        private async Task LoadAvailableRoomsAsync()
        {
            AvailableRooms.Clear();

            var rooms = await _db.Rooms
                .Include(r => r.Places)
                .Where(r => r.Places.Any(p => p.PlaceNumber == null)) // Є вільні місця
                .Select(r => new RoomDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Floor = r.Floor,
                    PlacesCount = r.PlacesCount,
                    OccupiedCount = r.Places.Count(p => p.PlaceNumber != null),
                    DisplayName = $"{r.Name} (поверх {r.Floor}) — {r.Places.Count(p => p.PlaceNumber != null)}/{r.PlacesCount}"
                })
                .ToListAsync();

            foreach (var room in rooms)
                AvailableRooms.Add(room);
        }

        partial void OnSelectedRoomChanged(RoomDto? value)
        {
            if (value != null)
                _ = LoadAvailablePlacesAsync(value.Id);
        }

        private async Task LoadAvailablePlacesAsync(int roomId)
        {
            AvailablePlaces.Clear();

            var places = await _db.RoomPlaces
                .Where(rp => rp.RoomId == roomId && rp.PlaceNumber == null)
                .Select(rp => new RoomPlaceDto
                {
                    Id = rp.Id,
                    RoomId = rp.RoomId,
                    DisplayNumber = $"Місце #{rp.Id}"
                })
                .ToListAsync();

            foreach (var place in places)
                AvailablePlaces.Add(place);
        }

        [RelayCommand]
        private async Task SettleAsync()
        {
            // Валідація
            if (string.IsNullOrWhiteSpace(FullName))
            {
                MessageBox.Show("Введіть ПІБ");
                return;
            }

            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
            {
                MessageBox.Show("Введіть коректний Email");
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Введіть пароль");
                return;
            }

            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                MessageBox.Show("Введіть телефон");
                return;
            }

            if (string.IsNullOrWhiteSpace(Gender))
            {
                MessageBox.Show("Оберіть стать");
                return;
            }

            if (SelectedFaculty == null)
            {
                MessageBox.Show("Оберіть факультет");
                return;
            }

            if (!Course.HasValue)
            {
                MessageBox.Show("Оберіть курс");
                return;
            }

            if (string.IsNullOrWhiteSpace(Group))
            {
                MessageBox.Show("Введіть групу");
                return;
            }

            if (string.IsNullOrWhiteSpace(FormOfEducation))
            {
                MessageBox.Show("Оберіть форму навчання");
                return;
            }

            if (SelectedRoom == null || SelectedPlace == null)
            {
                MessageBox.Show("Оберіть кімнату та місце");
                return;
            }

            // Перевірка чи Email вже існує
            var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == Email);
            if (existingUser != null)
            {
                MessageBox.Show("Користувач з таким Email вже існує");
                return;
            }

            // Визначити номер місця
            var occupiedCount = await _db.RoomPlaces
                .Where(rp => rp.RoomId == SelectedRoom.Id && rp.PlaceNumber != null)
                .CountAsync();

            int newRoomNumber = occupiedCount + 1;

            // Створити користувача
            var newUser = new UserInfo
            {
                FullName = FullName,
                Email = Email,
                PasswordHash = Password, // TODO: Хешування
                PhoneNumber = PhoneNumber,
                Gender = Gender,
                FacultyId = SelectedFaculty.Id,
                Course = Course.Value,
                Group = Group,
                FormOfEducation = FormOfEducation,
                IsAdmin = false,
                IsLivingInDormitory = true,
                RoomPlaceId = SelectedPlace.Id
            };

            _db.Users.Add(newUser);

            // Оновити RoomPlace
            var roomPlace = await _db.RoomPlaces.FindAsync(SelectedPlace.Id);
            if (roomPlace != null)
            {
                roomPlace.PlaceNumber = newRoomNumber;
            }

            await _db.SaveChangesAsync();

            MessageBox.Show($"Мешканця {FullName} успішно заселено!\nКімната: {SelectedRoom.Name}, місце №{newRoomNumber}");

            Back();
        }

        [RelayCommand]
        private void Back()
        {
            _mainWindow.NavigateTo(new AdminView(_mainWindow, _currentUser));
        }
    }
}