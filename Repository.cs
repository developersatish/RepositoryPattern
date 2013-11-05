using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Web;

namespace Portal.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private  MyContext _context;
        private  DbSet<T> _dbSet;

        public Repository(MyContext context)
        {
            this._context = context;
            this._dbSet = context.Set<T>();
        }

      
        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetById(Guid id)
        {
            T find = _dbSet.Find(id);
            return find;
        }

        public void InsertModel(T t)
        {
            _dbSet.Add(t);
        }

        public void UpdateModel(T t)
        {
            T attach = _dbSet.Attach(t);
            _context.Entry(attach).State = EntityState.Modified;
        }

        public void DeleteModel(Guid id)
        {
            T delete = _dbSet.Find(id);
            DeleteModel(delete);
        }

        public void DeleteModel(T t)
        {
            if (_context.Entry(t).State == EntityState.Detached)
            {
                _dbSet.Attach(t);
            }
            _dbSet.Remove(t);
        }

        public bool ExistModel(T t)
        {
            return _dbSet.Any();
        }

        public T One(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            var set = CreateIncludedSet(includes);

            return (predicate == null) ?
                   set.FirstOrDefault() :
                   set.FirstOrDefault(predicate);
        }

        public IQueryable<T> All(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            var set = CreateIncludedSet(includes);

            return (predicate == null) ? set : set.Where(predicate);
        }

        public bool Exists(Expression<Func<T, bool>> predicate = null)
        {
            var set = CreateSet();

            return (predicate == null) ? set.Any() : set.Any(predicate);
        }

        public int Count(Expression<Func<T, bool>> predicate = null)
        {
            var set = CreateSet();

            return (predicate == null) ?
                   set.Count() :
                   set.Count(predicate);
        }

        private IDbSet<T> CreateIncludedSet(
            IEnumerable<Expression<Func<T, object>>> includes)
        {
            var set = CreateSet();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    set.Include(include);
                }
            }

            return set;
        }


        private IDbSet<T> CreateSet()
        {
            return _context.Set<T>();
        }
    }
}