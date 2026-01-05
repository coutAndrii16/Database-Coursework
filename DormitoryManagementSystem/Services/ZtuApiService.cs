using DormitoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;

namespace DormitoryManagementSystem.Services
{
    public class ZtuApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ZtuApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://ztuapi.example.com/"),
                Timeout = TimeSpan.FromSeconds(5)
            };
        }
        public async Task<UserInfo> ValidateUserAsync(string email, string password)
        {
            using var context = new DatabaseContext();
          //  MessageBox.Show("ZtuApiService.ValidateUserAsync викликано");

            // Тут має бути хешування, але зараз просто порівняння як є
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password);
        }


        /*        public async Task<UserInfo?> ValidateUserAsync(string email, string password)
                {
                    var response = await _httpClient.PostAsJsonAsync("auth/login", new { email, password });
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadFromJsonAsync<UserInfo>();
                    }
                    return null;
                }*/

        public Task SyncDataToMyRemoteDbAsync(UserInfo userInfo)
        {
            // ZTU API не зберігає — заглушка
            return Task.CompletedTask;
        }

        public async Task<bool> IsServerAvailableAsync()
        {
            try
            {
                var responseTask = _httpClient.GetAsync("ping");
                var timeoutTask = Task.Delay(5000);  // Тайм-аут 5 секунд

                var completedTask = await Task.WhenAny(responseTask, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    Console.WriteLine("Тайм-аут: сервер не відповідає вчасно.");
                    return false;
                }

                var response = await responseTask;  // отримуємо результат запиту

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при підключенні до сервера: {ex.Message}");
                return false;
            }
        }


        public DateTime? LastSyncTime => null;
    }
}
