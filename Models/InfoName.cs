using System;
using System.Collections.Generic;

namespace HireMeApp.Models
{
    public partial class InfoName
    {
        public InfoName()
        {
            Picture = new HashSet<Picture>();
            TextBlock = new HashSet<TextBlock>();
        }

        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string Info_Name { get; set; }

        public virtual ICollection<Picture> Picture { get; set; }
        public virtual ICollection<TextBlock> TextBlock { get; set; }
    }
}
