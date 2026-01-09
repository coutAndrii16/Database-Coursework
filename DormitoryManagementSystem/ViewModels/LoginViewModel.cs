using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DormitoryManagementSystem.Models;
using DormitoryManagementSystem.Services;
using System.Linq;
using System.Windows;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DormitoryManagementSystem.Views;
using System.Diagnostics;

namespace DormitoryManagementSystem.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        // Автоматично генерує властивість з OnPropertyChanged

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password; //чому не хеш 13,05,2025 цікаво
        private readonly MainWindow _mainWindow;
        private readonly UserInfo user;

        public LoginViewModel(MainWindow mainWindow)
        {
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
        }

        // Команда викликає метод Login
        [RelayCommand(CanExecute = nameof(CanLogin))]
        private async Task LoginAsync()
        {
            if (!IsValidEmail(Email))
            {
                MessageBox.Show("Введіть правильну адресу пошти у форматі @student.ztu.edu.ua");
                return;
            }

            if (!IsValidPassword(Password))
            {
                MessageBox.Show("Пароль не може бути порожнім.");
                return;
            }

            try
            {
                // Спочатку блокуємо протерміновані акаунти
                var evictionService = new EvictionService(new DatabaseContext());
                await evictionService.BlockExpiredAccountsAsync();
                // Перевіряємо доступність серверів
                var (ztuAvailable, myAvailable) = await App.PingService.CheckServersAsync();

                if (!ztuAvailable && !myAvailable)
                {
                    MessageBox.Show("Обидва сервери недоступні. Перевірте інтернет-з'єднання.");
                    return;
                }

                UserInfo user = null;

                if (App.CurrentUser == null)
                {
                    // Перше підключення — логін через ZTU API
                    if (!ztuAvailable)
                    {
                        MessageBox.Show("Сервер ZTU недоступний. Неможливо увійти вперше.");
                        Debug.WriteLine("Сервер ZTU недоступний. Неможливо увійти вперше.");
                        return;
                    }
                    user = await App.ZtuApiService.ValidateUserAsync(Email, Password);

                    if (user == null)
                    {
                        MessageBox.Show("Невірний логін або пароль.");
                        return;
                    }
                    // Перевірка на блокування
                    if (user.IsDeleted == true)
                    {
                        MessageBox.Show("Ваш обліковий запис заблоковано через виселення.\nЗверніться до адміністрації гуртожитку.");
                        return;
                    }

                    // Перевірка на виселення (ще не заблоковано)
                    if (user.EvictionDate.HasValue && !user.IsLivingInDormitory)
                    {
                        var daysLeft = 7 - (DateTime.Now - user.EvictionDate.Value).Days;
                        if (daysLeft > 0)
                        {
                            MessageBox.Show($"Увага! Ви виселені з гуртожитку.\nОбліковий запис буде заблоковано через {daysLeft} днів.");
                        }
                    }

                    // Визначаємо, чи це адмін
                    user.IsAdmin = !Email.Contains("@student.ztu.edu.ua") && Email.Contains("@ztu.edu.ua");

                    // Синхронізація даних у мій сервер
                    if (myAvailable)
                    {
                        /*   await App.MyApiService.SyncDataToMyRemoteDbAsync(user);
                           MessageBox.Show($"Вхід успішний! Дані синхронізовано о {App.MyApiService.LastSyncTime?.ToShortTimeString()}");*/
                        Debug.WriteLine($"Типу синхронізовано");
                    }
                    else
                    {
                        MessageBox.Show("Вхід успішний! Але дані не синхронізовано — сервер недоступний.");
                    }

                    App.CurrentUser = user;
                }
                else
                {
                    // Повторний вхід — перевірка через My API
                    if (!myAvailable)
                    {
                        MessageBox.Show("Головний сервер недоступний. Неможливо увійти.");
                        return;
                    }

                    user = await App.MyApiService.ValidateUserAsync(Email, Password);

                    if (user == null)
                    {
                        MessageBox.Show("Невірний логін або пароль (локальний сервер).");
                        return;
                    }

                    App.CurrentUser = user;
                }

                if (App.CurrentUser.IsAdmin)
                {
                    _mainWindow.NavigateTo(new AdminView(_mainWindow, user)); // перекидає на адмінку
                }
                else if (App.CurrentUser.IsLivingInDormitory) 
                { 
                    _mainWindow.NavigateTo(new ResidentView(_mainWindow, user));//перекидає на студ
                }
                else
                {
                    // Виселений, але ще може переглядати
                    _mainWindow.NavigateTo(new ResidentView(_mainWindow, user));

                    Task.Delay(3000).ContinueWith(_ =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Application.Current.Shutdown();
                        });
                    });
                }
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Помилка мережі: не вдалось підключитись до сервера.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}");
            }
            finally
            {
                Password = null; // Завжди очищуємо пароль
            }
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@(student\.ztu\.edu\.ua|ztu\.edu\.ua)$";
            return Regex.IsMatch(email, pattern);
        }

        private bool IsValidPassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password);
        }
        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
        }

        partial void OnEmailChanged(string value)
        {
            LoginCommand.NotifyCanExecuteChanged();
        }
        partial void OnPasswordChanged(string value)
        {
            LoginCommand.NotifyCanExecuteChanged();
        }
    }
}
