using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSTServer.History;

namespace UST_ProjectManagement
{
    public class ClassPosition
    {
        /// <summary>
        /// Шифр позиции
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Id позиции в SQL
        /// </summary>
        public string pId { get; set; }
        /// <summary>
        /// Номер по ГП
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Наименование здания/сооружения
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Путь
        /// </summary>
        public string ShortPath { get; set; }
        /// <summary>
        /// Номер родительского проекта
        /// </summary>
        public string ParantProjectID { get; set; }
        /// <summary>
        /// Тег родительской стадии
        /// </summary>
        public string ParantStageTag { get; set; }
        /// <summary>
        /// ГИП
        /// </summary>
        public string prjGIP { get; set; }
        /// <summary>
        /// ГАП
        /// </summary>
        public string prjGAP { get; set; }
        /// <summary>
        /// Дата начала проекта
        /// </summary>
        public string prjStartDate { get; set; }
        /// <summary>
        /// Дата завершения проекта
        /// </summary>
        public string prjEndDate { get; set; }
        /// <summary>
        /// Список разделов
        /// </summary>
        public List<ClassSet> SetsList = new List<ClassSet>();
        /// <summary>
        /// ПРоцент выполнения по каждому подразделу.В Navisworks
        /// </summary>
        public Dictionary<string, int> SetCompletePersent = new Dictionary<string, int>();

        public List<ClassSubsetInfo> SubSetInfoList = new List<ClassSubsetInfo>();
        /// <summary>
        /// Состав проекта утвержден
        /// </summary>
        public bool SetListApproved { get; set; } = true;
        /// <summary>
        /// В состав проекта вносятся изменения
        /// </summary>
        public bool SetListeInRelease { get; set; } = false;
        /// <summary>
        /// Номер ревизии
        /// </summary>
        public int SetListeReleaseNo { get; set; } = 0;
        /// <summary>
        /// Координаты
        /// </summary>
        public string Coordinates { get; set; }

        public HistoryLog history {get; set;}

        public HistoryLog historyBIM { get; set; }

        public ClassPosition(string ShortPath)
        {
            this.ShortPath = ShortPath;
        }

        //public override bool Equals(object obj)
        //{
        //    ClassStage other = obj as ClassStage;
        //    if (other != null)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        if (this.Tag == other.Tag &&
        //            this.Index == other.Index &&
        //            this.ShortPath == other.ShortPath &&
        //            this.ParantProjectID == other.ParantProjectID)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}
    }
}
