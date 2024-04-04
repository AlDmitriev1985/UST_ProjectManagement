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
    public partial class Form_MessageBox : Form
    {
        Point sPoint;
        string Header = "";
        byte Mode = 0;
        //static Hook
        globalKeyboardHook gkh = new globalKeyboardHook();



        public Form_MessageBox(string text, string headertext, byte mode)
        {
            InitializeComponent();
            RefreshPanelsBackColor();
            StartPosition = FormStartPosition.CenterParent;
            label2.Text = text;
            label1.Text = headertext;
            Mode = mode;
            switch (Mode)
            {
                case 0:
                    button1.Text = "OK";
                    button1.DialogResult = DialogResult.OK;
                    panel2.Visible = false;
                    button2.Visible = false;
                    break;
                case 1:
                    button1.Text = "Да";
                    button1.DialogResult = DialogResult.Yes;
                    button2.Text = "Нет";
                    button2.DialogResult = DialogResult.No;
                    break;
            }

        }
        private void Form_MessageBox_Load(object sender, EventArgs e)
        {
            gkh.HookedKeys.Add(Keys.Enter);
            gkh.HookedKeys.Add(Keys.Escape);
            gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            gkh.unhook();
        }

        void gkh_KeyUp(object sender, KeyEventArgs e)
        {            
            button1_Click(button1, EventArgs.Empty);
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
            //DialogResult = DialogResult.Cancel;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            int H = 0;
            Panel panel = sender as Panel;
            //Rectangle rect = e.ClipRectangle;
            H = panel.Height;
            Size titleSize = label2.Bounds.Size;

            if (panel.Height < titleSize.Height)
            {
                this.Height += titleSize.Height - panel.Height;
                H += titleSize.Height - panel.Height;
            }

            if (panel.Width < titleSize.Width + label2.Margin.Right + label2.Margin.Left)
            {
                this.Width += (titleSize.Width - panel.Width + label2.Margin.Right + label2.Margin.Left);
            }
            else if (panel.Width > titleSize.Width + label2.Margin.Right + label2.Margin.Left)
            {
                this.Width -= (panel.Width - (titleSize.Width + label2.Margin.Right + label2.Margin.Left));
            }

            label2.Location = new Point(label2.Location.X, (H - titleSize.Height) / 2);
            
        }

        private void panel4_SizeChanged(object sender, EventArgs e)
        {
            //int w = (panel4.Width - 1) / 2;
            //button1.Width = w;
            //button2.Width = w;
        }

        
    }
}
