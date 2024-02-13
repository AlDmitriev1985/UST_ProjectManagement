using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public class ClassPositionSet: ClassSet
    {
        public string StageId { get; set; }
        /// <summary>
        /// Шифр позиции
        /// </summary>
        public string ParantPositionID { get; set; }
        /// <summary>
        /// Тег родительской стадии
        /// </summary>
        public string ParantStageTag { get; set; }

        public double PersentComplete { get; set; } = 0;

        public ClassPositionSet(string txt) : base (txt)
        {
            StageId = txt;
        }

    }
}
