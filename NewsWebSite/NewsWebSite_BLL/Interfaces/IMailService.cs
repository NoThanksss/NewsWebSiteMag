using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebSite_BLL.Interfaces
{
    public interface IMailService
    {
        Task SendNotification(string userEmail, string author, string articleName);
    }
}