using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsWebSite_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebSite_DAL
{
    public class NewsDbContext : IdentityDbContext<AccountDB>
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> options)
             : base(options)
        {
            Database.EnsureCreated();
        }

        //public DbContext(DbContextOptions<DbContext> options) : base(options) { }
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

            base.OnModelCreating(modelBuilder);
        }
    }
}
