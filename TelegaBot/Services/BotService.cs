using Telegram.Bot;
using Telegram.Bot.Types;
using TelegaBot.Services.Interfaces;
using TelegaBot.Services.Receiver;
using TelegaBot.Validators;
using Telegram.Bot.Types.Enums;
using User = TelegaBot.Models.User;

namespace TelegaBot.Services;

public class BotService(
    ITelegramBotClient botClient,
    IUserService userService,
    IUserMailService userMailService,
    MessageReceiver messageReceiver)
    : IBotService
{
    [Obsolete("Obsolete")]
    public async Task SendMessageAsync(long chatId, string message)
    {
        await botClient.SendTextMessageAsync(chatId, message);
    }

    [Obsolete("Obsolete")]
    public void Command(ITelegramBotClient telegramBotClient, Update update, CancellationToken cancellationToken)
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
                        _ = SendWelcomeMessage(chat.Id, telegramBotClient);
                        return;
                    case "/addMail":
                        _ = AddUserMail(chat.Id, telegramBotClient);
                        return;
                    default:
                        return;
                }
            }
        }
    }

    [Obsolete("Obsolete")]
    private async Task SendWelcomeMessage(long chatId, ITelegramBotClient telegramBotClient)
    {
        var user = new User
        {
            TelegramChatId = chatId,
            Id = await userService.GetUsersCountAsync() + 1
        };
        await userService.AddUserAsync(user);

        await telegramBotClient.SendTextMessageAsync(
            chatId,
            "Welcome to Telegram Bot!"
        );
    }

    [Obsolete("Obsolete")]
    private async Task AddUserMail(long chatId, ITelegramBotClient telegramBotClient)
    {
        try
        {
            int? lastErrorId = null; 
            string loginResponse;

            await telegramBotClient.SendTextMessageAsync(chatId, "📧 Введите вашу почту:");

            while (true)
            {
                var (response, messageId) = await messageReceiver.WaitForMessageAsync(chatId);
                int? lastMessageId = messageId;

                if (lastErrorId.HasValue)
                {
                    await telegramBotClient.DeleteMessageAsync(chatId, lastErrorId.Value);
                }

                if (string.IsNullOrWhiteSpace(response) || !EmailValidator.IsValid(response))
                {
                    await telegramBotClient.DeleteMessageAsync(chatId, lastMessageId.Value);
                    var errorMessage =
                        await telegramBotClient.SendTextMessageAsync(chatId, "❌ Некорректный email. Попробуйте снова:");
                    lastErrorId = errorMessage.MessageId;
                    continue;
                }

                var emailExists = await userMailService.NeedToAddUserMailAsync(chatId, response);
                if (!emailExists)
                {
                    await telegramBotClient.DeleteMessageAsync(chatId, lastMessageId.Value);
                    var errorMessage = await telegramBotClient.SendTextMessageAsync(chatId,
                        "⚠️ Такая почта уже зарегистрирована. Попробуйте снова:");
                    lastErrorId = errorMessage.MessageId;
                    continue;
                }

                loginResponse = response;
                break;
            }

            await telegramBotClient.SendTextMessageAsync(chatId, "🔑 Введите ваш пароль:");
            var (passwordResponse, _) = await messageReceiver.WaitForMessageAsync(chatId);

            await userMailService.AddUserMailAsync(chatId, loginResponse, passwordResponse);
            await telegramBotClient.SendTextMessageAsync(chatId, "✅ Почта успешно добавлена!");
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка: {ex.Message}");
        }
    }
}