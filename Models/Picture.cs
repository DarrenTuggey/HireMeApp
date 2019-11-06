using System;
using System.Collections.Generic;

namespace HireMeApp.Models
{
    public partial class Picture
    {
        public int Id { get; set; }
        public int InfoId { get; set; }
        public byte[] Pic { get; set; }
        public string ImageMimeType { get; set; }

        public virtual InfoName Info { get; set; }
    }
}
