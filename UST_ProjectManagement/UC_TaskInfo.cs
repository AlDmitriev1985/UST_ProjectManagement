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

        string From = "";
        string To = "";

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
            tableLayoutPanel3.BackColor = MainForm.HeaderColor;
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
        
        public void UpdateHistory(int id)
        {
            listView1.Items.Clear();
            listView1.Groups.Clear();

            listView1.Columns[0].Width = 120;
            listView1.Columns[1].Width = MainForm.CreatePanelWidth - 120;

            string taskNum = "";
            TD = null;
            int routetype = 0;
            HistoryLog historyLog = null;

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
                historyLog = JsonConvert.DeserializeObject<HistoryLog>(TD.TaskDepartmentHistory);
            }
            
            if (TD != null)
            {
                
            }
            if (historyLog != null)
            {
                historyLog.spHistory.Reverse();
                foreach (var story in historyLog.spHistory)
                {
                    ListViewGroup LvGr = new ListViewGroup(story.Date);
                    LvGr.Name = story.Date;
                    listView1.Groups.AddRange(new ListViewGroup[] { LvGr });

                    ListViewItem lvi1 = new ListViewItem(new string[] { "Изменениия внес:", story.User }, LvGr);
                    listView1.Items.Add(lvi1);

                    ListViewItem lvi2 = new ListViewItem(new string[] { "Статус:", GetStatus(story.Info) }, LvGr);
                    listView1.Items.Add(lvi2);

                    ListViewItem lvi3 = new ListViewItem(new string[] { "Комментарий:", story.Description }, LvGr);
                    listView1.Items.Add(lvi3);
                }
            }
            else
            {
                ListViewGroup LvGr = new ListViewGroup("История отсутствует");
                LvGr.Name = "История отсутствует";
                listView1.Groups.AddRange(new ListViewGroup[] { LvGr });

                ListViewItem lvi1 = new ListViewItem(new string[] { " ", " " }, LvGr);
                listView1.Items.Add(lvi1);
            }
            try
            {
                OpenTaskRoute?.Invoke($"{TaskCode} {From}->{To}", TD.StatusId, routetype);
            }
            catch
            {
                OpenTaskRoute?.Invoke($"{TaskCode} {From}->{To}", -1, routetype);
            }
        }

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

            PublishForm.mode = 1;
            PublishForm.modeApplication = modeApplication;
            PublishForm publishForm = new PublishForm();
            publishForm.button2.Click -= new System.EventHandler(publishForm.button2_Click);
            publishForm.textBox3.Text = GlobalData.SelectedPosition.PositionCode;
            publishForm.textBox4.Text = GlobalData.SelectedStage.StageTag;
            publishForm.spSectionsThree = positionInfo.ListSecThree;
            publishForm.CheckPositionCode(From, 1);
            publishForm.GetDepartmentAndSections(To, GlobalData.SelectedPosition);
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
        /// <summary>
        /// OPenTaskCard
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
                general.EditCoord += EditCoordinate;
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

        
    }
}
