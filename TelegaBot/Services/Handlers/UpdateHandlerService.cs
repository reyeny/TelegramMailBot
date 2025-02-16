using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegaBot.Services.Handlers
{
    public class UpdateHandlerService(ITelegramBotClient botClient)
    {
        private readonly ITelegramBotClient _botClient = botClient;

        [Obsolete("Obsolete")]
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is { } message && !string.IsNullOrEmpty(message.Text))
            {
                var chatId = message.Chat.Id;
                var messageText = message.Text;

                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"Вы написали: {messageText}",
                    cancellationToken: cancellationToken
                );
            }
        }

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Error in polling: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}