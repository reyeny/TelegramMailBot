namespace TelegaBot.Models;

public class UserMail
{
    public int Id { get; set; }
    public string Mail { get; set; }
    public string Password { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}