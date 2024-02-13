using POSTServer.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public class ClassSubsetInfo
    {
        public string SubSetId { get; set; }

        public int PercentComplete { get; set; }

        public string Status { get; set; }

        public int StatusId { get; set; }

        public string Responsible { get; set; }

        public string History { get; set; }

        public string HeadOfDep { get; set; }

        public HistoryLog history { get; set; }

        public string Comment { get; set; }

        public ClassSubsetInfo(string id)
        {
            SubSetId = id;
        }
    }
}
