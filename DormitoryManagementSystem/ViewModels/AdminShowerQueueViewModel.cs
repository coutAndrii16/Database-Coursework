using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DormitoryManagementSystem.Models;
using DormitoryManagementSystem.Services;
using DormitoryManagementSystem.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DormitoryManagementSystem.ViewModels
{
    public partial class AdminShowerQueueViewModel : ObservableObject
    {
        private readonly MainWindow _mainWindow;
        private readonly UserInfo _currentUser;
        private readonly DatabaseContext _db;
        private readonly ShowerReservationService _showerService;

        [ObservableProperty] private DateTime? _selectedDate = DateTime.Today;
        [ObservableProperty] private TimeSlotDto? _selectedTimeSlot;
        [ObservableProperty] private string _maleQueueCount = "(0/4)";
        [ObservableProperty] private string _femaleQueueCount = "(0/4)";

        public ObservableCollection<TimeSlotDto> TimeSlots { get; } = new();
        public ObservableCollection<QueueItemDto> MaleQueue { get; } = new();
        public ObservableCollection<QueueItemDto> FemaleQueue { get; } = new();

        public AdminShowerQueueViewModel(MainWindow mainWindow, UserInfo currentUser)
        {
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _db = new DatabaseContext();
            _showerService = new ShowerReservationService(_db);

            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await LoadTimeSlotsAsync();
            
            // Вибрати перший слот за замовчуванням
            SelectedTimeSlot = TimeSlots.FirstOrDefault();
            
            if (SelectedTimeSlot != null)
                await LoadQueuesAsync();
        }

        private async Task LoadTimeSlotsAsync()
        {
            // Завантажити всі можливі часові слоти
            var slots = await _db.ShowerSlots
                .Where(s => s.Date == DateTime.Today)
                .Select(s => new { s.StartTime, s.EndTime })
                .Distinct()
                .OrderBy(s => s.StartTime)
                .ToListAsync();

            foreach (var slot in slots)
            {
                TimeSlots.Add(new TimeSlotDto
                {
                    StartTime = slot.StartTime,
                    EndTime = slot.EndTime,
                    TimeRange = $"{slot.StartTime:hh\\:mm} – {slot.EndTime:hh\\:mm}"
                });
            }
        }

        [RelayCommand]
        private async Task LoadQueuesAsync()
        {
            if (SelectedDate == null || SelectedTimeSlot == null)
                return;

            MaleQueue.Clear();
            FemaleQueue.Clear();

            // Завантажити чоловічу чергу
            var maleSlot = await _db.ShowerSlots
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.User)
                        .ThenInclude(u => u.RoomPlace)
                            .ThenInclude(rp => rp.Room)
                .FirstOrDefaultAsync(s => 
                    s.Date == SelectedDate.Value.Date &&
                    s.StartTime == SelectedTimeSlot.StartTime &&
                    s.EndTime == SelectedTimeSlot.EndTime &&
                    s.Gender == "Чоловіча");

            if (maleSlot != null)
            {
                int queueNum = 1;
                foreach (var reservation in maleSlot.Reservations.OrderBy(r => r.CreatedAt))
                {
                    MaleQueue.Add(new QueueItemDto
                    {
                        QueueNumber = queueNum++,
                        UserName = reservation.User?.FullName ?? "Невідомо",
                        RoomInfo = reservation.User?.RoomPlace?.Room != null
                            ? $"{reservation.User.RoomPlace.Room.Name} ({reservation.User.RoomPlace.PlaceNumber})"
                            : "—",
                        ReservationTime = reservation.CreatedAt.ToString("dd.MM HH:mm")
                    });
                }
                MaleQueueCount = $"({MaleQueue.Count}/{maleSlot.MaxReservations})";
            }
            else
            {
                MaleQueueCount = "(0/4)";
            }

            // Завантажити жіночу чергу
            var femaleSlot = await _db.ShowerSlots
                .Include(s => s.Reservations)
                    .ThenInclude(r => r.User)
                        .ThenInclude(u => u.RoomPlace)
                            .ThenInclude(rp => rp.Room)
                .FirstOrDefaultAsync(s => 
                    s.Date == SelectedDate.Value.Date &&
                    s.StartTime == SelectedTimeSlot.StartTime &&
                    s.EndTime == SelectedTimeSlot.EndTime &&
                    s.Gender == "Жіноча");

            if (femaleSlot != null)
            {
                int queueNum = 1;
                foreach (var reservation in femaleSlot.Reservations.OrderBy(r => r.CreatedAt))
                {
                    FemaleQueue.Add(new QueueItemDto
                    {
                        QueueNumber = queueNum++,
                        UserName = reservation.User?.FullName ?? "Невідомо",
                        RoomInfo = reservation.User?.RoomPlace?.Room != null
                            ? $"{reservation.User.RoomPlace.Room.Name} ({reservation.User.RoomPlace.PlaceNumber})"
                            : "—",
                        ReservationTime = reservation.CreatedAt.ToString("dd.MM HH:mm")
                    });
                }
                FemaleQueueCount = $"({FemaleQueue.Count}/{femaleSlot.MaxReservations})";
            }
            else
            {
                FemaleQueueCount = "(0/4)";
            }
        }

        [RelayCommand]
        private void Back()
        {
            _mainWindow.NavigateTo(new AdminView(_mainWindow, _currentUser));
        }
    }

    // DTO класи
    public class TimeSlotDto
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string TimeRange { get; set; } = string.Empty;
    }

    public class QueueItemDto
    {
        public int QueueNumber { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string RoomInfo { get; set; } = string.Empty;
        public string ReservationTime { get; set; } = string.Empty;
    }
}