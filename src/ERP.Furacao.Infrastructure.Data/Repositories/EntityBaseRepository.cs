using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ERP.Furacao.Application.Repositories;
using ERP.Furacao.Infrastructure.Data.Contexts;

namespace ERP.Furacao.Infrastructure.Data.Repositories
{
    public class EntityBaseRepository<T> : IEntityRepository<T> where T : class
    {
        private readonly ApplicationContext _context;

        public EntityBaseRepository(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).ToList();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public T GetById(int id)
        {
            var entity = _context.Set<T>().Find(id);
            _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Add(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public void AddRange(IEnumerable<T> entities) { _context.AddRange(entities); _context.SaveChanges(); }

        public void Update(T entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }

        public void UpdateRange(IEnumerable<T> entities) { _context.UpdateRange(entities); _context.SaveChanges(); }

        public void Remove(T entity) { _context.Remove(entity); _context.SaveChanges(); }

        public void Remove(int id)
        {
            var entity = _context.Set<T>().Find(id);
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<T> entities) { _context.RemoveRange(entities); _context.SaveChanges(); }
    }
}
