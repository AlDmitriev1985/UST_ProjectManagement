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


namespace UST_ProjectManagement
{
    public partial class Form_AddTask : Form
    {
        Point sPoint;
        Project Project = null;
        Stage Stage = null;
        public Position Position = null;
        public PositionInfo PositionInfo = null;

        public Form_AddTask()
        {
            InitializeComponent();
            RefreshPanelsBackColor();

            foreach (Control item in panel1.Controls)
            {
                if(item.GetType() == typeof(ComboBox))
                {
                    (item as ComboBox).MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
                }

            }
        }

        #region --- FormEvents ---
        private void usT_CloseButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
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
                        panel.BackColor = MainForm.HeaderColor;
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

        private void comboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            var pStages = RequestInfo.lb.StageProjects.Where(x => x.ProjectId == comboBox2.Text).OrderBy(x => x.StageId);
            foreach(var pstage in pStages)
            {
                var Stage = RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == pstage.StageId);
                if (Stage != null)
                {
                    comboBox3.Items.Add(Stage.StageTag);
                }
            }
            if(comboBox3.Items.Count == 0)
            {
                comboBox3.Items.Add("<none>");
            }
            comboBox3.SelectedIndex = 0;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();
            try
            {
                Stage = RequestInfo.lb.Stages.FirstOrDefault(x => x.StageTag == comboBox3.Text);
                comboBox4.Items.AddRange(RequestInfo.lb.Positions.Where(x => x.ProjectId == comboBox2.Text && x.StageId == Stage.StageId).OrderBy(x => x.PositionCode).Select(x => x.PositionCode).ToArray());
            }
            catch
            {
                comboBox4.Items.Add("<none>");
            }
            comboBox4.SelectedIndex = 0;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox5.Items.Clear();
            Project = RequestInfo.lb.Projects.FirstOrDefault(x => x.ProjectId == comboBox2.Text);
            Position = RequestInfo.lb.Positions.FirstOrDefault(x => x.PositionCode == comboBox4.Text && x.StageId == Stage.StageId);
            if (Project != null && Stage != null && Position != null)
            {
                PositionInfo = new PositionInfo(Project, Stage, Position);
                if (Position != null)
                {
                    comboBox5.Items.AddRange(PositionInfo.scheduleItems.OrderBy(x => x.SecThreeTag).Select(x => x.SecThreeTag + x.SecThreePostfix).ToArray());
                    try
                    {
                        comboBox5.Items.Remove(comboBox1.Text);
                    }
                    catch { }
                }
                else
                {
                    comboBox5.Items.Add("<none>");
                }
            }
            else
            {
                comboBox5.Items.Add("<none>");
            }
            try
            {
                comboBox5.SelectedIndex = 0;
            }
            catch { }
        }
    }
}
