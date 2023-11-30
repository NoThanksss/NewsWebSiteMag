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

        public IEnumerable<ArticleDB> GetAll()
        {
            return _dbSet.ToList();
        }

        public IEnumerable<ArticleDB> GetAllWithThemeDbs()
        {
            return _dbSet.Include(x => x.ArticleThemeDBs).ToList();
        }

        public ArticleDB UpdateEntity(ArticleDB entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();

            return entity;
        }

        public ArticleDB UpdateThemes(ArticleDB entity, List<Guid> themesToAdd, List<Guid> themesToRemove)
        {

            var article = _context.ArticleDBs.Include(x => x.ArticleThemeDBs).FirstOrDefault(x => x.Id == entity.Id);
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
            _context.SaveChanges();

            return entity;
        }

        public ArticleDB AddEntity(ArticleDB entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public ArticleDB GetById(Guid id)
        {
            return _dbSet.Include(x => x.ArticleThemeDBs).First(x => x.Id == id);
        }

        public Task DeleteEntity(Guid id)
        {
            var entityToRemove = _dbSet.Find(id);
            _context.Entry<ArticleDB>(entityToRemove).State = EntityState.Deleted;
            var comments = _context.CommentDBs.Where(x => x.ArticleId == entityToRemove.Id);
            foreach (var comment in comments)
            {
                _context.Entry(comment).State = EntityState.Deleted;
            }

            return Task.FromResult(_context.SaveChanges());
        }
    }
}
