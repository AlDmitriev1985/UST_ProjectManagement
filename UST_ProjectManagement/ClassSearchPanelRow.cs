using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public class ClassSearchPanelRow
    {
        public string pNumber { get; set; }
        public string pShortName { get; set; }
        public string pStage { get; set; }
        public string pStartDate { get; set; }
        public string pEndDate { get; set; }
        public string pRelDate { get; set; }
        public string pRespons { get; set; }


        public ClassSearchPanelRow(string Number, string Stage)
        {
            this.pNumber = Number;
            this.pStage = Stage;
        }
    }
}
