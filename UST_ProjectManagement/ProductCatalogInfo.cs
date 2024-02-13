using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryDB.DB;

namespace UST_ProjectManagement
{
    public class ProductCatalogInfo
    {
        public int CatalogId;

        public string CatalogCode;

        public string CatalogName;

        public string CatalogResponsble;

        public List<Product> Products = new List<Product>();

        public ProductCatalogInfo (ProductGroup catalog)
        {
            Products = RequestInfo.lb.Products.FindAll(x => x.ProductGroupId == catalog.ProductGroupId);
            CatalogId = catalog.ProductGroupId;
            CatalogCode = catalog.ProductGroupCode;
            CatalogName = catalog.ProductGroupFullName;

            User Respons = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId == catalog.UserId);
            CatalogResponsble = $"{Respons.UserSurname} {Respons.UserName} {Respons.UserMidlName}";
        }

        public string GetNewProductIndex(string year)
        {
            int index = 0;
            List<Product> pGroups = Products.FindAll(x => x.ProductCode.Split('-')[1].Split('.')[0] == "R" + year).ToList();
            index = pGroups.Count() + 1;

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
    }
}
