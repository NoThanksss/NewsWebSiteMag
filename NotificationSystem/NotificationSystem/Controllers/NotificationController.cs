using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;

namespace NotificationSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody] NotificationModel model)
            {
                try
                {
                    var mailMessage = new MimeMessage();
                    mailMessage.From.Add(new MailboxAddress("newswebsite@gmail.com", "newswebsite@gmail.com"));
                    mailMessage.To.Add(new MailboxAddress("subscriber", model.userEmail));
                    mailMessage.Subject = "Нова стаття";
                    mailMessage.Body = new TextPart("plain")
                    {
                        Text = $"Нова стаття {model.articleName} від {model.author}"
                    };
                    using (var smtpClient = new SmtpClient())
                    {
                        await smtpClient.ConnectAsync("smtp.gmail.com", 465, true);
                        await smtpClient.AuthenticateAsync("nothanksyou17@gmail.com", "btbj xniu tims tebv");
                        await smtpClient.SendAsync(mailMessage);
                        await smtpClient.DisconnectAsync(true);
                    }
                return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
    }
}
