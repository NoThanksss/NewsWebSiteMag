using NewsWebSite_BLL.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebSite_BLL.Models
{
    public class User
    {
        [SwaggerExclude]
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

    }
}
