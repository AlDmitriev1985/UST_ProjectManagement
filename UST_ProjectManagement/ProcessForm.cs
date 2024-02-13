using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UILib = UST_UILibrary.UILibrary;

namespace UST_ProjectManagement
{
    public partial class ProcessForm : Form
    {
        List<Control> picList = new List<Control>();

        public delegate void fClose();
        public event fClose _Close;

        public ProcessForm()
        {
            try
            {
                InitializeComponent();
                this.TopMost = true;
                this.Location = new Point(Convert.ToInt32(GlobalData.X + GlobalData.Width / 2 - this.Width / 2),
                                          Convert.ToInt32(GlobalData.Y + GlobalData.Height / 2 - this.Height / 2));

                try
                {
                    Shown += new EventHandler(StartForm_Shown);
                }
                catch
                {
                }
            }
            catch 
            {

            }
        }

        private void StartForm_Shown(object sender, EventArgs e)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(delegate (object unused)
        {
            int i = 1;
            while (!GlobalMethodes._stop)
            {
                UpdateLoadInfo(i);
                Thread.Sleep(500);
                if (i < 6) i += 1;
                else i = 1;
            }

            CloseForm();
            this.DialogResult = DialogResult.OK;
            _Close?.Invoke();
        }));
            }
            catch
            {
            }
        }

        public void UpdateLoadInfo(int index)
        {
            try
            {
                if (InvokeRequired)
                    Invoke(new Action<int>(UpdateLoadInfo), index);
                else
                {
                    foreach (PictureBox _pic in picList)
                    {
                        if (_pic.Tag.ToString() == index.ToString())
                        {
                            _pic.Image = Properties.Resources.UnyCar_Blue_50c50 as Bitmap;
                        }
                        else
                        {
                            _pic.Image = Properties.Resources.Track_50x50 as Bitmap;
                        }
                    }
                    label1.Text = GlobalData.loadInfo;
                }
            }
            catch
            {

            }
        }

        public void CloseForm()
        {
            try
            {
                if (InvokeRequired) Invoke(new Action(CloseForm));
                else
                {
                    this.Close();
                }
            }
            catch
            {
            }
        }

        private void ProcessForm_Load(object sender, EventArgs e)
        {
            picList.Add(pictureBox1);
            picList.Add(pictureBox2);
            picList.Add(pictureBox3);
            picList.Add(pictureBox4);
            picList.Add(pictureBox5);
            picList.Add(pictureBox6);
            picList = picList.OrderBy(t => t.Tag).ToList();
        }

        private void usT_CloseButton1_Click(object sender, EventArgs e)
        {
            try
            {
                CloseForm();
                _Close?.Invoke();
            }
            catch
            {
            }
        }
    }
}
