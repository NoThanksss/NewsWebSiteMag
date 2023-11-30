using NewsWebSite_BLL.Models;

namespace NewsWebSite_BLL.Interfaces
{
    public interface IAccountService
    {
        IEnumerable<Account> GetAllAccounts();
        Account AddNewAccount(Account account);
        void DeleteAccount(Guid id);
        Account UpdateAccount(Account updatedAccount);
        Task<Account> GetAccountByIdAsync(Guid id);
        Account GetAccountById(Guid id);
        void Subscribe(Guid authorId, Guid subscriberId);

    }
}
