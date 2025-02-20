namespace TelegaBot.Services.Receiver;

public class MessageReceiver
{
    private readonly Dictionary<long, TaskCompletionSource<(string, int)>> _waitingUsers = new();
    
    public Task<(string, int)> WaitForMessageAsync(long chatId)
    {
        var tcs = new TaskCompletionSource<(string, int)>();
        _waitingUsers[chatId] = tcs;
        return tcs.Task;
    }
    
    public void ReceiveMessage(long chatId, string message, int messageId)
    {
        if (_waitingUsers.TryGetValue(chatId, out var tcs))
        {
            tcs.SetResult((message, messageId));
            _waitingUsers.Remove(chatId);
        }
    }
}