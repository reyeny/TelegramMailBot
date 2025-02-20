using Microsoft.EntityFrameworkCore;
using TelegaBot.Context;
using TelegaBot.Models;
using TelegaBot.Services.Interfaces;

namespace TelegaBot.Services;

public class UserMailService(TelegaBotContext context) : IUserMailService
{
    public async Task<bool> AddUserMailAsync(long userChatId, string mail, string password)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.TelegramChatId == userChatId);
        if (user is null)
            throw new Exception($"User {userChatId} not found");

        var mailExists = await context.UserMails.AnyAsync(u => u.UserId == user.Id && u.Mail == mail);
        if (mailExists)
            throw new Exception("This email is already linked to your account.");

        var userMail = new UserMail
        {
            UserId = user.Id,
            Password = HashPassword(password), 
            Mail = mail
        };

        await context.UserMails.AddAsync(userMail); 
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> NeedToAddUserMailAsync(long userChatId, string mail)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.TelegramChatId == userChatId);
        if (user is null)
            return false;

        bool mailExists = await context.UserMails.AnyAsync(u => u.UserId == user.Id && u.Mail == mail);
        return !mailExists;
    }
    
    private static string HashPassword(string password) 
        => BCrypt.Net.BCrypt.HashPassword(password);
}