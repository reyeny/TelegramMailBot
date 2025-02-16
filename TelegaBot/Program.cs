using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TelegaBot.Context;
using TelegaBot.Controller;
using TelegaBot.Services;
using TelegaBot.Services.Handlers;
using TelegaBot.Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Polling;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

var botToken = builder.Configuration["TelegramBotSettings:BotToken"];
if (string.IsNullOrWhiteSpace(botToken))
    throw new Exception("Bot token not found. Make sure it is set in user secrets or configuration.");

builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));
builder.Services.AddSingleton(new ReceiverOptions());
builder.Services.AddScoped<UpdateHandlerService>();
builder.Services.AddScoped<BotController>();
builder.Services.AddScoped<IBotService, BotService>();
builder.Services.AddScoped<IUserService, UserService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TelegaBotContext>(opt => opt.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello from root!");
app.MapControllers();

// Запускаем бота
var bot = app.Services.GetRequiredService<BotController>();
bot.StartBot();

app.Run();