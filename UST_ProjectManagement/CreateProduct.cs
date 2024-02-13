using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public class CreateProduct
    {
        public byte Type { get; set; } = 0;

        public int ProductGroupId { get; set; }

        public int UserId { get; set; }

        public string ProductCode { get; set; }

        public string ProductGroupCode { get; set; }

        public string ProductFullName { get; set; }

        public string ProductDescription { get; set; }

        public string ProductDataStart { get; set; }

        public string ProductDataEnd { get; set; }

        public string ProductAuthor { get; set; }

      
       public CreateProduct ()
        {

        }

    }
}
