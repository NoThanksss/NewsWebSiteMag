using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NewsWebSite_BLL.Exceptions;
using NewsWebSite_BLL.Interfaces;
using NewsWebSite_BLL.Models;
using NewsWebSite_DAL.Interfaces;
using NewsWebSite_DAL.Models;

namespace NewsWebSite_BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IMapper mapper, IAccountRepository accountRepository, ILogger<AccountService> logger, IMemoryCache cache)
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
            _logger = logger;
            _cache = cache;
        }    

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            try
            {
                var accounts = await _accountRepository.GetAllAsync().ToListAsync();
                return _mapper.Map<List<Account>>(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAllAccounts method");
                throw new AccountServiceException(ex.Message);
            }
        }

        public async Task DeleteAccountAsync(Guid id)
        {
            try 
            { 
                await _accountRepository.DeleteEntityAsync(id);
                _cache.Remove(id.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in DeleteAccount method");
                throw new AccountServiceException(ex.Message);
            }
        }

        public async Task<Account> UpdateAccountAsync(Account updatedAccount)
        {
            try 
            {
                var mappedAccount = _mapper.Map<AccountDB>(updatedAccount);
                var existingAccount = await _accountRepository.GetByIdAsync(Guid.Parse(mappedAccount.Id));
                existingAccount.FirstName = mappedAccount.FirstName;
                existingAccount.LastName = mappedAccount.LastName;
                existingAccount.UserDBId = mappedAccount.UserDBId;

                var result = await _accountRepository.UpdateEntityAsync(existingAccount);
                if (result != null)
                {
                    _cache.Set(result.Id, result,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }

                return _mapper.Map<Account>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in UpdateAccount method");
                throw new AccountServiceException(ex.Message);
            }
        }

        public async Task<Account> GetAccountByIdAsync(Guid id)
        {
            try
            {
                AccountDB account = null;
                if (!_cache.TryGetValue(id.ToString(), out account))
                {
                    account = await _accountRepository.GetByIdAsync(id);
                    if (account == null)
                    {
                        _logger.LogError($"Account with id {id} doesn't exist.");
                        throw new AccountServiceException($"Account with id {id} doesn't exist.");
                    }
                    _cache.Set(account.Id, account,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }

                return _mapper.Map<Account>(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAccountById method");
                throw new AccountServiceException(ex.Message);
            }
        }

        public async Task Subscribe(Guid authorId, Guid subscriberId)
        {
            try
            {
                var authorAccount = await _accountRepository.GetByIdAsync(authorId);
                var subscriberAccount = await _accountRepository.GetByIdAsync(subscriberId);

                if (authorAccount.Subscibers == null)
                {
                    authorAccount.Subscibers = new List<AccountDB>();
                }
                if (subscriberAccount.Subscribtions == null)
                {
                    subscriberAccount.Subscribtions = new List<AccountDB>();
                }

                authorAccount.Subscibers.Add(subscriberAccount);
                subscriberAccount.Subscribtions.Add(authorAccount);

                var updatedAuthor = await _accountRepository.UpdateEntityAsync(authorAccount);
                var updatedSub = await _accountRepository.UpdateEntityAsync(subscriberAccount);
                if (updatedAuthor != null && updatedSub != null)
                {
                    _cache.Set(updatedAuthor.Id, updatedAuthor,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                    _cache.Set(updatedSub.Id, updatedSub,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in Subscribe method");
                throw new AccountServiceException(ex.Message);
            }
        }
    }
}
