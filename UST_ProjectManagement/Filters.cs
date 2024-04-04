using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UST_ProjectManagement
{
    public static class Filters
    {
        /// <summary>
        /// Наборы фильтров для каждой вкладки
        /// </summary>
        public static Dictionary<int, List<string>> filtersSets = new Dictionary<int, List<string>>()
        {
            {0, new List<string>(){ "Шифр", "Стадия", "ГИП", "ГАП", "% выполнения", "Тип" } },
            {1, new List<string>(){ "Шифр", "Стадия", "ГИП" } },
            {2, new List<string>(){ "Шифр", "Стадия", "ГИП"} },
            {3, new List<string>(){ "Шифр", "Стадия", "От раздела", "Для раздела", "Выдал", "Получил", "Статус" } },
            {4, new List<string>(){ "Шифр", "Автор", "Тип" } },
        };
        /// <summary>
        /// Сопоставление фильтров со столбцами таблицы по каждой вкладке
        /// </summary>
        public static Dictionary<int, Dictionary<int, int>> filterColumns = new Dictionary<int, Dictionary<int, int>>()
        {
            {0, new Dictionary<int, int>(){{0, 0}, {1, 2}, { 2, 3}, { 3, 4 }, {4, 7 }, { 5, 8 } } },
            {1, new Dictionary<int, int>(){{0, 0}, {1, 2}, { 2, 3} } },
            {2, new Dictionary<int, int>(){{0, 0}, {1, 2}, { 2, 3} } },
            {3, new Dictionary<int, int>(){{0, 0}, {1, 1}, { 2, 4}, { 3, 5 }, {4, 6 }, { 5, 7 }, { 6, 8 } } },
            {4, new Dictionary<int, int>(){{0, 0}, {1, 3}, { 2, 6} } }
        };
        /// <summary>
        /// Списки элементов для фильтрации с указанием номера фильтра для каждой вкладки
        /// Формируется при генерации таблицы
        /// </summary>
        public static Dictionary<int, Dictionary<int, List<string>>> filterItems = new Dictionary<int, Dictionary<int, List<string>>>()
        {
            {0, new Dictionary<int, List<string>>() },
            {1, new Dictionary<int, List<string>>() },
            {2, new Dictionary<int, List<string>>() },
            {3, new Dictionary<int, List<string>>() },
            {4, new Dictionary<int, List<string>>() }
        };
        /// <summary>
        /// Заданные фильтры по каждой вкладке
        /// Заполняются по команде SelectIndexChanged для каждого фильтра
        /// </summary>
        public static Dictionary<int, Dictionary<int, string>> filters = new Dictionary<int, Dictionary<int, string>>()
        {
            {0, new Dictionary<int, string>() },
            {1, new Dictionary<int, string>() },
            {2, new Dictionary<int, string>() },
            {3, new Dictionary<int, string>() },
            {4, new Dictionary<int, string>() }
        };

       
    }
}
