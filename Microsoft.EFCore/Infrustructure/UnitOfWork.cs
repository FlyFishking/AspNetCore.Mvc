using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Microsoft.EFCore.Infrustructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly DbContext dbContext;

        public UnitOfWork(DbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public event EventHandler<EventArgs> SaveCompleteHandler;

        public bool IsTransactionBegin => dbContext?.Database.CurrentTransaction != null;

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
            //            var tran = dbContext.Database.CurrentTransaction.GetDbTransaction();
        }

        public void CommitTransaction()
        {
            if (IsTransactionBegin)
            {
                SaveChanges();
                dbContext.Database.CommitTransaction();
            }
        }

        public void RollBackTransaction()
        {
            if (IsTransactionBegin)
            {
                dbContext.Database.RollbackTransaction();
            }
        }

        private void SaveComplete()
        {
            SaveCompleteHandler?.Invoke(this, EventArgs.Empty);
        }
    }
}