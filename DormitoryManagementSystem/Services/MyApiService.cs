using DormitoryManagementSystem.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DormitoryManagementSystem.Services
{
    public class MyApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        public DateTime? LastSyncTime { get; private set; }

        public MyApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://myapi.example.com/"),
                Timeout = TimeSpan.FromSeconds(5)
            };
        }

        public Task<UserInfo?> ValidateUserAsync(string email, string password)
        {
            // Ми не валідатор — повертаємо null
            return Task.FromResult<UserInfo?>(null);
        }

        public async Task SyncDataToMyRemoteDbAsync(UserInfo student)//немає
        {
            await _httpClient.PostAsJsonAsync("students/sync", student);
            LastSyncTime = DateTime.Now;
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

                var response = await responseTask;

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при підключенні до сервера: {ex.Message}");
                return false;
            }
        }

    }
}
