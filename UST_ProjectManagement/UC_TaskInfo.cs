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
using Newtonsoft.Json;
using POSTServer.History;
using CardTask;

namespace UST_ProjectManagement
{
    public partial class UC_TaskInfo : UserControl
    {
        public delegate void OpenTaskRoute_Click(string txt, int statusId, int routetype);
        public event OpenTaskRoute_Click OpenTaskRoute;

        public delegate void ChangeTaskStatus_Click(string from, string to, int row);
        public event ChangeTaskStatus_Click ChangeTaskStatus;

        public delegate void GetTaskOpenning(string json, int attachId, int TaskId, string NumberTask, string tag);
        public event GetTaskOpenning GetOpenning;

        public delegate void GetTaskTopo(string json, int attachId, int TaskId, string NumberTask);
        public event GetTaskTopo GetTopo;

        public delegate void TaskCoord(object obj);
        public event TaskCoord EditCoord;


        /// <summary>
        /// Режим запуска приложения, 0 - Manager; 1 - Revit
        /// </summary>
        public static byte modeApplication = 1;

        List<Task> Tasks;
        DataGridView dataGridView;
        int rowindex = 0;
        TaskDepartment TD;
        string TaskCode = "";
        Task SelectedTask;

        public string From = "";
        public string To = "";

        ContextMenuStrip contextMenu = new ContextMenuStrip();
        ToolStripMenuItem stripItemOpen = new ToolStripMenuItem();
        ToolStripMenuItem stripItemPromote = new ToolStripMenuItem();

        public PublishForm general = new PublishForm();

        public UC_TaskInfo()
        {
            InitializeComponent();

            dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersVisible = false;
            //dataGridView.ColumnHeadersVisible = false;
            dataGridView.ReadOnly = true;
            dataGridView.MultiSelect = false;
            dataGridView.BackgroundColor = Color.WhiteSmoke;
            dataGridView.Margin = new Padding(0);
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.AllowUserToResizeColumns = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToOrderColumns = false;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.ColumnHeadersHeight = 30;
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.AliceBlue;
            

            dataGridView.Columns.Add("column0", "");
            dataGridView.Columns[0].Width = 20;


            dataGridView.Columns.Add("column1", "№");
            dataGridView.Columns[1].Width = 70;

            dataGridView.Columns.Add("column3", "Имя");
            dataGridView.Columns[2].Width = 150;

            dataGridView.Columns.Add("column4", "Статус");
            dataGridView.Columns[3].Width = 100;

            dataGridView.Columns.Add("column5", "tdid");
            dataGridView.Columns[4].Width = 100;
            dataGridView.Columns[4].Visible = false;

            dataGridView.RowsDefaultCellStyle.SelectionBackColor = MainForm.HeaderColor;
            dataGridView.Columns[0].DefaultCellStyle.SelectionBackColor = DefaultBackColor;
            dataGridView.SelectionChanged += new EventHandler(DG_Cell_Select);
            dataGridView.SizeChanged += new EventHandler(DG_SizeChanged);
            dataGridView.CellClick += new DataGridViewCellEventHandler(DG_Cell_Click);
            //dataGridView.CellMouseEnter += new DataGridViewCellEventHandler(DG_Cell_MouseEnter);
            //dataGridView.CellMouseLeave += new DataGridViewCellEventHandler(DG_Cell_MouseLeave);

            panelDG.Controls.Add(dataGridView);
            dataGridView.MouseDown += dataGridView_MouseDown;

            stripItemOpen.Text = "Открыть";
            stripItemOpen.Image = Properties.Resources.Btn_OpenCard_20x20;
            stripItemOpen.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemOpen);

            stripItemPromote.Text = "Изменить статус";
            stripItemPromote.Image = Properties.Resources.Btn_Promote_20x20;
            stripItemPromote.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemPromote);

            tableLayoutPanel2.BackColor = MainForm.HeaderColor;
            panel_Color.BackColor = MainForm.HeaderColor;
            //button1.BackColor = MainForm.HeaderColor;
            //button2.BackColor = MainForm.HeaderColor; 
        }

        private void UC_TaskInfo_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(button3, "Изменить статус");
            toolTip.SetToolTip(button4, "Открыть карточку задания");
            toolTip.SetToolTip(button2, "Создать задание");
        }

        public void UpdateDG(List<int> ids, List<int>tdids, string from, string to)
        {
            From = from;
            To = to;

            tableLayoutPanel1.RowStyles[1].Height = (this.Height - 30) / 3;
            tableLayoutPanel1.RowStyles[2].Height = 5;


            dataGridView.Rows.Clear();
            Tasks = new List<Task>();
            label1.Text = $"Задание от {from} для {to}";
            if (ids != null)
            {
                foreach (int id in ids)
                {
                    Task task = RequestInfo.lb.Tasks.FirstOrDefault(x => x.TaskId == id);
                    if (task != null) Tasks.Add(task);
                }

                Tasks = Tasks.OrderByDescending(x => x.TaskId).ToList();
                int routetype = 0;
                foreach (Task t in Tasks)
                {
                    //t.TaskDepartments = t.TaskDepartments.Where(p => p.PositionId == positionId).OrderByDescending(x => x.TaskDepartmentId).ToList();
                    t.TaskDepartments = t.TaskDepartments.OrderByDescending(x => x.TaskDepartmentId).ToList();
                    
                    foreach (TaskDepartment td in t.TaskDepartments)
                    {
                        if (tdids.Contains(td.TaskDepartmentId))
                        {
                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataGridView);
                            row.Cells[0].Style.BackColor = GlobalMethodes.GetCellColor(td.StatusId, GlobalMethodes.GetRouteType(t, td.TaskDepartmentId));
                            row.Cells[1].Value = $"{t.TaskNumer}-{t.TaskYear}";
                            row.Cells[2].Value = t.TaskName;
                            Status status = RequestInfo.lb.Status.FirstOrDefault(x => x.StatusId == td.StatusId);
                            row.Cells[3].Value = status.StatusName;
                            row.Cells[4].Value = td.TaskDepartmentId;
                            dataGridView.Rows.Add(row);
                            row.Height = 30;
                        }
                    }

                } 
            }
            if (dataGridView.Rows.Count > 0)
            {
                DG_Cell_Click(dataGridView.Rows[0].Cells[1], new DataGridViewCellEventArgs(1, 0));
                //DG_Cell_Select(dataGridView.Rows[0].Cells[1], EventArgs.Empty);
            }
            else
            {
                UpdateHistory(-1);
            }
        }

        private void DG_SizeChanged(object sender, EventArgs e)
        {
            dataGridView.Columns[2].Width = dataGridView.Width - 190 - 2;
        }

        private void DG_Cell_Click(object sender, DataGridViewCellEventArgs e)
        {
            rowindex = e.RowIndex;
            if (dataGridView.Rows.Count > 0)
            {
                //dataGridView.Rows[e.RowIndex].Cells[0].Selected = false;
                //dataGridView.Rows[e.RowIndex].Cells[1].Selected = true;
                DG_Cell_Select(dataGridView.Rows[0].Cells[1], EventArgs.Empty);
            }
            else
            {
                UpdateHistory(-1);
            }
        }

        private void DG_Cell_Select (object sender, EventArgs e)
        {   
            if (dataGridView.Rows.Count > 0 && rowindex >= 0)
            {
                if (dataGridView.Rows[rowindex].Cells[0].Style.BackColor != Color.White)
                {
                    dataGridView.RowsDefaultCellStyle.SelectionBackColor = dataGridView.Rows[rowindex].Cells[0].Style.BackColor;
                }
                else
                {
                    dataGridView.RowsDefaultCellStyle.SelectionBackColor = MainForm.HeaderColor;
                }
                if (dataGridView.RowsDefaultCellStyle.SelectionBackColor == Color.Gainsboro || dataGridView.RowsDefaultCellStyle.SelectionBackColor == Color.White)
                {
                    dataGridView.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
                }
                else
                {
                    dataGridView.RowsDefaultCellStyle.SelectionForeColor = Color.White;
                }
                //dataGridView.Rows[rowindex].Cells[0].Selected = false;
                if (dataGridView.SelectedCells.Count > 0 && dataGridView.SelectedCells[0].RowIndex == rowindex)
                {
                    int id = Convert.ToInt32(dataGridView.Rows[rowindex].Cells[4].Value);
                    UpdateHistory(id);
                }
            }
            rowindex = 0;

        }

        public bool SelectRowByTdIdAndOpenTask(string tdid)
        {
            foreach(DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells[4].Value.ToString() == tdid)
                {
                    row.Selected = true;
                    rowindex = row.Index;
                    DG_Cell_Select(row.Cells[1], EventArgs.Empty);
                    return true;
                }
            }
            return false;
        }

        #region HistoryPanel
       
        List<string> panelsNum = new List<string>();
        int num = 0;
        int Y = 5;
        int X = 5;

        public void UpdateHistory(int id)
        {
            panel_History.Controls.Clear();
            panelsNum.Clear();



            string taskNum = "";
            TD = null;
            int routetype = 0;
            HistoryLog history = null;

            foreach (Task tsk in Tasks)
            {
                TaskCode = $"{tsk.TaskNumer}-{tsk.TaskYear}";
                SelectedTask = tsk;
                TD = tsk.TaskDepartments.FirstOrDefault(x => x.TaskDepartmentId == id);
                routetype = GlobalMethodes.GetRouteType(tsk, id);
                if (TD != null) break;
            }


            if (TD != null && TD.TaskDepartmentHistory != null)
            {
                history = JsonConvert.DeserializeObject<HistoryLog>(TD.TaskDepartmentHistory);
            }


            Y = 5;
            if (history != null)
            {
                history.spHistory.Reverse();
                foreach (var story in history.spHistory)
                {
                    Panel panel = new Panel();
                    panel.Name = (num + 1).ToString();
                    panelsNum.Add((num + 1).ToString());
                    panel.BackColor = Color.White;
                    panel.Width = panel_History.Width - 10;
                    panel.BorderStyle = BorderStyle.FixedSingle;
                    panel.Location = new Point(5, Y);

                    Label label0 = new Label();
                    label0.Name = "label0";
                    label0.Text = story.Date;
                    label0.Width = label0.PreferredWidth;
                    label0.ForeColor = Color.Gray;
                    label0.TextAlign = ContentAlignment.TopRight;
                    label0.Location = new Point(panel.Width - label0.Width - 15, 5);
                    panel.Controls.Add(label0);

                    List<Label> labels = new List<Label>();

                    Label label1 = new Label();
                    label1.Name = "header";
                    label1.Text = "Изм. внес:";
                    labels.Add(label1);

                    Label label2 = new Label();
                    label2.Name = "header";
                    label2.Text = "Статус:";
                    labels.Add(label2);

                    Label label3 = new Label();
                    label3.Name = "header";
                    label3.Text = "Коммент.:";
                    labels.Add(label3);

                    foreach (Label label in labels)
                    {
                        label.ForeColor = Color.Gray;
                        panel.Controls.Add(label);
                    }

                    List<Label> infolabels = new List<Label>();

                    Label label1_1 = new Label();
                    label1_1.Name = "discription";
                    User user = RequestInfo.lb.Users.FirstOrDefault(x => x.UserAccount == story.User);
                    if (user != null)
                    {
                        label1_1.Text = user.UserSurname + " " + user.UserName;
                    }
                    else
                    {
                        label1_1.Text = story.User;
                    }
                    infolabels.Add(label1_1);

                    Label label2_1 = new Label();
                    label2_1.Name = "discription";
                    label2_1.Text = GetStatus(story.Info, RequestInfo.lb);
                    infolabels.Add(label2_1);

                    Label label3_1 = new Label();
                    label3_1.Name = "discription";
                    label3_1.Text = story.Description;
                    infolabels.Add(label3_1);

                    foreach (Label label in infolabels)
                    {
                        panel.Controls.Add(label);
                    }

                    panel.Height = 100;
                    panel_History.Controls.Add(panel);
                    panel.Paint += new PaintEventHandler(panel_Paint);
                    num += 1;
                }
            }
            else
            {
                Label label = new Label();
                label.Text = "<Пусто>";
                label.Width = panel_History.Width - 10;
                label.ForeColor = Color.Gray;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Location = new Point(panel_History.Width / 2 - label.Width / 2, panel_History.Height / 2 - label.Height / 2);
                panel_History.Controls.Add(label);
            }
        }

        public void UpdateHistory1 (int id, string from, string to)
        {
            label1.Text = $"Задание от {from} для {to}";
            tableLayoutPanel1.RowStyles[1].Height = 0;
            tableLayoutPanel1.RowStyles[2].Height = 0;

            panel_History.Controls.Clear();
            panelsNum.Clear();

            TD = RequestInfo.lb.TaskDepartments.FirstOrDefault(X => X.TaskDepartmentId == id);
            HistoryLog history = null;

            if (TD != null && TD.TaskDepartmentHistory != null)
            {
                history = JsonConvert.DeserializeObject<HistoryLog>(TD.TaskDepartmentHistory);
            }


            Y = 5;
            if (history != null)
            {
                history.spHistory.Reverse();
                foreach (var story in history.spHistory)
                {
                    Panel panel = new Panel();
                    panel.Name = (num + 1).ToString();
                    panelsNum.Add((num + 1).ToString());
                    panel.BackColor = Color.White;
                    panel.Width = panel_History.Width - 10;
                    panel.BorderStyle = BorderStyle.FixedSingle;
                    panel.Location = new Point(5, Y);

                    Label label0 = new Label();
                    label0.Name = "label0";
                    label0.Text = story.Date;
                    label0.Width = label0.PreferredWidth;
                    label0.ForeColor = Color.Gray;
                    label0.TextAlign = ContentAlignment.TopRight;
                    label0.Location = new Point(panel.Width - label0.Width - 15, 5);
                    panel.Controls.Add(label0);

                    List<Label> labels = new List<Label>();

                    Label label1 = new Label();
                    label1.Name = "header";
                    label1.Text = "Изм. внес:";
                    labels.Add(label1);

                    Label label2 = new Label();
                    label2.Name = "header";
                    label2.Text = "Статус:";
                    labels.Add(label2);

                    Label label3 = new Label();
                    label3.Name = "header";
                    label3.Text = "Коммент.:";
                    labels.Add(label3);

                    foreach (Label label in labels)
                    {
                        label.ForeColor = Color.Gray;
                        panel.Controls.Add(label);
                    }

                    List<Label> infolabels = new List<Label>();

                    Label label1_1 = new Label();
                    label1_1.Name = "discription";
                    User user = RequestInfo.lb.Users.FirstOrDefault(x => x.UserAccount == story.User);
                    if (user != null)
                    {
                        label1_1.Text = user.UserSurname + " " + user.UserName;
                    }
                    else
                    {
                        label1_1.Text = story.User;
                    }
                    infolabels.Add(label1_1);

                    Label label2_1 = new Label();
                    label2_1.Name = "discription";
                    label2_1.Text = GetStatus(story.Info, RequestInfo.lb);
                    infolabels.Add(label2_1);

                    Label label3_1 = new Label();
                    label3_1.Name = "discription";
                    label3_1.Text = story.Description;
                    infolabels.Add(label3_1);

                    foreach (Label label in infolabels)
                    {
                        panel.Controls.Add(label);
                    }

                    panel.Height = 100;
                    panel_History.Controls.Add(panel);
                    panel.Paint += new PaintEventHandler(panel_Paint);
                    num += 1;
                }
            }
            else
            {
                Label label = new Label();
                label.Text = "<Пусто>";
                label.Width = panel_History.Width - 10;
                label.ForeColor = Color.Gray;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Location = new Point(panel_History.Width / 2 - label.Width / 2, panel_History.Height / 2 - label.Height / 2);
                panel_History.Controls.Add(label);
            }
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {

            int w = panel_History.Width;
            int step = 25;
            int height = 0;

            Panel panel = sender as Panel;

            if (panelsNum.Contains(panel.Name))
            {
                panelsNum.Remove(panel.Name);
                List<Label> headers = new List<Label>();
                List<Label> discriptions = new List<Label>();

                foreach (Control control in panel.Controls)
                {
                    try
                    {
                        Label label = control as Label;
                        if (label.Name == "header")
                        {
                            headers.Add(label);
                        }
                        else if (label.Name == "discription")
                        {
                            discriptions.Add(label);
                        }
                    }
                    catch { }
                }

                try
                {
                    int W = 0;
                    foreach (Label label in headers)
                    {
                        label.Width = label.PreferredWidth;
                        if (W < label.Width) W = label.Width;
                    }

                    int W1 = panel.Width - W - 15;
                    foreach (Label label in discriptions)
                    {
                        label.Width = W1;
                        panel.Controls.Add(label);
                        RefreshLabel(label);
                    }

                    int rowY = 25;
                    int columnX = W + 10;
                    for (int i = 0; i < headers.Count; i++)
                    {
                        headers[i].Location = new Point(5, rowY);
                        discriptions[i].Location = new Point(columnX, rowY);

                        if (discriptions[i].Height > step)
                        {
                            rowY += discriptions[i].Height;
                        }
                        else
                        {
                            rowY += step;
                        }
                        if (i == headers.Count - 1)
                        {
                            height = discriptions[i].Location.Y + discriptions[i].Height + 10;
                        }
                    }
                    panel.Height = height;
                    panel.Location = new Point(X, Y);
                    Y += panel.Height + 5;

                    if (!panel_History.AutoScroll && Y - 5 > panel_History.Height)
                    {
                        panel_History.AutoScroll = true;
                        this.Width += 15;
                    }
                }
                catch
                {
                }
            }
        }

        private void RefreshLabel(Label label)
        {
            int limit = label.Width;
            string txt = "";
            string txt0 = "";

            if (label.PreferredSize.Width + 5 > limit)
            {
                string[] spliter = label.Text.Split(' ');
                for (int i = 0; i < spliter.Length; i++)
                {
                    txt += spliter[i] + " ";
                    label.Text = txt;
                    if (label.PreferredSize.Width > limit)
                    {
                        label.Text = txt0 + Environment.NewLine + spliter[i] + " ";
                        txt = label.Text;
                        txt0 = label.Text;
                    }
                    else
                    {
                        txt0 = txt;
                    }
                }
            }
            if (label.Height < label.PreferredSize.Height)
            {
                label.Height = label.PreferredSize.Height + 5;
            }
        }

        private string GetStatus(string status, LibraryDB.LibraryDB lb)
        {
            string[] spl = status.Split(':');
            int id = -1;
            try
            {
                id = Convert.ToInt32(spl[1]);
            }
            catch { }
            if (id != -1)
            {
                var st = lb.Status.FirstOrDefault(x => x.StatusId == id);
                if (st != null) status = st.StatusName;
            }
            return status;
        } 
        #endregion

        //public void UpdateHistory(int id)
        //{
        //    listView1.Items.Clear();
        //    listView1.Groups.Clear();

        //    listView1.Columns[0].Width = 120;
        //    listView1.Columns[1].Width = MainForm.CreatePanelWidth - 120;

        //    string taskNum = "";
        //    TD = null;
        //    int routetype = 0;
        //    HistoryLog historyLog = null;

        //    foreach (Task tsk in Tasks)
        //    {
        //        TaskCode = $"{tsk.TaskNumer}-{tsk.TaskYear}";
        //        SelectedTask = tsk;
        //        TD = tsk.TaskDepartments.FirstOrDefault(x => x.TaskDepartmentId == id);
        //        routetype = GlobalMethodes.GetRouteType(tsk, id);
        //        if (TD != null) break;
        //    }


        //    if (TD != null && TD.TaskDepartmentHistory != null)
        //    {
        //        historyLog = JsonConvert.DeserializeObject<HistoryLog>(TD.TaskDepartmentHistory);
        //    }

        //    if (TD != null)
        //    {

        //    }
        //    if (historyLog != null)
        //    {
        //        historyLog.spHistory.Reverse();
        //        foreach (var story in historyLog.spHistory)
        //        {
        //            ListViewGroup LvGr = new ListViewGroup(story.Date);
        //            LvGr.Name = story.Date;
        //            listView1.Groups.AddRange(new ListViewGroup[] { LvGr });

        //            ListViewItem lvi1 = new ListViewItem(new string[] { "Изменениия внес:", story.User }, LvGr);
        //            listView1.Items.Add(lvi1);

        //            ListViewItem lvi2 = new ListViewItem(new string[] { "Статус:", GetStatus(story.Info) }, LvGr);
        //            listView1.Items.Add(lvi2);

        //            ListViewItem lvi3 = new ListViewItem(new string[] { "Комментарий:", story.Description }, LvGr);
        //            listView1.Items.Add(lvi3);
        //        }
        //    }
        //    else
        //    {
        //        ListViewGroup LvGr = new ListViewGroup("История отсутствует");
        //        LvGr.Name = "История отсутствует";
        //        listView1.Groups.AddRange(new ListViewGroup[] { LvGr });

        //        ListViewItem lvi1 = new ListViewItem(new string[] { " ", " " }, LvGr);
        //        listView1.Items.Add(lvi1);
        //    }
        //    try
        //    {
        //        OpenTaskRoute?.Invoke($"{TaskCode} {From}->{To}", TD.StatusId, routetype);
        //    }
        //    catch
        //    {
        //        OpenTaskRoute?.Invoke($"{TaskCode} {From}->{To}", -1, routetype);
        //    }
        //}

        private string GetStatus(string status)
        {
            string[] spl = status.Split(':');
            int id = -1;
            try
            {
                id = Convert.ToInt32(spl[1]);
            }
            catch { }
            if (id != -1)
            {
                var st = RequestInfo.lb.Status.FirstOrDefault(x => x.StatusId == id);
                if (st != null) status = st.StatusName;
            }
            return status;
        }
        /// <summary>
        /// AddTask
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button2_Click(object sender, EventArgs e)
        {
            PositionInfo positionInfo = new PositionInfo(GlobalData.SelectedProject, GlobalData.SelectedStage, GlobalData.SelectedPosition);
            bool OpenCardTask = true;
            Position position = null;
            ScheduleItem scheduleItem = null;
            Form_AddTask form = null;

            var usersets = positionInfo.scheduleItems.Where(x => x.DepId == GlobalData.user.DepartmentId || x.DelegatedDepId == GlobalData.user.DepartmentId).Select(x => x.SecThreeTag + x.SecThreePostfix);

            if (tableLayoutPanel1.RowStyles[1].Height == 0 || !usersets.Contains(From))
            {
                form = new Form_AddTask();
                OpenCardTask = false;
                try
                {
                    form.comboBox1.Items.AddRange(usersets.ToArray());
                    form.comboBox1.SelectedIndex = 0;
                }
                catch { }
                form.StartPosition = FormStartPosition.CenterParent;
                form.comboBox2.Items.AddRange(RequestInfo.lb.Projects.Select(x => x.ProjectId).ToArray());
                form.comboBox2.Text = positionInfo.ProjectId;
                form.comboBox3.Text = positionInfo.StageTag;
                form.comboBox4.Text = positionInfo.Code;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (form.comboBox1.Text != "<none>")
                    {
                        From = form.comboBox1.Text;
                        OpenCardTask = true;
                    }
                    position = form.Position;
                    if (position.PositionCode != GlobalData.SelectedPosition.PositionCode)
                    {
                        scheduleItem = form.PositionInfo.scheduleItems.FirstOrDefault(x => x.SecThreeTag + x.SecThreePostfix == form.comboBox5.Text);
                    }
                    else
                    {
                        To = form.comboBox5.Text;
                    }
                    
                }
            }
            else
            {
                position = GlobalData.SelectedPosition;
            }

            if (OpenCardTask)
            {
                PublishForm.mode = 1;
                PublishForm.modeApplication = modeApplication;
                PublishForm publishForm = new PublishForm();
                publishForm.button2.Click -= new System.EventHandler(publishForm.button2_Click);
                publishForm.textBox3.Text = GlobalData.SelectedPosition.PositionCode;
                publishForm.textBox4.Text = GlobalData.SelectedStage.StageTag;
                publishForm.spSectionsThree = positionInfo.ListSecThree;
                publishForm.CheckPositionCode(From, 1);
                if(scheduleItem == null)
                {
                    publishForm.GetDepartmentAndSections(To, position);
                }
                else
                {
                    //scheduleItem = form.PositionInfo.scheduleItems.FirstOrDefault(x => x.SecThreeTag + x.SecThreePostfix == form.comboBox5.Text);
                    CardTask.DepTaskUsers depTaskUser = new DepTaskUsers();
                    depTaskUser.SectionThree = RequestInfo.lb.SectionsThrees.FirstOrDefault(x => x.SectionThreeId == scheduleItem.SecThreeId);
                    depTaskUser.Department = RequestInfo.lb.Departments.FirstOrDefault(x => x.DepartmentId == scheduleItem.DepId || x.DepartmentId == scheduleItem.DelegatedDepId);
                    depTaskUser.Pos = position;
                    depTaskUser.SectionPositionNumber = scheduleItem.SecThreePostfix.ToString();
                    //depTaskUser.SectionPositionNumber = form.PositionInfo.SectionsPositions.FirstOrDefault(x => x.SectionThreeId == scheduleItem.SecThreeId).SectionPositionNumber;
                    depTaskUser.SectionThree.SectionPositionNumber = scheduleItem.SecThreePostfix.ToString();
                    depTaskUser.HeadDep = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId == depTaskUser.Department.DepartmentHeade);
                    //depTaskUser.TaskUserId = GlobalData.user;
                    depTaskUser.SectionPositionNumber = scheduleItem.SecThreePostfix.ToString();
                    publishForm.spTaskDep.Add(depTaskUser);
                    publishForm.GetDepartmentAndSections(depTaskUser, scheduleItem.SecThreeTag + scheduleItem.SecThreePostfix);
                }

                publishForm.StartPosition = FormStartPosition.CenterParent;
                if (publishForm.ShowDialog() == DialogResult.OK)
                {
                    string[] answers = publishForm.CreateTaskDB();
                    if (answers[0] == "Готово")
                    {
                        MessageBox.Show(answers[1], "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ChangeTaskStatus?.Invoke(From, To, dataGridView.Rows.Count);
                    }
                    else if (answers[0] == "Error")
                    {
                        MessageBox.Show(answers[1], "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } 
            }
        }
        /// <summary>
        /// OpenTaskCard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button4_Click(object sender, EventArgs e)
        {
            if (TD != null)
            {
                PublishForm.mode = 2;
                PublishForm.modeApplication = modeApplication;
                general = new PublishForm();
                general.GetOpenning += GetOpen;
                general.GetTOPO += GetTop;
                general.EditCoord += EditCoordinate;//Unhold
                general.StartPosition = FormStartPosition.CenterParent;
                general.dataGridView_Files.CellContentDoubleClick += new DataGridViewCellEventHandler(general.dataGridView_Files_CellMouseDoubleClick);

                general.textBox1.TextChanged -= new System.EventHandler(general.textBox1_TextChanged);
                general.textBox3.TextChanged -= new System.EventHandler(general.textBox3_TextChanged);
                general.textBox4.TextChanged -= new System.EventHandler(general.textBox3_TextChanged);
                general.comboBox1.SelectedValueChanged -= new System.EventHandler(general.comboBox1_SelectedValueChanged);
                general.comboBox1.TextChanged -= new System.EventHandler(general.comboBox1_TextChanged);
                general.comboBox2.SelectedValueChanged -= new System.EventHandler(general.comboBox2_SelectedValueChanged);
                general.comboBox3.SelectedValueChanged -= new System.EventHandler(general.comboBox3_SelectedValueChanged);
                general.comboBox3.TextChanged -= new System.EventHandler(general.comboBox3_TextChanged);

                general.GetTaskInfo(TD.TaskDepartmentId);


                if (general.ShowDialog() == DialogResult.Retry)
                {
                    try
                    {
                        button3_Click(button3, EventArgs.Empty);
                    }
                    catch
                    {
                    }
                } 

            }
        }

        private void EditCoordinate(object obj)
        {
            EditCoord?.Invoke(general);
        }

        private void GetOpen(string json, int attachId, int TaskId, string NumberTask, string tagFrom)
        {
            GetOpenning?.Invoke(json, attachId, TaskId, NumberTask, tagFrom);
        }

        private void GetTop(string json, int attachId, int TaskId, string NumberTask)
        {
            GetTopo?.Invoke(json, attachId, TaskId, NumberTask);
        }

        /// <summary>
        /// OpenRouteCard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button3_Click(object sender, EventArgs e)
        {
            try
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
                general.textBox1.TextChanged -= new System.EventHandler(general.textBox1_TextChanged);
                general.textBox2.TextChanged -= new System.EventHandler(general.textBox1_TextChanged);

                string pId = "";
                try
                {
                    pId = RequestInfo.lb.Positions.FirstOrDefault(x => x.PositionId == SelectedTask.PositionId).PositionCode;
                }
                catch (Exception)
                {
                    pId = GlobalData.SelectedPosition.PositionCode;
                }
                general.textBox1.Text = pId;
                general.textBox2.Text = GlobalData.SelectedStage.StageTag;
                general.textBox3.Text = TaskCode;
                From = GlobalMethodes.RefreshSectionTag(From);
                general.textBox4.Text = From;


                general.textBox1.TextChanged += new System.EventHandler(general.textBox1_TextChanged);
                general.textBox2.TextChanged += new System.EventHandler(general.textBox1_TextChanged);
                general.Test();

                DialogResult result = general.ShowDialog();
                if (result == DialogResult.OK)
                {
                    ChangeTaskStatus?.Invoke(From, To, dataGridView.Rows.Count);
                }
            }
            catch
            {
            }
        }

        private void dataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                var ht = dataGridView.HitTest(e.X, e.Y);
                foreach (DataGridViewCell cell in dataGridView.SelectedCells)
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
            if (sender.ToString() == stripItemOpen.Text)
            {
                button4_Click(button4, EventArgs.Empty);
            }
            else if (sender.ToString() == stripItemPromote.Text)
            {
                button3_Click(button3, EventArgs.Empty);
            }
        }

        private void tableLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                if (tableLayoutPanel1.RowStyles[1].Height > 0)
                {
                    tableLayoutPanel1.RowStyles[1].Height = (this.Height - 30) / 3;
                    tableLayoutPanel1.RowStyles[2].Height = 5;
                }
            }
            catch { }
        }
    }
}
