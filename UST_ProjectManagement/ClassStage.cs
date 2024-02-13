using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public class ClassStage
    {
        public string Id { get; set; }
        public string Tag { get; set; }
        public string Index { get; set; }
        public string ShortPath { get; set; }
        public string ParantProjectID { get; set; }
        public string ParantProductID { get; set; }

        /// <summary>
        /// Список разделов
        /// </summary>
        public List<ClassSet> SetsList = new List<ClassSet>();
        /// <summary>
        /// Состав проекта утвержден
        /// </summary>
        public bool SetListApproved { get; set; } = true;
        /// <summary>
        /// В состав проекта вносятся изменения
        /// </summary>
        public bool SetListeInRelease { get; set; } = false;

        public int SetListeReleaseNo { get; set; } = 0;

        public ClassStage(string ShortPath)
        {
            this.ShortPath = ShortPath;
        }

        public override bool Equals(object obj)
        {
            ClassStage other = obj as ClassStage;
            if (other == null)
            {
                return false;
            }
            else
            {
                if (this.Tag == other.Tag &&
                    this.Index == other.Index &&
                    this.ShortPath == other.ShortPath &&
                    this.ParantProjectID == other.ParantProjectID &&
                    this.ParantProductID == other.ParantProductID)
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

        public ClassStage Dublicate()
        {
            ClassStage result = new ClassStage(this.ShortPath);
            result.Id = this.Id;
            result.Tag = this.Tag;
            result.Index = this.Index;
            result.ParantProjectID = this.ParantProjectID;
            result.ParantProductID = this.ParantProductID;
            result.SetsList = this.SetsList;
            result.SetListApproved = this.SetListApproved;
            result.SetListeInRelease = this.SetListeInRelease;

            return result;
        }
    }
}
