using TelegaBot.Context;
using TelegaBot.Services.Interfaces;
using Telegram.Bot;

namespace TelegaBot.Services;

public class BotService(TelegaBotContext dbContext, TelegramBotClient botClient) : IBotService
{
    private readonly TelegaBotContext _dbContext = dbContext;
    private readonly TelegramBotClient _botClient = botClient;


    [Obsolete("Obsolete")]
    public async Task SendMessageAsync(long chatId, string message)
    {
        await _botClient.SendTextMessageAsync(
            chatId,
            message);
    }
}