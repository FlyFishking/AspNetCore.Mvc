using System.Threading.Tasks;

namespace Microsoft.EFCore.Infrustructure
{
    public interface IUnitOfWork
    {
        bool IsTransactionBegin { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollBackTransaction();
    }
}
