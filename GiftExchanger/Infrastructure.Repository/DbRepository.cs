using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Infrasturcture.Data;
using CommonLibrary.Interfaces;

namespace Infrastructure.Repository
{
    public class DbRepository<T> : IRepository<T>
        where T : class
    {
        private DbSet<T> dbSet;
        private DbContext context;
        public DbRepository(GiftExchangerContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public IQueryable<T> All => this.dbSet;

        public void Add(T entity) =>
            this.dbSet.Add(entity);

        public async Task AddAsync(T entity) =>
            await this.dbSet.AddAsync(entity);

        public void AddRange(ICollection<T> entities) =>
            this.dbSet.AddRange(entities);

        public async Task AddRangeAsync(ICollection<T> entities) =>
            await this.dbSet.AddRangeAsync(entities);

        public void Remove(T entity) =>
            this.dbSet.Remove(entity);

        public void RemoveRange(ICollection<T> entities) =>
            this.dbSet.RemoveRange(entities);

        public int SaveChanges() =>
            this.context.SaveChanges();

        public async Task<int> SaveChangesAsync() =>
            await this.context.SaveChangesAsync();
    }
}