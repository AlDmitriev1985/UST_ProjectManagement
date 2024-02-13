using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDB.DB;

namespace UST_ProjectManagement
{
    public class TechSulutionStageInfo
    {
        public string Code;
        public int ID;
        public int StageId;
        public string StageTag;
        public string ProductName;
        public string Discription;
        public string GIP;
        public string SectionsTxt;
        public string PersentComplete;
        public int LanguageId = 3;
        public bool SetListeInRelease;

        List<SectionsTechSolution> SectionsSolutions = new List<SectionsTechSolution>();
        public Dictionary<SectionsThree, SectionsTechSolution> SectionsDict = new Dictionary<SectionsThree, SectionsTechSolution>();
        public Dictionary<string, SectionsThree> SecThreeDict = new Dictionary<string, SectionsThree>();
        public List<SectionsThree> ListSecThree = new List<SectionsThree>();
        public List<ScheduleItem> scheduleItems = new List<ScheduleItem>();

        public TechSulutionStageInfo(TechSolution techSolution, Stage stage)
        {
            Code = techSolution.TechSolutionCode;
            ID = techSolution.TechSolutionId;
            StageTag = stage.StageTag;
            StageId = stage.StageId;
            ProductName = techSolution.TechSolutionFullName;
            Discription = techSolution.TechSolutionDescription;
            if (Discription == "") Discription = "-";
            LanguageId = stage.LanguageId.Value;

            User gip = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId == techSolution.UserId);
            if (gip != null) GIP = $"{gip.UserSurname} {gip.UserName} {gip.UserMidlName}";

            SectionsSolutions = RequestInfo.lb.SectionsTechSolutions.FindAll(x => x.TechSolutionId == techSolution.TechSolutionId).ToList();
            foreach(var secP in SectionsSolutions)
            {
                SectionsThree section = RequestInfo.lb.SectionsThrees.FirstOrDefault(x => x.SectionThreeId == secP.SectionThreeId);
                //ListSecThree.Add(section);
                
                secP.Status = RequestInfo.lb.Status.FirstOrDefault(x => x.StatusId == secP.StatusId);
                if (section != null)
                {
                    SectionsDict.Add(section, secP);
                    ScheduleItem item = new ScheduleItem();
                    item.PositionCode = Code;
                    item.SecOneId = section.SectionOneId.Value;

                    item.SecThreeId = section.SectionThreeId;
                    item.SecThreeNum = section.SectionThreeNum;

                    if (stage.LanguageId == 2)
                    {
                        item.SecThreeTag = section.SectionThreeTagEng;
                        item.SecThreeName = section.SectionThreeNameEng;

                        SectionsTxt += section.SectionThreeTagEng + ", ";
                        SecThreeDict.Add(section.SectionThreeTagEng, section);
                    }
                    else
                    {
                        item.SecThreeTag = section.SectionThreeTagRus;
                        item.SecThreeName = section.SectionThreeNameRus;

                        SectionsTxt += section.SectionThreeTagRus + ", ";
                        SecThreeDict.Add(section.SectionThreeTagRus, section);
                    }
                    scheduleItems.Add(item);
                }
            }
            


            if (SectionsSolutions.Count() > 0)
            {
                PersentComplete = (SectionsSolutions.Sum(x => x.SectionProgress) / SectionsSolutions.Count()).ToString() + "%";
            }
            else
            {
                PersentComplete = "0%";
            }

            if (SectionsSolutions.Count > 0) SetListeInRelease = true;
            else SetListeInRelease = false;
        }
    }
}
