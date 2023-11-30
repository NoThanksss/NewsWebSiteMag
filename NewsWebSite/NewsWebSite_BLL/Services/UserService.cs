using AutoMapper;
using Microsoft.AspNet.Identity;
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
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IMemoryCache _cache;

        public UserService(IMapper mapper, IUserRepository userRepository, ILogger<UserService> logger, IMemoryCache cache)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _logger = logger;
            _cache = cache;

        }

        public async Task<IEnumerable<User>> GetAllUsersAsync() 
        {
            try 
            { 
                return _mapper.Map<List<User>>(await _userRepository.GetAllAsync().ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAllUsers method");
                throw new UserServiceException(ex.Message);
            }
        }

        public async Task<User> AddNewUserAsync(User user)
        {
            try 
            { 
                var userToAdd = _mapper.Map<UserDB>(user);
                var newUser = await _userRepository.AddEntityAsync(userToAdd);
                if (newUser != null)
                {
                    _cache.Set(newUser.Id, newUser,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
                return _mapper.Map<User>(newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in AddNewUser method");
                throw new UserServiceException(ex.Message);
            }
        }

        public async Task DeleteUserAsync(Guid id)
        {
            try 
            { 
                await _userRepository.DeleteEntityAsync(id);
                _cache.Remove(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in DeleteUser method");
                throw new UserServiceException(ex.Message);
            }
        }

        public async Task<User> UpdateUserAsync(User updatedUser)
        {
            try 
            { 
                var mappedUser = _mapper.Map<UserDB>(updatedUser);
                var result = await _userRepository.UpdateEntityAsync(mappedUser);
                if (result != null)
                {
                    _cache.Set(result.Id, result,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }

                return _mapper.Map<User>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in UpdateUser method");
                throw new UserServiceException(ex.Message);
            }
        }

        public async Task<User> GetUserByIdAsync(Guid id) 
        {
            try 
            {
                UserDB user = null;
                if (!_cache.TryGetValue(id, out user))
                {
                    user = await _userRepository.GetByIdAsync(id);
                    if (user == null)
                    {
                        _logger.LogError($"User with id {id} doesn't exist.");
                        throw new UserServiceException($"User with id {id} doesn't exist.");
                    }

                    _cache.Set(user.Id, user,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }

                return _mapper.Map<User>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetUserById method");
                throw new UserServiceException(ex.Message);
            }
        }
    }
}
