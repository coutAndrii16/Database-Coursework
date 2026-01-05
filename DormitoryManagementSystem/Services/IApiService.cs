using DormitoryManagementSystem.Models;
using System;
using System.Threading.Tasks;

namespace DormitoryManagementSystem.Services
{
    public interface IApiService
    {
        Task<UserInfo?> ValidateUserAsync(string email, string password);
        Task SyncDataToMyRemoteDbAsync(UserInfo userInfo);
        Task<bool> IsServerAvailableAsync();
        DateTime? LastSyncTime { get; }
    }
}
