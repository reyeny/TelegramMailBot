using TelegaBot.Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace TelegaBot.Controller
{
    public class BotController(IBotService botService)
    {
        public async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                botService.Command(botClient, update, cancellationToken);
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