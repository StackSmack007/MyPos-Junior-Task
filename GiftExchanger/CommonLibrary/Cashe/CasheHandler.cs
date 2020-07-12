using System.Threading.Tasks;
using CommonLibrary.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CommonLibrary
{
    public class CasheHandler : ICasheHandler
    {
        private readonly IDistributedCache cashe;

        public CasheHandler(IDistributedCache cashe)
        {
            this.cashe = cashe;
        }

        public async Task<bool> HasDataAsync(string key) =>
            !string.IsNullOrEmpty(await this.cashe.GetStringAsync(key));

        public async Task<T> GetData<T>(string key) =>
            Deserialize<T>(await this.cashe.GetStringAsync(key));

        public async Task ClearDataAsync(string key) =>
            await this.cashe.SetStringAsync(key, string.Empty);

        public async Task SetDataAsync(string key, object obj) =>
            await this.cashe.SetStringAsync(key, Serialize(obj));

        private string Serialize(object obj) =>
           JsonConvert.SerializeObject(obj);

        private T Deserialize<T>(string str) =>
            JsonConvert.DeserializeObject<T>(str);
    }
}