using DataAccess.Abstract;
using Domain.Entity;
using Moq;

namespace BankApp_Test.RepositoryTest;

public class UserRepositoryTest
{
    [Fact]
    public async Task AddUserAsync_ValidUser_ReturnsUser()
    {
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(repo => repo.AddUserAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) =>
            {
                u.Id = 1;
                return u;
            });

        var user = new User
        {
            Username = "test",
            Email = "test@gmail.com",
            Password = "123456",
            ConfirmPassword = "123456",
            Role = "Customer"
        };

        var result = await mockRepo.Object.AddUserAsync(user);
        var userResult = Assert.IsType<User>(result);
        Assert.Equal(1, userResult.Id);
    }

    [Fact]
    public async Task AddUserAsync_InvalidUser_ReturnsNull()
    {
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(repo => repo.AddUserAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) =>
            {
                u.Id = 1;
                return u;
            });

        var user = new User
        {
            Username = "user",
            Email = "test@gmail.com",
            Password = "1234565",
            ConfirmPassword = "123456",
            Role = "Customer"
        };

        var result = await mockRepo.Object.AddUserAsync(user);
        var userResult = Assert.IsType<User>(result);
        Assert.Equal(1, userResult.Id);

        Assert.NotEqual(user.Password, user.ConfirmPassword);
    }

    [Fact]
    public async Task GetUserAsync_ValidId_ReturnsUser()
    {
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(repo => repo.GetUserAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) =>
            {
                var user = new User
                {
                    Username = "user",
                };
                return user;
            });

        var result = await mockRepo.Object.GetUserAsync(6);
        var userResult = Assert.IsType<User>(result);
        Assert.Equal("user", userResult.Username);
    }
    
    [Fact]
    public async Task GetUserAsync_InvalidId_ReturnsNull()
    {
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(repo => repo.GetUserAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) =>
            {
                var user = new User
                {
                    Username = "use",
                };
                return user;
            });

        var result = await mockRepo.Object.GetUserAsync(6);
        var userResult = Assert.IsType<User>(result);
        Assert.NotEqual("user", userResult.Username);
    }
    
    [Fact]
    public async Task GetUserAsync_ValidUsername_ReturnsUser()
    {
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(repo => repo.GetUserByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync((string username) =>
            {
                var user = new User
                {
                    Username = "user",
                };
                return user;
            });

        var result = await mockRepo.Object.GetUserByUsernameAsync("user");
        var userResult = Assert.IsType<User>(result);
        Assert.Equal("user", userResult.Username);
    }
}