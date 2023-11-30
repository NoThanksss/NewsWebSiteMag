using NewsWebSite_BLL.Models;

namespace NewsWebSite_BLL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> AddNewUserAsync(User user);
        Task DeleteUserAsync(Guid id);
        Task<User> UpdateUserAsync(User updatedUser);
        Task<User> GetUserByIdAsync(Guid id);
    }
}
