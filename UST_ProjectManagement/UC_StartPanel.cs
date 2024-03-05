using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryDB.DB;
using System.Diagnostics;
using CardTask;
using Newtonsoft.Json;

namespace UST_ProjectManagement
{
    public partial class UC_StartPanel : UserControl
    {
        public delegate void UpdateForm_Click(bool status);
        public event UpdateForm_Click UpdateForm;

        public delegate void StartProcaess();
        public event StartProcaess StartProcess;

        UC_Search_Projects uC_Search_Projects;

        DataGridView dataGrid;
        ContextMenuStrip contextMenu = new ContextMenuStrip();
        ToolStripMenuItem stripItemOpen = new ToolStripMenuItem();
        ToolStripMenuItem stripItemOpenDir = new ToolStripMenuItem();
        ToolStripMenuItem stripItemGetPath = new ToolStripMenuItem();
        static MainForm mainForm;
        bool openFiltersPanel = false;
        List<string> header1;
        List<string> header2;
        int Mode = 0;
        public UC_StartPanel()
        {
            InitializeComponent();
            panel1.BackColor = MainForm.HeaderColor;
            panel5.BackColor = MainForm.HeaderColor;
            dataGrid = new DataGridView();
            dataGrid.Dock = DockStyle.Fill;
            dataGrid.BorderStyle = BorderStyle.None;
            dataGrid.Margin = new Padding(0);
            dataGrid.ScrollBars = ScrollBars.Vertical;
            dataGrid.SizeChanged += new EventHandler(DG_SizeChanged);
            dataGrid.MouseDown += dataGridView_MouseDown;
            tableLayoutPanel1.Controls.Add(dataGrid, 0, 3);
            header1 = new List<string>() { "Шифр", "Наименование", "Стадия", "ГИП", "ГАП", "Начало", "Конец", "%", "Тип", "", "" };
            header2 = new List<string>() { "Шифр", "Стадия", "Номер задания", "Наименование", "От раздела", "Для раздела", "Выдал", "Получил", "Статус", "taskId", "taskdepId" };
            CardTask.Methodes_DataGrid.CreateDataGrid(dataGrid, header1);

            stripItemOpen.Text = "Открыть карточку";
            stripItemOpen.Image = Properties.Resources.Btn_OpenCard_20x20;
            stripItemOpen.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemOpen);

            stripItemOpenDir.Text = "Открыть в проводнике";
            stripItemOpenDir.Image = Properties.Resources.Folder_20x20;
            stripItemOpenDir.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemOpenDir);

            stripItemGetPath.Text = "Получить путь";
            stripItemGetPath.Image = Properties.Resources.GetPath;
            stripItemGetPath.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemGetPath);

            uC_Search_Projects = new UC_Search_Projects();
            uC_Search_Projects.Dock = DockStyle.Fill;
            uC_Search_Projects.Margin = new Padding(0);
            panel4.Controls.Add(uC_Search_Projects);
            uC_Search_Projects.SearchObjects += UpdatePanels;
            if (!MainForm.firstStart)
            {
                try
                {
                    uC_Search_Projects.StartProcess += StProcess;
                }
                catch
                {
                } 
            }
        }

        public void ButtonSearch_Click()
        {
            uC_Search_Projects.Button_Click(uC_Search_Projects.button, EventArgs.Empty);
        }

        private void StProcess()
        {
            if (!MainForm.firstStart)
            {
                try
                {
                    GlobalData.loadInfo = "Поиск...";
                    StartProcess?.Invoke();
                }
                catch
                {
                } 
            }
        }

        public void UpdatePanels(string[,] filters = null)
        {
            //GlobalData.loadInfo = "Поиск...";
            //StartProcess?.Invoke();

            UpdateFilterPanel(openFiltersPanel, Mode);
            uC_Search_Projects.Visible = false;
            dataGrid.Rows.Clear();
            UpdateDataGridColumns();

            if (Mode == 0)
            {
                uC_Search_Projects.Visible = true;
                List<Project> sortP = RequestInfo.lb.Projects.OrderByDescending(x => x.ProjectId).ToList();
                if(filters != null)
                {
                    sortP = uC_Search_Projects.GetProjectsByFilter(filters, sortP);
                }
                
                
                foreach (Project project in sortP)
                {
                    var stagesList = RequestInfo.lb.StageProjects.FindAll(x => x.ProjectId == project.ProjectId);
                    foreach (StageProject stageProject in stagesList)
                    {
                        if (filters == null || filters[0,2] == "<Нет>" || filters[0, 2] == null || stageProject.StageId.Value.ToString() == filters[0,2])
                        {
                            Stage stage = RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == stageProject.StageId);
                            var positions = RequestInfo.lb.Positions.Where(p => p.ProjectId == project.ProjectId).Where(s => s.StageId == stage.StageId).OrderByDescending(i => i.PositionId).ToList();
                            if (filters != null && filters[0, 3] != null && filters[0, 3] != "<Нет>")
                            {
                                positions = positions.Where(x => x.PositionId.ToString() == filters[0, 3]).ToList();
                            }
                            if (filters != null && filters[0,4] != null && filters[0,4] != "<Нет>")
                            {
                                positions = positions.Where(x => x.PositionUserIdGIP.ToString() == filters[0, 4]).ToList();
                            }
                            if (filters != null && filters[0, 5] != null && filters[0, 5] != "<Нет>")
                            {
                                positions = positions.Where(x => x.PositionUserIdGAP.ToString() == filters[0, 5]).ToList();
                            }

                            foreach (Position position in positions)
                            {
                                PositionInfo positionInfo = new PositionInfo(project, stage, position);
                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(dataGrid);
                                row.Cells[0].Value = positionInfo.Code;
                                row.Cells[1].Value = positionInfo.PositionName;
                                row.Cells[2].Value = positionInfo.StageTag;
                                row.Cells[3].Value = positionInfo.GIP;
                                row.Cells[4].Value = positionInfo.GAP;
                                row.Cells[5].Value = positionInfo.StartDate;
                                row.Cells[6].Value = positionInfo.EndDate;
                                row.Cells[7].Value = positionInfo.PersentComplete;
                                row.Cells[8].Value = "Проекты";
                                row.Height = CardTask.Methodes_DataGrid.RowHeight;
                                dataGrid.Rows.Add(row);
                            } 
                        }

                    }
                }
            }
            else if (Mode == 1)
            {
                List<Product> sortP = RequestInfo.lb.Products.OrderByDescending(x => x.ProductId).ToList();
                foreach (Product product in sortP)
                {
                    var stagesList = RequestInfo.lb.StageProducts.FindAll(x => x.ProductId == product.ProductId);
                    foreach (StageProduct stageProduct in stagesList)
                    {
                        Stage stage = RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == stageProduct.StageId);

                        ProductInfo productInfo = new ProductInfo(product, stage);
                        DataGridViewRow row = new DataGridViewRow();

                        row.CreateCells(dataGrid);
                        row.Cells[0].Value = productInfo.Code;
                        row.Cells[1].Value = productInfo.Name;
                        row.Cells[2].Value = productInfo.StageTag;
                        row.Cells[3].Value = productInfo.Responsible;
                        row.Cells[4].Value = "";
                        row.Cells[5].Value = productInfo.StartDate;
                        row.Cells[6].Value = productInfo.EndDate;
                        row.Cells[7].Value = productInfo.PersentComplete;
                        row.Cells[8].Value = "Продукты";
                        row.Height = CardTask.Methodes_DataGrid.RowHeight;
                        dataGrid.Rows.Add(row);

                    }
                }
            }
            else if (Mode == 2)
            {
                List<TechSolution> sortP = RequestInfo.lb.TechSolutions.OrderByDescending(x => x.TechSolutionId).ToList();
                foreach (TechSolution techSolution in sortP)
                {
                    var stagesList = RequestInfo.lb.StageTechSolutions.FindAll(x => x.TechSolutionId == techSolution.TechSolutionId);
                    foreach (StageTechSolution stageTech in stagesList)
                    {
                        Stage stage = RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == stageTech.StageId);

                        TechSolutionInfo techSolutionInfo = new TechSolutionInfo(techSolution, stage);
                        DataGridViewRow row = new DataGridViewRow();

                        row.CreateCells(dataGrid);
                        row.Cells[0].Value = techSolutionInfo.Code;
                        row.Cells[1].Value = techSolutionInfo.Name;
                        row.Cells[2].Value = techSolutionInfo.StageTag;
                        row.Cells[3].Value = techSolutionInfo.Responsible;
                        row.Cells[4].Value = "";
                        row.Cells[5].Value = techSolutionInfo.StartDate;
                        row.Cells[6].Value = techSolutionInfo.EndDate;
                        row.Cells[7].Value = techSolutionInfo.PersentComplete;
                        row.Cells[8].Value = "Тех.решения";
                        row.Height = CardTask.Methodes_DataGrid.RowHeight;
                        dataGrid.Rows.Add(row);

                    }
                }
                
            }
            else if (Mode == 3)
            {
                uC_Search_Projects.Visible = true;
                List<Project> sortP = RequestInfo.lb.Projects.OrderByDescending(x => x.ProjectId).ToList();
                if (filters != null)
                {
                    sortP = uC_Search_Projects.GetProjectsByFilter(filters, sortP);
                }
                try
                {
                    List<LibraryDB.DB.Task> tasks = RequestInfo.lb.Tasks.ToList().OrderByDescending(x => x.TaskId).ToList();
                    foreach (LibraryDB.DB.Task task in tasks)
                    {
                        if(filters != null && filters[0,4] != null && filters[0, 4] != "<Нет>" && task.SectionThreeId.ToString() != filters[0, 4])
                        {
                            continue;
                        }

                        Position position = RequestInfo.lb.Positions.FirstOrDefault(x => x.PositionId == task.PositionId);
                        if (filters != null && filters[0, 3] != null && filters[0, 3] != "<Нет>" && position.PositionId.ToString() != filters[0, 3])
                        {
                            continue;
                        }
                        Project project = sortP.FirstOrDefault(x => x.ProjectId == position.ProjectId);
                        Stage stage = RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == position.StageId);


                        if (filters != null && filters[0, 2] != null && filters[0, 2] != "<Нет>")
                        {
                            if (stage.StageId.ToString() != filters[0, 2])
                            {
                                stage = null;
                            }
                        }

                        if (project != null && stage != null)
                        {
                            var taskDepartments = RequestInfo.lb.TaskDepartments.Where(x => x.TaskId == task.TaskId);  
                            SectionsThree sectionFrom = RequestInfo.lb.SectionsThrees.FirstOrDefault(x => x.SectionThreeId == task.SectionThreeId);
                            User userFrom = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId == task.TaskUserId);
                            foreach (TaskDepartment taskDepartment in taskDepartments)
                            {
                                SectionsThree sectionTo = RequestInfo.lb.SectionsThrees.FirstOrDefault(x => x.SectionThreeId == taskDepartment.SectionThreeId);
                                if (filters != null && filters[0, 5] != null && filters[0, 5] != "<Нет>" && sectionTo.SectionThreeId.ToString() != filters[0, 5])
                                {
                                    continue;
                                }
                                User userTo = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId == taskDepartment.TaskDepartmentUserId);
                                Status status = RequestInfo.lb.Status.FirstOrDefault(x => x.StatusId == taskDepartment.StatusId);

                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(dataGrid);
                                row.Cells[0].Value = position.PositionCode;
                                row.Cells[1].Value = stage.StageTag;
                                row.Cells[2].Value = $"{task.TaskNumer}-{task.TaskYear}";
                                row.Cells[3].Value = task.TaskName;
                                if (stage.LanguageId == 2)
                                {
                                    row.Cells[4].Value = sectionFrom.SectionThreeTagEng;
                                    row.Cells[5].Value = sectionTo.SectionThreeTagEng;
                                }
                                else
                                {
                                    row.Cells[4].Value = sectionFrom.SectionThreeTagRus;
                                    row.Cells[5].Value = sectionTo.SectionThreeTagRus;
                                }
                                row.Cells[6].Value = $"{userFrom.UserSurname} {userFrom.UserName}";
                                row.Cells[7].Value = $"{userTo.UserSurname} {userTo.UserName}";
                                row.Cells[8].Value = status.StatusName;
                                row.Cells[8].Style.BackColor = GlobalMethodes.GetCellColor(status.StatusId, 0);
                                row.Cells[9].Value = task.TaskId;
                                row.Cells[10].Value = taskDepartment.TaskDepartmentId;
                                row.Height = CardTask.Methodes_DataGrid.RowHeight;
                                dataGrid.Rows.Add(row);
                            } 
                        }



                    }
                }
                catch
                {

                }
               
            }
            DG_SizeChanged(dataGrid, EventArgs.Empty);
            GlobalMethodes._stop = true;
        }

        private void UpdateDataGridColumns()
        {
            List<string> header = new List<string>();
            dataGrid.ColumnHeadersHeight = 40;
            if (Mode < 3)
            {
                header = header1;   
            }
            else
            {
                header = header2;
            }
            for (int c = 0; c < dataGrid.Columns.Count; c++)
            {
                dataGrid.Columns[c].HeaderText = header[c];
                if (c < 9)
                {
                    dataGrid.Columns[c].Visible = true; 
                }
                else
                {
                    dataGrid.Columns[c].Visible = false;
                }
            }
            if (Mode == 1 || Mode == 2)
            {
                dataGrid.Columns[4].Visible = false;
            }
            else if (Mode == 3)
            {
                //dataGrid.Columns[8].Visible = false;
            }
        }

        private void UpdateFilterPanel(bool open, int mode)
        {
            if (open)
            {
                button1.Text = "˄";
                panel4.Visible = true;
                tableLayoutPanel1.RowStyles[1].Height = 70;
                if (uC_Search_Projects.IsCreatePanels) uC_Search_Projects.CreatePanels(mode);
            }
            else
            {
                button1.Text = "˅";
                panel4.Visible = false;
                tableLayoutPanel1.RowStyles[1].Height = 0;
            }
        }

        public void DG_SizeChanged(object sender, EventArgs e)
        {
            int cWidth = 0;
            if (Mode < 3)
            {
                try
                {
                    if (dataGrid.Columns[0].Visible)
                    {
                        dataGrid.Columns[0].Width = 150;
                        cWidth += dataGrid.Columns[0].Width;
                    }
                    if (dataGrid.Columns[2].Visible)
                    {
                        dataGrid.Columns[2].Width = 50;
                        cWidth += dataGrid.Columns[2].Width;
                    }
                    if (dataGrid.Columns[3].Visible)
                    {
                        dataGrid.Columns[3].Width = 220;
                        cWidth += dataGrid.Columns[3].Width;
                    }
                    if (dataGrid.Columns[4].Visible)
                    {
                        dataGrid.Columns[4].Width = 220;
                        cWidth += dataGrid.Columns[4].Width;
                    }
                    if (dataGrid.Columns[5].Visible)
                    {
                        dataGrid.Columns[5].Width = 100;
                        cWidth += dataGrid.Columns[5].Width;
                    }
                    if (dataGrid.Columns[6].Visible)
                    {
                        dataGrid.Columns[6].Width = 100;
                        cWidth += dataGrid.Columns[6].Width;
                    }
                    if (dataGrid.Columns[7].Visible)
                    {
                        dataGrid.Columns[7].Width = 50;
                        cWidth += dataGrid.Columns[7].Width;
                    }
                    if (dataGrid.Columns[8].Visible)
                    {
                        dataGrid.Columns[8].Width = 100;
                        cWidth += dataGrid.Columns[8].Width;
                    };
                    dataGrid.Columns[1].Width = dataGrid.Width - (cWidth + 0);

                }
                catch { } 
            }
            else
            {
                try
                {
                    if (dataGrid.Columns[0].Visible)
                    {
                        dataGrid.Columns[0].Width = 120;
                        cWidth += dataGrid.Columns[0].Width;
                    }
                    if (dataGrid.Columns[1].Visible)
                    {
                        dataGrid.Columns[1].Width = 80;
                        cWidth += dataGrid.Columns[1].Width;
                    }
                    if (dataGrid.Columns[2].Visible)
                    {
                        dataGrid.Columns[2].Width = 80;
                        cWidth += dataGrid.Columns[2].Width;
                    }
                    if (dataGrid.Columns[4].Visible)
                    {
                        dataGrid.Columns[4].Width = 80;
                        cWidth += dataGrid.Columns[4].Width;
                    }
                    if (dataGrid.Columns[5].Visible)
                    {
                        dataGrid.Columns[5].Width = 80;
                        cWidth += dataGrid.Columns[5].Width;
                    }
                    if (dataGrid.Columns[6].Visible)
                    {
                        dataGrid.Columns[6].Width = 170;
                        cWidth += dataGrid.Columns[6].Width;
                    }
                    if (dataGrid.Columns[7].Visible)
                    {
                        dataGrid.Columns[7].Width = 170;
                        cWidth += dataGrid.Columns[7].Width;
                    }
                    if (dataGrid.Columns[8].Visible)
                    {
                        dataGrid.Columns[8].Width = 120;
                        cWidth += dataGrid.Columns[8].Width;
                    };
                    dataGrid.Columns[3].Width = dataGrid.Width - (cWidth + 0);

                }
                catch { }
            }
            
        }

        public void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton.Checked)
            {
                StProcess();
                if (Mode != radioButton.TabIndex)
                {
                    Mode = radioButton.TabIndex;
                    //uC_Search_Projects.CleareFilters(Mode);
                    uC_Search_Projects.CreatePanels(Mode);
                }

                try
                {
                    UpdatePanels(GlobalData.FilterList[Mode]);
                }
                catch
                {
                    UpdatePanels();
                }
            }
            
        }


        private void dataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (radioButton4.Checked)
                {
                    stripItemOpen.Text = "Открыть карточку проекта";
                    stripItemOpen.Image = Properties.Resources.Btn_OpenCard_20x20;

                    stripItemOpenDir.Text = "Открыть карточку задания";
                    stripItemOpenDir.Image = Properties.Resources.Btn_OpenCard_20x20;

                    stripItemGetPath.Text = "Изменить статус";
                    stripItemGetPath.Image = Properties.Resources.Btn_Promote_20x20;
                }
                else
                {
                    stripItemOpen.Text = "Открыть карточку";
                    stripItemOpen.Image = Properties.Resources.Btn_OpenCard_20x20;

                    stripItemOpenDir.Text = "Открыть в проводнике";
                    stripItemOpenDir.Image = Properties.Resources.Folder_20x20;

                    stripItemGetPath.Text = "Получить путь";
                    stripItemGetPath.Image = Properties.Resources.GetPath;
                }

                var ht = dataGrid.HitTest(e.X, e.Y);
                foreach (DataGridViewCell cell in dataGrid.SelectedCells)
                {
                    if (ht.RowIndex == cell.RowIndex)
                    {
                        contextMenu.Show(MousePosition);
                        break;
                    }
                }
            }
        }

        private void toolStripItem_Click(object sender, EventArgs args)
        {
            int index = 0;
            string code = "";
            string stage = "";

            if (radioButton1.Checked)
            {
                index = 0;
                code = dataGrid.SelectedRows[0].Cells[0].Value.ToString();
                stage = dataGrid.SelectedRows[0].Cells[2].Value.ToString();
            }
            else if (radioButton2.Checked)
            {
                index = 1;
                string[] sCode = dataGrid.SelectedRows[0].Cells[0].Value.ToString().Split('-');
                code = sCode[sCode.Length - 1];
                stage = dataGrid.SelectedRows[0].Cells[2].Value.ToString();
            }
            else if (radioButton3.Checked)
            {
                index = 2;
                code = dataGrid.SelectedRows[0].Cells[0].Value.ToString();
                stage = dataGrid.SelectedRows[0].Cells[2].Value.ToString();
            }
            else if (radioButton4.Checked)
            {
                index = 0;
                code = dataGrid.SelectedRows[0].Cells[0].Value.ToString();
                stage = dataGrid.SelectedRows[0].Cells[1].Value.ToString();
            }

            List<TreeNode> existingNodes = SearchTreeNodes(code, GlobalData.NaviTreeView.View.Nodes[index], 0);

            if (existingNodes.Count > 0)
            {
                foreach (TreeNode node in existingNodes)
                {
                    if (index == 0)
                    {
                        string[] sPath = node.FullPath.Split('\\');
                        string stageCode = sPath[sPath.Length - 2];
                        string[] sStageCode = stageCode.Split('_');
                        string Stage = sStageCode[sStageCode.Length - 1];
                        if (Stage == stage)
                        {
                            if (sender.ToString() == stripItemOpen.Text)
                            {
                                GlobalData.NaviTreeView.View.SelectedNode = node;
                                GlobalData.NaviTreeView.View.Focus();
                            }
                            else if (sender.ToString() == stripItemOpenDir.Text)
                            {
                                if (!radioButton4.Checked)
                                {
                                    string NodePath = MainForm.MainPath_rezerv + node.FullPath + @"\";
                                    Process.Start(new ProcessStartInfo("explorer.exe", " /e, " + NodePath));
                                    GlobalMethodes.CreateLog("Открыть в Проводнике"); 
                                }
                                else
                                {
                                    try
                                    {
                                        int tdId = Convert.ToInt32(dataGrid.SelectedRows[0].Cells[10].Value);
                                        var TD = RequestInfo.lb.TaskDepartments.FirstOrDefault(x => x.TaskDepartmentId == tdId);
                                        if (TD != null)
                                        {
                                            PublishForm general;
                                            PublishForm.mode = 2;
                                            PublishForm.modeApplication = 1;
                                            general = new PublishForm();
                                            general.StartPosition = FormStartPosition.CenterParent;
                                            general.dataGridView_Files.CellContentDoubleClick += new DataGridViewCellEventHandler(general.dataGridView_Files_CellMouseDoubleClick);


                                            general.GetTaskInfo(TD.TaskDepartmentId);

                                            if (general.ShowDialog() == DialogResult.OK)
                                            {

                                            }
                                        }
                                    }
                                    catch 
                                    {
                                    }
                                }
                            }
                            else if (sender.ToString() == stripItemGetPath.Text)
                            {
                                if (!radioButton4.Checked)
                                {
                                    Clipboard.SetText(MainForm.MainPath_rezerv + node.FullPath + @"\"); 
                                }
                                else
                                {
                                    string[] infos = PublishForm.PostRequest("InfoThree", null, @"http://10.10.25.130:8085/Info/");

                                    LibraryDB.LibraryDB lb = null;
                                    if (infos[0] == "Готово")
                                    {
                                        lb = JsonConvert.DeserializeObject<LibraryDB.LibraryDB>(infos[1]);
                                    }

                                    GetTaskForm.modeTest = "0";
                                    GetTaskForm general = new GetTaskForm();
                                    general.StartPosition = FormStartPosition.CenterParent;
                                    general.lb = lb;
                                    general.textBox1.Text = dataGrid.SelectedRows[0].Cells[0].Value.ToString();
                                    general.textBox2.Text = dataGrid.SelectedRows[0].Cells[1].Value.ToString();
                                    general.textBox3.Text = dataGrid.SelectedRows[0].Cells[2].Value.ToString();
                                    general.textBox4.Text = dataGrid.SelectedRows[0].Cells[4].Value.ToString();

                                    general.Test();

                                    DialogResult result = general.ShowDialog();
                                    if (result == DialogResult.OK)
                                    {
                                        RequestInfo.requestInfoThree();
                                        try
                                        {
                                            UpdatePanels(GlobalData.FilterList[radioButton4.TabIndex]);
                                        }
                                        catch
                                        {
                                            UpdatePanels();
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                    else
                    {
                        if (node.Nodes != null && node.Nodes.Count > 0)
                        {
                            if (sender.ToString() == stripItemOpen.Text)
                            {
                                GlobalData.NaviTreeView.View.SelectedNode = node.Nodes[0];
                                GlobalData.NaviTreeView.View.Focus();
                            }
                            else if (sender.ToString() == stripItemOpenDir.Text)
                            {
                                string NodePath = MainForm.MainPath_rezerv + node.Nodes[0].FullPath + @"\";
                                Process.Start(new ProcessStartInfo("explorer.exe", " /e, " + NodePath));
                                GlobalMethodes.CreateLog("Открыть в Проводнике");
                            }
                            else if (sender.ToString() == stripItemGetPath.Text)
                            {
                                Clipboard.SetText(MainForm.MainPath_rezerv + node.Nodes[0].FullPath + @"\");
                            }
                        }
                        else
                        {
                            if (sender.ToString() == stripItemOpen.Text)
                            {
                                GlobalData.NaviTreeView.View.SelectedNode = node;
                                GlobalData.NaviTreeView.View.Focus();
                            }
                            else if (sender.ToString() == stripItemOpenDir.Text)
                            {
                                string NodePath = MainForm.MainPath_rezerv + node.FullPath + @"\";
                                Process.Start(new ProcessStartInfo("explorer.exe", " /e, " + NodePath));
                                GlobalMethodes.CreateLog("Открыть в Проводнике");
                            }
                            else if (sender.ToString() == stripItemGetPath.Text)
                            {
                                Clipboard.SetText(MainForm.MainPath_rezerv + node.FullPath + @"\");
                            }
                        }
                        break;
                    }
                }

            }
            else
            {
                MessageBox.Show($"{code} не найден", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public List<TreeNode> SearchTreeNodes(string code, TreeNode node, int index)
        {
            List<TreeNode> result = new List<TreeNode>();
            for (int i = index; i < node.Nodes.Count; i++)
            {
                if (node.Nodes[i].Text == code)
                {
                    result.Add(node.Nodes[i]);
                }
                else
                {
                    if (node.Nodes[i].Nodes != null && node.Nodes[i].Nodes.Count > 0)
                    {
                        result.AddRange(SearchTreeNodes(code, node.Nodes[i], 0));
                    }
                }
            }
            return result;
        }

        private TreeNode GetTreeViewNode(TreeNode node)
        {
            return null;
        }
        /// <summary>
        /// OpenFilterPanel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFiltersPanel)
            {
                openFiltersPanel = false; 
            }
            else
            {
                openFiltersPanel = true;
            }
            UpdateFilterPanel(openFiltersPanel, Mode);
        }
    }
}
