﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Glinterion.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        public string Login { get; set; }

        public int RoleId { get; set; }

        //public int AvatarId { get; set; }
        
        [Required]
        public string Email { get; set; }

        [InverseProperty("Users")]
        public virtual Role Role { get; set; }

        //[ForeignKey("AvatarId")]
        //[InverseProperty("User")]
        //public virtual Photo Avatar { get; set; }

        public int AccountId { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Password { get; set; }

        [InverseProperty("Users")]
        public virtual Account Account { get; set; }

        public virtual ICollection<Photo> Photos { get; set; } 
    }
}