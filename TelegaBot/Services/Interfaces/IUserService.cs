using TelegaBot.Models;

namespace TelegaBot.Services.Interfaces;

public interface IUserService
{
    Task<int> GetUsersCountAsync();
    Task<User?> GetUser(long userId);
    Task<bool> AddUserAsync(User? user);
}