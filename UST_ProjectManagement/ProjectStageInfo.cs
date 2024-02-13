using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDB.DB;

namespace UST_ProjectManagement
{
    public class ProjectStageInfo
    {
        public Project Project;
        public Stage Stage;

        public string ProjectId;
        public string StageTag;
        public string StageName;
        public string PositionsTxt = "";
        public string GIPs = "";
        public string GAPs = "";
        public string StartDate;
        public string EndDate;

        public List<Position> Positions = new List<Position>();

        public ProjectStageInfo(Project project, Stage stage)
        {
            Project = project;
            Stage = stage;

            ProjectId = project.ProjectId;
            StageTag = stage.StageTag;
            StageName = stage.StageName;
            Positions = RequestInfo.lb.Positions.FindAll(x => x.ProjectId == project.ProjectId).FindAll(c => c.StageId == stage.StageId);
            Positions = Positions.OrderBy(x => x.PositionPos).ToList();
            List<string> gips = new List<string>();
            List<string> gaps = new List<string>();
            List<DateTime> startDates = new List<DateTime>();
            List<DateTime> endDates = new List<DateTime>();

            for (int i = 0; i < Positions.Count(); i++)
            {
                PositionsTxt += Positions[i].PositionPos;
                if (i < Positions.Count() - 1)
                {
                    PositionsTxt += "; ";
                }
                User gip = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId == Positions[i].PositionUserIdGIP);
                if (gip != null && !gips.Contains($"{gip.UserSurname} {gip.UserName} {gip.UserMidlName}"))
                {
                    gips.Add($"{gip.UserSurname} {gip.UserName} {gip.UserMidlName}");
                }

                User gap = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId == Positions[i].PositionUserIdGAP);
                if (gap != null && !gaps.Contains($"{gap.UserSurname} {gap.UserName} {gap.UserMidlName}"))
                {
                    gaps.Add($"{gap.UserSurname} {gap.UserName} {gap.UserMidlName}");
                }

                try
                {
                    startDates.Add(Convert.ToDateTime(Positions[i].PositionDataStart));
                    endDates.Add(Convert.ToDateTime(Positions[i].PositionDataEnd));
                }
                catch
                {
                }

            }
            for (int g = 0; g < gips.Count(); g++)
            {
                GIPs += gips[g];
                if (g < gips.Count() - 1)
                {
                    GIPs += "; ";
                }
            }

            for (int g = 0; g < gaps.Count(); g++)
            {
                GAPs += gaps[g];
                if (g < gaps.Count() - 1)
                {
                    GAPs += "; ";
                }
            }

            try
            {
                StartDate = startDates.Min().ToString();
            }
            catch
            {
                StartDate = "-";
            }
            try
            {
                EndDate = endDates.Max().ToString();
            }
            catch
            {
                EndDate = "-";
            }
        }

        public string CreateNewPositionId(bool subposition)
        {
            int num = 0;

            if (Positions.Count > 0)
            {
                foreach (Position p in Positions)
                {
                    int pNum = 0;
                    try
                    {
                        pNum = Convert.ToInt32(p.PositionPos);
                    }
                    catch
                    {
                        string[] sPos = p.PositionPos.Split();
                        if (sPos.Length > 1)
                        {
                            pNum = Convert.ToInt32(sPos[1]);
                        }
                    }
                    if (pNum > num)
                    {
                        num = pNum;
                    }
                }

                if (!subposition)
                {
                    if (num < 10)
                    {
                        return "0" + (num + 1).ToString();
                    }
                    else
                    {
                        return (num + 1).ToString();
                    } 
                }
                else
                {
                    return (num + 1).ToString();
                }
            }
            else
            {
                if (! subposition)
                {
                    return "00";
                }
                else
                {
                    return "1";
                }
            }
        }
    }
}
