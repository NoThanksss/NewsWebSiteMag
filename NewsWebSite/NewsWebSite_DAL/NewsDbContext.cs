using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsWebSite_DAL.Models;

namespace NewsWebSite_DAL
{
    public class NewsDbContext : IdentityDbContext<AccountDB>
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> options)
             : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<ArticleDB> ArticleDBs { get; set; }
        public DbSet<ArticleThemeDB> ArticleThemeDBs { get; set; }
        public DbSet<CommentDB> CommentDBs { get; set; }
        public DbSet<UserDB> UserDBs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleDB>()
                    .HasMany(c => c.ArticleThemeDBs)
                    .WithMany(s => s.ArticleDBs);

            modelBuilder.Entity<AccountDB>()
                .HasMany(s => s.Subscibers)
                .WithMany(c => c.Subscribtions)
                .UsingEntity(j => j.ToTable("SubscriptionMappingTable"));

            modelBuilder.Entity<ArticleDB>()
                .HasIndex(x => x.AccountId);

            modelBuilder.Entity<CommentDB>()
                .HasIndex(x => x.ArticleId);


            base.OnModelCreating(modelBuilder);
        }
    }
}
