using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDB.DB;

namespace UST_ProjectManagement
{
    public class ProductInfo
    {
        public int ID;

        public List<StageProduct> pStages = new List<StageProduct>();

        public StageProduct pStage;

        public List<SectionsProduct> pSections = new List<SectionsProduct>();

        public int LanguageId = 3;

        public string Code = "-";
        public string StageTag;
        public string Name;
        public string Responsible = "-";
        public string StartDate;
        public string EndDate;
        public string PersentComplete = "-";

        List<SectionsProduct> SectionsProduct = new List<SectionsProduct>();
        public Dictionary<SectionsThree, SectionsProduct> SectionsDict = new Dictionary<SectionsThree, SectionsProduct>();
        public List<SectionsThree> ListSecThree = new List<SectionsThree>();

        public ProductInfo(Product product, Stage stage)
        {
            Code = product.ProductCode;
            ID = product.ProductId;
            pStages = RequestInfo.lb.StageProducts.FindAll(x => x.ProductId == product.ProductId);
            

            pStage = pStages.FirstOrDefault(x => x.StageId == stage.StageId);


            pSections = RequestInfo.lb.SectionsProducts.Where(x => x.ProductId == product.ProductId).ToList();

            LanguageId = stage.LanguageId.Value;

            SectionsProduct = RequestInfo.lb.SectionsProducts.FindAll(x => x.ProductId == product.ProductId).ToList();

            foreach (var secP in SectionsProduct)
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
            User user = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId == product.UserId);
            Responsible = $"{user.UserSurname} {user.UserName} {user.UserMidlName}"; 
            Name = product.ProductFullName;
            StartDate = product.ProductDataStart;
            EndDate = product.ProductDataEnd;
        }

        public List<SectionsThree> GetSecThreeList()
        {
            List<SectionsThree> result = RequestInfo.lb.SectionsThrees.Where(y => (pSections.Select(x => x.SectionThreeId).ToList()).Contains(y.SectionThreeId)).ToList();

            return result;
        }


    }

}
