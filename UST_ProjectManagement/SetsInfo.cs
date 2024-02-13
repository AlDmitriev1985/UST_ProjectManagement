using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDB.DB;

namespace UST_ProjectManagement
{
    public class SetsInfo
    {
        private IEnumerable<IGrouping<int, SectionsTwo>> OneTow = RequestInfo.lb.SectionsTwoes.GroupBy(x => x.SectionOneId.Value);
        private IEnumerable<IGrouping<int, SectionsThree>> OneThree = RequestInfo.lb.SectionsThrees.GroupBy(x => x.SectionOneId.Value);
        private IEnumerable<IGrouping<int, SectionsThree>> TowThree = RequestInfo.lb.SectionsThrees.GroupBy(x => x.SectionTwoId.Value);

        public List<String> GetSecOneList(int langId)
        {
            List<string> OneList = new List<string>();
            
            foreach (var _set in RequestInfo.lb.SectionsOnes)
            {
                if (langId != 2)
                {
                    OneList.Add(_set.SectionOneNameRus);
                }
                else
                {
                    OneList.Add(_set.SectionOneNameEng);
                }
            }

            return OneList;
        }

        public List<String> GetSecTowList(string txt, int langId)
        {
            List<string> result = new List<string>();
            SectionsOne One = null;

            if (langId != 2)
            {
                try
                {
                    One = RequestInfo.lb.SectionsOnes.FirstOrDefault(x => x.SectionOneNameRus == txt);
                }
                catch { }
                
            }
            else
            {
                try
                {
                    One = RequestInfo.lb.SectionsOnes.FirstOrDefault(x => x.SectionOneNameEng == txt);
                }
                catch { }            
            }

            if (One != null)
            {
                List<SectionsTwo> Tows = RequestInfo.lb.SectionsTwoes.FindAll(x => x.SectionOneId == One.SectionOneId);

                foreach (var _set in Tows)
                {
                    if (langId != 2)
                    {
                        result.Add(_set.SectionTwoNameRus);
                    }
                    else
                    {
                        result.Add(_set.SectionTwoNameEng);
                    }
                }
            }
            return result;
        }

        public Dictionary<string, string> GetSecThreeList (string one, string tow, int langId)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            SectionsOne One;
            SectionsTwo Tow;

            if (langId != 2)
            {
                One = RequestInfo.lb.SectionsOnes.FirstOrDefault(x => x.SectionOneNameRus == one);
                Tow = RequestInfo.lb.SectionsTwoes.FirstOrDefault(x => x.SectionTwoNameRus == tow);
            }
            else
            {
                One = RequestInfo.lb.SectionsOnes.FirstOrDefault(x => x.SectionOneNameEng == one);
                Tow = RequestInfo.lb.SectionsTwoes.FirstOrDefault(x => x.SectionTwoNameEng == tow);
            }

            string tag = "";
            string txt = "";
            Dictionary<SectionsThree, string> existingSec = null;

            if (One == null && Tow == null)
            {
                foreach(var sec in RequestInfo.lb.SectionsThrees)
                {
                    if (langId != 2)
                    {
                        tag = sec.SectionThreeTagRus;
                        txt = sec.SectionThreeNameRus;
                    }
                    else
                    {
                        tag = sec.SectionThreeTagEng;
                        txt = sec.SectionThreeNameEng;
                    }
                    result.Add(tag, txt);
                }
            }
            else if (One != null && Tow == null)
            {
                var onethree = OneThree.FirstOrDefault(x => x.Key == One.SectionOneId);
                foreach (var sec in onethree)
                {
                    if (langId != 2)
                    {
                        tag = sec.SectionThreeTagRus;
                        txt = sec.SectionThreeNameRus;
                    }
                    else
                    {
                        tag = sec.SectionThreeTagEng;
                        txt = sec.SectionThreeNameEng;
                    }
                    result.Add(tag, txt);
                }
            }
            else
            {
                var towthree = TowThree.FirstOrDefault(x => x.Key == Tow.SectionTwoId);
                foreach (var sec in towthree)
                {
                    if (langId != 2)
                    {
                        tag = sec.SectionThreeTagRus;
                        txt = sec.SectionThreeNameRus;
                    }
                    else
                    {
                        tag = sec.SectionThreeTagEng;
                        txt = sec.SectionThreeNameEng;
                    }
                    result.Add(tag, txt);
                }
            }

            return result;
        }

        public ScheduleItem CtreateScheduleItem(string name, int langId, ScheduleItem existingItem = null)
        {
            ScheduleItem result = null;
            SectionsThree section = null;
            string sectionname = "";
            string tag = "";

            if (langId != 2)
            {
                section = RequestInfo.lb.SectionsThrees.FirstOrDefault(x => x.SectionThreeNameRus == name);
                if (section != null)
                {
                    sectionname = section.SectionThreeNameRus;
                    tag = section.SectionThreeTagRus;
                }
            }
            else
            {
                section = RequestInfo.lb.SectionsThrees.FirstOrDefault(x => x.SectionThreeNameEng == name);
                if (section != null)
                {
                    sectionname = section.SectionThreeNameEng;
                    tag = section.SectionThreeTagEng;
                }
            }

            if (section != null)
            {
                result = new ScheduleItem();
                if (existingItem != null)
                {
                    result.PositionCode = existingItem.PositionCode;
                }
                else
                {
                    if (GlobalData.SelectedPosition != null)
                    {
                        result.PositionCode = GlobalData.SelectedPosition.PositionCode; 
                    }
                    else if (GlobalData.SelectedProduct != null)
                    {
                        result.PositionCode = GlobalData.SelectedProduct.ProductCode;
                    }
                    else if (GlobalData.SelectedTechSolution != null)
                    {
                        result.PositionCode = GlobalData.SelectedTechSolution.TechSolutionCode;
                    }
                }
                result.SecOneId = section.SectionOneId.Value;
                result.SecThreeId = section.SectionThreeId;
                result.SecThreeName = sectionname;
                result.SecThreeNum = section.SectionThreeNum;
                result.SecThreeTag = tag;
            }

            return result;
        }

        public void GetNewScheduleItemIndex(ScheduleItem item, List<ScheduleItem>items)
        {
            int index = 0;
            int num = 0;
            try
            {
                var indexes = items.Where(x => x.SecThreeTag == item.SecThreeTag).Select(i => i.SecThreePostfix);
                foreach(var ind in indexes)
                {
                    int i = 0;
                    try
                    {
                       i = Convert.ToInt32(ind);
                    }
                    catch
                    {
                        i = 0;
                    }
                    if (i > index) index = i;
                    num++;
                }

            }
            catch 
            {
            }
            if (num > 0)
            {
                item.SecThreePostfix = (index + 1).ToString();
            }
        }
    }
}
