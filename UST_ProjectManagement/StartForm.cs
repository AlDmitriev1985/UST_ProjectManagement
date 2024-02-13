using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UST_ProjectManagement
{
    public partial class StartForm : Form
    {
        public delegate void fClose();
        public event fClose _Close;

        public StartForm()
        {
            InitializeComponent();
            Shown += new EventHandler(StartForm_Shown);
            label1.BackColor = Color.FromArgb(24, 144, 167);
        }

        private void StartForm_Shown(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate (object unused)
            {
                while(GlobalData.CloseStart != true)
                {
                    UpdateLoadInfo(GlobalData.loadInfo);
                    Thread.Sleep(50);
                }
                CloseForm();
                this.DialogResult = DialogResult.OK;
                _Close?.Invoke();
            }));

        }

        public void UpdateLoadInfo(string txt)
        {
            if (InvokeRequired)
                Invoke(new Action<string>(UpdateLoadInfo), txt);
            else
                label1.Text = txt;         
        }

        public void CloseForm()
        {
            if (InvokeRequired) Invoke(new Action(CloseForm));
            else
            {
                this.Close();
            }
        }
    }
}
