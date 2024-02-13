using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSTServer
{
    public class SectionProgress
    {
        public int Progress { get; set; }

        public int PositionId { get; set; }

        public int SectionThreeId { get; set; }
        /// <summary>
        /// 0 - Выпущено
        /// 1 - В работе
        /// 2 - На проверке
        /// 3 - На проверке в BIM отделе
        /// 4 - Проверено
        /// </summary>
        public int Status { get; set; }

        public string Description { get; set; } = "";
    }
}
