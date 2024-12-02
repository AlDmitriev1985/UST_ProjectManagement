using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UST_ProjectManagement
{
    public partial class UC_Comments : UserControl
    {
        public delegate void Cancel_Click();
        public event Cancel_Click Cancel;

        public delegate void Apply_Click();
        public event Apply_Click Apply;

        public UC_Comments()
        {
            InitializeComponent();
            tableLayoutPanel1.RowStyles[0].Height = 25;
            tableLayoutPanel1.RowStyles[1].Height = 5;
            tableLayoutPanel1.RowStyles[tableLayoutPanel1.RowCount - 1].Height = 25;
            tableLayoutPanel2.ColumnStyles[0].Width = (tableLayoutPanel1.Width - 1) / 2;
            tableLayoutPanel2.ColumnStyles[1].Width = 1;
            tableLayoutPanel2.ColumnStyles[2].Width = (tableLayoutPanel1.Width - 1) / 2;
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                GlobalData.Comment = textBox1.Text;
                Apply?.Invoke();
            }
            else
            {
                MessageBox.Show("Не указана причина отказа в утверждении.", "Предуреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Cancel?.Invoke();
        }
    }
}
