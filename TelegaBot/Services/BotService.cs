using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using TelegaBot.Services.Interfaces;
using TelegaBot.Services.Receiver;
using TelegaBot.Validators;
using Telegram.Bot.Types.Enums;
using User = TelegaBot.Models.User;

namespace TelegaBot.Services
{
    public class BotService(
        ITelegramBotClient botClient,
        ReceiverOptions receiverOptions,
        IUserService userService,
        IUserMailService userMailService,
        MessageReceiver messageReceiver)
        : IBotService
    {
        private readonly ReceiverOptions _receiverOptions = receiverOptions;
        private CancellationTokenSource? _cts;


        public async Task SendMessageAsync(long chatId, string message)
        {
            await botClient.SendTextMessageAsync(chatId, message);
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
                Id = await userService.GetUsersCountAsync() + 1
            };
            await userService.AddUserAsync(user);

            await botClient.SendTextMessageAsync(
                chatId,
                "Welcome to Telegram Bot!"
            );
        }

        [Obsolete("Obsolete")]
        private async Task AddUserMail(long chatId, ITelegramBotClient botClient)
        {
            try
            {
                await botClient.SendTextMessageAsync(chatId, "Введите вашу почту:");
                var loginResponse = await messageReceiver.WaitForMessageAsync(chatId);

                if (!EmailValidator.IsValid(loginResponse))
                {
                    await botClient.SendTextMessageAsync(chatId, "Вы ввели неверный логин. Попробуйте снова.");
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(loginResponse))
                {
                    await botClient.SendTextMessageAsync(chatId, "Логин не может быть пустым. Попробуйте снова.");
                    return;
                }
                
                var checkUserMail = await userMailService.NeedToAddUserMailAsync(chatId, loginResponse);
                if (!checkUserMail)
                {
                    await botClient.SendTextMessageAsync(chatId, "Такая почта уже существует.");
                    return;
                }

                await botClient.SendTextMessageAsync(chatId, "Введите ваш пароль:");
                var passwordResponse = await messageReceiver.WaitForMessageAsync(chatId);
                if (string.IsNullOrWhiteSpace(passwordResponse))
                {
                    await botClient.SendTextMessageAsync(chatId, "Пароль не может быть пустым. Попробуйте снова.");
                    return;
                }
                
                await userMailService.AddUserMailAsync(chatId, loginResponse, passwordResponse);
                await botClient.SendTextMessageAsync(chatId, "Почта успешно добавлена.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Произошла ошибка: {ex.Message}");
            }
        }


    }
}