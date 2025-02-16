using TelegaBot.Services.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace TelegaBot.Controller
{
    public class BotController(
        ITelegramBotClient botClient,
        ReceiverOptions receiverOptions,
        UpdateHandlerService updateHandler)
    {
        private CancellationTokenSource? _cts;

        public void StartBot()
        {
            _cts = new CancellationTokenSource();

            botClient.StartReceiving(
                updateHandler.HandleUpdateAsync,
                updateHandler.HandleErrorAsync,
                receiverOptions,
                _cts.Token
            );

            Console.WriteLine("Bot polling started.");
        }

        public void StopBot()
        {
            _cts?.Cancel();
            Console.WriteLine("Bot polling stopped.");
        }
        
    }
}
