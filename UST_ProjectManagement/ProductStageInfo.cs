using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDB.DB;

namespace UST_ProjectManagement
{
    public class ProductStageInfo
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

        List<SectionsProduct> SectionsProducts = new List<SectionsProduct>();
        public Dictionary<SectionsThree, SectionsProduct> SectionsDict = new Dictionary<SectionsThree, SectionsProduct>();
        public Dictionary<string, SectionsThree> SecThreeDict = new Dictionary<string, SectionsThree>();
        public List<SectionsThree> ListSecThree = new List<SectionsThree>();
        public List<ScheduleItem> scheduleItems = new List<ScheduleItem>();

        public ProductStageInfo(Product product, Stage stage)
        {
            Code = product.ProductCode;
            ID = product.ProductId;
            StageTag = stage.StageTag;
            StageId = stage.StageId;
            ProductName = product.ProductFullName;
            Discription = product.ProductDescription;
            if (Discription == "") Discription = "-";
            LanguageId = stage.LanguageId.Value;

            User gip = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId == product.UserId);
            if (gip != null) GIP = $"{gip.UserSurname} {gip.UserName} {gip.UserMidlName}";

            SectionsProducts = RequestInfo.lb.SectionsProducts.FindAll(x => x.ProductId == product.ProductId).ToList();
            foreach(var secP in SectionsProducts)
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
            if (SectionsTxt != null && SectionsTxt != "")
            {
                SectionsTxt = SectionsTxt.TrimEnd();
                SectionsTxt = SectionsTxt.TrimEnd(new char[] { ',', ','});
            }
            else
            {
                SectionsTxt = "-";
            }


            if (SectionsProducts.Count() > 0)
            {
                PersentComplete = (SectionsProducts.Sum(x => x.SectionProgress) / SectionsProducts.Count()).ToString() + "%";
            }
            else
            {
                PersentComplete = "0%";
            }

            if (SectionsProducts.Count > 0) SetListeInRelease = true;
            else SetListeInRelease = false;
        }
    }
}
