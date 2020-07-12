using System.Threading.Tasks;

namespace CommonLibrary.Interfaces
{
    public interface ICasheHandler
    {
        Task ClearDataAsync(string key);
        Task<bool> HasDataAsync(string key);
        Task<T> GetData<T>(string key);
        Task SetDataAsync(string key, object obj);
    }
}
