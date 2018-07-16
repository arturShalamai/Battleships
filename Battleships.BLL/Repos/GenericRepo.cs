﻿using Battleships.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.BLL.Repos
{
    class GenericRepo<T> : IRepository<T> where T : class
    {
        private readonly BattleshipsContext _context;

        public GenericRepo(BattleshipsContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task<List<T>> AllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task DeleteManyAsync(Expression<Func<T, bool>> filter)
        {
            
        }

        public async Task DeleteOneAsync(T entity)
        {
            if (entity != null) { _context.Set<T>().Remove(entity); }
        }

        public async Task<T> SingleAsync(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(filter);
        }

        public Task UpdateOneAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public async Task<List<T>> WhereAsync(Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().Where(filter).ToListAsync();
        }
    }
}