using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public class ClassCountry
    {
        /// <summary>
        /// Шифр проекта
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Полное название проекта
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Сокращенное название проекта
        /// </summary>
        public string ShortName { get; set; }

        public ClassCountry( string id)
        {
            ID = id;
        }
    }
}
