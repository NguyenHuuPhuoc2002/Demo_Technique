using Unit_Of_Work.Repository.Interface;

namespace Unit_Of_Work.Repository
{
    public interface IUnitOfWorkRepository
    {
        ISinhVienRepository Students { get; }
        void CommitTransaction();
        void RollbackTransaction();
        void BeginTransaction();
        Task<int> SaveChangesAsync();
    }
}