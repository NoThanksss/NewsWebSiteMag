

using AutoMapper;
using Microsoft.Extensions.Logging;
using NewsWebSite_BLL.Exceptions;
using NewsWebSite_BLL.Interfaces;
using NewsWebSite_BLL.Models;
using NewsWebSite_DAL.Interfaces;
using NewsWebSite_DAL.Models;
using NewsWebSite_DAL.Repositories;

namespace NewsWebSite_BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IMapper mapper, IUserRepository userRepository, ILogger<UserService> logger)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _logger = logger;

        }

        public IEnumerable<User> GetAllUsers() 
        {
            try 
            { 
                return _mapper.Map<List<User>>(_userRepository.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetAllUsers method");
                throw new UserServiceException(ex.Message);
            }
        }

        public User AddNewUser(User user)
        {
            try 
            { 
                var userToAdd = _mapper.Map<UserDB>(user);
                var newUser = _userRepository.AddEntity(userToAdd);

                return _mapper.Map<User>(newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in AddNewUser method");
                throw new UserServiceException(ex.Message);
            }
        }

        public void DeleteUser(Guid id)
        {
            try 
            { 
                _userRepository.DeleteEntity(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in DeleteUser method");
                throw new UserServiceException(ex.Message);
            }
        }

        public User UpdateUser(User updatedUser)
        {
            try 
            { 
                var mappedUser = _mapper.Map<UserDB>(updatedUser);

                return _mapper.Map<User>(_userRepository.UpdateEntity(mappedUser));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in UpdateUser method");
                throw new UserServiceException(ex.Message);
            }
        }

        public User GetUserById(Guid id) 
        {
            try 
            { 
                var user = _userRepository.GetById(id);
                if (user == null)
                {
                    _logger.LogError($"User with id {id} doesn't exist.");
                    throw new UserServiceException($"User with id {id} doesn't exist.");
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
