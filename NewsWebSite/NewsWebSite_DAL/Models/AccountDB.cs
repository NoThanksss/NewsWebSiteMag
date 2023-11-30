using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsWebSite_DAL.Models
{
    public class AccountDB : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid UserDBId { get; set; }
        public UserDB UserDB { get; set; }
        [InverseProperty("Subscribtions")]
        public virtual ICollection<AccountDB> Subscibers { get; set; }
        [InverseProperty("Subscibers")]
        public virtual ICollection<AccountDB> Subscribtions { get; set; }

    }
}
