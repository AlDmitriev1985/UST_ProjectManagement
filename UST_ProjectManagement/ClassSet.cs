using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public class ClassSet
    {
        public string SetId { get; set; }
        public string Language { get; set; } = "Рус";
        public string SetName
        {
            get
            {
                if (Language == "Рус")
                {
                    return SetNameRus;
                }
                else
                {
                    return SetNameEng;
                }
            }
            set
            {
                if (Language == "Рус")
                {
                    SetNameRus = value;
                }
                else
                {
                    SetNameEng = value;
                }
            }
        }
        public string SetGroupId { get; set; }
        public string SetGroupName
        {
            get
            {
                if (Language == "Рус")
                {
                    return SetGroupNameRus;
                }
                else
                {
                    return SetGroupNameEng;
                }
            }
            set
            {
                if (Language == "Рус")
                {
                    SetGroupNameRus = value;
                }
                else
                {
                    SetGroupNameEng = value;
                }
            }
        }
        public string SubSetId { get; set; }
        public string SubSetName
        {
            get
            {
                if (Language == "Рус")
                {
                    return SubSetNameRus;
                }
                else
                {
                    return SubSetNameEng;
                }
            }
            set
            {
                if (Language == "Рус")
                {
                    SubSetNameRus = value;
                }
                else
                {
                    SubSetNameEng = value;
                }
            }
        }
        public string SubSetTag 
        { 
            get
            {
                if (Language == "Рус")
                {
                    return SubSetTagRus;
                }
                else
                {
                    return SubSetTagEng;
                }
            }
            set
            {
                if (Language == "Рус")
                {
                    SubSetTagRus = value;
                }
                else
                {
                    SubSetTagEng = value;
                }
            }
        }



        string SetNameRus { get; set; }
        string SetNameEng { get; set; }
        
        string SetGroupNameRus { get; set; }
        string SetGroupNameEng { get; set; }
        
        public string SubSetTreeId { get; set; }

        string SubSetNameRus { get; set; }
        string SubSetNameEng { get; set; }

        string SubSetTagRus { get; set; }
        string SubSetTagEng { get; set; }


        public ClassSet(string Id)
        {
            this.SubSetId = SubSetId;
        }

        public override bool Equals(object obj)
        {
            ClassSet other = obj as ClassSet;
            if (other == null)
            {
                return false;
            }
            else
            {
                if (this.SetId == other.SetId &&
                    this.SetGroupId == other.SetGroupId &&
                    this.SubSetId == other.SubSetId)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
