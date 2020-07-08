using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonLibrary.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        IQueryable<T> All { get; }
        void Add(T entity);
        void AddRange(ICollection<T> entities);
        void RemoveRange(ICollection<T> entities);
        void Remove(T entity);

        int SaveChanges();

        Task AddAsync(T entity);
        Task<int> SaveChangesAsync();

        Task AddRangeAsync(ICollection<T> entities);
    }
}
