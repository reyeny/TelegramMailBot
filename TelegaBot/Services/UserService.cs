using TelegaBot.Context;
using TelegaBot.Models;
using TelegaBot.Services.Interfaces;
using Telegram.Bot;

namespace TelegaBot.Services;

public class UserService(TelegaBotContext context, TelegramBotClient botClient, IBotService botService) : IUserService
{
    private readonly TelegaBotContext _context = context;
    private readonly TelegramBotClient _botClient = botClient;
    private readonly IBotService _botService = botService;

    public async Task AddMailAsync(UserMail? userMail)
    {
        await _context.UserMails.AddAsync(userMail);
        await _context.SaveChangesAsync();

        await _botService.SendMessageAsync(
            userMail!.User.TelegramUserId,
            "Почта успешно добавлена!");
    }
}