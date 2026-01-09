using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DormitoryManagementSystem.Models;
using DormitoryManagementSystem.Services;
using DormitoryManagementSystem.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DormitoryManagementSystem.Views;


namespace DormitoryManagementSystem.ViewModels
{
    public partial class ResidentSearchViewModel : ObservableObject
    {
        private readonly MainWindow _mainWindow;
        private readonly UserInfo _currentUser;
        private readonly DatabaseContext _db;

        // Фільтри
        [ObservableProperty] private string _fullNameFilter = string.Empty;
        [ObservableProperty] private string _roomNameFilter = string.Empty;
        [ObservableProperty] private string _floorFilter = string.Empty;
        [ObservableProperty] private string _groupFilter = string.Empty;
        [ObservableProperty] private string? _courseFilter;
        [ObservableProperty] private Faculty? _selectedFaculty;

        // Результати
        [ObservableProperty] private UserInfo? _selectedResident;
        [ObservableProperty] private string _resultsTitle = "Результати пошуку (0)";
        [ObservableProperty] private bool _isResidentSelected;

        public ObservableCollection<Faculty> Faculties { get; } = new();
        public ObservableCollection<UserInfo> SearchResults { get; } = new();

        public ResidentSearchViewModel(MainWindow mainWindow, UserInfo currentUser)
        {
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _db = new DatabaseContext();

            _ = LoadFacultiesAsync();
        }

        private async Task LoadFacultiesAsync()
        {
            var faculties = await _db.Faculties.ToListAsync();
            Faculties.Add(new Faculty { Id = 0, Name = "Усі" }); // Для скидання фільтру
            foreach (var f in faculties)
                Faculties.Add(f);

            SelectedFaculty = Faculties[0]; // "Усі"
        }

        partial void OnSelectedResidentChanged(UserInfo? value)
        {
            IsResidentSelected = value != null;
        }

        [RelayCommand]
        private async Task SearchAsync()
        {
            SearchResults.Clear();

            var query = _db.Users
                .Include(u => u.Faculty)
                .Include(u => u.RoomPlace)
                    .ThenInclude(rp => rp.Room)
                .Where(u => u.IsLivingInDormitory && !u.IsAdmin);

            // Фільтр: ПІБ
            if (!string.IsNullOrWhiteSpace(FullNameFilter))
            {
                query = query.Where(u => u.FullName.Contains(FullNameFilter));
            }

            // Фільтр: Кімната
            if (!string.IsNullOrWhiteSpace(RoomNameFilter))
            {
                query = query.Where(u => u.RoomPlace != null && 
                                        u.RoomPlace.Room.Name.Contains(RoomNameFilter));
            }

            // Фільтр: Поверх
            if (!string.IsNullOrWhiteSpace(FloorFilter) && int.TryParse(FloorFilter, out int floor))
            {
                query = query.Where(u => u.RoomPlace != null && 
                                        u.RoomPlace.Room.Floor == floor);
            }

            // Фільтр: Факультет
            if (SelectedFaculty != null && SelectedFaculty.Id != 0)
            {
                query = query.Where(u => u.FacultyId == SelectedFaculty.Id);
            }

            // Фільтр: Курс
            if (!string.IsNullOrWhiteSpace(CourseFilter) && int.TryParse(CourseFilter, out int course))
            {
                query = query.Where(u => u.Course == course);
            }

            // Фільтр: Група
            if (!string.IsNullOrWhiteSpace(GroupFilter))
            {
                query = query.Where(u => u.Group != null && u.Group.Contains(GroupFilter));
            }

            var results = await query.ToListAsync();

            // Додати computed property для відображення кімнати
            foreach (var user in results)
            {
                SearchResults.Add(user);
            }

            ResultsTitle = $"Результати пошуку ({SearchResults.Count})";

            if (SearchResults.Count == 0)
            {
                MessageBox.Show("Нічого не знайдено за вказаними критеріями.");
            }
        }

        [RelayCommand]
        private void Clear()
        {
            FullNameFilter = string.Empty;
            RoomNameFilter = string.Empty;
            FloorFilter = string.Empty;
            GroupFilter = string.Empty;
            CourseFilter = null;
            SelectedFaculty = Faculties.FirstOrDefault();
            SearchResults.Clear();
            ResultsTitle = "Результати пошуку (0)";
        }

        [RelayCommand]
        private async Task EvictAsync()
        {
            if (SelectedResident == null)
                return;

            // 1. Відкрити діалог причини
            var dialog = new EvictionReasonDialog();
            if (dialog.ShowDialog() != true)
                return;

            var reason = dialog.Reason;

            // 2. Підтвердження
            var result = MessageBox.Show(
                $"Ви впевнені, що хочете виселити {SelectedResident.FullName}?\n\n" +
                $"Причина: {reason}\n\n" +
                $"Користувач отримає сповіщення, а через 7 днів обліковий запис буде заблоковано.",
                "Підтвердження виселення",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            // 3. Виселення
            var evictionService = new EvictionService(_db);
            var success = await evictionService.EvictResidentAsync(SelectedResident.Id, reason);

            if (success)
            {
                MessageBox.Show($"Мешканця {SelectedResident.FullName} успішно виселено.\n" +
                               $"Сповіщення надіслано.");
                
                await SearchAsync(); // Оновити список
            }
            else
            {
                MessageBox.Show("Помилка виселення.");
            }
        }

        [RelayCommand]
        private void ShowDetails()
        {
            if (SelectedResident == null)
                return;

            // TODO: Відкрити вікно з деталями
            MessageBox.Show($"Деталі для: {SelectedResident.FullName}\n" +
                           $"Email: {SelectedResident.Email}\n" +
                           $"Телефон: {SelectedResident.PhoneNumber}\n" +
                           $"Кімната: {SelectedResident.RoomPlace?.Room?.Name ?? "Не заселено"}\n" +
                           $"Факультет: {SelectedResident.Faculty?.Name}\n" +
                           $"Курс: {SelectedResident.Course}, Група: {SelectedResident.Group}");
        }

        [RelayCommand]
        private void ShowStatistics()
        {
            if (SelectedResident == null)
                return;

            // TODO: Відкрити вікно зі статистикою
            MessageBox.Show($"Статистика для: {SelectedResident.FullName}\n\n(Буде реалізовано пізніше)");
        }

        [RelayCommand]
        private void Back()
        {
            _mainWindow.NavigateTo(new AdminView(_mainWindow, _currentUser));
        }
    }
}