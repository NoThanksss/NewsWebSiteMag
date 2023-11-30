using Microsoft.EntityFrameworkCore;
using NewsWebSite_DAL.Interfaces;
using NewsWebSite_DAL.Models;

namespace NewsWebSite_DAL.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private NewsDbContext _context;
        private DbSet<ArticleDB> _dbSet;

        public ArticleRepository(NewsDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<ArticleDB>();
        }

        public IQueryable<ArticleDB> GetAllAsync()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<ArticleDB> GetAllWithThemeDbsAsync()
        {
            return _dbSet.Include(x => x.ArticleThemeDBs).AsQueryable();
        }

        public async Task<ArticleDB> UpdateEntityAsync(ArticleDB entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<ArticleDB> UpdateThemesAsync(ArticleDB entity, List<Guid> themesToAdd, List<Guid> themesToRemove)
        {

            var article = await GetByIdAsync(entity.Id);
            for (int i = 0; i < themesToRemove.Count; i++)
            {
                var theme = article.ArticleThemeDBs.FirstOrDefault(x => x.Id == themesToRemove[i]);
                if (theme != null)
                {
                    _context.ArticleDBs.Attach(article);
                    article.ArticleThemeDBs.Remove(theme);
                }
            }


                for (int i = 0; i < themesToAdd.Count; i++)
                {
                var theme = _context.ArticleThemeDBs.Include(x => x.ArticleDBs).FirstOrDefault(x => x.Id == themesToAdd[i]);
                if (theme != null)
                {
                    article.ArticleThemeDBs.Add(theme);
                    theme.ArticleDBs.Add(article);
                    _context.Entry(theme).State = EntityState.Modified;
                }
            }

            _context.Entry(article).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<ArticleDB> AddEntityAsync(ArticleDB entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<ArticleDB> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include(x => x.ArticleThemeDBs).FirstAsync(x => x.Id == id);
        }

        public async Task DeleteEntityAsync(Guid id)
        {
            var entityToRemove = await _dbSet.FindAsync(id);
            _context.Entry<ArticleDB>(entityToRemove).State = EntityState.Deleted;
            var comments = _context.CommentDBs.Where(x => x.ArticleId == entityToRemove.Id);
            foreach (var comment in comments)
            {
                _context.Entry(comment).State = EntityState.Deleted;
            }

            await _context.SaveChangesAsync();
        }
    }
}
