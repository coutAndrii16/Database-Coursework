using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DormitoryManagementSystem.Models;
using DormitoryManagementSystem.ViewModels;
using DormitoryManagementSystem.Services;
using System.Windows;
using System.Diagnostics;
using DormitoryManagementSystem.Views;

namespace DormitoryManagementSystem.ViewModels
{ 
public partial class ShowerReservationViewModel : ObservableObject
{
    private readonly ShowerReservationService _reservationService;
    private readonly int _currentUserId;
    private readonly string _currentUserGender;
        private readonly MainWindow _mainWindow;
        private readonly UserInfo _user;

        public int CurrentUserId => _currentUserId;

        public ObservableCollection<ShowerSlotWithReservation> FlatSlots { get; } = new ObservableCollection<ShowerSlotWithReservation>();
        public ObservableCollection<SlotGroup> GroupedSlots { get; } = new();

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private string _statusMessage = string.Empty;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public ICommand LoadSlotsCommand { get; }
        public ICommand ReserveSlotCommand { get; }
        public ICommand CancelReservationCommand => ReserveSlotCommand;
        public ShowerReservationViewModel(ShowerReservationService reservationService, int userId, string userGender, MainWindow mainWindow, UserInfo user)
        {
            _reservationService = reservationService;
            _currentUserId = userId;
            _currentUserGender = userGender;
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            _user = user ?? throw new ArgumentNullException(nameof(user));

            LoadSlotsCommand = new AsyncRelayCommand(LoadSlotsAsync);
            ReserveSlotCommand = new AsyncRelayCommand<ShowerSlotWithReservation>(ReserveSlotAsync);

        }

        public async Task LoadSlotsAsync()
        {
            // Генеруємо слоти для сьогодні, якщо їх ще немає
            await _reservationService.EnsureSlotsForTodayAsync();
            IsBusy = true;
            try
            {
                FlatSlots.Clear();
                var allSlots = await _reservationService.GetAvailableSlotsAsync(_currentUserGender) ?? new List<ShowerSlot>();
                // 2) ВІДСІКАЄМО ті слоти, де slot.Gender != наша стать
                var sameGenderSlots = allSlots
                    .Where(slot =>
                        !string.IsNullOrEmpty(slot.Gender) &&
                        slot.Gender.Equals(_currentUserGender, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                var slots = sameGenderSlots
                    .Where(slot =>
                        slot.Reservations.Count == 0 ||
                        slot.Reservations.All(r =>
                            r.User != null &&
                            !string.IsNullOrEmpty(r.User.Gender) &&
                            r.User.Gender.Equals(_currentUserGender, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                foreach (var slot in slots)
                {
                    for (int i = 0; i < slot.MaxReservations; i++)
                    {
                        var reservation = slot.Reservations.ElementAtOrDefault(i);
                        FlatSlots.Add(new ShowerSlotWithReservation
                        {
                            Slot = slot,
                            Reservation = reservation
                        });
                    }
                }
                GroupedSlots.Clear();
                var now = DateTime.Now;
                var groups = FlatSlots
                    .Where(s => s.Slot.Date + s.Slot.EndTime > now) // ← показуємо тільки майбутні слоти
                    .GroupBy(s => $"{s.Slot.StartTime:hh\\:mm} – {s.Slot.EndTime:hh\\:mm}")
                    .OrderBy(g => g.Key);

                foreach (var group in groups)
                {
                    GroupedSlots.Add(new SlotGroup
                    {
                        TimeLabel = group.Key,
                        Slots = group.ToList()
                    });
                }

                StatusMessage = $"Слоти завантажено. Загальна кількість: {FlatSlots.Count}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Помилка: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }


        private async Task ReserveSlotAsync(ShowerSlotWithReservation slot)
        {
            if (slot == null || slot.Slot == null)
            {
                StatusMessage = "Слот не вибрано або відсутній.";
                return;
            }

            IsBusy = true;
            try
            {
                if (slot.Reservation?.UserId == _currentUserId)
                {
                    bool canceled = await _reservationService.CancelReservationAsync(_currentUserId, slot.Slot.Id);
                    StatusMessage = canceled ? "Запис скасовано." : "Не вдалося скасувати.";
                }
                else
                {
                    bool already = await _reservationService.HasUserReservedTodayAsync(_currentUserId);
                    if (already)
                    {
                        StatusMessage = "Ви вже маєте запис на сьогодні.";
                        return;
                    }

                    bool ok = await _reservationService.TryAddReservationAsync(_currentUserId, slot.Slot.Id);
                    StatusMessage = ok ? "Запис успішний!" : "Слот зайнятий.";
                }

                await LoadSlotsAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Помилка: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        private void Back()
        {
            _mainWindow.NavigateTo(new ResidentView(_mainWindow, _user));
        }
    }

    /// <summary>
    /// Допоміжний клас для групування слотів за часом
    /// </summary>
    public class SlotGroup
    {
        public string TimeLabel { get; set; } = string.Empty;
        public List<ShowerSlotWithReservation> Slots { get; set; } = new();
    }
}