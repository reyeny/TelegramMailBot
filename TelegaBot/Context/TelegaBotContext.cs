using Microsoft.EntityFrameworkCore;
using TelegaBot.Models;

namespace TelegaBot.Context;

public class TelegaBotContext(DbContextOptions<TelegaBotContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserMail?> UserMails { get; set; }
}