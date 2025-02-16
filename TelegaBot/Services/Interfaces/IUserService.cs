using TelegaBot.Models;

namespace TelegaBot.Services.Interfaces;

public interface IUserService
{
    Task AddMailAsync(UserMail? userMail);
}