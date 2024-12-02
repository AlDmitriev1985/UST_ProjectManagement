using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryDB.DB;

namespace UST_ProjectManagement
{
    public partial class UC_HistoryPanel : UserControl
    {
        public delegate void OpenHistory_Click(bool open, POSTServer.History.HistoryLog history);
        public event OpenHistory_Click HistoryOpen;

        public UC_HistoryPanel()
        {
            InitializeComponent();
            tableLayoutPanel6.BackColor = MainForm.HeaderColor;
            //UpdateHistory(null);
        }

        public void UpdateHistory_Old(POSTServer.History.HistoryLog history)
        {
            listView1.Items.Clear();
            listView1.Groups.Clear();
            listView1.Columns[0].Width = 120;
            listView1.Columns[1].Width = MainForm.CreatePanelWidth - 120;
            if (history != null && history.spHistory.Count > 0)
            {
                for(int i = history.spHistory.Count - 1; i >= 0; i-- )
                {
                    ListViewGroup LvGr = new ListViewGroup(history.spHistory[i].Date);
                    LvGr.Name = history.spHistory[i].Date;
                    listView1.Groups.AddRange(new ListViewGroup[] { LvGr });

                    ListViewItem lvi1 = new ListViewItem(new string[] { "Изменениия внес:", history.spHistory[i].User}, LvGr);
                    listView1.Items.Add(lvi1);

                    string txt = "";
                    if (history.spHistory[i].Info.Contains("Статус"))
                    {
                        string[] sp = history.spHistory[i].Info.Split(':');
                        try
                        {
                            int id = Convert.ToInt32(sp[1].Trim());
                            txt = RequestInfo.lb.Status.FirstOrDefault(x => x.StatusId == id).StatusName;
                        }
                        catch
                        {
                            txt = history.spHistory[i].Info;
                        }
                    }
                    else
                    {
                        txt = history.spHistory[i].Info;
                    }

                    ListViewItem lvi2 = new ListViewItem(new string[] { "Значение:", txt }, LvGr);
                    listView1.Items.Add(lvi2);

                    if (history.spHistory[i].Description != null && history.spHistory[i].Description != "")
                    {
                        ListViewItem lvi3 = new ListViewItem(new string[] { "Комментарий:", history.spHistory[i].Description }, LvGr);
                        listView1.Items.Add(lvi3); 
                    }
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
            HistoryOpen?.Invoke(false, null);
        }
        List<string> panelsNum = new List<string>();
        int num = 0;
        int Y = 5;
        int X = 5;

        public void UpdateHistory(POSTServer.History.HistoryLog history)
        {
            panel1.Controls.Clear();
            panelsNum.Clear();
            Y = 5;
            if (history != null)
            {
                history.spHistory.Reverse();
                foreach (var story in history.spHistory)
                {
                    Panel panel = new Panel();
                    panel.Name = (num + 1).ToString();
                    panelsNum.Add((num + 1).ToString());
                    panel.BackColor = Color.White;
                    panel.Width = panel1.Width - 10;
                    panel.BorderStyle = BorderStyle.FixedSingle;
                    panel.Location = new Point(5, Y);

                    Label label0 = new Label();
                    label0.Name = "label0";
                    label0.Text = story.Date;
                    label0.Width = label0.PreferredWidth;
                    label0.ForeColor = Color.Gray;
                    label0.TextAlign = ContentAlignment.TopRight;
                    label0.Location = new Point(panel.Width - label0.Width - 15, 5);
                    panel.Controls.Add(label0);

                    List<Label> labels = new List<Label>();

                    Label label1 = new Label();
                    label1.Name = "header";
                    label1.Text = "Изм. внес:";
                    labels.Add(label1);

                    Label label2 = new Label();
                    label2.Name = "header";
                    label2.Text = "Статус:";
                    labels.Add(label2);

                    Label label3 = new Label();
                    label3.Name = "header";
                    label3.Text = "Коммент.:";
                    labels.Add(label3);

                    foreach (Label label in labels)
                    {
                        label.ForeColor = Color.Gray;
                        panel.Controls.Add(label);
                    }

                    List<Label> infolabels = new List<Label>();

                    Label label1_1 = new Label();
                    label1_1.Name = "discription";
                    User user = RequestInfo.lb.Users.FirstOrDefault(x => x.UserAccount == story.User);
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
                    label2_1.Text = GetStatus(story.Info, RequestInfo.lb);
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
                    panel1.Controls.Add(panel);
                    panel.Paint += new PaintEventHandler(panel_Paint);
                    num += 1;
                }
            }
            else
            {
                Label label = new Label();
                label.Text = "<Пусто>";
                label.Width = panel1.Width - 10;
                label.ForeColor = Color.Gray;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Location = new Point(panel1.Width / 2 - label.Width / 2, panel1.Height / 2 - label.Height / 2);
                panel1.Controls.Add(label);
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

        private void panel_Paint(object sender, PaintEventArgs e)
        {

            int w = panel1.Width;
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

                    if (!panel1.AutoScroll && Y - 5 > panel1.Height)
                    {
                        panel1.AutoScroll = true;
                        this.Width += 15;
                    }
                }
                catch
                {
                }
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
    }
}
