using NewsWebSite_BLL.Models;

namespace NewsWebSite_BLL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User AddNewUser(User user);
        void DeleteUser(Guid id);
        User UpdateUser(User updatedUser);
        User GetUserById(Guid id);
    }
}
