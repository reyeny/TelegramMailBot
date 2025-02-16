using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TelegaBot.Context;
using TelegaBot.Services;
using TelegaBot.Services.Interfaces;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

var botToken = builder.Configuration["TelegramBotSettings:BotToken"];

if (string.IsNullOrWhiteSpace(botToken))
    throw new Exception("Bot token not found. Make sure it is set in user secrets or configuration.");


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TelegaBotContext>(opt => opt.UseNpgsql(connectionString));


builder.Services.AddScoped<IBotService, BotService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));
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

app.Run();