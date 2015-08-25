using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Glinterion.Models
{
    public class Photo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhotoId { get; set; }

        [Required]
        public string SrcPreview { get; set; }

        [Required]
        public string SrcOriginal { get; set; }

        public string Description { get; set; }

        public double? Rating { get; set; }

        [Required]
        // in MB
        public double Size { get; set; }

        public int UserId { get; set; }

        //[InverseProperty("Photos")]
        public virtual User User { get; set; }
    }
}