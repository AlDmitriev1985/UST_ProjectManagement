using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public class SectionsProduct
    {
        public string Path { get; set; }

        public int ProductId { get; set; }

        public int StageId { get; set; }

        public int UserId { get; set; }

        public bool Rus { get; set; }

        public byte Mode { get; set; }

        public List<Section> spSection = new List<Section>();
    }
}
