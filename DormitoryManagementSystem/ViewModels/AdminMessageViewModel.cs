using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DormitoryManagementSystem.Models;
using DormitoryManagementSystem.Services;
using DormitoryManagementSystem.Views;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace DormitoryManagementSystem.ViewModels
{
    public partial class AdminMessageViewModel : ObservableObject
    {
        private readonly MainWindow _mainWindow;
        private readonly UserInfo _currentUser;
        private readonly AdminMessageService _service;

        // Поточне оголошення
        [ObservableProperty] private AdminMessage? _currentMessage;
        [ObservableProperty] private string _currentMessageTitle = string.Empty;
        [ObservableProperty] private string _currentMessageContent = string.Empty;
        [ObservableProperty] private Visibility _currentMessageVisibility = Visibility.Collapsed;

        // Нове оголошення
        [ObservableProperty] private string _newTitle = string.Empty;
        [ObservableProperty] private string _newContent = string.Empty;

        public AdminMessageViewModel(MainWindow mainWindow, UserInfo currentUser)
        {
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _service = new AdminMessageService(new DatabaseContext());

            _ = LoadCurrentMessageAsync();
        }

        private async Task LoadCurrentMessageAsync()
        {
            CurrentMessage = await _service.GetCurrentActiveAsync();

            if (CurrentMessage != null)
            {
                CurrentMessageTitle = CurrentMessage.Title;
                CurrentMessageContent = CurrentMessage.Content;
                CurrentMessageVisibility = Visibility.Visible;
            }
            else
            {
                CurrentMessageVisibility = Visibility.Collapsed;
            }
        }

        [RelayCommand]
        private async Task PublishAsync()
        {
            // Валідація
            if (string.IsNullOrWhiteSpace(NewTitle))
            {
                MessageBox.Show("Введіть заголовок оголошення");
                return;
            }

            if (string.IsNullOrWhiteSpace(NewContent))
            {
                MessageBox.Show("Введіть текст оголошення");
                return;
            }

            // Підтвердження
            var result = MessageBox.Show(
                $"Опублікувати нове оголошення?\n\n" +
                $"Заголовок: {NewTitle}\n\n" +
                $"Текст: {NewContent}\n\n" +
                $"Оголошення одразу побачать всі резиденти.",
                "Підтвердження публікації",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            // Створити оголошення
            await _service.CreateMessageAsync(NewTitle, NewContent);

            MessageBox.Show("✅ Оголошення успішно опубліковано!\n\nРезиденти побачать його при наступному вході.");

            // Очистити форму
            ClearForm();

            // Оновити поточне оголошення
            await LoadCurrentMessageAsync();
        }

        [RelayCommand]
        private async Task DeactivateAsync()
        {
            if (CurrentMessage == null)
                return;

            var result = MessageBox.Show(
                $"Деактивувати оголошення?\n\n" +
                $"Заголовок: {CurrentMessage.Title}\n\n" +
                $"Резиденти більше не побачать це оголошення.",
                "Підтвердження деактивації",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            await _service.DeactivateMessageAsync(CurrentMessage.Id);

            MessageBox.Show("Оголошення деактивовано.");

            // Оновити відображення
            await LoadCurrentMessageAsync();
        }

        [RelayCommand]
        private void ClearForm()
        {
            NewTitle = string.Empty;
            NewContent = string.Empty;
        }

        [RelayCommand]
        private void Back()
        {
            _mainWindow.NavigateTo(new AdminView(_mainWindow, _currentUser));
        }
    }
}