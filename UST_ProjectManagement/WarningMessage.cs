using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Update
{
	public partial class WarningMessage : Form
	{
		public string Link { get; set; }
		public WarningMessage()
		{
			InitializeComponent();
			TopMost = true;	
			dataGridView1.RowHeadersVisible = false;
			dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dataGridView1.BackgroundColor = Color.WhiteSmoke;
			dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			dataGridView1.ReadOnly = true;
			dataGridView1.ScrollBars = ScrollBars.Vertical;
			dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dataGridView1.AllowUserToAddRows = false;
			dataGridView1.AllowUserToResizeRows = false;
			dataGridView1.AllowUserToResizeColumns = false;
			dataGridView1.ColumnHeadersHeight = 40;
			dataGridView1.DefaultCellStyle.SelectionBackColor = Color.SteelBlue;
		}		

		private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			TopMost = false;
			DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
			string link = row.Cells[3].Value.ToString();
			link = link.Replace("'", "|");
			var links = link.Split('|');
			if(links.Length > 1)
				link = links[1];
			link = links[0];

			System.Diagnostics.Process.Start(link);
		}

        private void WarningMessage_Paint(object sender, PaintEventArgs e)
        {
			tableLayoutPanel1.Location = new Point(4, 35);
			tableLayoutPanel1.Size = new Size(Width - 8, Height - 38);

		}

		private void button1_Click(object sender, EventArgs e)
		{
			//List<OverdueTask> spWarningOverdue = General.requestCheckOverdue();
			//dataGridView1.Rows.Clear();

			//if (spWarningOverdue.Count != 0)
			//{
			//	var oTask = spWarningOverdue.FirstOrDefault(x => x.UserId == x.ManagmentHeade);
			//	if (oTask != null)
			//	{
			//		dataGridView1.Columns[1].Visible = true;
			//	}

			//	ArrayList row = new ArrayList();
			//	foreach (OverdueTask item in spWarningOverdue)
			//	{
			//		try
			//		{
			//			row = new ArrayList();
			//			row.Add(item.OverdueTaskId);
			//			row.Add(item.UserSurname + " " + item.UserName + " " + item.UserMidlName);
			//			row.Add(item.OverdueTaskDays);
			//			row.Add(item.OverdueTaskLink);

			//			dataGridView1.Rows.Add(row.ToArray());
			//		}
			//		catch
			//		{

			//		}
			//	}
			//}
		}
	}
}
