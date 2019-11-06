using System;
using System.Collections.Generic;

namespace HireMeApp.Models
{
    public partial class AccessLogs
    {
        public int IdColumn { get; set; }
        public string PageName { get; set; }
        public DateTime AccessDate { get; set; }
    }
}
