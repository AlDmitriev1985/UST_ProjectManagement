using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public class ClassCreatedTechSolutionStage
    {
        public string StageId { get; set; }

        public string TechSolutionId { get; set; }

        public string UserId { get; set; }

        public string StageTechSolutionDataStart { get; set; }

        public string StageTechSolutionDataEnd { get; set; }

        public string TechSolutionAuthor { get; set; }

        public string Path { get; set; }

        public ClassCreatedTechSolutionStage() { }
    }
}
