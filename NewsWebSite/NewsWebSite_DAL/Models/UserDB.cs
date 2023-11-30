using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebSite_DAL.Models
{
    public class UserDB
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public AccountDB AccountDB { get; set; }
    }
}
