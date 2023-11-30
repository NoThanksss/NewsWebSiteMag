using NewsWebSite_DAL.Interfaces;
using NewsWebSite_DAL.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebSite_DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private NewsDbContext _context;
        private DbSet<UserDB> _dbSet;

        public UserRepository(NewsDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<UserDB>();
        }

        public IQueryable<UserDB> GetAllAsync()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<UserDB> UpdateEntityAsync(UserDB entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<UserDB> AddEntityAsync(UserDB entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<UserDB> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task DeleteEntityAsync(Guid id)
        {
            var entityToRemove = await _dbSet.FindAsync(id);
            _dbSet.Remove(entityToRemove);

            await _context.SaveChangesAsync();
        }
    }
}
