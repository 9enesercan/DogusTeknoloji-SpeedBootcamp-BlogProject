using System.Linq.Expressions;

namespace BlogProject.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync();
        Task<bool> AnyAsync(Func<object, bool> value);



    }

}
