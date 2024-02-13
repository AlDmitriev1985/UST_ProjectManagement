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
    public partial class UC_CalendarPanel : UserControl
    {
        public delegate void ButtonApply_Click();
        public event ButtonApply_Click ApplyDate;

        public string Date;
        public DateTime SelectedDate;

        public UC_CalendarPanel()
        {
            InitializeComponent();
            
            tableLayoutPanel1.BackColor = MainForm.HeaderColor;
            


            if (SelectedDate != null)
            {
                //monthCalendar1.SelectionStart = new DateTime(2021, 12, 10);
                //monthCalendar1.SelectionEnd = new DateTime(2021, 12, 10);
            }
            
        }

        public void buttonApply_Click(object sender, EventArgs e)
        {          
            string[] date = monthCalendar1.SelectionRange.Start.Date.ToString().Split(' ');
            Date = date[0];
            ApplyDate?.Invoke();
        }

        public void UpdatePanelSize()
        {
            this.Size = new Size(178, 202);
        }
        
        /// <summary>
        /// Выбор даты по щелчку мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            if (monthCalendar1.SelectionRange.Start.Date != null)
            {
                buttonApply_Click(this.buttonApply, EventArgs.Empty);
            }
        }
    }
}
 