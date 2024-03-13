using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDB.DB;
using Newtonsoft.Json;
using POSTServer.History;

namespace UST_ProjectManagement
{
    public class PositionInfo
    {
        public string Code;
        public int ID;
        public string StageTag;
        public int StageId;
        public string ProjectName;
        public string PositionName;
        public string GIP = "-";
        public string GAP = "-";
        public string StartDate;
        public string EndDate;
        public string SectionsTxt;
        public string PersentComplete;
        public int LanguageId;
        public bool SetListeInRelease;
        public string CoordinationStatus;
        public string BasePoint;
        public LibraryDB.DB.Task axistask;

        public List<SectionsPosition> SectionsPositions = new List<SectionsPosition>();
        public Dictionary<SectionsPosition, SectionsThree> SectionsDict = new Dictionary<SectionsPosition, SectionsThree>();
        //public Dictionary<string, SectionsThree> SetsDict = new Dictionary<string, SectionsThree>();
        public List<SectionsThree> ListSecThree = new List<SectionsThree>();
        public List<HistoryInfo> coordinationHistory = new List<HistoryInfo>();
        public List<ScheduleItem> scheduleItems = new List<ScheduleItem>();
        

        public PositionInfo(Project project, Stage stage,Position position)
        {
            Code = position.PositionCode;
            BasePoint = position.PositionBasePointIntersection;
            axistask = TryGetTaskAxis(position);
            ID = position.PositionId;
            StageTag = stage.StageTag;
            StageId = stage.StageId;
            ProjectName = project.ProjectName;
            PositionName = position.PositionName;
            LanguageId = stage.LanguageId.Value;

            User gip = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId == position.PositionUserIdGIP);
            if (gip != null) GIP = $"{gip.UserSurname} {gip.UserName} {gip.UserMidlName}";

            User gap = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId == position.PositionUserIdGAP);
            if (gap != null) GAP = $"{gap.UserSurname} {gap.UserName} {gap.UserMidlName}";

            StartDate = position.PositionDataStart;
            EndDate = position.PositionDataEnd;

            SectionsPositions = RequestInfo.lb.SectionsPositions.FindAll(x => x.PositionId == position.PositionId).ToList();
            if (SectionsPositions.Count > 0) SetListeInRelease = true;
            else SetListeInRelease = false;

            foreach(var secP in SectionsPositions)
            {
                List <SectionsThree> sections = RequestInfo.lb.SectionsThrees.Where(x => x.SectionThreeId == secP.SectionThreeId).ToList();
                
                //ListSecThree.Add(section);
                secP.Status = RequestInfo.lb.Status.FirstOrDefault(x => x.StatusId == secP.StatusId);

                foreach (var section in sections)
                {
                    
                    if (section != null)
                    {
                        
                        try
                        {
                            SectionsThree sectionsThree = new SectionsThree();
                            sectionsThree.SectionThreeId = section.SectionThreeId;
                            sectionsThree.SectionThreeNum = section.SectionThreeNum;
                            sectionsThree.SectionThreeTagEng = section.SectionThreeTagEng + secP.SectionPositionNumber;
                            sectionsThree.SectionThreeNameEng = section.SectionThreeNameEng;
                            sectionsThree.SectionThreeTagRus = section.SectionThreeTagRus + secP.SectionPositionNumber;
                            sectionsThree.SectionThreeNameRus = section.SectionThreeNameRus;
                            sectionsThree.SectionPositionNumber = secP.SectionPositionNumber;
                            if (secP.DepartmentIdDelegation == null)
                            {
                                sectionsThree.DepartmentId = section.DepartmentId; 
                            }
                            else
                            {
                                sectionsThree.DepartmentId = secP.DepartmentIdDelegation.GetValueOrDefault();
                            }

                            ListSecThree.Add(sectionsThree);


                            SectionsDict.Add(secP, section);
                            ScheduleItem item = new ScheduleItem();
                            item.PositionCode = Code;
                            item.SecOneId = section.SectionOneId.Value;
                            item.SecTowId = section.SectionTwoId.Value;

                            item.SecThreeId = section.SectionThreeId;
                            item.SecThreeNum = section.SectionThreeNum;
                            try
                            {
                                item.SecThreePostfix = secP.SectionPositionNumber;
                            }

                            catch
                            {
                            }
                            try
                            {
                                item.DelegatedDepId = secP.DepartmentIdDelegation.Value;
                            }
                            catch
                            {
                            }

                            if (stage.LanguageId == 2)
                            {
                                item.SecThreeTag = section.SectionThreeTagEng;
                                item.SecThreeName = section.SectionThreeNameEng;

                                secP.Tag = section.SectionThreeTagEng;
                                
                                SectionsTxt += section.SectionThreeTagEng + secP.SectionPositionNumber + ", ";
                                
                            }
                            else
                            {
                                item.SecThreeTag = section.SectionThreeTagRus;
                                item.SecThreeName = section.SectionThreeNameRus;

                                secP.Tag = section.SectionThreeTagRus;

                                SectionsTxt += section.SectionThreeTagRus + secP.SectionPositionNumber + ", ";
                            }
                            secP.TagNum = secP.Tag + secP.SectionPositionNumber;
                            item.Progress = secP.SectionProgress;
                            item.Status = secP.Status;
                            item.History = secP.StatusHistory;
                            scheduleItems.Add(item);

                        }
                        catch { } 
                    }
                }
            }
            if (SectionsTxt != "")
            {
                try
                {
                    SectionsTxt = SectionsTxt.TrimEnd();
                    SectionsTxt = SectionsTxt.TrimEnd(new char[] { ',', ',' });
                }
                catch
                {

                }
            }
            else
            {
                SectionsTxt = "-";
            }


            if (SectionsPositions.Count() > 0)
            {
                PersentComplete = (SectionsPositions.Sum(x => x.SectionProgress) / SectionsPositions.Count()).ToString() + "%";
            }
            else
            {
                PersentComplete = "0%";
            }
            GetCoordinationHistory(position, out coordinationHistory, out CoordinationStatus);
        }

        private LibraryDB.DB.Task TryGetTaskAxis(Position position)
        {
            HistoryLog historyLog = null;
            try
            {
                historyLog = JsonConvert.DeserializeObject<HistoryLog>(position.PositionHistory);
            }
            catch { }

            if (historyLog != null)
            {
                for (int i = historyLog.spHistory.Count - 1; i >= 0; i--)
                {
                    string[] split = historyLog.spHistory[i].Info.Split('|');
                    if (split[0] == "Передана информация по разбивочному плану.")
                    {
                        try
                        {
                            int taskId = Convert.ToInt32(split[1]);
                            LibraryDB.DB.Task task = RequestInfo.lb.Tasks.FirstOrDefault(x => x.TaskId == taskId);
                            return task;
                        }
                        catch 
                        {
                            return null;
                        }
                    }
                }
            }
            return null;
        }

        private void GetCoordinationHistory(Position position, out List<HistoryInfo> historyInfo, out string status)
        {
            historyInfo = new List<HistoryInfo>();
            status = "Координаты не переданы";

            HistoryLog historyMP = new HistoryLog();
            HistoryLog historyBIM = new HistoryLog();
            try
            {
                historyMP = JsonConvert.DeserializeObject<HistoryLog>(GlobalData.SelectedPosition.PositionCoordinateHistory);
            }
            catch { }
            try
            {
                historyBIM = JsonConvert.DeserializeObject<HistoryLog>(GlobalData.SelectedPosition.PositionCoordinateHistoryBIM);
            }
            catch { }

            historyInfo.AddRange(historyMP.spHistory.ToArray());
            historyInfo.AddRange(historyBIM.spHistory.ToArray());
            historyInfo = historyInfo.OrderBy(x => x.Date).ToList();

            if (historyInfo.Count > 0)
            {
                string account = historyInfo[0].User;
                User user = RequestInfo.lb.Users.FirstOrDefault(a => a.UserAccount == account);
                if (user != null)
                {
                    if (user.FunctionId == 7) status = "Позиция скоординирована";
                    else status = "Позиция не скоординированы";
                }
            }
        }

        public void GedDepByTag(string tag, out string depName, out string headName, out Department department, out User depHeade, out SectionsThree sectionsThree)
        {
            depName = "";
            headName = "";
            department = null;
            depHeade = null;
            sectionsThree = ListSecThree.FirstOrDefault(x => x.SectionThreeTagRus == tag || x.SectionThreeTagEng == tag);
            SectionsThree secThree = sectionsThree;
            try
            {
                department = RequestInfo.lb.Departments.FirstOrDefault(x => x.DepartmentId == secThree.DepartmentId);
                depName = department.DepartmentName;
                var headeId = department.DepartmentHeade;
                depHeade = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId == headeId);
                headName = depHeade.UserSurname + " " + depHeade.UserName;
            }
            catch { }
        }

    }
}
