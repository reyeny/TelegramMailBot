using TelegaBot.Models;

namespace TelegaBot.Services.Interfaces;

public interface IUserMailService
{
    Task<bool> AddUserMailAsync(long userChatId, string mail, string password);
    Task<bool> NeedToAddUserMailAsync(long userChatId, string mail);
    
}