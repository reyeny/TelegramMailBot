namespace TelegaBot.Services.Interfaces;

public interface IBotService
{
    Task SendMessageAsync(long chatId, string message);
}