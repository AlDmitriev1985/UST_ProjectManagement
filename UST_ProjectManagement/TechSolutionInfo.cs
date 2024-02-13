using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDB.DB;

namespace UST_ProjectManagement
{
    public class TechSolutionInfo
    {
        public int ID;

        public List<StageTechSolution> pStages = new List<StageTechSolution>();

        public StageTechSolution pStage;

        public List<SectionsTechSolution> pSections = new List<SectionsTechSolution>();

        public int LanguageId = 3;

        public string Code = "-";
        public string StageTag;
        public string Name;
        public string Responsible = "-";
        public string StartDate;
        public string EndDate;
        public string PersentComplete = "-";

        List<SectionsTechSolution> SectionsSolution = new List<SectionsTechSolution>();
        public Dictionary<SectionsThree, SectionsTechSolution> SectionsDict = new Dictionary<SectionsThree, SectionsTechSolution>();
        public List<SectionsThree> ListSecThree = new List<SectionsThree>();

        public TechSolutionInfo(TechSolution solution, Stage stage)
        {
            Code = solution.TechSolutionCode;
            ID = solution.TechSolutionId;

            pStages = RequestInfo.lb.StageTechSolutions.FindAll(x => x.TechSolutionId == solution.TechSolutionId);

            pStage = pStages.FirstOrDefault(x => x.StageId == stage.StageId);

            pSections = RequestInfo.lb.SectionsTechSolutions.Where(x => x.TechSolutionId == solution.TechSolutionId).ToList();

            LanguageId = stage.LanguageId.Value;

            SectionsSolution = RequestInfo.lb.SectionsTechSolutions.FindAll(x => x.TechSolutionId == solution.TechSolutionId).ToList();

            foreach (var secP in SectionsSolution)
            {
                SectionsThree section = RequestInfo.lb.SectionsThrees.FirstOrDefault(x => x.SectionThreeId == secP.SectionThreeId);
                ListSecThree.Add(section);
                secP.Status = RequestInfo.lb.Status.FirstOrDefault(x => x.StatusId == secP.StatusId);
            }

            try
            {
                StageTag = RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == pStage.StageId).StageTag;
            }
            catch
            {

            }
            User user = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId == solution.UserId);
            Responsible = $"{user.UserSurname} {user.UserName} {user.UserMidlName}";
            Name = solution.TechSolutionFullName;
            StartDate = solution.TechSolutionDataStart;
            EndDate = solution.TechSolutionDataEnd;
        }

        public List<SectionsThree> GetSecThreeList()
        {
            List<SectionsThree> result = RequestInfo.lb.SectionsThrees.Where(y => (pSections.Select(x => x.SectionThreeId).ToList()).Contains(y.SectionThreeId)).ToList();

            return result;
        }


    }

}
