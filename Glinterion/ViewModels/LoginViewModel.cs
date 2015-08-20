using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Glinterion.ViewModels
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Login is incorrect!")]
        [DisplayName("Login")]
        public string Login { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter the password!")]
        [DisplayName("Password")]
        public string Password { get; set; }

        
    }
}