using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public class ClassStageType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Index { get; set; }

        public string Lable { get; set; }

        public ClassStageType (string stageId, string tag, string name, string index)
        {
            Id = stageId;
            Tag = tag;
            Name = name;
            Index = index;
            Lable = index + "_" + Tag;
        }
    }
}
