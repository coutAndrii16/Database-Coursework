using System;
using System.Threading.Tasks;

namespace DormitoryManagementSystem.Services
{
    public class PingService
    {
        private readonly IApiService _ztuApi;
        private readonly IApiService _myApi;

        public PingService(IApiService ztuApi, IApiService myApi)
        {
            _ztuApi = ztuApi;
            _myApi = myApi;
        }

        public async Task<(bool ztuAvailable, bool myAvailable)> CheckServersAsync()
        {/*
            var ztuTask = _ztuApi.IsServerAvailableAsync();
            var myTask = _myApi.IsServerAvailableAsync();
            await Task.WhenAll(ztuTask, myTask);
            return (ztuTask.Result, myTask.Result);*/
            bool isZtuServerAvailable = true; // було await IsServerAvailableAsync(ztuApiUrl)
            bool isOwnServerAvailable = true; // тимчасово імітуємо доступність
            await Task.CompletedTask;

            return (isZtuServerAvailable, isOwnServerAvailable);
        }

    }
}
