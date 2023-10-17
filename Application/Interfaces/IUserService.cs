using Domain.Entity;

namespace Application.Interfaces;

public interface IUserService
{
    Task<User> AddUserAsync(User user);
    
    Task<User> GetUserAsync(int id);
    
    Task<User> GetUserByUsernameAsync(string username);
}