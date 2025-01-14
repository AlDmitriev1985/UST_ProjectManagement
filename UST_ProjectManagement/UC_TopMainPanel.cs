using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UILib = UST_UILibrary.UILibrary;


namespace UST_ProjectManagement
{
    public partial class UC_TopMainPanel : UserControl
    {
        public delegate void HomeButton_Click(byte mode);
        public event HomeButton_Click OpenHomePanel;

        public delegate void ProjectButton_Click();
        public event ProjectButton_Click OpenProjectPanel;

        public delegate void SearchButton_Click();
        public event SearchButton_Click OpenSearchPanel;

        public delegate void AdminButton_Click();
        public event AdminButton_Click OpenAdminPanel;

        public delegate void NavisButton_Click();
        public event NavisButton_Click OpenNavisPanel;

        public delegate void UpdateButton_Click();
        public event UpdateButton_Click UpdateForm;

        private List<Button> btnList = new List<Button>();
        Color btnColor = Color.DimGray;

        ToolTip tool = new ToolTip();

        public UC_TopMainPanel()
        {
            InitializeComponent();
        }

        private void UC_TopMainPanel_Load(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(buttonHome, "Перейти на стартовую страницу");
            TT.SetToolTip(buttonSearch, "Перейти на панель поиска");
            TT.SetToolTip(buttonProject, "Перейти в карточку проекта");
            TT.SetToolTip(buttonAdminPanel, "Перейти на панель администратора");

            UpdateUserControlsVisible();

            
            //btnList.Add(buttonHome);
            //btnList.Add(buttonSearch);
            //btnList.Add(buttonProject);
            //btnList.Add(buttonAdminPanel);
            //btnList.Add(buttonAdminNavis);

            //foreach (Button btn in btnList)
            //{
            //    btn.Height = 79; 
            //    btn.Width = 59;
            //    btn.BackColor = btnColor;
            //    btn.FlatAppearance.MouseOverBackColor = MainForm.HeaderColor;
            //    btn.FlatAppearance.MouseDownBackColor = MainForm.HeaderColor;
            //}

            foreach(Control ctrl in flowLayoutPanel1.Controls)
            {
                try
                {
                    Button btn = ctrl as Button;
                    btnList.Add(btn);
                    btn.Height = 79;
                    btn.Width = 59;
                    btn.BackColor = btnColor;
                    btn.FlatAppearance.MouseOverBackColor = MainForm.HeaderColor;
                    btn.FlatAppearance.MouseDownBackColor = MainForm.HeaderColor;
                }
                catch
                {

                }
            }
            
        }

        /// <summary>
        /// Вызов стартовой страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonHome_Click(object sender, EventArgs e)
        {

            GlobalData.OpenPanelIndex = Convert.ToByte((sender as Button).TabIndex);
            UpdateMainBtnColor();
            OpenHomePanel?.Invoke(0);
        }

        /// <summary>
        /// Вызов панели проекта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonProject_Click(object sender, EventArgs e)
        {
            if (GlobalData.SelectedProject != null)
            {
                GlobalData.OpenPanelIndex = Convert.ToByte((sender as Button).TabIndex);
                UpdateMainBtnColor();
                OpenProjectPanel?.Invoke();
            }
            else
            {
                MessageBox.Show("Картачка проекта не может быть открыта, так как не выбран проект.\n\n" +
                                "Выберите проект", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        /// <summary>
        /// Вызов поисковой панели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            GlobalData.OpenPanelIndex = Convert.ToByte((sender as Button).TabIndex);
            UpdateMainBtnColor();
            OpenSearchPanel?.Invoke();
        }
        /// <summary>
        /// Вызов панели администратора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAdminPanel_Click(object sender, EventArgs e)
        {
            GlobalData.OpenPanelIndex = Convert.ToByte((sender as Button).TabIndex);
            UpdateMainBtnColor();
            GlobalData.SelectedCountry = null;
            GlobalData.SelectedProject = null;
            GlobalData.SelectedStage = null;
            GlobalData.SelectedPosition = null;
            GlobalData.SelectedProductCatalog = null;
            GlobalData.SelectedProduct = null;
            OpenAdminPanel?.Invoke();
        }

        private void buttonAdminNavis_Click(object sender, EventArgs e)
        {
            GlobalData.OpenPanelIndex = Convert.ToByte((sender as Button).TabIndex);
            UpdateMainBtnColor();
            OpenNavisPanel?.Invoke();
        }

        private void UpdateUserControlsVisible()
        {
            if (GlobalData.UserRole == "Admin")
            {
                buttonAdminPanel.Visible = true;
                buttonAdminNavis.Visible = true;
            }
            else
            {
                buttonAdminPanel.Visible = false;
                buttonAdminNavis.Visible = false;
            }
        }

        /// <summary>
        /// Обновление цвета кнопки
        /// </summary>
        public void UpdateMainBtnColor()
        {
            foreach(Button btn in btnList)
            {
                if (Convert.ToByte(btn.TabIndex) == GlobalData.OpenPanelIndex)
                {
                    btn.BackColor = MainForm.HeaderColor;
                }
                else
                {
                    btn.BackColor = btnColor;
                }
            }
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            UpdateForm?.Invoke();
        }  

        private void button_Update_MouseEnter(object sender, EventArgs e)
        {

            tool.SetToolTip(button_Update, $"Обновить все\n\n" +
                $"Последнее обновление\n" +
                $"выполнено в {MainForm.lastUpdate.TimeOfDay.Hours}:{MainForm.lastUpdate.TimeOfDay.Minutes}");
        }
    }
}
