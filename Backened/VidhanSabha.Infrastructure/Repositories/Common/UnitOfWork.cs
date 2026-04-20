using Microsoft.EntityFrameworkCore.Storage;
using VidhanSabha.Application.Common.UnitOfWork;
using VidhanSabha.Infrastructure.Persistence;

namespace VidhanSabha.Infrastructure.Repositories.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _db;
        private IDbContextTransaction? _transaction;
        private int _transactionDepth = 0;              // ✅ Added

        public UnitOfWork(DatabaseContext db)
        {
            _db = db;
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _db.Database.BeginTransactionAsync();
            }
            _transactionDepth++;                        // ✅ Always increment
        }

        public async Task CommitAsync()
        {
            _transactionDepth--;                        // ✅ Decrement first

            if (_transactionDepth == 0 && _transaction != null)
            {
                await _transaction.CommitAsync();       // ✅ Only outermost commits
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
                _transactionDepth = 0;                  // ✅ Reset on rollback
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}