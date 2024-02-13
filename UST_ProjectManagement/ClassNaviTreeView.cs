using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace UST_ProjectManagement
{
    public class ClassNaviTreeView
    {

        public string MainPath { get; set; }

        public List<ClassFolder> mainFolders = new List<ClassFolder>();

        public List<ClassFolder> FullFoldersList = new List<ClassFolder>();

        public TreeView View { get; set; }

        private int count = 0;

        public ClassNaviTreeView (string path, TreeView view)
        {
            MainPath = path;
            View = view;
            CreateTreeView_New();
            //FillMainDirectNodes();
        }

        /// <summary>
        /// Получить папки Верхнего уровня
        /// </summary>
        public void FillMainDirectNodes()
        {
            //try
            //{
            //    string[] maindirs = System.IO.Directory.GetDirectories(MainPath);
            //    for (int i = 0; i < maindirs.Length; i++)
            //    {
            //        string[] array = maindirs[i].Split('\\');
            //        string nodeName = array[array.Length - 1];

            //        if (nodeName == "01_Проекты" || nodeName == "02_Продукты")
            //        {
            //            TreeNode dirNode = new TreeNode(nodeName);
            //            dirNode.Tag = maindirs[i];

            //            ClassFolder nFolder = new ClassFolder(nodeName, "", maindirs[i]);
            //            nFolder.dirNode = dirNode;
            //            nFolder.Level = 0;
            //            if (nFolder.Name == "02_Продукты")
            //            {
            //                GetSubFolders(nFolder, nFolder.Level, 4, 1);
            //            }
            //            else if (nFolder.Name == "01_Проекты")
            //            {
            //                GetSubFolders(nFolder, nFolder.Level, 3, 0);
            //            }

            //            if (!mainFolders.Contains(nFolder)) mainFolders.Add(nFolder);
            //            if (!FullFoldersList.Contains(nFolder)) FullFoldersList.Add(nFolder); 
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            //}

            //CreateTreeView();
        }
        /// <summary>
        /// Получить подпапки для выбранной папки
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Parent"></param>
        /// <returns></returns>
        public void GetSubFolders(ClassFolder sFolder, int level, int limit, int type)
        {          
            if (level <= limit)
            {
                
                string[] maindirs;
                try
                {
                    maindirs = System.IO.Directory.GetDirectories(sFolder.Path);

                    for (int i = 0; i < maindirs.Length; i++)
                    {
                        TreeNode dirNode = new TreeNode(new DirectoryInfo(maindirs[i]).Name);
                        dirNode.Tag = maindirs[i];

                        ClassFolder Folder = new ClassFolder(dirNode.Text, sFolder.Name, maindirs[i]);
                        Folder.Level = level + 1;
                        Folder.dirNode = dirNode;
                        string[] spath = maindirs[i].Split('\\');
                        switch (type)
                        {
                            case 0:
                                if (spath.Length > 3)
                                {
                                    for (int t = 4; t < spath.Length; t++)
                                    {
                                        Folder.ShortPath = Folder.ShortPath + spath[t] + "\\";
                                    }
                                }
                                break;
                            case 1:
                                if (spath.Length > 2)
                                {
                                    for (int t = 3; t < spath.Length; t++)
                                    {
                                        Folder.ShortPath = Folder.ShortPath + spath[t] + "\\";
                                    }
                                }
                                break;
                        }
                        

                        GetSubFolders(Folder, Folder.Level, limit, type);

                        if (!sFolder.subList.Contains(Folder)) sFolder.subList.Add(Folder);
                        if (!FullFoldersList.Contains(Folder)) FullFoldersList.Add(Folder);
                    }

                    count += 1;
                }
                catch { } 
            }
        }
        /// <summary>
        /// Перестроить дерево
        /// </summary>
        /// <param name="view"></param>
        public void CreateTreeView()
        {
            //View.Nodes.Clear();
            //for (int i = 0; i < mainFolders.Count; i++)
            //{
            //    View.Nodes.Add(mainFolders[i].dirNode);
            //    ///View.Nodes[i].Nodes.Add(AddChildNode(mainFolders[i], View.Nodes[i]));
            //    AddChildNode(mainFolders[i], View.Nodes[i]);
            //}
        }

        private void AddChildNode(ClassFolder folder, TreeNode sNode)
        {
            //TreeNode node = sNode;
            if (folder.subList.Count > 0)
            {
                for (int s = 0; s < folder.subList.Count; s++)
                {
                    if (!sNode.Nodes.Contains(folder.subList[s].dirNode)) sNode.Nodes.Add(folder.subList[s].dirNode);
                    if (folder.subList[s].subList.Count > 0)
                    {
                        AddChildNode(folder.subList[s], sNode.Nodes[s]);
                    }
                }
            }

            //return node;
        }

        public void CreateTreeView_New()
        {
            View.Nodes.Clear();
            
            TreeNode nodeProjects = new TreeNode("01_Проекты");
            nodeProjects.Tag = "Projects";
            View.Nodes.Add(nodeProjects);
            AddChildNode_Countries(nodeProjects);
           
            TreeNode nodeProducts = new TreeNode("02_Продукты");
            nodeProducts.Tag = "Products";
            View.Nodes.Add(nodeProducts);
            AddChildeNodes_ProductsCataloges(nodeProducts);
            
            TreeNode nodeTechSolution = new TreeNode("03_ТехРешения");
            nodeTechSolution.Tag = "TechSolutions";
            View.Nodes.Add(nodeTechSolution);
            AddChildeNodes_TechSolution(nodeTechSolution);
        }


        private void AddChildNode_Countries(TreeNode node)
        {
            var GroupByCounties = RequestInfo.lb.Projects.GroupBy(n => n.NationId);
            foreach (var itm in GroupByCounties)
            {
                var nation = RequestInfo.lb.Nations.FirstOrDefault(n => n.NationId == itm.Key);
                if (nation != null)
                {
                    if (true)
                    {
                        TreeNode countryNode = new TreeNode();
                        countryNode.Text = nation.NationId + "_" + nation.NationName;
                        countryNode.Tag = "Country";
                        node.Nodes.Add(countryNode);

                        foreach (var prj in itm)
                        {
                            if (prj.ProjectLinkId == null)
                            {
                                RecProject(RequestInfo.lb, prj.ProjectId, countryNode);
                            }
                        } 
                    }
                }

            }
        }

        private void AddChildeNodes_ProductsCataloges(TreeNode node)
        {
            foreach(var pgroup in RequestInfo.lb.ProductGroups)
            {
                TreeNode groupNode = new TreeNode();
                groupNode.Text = pgroup.ProductGroupCode;
                groupNode.Tag = "ProductsCatalog";
                node.Nodes.Add(groupNode);

                var lst = RequestInfo.lb.Products.Where(x => x.ProductGroupId == pgroup.ProductGroupId);
                foreach (var prod in lst)
                {
                    RecProducts(RequestInfo.lb, prod.ProductCode, prod.ProductId, groupNode);
                }
            }
        }

        private void AddChildeNodes_TechSolution(TreeNode node)
        {
            foreach (var pgroup in RequestInfo.lb.TechSolutionGroups)
            {
                TreeNode groupNode = new TreeNode();
                groupNode.Text = pgroup.TechSolutionGroupCode;
                groupNode.Tag = "TechSolutionsCatalog";
                node.Nodes.Add(groupNode);

                var lst = RequestInfo.lb.TechSolutions.Where(x => x.TechSolutionGroupId == pgroup.TechSolutionGroupId);
                foreach (var prod in lst)
                {
                    RecTechSolution(RequestInfo.lb, prod.TechSolutionCode, prod.TechSolutionId, groupNode);
                }
            }
        }

        public void SelectTreeNode(ClassFolder folder)
        {

            if (folder != null)
            {
                View.SelectedNode = folder.dirNode;
                folder.dirNode.Checked = true;
            }

            View.Focus();
        }

        public bool SelectTreeNode_New(int mode = 0)
        {
            try
            {
                int start = 2;
                if (mode == 1) start = 0;
                string[] split = GlobalData.SelectedDirPath.Split('\\');
                string fullPath = "";
                TreeNode treeNode = null;

                if (split.Length > 1)
                {
                    for (int i = start; i < split.Length - 1; i++)
                    {
                        fullPath += split[i];
                        if (mode == 0 && i == 2 || mode == 1 && i == 0)
                        {
                            foreach (TreeNode node in View.Nodes)
                            {
                                if (node.FullPath == fullPath)
                                {
                                    treeNode = node;
                                    fullPath += "\\";
                                    break;
                                }
                            }
                        }
                        else
                        {
                            treeNode = FindNode(treeNode, fullPath);
                            if (treeNode != null)
                            {
                                fullPath += "\\";
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                if (treeNode != null)
                {
                    View.SelectedNode = treeNode;
                    return true;
                }
                else
                {
                    try
                    {
                        View.SelectedNode = View.Nodes[0];
                    }
                    catch
                    {

                    }
                }
                View.Focus();
            }
            catch
            {
            }
            return false;
        }

        private TreeNode FindNode(TreeNode node, string path)
        {
            foreach (TreeNode n in node.Nodes)
            {
                if (n.FullPath == path)
                {
                    return n;
                }
            }
            return null;
        }

        

        public void AddSubfolder(ClassFolder folder, int type)
        {
            int startcount = folder.subList.Count();
            GetSubFolders(folder, folder.Level, folder.Level + 0, type);
            if (folder.subList.Count == startcount)
            {
                Thread.Sleep(5000);
                AddSubfolder(folder, type);
            }
        }

        public void ExpandNode(ClassFolder folder)
        {
            if (folder != null && folder.subList.Count > 0)
            {
                folder.dirNode.Expand();
            }

        }

        static void RecProject(LibraryDB.LibraryDB lb, string ProjectId, TreeNode treeNode, byte mode = 0, int stageid = -1)
        {
            TreeNode treeNodeChild = new TreeNode(ProjectId);
            treeNodeChild.Tag = "Project";
            if (mode == 1) treeNodeChild.Tag = "SubProject";
            treeNode.Nodes.Add(treeNodeChild);

            var linkProjects = lb.Projects.Where(x => x.ProjectLinkId == ProjectId).ToList();
            var linkPositions = lb.Positions.Where(x => x.ProjectId == ProjectId).ToList();

            Dictionary<int, List<string>> Pairs = new Dictionary<int, List<string>>();

            List<LibraryDB.DB.StageProject> _StageProjects = lb.StageProjects.Where(x => x.ProjectId == ProjectId).ToList();
            foreach (var _sp in _StageProjects)
            {
                if (_sp.StageId != 1)
                {
                    Pairs.Add(_sp.StageId.Value, new List<string>()); 
                }
            }
            foreach (var link in linkProjects)
            {
                List<LibraryDB.DB.StageProject> linkStageProjects = lb.StageProjects.Where(x => x.ProjectId == link.ProjectId).ToList();
                foreach (var stageP in linkStageProjects)
                {
                    try
                    {
                        if (!Pairs.ContainsKey(stageP.StageId.Value))
                        {
                            List<string> _value = new List<string>();
                            _value.Add(link.ProjectId);
                            Pairs.Add(stageP.StageId.Value, _value);
                        }
                        else
                        {
                            if (!Pairs[stageP.StageId.Value].Contains(link.ProjectId))
                            {
                                Pairs[stageP.StageId.Value].Add(link.ProjectId);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
            foreach (var position in linkPositions)
            {
                try
                {
                    if (!Pairs.ContainsKey(position.StageId.Value))
                    {
                        List<string> _value = new List<string>();
                        _value.Add(position.PositionCode);
                        Pairs.Add(position.StageId.Value, _value);
                    }
                    else
                    {
                        if (!Pairs[position.StageId.Value].Contains(position.PositionCode))
                        {
                            Pairs[position.StageId.Value].Add(position.PositionCode);
                        }
                    }
                }
                catch
                {

                }
            }

            //var lst = lb.Positions.Where(x => x.ProjectId == ProjectId).ToList();
            //var lstGroup = lst.GroupBy(x => x.StageId);
            //var spStage = lb.StageProjects.Where(x => x.ProjectId == ProjectId).ToList();
            foreach (var pos in Pairs)
            {
                pos.Value.Sort();
                if (mode == 0)
                {
                    var stage = lb.Stages.FirstOrDefault(x => x.StageId == pos.Key);
                    TreeNode treeNodeStage = new TreeNode($"{stage.StageIndex}_{stage.StageTag}");
                    //spStage.RemoveAll(x => x.StageId == stage.StageId);
                    treeNodeStage.Tag = "Stage";
                    treeNodeChild.Nodes.Add(treeNodeStage); 
                    foreach (var position in pos.Value)
                    {

                        if (linkProjects != null && linkProjects.FirstOrDefault(x => x.ProjectId == position) != null)
                        {
                            var linkProject = linkProjects.FirstOrDefault(x => x.ProjectId == position);
                            RecProject(lb, linkProject.ProjectId, treeNodeStage, 1, pos.Key);
                        }
                        else
                        {
                            TreeNode treeNodePosition = new TreeNode(position);
                            string tag = "Position";
                            treeNodePosition.Tag = tag;
                            treeNodeStage.Nodes.Add(treeNodePosition);
                        }
                    }
                }
                else
                {
                    foreach (var position in pos.Value)
                    {
                        if (linkProjects != null && linkProjects.FirstOrDefault(x => x.ProjectId == position) != null)
                        {
                            var linkProject = linkProjects.FirstOrDefault(x => x.ProjectId == position);
                            RecProject(lb, linkProject.ProjectId, treeNodeChild, 1, pos.Key);
                        }
                        else
                        {
                            TreeNode treeNodePosition = new TreeNode(position);
                            string tag = "SubPosition";
                            treeNodePosition.Tag = tag;
                            treeNodeChild.Nodes.Add(treeNodePosition);
                        }

                    }
                }
                
            }

            //if (spStage.Count != 0)
            //{
            //    foreach (var sPr in spStage)
            //    {
            //        if (sPr.StageId == 1) continue;
            //        var stage = lb.Stages.First(x => x.StageId == sPr.StageId);
            //        System.Windows.Forms.TreeNode treeNodeStage = new System.Windows.Forms.TreeNode($"{stage.StageIndex}_{stage.StageTag}");

            //        treeNodeStage.Tag = "Stage";
            //        treeNodeChild.Nodes.Add(treeNodeStage);
            //    }
            //}
        }

        //static void RecProject(LibraryDB.LibraryDB lb, string ProjectId, TreeNode treeNode)
        //{
        //    TreeNode treeNodeChild = new TreeNode(ProjectId);
        //    treeNodeChild.Tag = "Project";
        //    treeNode.Nodes.Add(treeNodeChild);
        //    var y = lb.Projects.Where(x => x.ProjectLinkId == ProjectId).ToList();
        //    if (y.Count != 0)
        //    {
        //        foreach (var tt in y)
        //        {
        //            //Console.WriteLine(ProjectId);
        //            RecProject(lb, tt.ProjectId, treeNodeChild);
        //        }
        //    }

        //    var lst = lb.Positions.Where(x => x.ProjectId == ProjectId).ToList();
        //    var lstGroup = lst.GroupBy(x => x.StageId);
        //    var spStage = lb.StageProjects.Where(x => x.ProjectId == ProjectId).ToList();
        //    foreach (var pos in lstGroup)
        //    {
        //        var stage = lb.Stages.First(x => x.StageId == pos.Key);
        //        TreeNode treeNodeStage = new TreeNode($"{stage.StageIndex}_{stage.StageTag}");
        //        spStage.RemoveAll(x => x.StageId == stage.StageId);
        //        treeNodeStage.Tag = "Stage";
        //        treeNodeChild.Nodes.Add(treeNodeStage);

        //        foreach (var position in pos)
        //        {
        //            TreeNode treeNodePosition = new TreeNode(position.PositionCode);
        //            treeNodePosition.Tag = "Position";
        //            treeNodeStage.Nodes.Add(treeNodePosition);
        //        }
        //    }

        //    if (spStage.Count != 0)
        //    {
        //        foreach (var sPr in spStage)
        //        {
        //            if (sPr.StageId == 1) continue;
        //            var stage = lb.Stages.First(x => x.StageId == sPr.StageId);
        //            System.Windows.Forms.TreeNode treeNodeStage = new System.Windows.Forms.TreeNode($"{stage.StageIndex}_{stage.StageTag}");

        //            treeNodeStage.Tag = "Stage";
        //            treeNodeChild.Nodes.Add(treeNodeStage);
        //        }
        //    }
        //}

        static void RecProducts(LibraryDB.LibraryDB lb, string ProductCode, int ProductId, System.Windows.Forms.TreeNode treeNode)
        {
            string[] txt = ProductCode.Split('-');
            string name = ProductCode;
            if (txt.Length > 1) name = txt[1];
            TreeNode treeNodeChild = new TreeNode(name);
            treeNodeChild.Tag = "Product";
            treeNode.Nodes.Add(treeNodeChild);
            

            var lst = lb.StageProducts.Where(x => x.ProductId == ProductId).ToList();
            var lstGroup = lst.GroupBy(x => x.StageId);
            foreach (var pos in lstGroup)
            {
                var stage = lb.Stages.First(x => x.StageId == pos.Key);
                TreeNode treeNodeStage = new TreeNode($"{stage.StageIndex}_{stage.StageTag}");
                treeNodeStage.Tag = "StageProduct";
                treeNodeChild.Nodes.Add(treeNodeStage);
            }
        }

        static void RecTechSolution (LibraryDB.LibraryDB lb, string Code, int Id, System.Windows.Forms.TreeNode treeNode)
        {
            string[] txt = Code.Split('-');
            string name = Code;
            if (txt.Length > 1) name = txt[1];
            TreeNode treeNodeChild = new TreeNode(name);
            treeNodeChild.Tag = "TechSolution";
            treeNode.Nodes.Add(treeNodeChild);


            var lst = lb.StageTechSolutions.Where(x => x.TechSolutionId == Id).ToList();
            var lstGroup = lst.GroupBy(x => x.StageId);
            foreach (var pos in lstGroup)
            {
                var stage = lb.Stages.First(x => x.StageId == pos.Key);
                TreeNode treeNodeStage = new TreeNode($"{stage.StageIndex}_{stage.StageTag}");
                treeNodeStage.Tag = "StageTechSolution";
                treeNodeChild.Nodes.Add(treeNodeStage);
            }
        }
    }
}
