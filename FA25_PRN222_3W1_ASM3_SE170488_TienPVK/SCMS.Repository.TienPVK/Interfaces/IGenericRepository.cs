using System.Linq.Expressions;

namespace SCMS.Repository.TienPVK.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(T entity);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>? include = null);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>? include = null);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>? include = null);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<(IEnumerable<T> items, int totalCount, int totalPages, bool hasPrevious, bool hasNext)> GetPagedAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, object>>? include = null,
            Expression<Func<T, object>>? orderBy = null,
            bool orderByDescending = false);
    }
}
