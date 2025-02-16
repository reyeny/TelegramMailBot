using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegaBot.Services.Interfaces
{
    public interface IBotService
    {
        Task SendMessageAsync(long chatId, string message);
        void Command(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

    }
}