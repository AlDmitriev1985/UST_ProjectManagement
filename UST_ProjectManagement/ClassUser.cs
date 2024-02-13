using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public class ClassUser
    {
        public string ID { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Midlname { get; set; }
        public string FullName { get; set; }
        public string Function { get; set; }
        public string Account { get; set; }

        public ClassUser (string Id, string Surname, string Name, string Midlname)
        {
            this.ID = Id;
            this.Surname = Surname;
            this.Name = Name;
            this.Midlname = Midlname;
            FullName = Surname + " " + Name + " " + Midlname;
        }

        public override bool Equals(object obj)
        {
            ClassUser other = obj as ClassUser;
            if (other == null)
            {
                return false;
            }
            else
            {
                if (this.ID == other.ID &&
                    this.Surname == other.Surname &&
                    this.Name == other.Name &&
                    this.Midlname == other.Midlname &&
                    this.Function == other.Function)
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
