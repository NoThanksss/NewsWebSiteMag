﻿using System.ComponentModel.DataAnnotations;

namespace NewsWebSite_BLL.Models
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}