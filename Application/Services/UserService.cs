using Application.Interfaces;
using DataAccess.Abstract;
using Domain.Entity;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public Task<User> AddUserAsync(User user)
    {
        return _userRepository.AddUserAsync(user);
    }

    public Task<User> GetUserAsync(int id)
    {
        return _userRepository.GetUserAsync(id);
    }

    public Task<User> GetUserByUsernameAsync(string username)
    {
        return _userRepository.GetUserByUsernameAsync(username);
    }
}