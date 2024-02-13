using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public class CreateTechSolution
    {
        public byte Type { get; set; } = 0;

        public int TechSolutionGroupId { get; set; }

        public int UserId { get; set; }

        public string TechSolutionCode { get; set; }

        public string TechSolutionGroupCode { get; set; }

        public string TechSolutionFullName { get; set; }

        public string TechSolutionDescription { get; set; }

        public string TechSolutionDataStart { get; set; }

        public string TechSolutionDataEnd { get; set; }

        public string TechSolutionAuthor { get; set; }


    }
}
