using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace UST_ProjectManagement
{
    public partial class UC_NavigationPanel : UserControl
    {
        public delegate void PrjectPanel_Click();
        public event PrjectPanel_Click OpenProjectPanel;

        public delegate void HomePanel_Click();
        public event HomePanel_Click OpenHomePanel;

        public delegate void ThreeNode_Click();
        public event ThreeNode_Click ThreeNodeCick;

        public delegate void toolStripItem_Click(byte mode);
        public event toolStripItem_Click CreateProject;
        public event toolStripItem_Click CreatePosition;
        public event toolStripItem_Click CreateStage;
        public event toolStripItem_Click CreateProduct;
        public event toolStripItem_Click CreateTechSolution;
        public event toolStripItem_Click CreateProductAsProject;
        public event toolStripItem_Click CreateSubProject;

        public delegate void EditSetList_Click();
        public event EditSetList_Click EditSetsList;

        ToolStripMenuItem toolStripItem1 = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItem2 = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItem3 = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItem4 = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItem5 = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItem6 = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItem7 = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItem8 = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItem9 = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItem10 = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItem11 = new ToolStripMenuItem();

        TreeNode SelNode = null;
        string NodePath = "";
        bool open = false;
        bool click = false;


        public UC_NavigationPanel()
        {
            InitializeComponent();
        }

        private void treeViewNavigation_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            GetNodeInfo(e.Node);
            if (GlobalData.SelectedMainFolderName != null ||
                    GlobalData.SelectedCountry != null ||
                    GlobalData.SelectedProject != null ||
                    GlobalData.SelectedStage != null ||
                    GlobalData.SelectedPosition != null ||
                    GlobalData.SelectedProductCatalog != null ||
                    GlobalData.SelectedProduct != null ||
                    GlobalData.SelectedTechSolutionCatalog != null ||
                    GlobalData.SelectedTechSolution != null)
            {
                OpenProjectPanel?.Invoke();
            }
            else
            {
                OpenHomePanel?.Invoke();
            }
        }

        public void GetNodeInfo(TreeNode e)
        {
            GlobalData.SelectedDirPath = MainForm.MainPath_rezerv + e.FullPath + @"\";
            GlobalData.SelectedNode = e;
            GlobalData.SelectedMainFolderName = null;
            //if (GlobalData.SelectedPosition != null) GlobalData.SelectedPosition.SetListeInRelease = false;
            GlobalData.SelectedCountry = null;
            GlobalData.SelectedProject = null;
            GlobalData.SelectedStage = null;
            GlobalData.SelectedPosition = null;
            GlobalData.SelectedProductCatalog = null;
            GlobalData.SelectedProduct = null;
            GlobalData.SelectedTechSolutionCatalog = null;
            GlobalData.SelectedTechSolution = null;
            GlobalData.ReleasedSetList.Clear();

            string[] txt = e.Text.Split('_');
            string[] path = e.FullPath.Split('\\');
            string[] stage = null;
            if (path.Length >  2)
            {
                stage = path[path.Length - 2].Split('_');
            }

            switch (e.Tag)
            {
                case "Projects":
                case "Products":
                case "TechSolutions":
                    GlobalData.SelectedMainFolderName = e.Text;
                    break;
                case "Country":
                    GlobalData.SelectedCountry = RequestInfo.lb.Nations.FirstOrDefault(n => n.NationId == txt[0]);
                    break;
                case "Project":
                    GlobalData.SelectedProject = RequestInfo.lb.Projects.FirstOrDefault(n => n.ProjectId == e.Text);
                    break;
                case "SubProject":
                    GlobalData.SelectedProject = RequestInfo.lb.Projects.FirstOrDefault(n => n.ProjectId == e.Text);
                    string[] substage1 = path[e.Level - 1].Split('_');
                    GlobalData.SelectedStage = RequestInfo.lb.Stages.FirstOrDefault(n => n.StageTag == substage1[substage1.Length - 1]);
                    break;
                case "Stage":
                    GlobalData.SelectedProject = RequestInfo.lb.Projects.FirstOrDefault(id => id.ProjectId == path[2]);
                    GlobalData.SelectedStage = RequestInfo.lb.Stages.FirstOrDefault(n => n.StageTag == txt[1]);
                    break;
                case "StageProduct":
                    GlobalData.SelectedProduct = RequestInfo.lb.Products.FirstOrDefault(id => id.ProductCode == e.Parent.Parent.Text + "-" + e.Parent.Text);
                    GlobalData.SelectedStage = RequestInfo.lb.Stages.FirstOrDefault(n => n.StageTag == txt[1]);
                    break;
                case "StageTechSolution":
                    GlobalData.SelectedTechSolution = RequestInfo.lb.TechSolutions.FirstOrDefault(id => id.TechSolutionCode == e.Parent.Text);
                    GlobalData.SelectedStage = RequestInfo.lb.Stages.FirstOrDefault(n => n.StageTag == txt[1]);
                    break;
                case "Position":
                    GlobalData.SelectedStage = RequestInfo.lb.Stages.FirstOrDefault(n => n.StageTag == stage[stage.Length - 1]);
                    GlobalData.SelectedPosition = RequestInfo.lb.Positions.FindAll(p => p.PositionCode == e.Text).FirstOrDefault(s => s.StageId == GlobalData.SelectedStage.StageId);
                    GlobalData.SelectedProject = RequestInfo.lb.Projects.FirstOrDefault(n => n.ProjectId == GlobalData.SelectedPosition.ProjectId);
                    break;
                case "SubPosition":
                    string[] substage = path[path.Length - 3].Split('_');
                    GlobalData.SelectedStage = RequestInfo.lb.Stages.FirstOrDefault(n => n.StageTag == substage[substage.Length - 1]);
                    GlobalData.SelectedPosition = RequestInfo.lb.Positions.FindAll(p => p.PositionCode == e.Text).FirstOrDefault(s => s.StageId == GlobalData.SelectedStage.StageId);
                    GlobalData.SelectedProject = RequestInfo.lb.Projects.FirstOrDefault(n => n.ProjectId == GlobalData.SelectedPosition.ProjectId);
                    break;
                case "ProductsCatalog":
                    GlobalData.SelectedProductCatalog = RequestInfo.lb.ProductGroups.FirstOrDefault(c => c.ProductGroupCode == e.Text);
                    break;
                case "Product":
                    GlobalData.SelectedProduct = RequestInfo.lb.Products.FirstOrDefault(c => c.ProductCode == e.Parent.Text + "-" + e.Text);
                    break;
                case "TechSolutionsCatalog":
                    GlobalData.SelectedTechSolutionCatalog = RequestInfo.lb.TechSolutionGroups.FirstOrDefault(c => c.TechSolutionGroupCode == e.Text);
                    break;
                case "TechSolution":
                    GlobalData.SelectedTechSolution = RequestInfo.lb.TechSolutions.FirstOrDefault(c => c.TechSolutionCode == e.Text);
                    break;
            }
        }

        private void treeViewNavigation_BeforeSelect_Old(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                //ThreeNodeCick?. Invoke();
                //string DirShortName = e.Node.Text;
                //string[] nodeName = e.Node.Text.ToString().Split(new char[] { '_' });
                //GlobalData.SelectedFolderPath = e.Node.Tag.ToString();
                //string NodeShortPath = e.Node.Tag.ToString();

                //GlobalData.SelectedMainFolderName = null;
                //if (GlobalData.SelectedPosition != null) GlobalData.SelectedPosition.SetListeInRelease = false;
                //GlobalData.SelectedCountry = null;
                //GlobalData.SelectedProject = null;
                //GlobalData.SelectedStage = null;
                //GlobalData.SelectedPosition = null;
                //GlobalData.SelectedProductCatalog = null;
                //GlobalData.SelectedProduct = null;
                //GlobalData.ReleasedSetList.Clear();

                //if (DirShortName == "01_Проекты" || DirShortName == "02_Продукты")
                //{
                //    GlobalData.SelectedMainFolderName = DirShortName;
                //}

                //if (NodeShortPath != "")
                //{
                    
                //    string[] spliteShortPath = NodeShortPath.Split('\\');
                //    if (spliteShortPath.Length >= 3 && spliteShortPath[2] == "01_Проекты")
                //    {
                //        if (spliteShortPath.Length > 3)
                //        {
                //            foreach (ClassCountry _countr in GlobalData.CountryList)
                //            {
                //                string cName = _countr.ID + "_" + _countr.ShortName;
                //                if (cName == DirShortName)
                //                {
                //                    GlobalData.SelectedCountry = _countr;
                //                    break;
                //                }
                //            }
                //        }
                //        if (spliteShortPath.Length > 4)
                //        {
                //            string sPath = spliteShortPath[4] + @"\";

                //            foreach (ClassProject _prj in GlobalData.ProjectList)
                //            {
                //                if (sPath == _prj.prjShortDirPath)
                //                {
                //                    _prj.prjDirPath = @"\" + spliteShortPath[3] + @"\" + sPath;
                //                    GlobalData.SelectedProject = _prj;
                //                    break;
                //                }
                //            }
                //        }
                //        if (spliteShortPath.Length > 5)
                //        {
                //            string sPath = spliteShortPath[4] + @"\" + spliteShortPath[5] + @"\";

                //            foreach (ClassStage _stage in GlobalData.StageList)
                //            {
                //                if (sPath == _stage.ShortPath)
                //                {
                //                    GlobalData.SelectedStage = _stage;
                //                    break;
                //                }
                //            }
                //        }
                //        if (spliteShortPath.Length > 6)
                //        {
                //            string sPath = spliteShortPath[4] + @"\" + spliteShortPath[5] + @"\" + spliteShortPath[6] + @"\";
                //            string[] splitname = DirShortName.Split('_');

                //            if (splitname.Length > 1 && splitname[1] == "ГИП")
                //            {
                //                GlobalData.GIPfolder = true;
                //            }
                //            else
                //            {
                //                foreach (ClassPosition _pos in GlobalData.PositionList)
                //                {
                //                    if (sPath == _pos.ShortPath)
                //                    {
                //                        GlobalData.SelectedPosition = _pos;
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        if (spliteShortPath.Length > 7)
                //        {
                //            string sPath = spliteShortPath[4] + @"\" + spliteShortPath[5] + @"\" + spliteShortPath[6] + @"\" + spliteShortPath[7] + @"\";

                //            foreach (ClassPrductCatalog _pCat in GlobalData.ProductCategoriesList)
                //            {
                //                if (sPath == _pCat.ShortDirPath)
                //                {
                //                    //_pCat.ShortDirPath = @"\" + spliteShortPath[3] + @"\" + sPath;
                //                    GlobalData.SelectedProductCatalog = _pCat;
                //                    break;
                //                }
                //            }
                //        }
                //    }
                //    else if (spliteShortPath.Length >= 3 && spliteShortPath[2] == "02_Продукты")
                //    {
                //        if (spliteShortPath.Length > 3)
                //        {
                //            string sPath = spliteShortPath[3] + @"\";
                //            foreach (ClassPrductCatalog _pcatalog in GlobalData.ProductCategoriesList)
                //            {
                //                if (sPath == _pcatalog.ShortDirPath)
                //                {
                //                    GlobalData.SelectedProductCatalog = _pcatalog;
                //                    break;
                //                }
                //            }
                //        }
                //        if (spliteShortPath.Length > 4)
                //        {
                //            string sPath = spliteShortPath[3] + @"\"+ spliteShortPath[4] + @"\";
                //            foreach (ClassProduct _prod in GlobalData.ProductList)
                //            {
                //                if (sPath == _prod.ShortDirPath)
                //                {
                //                    GlobalData.SelectedProduct = _prod;
                //                    break;
                //                }
                //            }
                //        }
                //        if (spliteShortPath.Length > 5)
                //        {
                //            string sPath = spliteShortPath[3] + @"\" + spliteShortPath[4] + @"\" + spliteShortPath[5] + @"\";
                            
                //            foreach (ClassStage _stage in GlobalData.StageList)
                //            {
                //                if (sPath == _stage.ShortPath)
                //                {
                //                    GlobalData.SelectedStage = _stage;
                //                    break;
                //                }
                //            }
                //        }
                //    }
                //}
                //if (GlobalData.SelectedMainFolderName != null ||
                //    GlobalData.SelectedCountry != null ||
                //    GlobalData.SelectedProject != null || 
                //    GlobalData.SelectedStage != null || 
                //    GlobalData.SelectedPosition != null || 
                //    GlobalData.SelectedProductCatalog != null ||
                //    GlobalData.SelectedProduct != null)
                //{
                //    OpenProjectPanel?.Invoke();
                //}
                //else
                //{
                //    OpenHomePanel?.Invoke();
                //}

            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        /// <summary>
        /// Выбрать раздел CRD
        /// </summary>
        //public void SelectSetTreeNode()
        //{
        //    SelNode.Checked = true;
        //    SelNode.Expand();
        //    treeViewNavigation.Focus();
        //}
        
        private void treeViewNavigation_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            //string DirShortName = e.Node.Text;
            //List<ClassFolder> selF = GlobalData.NaviTreeView.FullFoldersList.Where(n => n.Name == DirShortName).ToList();
            //if(selF.Count > 0)
            //{
            //    ClassFolder sF = selF.First();
            //    GlobalData.NaviTreeView.GetSubFolders(sF, sF.Level, sF.Level + 2);
            //    GlobalData.NaviTreeView.CreateTreeView();
                
            //}
            //e.Node.Expand();
        }

        private void treeViewNavigation_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {  
            if (e.Button == MouseButtons.Right)
            {
                click = true;
                GetNodeInfo(e.Node);
                open = false;
                toolStripItem1.Text = "Открыть в Проводнике";
                toolStripItem1.Image = Properties.Resources.Folder_20x20 as Bitmap;
                toolStripItem1.Click += new EventHandler(toolStripItem1_Click);
                ContextMenuStrip strip = new ContextMenuStrip();

                e.Node.ContextMenuStrip = strip;
                e.Node.ContextMenuStrip.Items.Add(toolStripItem1);

                toolStripItem2.Text = "Получить путь";
                toolStripItem2.Image = Properties.Resources.GetPath as Bitmap;
                toolStripItem2.Click += new EventHandler(toolStripItem2_Click);

                e.Node.ContextMenuStrip.Items.Add(toolStripItem2);


                if (GlobalData.SelectedCountry != null && e.Node.Tag == "Country")
                {
                    if (GlobalData.UserRole == "Admin")
                    {
                        toolStripItem4.Text = "Создать проект";
                        toolStripItem4.Image = Properties.Resources.Add_20x20 as Bitmap;
                        toolStripItem4.Click += new EventHandler(toolStripItemAdd_Click);
                        e.Node.ContextMenuStrip.Items.Add(toolStripItem4); 
                    }

                    if (GlobalData.UserRole == "Admin" || GlobalData.UserRole == "Manager")
                    {
                        toolStripItem9.Text = "Создать тех.решение";
                        toolStripItem9.Image = Properties.Resources.Add_20x20 as Bitmap;
                        toolStripItem9.Click += new EventHandler(toolStripItemAdd_Click);
                        e.Node.ContextMenuStrip.Items.Add(toolStripItem9); 
                    }
                }
                else if (GlobalData.SelectedProject != null && e.Node.Tag == "Project")
                {
                    if (GlobalData.UserRole == "Admin" || GlobalData.UserRole == "Manager")
                    {
                        toolStripItem5.Text = "Создать стадию";
                        toolStripItem5.Image = Properties.Resources.Add_20x20 as Bitmap;
                        toolStripItem5.Click += new EventHandler(toolStripItemAdd_Click);

                        e.Node.ContextMenuStrip.Items.Add(toolStripItem5); 
                    }
                }
                else if (GlobalData.SelectedStage != null && e.Node.Tag == "Stage"||
                    GlobalData.SelectedProject !=null && e.Node.Tag == "SubProject")
                {
                    if (GlobalData.UserRole == "Admin" || GlobalData.UserRole == "Manager")
                    {
                        toolStripItem6.Text = "Создать поз.по ГП";
                        toolStripItem6.Image = Properties.Resources.Add_20x20 as Bitmap;
                        toolStripItem6.Click += new EventHandler(toolStripItemAdd_Click);

                        e.Node.ContextMenuStrip.Items.Add(toolStripItem6);

                        toolStripItem10.Text = "Создать подпроект";
                        toolStripItem10.Image = Properties.Resources.Add_20x20 as Bitmap;
                        toolStripItem10.Click += new EventHandler(toolStripItemAdd_Click);

                        e.Node.ContextMenuStrip.Items.Add(toolStripItem10); 
                    }
                }
                else if (GlobalData.SelectedPosition != null && (e.Node.Tag == "Position" || e.Node.Tag == "SubPosition"))
                {
                    toolStripItem3.Text = "Открыть в Navisworks";
                    toolStripItem3.Image = Properties.Resources.OpenNavisworks_25x25 as Bitmap;
                    toolStripItem3.Click += new EventHandler(toolStripItem3_Click);
                    e.Node.ContextMenuStrip.Items.Add(toolStripItem3);

                    if (GlobalData.UserRole == "Admin" || GlobalData.UserRole == "Manager")
                    {
                        toolStripItem11.Text = "Добавить разделы";
                        toolStripItem11.Image = Properties.Resources.Add_20x20 as Bitmap;
                        toolStripItem11.Click += new EventHandler(toolStripItemAdd_Click);
                        e.Node.ContextMenuStrip.Items.Add(toolStripItem11); 
                    }
                }
                else if (GlobalData.SelectedProductCatalog != null && e.Node.Level == 1)
                {
                    if (GlobalData.UserRole == "Admin" || GlobalData.UserRole == "Manager")
                    {
                        toolStripItem7.Text = "Создать продукт";
                        toolStripItem7.Image = Properties.Resources.Add_20x20 as Bitmap;
                        toolStripItem7.Click += new EventHandler(toolStripItemAdd_Click);

                        e.Node.ContextMenuStrip.Items.Add(toolStripItem7); 
                    }
                }
                else if (GlobalData.SelectedTechSolutionCatalog != null && e.Node.Level == 1)
                {
                    if (GlobalData.UserRole == "Admin" || GlobalData.UserRole == "Manager")
                    {
                        toolStripItem8.Text = "Создать тех.решение";
                        toolStripItem8.Image = Properties.Resources.Add_20x20 as Bitmap;
                        toolStripItem8.Click += new EventHandler(toolStripItemAdd_Click);

                        e.Node.ContextMenuStrip.Items.Add(toolStripItem8); 
                    }
                }
                else if (GlobalData.SelectedStage != null && (e.Node.Tag == "StageProduct" || e.Node.Tag == "StageTechSolution"))
                {
                    if (GlobalData.UserRole == "Admin" || GlobalData.UserRole == "Manager")
                    {
                        toolStripItem11.Text = "Добавить разделы";
                        toolStripItem11.Image = Properties.Resources.Add_20x20 as Bitmap;
                        toolStripItem11.Click += new EventHandler(toolStripItemAdd_Click);
                        e.Node.ContextMenuStrip.Items.Add(toolStripItem11); 
                    }
                }

                NodePath = "";
                NodePath = MainForm.MainPath_rezerv + e.Node.FullPath + @"\";
            }
        }

        private void toolStripItem1_Click(object sender, EventArgs args)
        {
            if (open == false)
            {
                Process.Start(new ProcessStartInfo("explorer.exe", " /e, " + NodePath));
                GlobalMethodes.CreateLog("Открыть в Проводнике");
                open = true;
            }

        }

        private void toolStripItem2_Click(object sender, EventArgs args)
        {
            if (open == false)
            {
                Clipboard.SetText(NodePath);
                //Process.Start(new ProcessStartInfo("explorer.exe", " /e, " + NodePath));
                //GlobalMethodes.CreateLog("Открыть в Проводнике");
                open = true;
            }

        }

        private void toolStripItem3_Click(object sender, EventArgs args)
        {
            if (open == false)
            {
                PositionInfo info = new PositionInfo(GlobalData.SelectedProject, GlobalData.SelectedStage, GlobalData.SelectedPosition);
                string path = NodePath + @"00.01_CRD\08_NWD\" + GlobalData.SelectedPosition.PositionCode + "_UST_"+ info.StageTag +"_GF_22.nwd";
                //if (File.Exists(path))
                //{
                //    using(Process.Start(new ProcessStartInfo("explorer.exe", " /e, " + path)))
                //    {

                //    }

                //}
                //else
                //{
                //    MessageBox.Show("Отсутствует координационная моодель:\n\n" + path + "\n\nОбратитесь в BIM-отдел.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                GlobalMethodes.CopyAndOpenFile(path);
                GlobalMethodes.CreateLog("Открыть в Проводнике");
                open = true;
            }

        }

        private void toolStripItemAdd_Click(object sender, EventArgs args)
        {
            if (click)
            {
                if (sender.ToString() == toolStripItem4.Text)
                {
                    CreatePosition?.Invoke(1);
                    click = false;
                }
                else if (sender.ToString() == toolStripItem5.Text)
                {
                    CreateStage?.Invoke(0);
                    click = false;
                }
                else if (sender.ToString() == toolStripItem6.Text)
                {
                    CreatePosition?.Invoke(0);
                    click = false;
                }
                else if (sender.ToString() == toolStripItem7.Text)
                {
                    CreateProduct?.Invoke(0);
                    click = false;
                }
                else if (sender.ToString() == toolStripItem8.Text)
                {
                    CreateTechSolution?.Invoke(0);
                    click = false;
                }
                else if (sender.ToString() == toolStripItem9.Text)
                {
                    CreatePosition?.Invoke(2);
                    click = false;
                }
                else if (sender.ToString() == toolStripItem10.Text)
                {
                    CreatePosition?.Invoke(3);
                    click = false;
                }
                else if (sender.ToString() == toolStripItem11.Text)
                {
                    EditSetsList?.Invoke();
                    click = false;
                } 
            }
            
        }

        public bool IsFileInUse(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("'path' cannot be null or empty.", "path");

            try
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read)) { }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }
    }
}
