﻿using System;
using System.Collections.Generic;

namespace HireMeApp.Models
{
    public partial class TextBlock
    {
        public int Id { get; set; }
        public int InfoId { get; set; }
        public string Textblock1 { get; set; }

        public virtual InfoName Info { get; set; }
    }
}