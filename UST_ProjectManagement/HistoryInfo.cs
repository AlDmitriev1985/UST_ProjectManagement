using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSTServer.History
{
    public class HistoryInfo
    {
        public string User { get; set; }

        public string Date { get; set; }

        public string Info { get; set; }

        public string Description { get; set; } = "";
    }
}
