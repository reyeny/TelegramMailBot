using TelegaBot.Services.Interfaces;
using TelegaBot.Services.Receiver;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegaBot.Controller
{
    public class BotController(IBotService botService, MessageReceiver messageReceiver)
    {
        public async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                botService.Command(botClient, update, cancellationToken);
                
                if (update is { Type: UpdateType.Message, Message.Text: { } messageText })
                {
                    var chatId = update.Message.Chat.Id;
                    var messageId = update.Message.MessageId; 

                    messageReceiver.ReceiveMessage(chatId, messageText, messageId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        
        
        public Task HandleErrorAsync(
            ITelegramBotClient client,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }
}