﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Glinterion.Models
{
    public class Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        [Required]
        public string Name { get; set; }

        public long DurationTicks { get; set; }

        [NotMapped]
        public TimeSpan Duration { get { return TimeSpan.FromTicks(DurationTicks); } set { DurationTicks = value.Ticks; } }

        // to be colored on profile page
        public Color Color { get; set; }

        public virtual ICollection<User> Users { get; set; } 
    }
}