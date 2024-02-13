using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public class ClassProduct
    {
        /// <summary>
        /// Шифр проекта (RYY.XXX)
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Id позиции в SQL
        /// </summary>
        public string pId { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Индекс
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Рписание
        /// </summary>
        public string Discription { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Дата завершения
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Путь к папке
        /// </summary>
        public string DirPath { get; set; }

        /// <summary>
        /// Сокращенный путь (без страны)
        /// </summary>
        public string ShortDirPath { get; set; }

        /// <summary>
        /// ID категории продукта
        /// </summary>
        public string ProdCategoryId { get; set; }

        /// <summary>
        /// Ответственный
        /// </summary>
        public string Responsible {get; set;}

        /// <summary>
        /// Стадия
        /// </summary>
        public string Stage { get; set; }

        public string Language { get; set; } = "Рус";

        public ClassProduct (string id)
        {
            ID = id;
            RefreshValues();
        }

        public void RefreshValues()
        {
            try
            {
                Year = ID.Substring(1, 2);
            }
            catch { }

            try
            {
                string[] fullid = ID.Split('.');
                if (fullid.Length > 1)
                {
                    char[] _index = fullid[1].ToCharArray();
                    string asnumber = "";
                    foreach (char _ch in _index)
                    {
                        if (Char.IsDigit(_ch) && _ch != 0)
                        {
                            asnumber += _ch;
                        }
                    }
                    if (asnumber != "")
                    {
                        Index = Convert.ToInt32(asnumber);
                    }
                }
            }
            catch { }
        }

        public override bool Equals(object obj)
        {
            ClassProduct other = obj as ClassProduct;
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
