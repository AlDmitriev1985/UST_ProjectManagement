using LibraryDB.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public class ScheduleItem
    {
        public int SecOneId { get; set; }
        public int SecTowId { get; set; }
        public int SecThreeId { get; set; }
        public string SecThreeNum { get; set; }
        public string SecThreeTag { get; set; }
        public string SecThreePostfix { get; set; }
        public string SecThreeName { get; set; }           
        public int? DelegatedDepId { get; set; }
        public int? Progress { get; set; }
        public Status Status { get; set; }
        public string History { get; set; }

        public string PositionCode { get; set; }


        public override bool Equals(object obj)
        {
            ScheduleItem other = obj as ScheduleItem;
            if (other == null)
            {
                return false;
            }
            else
            {
                if (SecOneId == other.SecOneId &&
                    SecThreeId == other.SecThreeId &&
                    SecThreePostfix == other.SecThreePostfix)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
