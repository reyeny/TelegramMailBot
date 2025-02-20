namespace TelegaBot.Services.Receiver;

public class MessageReceiver
{
    private readonly Dictionary<long, TaskCompletionSource<string>> _waitingUsers = new();

    public Task<string> WaitForMessageAsync(long chatId)
    {
        var tcs = new TaskCompletionSource<string>();
        _waitingUsers[chatId] = tcs;
        return tcs.Task;
    }

    public void ReceiveMessage(long chatId, string message)
    {
        if (_waitingUsers.TryGetValue(chatId, out var tcs))
        {
            tcs.SetResult(message);
            _waitingUsers.Remove(chatId);
        }
    }
}