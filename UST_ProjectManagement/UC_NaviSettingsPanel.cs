using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI_Library = UST_UILibrary.UILibrary;

namespace UST_ProjectManagement
{
    public partial class UC_NaviSettingsPanel : UserControl
    {
        public delegate void btnMain_Click();
        public event btnMain_Click mainPanel;

        public delegate void btnInfo_Click();
        public event btnInfo_Click infoPanel;

        public delegate void btnSchedule_Click();
        public event btnSchedule_Click schedulePanel;

        List<Control> ButtonsList = new List<Control>();

        public UC_NaviSettingsPanel()
        {
            InitializeComponent();
            ButtonsList.Add(button1);
            ButtonsList.Add(button3);
            ///UI_Library.GetAllTypedControls(tableLayoutPanel1, ButtonsList, typeof(Button));
        }
        /// <summary>
        /// Общее
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            
            SelectPressedButton(sender as Button);
            mainPanel?.Invoke();
        }
        /// <summary>
        /// Информация о проекте
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            infoPanel?.Invoke();
            SelectPressedButton(sender as Button);
        }
        /// <summary>
        /// Состав проекта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            schedulePanel?.Invoke();
            SelectPressedButton(sender as Button);
        }

        public void ButtonMainClick()
        {
            button1_Click(this.button1, EventArgs.Empty);
        }

        public void ButtonSetClick()
        {
            button3_Click(this.button3, EventArgs.Empty);
        }

        private void SelectPressedButton(Button selbtn)
        {
            
            foreach(Button btn in ButtonsList)
            {
                if (btn == selbtn)
                {
                    btn.BackColor = Color.Gainsboro;                //btn.BackColor = Color.LightSteelBlue;
                    
                }
                else
                {
                    btn.BackColor = Color.WhiteSmoke;
                }
            }
        
        }
    }
}
