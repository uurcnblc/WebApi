using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApiAuth.DB;
using WebApiAuth.Repository.Interfaces;

namespace WebApiAuth.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        private readonly MarketContext _db;
        internal DbSet<T> dbSet;
        public BaseRepository(MarketContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
            _db.SaveChanges();
        }


        public async Task AddAsyc(T entity)
        {
            dbSet.Add(entity);
            _db.SaveChanges();

        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public async Task<T?> GetAsync(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, Func<IQueryable<T>, IOrderedEnumerable<T>>? orderby, string? includeProperties)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            if (orderby != null)
            {
                return orderby(query).ToList();
            }

            return query.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter, Func<IQueryable<T>, IOrderedEnumerable<T>>? orderby, string? includeProperties)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            if (orderby != null)
            {
                return orderby(query).ToList();
            }

            return query.ToList();
        }



        public T GetFirstOrDefault(Expression<Func<T, bool>>? filter, string? includeProperties)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? filter, string? includeProperties)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(int id)
        {
            T entity = dbSet.Find(id);
            Remove(entity);
        }
        public async Task RemoveAsync(int id)
        {
            T entity = dbSet.Find(id);
            Remove(entity);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
            _db.SaveChanges();

        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            _db.SaveChanges();

        }

        public void RemoveRange(IEnumerable<T> entityList)
        {
            dbSet.RemoveRange(entityList);
            _db.SaveChanges();

        }

        public async Task RemoveRangeAsync(IEnumerable<T> entityList)
        {
            dbSet.RemoveRange(entityList);
            _db.SaveChanges();

        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
            _db.SaveChanges();
        }

        public async Task UpdateAsync(T entity)
        {
            dbSet.Update(entity);
            _db.SaveChanges();
        }
    }
}
