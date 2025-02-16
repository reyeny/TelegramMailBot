using Telegram.Bot;
using Telegram.Bot.Polling;
using TelegaBot.Services.Handler;
using TelegaBot.Services.Interfaces;

namespace TelegaBot.Services
{
    public class BotService : IBotService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ReceiverOptions _receiverOptions;
        private readonly UpdateHandlerService _updateHandler;
        private CancellationTokenSource? _cts;

        public BotService(
            ITelegramBotClient botClient, 
            ReceiverOptions receiverOptions, 
            UpdateHandlerService updateHandler)
        {
            _botClient = botClient;
            _receiverOptions = receiverOptions;
            _updateHandler = updateHandler;
        }

        [Obsolete("Obsolete")]
        public async Task SendMessageAsync(long chatId, string message)
        {
            await _botClient.SendTextMessageAsync(chatId, message);
        }

        [Obsolete("Obsolete")]
        public void Start()
        {
            _cts = new CancellationTokenSource();

            _botClient.StartReceiving(
                _updateHandler.HandleUpdateAsync,
                _updateHandler.HandleErrorAsync,
                _receiverOptions,
                _cts.Token
            );

            Console.WriteLine("Bot polling started.");
        }

        public void Stop()
        {
            _cts?.Cancel();
            Console.WriteLine("Bot polling stopped.");
        }
    }
}