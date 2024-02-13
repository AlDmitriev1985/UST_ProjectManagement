using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public class ClassPrductCatalog
    {
        /// <summary>
        /// Шифр проекта
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Id позиции в SQL
        /// </summary>
        public string pId { get; set; }

        /// <summary>
        /// Наименование категории
        /// </summary>
        public string Name { get; set; } = "Автономные объекты ПГС";

        /// <summary>
        /// Описание
        /// </summary>
        public string Discriptions { get; set; }

        /// <summary>
        /// Ответственный
        /// </summary>
        public string Responsieble { get; set; } = "Кошелев Алексей Геннадьевич";

        /// <summary>
        /// Путь к папке
        /// </summary>
        public string DirPath { get; set; }

        /// <summary>
        /// Сокращенный путь (без страны)
        /// </summary>
        public string ShortDirPath { get; set; }


        public List<ClassProduct> prodList = new List<ClassProduct>();

        public ClassPrductCatalog(string id)
        {
            ID = id;
        }

        public override bool Equals(object obj)
        {
            ClassPrductCatalog other = obj as ClassPrductCatalog;
            if (other == null)
            {
                return false;
            }
            else
            {
                if (this.ID == other.ID)
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
