using Microsoft.EntityFrameworkCore.Storage;
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
        Task AddAsync(T entity);

        void AddRange(ICollection<T> entities);
        Task AddRangeAsync(ICollection<T> entities);

        void Remove(T entity);
        void RemoveRange(ICollection<T> entities);

        int SaveChanges();
        Task<int> SaveChangesAsync();

        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
