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
    public partial class EditSetForm : Form
    {
        Point sPoint;
        //public PositionInfo positionInfo;
        //public ProductStageInfo productInfo;
        //public TechSulutionStageInfo solutionInfo;
        //Dictionary<SectionsPosition, SectionsThree> setDict;
        //public List<string> codes;
        public List<ScheduleItem> existingitems = new List<ScheduleItem>();
        public ScheduleItem selecteditem = null;
        List<Department> departments;
        List<Managment> managments;
        bool click = false;

        public EditSetForm()
        {
            InitializeComponent();
            RefreshPanelsBackColor();
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
            foreach(Control control in controls)
            {
                try
                {
                    Panel panel = control as Panel;
                    if(panel.BackColor == Color.SteelBlue)
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
                foreach(Control ctrl in control.Controls)
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
            textBox3.Text = selecteditem.PositionCode;
            textBox1.Text = selecteditem.SecThreeTag;
            textBox2.Text = selecteditem.SecThreePostfix;

            departments = RequestInfo.lb.Departments.Where(x => x.ManagmentId == 65 || x.ManagmentId == 66 || x.ManagmentId == 129).ToList();
            //var firstD = departments.FirstOrDefault(x => x.DepartmentName == "-");
            //if (firstD != null) comboBox1.Items.Add(firstD.DepartmentName);
            //List<string> depNames = departments.Select(x => x.DepartmentName).Where(z => z != "-").ToList();
            //comboBox1.Items.AddRange(depNames.ToArray());

            //try
            //{
            //    var dep = departments.FirstOrDefault(x => x.DepartmentId == selecteditem.DelegatedDepId);
            //    comboBox1.Text = dep.DepartmentName;
            //}
            //catch
            //{
            //    comboBox1.SelectedIndex = 0;
            //}


            managments = RequestInfo.lb.Managments.Where(x => x.ManagmentId == 65 || x.ManagmentId == 66 || x.ManagmentId == 129).ToList();
            List<string> manageNames = managments.OrderBy(n => n.ManagmentFullName).Select(x => x.ManagmentFullName).ToList();
            comboBox2.Items.Add("-");
            comboBox2.Items.AddRange(manageNames.ToArray());

            Department dep = departments.FirstOrDefault(x => x.DepartmentId == selecteditem.DelegatedDepId);
            Managment managment = null;
            try
            {
                managment = managments.FirstOrDefault(x => x.ManagmentId == dep.ManagmentId);
                comboBox2.Text = managment.ManagmentFullName;
            }
            catch
            {
                try
                {
                    comboBox2.SelectedIndex = 0;
                }
                catch
                {
                }
            }

            List<string> depNames = null;
            try
            {
                depNames = departments.Where(x => x.ManagmentId == managment.ManagmentId && x.DepartmentName != "-").Select(n => n.DepartmentName).ToList();
            }
            catch
            {
                depNames = departments.Where(x => x.DepartmentName != "-").Select(n => n.DepartmentName).ToList();
            }
            comboBox1.Items.Add("-");
            if(depNames != null && dep != null)
            {
                comboBox1.Items.AddRange(depNames.ToArray());
                comboBox1.Text = dep.DepartmentName;
            }
            else
            {
                try
                {
                    comboBox1.SelectedIndex = 0;
                }
                catch
                {

                }
            }
        }


        /// <summary>
        /// Apply
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() != selecteditem.SecThreePostfix)
            {
                ScheduleItem err = existingitems.FirstOrDefault(x => x.SecThreePostfix == textBox2.Text.Trim());
                if (err != null)
                {
                    MessageBox.Show("Раздел с таким постфиксом уже существует.\nУкажите другой постфикс.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    selecteditem.SecThreePostfix = textBox2.Text.Trim();
                }
            }
            Department department = departments.FirstOrDefault(x => x.DepartmentName == comboBox1.Text);
            if (department != null)
            {
                selecteditem.DelegatedDepId = department.DepartmentId;
            }
            else
            {
                selecteditem.DelegatedDepId = -1;
            }
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

        private void comboBox2_Click(object sender, EventArgs e)
        {
            click = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (click)
            {
                comboBox1.Items.Clear();
                comboBox1.Items.Add("-");
                List<string> depNames = null;
                if (comboBox2.SelectedIndex == 0)
                {
                    depNames = departments.Where(n => n.DepartmentName != "-").Select(n => n.DepartmentName).ToList();
                    
                }
                else
                {
                    Managment managment = managments.FirstOrDefault(x => x.ManagmentFullName == comboBox2.Text);
                    if (managment != null)
                    {
                        depNames = departments.Where(x => x.ManagmentId == managment.ManagmentId && x.DepartmentName != "-").Select(n => n.DepartmentName).ToList(); 
                    }
                }
                try
                {
                    comboBox1.Items.AddRange(depNames.ToArray());
                }
                catch 
                {
                }
                comboBox1.SelectedIndex = 0;
            }
            click = false;
        }
    }
}
