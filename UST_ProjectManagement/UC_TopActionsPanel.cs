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
    public partial class UC_TopActionsPanel : UserControl
    {
        public delegate void ButtonStage_Click(byte mode);
        public event ButtonStage_Click CreateStagePanel;

        public delegate void ButtonPosition_Click(byte mode);
        public event ButtonPosition_Click CreatePositionPanel;

        public delegate void ButtonDiscipline_Click();
        public event ButtonDiscipline_Click CreateDisciplinePanel;

        public delegate void ButtonApprove_Click();
        public event ButtonApprove_Click ApproveSetList;

        public delegate void ButtonValeraStart_Click();
        public event ButtonValeraStart_Click ValeraStart;

        public delegate void ButtonValeraApprove();
        public event ButtonValeraApprove ValeraApprove;

        public delegate void ButtonNWFStart_Click();
        public event ButtonNWFStart_Click NWFStart;

        public delegate void ButtonCancelSetChanges();
        public event ButtonCancelSetChanges CancelSetChanges;
        public event ButtonCancelSetChanges EditSetsList;

        public delegate void ButtonNWFApprove();
        public event ButtonNWFApprove NWFApprove;

        List<Button> btnList = new List<Button>();

        PositionInfo positionInfo = null;
        ProductStageInfo productInfo = null;
        TechSulutionStageInfo solutionInfo = null;

        public UC_TopActionsPanel()
        {
            InitializeComponent();
            btnList.Add(button1);
            btnList.Add(button2);
            btnList.Add(button3);
            btnList.Add(button4);
            btnList.Add(button5);
            btnList.Add(button6);
        }

        private void UC_TopActionsPanel_Load(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button1, "Добавить стадию\n\n" +
                                   "Для активации, нужно\n" +
                                   "выбрать Проект.");
            TT.SetToolTip(button2, "Добавить позицию по ГП\n" +
                                   "или продукт\n\n" +
                                   "Для активации, нужно\n" +
                                   "выбрать Стадию или Каталог продуктов.");
            TT.SetToolTip(button3, "Добавить раздел для проекта\n" +
                                   "или продукта\n\n" +
                                   "Для активации, нужно\n" +
                                   "выбрать поз.по ГП. или продукт");
            TT.SetToolTip(button4, "Утвердить Состав проекта\n\n" +
                                   "Для активации, нужно\n" +
                                   "подготовить Состав проекта.");
            TT.SetToolTip(button5, "Создать ЦФХ\n\n" +
                                   "Запускается Revit и создаются\n" +
                                   " ЦФХ в соответствии с\n" +
                                   "составом проекта");
            TT.SetToolTip(button6, "Подтвердить создание ЦФХ\n\n" +
                                   "Фиксируется факт завершения\n" +
                                   "формирования проекта и\n" +
                                   "передачи его в работу.");

            UpdatePanelBubbtonsVisibleStatus();

            foreach (Button btn in btnList)
            {
                btn.Height = 59;
                btn.Width = 59;
                //btn.BackColor = btnColor;
                btn.FlatAppearance.MouseOverBackColor = MainForm.HeaderColor;
                btn.FlatAppearance.MouseDownBackColor = MainForm.HeaderColor;
            }
            tableLayoutPanel1.RowStyles[0].Height = 60;
            tableLayoutPanel1.ColumnStyles[0].Width = 180;
            tableLayoutPanel1.ColumnStyles[1].Width = 1;
            tableLayoutPanel1.ColumnStyles[2].Width = 180;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (GlobalData.CreateStage == true || GlobalData.SelectedProduct != null || GlobalData.SelectedTechSolution != null)
            {
                CreateStagePanel?.Invoke(0);
            }
            else
            {
                MessageBox.Show("Функция Добавление стадии доступна если \nв Диспетчере проектов выбран Проект, Продукт или Тех.решение.\n\nВыберите Проект, Продукт или Тех.решение.", 
                                "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void button2_Click(object sender, EventArgs e)
        {
            if (GlobalData.CreatePosition == true)
            {
                CreatePositionPanel?.Invoke(0); 
            }
            else if ((GlobalData.SelectedProductCatalog != null && GlobalData.SelectedProduct == null) 
                 || (GlobalData.SelectedTechSolutionCatalog != null && GlobalData.SelectedTechSolution == null))
            {
                CreatePositionPanel?.Invoke(0);
            }
            else
            {
                MessageBox.Show("Функция Добавление позиции по ГП доступна если \nв Диспетчере проектов выбрана Стадия.\n\nВыберите Стадию.",
                                "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        public void button4_Click(object sender, EventArgs e)
        {
            if (GlobalData.SelectedPosition != null)
            {
                if (GlobalData.addSubSetList.Count > 0)
                {
                    DialogResult result = MessageBox.Show("После утверждения Состава проекта,\n" +
                                                          "возможность удаления выбранных разделов\n" +
                                                          "будет отключена.\n\n" +
                                                          "Утвердить Состав проекта?", "Предупреждение",
                                                          MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        positionInfo.SetListeInRelease = false;
                        ApproveSetList?.Invoke();
                    }
                }
                else
                {
                    DialogResult message = MessageBox.Show("Функция Утверждение состава \n" +
                                    "проекта не доступна так как\n" +
                                    "состав проекта уже утвержден\n" +
                                    "или не выбрано ни одного раздела.",
                                    "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (message == DialogResult.Yes)
                    {
                        productInfo.SetListeInRelease = false;
                        CancelSetChanges?.Invoke();
                    }
                }
            }
            else if (GlobalData.SelectedProduct != null && GlobalData.SelectedStage != null)
            {
                if (GlobalData.addSubSetList.Count > 0)
                {
                    DialogResult result = MessageBox.Show("После утверждения Состава продукта,\n" +
                                                          "возможность удаления выбранных разделов\n" +
                                                          "будет отключена.\n\n" +
                                                          "Утвердить Состав продукта?", "Предупреждение",
                                                          MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        solutionInfo.SetListeInRelease = false;
                        ApproveSetList?.Invoke();
                    }
                }
                else
                {
                    DialogResult message = MessageBox.Show("Функция Утверждение состава \n" +
                                   "продукта не доступна так как\n" +
                                   "состав проекта уже утвержден\n" +
                                   "или не выбрано ни одного раздела.",
                                   "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (message == DialogResult.Yes)
                    {
                        CancelSetChanges?.Invoke();
                    }
                }
            }
            else if (GlobalData.SelectedTechSolution != null && GlobalData.SelectedStage != null)
            {
                if (GlobalData.addSubSetList.Count > 0)
                {
                    DialogResult result = MessageBox.Show("После утверждения Состава тех.решения,\n" +
                                                          "возможность удаления выбранных разделов\n" +
                                                          "будет отключена.\n\n" +
                                                          "Утвердить Состав тех.решения?", "Предупреждение",
                                                          MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        ApproveSetList?.Invoke();
                    }
                }
                else
                {
                    DialogResult message = MessageBox.Show("Функция Утверждение состава \n" +
                                    "тех.решения не доступна так как\n" +
                                    "состав проекта уже утвержден\n" +
                                    "или не выбрано ни одного раздела.",
                                    "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (message == DialogResult.Yes)
                    {
                        CancelSetChanges?.Invoke();
                    }
                }
            }
        }

        public void UpdateButtonsEnabled()
        {
            if (GlobalData.SelectedProject != null)
            {
                if (GlobalData.SelectedStage != null )
                {
                    GlobalData.CreateStage = false;
                    GlobalData.CreatePosition = true;
                    GlobalData.CreateSet = false;
                    GlobalData.ApproveSetList = false;

                    button1.BackColor = Color.Silver;
                    button2.BackColor = Color.Gray;
                    button3.BackColor = Color.Silver;
                    button4.BackColor = Color.Silver;

                    if (GlobalData.SelectedPosition != null)
                    {
                        GlobalData.CreateStage = false;
                        GlobalData.CreatePosition = false;

                        button1.BackColor = Color.Silver;
                        button2.BackColor = Color.Silver;
                        button3.BackColor = Color.Gray;
                        //if (GlobalData.SelectedPosition.SetListApproved == false || GlobalData.SelectedPosition.SetListeInRelease == true)
                        //{                           
                        //    GlobalData.CreateSet = true;
                        //    if (GlobalData.SelectedPosition.SetsList.Count>0)
                        //    {
                        //        button4.BackColor = Color.Gray;
                        //        GlobalData.ApproveSetList = true;
                        //    }
                        //    else
                        //    {
                        //        button4.BackColor = Color.Silver;
                        //        GlobalData.ApproveSetList = false;
                        //    }
                        //}
                        //else
                        //{
                        //    GlobalData.CreateSet = false;
                        //    GlobalData.ApproveSetList = false;

                        //    //button3.BackColor = Color.Silver;
                        //    button4.BackColor = Color.Silver;
                        //}
                    }
                    else
                    {
                        GlobalData.CreateStage = false;
                        GlobalData.CreatePosition = true;
                        GlobalData.CreateSet = false;
                        GlobalData.ApproveSetList = false;

                        button1.BackColor = Color.Silver;
                        button2.BackColor = Color.Gray;
                        button3.BackColor = Color.Silver;
                        button4.BackColor = Color.Silver;
                    }
                    
                }
                else
                {
                    GlobalData.CreateStage = true;
                    GlobalData.CreatePosition = false;
                    GlobalData.CreateSet = false;
                    GlobalData.ApproveSetList = false;

                    button1.BackColor = Color.Gray;
                    button2.BackColor = Color.Silver;
                    button3.BackColor = Color.Silver;
                    button4.BackColor = Color.Silver;
                }
                
            }
            else if (GlobalData.SelectedProductCatalog != null || GlobalData.SelectedTechSolutionCatalog != null)
            {
                button1.BackColor = Color.Silver;
                button2.BackColor = Color.Gray;
                button3.BackColor = Color.Silver;
                button4.BackColor = Color.Silver;
            }
            else if ((GlobalData.SelectedProduct != null || GlobalData.SelectedTechSolution != null) && GlobalData.SelectedStage == null)
            {
                button1.BackColor = Color.Gray;
                button2.BackColor = Color.Silver;
                button3.BackColor = Color.Silver;
                button4.BackColor = Color.Silver;
            }
            else if ((GlobalData.SelectedProduct != null || GlobalData.SelectedTechSolution != null) && GlobalData.SelectedStage != null)
            {
                GlobalData.CreateStage = true;
                GlobalData.CreatePosition = false;
                GlobalData.CreateSet = true;
                GlobalData.ApproveSetList = false;

                button1.BackColor = Color.Silver;
                button2.BackColor = Color.Silver;
                button3.BackColor = Color.Gray;
            }
            else
            {
                GlobalData.CreateStage = false;
                GlobalData.CreatePosition = false;
                GlobalData.CreateSet = false;
                GlobalData.ApproveSetList = false;

                button1.BackColor = Color.Silver;
                button2.BackColor = Color.Silver;
                button3.BackColor = Color.Silver;
                button4.BackColor = Color.Silver;
            }
        }

        public void btnApproveClick(PositionInfo _positionInfo, ProductStageInfo _productInfo, TechSulutionStageInfo _solutionInfo)
        {
            positionInfo = _positionInfo;
            productInfo = _productInfo;
            solutionInfo = _solutionInfo;
            button4_Click(this.button4, EventArgs.Empty);
        }

        /// <summary>
        /// Подтверждение факата развертывания Валеры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            if (GlobalData.OpenPanelIndex == 3)
            {
                ValeraApprove?.Invoke();
            }
            else if (GlobalData.OpenPanelIndex == 4 && button6.BackColor == Color.Gray)
            {
                NWFApprove?.Invoke();
            }
            
        }

       // public void btnEdotClick(byte mode, Dictionary<SectionsThree, string> dict)
       //{
       //     button3_Click(this.button3, EventArgs.Empty);
       //}


        /// <summary>
        /// Запуск Валеры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            if (GlobalData.OpenPanelIndex == 3)
            {
                ValeraStart?.Invoke(); 
            }
            else if (GlobalData.OpenPanelIndex == 4 && button5.BackColor == Color.Gray)
            {
                NWFStart?.Invoke();
            }
        }

        public void UpdatePanelBubbtonsVisibleStatus()
        {
            if (GlobalData.UserRole == "Admin" && (GlobalData.OpenPanelIndex == 3 || GlobalData.OpenPanelIndex == 4))
            {
                button5.Visible = true;
                button6.Visible = true;
                tableLayoutPanel1.ColumnStyles[2].Width = 180;
                label2.Text = "Утвердить";
            }
            else
            {
                button5.Visible = false;
                button6.Visible = false;
                tableLayoutPanel1.ColumnStyles[2].Width = 60;
                label2.Text = "Утв..";
            }
        }

        public void UpdatePanelBubbtonsEnabled(byte code)
        {
            if (code == 0)
            {
                button5.BackColor = Color.Silver;
                button6.BackColor = Color.Silver;
            }
            else
            {
                button5.BackColor = Color.Gray;
                button6.BackColor = Color.Gray;
            }
        }

        private List<ClassSet> DublicateSetList (List<ClassSet> dList)
        {
            List<ClassSet> result = new List<ClassSet>();
            foreach(ClassSet _set in dList)
            {
                if (!result.Contains(_set))
                result.Add(_set);
            }
            return result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EditSetsList?.Invoke();
        }
    }
}
