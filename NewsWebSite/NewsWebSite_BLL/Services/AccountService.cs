using AutoMapper;
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
        private readonly ILogger<AccountService> _logger;

        public AccountService(IMapper mapper, IAccountRepository accountRepository, ILogger<AccountService> logger)
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
            _logger = logger;
        }    

        public IEnumerable<Account> GetAllAccounts()
        {
            try
            { 
                return _mapper.Map<List<Account>>(_accountRepository.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAllAccounts method");
                throw new AccountServiceException(ex.Message);
            }
        }

        public Account AddNewAccount(Account account)
        {
            try
            { 
                var accountToAdd = _mapper.Map<AccountDB>(account);
                var newAccount = _accountRepository.AddEntity(accountToAdd);

                return _mapper.Map<Account>(newAccount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in AddNewAccount method");
                throw new AccountServiceException(ex.Message);
            }
        }

        public void DeleteAccount(Guid id)
        {
            try 
            { 
                _accountRepository.DeleteEntity(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in DeleteAccount method");
                throw new AccountServiceException(ex.Message);
            }
        }

        public Account UpdateAccount(Account updatedAccount)
        {
            try 
            {
                var mappedAccount = _mapper.Map<AccountDB>(updatedAccount);
                var existingAccount = _accountRepository.GetById(Guid.Parse(mappedAccount.Id));
                existingAccount.FirstName = mappedAccount.FirstName;
                existingAccount.LastName = mappedAccount.LastName;
                existingAccount.UserDBId = mappedAccount.UserDBId;

                return _mapper.Map<Account>(_accountRepository.UpdateEntity(existingAccount));
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
                var account = await _accountRepository.GetByIdAsync(id);

                return _mapper.Map<Account>(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAccountByIdAsync method");
                throw new AccountServiceException(ex.Message);
            }
        }
        public Account GetAccountById(Guid id)
        {
            try
            { 
                var account = _accountRepository.GetById(id);
                if (account == null)
                {
                    _logger.LogError($"Account with id {id} doesn't exist.");
                    throw new AccountServiceException($"Account with id {id} doesn't exist.");
                }

                return _mapper.Map<Account>(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAccountById method");
                throw new AccountServiceException(ex.Message);
            }
        }

        public void Subscribe(Guid authorId, Guid subscriberId)
        {
            try
            {
                var authorAccount = _accountRepository.GetById(authorId);
                var subscriberAccount = _accountRepository.GetById(subscriberId);

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

                var updatedAuthor = _accountRepository.UpdateEntity(authorAccount);
                var updatedSub = _accountRepository.UpdateEntity(subscriberAccount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in Subscribe method");
                throw new AccountServiceException(ex.Message);
            }
        }
    }
}
