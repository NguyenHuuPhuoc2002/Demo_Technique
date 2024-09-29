using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Unit_Of_Work.Data;
using Unit_Of_Work.Repository.Interface;

namespace Unit_Of_Work.Repository
{
    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private readonly QlSinhVienContext _dbContext;
        private readonly ISinhVienRepository _sinhVien;
        private IDbContextTransaction? _transaction = null;

        public UnitOfWorkRepository(QlSinhVienContext context)
        {
            _dbContext = context;
            _sinhVien = new SinhVienRepository(context, this);
        }
        public ISinhVienRepository Students => _sinhVien;

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public void BeginTransaction()
        {
            _transaction = _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction.Dispose();
            }
        }

        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
