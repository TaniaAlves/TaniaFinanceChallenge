using Infrastructure.ErrorMessages;
using Infrastructure.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Repositorys.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        IQueryable<T> GetAll();
        Task<List<T>> GetAllWithPaging(Page page);
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        ErrorMessage<T> BadRequestMessage(T entity, string msg);
        ErrorMessage<T> NotFoundMessage(T entity);
    }
}
