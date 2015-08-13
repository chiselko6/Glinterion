using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glinterion.Models
{
    public class Photo
    {
        public int ID { get; set; }

        public string SrcPreview { get; set; }

        public string SrcOriginal { get; set; }

        public string Description { get; set; }

        public double Rating { get; set; }

        // in MB
        public double Size { get; set; }
    }
}