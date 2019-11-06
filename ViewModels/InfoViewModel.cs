using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireMeApp.Models;

namespace HireMeApp.ViewModels
{
    public class InfoViewModel
    {
        public List<InfoName> InfoNameVM { get; set; }
        public List<TextBlock> TextBlockVM { get; set; }
        public List<Picture> PictureVM { get; set; }

    }
}
