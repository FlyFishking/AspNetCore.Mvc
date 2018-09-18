using System.Threading.Tasks;

namespace WebApplication3.Componet
{
    public interface IUnitOfWork
    {
//        bool IsInTransaction { get; }
//        event EventHandler<EventArgs> SaveCompleteHandler;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void BeginTransaction();
        //        void BeginTransaction(IsolationLevel isolationLevel);
        void CommitTransaction();
        void RollBackTransaction();
    }
}
