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

        public IEnumerable<ArticleThemeDB> GetAll()
        {
            return _dbSet.ToList();
        }

        public IEnumerable<ArticleThemeDB> GetAllWithArticles()
        {
            return _dbSet.Include(x => x.ArticleDBs).ToList();
        }

        public ArticleThemeDB UpdateEntity(ArticleThemeDB entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();

            return entity;
        }

        public ArticleThemeDB AddEntity(ArticleThemeDB entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public ArticleThemeDB GetById(Guid id)
        {
            return _dbSet.Find(id);
        }

        public Task DeleteEntity(Guid id)
        {
            var entityToRemove = _dbSet.Find(id);
            _dbSet.Remove(entityToRemove);

            return Task.FromResult(_context.SaveChanges());
        }
    }
}
