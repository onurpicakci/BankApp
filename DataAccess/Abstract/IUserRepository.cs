using Domain.Entity;

namespace DataAccess.Abstract;

public interface IUserRepository
{
    Task<User> AddUserAsync(User user);
    
    Task<User> GetUserAsync(int id);
    
    Task<User> GetUserByUsernameAsync(string username);
}