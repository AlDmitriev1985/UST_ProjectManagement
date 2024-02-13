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
    public partial class UC_HistoryPanel : UserControl
    {
        public delegate void OpenHistory_Click(bool open);
        public event OpenHistory_Click HistoryOpen;

        public UC_HistoryPanel()
        {
            InitializeComponent();
            tableLayoutPanel6.BackColor = MainForm.HeaderColor;
            UpdateHistory();
        }

        public void UpdateHistory()
        {
            listView1.Items.Clear();
            listView1.Groups.Clear();
            listView1.Columns[0].Width = 120;
            listView1.Columns[1].Width = MainForm.CreatePanelWidth - 120;
            if (GlobalData.historyLog != null && GlobalData.historyLog.spHistory != null && GlobalData.historyLog.spHistory.Count > 0)
            {
                foreach(var story in GlobalData.historyLog.spHistory)
                {
                    ListViewGroup LvGr = new ListViewGroup(story.Date);
                    LvGr.Name = story.Date;
                    listView1.Groups.AddRange(new ListViewGroup[] { LvGr });

                    ListViewItem lvi1 = new ListViewItem(new string[] { "Изменениия внес:", story.User}, LvGr);
                    listView1.Items.Add(lvi1);

                    ListViewItem lvi2 = new ListViewItem(new string[] { "Значение:", story.Info }, LvGr);
                    listView1.Items.Add(lvi2);
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
        }

        private void usT_CloseButton1_Click(object sender, EventArgs e)
        {
            HistoryOpen?.Invoke(false);
        }
    }
}
