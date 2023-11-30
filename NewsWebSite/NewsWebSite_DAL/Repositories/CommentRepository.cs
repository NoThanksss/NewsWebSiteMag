using Microsoft.EntityFrameworkCore;
using NewsWebSite_DAL.Interfaces;
using NewsWebSite_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebSite_DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private NewsDbContext _context;
        private DbSet<CommentDB> _dbSet;

        public CommentRepository(NewsDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<CommentDB>();
        }

        public IQueryable<CommentDB> GetAllAsync()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<CommentDB> UpdateEntityAsync(CommentDB entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<CommentDB> AddEntityAsync(CommentDB entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<CommentDB> GetByIdAsync(Guid id)
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
