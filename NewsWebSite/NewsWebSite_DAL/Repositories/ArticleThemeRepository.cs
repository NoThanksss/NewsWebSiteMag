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
    public class ArticleThemeRepository : IArticleThemeRepository
    {
        private NewsDbContext _context;
        private DbSet<ArticleThemeDB> _dbSet;

        public ArticleThemeRepository(NewsDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<ArticleThemeDB>();
        }

        public IQueryable<ArticleThemeDB> GetAllAsync()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<IEnumerable<ArticleThemeDB>> GetAllWithArticlesAsync()
        {
            return await _dbSet.Include(x => x.ArticleDBs).ToListAsync();
        }

        public async Task<ArticleThemeDB> UpdateEntityAsync(ArticleThemeDB entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<ArticleThemeDB> AddEntityAsync(ArticleThemeDB entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<ArticleThemeDB> GetByIdAsync(Guid id)
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
