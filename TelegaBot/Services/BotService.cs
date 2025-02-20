using TelegaBot.Migrations;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using TelegaBot.Services.Interfaces;
using Telegram.Bot.Types.Enums;
using User = TelegaBot.Models.User;

namespace TelegaBot.Services
{
    public class BotService : IBotService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ReceiverOptions _receiverOptions;
        private CancellationTokenSource? _cts;
        private readonly IUserService _userService;

        public BotService(
            ITelegramBotClient botClient,
            ReceiverOptions receiverOptions,
            IUserService userService)
        {
            _botClient = botClient;
            _receiverOptions = receiverOptions;
            _userService = userService;
        }

        public async Task SendMessageAsync(long chatId, string message)
        {
            await _botClient.SendTextMessageAsync(chatId, message);
        }

        [Obsolete("Obsolete")]
        public void Command(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            var chat = message!.Chat;

            switch (update.Type)
            {
                case UpdateType.Message:
                {
                    switch (message.Text)
                    {
                        case "/start":
                            _ = SendWelcomeMessage(chat.Id, botClient);
                            return;
                        case "/addMail":
                            _ = AddUserMail(chat.Id, botClient);
                            return;
                        default:
                            return;
                    }
                }
            }
        }

        [Obsolete("Obsolete")]
        private  async Task SendWelcomeMessage(long chatId, ITelegramBotClient botClient)
        {
            var user = new User
            {
                TelegramChatId = chatId,
                Id = await _userService.GetUsersCountAsync() + 1
            };
            await _userService.AddUserAsync(user);

            await botClient.SendTextMessageAsync(
                chatId,
                "Welcome to Telegram Bot!"
            );
        }

        private async Task AddUserMail(long chatId, ITelegramBotClient botClient)
        {
            
            await botClient.SendTextMessageAsync(chatId, "Введите ваш логин:");
            
            
        }
    }
}