using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Web;

namespace Glinterion.Models
{
    public class Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        [Required]
        public string Name { get; set; }

        public long? DurationTicks { get; set; }

        [NotMapped]
        public TimeSpan Duration { get { return TimeSpan.FromTicks(DurationTicks ?? 0); } set { DurationTicks = value.Ticks; } }

        public double? MaxSize { get; set; }

        // to be colored on profile page
        [NotMapped]
        public Color Color
        {
            get { if (ColorValue != null) return Color.FromArgb(ColorValue.Value); 
                return Color.Black;
            }
            set { ColorValue = value.ToArgb(); }
        }

        public int? ColorValue
        {
            get;
            set;
        }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<AccountSerial> Serials { get; set; }
    }
}