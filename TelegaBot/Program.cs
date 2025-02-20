using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TelegaBot.Context;
using TelegaBot.Controller;
using TelegaBot.Services;
using TelegaBot.Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TelegaBotContext>(opt => opt.UseNpgsql(connectionString));

var botToken = builder.Configuration["TelegramBotSettings:BotToken"];
if (string.IsNullOrWhiteSpace(botToken))
    throw new Exception("Bot token not found.");

builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));
builder.Services.AddSingleton(new ReceiverOptions());
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBotService, BotService>();
builder.Services.AddScoped<IUserMailService, UserMailService>();
builder.Services.AddScoped<BotController>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var botClient = app.Services.GetRequiredService<ITelegramBotClient>();
var botController = app.Services.GetRequiredService<BotController>();

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = [UpdateType.Message]
};

// Создаем CancellationTokenSource
using var cts = new CancellationTokenSource();

// Запускаем получение обновлений, передавая методы с корректными сигнатурами
botClient.StartReceiving(
    botController.UpdateHandler,
    botController.HandleErrorAsync,
    receiverOptions,
    cts.Token);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();