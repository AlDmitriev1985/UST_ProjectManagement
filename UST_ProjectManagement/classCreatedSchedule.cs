using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public class classCreatedSchedule
    {
        public string ID { get; set; }

        public string PositionId { get; set; }
        public string Responsble { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }

        public classCreatedSchedule(string id)
        {
            ID = id;
        }
    }
}
