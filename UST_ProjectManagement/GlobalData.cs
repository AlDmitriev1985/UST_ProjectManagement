using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryDB;
using LibraryDB.DB;
using POSTServer.History;

namespace UST_ProjectManagement
{
    public static class GlobalData
    {
        public static User user;

        public static int PrjNaviBtnIndex = 0;

        public static ClassNaviTreeView NaviTreeView;

        public static ClassFolder CreatedFolder;

        public static int X;

        public static int Y;

        public static int Width;

        public static int Height;
        
        public static string loadInfo = "Загрузка...";

        public static bool CloseStart = false;

        //public static List<string> AdminsList = new List<string>();

        public static List<TreeNode> NodeList = new List<TreeNode>();
        
        public static List<ClassFolder> FolderList = new List<ClassFolder>();

        public static List<ClassCountry> CountryList = new List<ClassCountry>();
        
        public static List<ClassProject> ProjectList = new List<ClassProject>();

        public static List<ClassStage> StageList = new List<ClassStage>();

        public static List<ClassPosition> PositionList = new List<ClassPosition>();

        public static List<ClassStageType> StageTypeList = new List<ClassStageType>();

        public static List<ClassPrductCatalog> ProductCategoriesList = new List<ClassPrductCatalog>();

        public static List<ClassProduct> ProductList = new List<ClassProduct>();

        public static Dictionary<string, string> LanguagesList = new Dictionary<string, string>();

        public static string[] Directorys; //Сисок папок для TreeView


        public static List<ClassUser> User_FullList = new List<ClassUser>();

        public static List<string> User_GIPList = new List<string>();

        public static List<string> User_GAPList = new List<string>();


        public static string SelectedMainFolderName;

        public static string SelectedDirPath;
        public static string BuferDirPath;

        public static TreeNode SelectedNode;

        //public static ClassCountry SelectedCountry;
        public static Nation SelectedCountry;

        //public static ClassProject SelectedProject;
        public static Project SelectedProject;

        //public static ClassStage SelectedStage;
        public static Stage SelectedStage;

        public static ClassStage CreatedStage;

        public static bool GIPfolder = false;

        //public static ClassPosition SelectedPosition;
        public static Position SelectedPosition;

        //public static ClassPrductCatalog SelectedProductCatalog;
        public static ProductGroup SelectedProductCatalog;

        //public static ClassProduct SelectedProduct;
        public static Product SelectedProduct;

        public static TechSolutionGroup SelectedTechSolutionCatalog;

        public static TechSolution SelectedTechSolution;

        //public static ClassProduct CreatedProduct;

        //public static List<ClassSet> SetList = new List<ClassSet>();

        /// <summary>
        /// Разрешено/Запрещено создавать Стадии
        /// </summary>
        public static bool CreateStage = false;
        /// <summary>
        /// Разрешено/Запрещено создавать Позиции по ГП
        /// </summary>
        public static bool CreatePosition = false;
        /// <summary>
        /// Разрешено/Запрещено создавать Разделы
        /// </summary>
        public static bool CreateSet = false;
        /// <summary>
        /// Разрешено/Запрещено утверждать Состав проекта
        /// </summary>
        public static bool ApproveSetList = false;
        /// <summary>
        /// Если Администратор, то true иначе false
        /// </summary>
        public static string UserRole = "User";///Admin, Manager
        ///// <summary>
        ///// Имя пользователя
        ///// </summary>
        //public static string UserName;
        ///// <summary>
        ///// Id пользователя
        ///// </summary>
        //public static string UserId;
        /// <summary>
        /// Номер открытой панели: 0 - Стартовая; 1 - поиск; 2 - Проекты; 3 - админка; 4 - Navis
        /// </summary>
        public static byte OpenPanelIndex;
        /// <summary>
        /// Списока зарелиженных разделов
        /// </summary>
        public static List<ClassSet> ReleasedSetList = new List<ClassSet>();


        public static CreateProduct newProduct = new CreateProduct();
        /// <summary>
        /// Список разделов досупных пользователю
        /// </summary>
        public static List<string> UserSets = new List<string>();

        public static List<string> HeadOfDepList = new List<string>();

        public static List<string> Statuses = new List<string>() { "Выполнено", "В работе", "На проверке", "На проверке BIM", "Проверено", "Подготовка DWF" };

        public static string Comment;

        //public static List<SectionsThree> addSubSetList = new List<SectionsThree>();

        public static List<ScheduleItem> addSubSetList = new List<ScheduleItem>();

        public static HistoryLog historyLog;

        public static List<string[,]> FilterList = new List<string[,]>();

        ///public static string[,] filters1;
        ///
        public static string TempDirPath = Convert.ToString(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\UST_Temp\");

        public static string FiltersFileName = "filters.txt";

        public static List<string> Filters0 = new List<string>() { "Страна", "Шифр", "Стадия", "Позиция по ГП", "ГИП", "ГАП" };
        public static List<string> Filters3 = new List<string>() { "Страна", "Шифр", "Стадия", "Позиция по ГП", "От раздела", "Для раздела" };
    }
}
