using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Componet
{
    public class UnitOfWork<TContext> : IUnitOfWork
        where TContext : DbContext
    {
        private readonly TContext dbContext;

        public UnitOfWork(TContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        //        public bool IsInTransaction { get; }
        public event EventHandler<EventArgs> SaveCompleteHandler;

        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
        public void BeginTransaction()
        {
            dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            dbContext.Database.CommitTransaction();
        }

        public void RollBackTransaction()
        {
            dbContext.Database.RollbackTransaction();
        }

        private void SaveComplete()
        {
            SaveCompleteHandler?.Invoke(this, EventArgs.Empty);
        }
    }
}