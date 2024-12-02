using LibraryDB.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UST_ProjectManagement;
using POSTServer.History;

namespace UST_ProjectManagement
{
    public partial class Form_Comment : Form
    {
        Point sPoint;
        public static Color HeaderColor = Color.FromArgb(16, 110, 190);
        int X = 5;
        int Y = 5;
        int num = 0;
        List<string> panelsNum = new List<string>();

        public Form_Comment(TaskDepartment taskDepartment, LibraryDB.LibraryDB lb)
        {
            InitializeComponent();
            RefreshPanelsBackColor();
            UpdateHistory(taskDepartment, lb);
        }

        #region --- FormEvents ---
        private void usT_CloseButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void usT_MaximizeButton1_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void usT_MinimizeButton1_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Minimized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void panel_Top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                sPoint = new Point(e.X, e.Y);
            }
        }

        private void panel_Top_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Left += e.X - sPoint.X;
                Top += e.Y - sPoint.Y;
            }
        }

        private void panel_Right_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{
            //    sPoint = new Point(e.X, e.Y);
            //}
        }

        private void panel_Right_MouseMove(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{
            //    Width += e.X - sPoint.X;
            //}
        }

        private void panel_Bottom_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                sPoint = new Point(e.X, e.Y);
            }
        }

        private void panel_Bottom_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Height += e.Y - sPoint.Y;
            }
        }

        private void RefreshPanelsBackColor()
        {
            List<Control> controls = new List<Control>();
            GetControls(this, controls);
            foreach (Control control in controls)
            {
                try
                {
                    Panel panel = control as Panel;
                    if (panel.BackColor == Color.SteelBlue)
                    {
                        panel.BackColor = HeaderColor;
                    }
                }
                catch
                {

                }
            }
        }

        private void GetControls(Control control, List<Control> controls)
        {

            if (control.Controls.Count > 0)
            {
                foreach (Control ctrl in control.Controls)
                {
                    controls.Add(ctrl);
                    if (ctrl.Controls.Count > 0)
                    {
                        GetControls(ctrl, controls);
                    }
                }
            }
        }

        #endregion

        public void RefreshControls()
        {

        }

        public void UpdateHistory(TaskDepartment taskDepartment, LibraryDB.LibraryDB lb)
        {
            panel_main.Controls.Clear();
            panelsNum.Clear();
            Y = 5;
            HistoryLog historyLog = null;

            if (taskDepartment != null && taskDepartment.TaskDepartmentHistory != null)
            {
                historyLog = Newtonsoft.Json.JsonConvert.DeserializeObject<HistoryLog>(taskDepartment.TaskDepartmentHistory);
            }

            if (historyLog != null)
            {
                historyLog.spHistory.Reverse();
                foreach (var story in historyLog.spHistory)
                {
                    Panel panel = new Panel();
                    panel.Name = (num + 1).ToString();
                    panelsNum.Add((num + 1).ToString());
                    panel.BackColor = Color.White;
                    panel.Width = panel_main.Width - 10;
                    panel.BorderStyle = BorderStyle.FixedSingle;
                    panel.Location = new Point(5, Y);

                    Label label0 = new Label();
                    label0.Name = "label0";
                    label0.Text = story.Date;
                    label0.Width = label0.PreferredWidth;
                    label0.ForeColor = Color.Gray;
                    label0.TextAlign = ContentAlignment.TopRight;
                    label0.Location = new Point(panel.Width - label0.Width - 5, 5);
                    panel.Controls.Add(label0);

                    List<Label> labels = new List<Label>();

                    Label label1 = new Label();
                    label1.Name = "header";
                    label1.Text = "Изменениия внес:";
                    labels.Add(label1);

                    Label label2 = new Label();
                    label2.Name = "header";
                    label2.Text = "Статус:";
                    labels.Add(label2);

                    Label label3 = new Label();
                    label3.Name = "header";
                    label3.Text = "Комментарий:";
                    labels.Add(label3);

                    foreach (Label label in labels)
                    {
                        label.ForeColor = Color.Gray;
                        panel.Controls.Add(label);
                    }

                    List<Label> infolabels = new List<Label>();

                    Label label1_1 = new Label();
                    label1_1.Name = "discription";
                    User user = lb.Users.FirstOrDefault(x => x.UserAccount == story.User);
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
                    label2_1.Text = GetStatus(story.Info, lb);
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
                    panel_main.Controls.Add(panel);
                    panel.Paint += new PaintEventHandler(panel_Paint);
                    num += 1;
                }
            }
            else
            {
                Label label = new Label();
                label.Text = "<Пусто>";
                label.Width = panel_main.Width - 10;
                label.ForeColor = Color.Gray;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Location = new Point(panel_main.Width / 2 - label.Width / 2, panel_main.Height / 2 - label.Height / 2);
                panel_main.Controls.Add(label);
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

        private void panel_Paint(object sender, PaintEventArgs e)
        {

            int w = panel_main.Width;
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

                    if (!panel_main.AutoScroll && Y - 5 > panel_main.Height)
                    {
                        panel_main.AutoScroll = true;
                        this.Width += 15;
                    }
                }
                catch
                {
                }
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

        /// <summary>
        /// Apply
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
        /// <summary>
        /// Cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
