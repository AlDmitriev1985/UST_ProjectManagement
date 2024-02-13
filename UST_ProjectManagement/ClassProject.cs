using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public class ClassProject
    {
        /// <summary>
        /// Шифр проекта
        /// </summary>
        public string prjID { get; set; }
        /// <summary>
        /// Полное название проекта
        /// </summary>
        public string prjFullName { get; set; }
        /// <summary>
        /// Сокращенное название проекта
        /// </summary>
        public string prjShortName { get; set; }
        /// <summary>
        /// Шифр родительского проекта
        /// </summary>
        public string prjLinkId { get; set; }
        /// <summary>
        /// Название родительского проекта
        /// </summary>
        public string prjLinkName { get; set; }
        /// <summary>
        /// Стадия проектирования
        /// </summary>
        public string prjStage { get; set; }
        /// <summary>
        /// ГИП
        /// </summary>
        public string prjResponsieble { get; set; }
        /// <summary>
        /// Дата начала проекта
        /// </summary>
        public string prjStartDate { get; set; }
        /// <summary>
        /// Дата завершения проекта
        /// </summary>
        public string prjEndDate { get; set; }
        /// <summary>
        /// Очередь строительства
        /// </summary>
        public string prjRound { get; set; }
        /// <summary>
        /// Страна строительства
        /// </summary>
        public string prjCountry { get; set; }
        /// <summary>
        /// Код страны
        /// </summary>
        public string prjCountryID { get; set; }
        /// <summary>
        /// Короткое название страны
        /// </summary>
        public string prjCountryShortName { get; set; }
        /// <summary>
        /// Описание проекта
        /// </summary>
        public string prjDiscription { get; set; }

        /// <summary>
        /// Дата публикации проекта
        /// </summary>
        public string prjReleaseDate { get; set; }

        /// <summary>
        /// Язык проекта
        /// </summary>
        public string prjLanguage { get; set; } = "-";

        /// <summary>
        /// Путь к папке
        /// </summary>
        public string prjDirPath { get; set; }

        /// <summary>
        /// Сокращенный путь (без страны)
        /// </summary>
        public string prjShortDirPath { get; set; }




        public List<ClassProject> prjPositions = new List<ClassProject>();

        public ClassProject(string _prjNumber)
        {
            //prjNumber = _prjNumber;
            prjID = _prjNumber;

        }
    }
}
