using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace WebApiAuth.Repository.Interfaces
{
    public interface IBaseRepository<T> where T : class, new()
    {
        T Get(int id);
        Task<T> GetAsync(int id);
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>>? filter,
            Func<IQueryable<T>, IOrderedEnumerable<T>>? orderby,
            string? includeProperties
            );
        Task<IEnumerable<T>> GetAllAsync(
    Expression<Func<T, bool>>? filter,
    Func<IQueryable<T>, IOrderedEnumerable<T>>? orderby,
    string? includeProperties
    );
        Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>>? filter,
            string? includeProperties
            );
        T GetFirstOrDefault(
    Expression<Func<T, bool>>? filter,
    string? includeProperties
    );
        void Add(T entity);
        Task AddAsyc(T entity);
        void Remove(int id);
        Task RemoveAsync(int id);
        void Remove(T entity);
        Task RemoveAsync(T entity);
        void RemoveRange(IEnumerable<T> entityList);
        Task RemoveRangeAsync(IEnumerable<T> entityList);
        void Update(T entity);
        Task UpdateAsync(T entity);
    }
}
