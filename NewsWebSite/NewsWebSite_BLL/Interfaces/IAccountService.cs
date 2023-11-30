using NewsWebSite_BLL.Models;

namespace NewsWebSite_BLL.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task DeleteAccountAsync(Guid id);
        Task<Account> UpdateAccountAsync(Account updatedAccount);
        Task<Account> GetAccountByIdAsync(Guid id);
        Task Subscribe(Guid authorId, Guid subscriberId);

    }
}
