using Microsoft.AspNetCore.Mvc;
using TelegaBot.Models;
using TelegaBot.Services.Interfaces;
using Telegram.Bot.Types;

namespace TelegaBot.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class BotController : ControllerBase
    {
        private readonly IBotService _botService;
        private readonly IUserService _userService;

        public BotController(IBotService botService, IUserService userService)
        {
            _botService = botService;
            _userService = userService;
        }

        [HttpPost("update")]
        public async Task<IActionResult> PostUpdate([FromBody] Update update)
        {
            if (update?.Message is not null)
            {
                var chatId = update.Message.Chat.Id;
                var messageText = update.Message.Text;

                await _botService.SendMessageAsync(chatId, $"Вы написали: {messageText}");
            }

            return Ok();
        }

        [HttpPost("add-mail")]
        public async Task<IActionResult> AddMail([FromBody] UserMail? mail)
        {
            if (mail == null)
                return BadRequest("Mail data is null");

            await _userService.AddMailAsync(mail);
            return Ok(mail);
        }

        // Пример эндпоинта для отправки сообщения (вручную, из вашего кода)
        [HttpPost("send-message")]
        public async Task<IActionResult> SendTestMessage([FromQuery] long chatId, [FromQuery] string message)
        {
            await _botService.SendMessageAsync(chatId, message);
            return Ok("Message sent");
        }
    }
}