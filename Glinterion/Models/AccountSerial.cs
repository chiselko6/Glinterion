using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Glinterion.Models
{
    public class AccountSerial
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountSerialId { get; set; }

        [Required]
        [StringLength(maximumLength: 18, MinimumLength = 18)]
        public string Serial { get; set; }

        public int AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}