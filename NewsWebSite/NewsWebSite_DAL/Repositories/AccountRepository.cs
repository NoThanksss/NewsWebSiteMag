using Microsoft.EntityFrameworkCore;
using NewsWebSite_DAL.Interfaces;
using NewsWebSite_DAL.Models;

namespace NewsWebSite_DAL.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private NewsDbContext _context;
        private DbSet<AccountDB> _dbSet;

        public AccountRepository(NewsDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<AccountDB>();
        }

        public IEnumerable<AccountDB> GetAll()
        {
            return _dbSet;
        }

        public AccountDB UpdateEntity(AccountDB entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();

            return entity;
        }

        public AccountDB AddEntity(AccountDB entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public async Task<AccountDB> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include(x => x.Subscribtions).Include(x => x.Subscibers).FirstAsync(x => x.Id == id.ToString());
        }

        public Task DeleteEntity(Guid id)
        {
            var entityToRemove = _dbSet.Find(id.ToString());
            _context.Entry<AccountDB>(entityToRemove).State = EntityState.Deleted;
            var user = _context.UserDBs.First(x => x.Id == entityToRemove.UserDBId);
            _context.Entry<UserDB>(user).State = EntityState.Deleted;

            return Task.FromResult(_context.SaveChanges());
        }

        public AccountDB GetById(Guid id)
        {
            return _dbSet.Include(x => x.Subscribtions).Include(x => x.Subscibers).First(x => x.Id == id.ToString());
        }
    }
}

