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

        public IQueryable<AccountDB> GetAllAsync()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<AccountDB> UpdateEntityAsync(AccountDB entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<AccountDB> AddEntityAsync(AccountDB entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<AccountDB> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include(x => x.Subscribtions).Include(x => x.Subscibers).FirstAsync(x => x.Id == id.ToString());
        }

        public async Task DeleteEntityAsync(Guid id)
        {
            var entityToRemove = await _dbSet.FindAsync(id.ToString());
            _context.Entry<AccountDB>(entityToRemove).State = EntityState.Deleted;
            var user = await _context.UserDBs.FindAsync(entityToRemove.UserDBId);
            _context.Entry<UserDB>(user).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }
    }
}

