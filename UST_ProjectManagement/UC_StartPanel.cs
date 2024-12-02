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
using System.IO;

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
        bool openFiltersPanel = true;
        List<string> header1;
        List<string> header2;
        List<string> header3;
        List<string> header4;
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
            header1 = new List<string>() { "Проект", "Шифр", "Наименование", "Стадия", "ГИП", "ГАП", "Начало", "Конец", "%", "Тип", "ID", "" };
            header2 = new List<string>() { "Проект", "Шифр", "Стадия", "Номер задания", "Наименование", "От раздела", "Для раздела", "Выдал", "Получил", "Статус", "Task Id", "Task Dep Id" };
            header3 = new List<string>() { "Шифр", "Наименование", "Стадия", "Автор", "Наименование элемента", "Обновлено", "Тип", "Id",  "", "", "", ""};
            header4 = new List<string>() { "Шифр", "Наименование", "Стадия", "ГИП", "ГАП", "Начало", "Конец", "%", "Тип", "ID", "", ""};
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
            uC_Search_Projects.Dock = DockStyle.Bottom;
            uC_Search_Projects.Height = panel4.Height - panel5.Height;
            uC_Search_Projects.Margin = new Padding(0);
            panel4.Controls.Add(uC_Search_Projects);
            uC_Search_Projects.comboBoxSelectIndexChanged += UpdatePanels_1;
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

        private void UpdatePanels_1()
        {
            UpdatePanels();
        }

        public void UpdatePanels(string[,] filters = null)
        {
            GlobalData.loadInfo = "Поиск...";
            StartProcess?.Invoke();

            //UpdateFilterPanel(openFiltersPanel, Mode);
            uC_Search_Projects.Visible = false;
            dataGrid.Rows.Clear();
            UpdateDataGridColumns();

            try
            {
                for (int f = 0; f < Filters.filterColumns[Mode].Count; f++)
                {
                    Filters.filterItems[Mode][f].Clear();
                }
            }
            catch { }

            if (Mode == 0)
            {
                uC_Search_Projects.Visible = true;
                List<Project> sortP = RequestInfo.lb.Projects.OrderByDescending(x => x.ProjectId).ToList();
                foreach (Project project in sortP)
                {
                    var stagesList = RequestInfo.lb.StageProjects.FindAll(x => x.ProjectId == project.ProjectId);
                    foreach (StageProject stageProject in stagesList)
                    {
                        Stage stage = RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == stageProject.StageId);
                        var positions = RequestInfo.lb.Positions.Where(p => p.ProjectId == project.ProjectId).Where(s => s.StageId == stage.StageId).OrderByDescending(i => i.PositionId).ToList();

                        foreach (Position position in positions)
                        {
                            PositionInfo positionInfo = new PositionInfo(project, stage, position);
                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataGrid);
                            row.Cells[0].Value = position.ProjectId;
                            row.Cells[1].Value = positionInfo.Code;
                            row.Cells[2].Value = positionInfo.PositionName;
                            row.Cells[3].Value = positionInfo.StageTag;
                            row.Cells[4].Value = positionInfo.GIP;
                            row.Cells[5].Value = positionInfo.GAP;
                            row.Cells[6].Value = positionInfo.StartDate;
                            row.Cells[7].Value = positionInfo.EndDate;
                            row.Cells[8].Value = positionInfo.PersentComplete;
                            row.Cells[9].Value = "Проекты";
                            row.Cells[10].Value = positionInfo.ID;
                            row.Height = CardTask.Methodes_DataGrid.RowHeight;
                            dataGrid.Rows.Add(row);

                            for (int f = 0; f < Filters.filterColumns[Mode].Count; f++)
                            {
                                int index = Filters.filterColumns[Mode][f];
                                if (Filters.filters[Mode].ContainsKey(f))
                                {
                                    if (Filters.filters[Mode][f] != "" && Filters.filters[Mode][f] != "<Нет>")
                                    {
                                        if (row.Cells[index].Value.ToString() != Filters.filters[Mode][f])
                                        {
                                            row.Visible = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (row.Visible)
                            {
                                for (int f = 0; f < Filters.filterColumns[Mode].Count; f++)
                                {
                                    int index = Filters.filterColumns[Mode][f];
                                    if (!Filters.filterItems[Mode].ContainsKey(f))
                                    {
                                        Filters.filterItems[Mode].Add(f, new List<string>());
                                        //Filters.filterItems[Mode][f] = new List<string>();
                                    }

                                    if (!Filters.filterItems[Mode][f].Contains(row.Cells[index].Value.ToString()))
                                    {
                                        Filters.filterItems[Mode][f].Add(row.Cells[index].Value.ToString());
                                    }
                                }
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
                        row.Cells[9].Value = productInfo.ID;
                        row.Height = CardTask.Methodes_DataGrid.RowHeight;
                        dataGrid.Rows.Add(row);

                        for (int f = 0; f < Filters.filterColumns[Mode].Count; f++)
                        {
                            int index = Filters.filterColumns[Mode][f];
                            if (Filters.filters[Mode].ContainsKey(f))
                            {
                                if (Filters.filters[Mode][f] != "" && Filters.filters[Mode][f] != "<Нет>")
                                {
                                    if (row.Cells[index].Value.ToString() != Filters.filters[Mode][f])
                                    {
                                        row.Visible = false;
                                        break;
                                    }
                                }
                            }
                        }
                        if (row.Visible)
                        {
                            for (int f = 0; f < Filters.filterColumns[Mode].Count; f++)
                            {
                                int index = Filters.filterColumns[Mode][f];
                                if (!Filters.filterItems[Mode].ContainsKey(f))
                                {
                                    Filters.filterItems[Mode].Add(f, new List<string>());
                                    //Filters.filterItems[Mode][f] = new List<string>();
                                }

                                if (!Filters.filterItems[Mode][f].Contains(row.Cells[index].Value.ToString()))
                                {
                                    Filters.filterItems[Mode][f].Add(row.Cells[index].Value.ToString());
                                }
                            }
                        }
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
                        row.Cells[9].Value = techSolutionInfo.ID;
                        row.Height = CardTask.Methodes_DataGrid.RowHeight;
                        dataGrid.Rows.Add(row);

                        for (int f = 0; f < Filters.filterColumns[Mode].Count; f++)
                        {
                            int index = Filters.filterColumns[Mode][f];
                            if (Filters.filters[Mode].ContainsKey(f))
                            {
                                if (Filters.filters[Mode][f] != "" && Filters.filters[Mode][f] != "<Нет>")
                                {
                                    if (row.Cells[index].Value.ToString() != Filters.filters[Mode][f])
                                    {
                                        row.Visible = false;
                                        break;
                                    }
                                }
                            }
                        }
                        if (row.Visible)
                        {
                            for (int f = 0; f < Filters.filterColumns[Mode].Count; f++)
                            {
                                int index = Filters.filterColumns[Mode][f];
                                if (!Filters.filterItems[Mode].ContainsKey(f))
                                {
                                    Filters.filterItems[Mode].Add(f, new List<string>());
                                    //Filters.filterItems[Mode][f] = new List<string>();
                                }

                                if (!Filters.filterItems[Mode][f].Contains(row.Cells[index].Value.ToString()))
                                {
                                    Filters.filterItems[Mode][f].Add(row.Cells[index].Value.ToString());
                                }
                            }
                        }
                    }
                }
                
            }
            else if (Mode == 3)
            {
                uC_Search_Projects.Visible = true;
                
                //if (filters != null)
                //{
                //    sortP = uC_Search_Projects.GetProjectsByFilter(filters, sortP);
                //}

                try
                {
                    for (int f = 0; f < Filters.filterColumns[Mode].Count; f++)
                    {
                        Filters.filterItems[Mode][f].Clear();
                        //Filters.filterItems[Mode][f].Add("<Нет>");
                    }
                }
                catch { }

                try
                {
                    List<LibraryDB.DB.Task> tasks = RequestInfo.lb.Tasks.ToList().OrderByDescending(x => x.TaskId).ToList();
                    foreach (LibraryDB.DB.Task task in tasks)
                    {
                        try
                        {
                            List<Project> sortP = RequestInfo.lb.Projects.OrderByDescending(x => x.ProjectId).ToList();
                            Position position = RequestInfo.lb.Positions.FirstOrDefault(x => x.PositionId == task.PositionId);
                            Project project = sortP.FirstOrDefault(x => x.ProjectId == position.ProjectId);
                            Stage stage = RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == position.StageId);

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
                                    row.Cells[0].Value = position.ProjectId;
                                    row.Cells[1].Value = position.PositionCode;
                                    row.Cells[2].Value = stage.StageTag;
                                    row.Cells[3].Value = $"{task.TaskNumer}-{task.TaskYear}";
                                    row.Cells[4].Value = task.TaskName;
                                    if (stage.LanguageId == 2)
                                    {
                                        row.Cells[5].Value = sectionFrom.SectionThreeTagEng;
                                        row.Cells[6].Value = sectionTo.SectionThreeTagEng;
                                    }
                                    else
                                    {
                                        row.Cells[5].Value = sectionFrom.SectionThreeTagRus;
                                        row.Cells[6].Value = sectionTo.SectionThreeTagRus;
                                    }
                                    row.Cells[7].Value = $"{userFrom.UserSurname} {userFrom.UserName}";
                                    if (userTo.UserSurname != "-")
                                    {
                                        row.Cells[8].Value = $"{userTo.UserSurname} {userTo.UserName}";
                                    }
                                    else
                                    {
                                        row.Cells[8].Value = GetAsignie(task, taskDepartment, status);
                                    }
                                    row.Cells[9].Value = status.StatusName;
                                    row.Cells[9].Style.BackColor = GlobalMethodes.GetCellColor(status.StatusId, 0);
                                    row.Cells[10].Value = task.TaskId;
                                    row.Cells[11].Value = taskDepartment.TaskDepartmentId;
                                    row.Height = CardTask.Methodes_DataGrid.RowHeight;
                                    dataGrid.Rows.Add(row);

                                    for (int f = 0; f < Filters.filterColumns[Mode].Count; f++)
                                    {
                                        int index = Filters.filterColumns[Mode][f];
                                        if (Filters.filters[Mode].ContainsKey(f))
                                        {
                                            if (Filters.filters[Mode][f] != "" && Filters.filters[Mode][f] != "<Нет>")
                                            {
                                                if (row.Cells[index].Value.ToString() != Filters.filters[Mode][f])
                                                {
                                                    row.Visible = false;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (row.Visible)
                                    {
                                        for (int f = 0; f < Filters.filterColumns[Mode].Count; f++)
                                        {
                                            int index = Filters.filterColumns[Mode][f];
                                            if (!Filters.filterItems[Mode].ContainsKey(f))
                                            {
                                                Filters.filterItems[Mode].Add(f, new List<string>());
                                                //Filters.filterItems[Mode][f] = new List<string>();
                                            }

                                            if (!Filters.filterItems[Mode][f].Contains(row.Cells[index].Value.ToString()))
                                            {
                                                Filters.filterItems[Mode][f].Add(row.Cells[index].Value.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                }
                catch
                {

                }
            }
            else if (Mode == 4)
            {
                uC_Search_Projects.Visible = true;
                var invNWDs = RequestInfo.lb.InventorToNwds.Where(x => x.PositionId != null);
                List<int> pIds = invNWDs.Select(x => x.PositionId.Value).Distinct().ToList();

                try
                {
                    for (int f = 0; f < Filters.filterColumns[Mode].Count; f++)
                    {
                        Filters.filterItems[Mode][f].Clear();
                    }
                }
                catch {}
                try
                {
                    List<string> nwdpathList = new List<string>();
                    foreach (int id in pIds)
                    {
                        try
                        {
                            Position position = RequestInfo.lb.Positions.FirstOrDefault(x => x.PositionId == id);
                            Project project = RequestInfo.lb.Projects.FirstOrDefault(x => x.ProjectId == position.ProjectId);
                            Stage stage = RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == position.StageId);
                            List < InventorToNwd > inventors = RequestInfo.lb.InventorToNwds.Where(z => z.Account != "-").Where(x => x.PositionId == id).ToList();
                            List<InventorDwg> inventorDwgs = RequestInfo.lb.InventorDwgs.Where(z => z.Account != "-").Where(x => x.PositionId == id).ToList();

                            foreach(InventorDwg dwg in inventorDwgs)
                            {
                                InventorToNwd item = new InventorToNwd();
                                item.InventorToNwdId = dwg.InventorDwgId;
                                item.PositionId = dwg.PositionId;
                                item.InventorToNwdInfo = dwg.InventorDwgInfo;
                                item.Account = dwg.Account;
                                item.UserSurname = dwg.UserSurname;
                                item.Name = dwg.Name;
                                item.MidlName = dwg.MidlName;
                                item.InventorToNwdDate = dwg.InventorToDwgDate;
                                item.InventorToNwdLastWriteTime = dwg.InventorToDwgLastWriteTime;
                                inventors.Add(item);
                            }
                            inventors = inventors.OrderByDescending(x => Convert.ToDateTime(x.InventorToNwdLastWriteTime)).ToList();


                            if (inventors != null && inventors.Count > 0)
                            {
                                string _path = inventors.FirstOrDefault().InventorToNwdInfo;
                                string path = "";
                                string date = "";
                                if (GetNWDfileInfo(project.ProjectId, stage.StageTag, out date, out path) && !nwdpathList.Contains(path))
                                {
                                    DataGridViewRow row = new DataGridViewRow();
                                    nwdpathList.Add(path);
                                    row.CreateCells(dataGrid);
                                    row.Cells[0].Value = project.ProjectId;
                                    row.Cells[1].Value = project.ProjectName;
                                    row.Cells[2].Value = stage.StageTag;
                                    row.Cells[3].Value = "BIM-модель";
                                    row.Cells[4].Value = project.ProjectName;
                                    row.Cells[5].Value = date;
                                    row.Cells[6].Value = "Проект";
                                    row.Cells[7].Value = "-";
                                    row.Cells[8].Value = path;
                                    row.Height = 30;
                                    dataGrid.Rows.Add(row);

                                    for (int f = 0; f < Filters.filterColumns[Mode].Count; f++)
                                    {
                                        int index = Filters.filterColumns[Mode][f];
                                        if (Filters.filters[Mode].ContainsKey(f))
                                        {
                                            if (Filters.filters[Mode][f] != "" && Filters.filters[Mode][f] != "<Нет>")
                                            {
                                                if (row.Cells[index].Value.ToString() != Filters.filters[Mode][f])
                                                {
                                                    row.Visible = false;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (row.Visible)
                                    {
                                        for (int f = 0; f < Filters.filterColumns[Mode].Count; f++)
                                        {
                                            int index = Filters.filterColumns[Mode][f];
                                            if (!Filters.filterItems[Mode].ContainsKey(f))
                                            {
                                                Filters.filterItems[Mode].Add(f, new List<string>());
                                            }

                                            if (!Filters.filterItems[Mode][f].Contains(row.Cells[index].Value.ToString()))
                                            {
                                                Filters.filterItems[Mode][f].Add(row.Cells[index].Value.ToString());
                                            }
                                        }
                                    }
                                }
                            }


                            foreach (InventorToNwd nwd in inventors)
                            {
                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(dataGrid);
                                row.Cells[0].Value = position.PositionCode;
                                row.Cells[1].Value = position.PositionName;
                                row.Cells[2].Value = stage.StageTag;
                                row.Cells[3].Value = nwd.UserSurname + " " + nwd.Name + " " + nwd.MidlName;
                                string name = "-";
                                string path = "";
                                string type = "";
                                GetInventorInfo(nwd.InventorToNwdInfo, out name, out path, out type);
                                row.Cells[4].Value = name;
                                row.Cells[5].Value = nwd.InventorToNwdLastWriteTime;
                                row.Cells[6].Value = type;
                                row.Cells[7].Value = nwd.InventorToNwdId;
                                row.Cells[8].Value = path;
                                row.Height = 30;
                                dataGrid.Rows.Add(row);

                                for (int f = 0; f < Filters.filterColumns[Mode].Count; f++)
                                {
                                    int index = Filters.filterColumns[Mode][f];
                                    if (Filters.filters[Mode].ContainsKey(f))
                                    {
                                        if (Filters.filters[Mode][f] != "" && Filters.filters[Mode][f] != "<Нет>")
                                        {
                                            if (row.Cells[index].Value.ToString() != Filters.filters[Mode][f])
                                            {
                                                row.Visible = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (row.Visible)
                                {
                                    for (int f = 0; f < Filters.filterColumns[Mode].Count; f++)
                                    {
                                        int index = Filters.filterColumns[Mode][f];
                                        if (!Filters.filterItems[Mode].ContainsKey(f))
                                        {
                                            Filters.filterItems[Mode].Add(f, new List<string>());
                                        }

                                        if (!Filters.filterItems[Mode][f].Contains(row.Cells[index].Value.ToString()))
                                        {
                                            Filters.filterItems[Mode][f].Add(row.Cells[index].Value.ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch {}
                    }
                }
                catch
                {

                }

            }
            DG_SizeChanged(dataGrid, EventArgs.Empty);
            GlobalMethodes._stop = true;
        }

        private string GetAsignie(LibraryDB.DB.Task task, TaskDepartment taskDepartment, Status status)
        {
            string user = "--";
            Department department = null;
            User header = null;

            switch (status.StatusId)
            {
                case 6:
                    try
                    {
                        department = RequestInfo.lb.Departments.FirstOrDefault(x => x.DepartmentId == task.DepartmentId);
                        header = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId == department.DepartmentHeade.Value);

                    }
                    catch { }
                    break;
                case 7:
                case 20:
                    try
                    {
                        var position = RequestInfo.lb.Positions.FirstOrDefault(x => x.PositionId == task.PositionId);
                        header = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId == position.PositionUserIdGIP);
                    }
                    catch { }
                    break;
                case 9:
                    try
                    {
                        department = RequestInfo.lb.Departments.FirstOrDefault(x => x.DepartmentId == taskDepartment.DepartmentId);
                        header = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId == department.DepartmentHeade.Value);
                    }
                    catch { }
                    break;
                case 13:
                    try
                    {
                        header = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId ==task.TaskUserId);
                    }
                    catch { }
                    break;

            }
            if (header != null)
            {
                user = header.UserSurname + " " + header.UserName;
            }
            return user;
        }

        private bool GetNWDfileInfo(string code, string stage, out string date, out string nwdpath)
        {
            nwdpath = "";
            date = "";
            try
            {
                List<TreeNode> existingNodes = SearchTreeNodes(code, GlobalData.NaviTreeView.View.Nodes[0], 0);
                TreeNode node = existingNodes.FirstOrDefault();
                nwdpath += MainForm.MainPath_rezerv + node.FullPath + @"\";
                nwdpath += $"{node.Text}_CRD\\07_NWF\\{node.Text}_UST_{stage}_GF_22.nwd";

                if (File.Exists(nwdpath))
                {
                    date = File.GetLastWriteTime(nwdpath).ToString();
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch
            {
            }
            return false;
        }


        //public void ApplyFilters()
        //{
        //    try
        //    {
        //        for (int f = 0; f < Filters.filterColumns[Mode].Count; f++)
        //        {
        //            Filters.filterItems[Mode][f].Clear();
        //        }
        //    }
        //    catch { }

        //    for (int r = 0; r < dataGrid.Rows.Count; r++)
        //    {
        //        for (int f = 0; f < Filters.filterColumns[Mode].Count; f++)
        //        {
        //            int index = Filters.filterColumns[Mode][f];
        //            if (Filters.filters[Mode].ContainsKey(f))
        //            {
        //                if (Filters.filters[Mode][f] != "" && Filters.filters[Mode][f] != "<Нет>")
        //                {
        //                    if (dataGrid.Rows[r].Cells[index].Value.ToString() != Filters.filters[Mode][f])
        //                    {
        //                        dataGrid.Rows[r].Visible = false;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        if (dataGrid.Rows[r].Visible)
        //        {
        //            for (int f = 0; f < Filters.filterColumns[Mode].Count; f++)
        //            {
        //                int index = Filters.filterColumns[Mode][f];
        //                if (!Filters.filterItems[Mode].ContainsKey(f))
        //                {
        //                    Filters.filterItems[Mode].Add(f, new List<string>());
        //                }

        //                if (!Filters.filterItems[Mode][f].Contains(dataGrid.Rows[r].Cells[index].Value.ToString()))
        //                {
        //                    Filters.filterItems[Mode][f].Add(dataGrid.Rows[r].Cells[index].Value.ToString());
        //                }
        //            }
        //        } 
        //    }
        //}

        private void GetInventorInfo(string path, out string name, out string nwdpath, out string type)
        {
            name = "";
            nwdpath = "z:\\";
            type = "-";

            string[] spPath = path.Split('\\');
            if (spPath != null && spPath.Length > 0)
            {
                bool start = false;
                for(int i = 0; i < spPath.Length; i++)
                {
                    if (!start)
                    {
                        start = spPath[i] == "BIM02";
                    }
                    if (start)
                    {
                        if (i < spPath.Length -1)
                        {
                            nwdpath += spPath[i] + "\\"; 
                        }
                        else
                        {
                            name = GetName(spPath[i], out type);
                            if (type != "Чертеж")
                            {
                                nwdpath += name + ".nwd";
                            }
                            else
                            {
                                nwdpath += name + ".dwg";
                            }
                        }
                    }
                }
            }
            
        }

        private string GetName(string name, out string type)
        {
            string result = "";
            type = "-";
            string[] spName = name.Split('.');
            if (spName != null && spName.Length > 1)
            {
                if (spName.Length < 3)
                {
                    result = spName[0];
                }
                else
                {
                    for(int i = 0; i < spName.Length - 1; i ++)
                    {
                        result += spName[i];
                        if (i < spName.Length - 2)
                        {
                            result += ".";
                        }
                    }
                }
                switch(spName.LastOrDefault())
                {
                    case "iam":
                        type = "Сборка";
                        break;
                    case "ipt":
                        type = "Деталь";
                        break;
                    case "dwg":
                        type = "Чертеж";
                        break;
                }
            }

            return result;
        }

        private void UpdateDataGridColumns()
        {
            List<string> header = new List<string>();
            dataGrid.ColumnHeadersHeight = 40;
            if (Mode == 0)
            {
                header = header1;
            }
            else if (Mode < 3)
            {
                header = header4;   
            }
            else if (Mode == 3)
            {
                header = header2;
            }
            else
            {
                header = header3;
            }
            for (int c = 0; c < dataGrid.Columns.Count; c++)
            {
                dataGrid.Columns[c].HeaderText = header[c];
                if (GlobalData.UserRole != "Admin")
                {
                    if (c < 10)
                    {
                        dataGrid.Columns[c].Visible = true;
                    }
                    else
                    {
                        dataGrid.Columns[c].Visible = false;
                    } 
                }
                else
                {
                    if (Mode == 0)
                    {
                        if (c < 11)
                        {
                            dataGrid.Columns[c].Visible = true;
                        }
                        else
                        {
                            dataGrid.Columns[c].Visible = false;
                        }
                    }
                    else if (Mode < 3)
                    {
                        if (c < 10)
                        {
                            dataGrid.Columns[c].Visible = true;
                        }
                        else
                        {
                            dataGrid.Columns[c].Visible = false;
                        } 
                    }
                    else if (Mode == 3)
                    {
                        if (c < 12)
                        {
                            dataGrid.Columns[c].Visible = true;
                        }
                        else
                        {
                            dataGrid.Columns[c].Visible = false;
                        }
                    }
                    else
                    {
                        int num = 6;
                        if (GlobalData.UserRole == "Admin") num += 1;
                            if (c > num)
                        {
                            dataGrid.Columns[c].Visible = false;
                        }
                    }
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

        public void DG_SizeChanged(object sender, EventArgs e)
        {
            int cWidth = 0;
            if (Mode == 0)
            {
                try
                {
                    if (dataGrid.Columns[0].Visible)
                    {
                        dataGrid.Columns[0].Width = 100;
                        cWidth += dataGrid.Columns[0].Width;
                    }
                    if (dataGrid.Columns[1].Visible)
                    {
                        dataGrid.Columns[1].Width = 100;
                        cWidth += dataGrid.Columns[1].Width;
                    }
                    if (dataGrid.Columns[3].Visible)
                    {
                        dataGrid.Columns[3].Width = 50;
                        cWidth += dataGrid.Columns[3].Width;
                    }
                    if (dataGrid.Columns[4].Visible)
                    {
                        dataGrid.Columns[4].Width = 220;
                        cWidth += dataGrid.Columns[4].Width;
                    }
                    if (dataGrid.Columns[5].Visible)
                    {
                        dataGrid.Columns[5].Width = 220;
                        cWidth += dataGrid.Columns[5].Width;
                    }
                    if (dataGrid.Columns[6].Visible)
                    {
                        dataGrid.Columns[6].Width = 100;
                        cWidth += dataGrid.Columns[6].Width;
                    }
                    if (dataGrid.Columns[7].Visible)
                    {
                        dataGrid.Columns[7].Width = 100;
                        cWidth += dataGrid.Columns[7].Width;
                    }
                    if (dataGrid.Columns[8].Visible)
                    {
                        dataGrid.Columns[8].Width = 50;
                        cWidth += dataGrid.Columns[8].Width;
                    }
                    if (dataGrid.Columns[9].Visible)
                    {
                        dataGrid.Columns[9].Width = 100;
                        cWidth += dataGrid.Columns[9].Width;
                    };
                    if (GlobalData.UserRole == "Admin")
                    {
                        if (dataGrid.Columns[10].Visible)
                        {
                            dataGrid.Columns[10].Width = 80;
                            cWidth += dataGrid.Columns[10].Width;
                        };
                    }
                    dataGrid.Columns[2].Width = dataGrid.Width - (cWidth + 0);

                }
                catch { }
            }
            else if (Mode < 3)
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
                    if (GlobalData.UserRole == "Admin")
                    {
                        if (dataGrid.Columns[9].Visible)
                        {
                            dataGrid.Columns[9].Width = 80;
                            cWidth += dataGrid.Columns[9].Width;
                        };
                    }
                    dataGrid.Columns[1].Width = dataGrid.Width - (cWidth + 0);

                }
                catch { } 
            }
            else if (Mode == 3)
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
                        dataGrid.Columns[1].Width = 120;
                        cWidth += dataGrid.Columns[1].Width;
                    }
                    if (dataGrid.Columns[2].Visible)
                    {
                        dataGrid.Columns[2].Width = 80;
                        cWidth += dataGrid.Columns[2].Width;
                    }
                    if (dataGrid.Columns[3].Visible)
                    {
                        dataGrid.Columns[3].Width = 80;
                        cWidth += dataGrid.Columns[3].Width;
                    }
                    if (dataGrid.Columns[5].Visible)
                    {
                        dataGrid.Columns[5].Width = 80;
                        cWidth += dataGrid.Columns[5].Width;
                    }
                    if (dataGrid.Columns[6].Visible)
                    {
                        dataGrid.Columns[6].Width = 80;
                        cWidth += dataGrid.Columns[6].Width;
                    }
                    if (dataGrid.Columns[7].Visible)
                    {
                        dataGrid.Columns[7].Width = 170;
                        cWidth += dataGrid.Columns[7].Width;
                    }
                    if (dataGrid.Columns[8].Visible)
                    {
                        dataGrid.Columns[8].Width = 170;
                        cWidth += dataGrid.Columns[8].Width;
                    }
                    if (dataGrid.Columns[9].Visible)
                    {
                        dataGrid.Columns[9].Width = 120;
                        cWidth += dataGrid.Columns[9].Width;
                    };
                    if (GlobalData.UserRole == "Admin")
                    {
                        if (dataGrid.Columns[10].Visible)
                        {
                            dataGrid.Columns[10].Width = 80;
                            cWidth += dataGrid.Columns[10].Width;
                        };
                        if (dataGrid.Columns[11].Visible)
                        {
                            dataGrid.Columns[11].Width = 80;
                            cWidth += dataGrid.Columns[11].Width;
                        };
                    }
                    dataGrid.Columns[4].Width = dataGrid.Width - (cWidth + 0);

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
                    
                    if (dataGrid.Columns[2].Visible)
                    {
                        dataGrid.Columns[2].Width = 80;
                        cWidth += dataGrid.Columns[2].Width;
                    }
                    if (dataGrid.Columns[3].Visible)
                    {
                        dataGrid.Columns[3].Width = 200;
                        cWidth += dataGrid.Columns[3].Width;
                    }
                    if (dataGrid.Columns[5].Visible)
                    {
                        dataGrid.Columns[5].Width = 120;
                        cWidth += dataGrid.Columns[5].Width;
                    }
                    if (dataGrid.Columns[6].Visible)
                    {
                        dataGrid.Columns[6].Width = 80;
                        cWidth += dataGrid.Columns[6].Width;
                    }
                    if (GlobalData.UserRole == "Admin")
                    {
                        if (dataGrid.Columns[7].Visible)
                        {
                            dataGrid.Columns[7].Width = 80;
                            cWidth += dataGrid.Columns[7].Width;
                        };
                    }
                    dataGrid.Columns[1].Width = (dataGrid.Width - cWidth) / 3;
                    dataGrid.Columns[4].Width = ((dataGrid.Width - cWidth) / 3) * 2;
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
                    
                }

                try
                {
                    UpdatePanels(GlobalData.FilterList[Mode]);
                    uC_Search_Projects.CreatePanels(Mode);
                }
                catch
                {
                    UpdatePanels();
                    uC_Search_Projects.CreatePanels(Mode);
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
                else if (radioButton5.Checked)
                {
                    stripItemOpen.Text = "Открыть в документ";
                    stripItemOpen.Image = Properties.Resources.Btn_OpenCard_20x20;

                    stripItemOpenDir.Visible = false;
                    stripItemGetPath.Visible = false;
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
                code = dataGrid.SelectedRows[0].Cells[1].Value.ToString();
                stage = dataGrid.SelectedRows[0].Cells[3].Value.ToString();
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
                code = dataGrid.SelectedRows[0].Cells[1].Value.ToString();
                stage = dataGrid.SelectedRows[0].Cells[2].Value.ToString();
            }
            else if (radioButton5.Checked)
            {
                if (sender.ToString() == stripItemOpen.Text && dataGrid.SelectedRows.Count > 0)
                {
                    string path = dataGrid.SelectedRows[0].Cells[8].Value.ToString();
                    GlobalMethodes.CopyAndOpenFile(path);
                }
            }


            if (!radioButton5.Checked)
            {
                try
                {
                    List<TreeNode> existingNodes = SearchTreeNodes(code, GlobalData.NaviTreeView.View.Nodes[index], 0);

                    if (existingNodes.Count > 0)
                    {
                        foreach (TreeNode node in existingNodes)
                        {
                            if (index == 0)
                            {
                                string[] sPath = node.FullPath.Split('\\');
                                string stageCode = "";
                                if (node.Tag.ToString() == "SubPosition")
                                {
                                    stageCode = sPath[sPath.Length - 3];
                                }
                                else
                                {
                                    stageCode = sPath[sPath.Length - 2];
                                }
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
                                                int tdId = Convert.ToInt32(dataGrid.SelectedRows[0].Cells[11].Value);
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
                                                    general.comboBox3.Text = dataGrid.SelectedRows[0].Cells[5].Value.ToString();

                                                    general.textBox1.TextChanged -= new System.EventHandler(general.textBox1_TextChanged);
                                                    general.textBox3.TextChanged -= new System.EventHandler(general.textBox3_TextChanged);
                                                    general.textBox4.TextChanged -= new System.EventHandler(general.textBox3_TextChanged);
                                                    general.comboBox1.SelectedValueChanged -= new System.EventHandler(general.comboBox1_SelectedValueChanged);
                                                    general.comboBox1.TextChanged -= new System.EventHandler(general.comboBox1_TextChanged);
                                                    general.comboBox2.SelectedValueChanged -= new System.EventHandler(general.comboBox2_SelectedValueChanged);
                                                    general.comboBox3.SelectedValueChanged -= new System.EventHandler(general.comboBox3_SelectedValueChanged);
                                                    general.comboBox3.TextChanged -= new System.EventHandler(general.comboBox3_TextChanged);

                                                    if (general.ShowDialog() == DialogResult.Retry)
                                                    {
                                                        toolStripItem_Click(stripItemGetPath, EventArgs.Empty);
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
                                            general.textBox1.Text = dataGrid.SelectedRows[0].Cells[1].Value.ToString();
                                            general.textBox2.Text = dataGrid.SelectedRows[0].Cells[2].Value.ToString();
                                            general.textBox3.Text = dataGrid.SelectedRows[0].Cells[3].Value.ToString();
                                            general.textBox4.Text = dataGrid.SelectedRows[0].Cells[5].Value.ToString();

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
                catch
                {
                } 
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
            //UpdateFilterPanel(openFiltersPanel, Mode);
        }

        public void usT_HorizontalTabControl_Click(object sender, EventArgs e)
        {
            int index = (sender as UST_HorizontalTabControl).TabIndex;
            foreach (Control control in panel2.Controls)
            {
                if (control.TabIndex == index)
                {
                    (control as RadioButton).Checked = true;
                    break;
                }
            }
            foreach(Control btn in panel5.Controls)
            {
                UST_HorizontalTabControl tabControl = btn as UST_HorizontalTabControl;
                if (tabControl.TabIndex == index)
                {
                    tabControl.PressedStatus = true;
                }
                else
                {
                    tabControl.PressedStatus = false;
                }
                tabControl.Invalidate();
            }
        }
    }
}
