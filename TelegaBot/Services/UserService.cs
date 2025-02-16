using Microsoft.EntityFrameworkCore;
using TelegaBot.Context;
using TelegaBot.Models;
using TelegaBot.Services.Interfaces;
using Telegram.Bot;

namespace TelegaBot.Services
{
    public class UserService(TelegaBotContext context, ITelegramBotClient botClient) : IUserService
    {
        private readonly ITelegramBotClient _botClient = botClient;

        public async Task<int> GetUsersCountAsync() 
            => await context.Users.CountAsync();
    
        public async Task<User?> GetUser(long userId) 
            => await context.Users.FirstOrDefaultAsync(x => x.Id == userId);

        public async Task<bool> AddUserAsync(User? user)
        {
            var userCheck = await context.Users.FirstOrDefaultAsync(x => x.Id == user!.Id);
            if (userCheck != null && userCheck.TelegramChatId == user!.TelegramChatId)
                return false;
            
            await context.Users.AddAsync(user!);
            return true;
        }
    }
}