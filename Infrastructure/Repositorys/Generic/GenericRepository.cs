using Infrastructure.BaseClass;
using Infrastructure.Context;
using Infrastructure.ErrorMessages;
using Infrastructure.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace Infrastructure.Repositorys.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : EntityBase
    {
        private readonly DataContext _context;
        public List<string> problems = new List<string>();
        protected readonly DbSet<T> _dbSet;
        protected Func<IQueryable<T>, IIncludableQueryable<T, object>> _include;

        public GenericRepository(DataContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual void SetInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> include)
        {
            _include = include;
        }

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await SaveAllAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await SaveAllAsync();
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await SaveAllAsync();
        }


        private async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<T>> GetAllWithPaging(Page page)
        {
            var query = _dbSet
                .Skip((page.Number - 1) * page.Size)
                .Take(page.Size)
                .AsNoTracking();

            if (_include != null)
                query = _include(query);

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var query = _dbSet
                .Where(e => e.Id == id);

            if (_include != null)
                query = _include(query);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var query = _dbSet
                .Where(predicate);

            if (_include != null)
                query = _include(query);

            return await query.FirstOrDefaultAsync();
        }

        public ErrorMessage<T> BadRequestMessage(T entity, string msg)
        {
            problems.Add(msg);
            var result = new ErrorMessage<T>(HttpStatusCode.BadRequest.GetHashCode().ToString(), problems, entity);
            return result;
        }

        public ErrorMessage<T> NotFoundMessage(T entity)
        {
            problems.Add("This database doesn't contain that requested data.");
            var result = new ErrorMessage<T>(HttpStatusCode.NotFound.GetHashCode().ToString(), problems, entity);
            return result;
        }
    }
}
