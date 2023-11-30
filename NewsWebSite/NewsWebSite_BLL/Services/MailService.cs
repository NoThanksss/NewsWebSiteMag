using NewsWebSite_BLL.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;
using NewsWebSite_BLL.Exceptions;
using NewsWebSite_BLL.Models;
using System.Text;
using Newtonsoft.Json;

namespace NewsWebSite_BLL.Services
{
    public class MailService : IMailService
    {
        private readonly HttpClient client;
        public MailService(IHttpClientFactory factory)
        {
            client = factory.CreateClient();
        }
        public async Task SendNotification(string userEmail, string author, string articleName)
        {
            var body = new NotificationModel { articleName = articleName, author = author, userEmail = userEmail };
            var json = JsonConvert.SerializeObject(body);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PostAsync("https://localhost:7125/notification", data);
        }
    }
}