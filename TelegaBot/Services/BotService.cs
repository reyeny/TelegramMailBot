using TelegaBot.Services.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;
using TelegaBot.Services.Interfaces;

namespace TelegaBot.Services
{
    public class BotService(
        ITelegramBotClient botClient,
        ReceiverOptions receiverOptions,
        UpdateHandlerService updateHandler)
        : IBotService
    {
        private readonly ReceiverOptions _receiverOptions = receiverOptions;
        private readonly UpdateHandlerService _updateHandler = updateHandler;
        private CancellationTokenSource? _cts;

        
        [Obsolete("Obsolete")]
        public async Task SendMessageAsync(long chatId, string message)
        {
            await botClient.SendTextMessageAsync(chatId, message);
        }
        
    }
}