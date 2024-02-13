using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UST_ProjectManagement
{
    public class ClassFolder
    {
        public string Name { get; set; }
        public string ParantName { get; set; }
        public string Path { get; set; }
        public string ShortPath { get; set; }

        public List<ClassFolder> subList = new List<ClassFolder>();
        public TreeNode dirNode { get; set; }

        public int Level { get; set; } = 0;

        public ClassFolder (string Name, string ParantName, string Path)
        {
            this.Name = Name;
            this.ParantName = ParantName;
            this.Path = Path;
        }

        public override bool Equals(object obj)
        {
            ClassFolder other = obj as ClassFolder;
            if (other == null)
            {
                return false;
            }
            else
            {
                if (this.Name == other.Name &&
                    this.ParantName == other.ParantName &&
                    this.Path == other.Path &&
                    this.ShortPath == other.ShortPath)
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
