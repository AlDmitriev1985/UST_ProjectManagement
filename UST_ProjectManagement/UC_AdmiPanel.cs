using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace UST_ProjectManagement
{
    public partial class UC_AdmiPanel : UserControl
    {

        public delegate void SelectItmChange();
        public event SelectItmChange SelectStartBtn;
        public event SelectItmChange UnSelectStartBtn;

        public delegate void ButtonValeraStart_Click();
        public event ButtonValeraStart_Click ValeraStart;

        public delegate void ButtonValeraApprove();
        public event ButtonValeraApprove ValeraApprove;

        public delegate void ButtonNWFStart_Click();
        public event ButtonNWFStart_Click NWFStart;

        public delegate void ButtonNWFApprove();
        public event ButtonNWFApprove NWFApprove;


        ContextMenuStrip contextMenu = new ContextMenuStrip();
        ToolStripMenuItem stripItemStart = new ToolStripMenuItem();
        ToolStripMenuItem stripItemFinish = new ToolStripMenuItem();
        ToolStripMenuItem stripItemReset = new ToolStripMenuItem();
        ToolStripMenuItem stripItemRefresh = new ToolStripMenuItem();

        DataGridView dataGridView_Tasks = new DataGridView();
        List<string> columns_Tasks = new List<string>() { "Шифр", "Взял в работу", "Статус", "Дата", "Тип"};

        List<classCreatedSchedule> ScheduleList = new List<classCreatedSchedule>();
        public UC_AdmiPanel()
        {
            InitializeComponent();

            stripItemStart.Text = "Создать ЦФХ";
            stripItemStart.Image = Properties.Resources.Start_20x20;
            stripItemStart.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemStart);

            stripItemFinish.Text = "ЦФХ готовы";
            stripItemFinish.Image = Properties.Resources.Finish_20x20;
            stripItemFinish.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemFinish);

            stripItemReset.Text = "Сбросить";
            stripItemReset.Image = Properties.Resources.Reset_20x20;
            stripItemReset.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemReset);

            stripItemRefresh.Text = "Обновить";
            stripItemRefresh.Image = Properties.Resources.Refresh_20x20;
            stripItemRefresh.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemRefresh);

            CardTask.Methodes_DataGrid.CreateDataGrid(dataGridView_Tasks, columns_Tasks);
            dataGridView_Tasks.ColumnHeadersHeight = 55;
            dataGridView_Tasks.Margin = new Padding(0);
            dataGridView_Tasks.BorderStyle = BorderStyle.None;
            dataGridView_Tasks.Dock = DockStyle.Fill;
            dataGridView_Tasks.SelectionChanged += listView1_SizeChanged;
            dataGridView_Tasks.MouseDown += dataGridView_Tasks_MouseDown;
            dataGridView_Tasks.MultiSelect = false;
            Controls.Add(dataGridView_Tasks);
            dataGridView_Tasks.BringToFront();

            
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            int n = dataGridView_Tasks.Columns.Count;
            for (int i = 0; i < n - 1; i++)
            {
                dataGridView_Tasks.Columns[i].Width = dataGridView_Tasks.Width / n;
            }

            dataGridView_Tasks.Columns[n - 1].Width = dataGridView_Tasks.Width - (dataGridView_Tasks.Width / n * (n - 1));
        }

        private void dataGridView_Tasks_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                var ht = dataGridView_Tasks.HitTest(e.X, e.Y);

                foreach (DataGridViewRow row in dataGridView_Tasks.SelectedRows)
                {
                    if (ht.RowIndex == row.Index)
                    {
                        contextMenu.Show(MousePosition);
                        break;
                    }
            }
            }

        }

        private void toolStripItem_Click(object sender, EventArgs args)
        {
            if (sender.ToString() == stripItemStart.Text)
            {
                if (GlobalData.OpenPanelIndex == 3)
                {
                    ValeraStart?.Invoke();
                }
                else if (GlobalData.OpenPanelIndex == 4)
                {
                    NWFStart?.Invoke();
                }
            }
            else if (sender.ToString() == stripItemFinish.Text)
            {
                if (GlobalData.OpenPanelIndex == 3)
                {
                    ValeraApprove?.Invoke();
                }
                else if (GlobalData.OpenPanelIndex == 4)
                {
                    NWFApprove?.Invoke();
                }
            }
            else if (sender.ToString() == stripItemReset.Text)
            {
                GlobalMethodes.ResetRVT(GetSelectedCode(), GlobalData.user.UserAccount);
            }
            else if (sender.ToString() == stripItemRefresh.Text)
            {

            }
            if (GlobalData.OpenPanelIndex == 3)
            {
                GetAllSetList();
            }
            else if (GlobalData.OpenPanelIndex == 4)
            {
                GetAllNavisList();
            }
        }

        public void GetAllSetList()
        {
            ScheduleList.Clear();
            dataGridView_Tasks.Rows.Clear();

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/AddPosition/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";

                string text = "SectionGet" + Environment.NewLine;


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
                                string[] inf = line.Split(';');
                                classCreatedSchedule sch = new classCreatedSchedule(inf[0]);
                                sch.PositionId = inf[1];
                                sch.Responsble = inf[2];
                                sch.Status = inf[3];
                                sch.Date = inf[4];
                                try { sch.Type = inf[5]; }
                                catch { }
                                ScheduleList.Add(sch);

                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(dataGridView_Tasks);
                                row.SetValues(new string[] { sch.PositionId, sch.Responsble, sch.Status, sch.Date, sch.Type });
                                dataGridView_Tasks.Rows.Add(row);
                                row.Height = CardTask.Methodes_DataGrid.RowHeight;
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

            listView1_SizeChanged(dataGridView_Tasks, EventArgs.Empty);
        }

        public void GetAllNavisList()
        {
            ScheduleList.Clear();
            dataGridView_Tasks.Rows.Clear();

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://10.10.25.130:8085/AddPosition/");
                request.UseDefaultCredentials = true;
                request.ContentType = "text/html";
                request.Method = "POST";

                string text = "SectionGetNWF" + Environment.NewLine;


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
                                string[] inf = line.Split(';');
                                classCreatedSchedule sch = new classCreatedSchedule(inf[0]);
                                sch.PositionId = inf[1];
                                sch.Responsble = inf[2];
                                sch.Status = inf[3];
                                sch.Date = inf[4];
                                sch.Path = inf[5];
                                try { sch.Type = inf[6]; }
                                catch { }
                                ScheduleList.Add(sch);

                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(dataGridView_Tasks);
                                row.SetValues(new string[] { sch.PositionId, sch.Responsble, sch.Status, sch.Date, sch.Type });
                                dataGridView_Tasks.Rows.Add(row);
                                row.Height = CardTask.Methodes_DataGrid.RowHeight;

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
            listView1_SizeChanged(dataGridView_Tasks, EventArgs.Empty);
        }

        public string GetSelectedCode()
        {
            int index = 0;
            string type = "";

            foreach(DataGridViewRow itm in dataGridView_Tasks.SelectedRows)
            {
                index = itm.Index;
                type = itm.Cells[4].Value.ToString();
            }

            return ScheduleList[index].ID + ";" + type;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool result = false;
            if (dataGridView_Tasks.SelectedRows.Count > 0)
            {
                classCreatedSchedule sch = ScheduleList.Find(id => id.PositionId == dataGridView_Tasks.SelectedRows[0].Cells[0].Value.ToString());
                if (File.Exists(sch.Path)) result = true;
        
            }
            if (result == true) SelectStartBtn?.Invoke();
            else UnSelectStartBtn?.Invoke();


           // MessageBox.Show(result.ToString());

        }
    }
}
