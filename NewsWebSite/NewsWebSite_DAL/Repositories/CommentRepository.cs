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

        public IEnumerable<CommentDB> GetAll()
        {
            return _dbSet.ToList();
        }

        public CommentDB UpdateEntity(CommentDB entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();

            return entity;
        }

        public CommentDB AddEntity(CommentDB entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public CommentDB GetById(Guid id)
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
