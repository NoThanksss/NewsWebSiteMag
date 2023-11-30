using NewsWebSite_BLL.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsWebSite_BLL.Models
{
    public class Account
    {
        [SwaggerExclude]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid UserId { get; set; }
        [InverseProperty("Subscribtions")]
        public ICollection<Account> Subscibers { get; set; }
        [InverseProperty("Subscibers")]
        public ICollection<Account> Subscribtions { get; set; }

    }
}
