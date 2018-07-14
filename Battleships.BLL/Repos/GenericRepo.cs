using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.BLL.Repos
{
    class GenericRepo<T> : IRepository<T>
    {
        public Task AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> AllAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteManyAsync(Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOneAsync(Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task<T> SingleAsync(Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOneAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> WhereAsync(Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}
