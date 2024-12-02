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
using Task = LibraryDB.DB.Task;
using System.Threading;

namespace UST_ProjectManagement
{
    public partial class UC_TaskPanel : UserControl
    {
        //Panel_TasksGridPanel tasksGridPanel;
        //int gridStep = 60;
        int gridMinHeight = 30;
        int gridMaxHeight = 100;
        int gridMinWidth = 45;
        int gridMaxWidth = 100;
        int headerSize = 45;
        Color headerColor = Color.AliceBlue;
        Dictionary<string, string> rSections;
        Dictionary<string, int> rSectionsId;
        Dictionary<string, string> cSections;
        Dictionary<string, int> cSectionsId;
        PositionInfo positionInfo;

        Dictionary<string, string> incomingSections;
        Dictionary<string, int> incomingSectionIds;
        List<Task> incomingTasks;

        Dictionary<string, string> outcomingSections;
        Dictionary<string, int> outcomingSectionIds;
        List<Task> outcomingTasks;


        public delegate void OpenTask_Click(bool open, List<int> ids, List<int> tdids, string from, string to, int row);
        public event OpenTask_Click TaskOpen;

        public delegate void AddTask_Click(byte mode);
        public event AddTask_Click EditTask;

        public delegate void TaskRow_Click(int id, string from, string to);
        public event TaskRow_Click TaskRowClick;


        int rNumber = 14;
        int cNumber = 14;
        int num = 5;

        DataGridView dataGridView;
        DataGridView dataGridView1;


        ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
        ToolStripMenuItem toolStripMenuItem_AddTask = new ToolStripMenuItem();
        ToolStripMenuItem toolStripMenuItem_OpenTask = new ToolStripMenuItem();
        ToolStripMenuItem toolStripMenuItem_PromoteTask = new ToolStripMenuItem();
        ToolStripMenuItem toolStripMenuItem_AddOutTask = new ToolStripMenuItem();

        public UC_TaskPanel()
        {
            InitializeComponent();
            panel1.BackColor = MainForm.HeaderColor;
            usT_HorizontalTabControl2.Height = 25;
            usT_HorizontalTabControl2.Width = 200;
            usT_HorizontalTabControl2.PressedStatus = true;
            usT_HorizontalTabControl2.Invalidate();


            dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersVisible = false;
            dataGridView.ColumnHeadersVisible = false;
            dataGridView.MultiSelect = false;
            dataGridView.BackgroundColor = Color.WhiteSmoke;
            dataGridView.Margin = new Padding(0);
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.AllowUserToResizeColumns = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToOrderColumns = false;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.RowsDefaultCellStyle.SelectionBackColor = MainForm.HeaderColor;
            dataGridView.CellClick += new DataGridViewCellEventHandler(DG_Cell_Click);
            dataGridView.MouseDown += new MouseEventHandler(dataGridView_MouseDown);
            dataGridView.CellMouseEnter += new DataGridViewCellEventHandler(DG_Cell_MouseEnter);
            dataGridView.CellMouseLeave += new DataGridViewCellEventHandler(DG_Cell_MouseLeave);
            dataGridView.SizeChanged += new EventHandler(DG_SizeChanged);

            dataGridView1 = new DataGridView();
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersHeight = 50;
            dataGridView1.MultiSelect = false;
            dataGridView1.BackgroundColor = Color.WhiteSmoke;
            dataGridView1.Margin = new Padding(5);
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.BorderStyle = BorderStyle.FixedSingle;
            dataGridView1.RowsDefaultCellStyle.SelectionBackColor = MainForm.HeaderColor;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.SizeChanged += new EventHandler(DG1_SizeChanged);
            dataGridView1.CellClick += new DataGridViewCellEventHandler(DG1_Cell_Click);
            dataGridView1.MouseDown += new MouseEventHandler(dataGridView1_MouseDown);
            //dataGridView1.CellMouseEnter += new DataGridViewCellEventHandler(DG_Cell_MouseEnter);
            //dataGridView1.CellMouseLeave += new DataGridViewCellEventHandler(DG_Cell_MouseLeave);



            List<string> header1 = new List<string>() { "№ задания", "Наименование", "Тип задания", "От", "Для", "Статус", "tId", "tdId" };
            CardTask.Methodes_DataGrid.CreateDataGrid(dataGridView1, header1);
            dataGridView1.Columns[dataGridView1.Columns.Count - 2].Visible = false;
            dataGridView1.Columns[dataGridView1.Columns.Count - 1].Visible = false;
            panel8.Controls.Add(dataGridView1);

            toolStripMenuItem_AddTask.Text = "Создать задание";
            toolStripMenuItem_AddTask.Image = Properties.Resources.Btn_Add_20x20;
            toolStripMenuItem_AddTask.Click += new EventHandler(toolStripMenuItem_Click);
            contextMenuStrip.Items.Add(toolStripMenuItem_AddTask);

            toolStripMenuItem_OpenTask.Text = "Открыть карточку";
            toolStripMenuItem_OpenTask.Image = Properties.Resources.Btn_OpenCard_20x20;
            toolStripMenuItem_OpenTask.Click += new EventHandler(toolStripMenuItem_Click);
            contextMenuStrip.Items.Add(toolStripMenuItem_OpenTask);

            toolStripMenuItem_PromoteTask.Text = "Изменить статус";
            toolStripMenuItem_PromoteTask.Image = Properties.Resources.Btn_Promote_20x20;
            toolStripMenuItem_PromoteTask.Click += new EventHandler(toolStripMenuItem_Click);
            contextMenuStrip.Items.Add(toolStripMenuItem_PromoteTask);

            toolStripMenuItem_AddOutTask.Text = "Создать исходящую задачу";
            toolStripMenuItem_AddOutTask.Image = Properties.Resources.Btn_Add_20x20;
            toolStripMenuItem_AddOutTask.Click += new EventHandler(toolStripMenuItem_Click);
            contextMenuStrip.Items.Add(toolStripMenuItem_AddOutTask);


        }



        private void UC_TaskPanel_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(button1, "Обновить матрицу заданий");
        }

        public void UpdateControls()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange((new List<string> { "Показать все", "Показать задания", "Задания от моей дисциплины", "Задания для моей дисциплины" }).ToArray());
            comboBox1.SelectedIndex = 0;

            comboBox2.Items.Clear();
            comboBox2.Items.AddRange((new List<string> { "Матрица", "Таблица" }).ToArray());
            comboBox2.SelectedIndex = 0;

            UpdateDG();
        }

        public void UpdateDG(string from = null, string to = null, int taskrow = -1)
        {
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            var Sec = RequestInfo.lb.Sections;
            positionInfo = new PositionInfo(GlobalData.SelectedProject, GlobalData.SelectedStage, GlobalData.SelectedPosition);

            rSections = new Dictionary<string, string>();
            rSectionsId = new Dictionary<string, int>();

            cSections = new Dictionary<string, string>();
            cSectionsId = new Dictionary<string, int>();

            List<string> userSets = new List<string>();

            foreach (ScheduleItem item in positionInfo.scheduleItems)
            {
                try
                {
                    rSections.Add(item.SecThreeTag + item.SecThreePostfix, item.SecThreePostfix);
                    cSections.Add(item.SecThreeTag + item.SecThreePostfix, item.SecThreePostfix);

                    rSectionsId.Add(item.SecThreeTag + item.SecThreePostfix, item.SecThreeId);
                    cSectionsId.Add(item.SecThreeTag + item.SecThreePostfix, item.SecThreeId);
                }
                catch
                {

                }
            }


            Dictionary<string, int> incomingColumns = new Dictionary<string, int>();

            GetIncomingSectionsInfo(out incomingSections, out incomingSectionIds, out incomingTasks, out incomingColumns);

            foreach (var pair in incomingColumns)
            {
                if (!cSections.ContainsKey(pair.Key))
                {
                    cSections.Add(pair.Key, null);
                }
                if (!cSectionsId.ContainsKey(pair.Key))
                {
                    cSectionsId.Add(pair.Key, pair.Value);
                }
                if (!rSections.ContainsKey(pair.Key))
                {
                    rSections.Add(pair.Key, null);
                }
                if (!rSectionsId.ContainsKey(pair.Key))
                {
                    rSectionsId.Add(pair.Key, pair.Value);
                }
            }

            foreach (var pair in incomingSections)
            {
                try
                {
                    rSections.Add(pair.Key, pair.Value);
                }
                catch { }
            }
            foreach (var pair in incomingSectionIds)
            {
                try
                {
                    rSectionsId.Add(pair.Key, pair.Value);
                }
                catch { }
            }


            Dictionary<string, int> outcomingRows = new Dictionary<string, int>();
            GetOutcomingSectionsInfo(out outcomingSections, out outcomingSectionIds, out outcomingTasks, out outcomingRows);

            foreach (var pair in outcomingRows)
            {
                if (!cSections.ContainsKey(pair.Key))
                {
                    cSections.Add(pair.Key, null);
                }
                if (!cSectionsId.ContainsKey(pair.Key))
                {
                    cSectionsId.Add(pair.Key, pair.Value);
                }
                if (!rSections.ContainsKey(pair.Key))
                {
                    rSections.Add(pair.Key, null);
                }
                if (!rSectionsId.ContainsKey(pair.Key))
                {
                    rSectionsId.Add(pair.Key, pair.Value);
                }
            }

            foreach (var pair in outcomingSections)
            {
                try
                {
                    cSections.Add(pair.Key, pair.Value);
                }
                catch { }
            }
            foreach (var pair in outcomingSectionIds)
            {
                try
                {
                    cSectionsId.Add(pair.Key, pair.Value);
                }
                catch { }
            }



            foreach (string set in GlobalData.UserSets)
            {
                List<ScheduleItem> tt = positionInfo.scheduleItems.Where(x => x.SecThreeNum == set).ToList();
                if (tt != null) userSets.AddRange(tt.Select(x => x.SecThreeTag + x.SecThreePostfix).ToArray());
            }

            int gridWidth = gridMinWidth;
            int gridHeight = gridMinHeight;
            int columnsCount = cSections.Count;
            int rowsCount = rSections.Count;

            //int secNumber = Sections.Count;
            //if (columnsCount > cNumber) cNumber = columnsCount;
            //if (rowsCount > rNumber) rNumber = rowsCount;
            cNumber = columnsCount + 1;
            rNumber = rowsCount + 2;

            //GetGridStep(Number, out gridWidth, out gridHeight); 


            for (int t = 0; t <= cNumber; t++)
            {
                dataGridView.Columns.Add($"Column{t}", "");
                if (t == 0) dataGridView.Columns[t].Width = headerSize;
                else dataGridView.Columns[t].Width = gridWidth;
            }

            #region --- Create header ---
            DataGridViewRow headerrow = new DataGridViewRow();
            headerrow.CreateCells(dataGridView);
            headerrow.Cells[0].Style.BackColor = Color.AliceBlue;
            headerrow.Height = headerSize;
            int c = 1;
            foreach (var pair in cSections)
            {
                headerrow.Cells[c].Value = pair.Key;
                int id = -1;
                cSectionsId.TryGetValue(pair.Key, out id);
                headerrow.Cells[c].Tag = id;

                headerrow.Cells[c].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (!outcomingSections.ContainsKey(pair.Key))
                {
                    headerrow.Cells[c].Style.BackColor = Color.AliceBlue;
                }
                else
                {
                    headerrow.Cells[c].Style.BackColor = Color.LightSalmon;
                }
                c++;
            }
            for (int cc = c; cc <= cNumber; cc++)
            {
                headerrow.Cells[cc].Style.BackColor = Color.AliceBlue;
            }
            dataGridView.Rows.Add(headerrow);
            #endregion

            #region --- CreateRows ---
            int r = 1;

            foreach (var _row in rSectionsId)
            {
                c = 1;
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView);
                row.Height = gridHeight;
                row.Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                ToolTip tt = new ToolTip();
                if (!incomingSectionIds.ContainsKey(_row.Key))
                {
                    row.Cells[0].Style.BackColor = Color.AliceBlue;
                }
                else
                {
                    row.Cells[0].Style.BackColor = Color.LimeGreen;
                }
                row.Cells[0].Value = _row.Key;
                row.Cells[0].Tag = _row.Value;

                foreach (var _cell in cSectionsId)
                {
                    if (c != r)
                    {
                        int status = -1;
                        List<int> ids = new List<int>();
                        List<int> tdids = new List<int>();
                        int number = 0;
                        int accepted = 0;
                        bool inwork = false;

                        GetCellTaskStatus(_row.Key, _cell.Key, out status, out ids, out tdids, out number, out accepted, out inwork);

                        int num = 0;
                        foreach (int id in ids)
                        {
                            Task task = incomingTasks.FirstOrDefault(x => x.TaskId == id);
                            if (task != null)
                            {
                                num += 1;
                            }
                        }
                        if (num == ids.Count())
                        {
                            //status = -1;
                        }
                        row.Cells[c].Tag = status;
                        row.Cells[c].Style.BackColor = GlobalMethodes.GetCellColor(status, 0);
                        if (status == 10 || status == 13)
                        {
                            row.Cells[c].Style.ForeColor = Color.White;
                        }
                        else
                        {
                            row.Cells[c].Style.ForeColor = Color.Black;
                        }
                        if (tdids.Count > 0)
                        {
                            row.Cells[c].Style.Alignment = DataGridViewContentAlignment.BottomRight;

                            string txt = $"{accepted}/{number}";
                            if (inwork) txt += "*";
                            row.Cells[c].Value = txt;
                        }
                        else
                        {
                            row.Cells[c].Value = null;
                        }
                    }
                    else
                    {
                        row.Cells[r].Style.BackColor = headerColor;
                    }
                    c++;
                }
                for (int cc = c; cc <= cNumber; cc++)
                {
                    row.Cells[cc].Style.BackColor = Color.WhiteSmoke;
                }
                r++;
                dataGridView.Rows.Add(row);
            }
            for (int rr = r; rr < rNumber; rr++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView);
                row.Height = gridHeight;
                row.Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                row.Cells[0].Style.BackColor = Color.AliceBlue;
                for (int cc = 1; cc <= cNumber; cc++)
                {
                    row.Cells[cc].Style.BackColor = Color.WhiteSmoke;
                }
                dataGridView.Rows.Add(row);
            }
            #endregion

            panelGrid.Controls.Add(dataGridView);

            if (from == null || to == null || from == "" || to =="")
            {
                dataGridView.Rows[0].Cells[0].Selected = false;
            }
            else
            {
                int _row = GetRowIndexByKey(from);
                int _col = GetColumnsIndexByKey(to);
                dataGridView.Rows[_row].Cells[_col].Selected = true;
                DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(_col, _row);
                DG_Cell_Click(dataGridView.Rows[_row].Cells[_col], e);
            }
            if (taskrow == -1)
            {
                try
                {
                    TaskOpen(true, null, null, "..", "..", -1);
                }
                catch { }
                
            }
            else
            {
                //TaskOpen(true, null, null, "..", "..", taskrow);
            }
            
        }

        public void UpdateDG1()
        {
            dataGridView1.Rows.Clear();
            List<LibraryDB.DB.TaskDepartment> taskDeps = RequestInfo.lb.TaskDepartments.Where(x => x.PositionId == GlobalData.SelectedPosition.PositionId).ToList();
            List<Task> posTasks = new List<LibraryDB.DB.Task>();

            posTasks.AddRange(RequestInfo.lb.Tasks.Where(x => x.PositionId == GlobalData.SelectedPosition.PositionId));
           

            foreach (var _pTask in posTasks)
            {
                taskDeps.AddRange(RequestInfo.lb.TaskDepartments.Where(x => x.TaskId == _pTask.TaskId));
            }
            foreach (var td in taskDeps)
            {
                Task task = RequestInfo.lb.Tasks.FirstOrDefault(x => x.TaskId == td.TaskId);
                if (task != null)
                {
                    posTasks.Add(task);
                }
            }
            posTasks = posTasks.OrderByDescending(x => x.TaskId).ToList();

            taskDeps.Distinct();
            taskDeps.OrderByDescending(x => x.TaskDepartmentId);

            var gTask = taskDeps.GroupBy(x => x.TaskId).OrderByDescending(x => x.Key);
            foreach (var _task in gTask)
            {
                var Task = RequestInfo.lb.Tasks.FirstOrDefault(x => x.TaskId == _task.Key);
                
                if (Task != null && (comboBox1.SelectedIndex != 2 || (comboBox1.SelectedIndex == 2 && GlobalData.user.DepartmentId == Task.DepartmentId)))
                {
                    LibraryDB.DB.Type type = null;
                    try
                    {
                        type = RequestInfo.lb.Types.FirstOrDefault(x => x.TypeId == Task.TypeId);
                    }
                    catch { }
                    if (type == null) type = RequestInfo.lb.Types.FirstOrDefault(x => x.TypeId == 7);
                    SectionsThree secFrom = RequestInfo.lb.SectionsThrees.FirstOrDefault(x => x.SectionThreeId == Task.SectionThreeId);
                    string secFromNum = "";

                    SectionsThree secTo = null;

                    foreach (var _taskDep in _task)
                    {
                        if (comboBox1.SelectedIndex != 3 || (comboBox1.SelectedIndex == 3 && GlobalData.user.DepartmentId == _taskDep.DepartmentId))
                        {
                            secTo = RequestInfo.lb.SectionsThrees.FirstOrDefault(x => x.SectionThreeId == _taskDep.SectionThreeId);
                            var status = RequestInfo.lb.Status.FirstOrDefault(x => x.StatusId == _taskDep.StatusId);


                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataGridView1);
                            row.Height = 30;
                            row.Cells[0].Value = $"{Task.TaskNumer}-{Task.TaskYear}";
                            row.Cells[1].Value = Task.TaskName;
                            row.Cells[2].Value = type.TypeName;
                            if (secFrom != null)
                            {
                                if (GlobalData.SelectedProject != null && GlobalData.SelectedProject.LanguageId == 2)
                                {
                                    row.Cells[3].Value = secFrom.SectionThreeTagEng;
                                }
                                else
                                {
                                    row.Cells[3].Value = secFrom.SectionThreeTagRus;
                                }
                            }
                            else
                            {
                                row.Cells[3].Value = "-";
                            }
                            if (secTo != null)
                            {
                                if (GlobalData.SelectedProject != null && GlobalData.SelectedProject.LanguageId == 2)
                                {
                                    row.Cells[4].Value = $"{secTo.SectionThreeTagEng}{secTo.SectionPositionNumber}";
                                }
                                else
                                {
                                    row.Cells[4].Value = secTo.SectionThreeTagRus;
                                }
                            }
                            else
                            {
                                row.Cells[4].Value = "-";
                            }
                            if (status != null)
                            {
                                row.Cells[5].Value = status.StatusName;
                                row.Cells[5].Style.BackColor = GlobalMethodes.GetCellColor(status.StatusId, 0);
                            }
                            else
                            {
                                row.Cells[5].Value = "-";
                            }
                            row.Cells[6].Value = Task.TaskId;
                            row.Cells[7].Value = _taskDep.TaskDepartmentId;

                            dataGridView1.Rows.Add(row); 
                        }
                    } 
                }
            }
        }

        private bool DepartmentExistingInProject(int fromId, int toId)
        {
            if (GlobalData.user == null)
            {
                MainForm.UpdateUserAxes(MainForm.GetUserName());
            }
            User user = GlobalData.user;

            if (user != null)
            {
                try
                {
                    var fullAccess = RequestInfo.lb.Departments.Where(x => x.ManagmentId == 65 || x.ManagmentId == 63).Select(y => y.DepartmentId);
                    if (fullAccess.Contains(user.DepartmentId))
                    {
                        return true;
                    }
                }
                catch
                {

                }

                var userdepartment = RequestInfo.lb.Departments.FirstOrDefault(x => x.DepartmentId == user.DepartmentId);
                var usermanagement = RequestInfo.lb.Managments.FirstOrDefault(x => x.ManagmentId == userdepartment.ManagmentId);

                var fromdepartment = RequestInfo.lb.Departments.FirstOrDefault(x => x.DepartmentId == fromId);
                var fromkmanagement = RequestInfo.lb.Managments.FirstOrDefault(x => x.ManagmentId == fromdepartment.ManagmentId);

                var todepartment = RequestInfo.lb.Departments.FirstOrDefault(x => x.DepartmentId == toId);
                var tokmanagement = RequestInfo.lb.Managments.FirstOrDefault(x => x.ManagmentId == todepartment.ManagmentId);

                if (usermanagement.ManagmentId == fromkmanagement.ManagmentId || usermanagement.ManagmentId == tokmanagement.ManagmentId)
                {
                    return true;
                }
                else
                {
                    return false;
                } 
            }
            else
            {
                return false;
            }
        }

        private int GetRowIndexByKey(string key)
        {
            int result = 0;
            for(int i = 1; i < dataGridView.Rows.Count; i++)
            {
                if (dataGridView.Rows[i].Cells[0].Value.ToString() == key)
                {
                    return i;
                }
            }
            return result;
        }

        private int GetColumnsIndexByKey(string key)
        {
            int result = 0;
            for (int i = 1; i < dataGridView.Rows[0].Cells.Count; i++)
            {
                try
                {
                    if (dataGridView.Rows[0].Cells[i].Value.ToString() == key)
                    {
                        return i;
                    }
                }
                catch { }
            }
            return result;
        }

        private void GetIncomingSectionsInfo(out Dictionary<string, string> sections, out Dictionary<string, int> ids, out List<Task> tasks, out Dictionary<string, int> incColumns)
        {
            sections = new Dictionary<string, string>();
            ids = new Dictionary<string, int>();
            tasks = new List<LibraryDB.DB.Task>();
            incColumns = new Dictionary<string, int>();
            var taskDeps = RequestInfo.lb.TaskDepartments.Where(x => x.PositionId == positionInfo.ID).ToList();
            foreach(TaskDepartment td in taskDeps)
            {
                try
                {
                    SectionsThree sectionColumn = RequestInfo.lb.SectionsThrees.FirstOrDefault(x => x.SectionThreeId == td.SectionThreeId);
                    string tagColumn = "";
                    if (positionInfo.LanguageId == 2)
                    {
                        tagColumn = sectionColumn.SectionThreeTagEng;
                    }
                    else
                    {
                        tagColumn = sectionColumn.SectionThreeTagRus;
                    }
                    try
                    {
                        incColumns.Add(tagColumn, sectionColumn.SectionThreeId);
                    }
                    catch
                    {
                    }



                    Task task = RequestInfo.lb.Tasks.Where(t => t.PositionId != positionInfo.ID).FirstOrDefault(x => x.TaskId == td.TaskId);
                    SectionsThree section = RequestInfo.lb.SectionsThrees.FirstOrDefault(x => x.SectionThreeId == task.SectionThreeId);
                    string tag = "";
                    if (positionInfo.LanguageId == 2)
                    {
                        tag = section.SectionThreeTagEng;
                    }
                    else
                    {
                        tag = section.SectionThreeTagRus;
                    }
                    tag += task.SectionPositionNumber;
                    tag += "<-";

                    try
                    {
                        sections.Add(tag, task.SectionPositionNumber);
                        ids.Add(tag, task.SectionThreeId);
                    }
                    catch { }
                    if (!tasks.Select(x => x.TaskId).ToList().Contains(task.TaskId)) tasks.Add(task);
                }
                catch
                {
                }
            }
        }

        private void GetOutcomingSectionsInfo(out Dictionary<string, string> sections, out Dictionary<string, int> ids, out List<Task> tasks, out Dictionary<string, int> outRows)
        {
            sections = new Dictionary<string, string>();
            ids = new Dictionary<string, int>();
            tasks = new List<LibraryDB.DB.Task>();
            outRows = new Dictionary<string, int>();
            var extasks = RequestInfo.lb.Tasks.Where(t => t.PositionId == positionInfo.ID);
            
            foreach (Task tsk in extasks)
            {
                try
                {
                    SectionsThree rowSection = RequestInfo.lb.SectionsThrees.FirstOrDefault(x => x.SectionThreeId == tsk.SectionThreeId);
                    string rowTag = "";
                    if (positionInfo.LanguageId == 2)
                    {
                        rowTag = rowSection.SectionThreeTagEng;
                    }
                    else
                    {
                        rowTag = rowSection.SectionThreeTagRus;
                    }
                    try
                    {
                        outRows.Add(rowTag, rowSection.SectionThreeId);
                    }
                    catch { }


                    var taskDeps = RequestInfo.lb.TaskDepartments.Where(x => x.TaskId == tsk.TaskId).Where(y => y.PositionId != null && y.PositionId != positionInfo.ID).ToList();
                    if (taskDeps.Count > 0)
                    {
                        foreach (TaskDepartment td in taskDeps)
                        {
                            SectionsThree section = RequestInfo.lb.SectionsThrees.FirstOrDefault(x => x.SectionThreeId == td.SectionThreeId);
                            string tag = "";
                            if (positionInfo.LanguageId == 2)
                            {
                                tag = section.SectionThreeTagEng;
                            }
                            else
                            {
                                tag = section.SectionThreeTagRus;
                            }
                            tag += td.SectionPositionNumber;
                            tag += "->";

                            try
                            {
                                sections.Add(tag, td.SectionPositionNumber);
                                ids.Add(tag, td.SectionThreeId);
                            }
                            catch { }
                        }
                        if (!tasks.Select(x => x.TaskId).ToList().Contains(tsk.TaskId)) tasks.Add(tsk); 
                    }
                }
                catch
                {
                }
            }
        }
        

        private void GetGridStep(int num, out int width, out int height)
        {
            //width = 0;
            //height = 0;
            //try
            //{
            //    width = (dataGridView.Width - headerSize) / (num - 1);
            //    height = (dataGridView.Height - headerSize) / (num - 1);
            //}
            //catch { }

            //width = Math.Max(width, gridMinWidth);
            //width = Math.Min(width, gridMaxWidth);

            //height = Math.Max(height, gridMinHeight);
            //height = Math.Min(height, gridMaxHeight);
            width = gridMinWidth;
            height = gridMinHeight;
        }

        private void GetCellTaskStatus(string fromTag, string toTag, out int status, out List<int> cellTasksIds, out List<int> cellTDIds,out int number, out int accepted, out bool inwork)
        {
            status = -1;
            cellTasksIds = new List<int>();
            cellTDIds = new List<int>();
            number = 0;
            accepted = 0;
            inwork = false;

            var Tasks = RequestInfo.lb.Tasks.Where(p => p.PositionId == GlobalData.SelectedPosition.PositionId).ToList();
            try
            {
                Tasks.AddRange(incomingTasks.ToArray());
            }
            catch { }
            List<TaskDepartment> taskDepartments = new List<TaskDepartment>();
            List<TaskDepartment> cellTDs = new List<TaskDepartment>();
            if (Tasks != null && Tasks.Count() > 0)
            {
                var rowTasks = Tasks.Where(x => x.SectionThreeId == rSectionsId[fromTag]).Where(i => i.SectionPositionNumber == rSections[fromTag]);
                if (rowTasks != null && rowTasks.Count() > 0)
                {
                    foreach (Task task in rowTasks)
                    {
                        List<TaskDepartment> td = RequestInfo.lb.TaskDepartments.Where(x => x.TaskId == task.TaskId).ToList();
                        if(!incomingSections.ContainsKey(fromTag))
                        {
                            task.TaskDepartments = td;
                        }
                        else
                        {
                            task.TaskDepartments = td.Where(x => x.PositionId == positionInfo.ID).ToList();
                        }

                        if (td != null) taskDepartments.AddRange(task.TaskDepartments);
                    }

                    cellTDs = null;
                    if (!outcomingSections.ContainsKey(toTag))
                    {
                        if (!incomingSections.ContainsKey(fromTag))
                        {
                            cellTDs = taskDepartments.Where(x => x.SectionThreeId == cSectionsId[toTag]).Where(i => i.SectionPositionNumber == cSections[toTag]).Where(t => t.PositionId == null || t.PositionId == positionInfo.ID).ToList(); 
                        }
                        else
                        {
                            cellTDs = taskDepartments.Where(x => x.SectionThreeId == cSectionsId[toTag]).Where(i => i.SectionPositionNumber == cSections[toTag]).ToList();
                        }
                    }
                    else
                    {
                        cellTDs = taskDepartments.Where(x => x.SectionThreeId == cSectionsId[toTag]).Where(i => i.SectionPositionNumber == cSections[toTag]).Where(t => t.PositionId != null || t.PositionId != positionInfo.ID).ToList();
                    }
                    if (cellTDs != null && cellTDs.Count > 0)
                    {
                        cellTDIds = cellTDs.Select(x => x.TaskDepartmentId).ToList();
                        if (cellTDs.Count == 1)
                        {                           
                            TaskDepartment cellTD = cellTDs.FirstOrDefault(id => id.TaskDepartmentId == cellTDs.Max(x => x.TaskDepartmentId));
                            number = 1;
                            try
                            {
                                if (cellTD != null && DepartmentExistingInProject(rowTasks.FirstOrDefault(x => x.TaskId == cellTD.TaskId).DepartmentId, cellTD.DepartmentId))
                                {
                                    status = cellTD.StatusId;
                                    if (status == 10 || status == 12 || status == 14 || status == 17)
                                    {
                                        accepted = 1;
                                    }
                                    else if (status == 5)
                                    {
                                        inwork = true;
                                    }

                                }
                                else
                                {
                                    status = -1;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, "Errore");
                                status = -1;
                            }
                        }
                        else
                        {
                            List<int> statusIds = new List<int>();
                            statusIds = cellTDs.Select(x => x.StatusId).ToList();

                            List<int> analizeIds = new List<int>();
                            foreach(int id in statusIds)
                            {
                                if (id != 12 && id != 14 && id != 17 && id != 5 && !analizeIds.Contains(id))
                                {
                                    analizeIds.Add(id);
                                }
                                else if (id == 5)
                                {
                                    inwork = true;
                                }
                                if (id == 10 || id == 12 || id == 14 || id == 17)
                                {
                                    accepted += 1;
                                }
                                number += 1;
                            }
                            if (analizeIds.Count == 1)
                            {
                                status = analizeIds.First();
                            }
                            else if (analizeIds.Contains(13))
                            {
                                status = 13;
                            }
                            else
                            {
                                status = 8;
                            }
                        }
                        cellTasksIds = cellTDs.Select(x => x.TaskId).Distinct().ToList();
                    }
                }
            }
        }

        public void DG_SizeChanged(object sender, EventArgs e)
        {
            int gridWidth = 0;
            int gridHeight = 0;
            GetGridStep(dataGridView.Columns.Count, out gridWidth, out gridHeight);
            int cSumm = headerSize;
            for(int i = 1; i < dataGridView.Columns.Count - 1; i++)
            {
                dataGridView.Columns[i].Width = gridWidth;
                cSumm += gridWidth;
            }
            try
            {
                dataGridView.Columns[dataGridView.Columns.Count - 1].Width = dataGridView.Width - cSumm;
            }
            catch
            {
                dataGridView.Columns[dataGridView.Columns.Count - 1].Width = gridWidth;
            }

            int rSumm = headerSize;
            for (int i = 1; i < dataGridView.Rows.Count - 1; i++)
            {
                dataGridView.Rows[i].Height = gridHeight;
                rSumm += gridHeight;
            }
            try
            {
                dataGridView.Rows[dataGridView.Rows.Count - 1].Height = dataGridView.Height - rSumm;
            }
            catch
            {
                dataGridView.Rows[dataGridView.Rows.Count - 1].Height = gridHeight;
            }
        }

        public void DG1_SizeChanged(object sender, EventArgs e)
        {
            int cSumm = 0;
            for (int i = 0; i < dataGridView1.Columns.Count - 2; i++)
            {
                int W = 0;
                
                if (i == 0 || i == 5)
                {
                    W = 120;
                }
                else if (i == 2)
                {
                    W = 150;
                }
                else if (i == 3 || i == 4)
                {
                    W = 60;
                }
                dataGridView1.Columns[i].Width = W;
                cSumm += W;
            }
            try
            {
                dataGridView1.Columns[1].Width = dataGridView1.Width - cSumm;
            }
            catch
            {
                dataGridView1.Columns[1].Width = 300;
            }
        }

        private void DG_Cell_Click(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != 0 && e.ColumnIndex != 0 && e.RowIndex != e.ColumnIndex && e.RowIndex <= rSections.Count && e.ColumnIndex <= cSections.Count)
            {
                int status = -1;
                List<int> ids = new List<int>();
                List<int> tdids = new List<int>();

                string secfrom = dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                //secfrom = RefreshSectionTag(secfrom);

                string secto = dataGridView.Rows[0].Cells[e.ColumnIndex].Value.ToString();
                int number = 0;
                int accepted = 0;
                bool inwork = false;

                try
                {
                    GetCellTaskStatus(secfrom, secto, out status, out ids, out tdids, out number, out accepted, out inwork);
                }
                catch { }

                try
                {
                    TaskOpen?.Invoke(true, ids, tdids, secfrom, secto, 0);
                }
                catch
                {
                    TaskOpen?.Invoke(true, ids, tdids, "..", "..", -1);
                }

            }
            else
            {
                dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = false;
            }
        }

        private void DG1_Cell_Click(object sender, DataGridViewCellEventArgs e)
        {
            TaskRowClick(Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[7].Value), dataGridView1.SelectedRows[0].Cells[3].Value.ToString(), dataGridView1.SelectedRows[0].Cells[4].Value.ToString());
            string txt = $"{dataGridView1.SelectedRows[0].Cells[0].Value} {dataGridView1.SelectedRows[0].Cells[3].Value}->{dataGridView1.SelectedRows[0].Cells[4].Value}";
            var status = RequestInfo.lb.Status.FirstOrDefault(x => x.StatusName == dataGridView1.SelectedRows[0].Cells[5].Value.ToString());
            UpdateTaskRoute(txt, status.StatusId, 0);
        }

        private void DG_Cell_MouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            int status = -1;
            try
            {
                status = Convert.ToInt32(dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag);
            }
            catch
            {
                status = -1;
            }

            if (e.RowIndex != 0 && e.ColumnIndex != 0 && e.RowIndex != e.ColumnIndex && e.RowIndex <= rSections.Count && e.ColumnIndex <= cSections.Count)
            {
                //dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = GlobalMethodes.GetCellColor(status, 0, true);
                //if (dataGridView.SelectedCells.Count == 0) dataGridView.RowsDefaultCellStyle.SelectionBackColor = GetCellColor(status, true);
            }
        }

        private void DG_Cell_MouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            int status = -1;
            try
            {
                status = Convert.ToInt32(dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag);
            }
            catch
            {
                status = -1;
            }

            if (e.RowIndex != 0 && e.ColumnIndex != 0 && e.RowIndex != e.ColumnIndex && e.RowIndex <= rSections.Count && e.ColumnIndex <= cSections.Count)
            {
                dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = GlobalMethodes.GetCellColor(status, 0);
                //if (dataGridView.SelectedCells.Count == 0) dataGridView.RowsDefaultCellStyle.SelectionBackColor = GetCellColor(-1);
            }
        }

        private void usT_HorizontalTabControl2_Click(object sender, EventArgs e)
        {
            if (usT_HorizontalTabControl2.PressedStatus)
            {
                usT_HorizontalTabControl2.PressedStatus = false;
                usT_HorizontalTabControl2.Invalidate();
                tableLayoutPanel3.RowStyles[tableLayoutPanel3.RowCount - 1].Height = 0;
            }
            else
            {
                usT_HorizontalTabControl2.PressedStatus = true;
                usT_HorizontalTabControl2.Invalidate();
                tableLayoutPanel3.RowStyles[tableLayoutPanel3.RowCount - 1].Height = 100;
            }
        }

        public void UpdateTaskRoute(string txt, int statusId, int routetype)
        {
            usT_HorizontalTabControl2.Text = "Маршрут: " + txt;
            usT_HorizontalTabControl2.Invalidate();
            RefreshRoutePanel(statusId, routetype);
        }

        private void RefreshRoutePanel(int statusId, int routetype)
        {

            List<string> roles = new List<string>() { "Исполнитель 1", "Руководитель 1", "ГИП", "Руководитель 2", "Исполнитель 2" };
            if (routetype == 1) roles = new List<string>() { "Исполн.1", "Руковод.1", "ГИП", "Менедж.2", "Руковод.2", "Исполн.2" };
            else if (routetype == 2) roles = new List<string>() { "Исполн.1", "Руковод.1", "Менедж.1", "ГИП", "Руковод.2", "Исполн.2" };

            num = roles.Count();
            panel3.Controls.Clear();
            int pOffset = 50;
            int pWidth = 0;
            int pHeight = 40;
            pWidth = (panel3.Width - pOffset * num) / num;

            int X = pOffset/2;
            int Y = (panel3.Height - pHeight)/2;

            for (int i = 0; i < num;  i++)
            {      
                Panel panel = new Panel();
                panel.Location = new Point(X, Y);
                panel.Size = new Size(pWidth, pHeight);
                //panel.BorderStyle = BorderStyle.FixedSingle;
                panel.BackColor = Color.Gainsboro;
                panel.MouseEnter += new EventHandler(RoutePanelMouseEnter);
                panel.MouseLeave += new EventHandler(RoutePanelMouseLeave);

                Label label = new Label();
                label.AutoSize = true;
                label.Text = roles[i];
                label.TextAlign = ContentAlignment.MiddleCenter;       
                panel.Controls.Add(label);
                label.Margin = new Padding(0, 10, 0, 0);
                panel3.Controls.Add(panel);


                if (i < num - 1)
                {
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Location = new Point(X + panel.Width + 5, Y);
                    pictureBox.Size = new Size(pOffset -10, pHeight);
                    pictureBox.Image = Properties.Resources.Arrow_White_30x30;
                    pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                    panel3.Controls.Add(pictureBox); 
                }

                X += pOffset + pWidth;
            }
            RefreshRouteStatus(statusId, routetype);
            if (statusId == -1) RefreshRutePanels();
        }

        private void RoutePanelMouseEnter(object sender, EventArgs e)
        {
            Panel pnl = sender as Panel;
            int offset = 4;
            pnl.Size = new Size(pnl.Width + offset, pnl.Height + offset);
            pnl.Location = new Point(pnl.Location.X - offset / 2, pnl.Location.Y - offset / 2);
        }

        private void RoutePanelMouseLeave(object sender, EventArgs e)
        {
            Panel pnl = sender as Panel;
            int offset = 4;
            pnl.Size = new Size(pnl.Width - offset, pnl.Height - offset);
            pnl.Location = new Point(pnl.Location.X + offset / 2, pnl.Location.Y + offset / 2);
        }

        private void RefreshRouteStatus(int statusId, int routetype)
        {
            Color color = GlobalMethodes.GetCellColor(statusId, routetype);
            int num = -1;
            switch (statusId)
            {
                case -1:
                    num = -1;
                    break;
                case 5:
                    num = 0;
                    break;
                case 6:
                    num = 1;
                    break;
                case 7:             
                case 18:
                    num = 2;
                    break;
                case 20:
                    num = 3;
                    break;
                case 9:
                    if (routetype == 0) num = 3;
                    else num = 4;
                    break;
                case 8:
                    if (routetype == 0) num = 4;
                    else num = 5;
                    break;
                case 10:
                    num = -2;
                    break;
                case 12:
                    num = -3;
                    break;
                case 13:
                    num = -4;
                    break;
                case 19:
                    num = 3;
                    break;
            }
            int i = 0;
            foreach (Control ctrl in panel3.Controls)
            {
                try
                {
                    Panel panel = ctrl as Panel;
                    panel.BorderStyle = BorderStyle.FixedSingle;

                    if (num >= 0)
                    {
                        if (num == 0 && i == 0)
                        {
                            panel.BackColor = Color.Gainsboro;
                        }
                        else
                        {
                            if (i < num)
                            {
                                panel.BackColor = Color.Green;
                            }
                            else if (i == num)
                            {
                                panel.BackColor = Color.Orange;
                            }
                            else
                            {
                                panel.BackColor = Color.WhiteSmoke;
                            }
                        }
                    }
                    else
                    {
                        if (num == -1)
                        {
                            panel.BackColor = Color.WhiteSmoke;
                        }
                        else if (num == -2)
                        {
                            panel.BackColor = Color.Green;
                        }
                        else if (num == -3)
                        {
                            panel.BackColor = Color.Gray;
                        }
                        else if (num == -4)
                        {
                            panel.BackColor = Color.Red;
                        }
                    }

                    i += 1;

                    if (panel.BackColor == Color.Green || panel.BackColor == Color.Red || panel.BackColor == Color.Orange)
                    {
                        panel.Controls[0].ForeColor = Color.White;
                    }
                    else if (panel.BackColor == Color.WhiteSmoke)
                    {
                        panel.Controls[0].ForeColor = Color.DarkGray;
                    }
                    else
                    {
                        panel.Controls[0].ForeColor = Color.Black;
                    }
                    if (panel.BackColor == Color.Green || panel.BackColor == Color.Red || panel.BackColor == Color.Orange)
                    {
                        panel.Controls[0].ForeColor = Color.White;
                    }
                    else if (panel.BackColor == Color.WhiteSmoke)
                    {
                        panel.Controls[0].ForeColor = Color.DarkGray;
                    }
                    else
                    {
                        panel.Controls[0].ForeColor = Color.Black;
                    }
                }
                catch
                {
                    PictureBox pic = ctrl as PictureBox;
                    if (i <= num || num < -1)
                    {
                        pic.Image = Properties.Resources.Arrow_Gray_30x30;
                    }
                    else
                    {
                        pic.Image = Properties.Resources.Arrow_White_30x30;
                    }
                }
            }
        }

        private void RefreshRouteStatusOld(int statusId, int routetype)
        {
            Color color = GlobalMethodes.GetCellColor(statusId, routetype);
            int num = 0;
            switch (statusId)
            {
                case -1:
                    num = -1;
                    break;
                case 5:
                    num = 0;
                    break;
                case 6:
                    num = 1;
                    break;
                case 7:
                case 20:
                    num = 2;
                    break;
                case 8:             
                    num = 4;
                    break;
                case 9:
                case 19:
                    num = 3;
                    break;
                case 10:
                    num = 5;
                    break;
                case 13:
                    num = 6;
                    break;
            }

            int i = 0;
            foreach(Control ctrl in panel3.Controls)
            {
                try
                {
                    Panel panel = ctrl as Panel;
                    panel.BorderStyle = BorderStyle.FixedSingle;
                    if (statusId != 10)
                    {
                        if (i < num && num < 6)
                        {
                            if ((statusId == 8) && i == num - 1)
                            {
                                panel.BackColor = Color.Orange;
                            }
                            else if (num == 1 && i == 0)
                            {
                                panel.BackColor = Color.Gainsboro;
                            }
                            else
                            {
                                panel.BackColor = Color.Green;
                            }
                        }
                        else if (i == num)
                        {
                            if (i < 3)
                            {
                                if (i == 2 && statusId == 20 || statusId == 7)
                                {
                                    panel.BackColor = Color.Orange;
                                }
                                else
                                {
                                    panel.BackColor = Color.Gainsboro;
                                }
                            }
                            else panel.BackColor = Color.Orange;
                        }
                        else if (i == 0 && num == 6)
                        {
                            panel.BackColor = Color.Red;
                        }
                        else
                        {
                            panel.BackColor = Color.WhiteSmoke;
                        }

                        if (panel.BackColor == Color.Green || panel.BackColor == Color.Red || panel.BackColor == Color.Orange)
                        {
                            panel.Controls[0].ForeColor = Color.White;
                        }
                        else if (panel.BackColor == Color.WhiteSmoke)
                        {
                            panel.Controls[0].ForeColor = Color.DarkGray;
                        }
                        else
                        {
                            panel.Controls[0].ForeColor = Color.Black;
                        }
                        i += 1; 
                    }
                    else
                    {
                        panel.BackColor = Color.Green;
                    }
                }
                catch
                {
                    PictureBox pic = ctrl as PictureBox;
                    if (i <= num)
                    {
                        pic.Image = Properties.Resources.Arrow_Gray_30x30;
                    }
                    else
                    {
                        pic.Image = Properties.Resources.Arrow_White_30x30;
                    }
                }
            }
        }
        private void RefreshRutePanels()
        {
            int pOffset = 50;
            int pWidth = 0;
            int pHeight = 40;
            pWidth = (panel3.Width - pOffset * num) / num;

            int X = pOffset / 2;
            int Y = (panel3.Height - pHeight) / 2;

            foreach (Control ctrl in panel3.Controls)
            {
                try
                {
                    Panel panel = ctrl as Panel;
                    panel.Location = new Point(X, Y);
                    panel.Size = new Size(pWidth, pHeight);
                    X += pWidth;
                }
                catch
                {
                    ctrl.Location = new Point(X + 5, Y);
                    X += pOffset;
                }
            }
        }

        private void panel3_SizeChanged(object sender, EventArgs e)
        {
            RefreshRutePanels();
        }

        private void dataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                var ht = dataGridView.HitTest(e.X, e.Y);

                if (ht.RowIndex != 0 && ht.ColumnIndex != 0 && ht.RowIndex != ht.ColumnIndex)
                {
                    contextMenuStrip.Items[0].Visible = true;
                    contextMenuStrip.Items[1].Visible = true;
                    contextMenuStrip.Items[2].Visible = true;
                    contextMenuStrip.Items[3].Visible = true;

                    int status = -1;
                    List<int> ids = new List<int>();
                    List<int> tdids = new List<int>();
                    int number = 0;
                    int accepted = 0;
                    bool inwork = false;
                    try
                    {
                        string secfrom = dataGridView.Rows[ht.RowIndex].Cells[0].Value.ToString();
                        string secto = dataGridView.Rows[0].Cells[ht.ColumnIndex].Value.ToString();
                        GetCellTaskStatus(secfrom, secto, out status, out ids, out tdids,out number, out accepted, out inwork);
                    }
                    catch { }

                    try
                    {
                        TaskOpen?.Invoke(true, ids, tdids, dataGridView.Rows[ht.RowIndex].Cells[0].Value.ToString(), dataGridView.Rows[0].Cells[ht.ColumnIndex].Value.ToString(), 0);
                    }
                    catch
                    {
                        TaskOpen?.Invoke(true, ids, tdids, "..", "..", -1);
                    }

                    if (status == -1)
                    {
                        contextMenuStrip.Items[1].Enabled = false;
                        contextMenuStrip.Items[2].Enabled = false;
                    }
                    else
                    {
                        contextMenuStrip.Items[1].Enabled = true;
                        contextMenuStrip.Items[2].Enabled = true;
                    }
                    //contextMenuStrip.Items[3].Visible = false;
                    contextMenuStrip.Show(MousePosition);
                }
                else
                {
                    contextMenuStrip.Items[0].Enabled = false;
                    contextMenuStrip.Items[1].Enabled = false;
                    contextMenuStrip.Items[2].Enabled = false;
                    contextMenuStrip.Items[3].Visible = true;
                    contextMenuStrip.Show(MousePosition);
                }
            }
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip.Items[3].Visible = false;
                var ht = dataGridView1.HitTest(e.X, e.Y);

                if (ht.RowIndex != dataGridView1.SelectedRows[0].Index)
                {
                    contextMenuStrip.Items[1].Enabled = false;
                    contextMenuStrip.Items[2].Enabled = false;
                }
                else
                {
                    contextMenuStrip.Items[1].Enabled = true;
                    contextMenuStrip.Items[2].Enabled = true;
                }

                contextMenuStrip.Show(MousePosition);
            }
        }

        private void toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(sender.ToString() == toolStripMenuItem_AddTask.Text)
            {
                EditTask?.Invoke(0);
            }
            else if (sender.ToString() == toolStripMenuItem_OpenTask.Text)
            {
                EditTask?.Invoke(1);
            }
            else if (sender.ToString() == toolStripMenuItem_PromoteTask.Text)
            {
                EditTask?.Invoke(2);
            }
            else if (sender.ToString() == toolStripMenuItem_AddOutTask.Text)
            {
                EditTask?.Invoke(3);
            }
        }

        private void UpdateDataGridViewCellsVisibility(int mode)
        {
            UpdateDG();
            panel4.Enabled = false;
            DG_SizeChanged(dataGridView, EventArgs.Empty);

            int hRowsCount = 0;
            int height = dataGridView.Rows[1].Height;

            List<int> hColumns = Enumerable.Range(1, dataGridView.Columns.Count - 1).ToList();
            int width = dataGridView.Columns[1].Width;

            switch (mode)
            {
                case 1:                  
                    for (int r = 1; r < dataGridView.Rows.Count; r++)
                    {
                        bool hide = true;
                        foreach (DataGridViewCell cell in dataGridView.Rows[r].Cells)
                        {
                            var dd = cell.Tag;
                            if (cell.Tag != null && cell.Tag.ToString() != "-1")
                            {
                                hide = false;
                                if (hColumns.Contains(cell.ColumnIndex)) hColumns.Remove(cell.ColumnIndex);
                            }
                        }
                        if (hide)
                        {
                            dataGridView.Rows[r].Visible = false;
                            hRowsCount += 1;
                        }
                    }

                    break;
                case 2:
                    if (GlobalMethodes.ReadSQL_GetUserSets())
                    {
                        List<string> rowUserSets = new List<string>();
                        hColumns.Clear();

                        if (GlobalData.SelectedStage.LanguageId == 2)
                        {
                            foreach (string set in GlobalData.UserSets)
                            {
                                var s = RequestInfo.lb.SectionsThrees.FirstOrDefault(x => x.SectionThreeNum.ToString() == set);
                                if (s != null) rowUserSets.Add(s.SectionThreeTagEng);
                            }
                        }
                        else
                        {
                            foreach (string set in GlobalData.UserSets)
                            {
                                var s = RequestInfo.lb.SectionsThrees.FirstOrDefault(x => x.SectionThreeNum.ToString() == set);
                                if (s != null) rowUserSets.Add(s.SectionThreeTagRus);
                            }
                        }
                        for (int r = 1; r < dataGridView.Rows.Count; r++)
                        {
                            try
                            {
                                if (!rowUserSets.Contains(dataGridView.Rows[r].Cells[0].Value.ToString()))
                                {
                                    dataGridView.Rows[r].Visible = false;
                                    hRowsCount += 1;
                                }
                            }
                            catch { }
                        } 
                    }
                    break;
                case 3:                  
                    if (GlobalMethodes.ReadSQL_GetUserSets())
                    {
                        List<string> columnUserSets = new List<string>();

                        if (GlobalData.SelectedStage.LanguageId == 2)
                        {
                            foreach (string set in GlobalData.UserSets)
                            {
                                var s = RequestInfo.lb.SectionsThrees.FirstOrDefault(x => x.SectionThreeNum.ToString() == set);
                                if (s != null) columnUserSets.Add(s.SectionThreeTagEng);
                            }
                        }
                        else
                        {
                            foreach (string set in GlobalData.UserSets)
                            {
                                var s = RequestInfo.lb.SectionsThrees.FirstOrDefault(x => x.SectionThreeNum.ToString() == set);
                                if (s != null) columnUserSets.Add(s.SectionThreeTagRus);
                            }
                        }
                        for (int c = 1; c < dataGridView.Columns.Count; c++)
                        {
                            try
                            {
                                if (columnUserSets.Contains(dataGridView.Rows[0].Cells[c].Value.ToString()))
                                {
                                    hColumns.Remove(c);
                                }
                            }
                            catch { }
                        } 
                    }
                    break;
            }

            if (mode > 0)
            {
                if (hColumns.Count > 0)
                {
                    foreach (int c in hColumns)
                    {
                        dataGridView.Columns[c].Visible = false;
                        DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                        column.Width = width;
                        column.HeaderText = "";
                        dataGridView.Columns.Add(column);
                    }
                }

                for (int r = 0; r <= hRowsCount; r++)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridView);
                    row.Height = height;
                    dataGridView.Rows.Add(row);
                }

                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    for (int c = 0; c < dataGridView.Columns.Count; c++)
                    {
                        if (i == 0)
                        {
                            dataGridView.Rows[i].Cells[c].Style.BackColor = Color.AliceBlue;
                        }
                        else
                        {
                            if (i == c)
                            {
                                dataGridView.Rows[i].Cells[c].Style.BackColor = Color.AliceBlue;
                            }
                            if (i < dataGridView.Rows.Count - hRowsCount - 1)
                            {
                                if (c >= dataGridView.Columns.Count - hColumns.Count)
                                {
                                    dataGridView.Rows[i].Cells[c].Style.BackColor = Color.WhiteSmoke;
                                }
                            }
                            else
                            {
                                dataGridView.Rows[i].Cells[c].Style.BackColor = Color.WhiteSmoke;
                            }
                        }

                    } 
                }
                
            }
            panel4.Enabled = true;
        }

        /// <summary>
        /// Refresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                RequestInfo.requestInfoThree();
            }
            catch
            {
                MessageBox.Show("Сервер не доступен!");
            }
            try
            {
                var cell = dataGridView.SelectedCells[0];
                UpdateDG(dataGridView.Rows[cell.RowIndex].Cells[0].Value.ToString(), dataGridView.Rows[0].Cells[cell.ColumnIndex].Value.ToString(), 0);
            }
            catch
            {

            }
            DG_SizeChanged(this, EventArgs.Empty);

        }

        private void button_Help_Click(object sender, EventArgs e)
        {
            var url = @"https://kb.unitsky.com/pages/viewpage.action?pageId=63603411";
            System.Diagnostics.Process.Start(url);
        }

        public bool SelectTaskCell(string from, string to)
        {
            LibraryDB.DB.Task _task = null;
            try
            {
                _task = RequestInfo.lb.Tasks.FirstOrDefault(x => x.TaskId.ToString() == from);
            }
            catch { }

            LibraryDB.DB.TaskDepartment _taskDepartment = null;
            try
            {
                _taskDepartment = RequestInfo.lb.TaskDepartments.FirstOrDefault(x => x.TaskDepartmentId.ToString() == to);
            }
            catch { }

            if (_task != null && _taskDepartment != null)
            {
                int columnIndex = 0;
                int rowIndex = 0;

                for (int c = 1; c < dataGridView.Columns.Count; c++)
                {
                    if (dataGridView.Rows[0].Cells[c].Tag.ToString() == _taskDepartment.SectionThreeId.ToString())
                    {
                        columnIndex = c;
                        break;
                    }
                }

                for (int r = 1; r < dataGridView.Rows.Count; r++)
                {
                    if (dataGridView.Rows[r].Cells[0].Tag.ToString() == _task.SectionThreeId.ToString())
                    {
                        rowIndex = r;
                        break;
                    }
                }

                if (columnIndex > 0 && rowIndex > 0)
                {
                    DataGridViewCell cell = dataGridView.Rows[rowIndex].Cells[columnIndex];
                    dataGridView.CurrentCell = cell;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(columnIndex, rowIndex);
                    DG_Cell_Click(cell, e);





                    return true;
                } 
            }
            return false;
        }

        private void CreateVerticalText(PaintEventArgs e)
        {
            string myText = "От кого:";
            FontFamily fontFamily = new FontFamily("Bahnschrift");
            Font font = new Font(fontFamily, 12, FontStyle.Regular, GraphicsUnit.Point);
            Rectangle rectangle = e.ClipRectangle;
            PointF pointF = new PointF(rectangle.Width/2, rectangle.Height / 2);
            StringFormat stringFormat = new StringFormat();
            SolidBrush solidBrush = new SolidBrush(Color.Black);

            stringFormat.FormatFlags = StringFormatFlags.DirectionVertical;
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            //e.Graphics.RotateTransform(-90F);
            e.Graphics.DrawString(myText, font, solidBrush, pointF, stringFormat);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            CreateVerticalText(e);
        }

        private void panel5_SizeChanged(object sender, EventArgs e)
        {
            panel5.Invalidate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox!= null)
            {
                if (comboBox2.SelectedIndex == 0)
                {
                    UpdateDataGridViewCellsVisibility(comboBox.SelectedIndex); 
                }
                else
                {
                    UpdateDG1();
                    DataGridViewCellEventArgs ea = new DataGridViewCellEventArgs(0, dataGridView1.SelectedRows[0].Cells[0].RowIndex);
                    DG1_Cell_Click(dataGridView1.SelectedRows[0].Cells[0], ea);
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedIndex == 0)
            {
                try
                {
                    DataGridViewCellEventArgs ev = new DataGridViewCellEventArgs(dataGridView.SelectedCells[0].ColumnIndex, dataGridView.SelectedCells[0].RowIndex);
                    DG_Cell_Click(dataGridView.SelectedCells[0], ev);
                }
                catch { }
                tableLayoutPanel4.BringToFront();
            }
            else if (comboBox.SelectedIndex == 1)
            {
                UpdateDG1();  
                //TaskRowClick(Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[7].Value), dataGridView1.SelectedRows[0].Cells[3].Value.ToString(), dataGridView1.SelectedRows[0].Cells[4].Value.ToString());
                //string txt = $"{dataGridView1.SelectedRows[0].Cells[0].Value} {dataGridView1.SelectedRows[0].Cells[3].Value}->{dataGridView1.SelectedRows[0].Cells[4].Value}";
                //var status = RequestInfo.lb.Status.FirstOrDefault(x => x.StatusName == dataGridView1.SelectedRows[0].Cells[5].Value.ToString());
                //UpdateTaskRoute(txt, status.StatusId, 0);

                DataGridViewCellEventArgs ea = new DataGridViewCellEventArgs(0, dataGridView1.SelectedRows[0].Cells[0].RowIndex);
                DG1_Cell_Click(dataGridView1.SelectedRows[0].Cells[0], ea);
                dataGridView1.BringToFront();
            }
        }
    }
}
