using Newtonsoft.Json;
using POSTServer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using POSTServer.History;
using LibraryDB;
using LibraryDB.DB;
using System.Drawing;

namespace UST_ProjectManagement
{
    public static class GlobalMethodes
    {
        public static bool _stop = false;

        public static CreateNWF cNWF;

        /// <summary>
        /// Сохранение Настроек
        /// </summary>
        public static bool SaveToJSON(string dirpath, string filename, string txt)
        {
            if (!System.IO.Directory.Exists(dirpath)) System.IO.Directory.CreateDirectory(dirpath);
            string filepath = dirpath + filename;

            if (filepath != null && filepath != "")
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    // преобразуем строку в байты
                    byte[] array = System.Text.Encoding.UTF8.GetBytes(txt);
                    // запись массива байтов в файл
                    fs.Write(array, 0, array.Length);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Считывает текст для десириализации в Json
        /// </summary>
        public static string ReadFromJSON(string dirpath, string filename)
        {
            string path = dirpath + filename;
            string txt = null;
            if (File.Exists(path))
            {

                using (FileStream fs = File.OpenRead(path))
                {
                    byte[] array = new byte[fs.Length];
                    fs.Read(array, 0, array.Length);
                    txt = System.Text.Encoding.UTF8.GetString(array);
                }
            }
            return txt;
        }

        #region --- OldCode ---
        /// <summary>
        /// Чтение из базы данных
        /// </summary>
        /// <returns></returns>
        //public static void ReadSQL()
        //{
        //    using (SqlConnection connection = new SqlConnection(MainForm.ConnectionString))
        //    {
        //        // Подключение открыто
        //        connection.Open();
        //        SqlCommand command = new SqlCommand();

        //        command.CommandText = "SELECT Projects.ProjectId, Projects.ProjectName, Projects.ProjectFullName, Stages.StageTag, " +
        //                              "Projects.ProjectDataStart, Projects.ProjectDataEnd, Projects.ProjectDataRelease, Projects.ProjectGIP, " +
        //                              "Nations.NationId, Nations.NationFullName, Nations.NationName, Projects.ProjectLinkId, " +
        //                              "Languages.LanguageTag, Stages.StageIndex, Stages.StageId " +
        //                              "FROM StageProjects sPrj " +
        //                              "JOIN Projects ON sPrj.ProjectId = Projects.ProjectId " +
        //                              "JOIN Nations ON Projects.NationId = Nations.NationId " +
        //                              "JOIN Stages ON Stages.StageId = sPrj.StageId " +
        //                              "JOIN Languages ON Languages.LanguageId = Stages.LanguageId";

        //        ;

        //        command.Connection = connection;
        //        GlobalData.ProjectList.Clear();
        //        GlobalData.StageList.Clear();

        //        SqlDataReader reader = command.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                ClassProject project = new ClassProject(reader.GetValue(0).ToString());
        //                project.prjShortName = reader.GetValue(1).ToString();
        //                project.prjFullName = reader.GetValue(2).ToString();
        //                project.prjStage = reader.GetValue(3).ToString();
        //                project.prjStartDate = reader.GetValue(4).ToString();
        //                project.prjEndDate = reader.GetValue(5).ToString();
        //                project.prjReleaseDate = reader.GetValue(6).ToString();
        //                project.prjResponsieble = reader.GetValue(7).ToString();
        //                project.prjCountry = reader.GetValue(9).ToString();
        //                project.prjCountryID = reader.GetValue(8).ToString();
        //                project.prjCountryShortName = reader.GetValue(10).ToString();
        //                project.prjLanguage = reader.GetValue(12).ToString();

        //                object value = reader.GetValue(11);
        //                if ((Type.GetTypeCode(value.GetType()) != TypeCode.DBNull))
        //                {
        //                    project.prjLinkId = reader.GetValue(11).ToString();
        //                }
        //                else
        //                {
        //                    project.prjLinkId = "";
        //                }
        //                project.prjShortDirPath = reader.GetValue(0).ToString() + "\\";

        //                GlobalData.ProjectList.Add(project);

        //                /////УДАЛИТЬ///
        //                //if(!GlobalData.GIPList.Contains(project.prjResponsieble))
        //                //{
        //                //    GlobalData.GIPList.Add(project.prjResponsieble);
        //                //}
        //                /////УДАЛИТЬ///

        //                if (reader.GetValue(3).ToString() != "-")
        //                {
        //                    string StageShortPath = project.prjShortDirPath + reader.GetValue(13).ToString() +"_"+ reader.GetValue(3).ToString() + "\\";
        //                    ClassStage _Stage = new ClassStage(StageShortPath);
        //                    _Stage.Id = reader.GetValue(14).ToString();
        //                    _Stage.Tag = reader.GetValue(3).ToString();
        //                    _Stage.Index = reader.GetValue(13).ToString();
        //                    _Stage.ParantProjectID = project.prjID;

        //                    if (!GlobalData.StageList.Contains(_Stage) && StageShortPath !="")
        //                    {
        //                        GlobalData.StageList.Add(_Stage);
        //                    }

        //                }

        //            }
        //        }
        //        //MessageBox.Show(GlobalData.StageList.Count.ToString());
        //        reader.Close();
        //    }
        //    foreach (ClassProject _project in GlobalData.ProjectList)
        //    {
        //        if (_project.prjLinkId != "")
        //        {
        //            foreach (ClassProject _link in GlobalData.ProjectList)
        //            {
        //                if (_link.prjID == _project.prjLinkId)
        //                {
        //                    _link.prjPositions.Add(_project);
        //                }
        //            }
        //        }
        //    }
        //    //return result;
        //}

        /// <summary>
        /// Чтение из базы данных списка позиций по ГП
        /// </summary>
        //public static List<ClassPosition> ReadSQL_Positions()
        //{
        //    List<ClassPosition> result = new List<ClassPosition>();

        //    using (SqlConnection connection = new SqlConnection(MainForm.ConnectionString))
        //    {
        //        // Подключение открыто
        //        connection.Open();

        //        SqlCommand command = new SqlCommand();

        //        command.CommandText = string.Format("SELECT us.UserId, us.UserSurname, us.UserName, us.UserMidlName, " +
        //            "us2.UserId, us2.UserSurname, us2.UserName, us2.UserMidlName, ProjectId, p.PositionCode, PositionName, " +
        //            "PositionPos, _stage.StageTag, _stage.StageIndex, PositionDataStart, PositionDataEnd, p.PositionId, p.PositionCoordinate, " +
        //            "p.PositionCoordinateHistory, p.PositionCoordinateHistoryBIM FROM Positions p " +
        //            "JOIN Users us on us.Userid = PositionUserIdGIP " +
        //            "JOIN Users us2 on us2.Userid = PositionUserIdGAP " +
        //            "JOIN Stages _stage on _stage.StageId = p.StageId");

        //        command.Connection = connection;

        //        SqlDataReader reader = command.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                //string[] splitPos = reader.GetValue(9).ToString().Split('|');
        //                string ShortPath = reader.GetValue(8).ToString() + @"\" + reader.GetValue(13).ToString() + "_" + 
        //                    reader.GetValue(12).ToString() + @"\" + reader.GetValue(9).ToString() + @"\";
        //                ClassPosition _position = new ClassPosition(ShortPath);
        //                _position.ID = reader.GetValue(9).ToString();
        //                _position.Number = reader.GetValue(11).ToString();
        //                _position.Name = reader.GetValue(10).ToString();
        //                _position.ParantProjectID = reader.GetValue(8).ToString();
        //                _position.ParantStageTag = reader.GetValue(12).ToString();
        //                _position.prjGIP = reader.GetValue(1).ToString() + " " + reader.GetValue(2).ToString() + " " + reader.GetValue(3).ToString();
        //                _position.prjGAP = reader.GetValue(5).ToString() + " " + reader.GetValue(6).ToString() + " " + reader.GetValue(7).ToString();
        //                _position.prjStartDate = reader.GetValue(14).ToString();
        //                _position.prjEndDate = reader.GetValue(15).ToString();
        //                _position.pId = reader.GetValue(16).ToString();
        //                _position.Coordinates = reader.GetValue(17).ToString();
        //                try
        //                {
        //                    _position.history = JsonConvert.DeserializeObject<HistoryLog>(reader.GetValue(18).ToString());
        //                }
        //                catch
        //                {
        //                    _position.history = null;
        //                }
        //                try
        //                {
        //                    _position.historyBIM = JsonConvert.DeserializeObject<HistoryLog>(reader.GetValue(19).ToString());
        //                }
        //                catch
        //                {
        //                    _position.historyBIM = null;
        //                }

        //                result.Add(_position);
        //                //MessageBox.Show(_position.ShortPath);
        //            }
        //        }
        //        reader.Close();
        //    }
        //    return result;
        //}

        /// <summary>
        /// Чтение из базы данных списка позиций по ГП
        /// </summary>
        //public static List<ClassSet> ReadSQL_GenDiscipline()
        //{
        //    List<ClassSet> result = new List<ClassSet>();

        //    using (SqlConnection connection = new SqlConnection(MainForm.ConnectionString))
        //    {
        //        // Подключение открыто
        //        connection.Open();

        //        SqlCommand command = new SqlCommand();

        //        command.CommandText = string.Format("SELECT s1.SectionOneNum, s1.SectionOneNameRus, s1.SectionOneNameEng, " +
        //            "s2.SectionTwoNum, s2.SectionTwoNameRus, s2.SectionTwoNameEng, s3.SectionThreeNum, s3.SectionThreeNameRus, " +
        //            "s3.SectionThreeNameEng, s3.SectionThreeTagRus, s3.SectionThreeTagEng, s3.SectionThreeId FROM SectionsThree s3 " +
        //            "JOIN SectionsOne s1 on s1.SectionOneId = s3.SectionOneId " +
        //            "JOIN SectionsTwo s2 on s2.SectionTwoId = s3.SectionTwoId where s3.SectionThreeId != 7");

        //        command.Connection = connection;

        //        SqlDataReader reader = command.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                ClassSet _discipline = new ClassSet(reader.GetValue(6).ToString());
        //                _discipline.SetId = reader.GetValue(0).ToString();
        //                _discipline.SetGroupId = reader.GetValue(3).ToString();
        //                _discipline.SubSetId = reader.GetValue(6).ToString();
        //                _discipline.SubSetTreeId = reader.GetValue(11).ToString();

        //                _discipline.Language = "Eng";
        //                _discipline.SetName = reader.GetValue(2).ToString();
        //                _discipline.SetGroupName = reader.GetValue(5).ToString();               
        //                _discipline.SubSetName = reader.GetValue(8).ToString();
        //                _discipline.SubSetTag = reader.GetValue(10).ToString();

        //                _discipline.Language = "Рус";
        //                _discipline.SetName = reader.GetValue(1).ToString();
        //                _discipline.SetGroupName = reader.GetValue(4).ToString();
        //                _discipline.SubSetName = reader.GetValue(7).ToString();
        //                _discipline.SubSetTag = reader.GetValue(9).ToString();

        //                result.Add(_discipline);
        //            }
        //        }
        //        reader.Close();
        //    }
        //    return result;
        //}

        /// <summary>
        /// Поиск сетевой папки BIM02
        /// </summary>
        /// <param name="path"></param>
        //public static void FillMainDirectNodes(string path, TreeView view)
        //{
        //    try
        //    {
        //        string[] maindirs = System.IO.Directory.GetDirectories(path);
        //        foreach (string dir in maindirs)
        //        {
        //            TreeNode mainNode = new TreeNode();
        //            mainNode.Text = dir.Remove(0, dir.LastIndexOf("\\") + 1);
        //            view.Nodes.Add(mainNode);
        //            TreeNode tenpNode = new TreeNode();
        //            mainNode.Nodes.Add(tenpNode);
        //            //MessageBox.Show(mainNode.Text);

        //        }
        //    }
        //    catch (Exception ex) 
        //    {
        //        MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
        //    }
        //}

        /// <summary>
        /// Формирование диспетчера проектов
        /// </summary>
        /// <param name="driveNode"></param>
        /// <param name="path"></param>
        //public static void FillTreeNode(TreeNode driveNode, string path)
        //{
        //    try
        //    {
        //        string[] dirs = Directory.GetDirectories(path);
        //        //GlobalData.ProductCategoriesList.Clear();
        //        //GlobalData.ProductList.Clear();

        //        //GlobalData.ProjectList.Clear();
        //        foreach (string dir in dirs)
        //        {
        //            TreeNode dirNode = new TreeNode();
        //            dirNode.Text = dir.Remove(0, dir.LastIndexOf("\\") + 1);
        //            //if (dirNode.Text == "R22.001")
        //            //{
        //            //    MessageBox.Show(":)");
        //            //}

        //            string[] num = dir.Remove(0, dir.LastIndexOf("\\") + 1).Split('_');
        //            dirNode.Tag = num[0];
        //            FillTreeNode(dirNode, dir);
        //            driveNode.Nodes.Add(dirNode);
        //            string nodeName = dirNode.Text.ToString();

        //            string[] subs = dirNode.Text.ToString().Split('_');
        //            string fullpath = path + @"\" + nodeName;
        //            string[] subpath = fullpath.Split('\\');

        //            string Path = "";
        //            for (int i = 3; i < subpath.Length; i++)
        //            {
        //                Path = Path +@"\"+ subpath[i];
        //            }

        //            string[] category = dir.Split('\\');/// Проверка Продукт или Проект
        //            string shortPath = "";
        //            if (category[2] == "02_Продукты")
        //            {
        //                for (int s = 3; s < subpath.Length; s++)
        //                {
        //                    shortPath = shortPath + subpath[s] + @"\";
        //                }
        //            }
        //            else
        //            {
        //                for (int s = 4; s < subpath.Length; s++)
        //                {
        //                    shortPath = shortPath + subpath[s] + @"\";
        //                }
        //            }

        //            if (dirNode.Parent != null)
        //            {
        //                ClassFolder Folder = new ClassFolder(dirNode.Text, dirNode.Parent.Text, Path);
        //                Folder.ShortPath = shortPath;
        //                if (!GlobalData.FolderList.Contains(Folder))
        //                {
        //                    GlobalData.FolderList.Add(Folder);
        //                }
        //            }

        //            dirNode.Tag = shortPath;

        //            if (!GlobalData.NodeList.Contains(dirNode))
        //            {
        //                GlobalData.NodeList.Add(dirNode);
        //            }
        //        }

        //        UpdateProductCategories();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
        //    }

        ////ReadSQL();
        ////foreach (ProjectClass _project in GlobalData.ProjectList)
        ////{
        ////    foreach (FolderClass folder in GlobalData.FolderList)
        ////    {
        ////        if (_project.prjShortDirPath == folder.ShortPath)
        ////        {
        ////            _project.prjDirPath = folder.Path;
        ////            break;
        ////        }
        ////    }
        ////}
        //}

        /// <summary>
        /// Обновление Навигационной панели
        /// </summary>
        //public static void UpdateNavigationTreeView(TreeView view, string path)
        //{

        //    try
        //    {
        //        FillMainDirectNodes(path, view);
        //        //view.Nodes.Clear();

        //        string[] maindirs;
        //        maindirs = Directory.GetDirectories(path);
        //        GlobalData.Directorys = maindirs;

        //        for (int i = 0; i < GlobalData.Directorys.Length; i++)
        //        {
        //            TreeNode dirNode = new TreeNode(new DirectoryInfo(GlobalData.Directorys[i]).Name);
        //            string nodeName = dirNode.Text.ToString();
        //            string[] subs = dirNode.Text.ToString().Split('_');
        //            string fullpath = path + @"\" + nodeName;
        //            string[] subpath = fullpath.Split('\\');
        //            string Path = "";
        //            for (int p = 3; p < subpath.Length; p++)
        //            {
        //                Path = Path + subpath[p] + @"\";
        //            }

        //            string shortPath = "";
        //            for (int s = 4; s < subpath.Length; s++)
        //            {
        //                shortPath = shortPath + subpath[s] + @"\";
        //            }

        //            ClassFolder Folder = null;

        //            if (dirNode.Parent != null)
        //            {
        //                Folder = new ClassFolder(dirNode.Text, dirNode.Parent.Text, Path);
        //            }
        //            else
        //            {
        //                Folder = new ClassFolder(dirNode.Text, "", Path);
        //            }

        //            Folder.ShortPath = shortPath;
        //            Folder.dirNode = dirNode;
        //            Folder.subList = GetSubFolders(path + Path, Folder.Name);
        //            if (!GlobalData.FolderList.Contains(Folder))
        //            {
        //                GlobalData.FolderList.Add(Folder);
        //            }
        //        }

        //    }

        //    catch (Exception Ex)
        //    {
        //        MessageBox.Show(Ex.Message + Environment.NewLine + Ex.StackTrace);
        //    }

        //    CreateTreeView(view);

        //    //ReadSQL();

        //    //GlobalData.PositionList = ReadSQL_Positions();
        //    //ReadSQL_Users();
        //    //ReadSQL_ProductGroup();
        //    //ReadSQL_Product();

        //    //foreach (ClassProject _project in GlobalData.ProjectList)
        //    //{
        //    //    foreach (ClassFolder folder in GlobalData.FolderList)
        //    //    {
        //    //        if (_project.prjShortDirPath == folder.ShortPath)
        //    //        {
        //    //            _project.prjDirPath = folder.Path;
        //    //            break;
        //    //        }
        //    //    }
        //    //}
        //}

        //private static List<ClassFolder> GetSubFolders(string path, string Parent)
        //{
        //    List<ClassFolder> result = new List<ClassFolder>();
        //    string[] maindirs;
        //    try
        //    {
        //        maindirs = Directory.GetDirectories(path);
        //        GlobalData.Directorys = maindirs;

        //        for (int i = 0; i < GlobalData.Directorys.Length; i++)
        //        {
        //            TreeNode dirNode = new TreeNode(new DirectoryInfo(GlobalData.Directorys[i]).Name);
        //            string nodeName = dirNode.Text.ToString();
        //            string[] subs = dirNode.Text.ToString().Split('_');
        //            string fullpath = path + @"\" + nodeName;
        //            string[] subpath = fullpath.Split('\\');
        //            string Path = "";
        //            for (int p = 3; p < subpath.Length; p++)
        //            {
        //                Path = Path + subpath[p] + @"\";
        //            }

        //            string shortPath = "";
        //            for (int s = 4; s < subpath.Length; s++)
        //            {
        //                shortPath = shortPath + subpath[s] + @"\";
        //            }

        //            if (Parent != null)
        //            {
        //                ClassFolder Folder = new ClassFolder(dirNode.Text, Parent, Path);
        //                Folder.ShortPath = shortPath;
        //                Folder.dirNode = dirNode;
        //                result.Add(Folder);
        //            }
        //        }
        //    }
        //    catch { }
        //    return result;
        //}


        //public static void UpdateNavigationTreeView(TreeView view, string path)
        //{

        //    try
        //    {
        //        FillMainDirectNodes(path, view);

        //        //if (GlobalData.SelectedProduct == null) MessageBox.Show("Opssss1");
        //        //else MessageBox.Show("Ok");

        //        view.Nodes.Clear();

        //        //if (GlobalData.SelectedProduct == null) MessageBox.Show("Opssss2");
        //        //else MessageBox.Show("Ok");


        //        string[] maindirs;
        //        maindirs = Directory.GetDirectories(path);
        //        GlobalData.Directorys = maindirs;
        //        //MessageBox.Show(maindirs.Length.ToString());

        //        for (int i = 0; i < GlobalData.Directorys.Length; i++)
        //        {
        //            TreeNode dirNode = new TreeNode(new DirectoryInfo(GlobalData.Directorys[i]).Name);
        //            //dirNode.Tag = new DirectoryInfo(maindirs[i]).Name;
        //            FillTreeNode(dirNode, GlobalData.Directorys[i]);
        //            view.Nodes.Add(dirNode);

        //            string nodeName = dirNode.Text.ToString();
        //            //GlobalData.ProjectList.Clear();
        //            string[] subs = dirNode.Text.ToString().Split('_');
        //            string fullpath = path + @"\" + nodeName;
        //            string[] subpath = fullpath.Split('\\');

        //            string Path = "";
        //            for (int p = 3; p < subpath.Length; p++)
        //            {
        //                Path = Path + subpath[p] + @"\";
        //            }

        //            string shortPath = "";
        //            for (int s = 4; s < subpath.Length; s++)
        //            {
        //                shortPath = shortPath + subpath[s] + @"\";
        //            }

        //            if (dirNode.Parent != null)
        //            {
        //                ClassFolder Folder = new ClassFolder(dirNode.Text, dirNode.Parent.Text, Path);
        //                Folder.ShortPath = shortPath;
        //                if (!GlobalData.FolderList.Contains(Folder))
        //                {
        //                    GlobalData.FolderList.Add(Folder);
        //                }
        //            }

        //            dirNode.Tag = shortPath;

        //            if (!GlobalData.NodeList.Contains(dirNode))
        //            {
        //                GlobalData.NodeList.Add(dirNode);
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        MessageBox.Show(Ex.Message + Environment.NewLine + Ex.StackTrace);
        //    }



        //    ReadSQL();

        //    GlobalData.PositionList = ReadSQL_Positions();
        //    //ReadSQL_Users();
        //    //ReadSQL_ProductGroup();
        //    //ReadSQL_Product();

        //    foreach (ClassProject _project in GlobalData.ProjectList)
        //    {
        //        foreach (ClassFolder folder in GlobalData.FolderList)
        //        {
        //            if (_project.prjShortDirPath == folder.ShortPath)
        //            {
        //                _project.prjDirPath = folder.Path;
        //                break;
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Найти выбранный проект
        /// </summary>
        /// <param name="ProjectNo"></param>
        //public static void SearchSelectedProject(string ProjectNo)
        //{
        //    foreach (Project prj in RequestInfo.lb.Projects)
        //    {
        //        if (prj.ProjectId == ProjectNo)
        //        {
        //            GlobalData.SelectedProject = prj;
        //            break;
        //        }
        //    }
        //}

        /// <summary>
        /// Найти выбранную позицию
        /// </summary>
        /// <param name="ProjectNo"></param>
        //public static void SearchSelectedPosition(string PositionNo)
        //{
        //    //foreach (ClassPosition prj in GlobalData.PositionList)
        //    //{
        //    //    if (prj.ID == PositionNo)
        //    //    {
        //    //        GlobalData.SelectedPosition = prj;
        //    //        break;
        //    //    }
        //    //}
        //}

        /// <summary>
        /// Чтение из базы данных списка стадий
        /// </summary>
        /// <returns></returns>
        //public static List<ClassStageType> ReadSQL_Stages(int LanguegeIndex)
        //{
        //    List<ClassStageType> result = new List<ClassStageType>();
        //    int languege = LanguegeIndex;

        //    if (languege != 0)
        //    {
        //        using (SqlConnection connection = new SqlConnection(MainForm.ConnectionString))
        //        {
        //            // Подключение открыто
        //            connection.Open();

        //            SqlCommand command = new SqlCommand();

        //            command.CommandText = string.Format("SELECT StageTag, StageName, StageIndex, StageId FROM Stages where LanguageId = {0}", languege);

        //            command.Connection = connection;

        //            SqlDataReader reader = command.ExecuteReader();
        //            if (reader.HasRows)
        //            {
        //                while (reader.Read())
        //                {
        //                    ClassStageType stage = new ClassStageType(reader.GetValue(3).ToString(), 
        //                        reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(2).ToString());
        //                    result.Add(stage);
        //                }
        //            }
        //            reader.Close();
        //        }           
        //    }
        //    return result;
        //}

        /// <summary>
        /// Чтение из базы данных списка Языков
        /// </summary>
        /// <returns></returns>
        //public static Dictionary<string, string> ReadSQL_Languages()
        //{
        //    Dictionary<string, string> result = new Dictionary<string, string>();

        //    using (SqlConnection connection = new SqlConnection(MainForm.ConnectionString))
        //    {
        //        // Подключение открыто
        //        connection.Open();

        //        SqlCommand command = new SqlCommand();
        //        command.CommandText = "SELECT LanguageId, LanguageTag FROM Languages";

        //        command.Connection = connection;

        //        SqlDataReader reader = command.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                result.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
        //            }
        //        }
        //        reader.Close();
        //    }
        //    return result;
        //}

        /// <summary>
        /// Чтение из базы данных списка Пользователей
        /// </summary>
        /// <returns></returns>
        //public static List<ClassUser> ReadSQL_Users()
        //{
        //    List<ClassUser> result = new List<ClassUser>();

        //    using (SqlConnection connection = new SqlConnection(MainForm.ConnectionString))
        //    {
        //        // Подключение открыто
        //        connection.Open();

        //        SqlCommand command = new SqlCommand();
        //        //command.CommandText = "SELECT Users.UserSurname, Users.UserName, Users.UserMidlName, Users.UserId, Functions.FunctionTag, UserAccount " +
        //        //    "FROM Users JOIN Functions on Functions.FunctionId = Users.FunctionId where Functions.FunctionTag = 'GIP' " +
        //        //    "or Functions.FunctionTag = 'GAP' or Functions.FunctionTag = 'BIM'";

        //        command.CommandText = "SELECT Users.UserSurname, Users.UserName, Users.UserMidlName, Users.UserId, Functions.FunctionTag, UserAccount " +
        //                            "FROM Users JOIN Functions on Functions.FunctionId = Users.FunctionId";

        //        command.Connection = connection;

        //        SqlDataReader reader = command.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                ClassUser _User = new ClassUser(reader.GetValue(3).ToString(), 
        //                    reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), 
        //                    reader.GetValue(2).ToString());
        //                _User.Midlname = reader.GetValue(2).ToString();
        //                _User.Function = reader.GetValue(4).ToString();
        //                _User.Account = reader.GetValue(5).ToString();
        //                if (!result.Contains(_User))
        //                {
        //                    result.Add(_User);
        //                }
        //                ///MessageBox.Show(_User.Surname + " " + _User.Name + " / " + _User.Account);
        //            }
        //        }
        //        reader.Close();
        //    }
        //    return result;
        //}

        /// <summary>
        /// Чтение разделоав проекта при выборе поз.по ГП
        /// </summary>
        //public static void ReadSQL_PositionSet()
        //{
        //    //List<ClassSet> result = new List<ClassSet>();

        //    //using (SqlConnection connection = new SqlConnection(MainForm.ConnectionString))
        //    //{
        //    //    // Подключение открыто
        //    //    connection.Open();

        //    //    SqlCommand command = new SqlCommand();
        //    //    string pId = GlobalData.SelectedPosition.pId;
        //    //    GlobalData.SelectedPosition.SetsList.Clear();
        //    //    GlobalData.SelectedPosition.SubSetInfoList.Clear();

        //    //    //command.CommandText = string.Format("SELECT _sec.SectionThreeNum, _sec.SectionThreeNameRus, _sec.SectionThreeTagRus, " +
        //    //    //    "_sec.SectionThreeNameEng, _sec.SectionThreeTagEng, _sec2.SectionTwoNum, _sec2.SectionTwoNameRus, " +
        //    //    //    "_sec2.SectionTwoNameEng, sPos.SectionProgress, sPos.StatusId, sPos.StatusHistory FROM SectionsPositions sPos " +
        //    //    //    "JOIN SectionsThree _sec ON sPos.SectionThreeId = _sec.SectionThreeId and sPos.PositionId = {0}" +
        //    //    //    "JOIN SectionsTwo _sec2 ON _sec2.SectionTwoId = _sec.SectionTwoId where _sec.SectionThreeId != 7", pId);

        //    //    command.CommandText = string.Format("SELECT _sec.SectionThreeNum, _sec.SectionThreeNameRus, _sec.SectionThreeTagRus, " +
        //    //        "_sec.SectionThreeNameEng, _sec.SectionThreeTagEng, _sec2.SectionTwoNum, _sec2.SectionTwoNameRus, " +
        //    //        "_sec2.SectionTwoNameEng, sPos.SectionProgress, _status.StatusName, _user.UserAccount, sPos.StatusHistory, _status.StatusId FROM SectionsPositions sPos " +
        //    //        "JOIN SectionsThree _sec ON sPos.SectionThreeId = _sec.SectionThreeId and sPos.PositionId = {0} " +
        //    //        "JOIN SectionsTwo _sec2 ON _sec2.SectionTwoId = _sec.SectionTwoId " +
        //    //        "JOIN Status _status ON _status.StatusId = sPos.StatusId " +
        //    //        "JOIN Departments dep ON dep.DepartmentId = _sec.DepartmentId " +
        //    //        "JOIN Users _user ON _user.UserId = dep.DepartmentHeade " +
        //    //        "where _sec.SectionThreeId != 7", pId);

        //    //    command.Connection = connection;
        //    //    GlobalData.SelectedPosition.SetCompletePersent.Clear();
        //    //    SqlDataReader reader = command.ExecuteReader();

        //    //    if (reader.HasRows)
        //    //    {
        //    //        while (reader.Read())
        //    //        {
        //    //            int p = 0;
        //    //            try
        //    //            {
        //    //                p = Convert.ToInt32(reader.GetValue(8).ToString());
        //    //            }
        //    //            catch
        //    //            {
        //    //                p = 0;
        //    //            }
        //    //            foreach (ClassSet pSet in GlobalData.SetList)
        //    //            {
        //    //                if (pSet.SubSetId == reader.GetValue(0).ToString())
        //    //                {
        //    //                    ClassSubsetInfo ssInfo = new ClassSubsetInfo(pSet.SubSetId);
        //    //                    ssInfo.PercentComplete = p;
        //    //                    ssInfo.Status = reader.GetValue(9).ToString();
        //    //                    ssInfo.Responsible = reader.GetValue(10).ToString();
        //    //                    ssInfo.HeadOfDep = reader.GetValue(10).ToString();
        //    //                    try
        //    //                    {
        //    //                        ssInfo.history = JsonConvert.DeserializeObject<HistoryLog>(reader.GetValue(11).ToString());
        //    //                    }
        //    //                    catch
        //    //                    {
        //    //                        ssInfo.history = null;
        //    //                    }
        //    //                    ssInfo.StatusId = Convert.ToInt32(reader.GetValue(12).ToString());
        //    //                    GlobalData.SelectedPosition.SubSetInfoList.Add(ssInfo);
        //    //                    if (!GlobalData.SelectedPosition.SetsList.Contains(pSet))
        //    //                    {
        //    //                        GlobalData.SelectedPosition.SetsList.Add(pSet);
        //    //                    }
        //    //                    break;
        //    //                }
        //    //            }
        //    //            if (!GlobalData.HeadOfDepList.Contains(reader.GetValue(10).ToString())) GlobalData.HeadOfDepList.Add(reader.GetValue(10).ToString());
        //    //            ///MessageBox.Show(reader.GetValue(0).ToString());

        //    //        }
        //    //    }
        //    //    reader.Close();
        //    //    if (GlobalData.SelectedPosition.SetsList.Count > 0) GlobalData.SelectedPosition.SetListApproved = true;
        //    //}
        //    ////var groups = GlobalData.SelectedPosition.SubSetInfoList.GroupBy(x => x.Status);

        //    ////foreach(var gr in groups)
        //    ////{

        //    ////    foreach(var item in gr)
        //    ////    {
        //    ////        if (item.Status != 2) break;
        //    ////    }
        //    ////}
        //}

        /// <summary>
        /// Чтение разделоав проекта при выборе стадии для Продукта
        /// </summary>
        //public static void ReadSQL_ProdStageSet()
        //{
        //    //List<ClassSet> result = new List<ClassSet>();

        //    //using (SqlConnection connection = new SqlConnection(MainForm.ConnectionString))
        //    //{
        //    //    // Подключение открыто
        //    //    connection.Open();

        //    //    SqlCommand command = new SqlCommand();

        //    //    string pId = GlobalData.SelectedProduct.ProductId.ToString();
        //    //    string sId = GlobalData.SelectedStage.StageId.ToString();
        //    //    //GlobalData.SelectedStage.SetsList.Clear();

        //    //    //command.CommandText = string.Format("SELECT s.SectionThreeNum, s.SectionThreeNameRus, s.SectionThreeTagRus, s.SectionThreeNameEng, " +
        //    //    //    "s.SectionThreeTagEng,_sec2.SectionTwoNum, _sec2.SectionTwoNameRus, _sec2.SectionTwoNameEng from Products " +
        //    //    //    "cross apply string_split(ProductSections, ';')join SectionsThree s on s.SectionThreeTagRus = value " +
        //    //    //    "JOIN SectionsTwo _sec2 ON _sec2.SectionTwoId = s.SectionTwoId where ProductId = {0}", pId);

        //    //    command.CommandText = string.Format("SELECT s.SectionThreeNum, s.SectionThreeNameRus, s.SectionThreeTagRus, s.SectionThreeNameEng, s.SectionThreeTagEng," +
        //    //        "_sec2.SectionTwoNum, _sec2.SectionTwoNameRus, _sec2.SectionTwoNameEng from StageProducts cross apply string_split(StageProductSections, ';') " +
        //    //        "JOIN SectionsThree s on s.SectionThreeTagRus = value JOIN SectionsTwo _sec2 ON _sec2.SectionTwoId = s.SectionTwoId " +
        //    //        "where ProductId = {0} and StageId = {1}", pId, sId);

        //    //    command.Connection = connection;

        //    //    SqlDataReader reader = command.ExecuteReader();
        //    //    if (reader.HasRows)
        //    //    {
        //    //        while (reader.Read())
        //    //        {
        //    //            foreach (ClassSet pSet in GlobalData.SetList)
        //    //            {
        //    //                if (pSet.SubSetId == reader.GetValue(0).ToString())
        //    //                {
        //    //                    if (!GlobalData.SelectedStage.SetsList.Contains(pSet)) GlobalData.SelectedStage.SetsList.Add(pSet);
        //    //                    break;
        //    //                }
        //    //            }
        //    //            ///MessageBox.Show(reader.GetValue(0).ToString());

        //    //        }
        //    //    }
        //    //    reader.Close();
        //    //    if (GlobalData.SelectedStage.SetsList.Count > 0) GlobalData.SelectedStage.SetListApproved = true;
        //    //}
        //}

        /// <summary>
        /// Чтение Стран
        /// </summary>
        //public static List<ClassCountry> ReadSQL_Country()
        //{
        //    GlobalData.ProductList.Clear();
        //    List<ClassCountry> result = new List<ClassCountry>();

        //    using (SqlConnection connection = new SqlConnection(MainForm.ConnectionString))
        //    {
        //        // Подключение открыто
        //        connection.Open();

        //        SqlCommand command = new SqlCommand();
        //        GlobalData.CountryList.Clear();

        //        command.CommandText = string.Format("SELECT NationId, NationName, NationFullName FROM Nations");

        //        command.Connection = connection;

        //        SqlDataReader reader = command.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                ClassCountry _Country = new ClassCountry(reader.GetValue(0).ToString());
        //                _Country.ShortName = reader.GetValue(1).ToString();
        //                _Country.FullName = reader.GetValue(2).ToString();
        //                if (!result.Contains(_Country))
        //                {
        //                    result.Add(_Country);
        //                }
        //            }
        //        }
        //        reader.Close();

        //    }
        //    return result;
        //}

        /// <summary>
        /// Чтение библиотеки Групп продуктов
        /// </summary>
        /// <returns></returns>
        //public static List<ClassPrductCatalog> ReadSQL_ProductGroup()
        //{
        //    List<ClassPrductCatalog> result = new List<ClassPrductCatalog>();

        //    using (SqlConnection connection = new SqlConnection(MainForm.ConnectionString))
        //    {
        //        // Подключение открыто
        //        connection.Open();

        //        SqlCommand command = new SqlCommand();

        //        command.CommandText = string.Format("SELECT ProductGroupId, UserId, ProductGroupCode, ProductGroupFullName, ProductGroupDescription FROM ProductGroups ");

        //        command.Connection = connection;

        //        SqlDataReader reader = command.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                ClassPrductCatalog group = new ClassPrductCatalog(reader.GetValue(2).ToString());
        //                group.pId = reader.GetValue(0).ToString();
        //                group.Responsieble = (GlobalData.User_FullList.Where(id => id.ID == reader.GetValue(1).ToString()).First()).FullName;
        //                group.Name = reader.GetValue(3).ToString();
        //                group.Discriptions = reader.GetValue(4).ToString();
        //                group.ShortDirPath = group.ID + @"\";
        //                result.Add(group);
        //            }
        //        }
        //        reader.Close();
        //    }
        //    return result;
        //}

        /// <summary>
        /// Чтение библиотеки Продуктов
        /// </summary>
        /// <returns></returns>
        //public static List<ClassProduct> ReadSQL_Product()
        //{
        //    List<ClassProduct> result = new List<ClassProduct>();

        //    using (SqlConnection connection = new SqlConnection(MainForm.ConnectionString))
        //    {
        //        // Подключение открыто
        //        connection.Open();

        //        SqlCommand command = new SqlCommand();

        //        command.CommandText = string.Format("SELECT ProductId, UserId, ProductCode, ProductFullName, ProductDescription, ProductDataStart, ProductDataEnd, ProductGroupId FROM Products");

        //        command.Connection = connection;

        //        SqlDataReader reader = command.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                string[] sID = reader.GetValue(2).ToString().Split('-');
        //                ClassProduct product = new ClassProduct(sID[1]);
        //                product.pId = reader.GetValue(0).ToString();
        //                product.Responsible = (GlobalData.User_FullList.Where(id => id.ID == reader.GetValue(1).ToString()).First()).FullName;
        //                product.Name = reader.GetValue(3).ToString();
        //                product.Discription = reader.GetValue(4).ToString();
        //                product.StartDate = reader.GetValue(5).ToString();
        //                product.EndDate = reader.GetValue(6).ToString();
        //                try
        //                {
        //                    product.ProdCategoryId = (GlobalData.ProductCategoriesList.Where(id => id.pId == reader.GetValue(7).ToString()).First()).ID;
        //                }
        //                catch { }

        //                product.ShortDirPath = product.ProdCategoryId + @"\" + sID[1] + @"\";                     
        //                result.Add(product);
        //            }
        //        }
        //        reader.Close();
        //    }
        //    return result;
        //}


        //public static void ReadSQL_ProductStages()
        //{
        //    //List<ClassStage> Stages = new List<ClassStage>();
        //    //using (SqlConnection connection = new SqlConnection(MainForm.ConnectionString))
        //    //{
        //    //    // Подключение открыто
        //    //    connection.Open();

        //    //    SqlCommand command = new SqlCommand();

        //    //    command.CommandText = string.Format("SELECT StageProductId, StageId, ProductId, UserId, StageProductDataStart, StageProductDataEnd FROM StageProducts");

        //    //    command.Connection = connection;

        //    //    SqlDataReader reader = command.ExecuteReader();
        //    //    if (reader.HasRows)
        //    //    {
        //    //        while (reader.Read())
        //    //        {
        //    //            if (GlobalData.PositionList.Count != 0)
        //    //            {
        //    //                ClassStageType sType = null;
        //    //                //GlobalData.StageTypeList = ReadSQL_Stages(3);
        //    //                if (GlobalData.StageTypeList.Count != 0)
        //    //                {
        //    //                    sType = GlobalData.StageTypeList.Where(id => id.Id == reader.GetValue(1).ToString()).First();
        //    //                }
        //    //                if (GlobalData.ProductList.Count > 0)
        //    //                {
        //    //                    ClassProduct sProduct = GlobalData.ProductList.Where(id => id.pId == reader.GetValue(2).ToString()).First();
        //    //                    if (sProduct != null && sType != null)
        //    //                    {
        //    //                        ClassStage nStage = new ClassStage(sProduct.ShortDirPath + sType.Lable + @"\");
        //    //                        nStage.Tag = sType.Tag;
        //    //                        nStage.Index = sType.Index;
        //    //                        nStage.ParantProductID = sProduct.ID;
        //    //                        nStage.Id = reader.GetValue(1).ToString();
        //    //                        if (!GlobalData.StageList.Contains(nStage))
        //    //                        {
        //    //                            GlobalData.StageList.Add(nStage);
        //    //                        }
        //    //                    }
        //    //                }

        //    //            }
        //    //        }
        //    //    }
        //    //    reader.Close();
        //    //}
        //}
        #endregion

        static SqlConnection connection = null;
        static string connectionString = MainForm.ConnectionString;
        /// <summary>
        /// Получить список разделов, доступных исполнителю
        /// </summary>
        public static bool ReadSQL_GetUserSets()
        {
            
            GlobalData.UserSets.Clear();

            using (connection = new SqlConnection(MainForm.ConnectionString))
            {
                connection.ConnectionString = connectionString;
                // Подключение открыто
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.CommandText = string.Format("SELECT SectionThreeNum FROM SectionsThree " +
                    $"WHERE DepartmentId = (SELECT DepartmentId FROM Users WHERE UserAccount = '{GlobalData.user.UserAccount}')");

                command.Connection = connection;
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        GlobalData.UserSets.Add(reader.GetValue(0).ToString());
                    }
                }
                reader.Close();
            }
            return true;
        }

        /// <summary>
        /// Пост-запрос на создание стадии
        /// </summary>
        /// <returns></returns>
        public static bool CreateStage(string path, string Stage, string StageId, string Index)
        {
            // HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/");
            bool result = false;
            try
            {
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://127.0.0.1:8085/AddPosition/");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/AddPosition/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";

                string text = "";

                text = "Stage" + Environment.NewLine;
                text += path + Environment.NewLine;
                text += GlobalData.SelectedProject.ProjectId + Environment.NewLine;
                text += Stage + Environment.NewLine;
                text += StageId + Environment.NewLine;
                text += Index;
                //MessageBox.Show(text);

                byte[] byteArray = Encoding.UTF8.GetBytes(text);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line == "Успех!")
                                    result = true;
                            }
                        }
                    }
                    response.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }


            return result;
        }

        /// <summary>
        /// Пост-запрос на создание поз. по ГП
        /// </summary>
        /// <returns></returns>
        public static bool CreatePosition(string StageId, string PositionID, string PositionName, 
            string PositionNumber, string ProjectNumber, string FullPath, string GIP_Id, string GAP_Id, string StartDate, string EndDate)
        {
            // HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/");
            bool result = false;
            try
            {
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://127.0.0.1:8085/AddPosition/");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/AddPosition/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";

                string text = "";

                text = "Position" + Environment.NewLine;
                text += StageId + Environment.NewLine;
                text += PositionID + Environment.NewLine;
                text += PositionName + Environment.NewLine;
                text += PositionNumber + Environment.NewLine;
                text += ProjectNumber + Environment.NewLine;
                text += FullPath + Environment.NewLine;
                text += GIP_Id + Environment.NewLine;
                text += GAP_Id + Environment.NewLine;
                text += StartDate + Environment.NewLine;
                text += EndDate;
                ///MessageBox.Show(text);

                byte[] byteArray = Encoding.UTF8.GetBytes(text);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line == "Успех!")
                                    result = true;
                            }
                        }
                    }
                    response.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }


            return result;
        }

        /// <summary>
        /// Пост-запрос на создание поз. по ГП
        /// </summary>
        /// <returns></returns>
        public static bool CreateProject(string StartDate, string EndDate, string GIP, string NationFullName, string NationName,
            string NationId, string ProjectName, string ProjectFullName, string ProjectId)
        {
            bool result = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/CreateProject/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";

                CreateProject createProject = new CreateProject();
                createProject.ProjectDataStart = StartDate;
                createProject.ProjectDataEnd = EndDate;
                createProject.ProjectGIP = GIP;
                createProject.NationFullName = NationFullName;
                createProject.NationName = NationName;
                createProject.NationId = NationId;
                createProject.ProjectName = ProjectName;
                createProject.ProjectFullName = ProjectFullName;
                createProject.ProjectId = ProjectId;
                createProject.ProjectAuthor = GlobalData.user.UserAccount;

                byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(createProject, Formatting.Indented));
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line == "Успех!")
                                    result = true;
                            }
                        }
                    }
                    response.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }


            return result;
        }


        public static bool AddProjectLink(string ProjectId, string ProjectAuthor, string ProjectLinkId)
        {
            bool result = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/AddProjectLink/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";

                AddProjectLink addProjectLink = new AddProjectLink();
                addProjectLink.ProjectId = ProjectId;
                addProjectLink.ProjectAuthor = ProjectAuthor;
                addProjectLink.ProjectLinkId = ProjectLinkId;

                byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(addProjectLink, Formatting.Indented));
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line == "Успех!")
                                    result = true;
                            }
                        }
                    }
                    response.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }


            return result;
        }

        /// <summary>
        /// Пост-запрос на создание Разделов
        /// </summary>
        /// <returns></returns>
        public static bool CreatePositionSets(string PositionPath, string StageId, string PositionId, string UserId, List<ScheduleItem>SetsList, int mode)
        {
            bool result = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/AddPosition/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";



                string text = "";
                if (mode > 0)
                {
                    text = "SectionsAdd" + Environment.NewLine;
                }
                else
                {
                    text = "Sections" + Environment.NewLine;
                }
                text += PositionPath + Environment.NewLine;
                text += PositionId + Environment.NewLine;
                text += StageId + Environment.NewLine;
                text += UserId + Environment.NewLine;
                foreach (ScheduleItem _set in SetsList)
                {
                    string postfix = "-1";
                    if (_set.SecThreePostfix !=null && _set.SecThreePostfix != "" && _set.SecThreePostfix != "-1")
                    {
                        postfix = _set.SecThreePostfix;
                    }
                    string deldep = "-1";
                    if (_set.DelegatedDepId != null && _set.DelegatedDepId != 0 && _set.DelegatedDepId != -1)
                    {
                        deldep = _set.DelegatedDepId.ToString();
                    }

                    text += $"{_set.SecThreeId};{postfix};{deldep}" + Environment.NewLine;
                }

                byte[] byteArray = Encoding.UTF8.GetBytes(text);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line == "Успех!")
                                    result = true;
                            }
                        }
                    }
                    response.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }


            return result;
        }

        /// <summary>
        /// Пост-запрос на создание истории
        /// </summary>
        /// <returns></returns>
        public static bool CreateLog(string txt)
        {
            bool result = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/Info/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";

                string text = "";

                text = "Log" + Environment.NewLine;
                text += txt;
                //MessageBox.Show(text);

                byte[] byteArray = Encoding.UTF8.GetBytes(text);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line == "Успех!")
                                    result = true;
                            }
                        }
                    }
                    response.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }


            return result;
        }

        public static bool ResetRVT(string code, string user)
        {
            bool result = false;
            string txtCreate = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/AddPosition/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";

                string text = "SectionCreateReset" + Environment.NewLine;

                text += code;



                byte[] byteArray = Encoding.UTF8.GetBytes(text);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }


                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    response.Close();
                }
                catch
                {
                    //Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    //MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch
            {
                //Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                //MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return result;
        }

        public static bool CreateRVT(string code, string user, out string _answer)
        {
            bool result = false;
            _answer = "";
            string txtCreate = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/AddPosition/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";

                string text = "SectionCreate" + Environment.NewLine;

                text += code;



                byte[] byteArray = Encoding.UTF8.GetBytes(text);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }


                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {

                                if (line == "[В работе]" || line == "[Готово]")
                                {
                                    _answer = line;
                                    //_answer = line.Replace("[", "");
                                    //_answer = _answer.Replace("]", "");
                                    return true;
                                }


                                if (line.Contains("Синхронизация файла")) break;
                                txtCreate += line + Environment.NewLine;
                            }
                        }
                    }
                    response.Close();
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    //MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                //MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            string[] scode = code.Split(';');

            string pathCreate = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Autodesk\Revit\Autodesk Revit 2022\Journals\" + scode[0] + ".txt";
            string pathCreatePython = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Autodesk\Revit\Autodesk Revit 2022\Journals\" + "Create.py";

            File.WriteAllText(pathCreate, txtCreate, Encoding.GetEncoding(1251));

            string pathRVT = @"C:\Program Files\Autodesk\Revit 2022\Revit.exe";
            string python =
                "import os" + Environment.NewLine +
                "import shutil" + Environment.NewLine +
                "import subprocess, uuid" + Environment.NewLine +
                string.Format("pathRVT = {0}{1}{0}", '"', pathRVT.Replace(@"\", @"\\")) + Environment.NewLine +
                string.Format("pathCreate = {0}{1}{0}", '"', pathCreate.Replace(@"\", @"\\")) + Environment.NewLine +
                string.Format("subprocess.Popen([pathRVT, pathCreate])");


            File.WriteAllText(pathCreatePython, python, Encoding.GetEncoding(1251));

            Process.Start(pathCreatePython);
            return result;
        }

        public static bool CreateNWF(string code, string user, out string _answer)
        {
            bool result = false;
            _answer = "";
            string txt = "";
            string txtCreate = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/AddPosition/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";

                string text = "SectionCreateNWF" + Environment.NewLine;

                text += code;



                byte[] byteArray = Encoding.UTF8.GetBytes(text);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }


                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                txt += line + Environment.NewLine;
                                
                            }
                        }
                    }
                    response.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            if (txt != "Ошибка данных")
            {
                cNWF = JsonConvert.DeserializeObject<CreateNWF>(txt);
                _stop = false;
                StartCreateNWF(cNWF.arg1, 0);
                while (_stop != true)
                {
                    Thread.Sleep(1000);
                }
            }
            else
            {
                return false;
            }
            return result;
        }

        public static bool ChangeAccessStatus(string path, string posid, string secthreeid, int status)
        {
            bool result = false;
            string txtCreate = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/Info/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";

                string text = "Security" + Environment.NewLine;

                text += path + Environment.NewLine;
                text += posid + Environment.NewLine;
                text += secthreeid + Environment.NewLine;
                text += status.ToString();

                byte[] byteArray = Encoding.UTF8.GetBytes(text);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }


                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line == "Успех!")
                                    result = true;
                            }
                        }
                    }
                    response.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return result;
        }

        public static void UpdatePositionProgressByJson(string txt)
        {
            try
            {
                bool result = false;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/Info/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";

                string text = "";
                text = "SetSectionProgress" + Environment.NewLine;
                text += txt;

                byte[] byteArray = Encoding.UTF8.GetBytes(text);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line == "Успех!")
                                    result = true;
                            }
                        }
                    }
                    response.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }

        }

        private static void StartCreateNWF(object sender, EventArgs e)
        {
            if (File.Exists(cNWF.PathFileGFnwd))
            {
                if (File.Exists(cNWF.PathFileGFnwdGlobal))
                    StartCreateNWF(cNWF.arg4, 2);
                else
                    StartCreateNWF(cNWF.arg3, 1);
            }
            else
            {
                cNWF = null;
            }
        }

        private static void EndCreateNWF(object sender, EventArgs e)
        {
            if (File.Exists(cNWF.PathFileGFnwfGlobal))
                Console.WriteLine(cNWF.PathFileGFnwfGlobal + " : Создан");
            else
                Console.WriteLine(cNWF.PathFileGFnwfGlobal + " : Ошибка");

            if (File.Exists(cNWF.PathFileGFnwdGlobal))
                Console.WriteLine(cNWF.PathFileGFnwdGlobal + " : Создан");
            else
                Console.WriteLine(cNWF.PathFileGFnwdGlobal + " : Ошибка");

            //Console.WriteLine(DateTime.Now);
            //Console.WriteLine("Готово");
            _stop = true;
            cNWF = null;
        }

        public static void CopyAndOpenFile(string path)
        {
            if (File.Exists(path))
            {
                
                string dirpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ProjectManagement\Temp\OpenNWD\";
                if (!System.IO.Directory.Exists(dirpath))
                {
                    System.IO.Directory.CreateDirectory(dirpath);
                }

                string[] spPath = path.Split('\\');
                if (spPath != null && spPath.Length > 0)
                {
                    string _path = dirpath + spPath[spPath.Length - 1];
                    try
                    {
                        File.Copy(path, _path, true);
                        if (File.Exists(_path))
                        {
                            GlobalData.CreatedFiles.Add(_path);
                            using (Process.Start(new ProcessStartInfo("explorer.exe", " /e, " + _path))) ;
                        }
                        else
                        {
                            Form_MessageBox messageBox = new Form_MessageBox($"Не удалось открыть файл: {spPath[spPath.Length - 1]}", "Предупреждение", 0);
                            messageBox.ShowDialog();
                        }
                    }
                    catch
                    {
                        Form_MessageBox messageBox = new Form_MessageBox($"Не получилось скопировать файл в папку: {dirpath}\n Закройте файл: {spPath[spPath.Length - 1]}\n и попробуйте еще раз.", "Предупреждение", 0);
                        messageBox.ShowDialog();
                    }
                }
                GlobalMethodes.CreateLog($"Открытие NWD: {path}");
            }
            else
            {
                Form_MessageBox messageBox = new Form_MessageBox("Моодель элемента отсутствует. Обратитесь в BIM-отдел.", "Предупреждение",0);
                messageBox.ShowDialog();
                GlobalMethodes.CreateLog($"Отсутствует NWD: {path}");
            }
        }

        public static void DeleteCreatedFiles()
        {
            foreach(string path in GlobalData.CreatedFiles)
            {
                try
                {
                    File.Delete(path);
                }
                catch { }
            }
        }


        //private static void EndCreateNWF(object sender, EventArgs e)
        //{
        //    if (File.Exists(cNWF.PathFileGFnwfGlobal))
        //        Console.WriteLine(cNWF.PathFileGFnwfGlobal + " : Создан");
        //    else
        //        Console.WriteLine(cNWF.PathFileGFnwfGlobal + " : Ошибка");

        //    if (File.Exists(cNWF.PathFileGFnwdGlobal))
        //        Console.WriteLine(cNWF.PathFileGFnwdGlobal + " : Создан");
        //    else
        //        Console.WriteLine(cNWF.PathFileGFnwdGlobal + " : Ошибка");

        //    Console.WriteLine(DateTime.Now);
        //    Console.WriteLine("Готово");
        //    _stop = true;
        //    cNWF = null;
        //}

        //private static void StartCreateNWF(string arg, byte mode)
        //{
        //    Process iStartProcess = new Process(); // новый процесс
        //    iStartProcess.StartInfo.FileName = cNWF.PathRoamer; // путь к запускаемому файлу
        //    iStartProcess.StartInfo.Arguments = arg; // Info.cNWF.arg1; // эта строка указывается, если программа запускается с параметрами (здесь указан пример, для наглядности)
        //                                             //iStartProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal; // эту строку указываем, если хотим запустить программу в скрытом виде
        //    iStartProcess.EnableRaisingEvents = true;
        //    iStartProcess.Start(); // запускаем программу  

        //    if (mode == 0)
        //    {
        //        GlobalData.loadInfo = "Выполняется: Создание файлов nwf, nwd для\nотдельной позиции по ГП...";
        //        iStartProcess.Exited += StartCreateNWF;
        //    }
        //    else if (mode == 1)
        //    {
        //        GlobalData.loadInfo = "Выполняется: Создание файлов nwf, nwd для\nвсего проекта...";
        //        iStartProcess.Exited += EndCreateNWF;
        //    }
        //    else if (mode == 2)
        //    {
        //        GlobalData.loadInfo = "Выполняется: Обновление файлов nwf, nwd для\nвсего проекта...";
        //        iStartProcess.Exited += EndCreateNWF;
        //    }
        //}

        public static string RefreshSectionTag(string tag)
        {
            string result = "";
            char[] sptag = tag.ToCharArray();
            foreach (char ch in sptag)
            {
                string t = Convert.ToString(ch);
                if (t != "<" && t != ">" && t != "-")
                {
                    result += t;
                }
            }

            return result;
        }

        private static void StartCreateNWF(string arg, byte mode)
        {
            GlobalData.loadInfo = "Запуск процесса создания...";
            Process iStartProcess = new Process();
            iStartProcess.StartInfo.FileName = cNWF.PathRoamer;
            iStartProcess.StartInfo.Arguments = arg; // Info.cNWF.arg1;
            iStartProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            iStartProcess.EnableRaisingEvents = true;

            iStartProcess.Start(); // запускаем программу 
            if (mode == 0)
            {
                GlobalData.loadInfo = "Создание файлов nwf, nwd для отдельной\nпозиции по ГП...";
                iStartProcess.Exited += StartCreateNWF;
            }
            else if (mode == 1)
            {
                GlobalData.loadInfo = "Создание файлов nwf, nwd для всего проекта...";
                iStartProcess.Exited += EndCreateNWF;
            }
            else if (mode == 2)
            {
                GlobalData.loadInfo = "Обновление файлов nwf, nwd для всего проекта...";
                iStartProcess.Exited += EndCreateNWF;
                GlobalMethodes._stop = true;
            }
        }

        public static bool CreateProduct(string req)
        {
            bool result = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/CreateProduct/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(req); //JsonConvert.SerializeObject(newProduct, Formatting.Indented)
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line == "Успех!")
                                    result = true;
                            }
                        }
                    }
                    response.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return result;
        }

        public static bool CreateTechSolution(string req)
        {
            bool result = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/TechSolution/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(req);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line == "Успех!")
                                    result = true;
                            }
                        }
                    }
                    response.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return result;
        }

        public static bool CreateProductStage (string req)
        {
            bool result = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/CreateStageProduct/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(req);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line == "Успех!")
                                    result = true;
                            }
                        }
                    }
                    response.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return result;
        }

        public static bool CreateTechSolutionStage(string req)
        {
            bool result = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/CreateStageTechSolution/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(req);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line == "Успех!")
                                    result = true;
                            }
                        }
                    }
                    response.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return result;
        }

        public static bool CreateProductSet(string req)
        {
            bool result = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/SectionsProduct/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(req);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line == "Успех!")
                                    result = true;
                            }
                        }
                    }
                    response.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return result;
        }

        public static bool CreateTechSolutionSet(string req)
        {
            bool result = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/SectionsTechSolution/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(req);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (line == "Успех!")
                                    result = true;
                            }
                        }
                    }
                    response.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return result;
        }

        public static void UpdateCombaBox(List<string>ItemsList, ComboBox Box)
        {
            Box.Items.Clear();
            Box.Items.Add("<Не указано>");
            Box.Items.AddRange(ItemsList.ToArray());
        }
    
        public static void requestSectionApproved(string code)
        {

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/AddPosition/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";

                string text = "SectionCreateApproved" + Environment.NewLine;

                text += code;


                byte[] byteArray = Encoding.UTF8.GetBytes(text);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public static void requestNWFApproved(string code)
        {

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/AddPosition/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";

                string text = "SectionCreateNWFApproved" + Environment.NewLine;

                text += code;


                byte[] byteArray = Encoding.UTF8.GetBytes(text);
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public static void UpdateProductCategories()
        {
            foreach(ClassPrductCatalog _category in GlobalData.ProductCategoriesList)
            {
                _category.prodList.Clear();
                foreach(ClassProduct _prod in GlobalData.ProductList)
                {
                    if (_prod.ProdCategoryId == _category.ID)
                    {
                        _category.prodList.Add(_prod);
                    }
                }
            }
        }

        //public static void UpdateRelisedSetList()
        //{
        //    //if (GlobalData.SelectedPosition != null && GlobalData.SelectedPosition.SetListeInRelease == false)
        //    //{
        //    //    if (GlobalData.SelectedPosition.SetsList.Count > 0)
        //    //    {
        //    //        foreach (ClassSet _set in GlobalData.SelectedPosition.SetsList)
        //    //        {
        //    //            GlobalData.ReleasedSetList.Add(_set);
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        GlobalData.ReleasedSetList.Clear();
        //    //    }
        //    //}
        //    //else if (GlobalData.SelectedProduct != null && GlobalData.SelectedStage.SetListeInRelease == false)
        //    //{
        //    //    if (GlobalData.SelectedStage.SetsList.Count > 0)
        //    //    {
        //    //        foreach (ClassSet _set in GlobalData.SelectedStage.SetsList)
        //    //        {
        //    //            GlobalData.ReleasedSetList.Add(_set);
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        GlobalData.ReleasedSetList.Clear();
        //    //    }
        //    //}
        //}
        public static int GetRouteType(LibraryDB.DB.Task task, int tdId)
        {
            int result = 0;
            var TD = task.TaskDepartments.FirstOrDefault(x => x.TaskDepartmentId == tdId);
            if (TD != null)
            {
                var sp = RequestInfo.lb.SectionsPositions.Where(x => x.PositionId == task.PositionId).FirstOrDefault(c => c.SectionThreeId == task.SectionThreeId);
                if (sp != null && sp.DepartmentIdDelegation != null) result = 2;
                else
                {
                    var spd = RequestInfo.lb.SectionsPositions.Where(x => x.PositionId == task.PositionId).Where(c => c.SectionThreeId == TD.SectionThreeId).FirstOrDefault(n => n.SectionPositionNumber == TD.SectionPositionNumber);
                    if (spd != null && spd.DepartmentIdDelegation != null) result = 1;
                }
            }
            return result;
        }

        public static Color GetCellColor(int status, int routetype, bool enter = false)
        {
            Color result = Color.White;
            if (!enter)
            {
                switch (status)
                {
                    case -1:
                        result = Color.White;
                        break;
                    case 5:
                        result = Color.Gainsboro;
                        break;
                    case 7:
                    case 6:
                    case 8:
                    case 9:
                    case 18:
                    case 19:
                    case 20:
                        result = Color.Orange;
                        break;
                    case 10:
                        result = Color.Green;
                        break;
                    case 12:
                        result = Color.Gray;
                        break;
                    case 13:
                        result = Color.Red;
                        break;
                }
            }
            else
            {
                switch (status)
                {
                    case -1:
                        result = Color.Gainsboro;
                        break;
                    case 5:
                    case 6:
                    case 7:
                        result = Color.LightGray;
                        break;
                    case 8:
                    case 9:
                        result = Color.DarkOrange;
                        break;
                    case 10:
                        result = Color.DarkGreen;
                        break;
                    case 12:
                        result = Color.DarkGray;
                        break;
                    case 13:
                        result = Color.DarkRed;
                        break;
                }
            }
            return result;
        }

        public static void GetHistory(string history, out string date, out string user)
        {
            date = "";
            user = "";
            HistoryLog historyLog = null;
            if (history != null && history != "")
            {
                historyLog = JsonConvert.DeserializeObject<HistoryLog>(history);
            }
            if (historyLog != null)
            {
                POSTServer.History.HistoryInfo historylog = historyLog.spHistory.Last();
                try
                {
                    date = historylog.Date.Split(' ')[0];
                    user = historylog.User;
                }
                catch
                {
                }
            }
            else
            {
            }
        }
    }
}
