using Microsoft.AspNetCore.Mvc;
using TelegaBot.Models;
using TelegaBot.Services.Interfaces;

namespace TelegaBot.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class BotController(IBotService botService, IUserService userService) : ControllerBase
    {

        [HttpPost("add-mail")]
        public async Task<IActionResult> AddMail([FromBody] UserMail? mail)
        {
            if (mail == null)
                return BadRequest("Mail data is null");

            await userService.AddMailAsync(mail);
            return Ok(mail);
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendTestMessage([FromQuery] long chatId, [FromQuery] string message)
        {
            await botService.SendMessageAsync(chatId, message);
            return Ok("Message sent");
        }
    }
}