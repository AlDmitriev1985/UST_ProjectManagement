using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDB.DB;

namespace UST_ProjectManagement
{
    public class TechSolutionCatalogInfo
    {
        public int CatalogId;

        public string CatalogCode;

        public string CatalogName;

        public string CatalogResponsble;

        public List<TechSolution> TechSolutions = new List<TechSolution>();

        public TechSolutionCatalogInfo (TechSolutionGroup catalog)
        {
            TechSolutions = RequestInfo.lb.TechSolutions.FindAll(x => x.TechSolutionGroupId == catalog.TechSolutionGroupId);
            CatalogId = catalog.TechSolutionGroupId;
            CatalogCode = catalog.TechSolutionGroupCode;
            CatalogName = "Тех.решения за " + catalog.TechSolutionGroupFullName + " год";

            User Respons = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId == catalog.UserId);
            CatalogResponsble = $"{Respons.UserSurname} {Respons.UserName} {Respons.UserMidlName}";
        }

        public string GetNewSolutionIndex(string year)
        {
            int index = 0;
            List<TechSolution> tGroups = new List<TechSolution>();
            try
            {
                tGroups = TechSolutions.FindAll(x => x.TechSolutionGroupId == GlobalData.SelectedTechSolutionCatalog.TechSolutionGroupId).ToList();
            }
            catch
            {

            }
            int maxid = tGroups.Max(x => x.TechSolutionId);
            TechSolution solution = tGroups.Find(x => x.TechSolutionId == maxid);
            if (solution != null)
            {
                string[] fullcode = solution.TechSolutionCode.Split('.');
                char[] fullindex = fullcode[1].ToCharArray();
                for(int i = 0; i < fullindex.Length; i++)
                {
                    int ii = 0;
                    try
                    {
                        ii = Convert.ToInt32(Convert.ToString(fullindex[i]));
                    }
                    catch
                    {

                    }
                    if (ii > 0)
                    {
                        if(i == 0)
                        {
                            index += ii * 100;
                        }
                        else if (i == 1)
                        {
                            index += ii * 10;
                        }
                        else
                        {
                            index += ii;
                        }
                    }
                }
                index += 1;
                if (index < 10)
                {
                    return "00" + index;
                }
                else if (index < 100)
                {
                    return "0" + index;
                }
                else
                {
                    return index.ToString();
                }
            }
            else
            {
                return "001";
            }

            
            //index = tGroups.Count() + 1;

            
        }
    }
}
