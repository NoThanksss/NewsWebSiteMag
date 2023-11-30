using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewsWebSite_BLL.Interfaces;
using NewsWebSite_BLL.Mapping;
using NewsWebSite_BLL.Services;
using NewsWebSite_DAL;
using NewsWebSite_DAL.Interfaces;
using NewsWebSite_DAL.Models;
using NewsWebSite_DAL.Repositories;

namespace NewsWebSite_BLL.Builders
{
    public static class ServiceProviderBuilder
    {
        public static IServiceCollection ConfigureDataBase(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<NewsDbContext>(options =>
            {
                options.UseNpgsql("Server=localhost;Database=NewsSystem;Port=5434;User Id=postgres;Password=postgres").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            serviceCollection.AddIdentityCore<AccountDB>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<NewsDbContext>();

            serviceCollection.AddScoped<IAccountRepository, AccountRepository>();
            serviceCollection.AddScoped<IArticleRepository, ArticleRepository>();
            serviceCollection.AddScoped<IArticleThemeRepository, ArticleThemeRepository>();
            serviceCollection.AddScoped<ICommentRepository, CommentRepository>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();

            return serviceCollection;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(typeof(UserMappingProfile).Assembly);
            serviceCollection.AddHttpClient();
            serviceCollection.AddScoped<IMailService, MailService>();
            serviceCollection.AddScoped<IAccountService, AccountService>();
            serviceCollection.AddScoped<IArticleService, ArticleService>();
            serviceCollection.AddScoped<IArticleThemeService, ArticleThemeService>();
            serviceCollection.AddScoped<ICommentService, CommentService>();
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<JwtHandler>();
            serviceCollection.AddScoped<IAuthService, AuthService>();

            return serviceCollection;
        }
        public static async Task createRolesandUsers(this IApplicationBuilder app, RoleManager<IdentityRole> roleManager,
            UserManager<AccountDB> userManager, IUserRepository userRepository)
        {
            IdentityResult adminRoleResult;

            var adminRoleExist = await roleManager.RoleExistsAsync("Admin");
            if (!adminRoleExist)
            {
                adminRoleResult = await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
            }

            IdentityResult roleResult;

            var roleExist = await roleManager.RoleExistsAsync("User");
            if (!roleExist)
            {
                roleResult = await roleManager.CreateAsync(new IdentityRole() { Name = "User" });
            }


            var adminProfile = new UserDB()
            {
                PhoneNumber = "admin_phone",
                BirthDate = DateTime.UtcNow,
                FullName = "Admin",
                Gender = "Admin"
            };

            var admin = new AccountDB()
            {
                FirstName = "Admin",
                LastName = "Admin",
                Email = "Admin@123",
                UserName = "Admin@123",
            };

            var _admin = await userManager.FindByEmailAsync(admin.Email);
            if (_admin == null)
            {
                var _adminProfile = userRepository.AddEntity(adminProfile);
                admin.UserDBId = _adminProfile.Id;

                var createPowerUser = await userManager.CreateAsync(admin, "Admin@123");
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");

                }
            }

        }
    }
}
