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


namespace UST_ProjectManagement
{
    public partial class EditPercentForm : Form
    {
        Point sPoint;

        UC_TaskPanel uC_TaskPanel = new UC_TaskPanel();
        UC_TaskInfo uC_TaskInfo = new UC_TaskInfo();

        public EditPercentForm(string title = null)
        {
            InitializeComponent();
            RequestInfo.requestInfoThree();
            //GetSelectedProject();

            foreach (Control ctrl in tableLayoutPanel1.Controls)
            {
                if (ctrl.GetType() == typeof(Panel))
                {
                    ctrl.BackColor = MainForm.HeaderColor;
                }
            }
            foreach (Control ctrl in tableLayoutPanel2.Controls)
            {
                if (ctrl.GetType() == typeof(Panel))
                {
                    ctrl.BackColor = MainForm.HeaderColor;
                }
            }

            //tableLayoutPanel2.Controls.Add(uC_TaskPanel, 1, 0);
            //tableLayoutPanel2.Controls.Add(uC_TaskInfo, 3, 0);
            ////Controls.Add(uC_TaskPanel);
            //uC_TaskPanel.Margin = new Padding(0);
            //uC_TaskPanel.Dock = DockStyle.Fill;
            //uC_TaskPanel.UpdateDG();
            //uC_TaskPanel.TaskOpen += openTask;
            //uC_TaskPanel.EditTask += editTask;
            //openTask(true, null, null, "..", "..", -1);

            //uC_TaskInfo.Margin = new Padding(0);
            //uC_TaskInfo.Dock = DockStyle.Fill;
            //uC_TaskInfo.OpenTaskRoute += openTaskRoute;
            //uC_TaskInfo.ChangeTaskStatus += changeTaskStatus;
        }



        //private void openTask(bool open, List<int> ids, List<int> tdids, string from, string to, int row)
        //{
        //    uC_TaskInfo.UpdateDG(ids, tdids, from, to);
        //}

        //private void editTask(byte mode)
        //{
        //    switch (mode)
        //    {
        //        case 0:
        //            uC_TaskInfo.button2_Click(uC_TaskInfo.button2, EventArgs.Empty);
        //            break;
        //        case 1:
        //            uC_TaskInfo.button4_Click(uC_TaskInfo.button4, EventArgs.Empty);
        //            break;
        //        case 2:
        //            uC_TaskInfo.button3_Click(uC_TaskInfo.button3, EventArgs.Empty);
        //            break;
        //    }
        //}

        //private void openTaskRoute(string txt, int statusId)
        //{
        //    uC_TaskPanel.UpdateTaskRoute(txt, statusId);
        //}

        //private void changeTaskStatus(string from, string to, int row)
        //{
        //    RequestInfo.requestInfoThree();
        //    uC_TaskPanel.UpdateDG(from, to, row);
        //    uC_TaskPanel.DG_SizeChanged(this.uC_TaskPanel, EventArgs.Empty);
        //}

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
            if (e.Button == MouseButtons.Left)
            {
                sPoint = new Point(e.X, e.Y);
            }
        }

        private void panel_Right_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Width += e.X - sPoint.X;
            }
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




        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
