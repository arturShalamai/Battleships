using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.BLL.Repos
{
    public interface IRepository<T>
    {
        Task<List<T>> AllAsync();

        Task<List<T>> WhereAsync(Expression<Func<T, bool>> filter);

        Task<T> SingleAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);

        Task AddAsync(T entity);

        Task UpdateOneAsync(T entity);

        //Task UpdateManyAsync(T entity);

        Task DeleteOneAsync(T entity);

        Task DeleteManyAsync(Expression<Func<T, bool>> filter);


    }
}
