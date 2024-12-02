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
    public partial class UC_ProjectNaviPanel : UserControl
    {
        public delegate void btnMain_Click();
        public event btnMain_Click mainPanel;

        public delegate void btnSchedule_Click(byte mode);
        public event btnSchedule_Click schedulePanel;

        public delegate void btnCoordination_Click();
        public event btnCoordination_Click coordinationPanel;

        public delegate void btnApprove_Click();
        public event btnApprove_Click approvepanel;

        public delegate void btnTask_Click();
        public event btnTask_Click taskpanel;

        List<Control> btnList = new List<Control>();
        int selBtn = 0;

        public UC_ProjectNaviPanel()
        {
            InitializeComponent();
            this.BackColor = MainForm.HeaderColor;
            flowLayoutPanel1.BackColor = MainForm.HeaderColor;
            btnList.Add(usT_HorizontalTabControl1);
            btnList.Add(usT_HorizontalTabControl2);
            btnList.Add(usT_HorizontalTabControl3);
            btnList.Add(usT_HorizontalTabControl4);
            btnList.Add(usT_HorizontalTabControl5);
        }

        private void UC_ProjectNaviPanel_Load(object sender, EventArgs e)
        {

        }

        public void usT_HorizontalTabControl1_Click(object sender, EventArgs e)
        {
            string btnText = "Общее";
            int btnWidth = 120;
            GlobalData.PrjNaviBtnIndex = 0;
            selBtn = 0;

            if (GlobalData.SelectedMainFolderName != null && GlobalData.SelectedMainFolderName != "")
            {
                btnText = GlobalData.SelectedMainFolderName;
            }
            else if (GlobalData.SelectedCountry != null)
            {
                btnText = GlobalData.SelectedCountry.NationName;
            }
            else if (GlobalData.SelectedProject != null || GlobalData.SelectedStage != null || GlobalData.SelectedPosition != null || GlobalData.SelectedTechSolution != null)
            {
                btnText = "";
                string[] stxt = GlobalData.SelectedDirPath.Split('\\');
                for(int i = 4; i < stxt.Length; i++)
                {
                    btnText += stxt[i];
                    if (i < stxt.Length - 2)
                    {
                        btnText += " \\";
                    }
                }
            }

            usT_HorizontalTabControl1.Text = btnText;
            if (btnText.Length * 8 > 220)
            {
                btnWidth = btnText.Length * 10;
            }
            else if (btnText.Length * 8 > 150)
            {
                btnWidth = btnText.Length * 12;
            }
            else if (btnText.Length * 8 > 100)
            {
                btnWidth = btnText.Length * 12;
            }
            usT_HorizontalTabControl1.Width = btnWidth;


            UST_HorizontalTabControl btn = sender as UST_HorizontalTabControl;
            UpdateButtonsStatus(btn);
            mainPanel?.Invoke();
            CangeBtnVisible();
        }

        public void usT_HorizontalTabControl2_Click(object sender, EventArgs e)
        {
            selBtn = 1;
            UST_HorizontalTabControl btn = sender as UST_HorizontalTabControl;
            UpdateButtonsStatus(btn);
            if(GlobalData.SelectedStage != null && GlobalData.SelectedPosition == null)
            {
                if (GlobalData.SelectedTechSolution != null || GlobalData.SelectedProduct != null)
                {
                    schedulePanel?.Invoke(0); 
                }
                else
                {
                    schedulePanel?.Invoke(1);
                }
            }
            else
            {
                schedulePanel?.Invoke(0);
            }
            
            GlobalData.PrjNaviBtnIndex = 1;
            
        }

        public void usT_HorizontalTabControl3_Click(object sender, EventArgs e)
        {
            selBtn = 2;
            UST_HorizontalTabControl btn = sender as UST_HorizontalTabControl;
            UpdateButtonsStatus(btn);
            coordinationPanel?.Invoke();
            GlobalData.PrjNaviBtnIndex = 2;
            
        }

        public void usT_HorizontalTabControl4_Click(object sender, EventArgs e)
        {
            selBtn = 3;
            UST_HorizontalTabControl btn = sender as UST_HorizontalTabControl;
            UpdateButtonsStatus(btn);
            approvepanel?.Invoke();
            GlobalData.PrjNaviBtnIndex = 3;
            
        }
        public void usT_HorizontalTabControl5_Click(object sender, EventArgs e)
        {
            selBtn = 4;
            UST_HorizontalTabControl btn = sender as UST_HorizontalTabControl;
            UpdateButtonsStatus(btn);
            taskpanel?.Invoke();
            GlobalData.PrjNaviBtnIndex = 4;
            
        }

        public void ButtonClick(int index)
        {
            //switch (index)
            //{
            //    case 0:
            //        usT_HorizontalTabControl1_Click(this.usT_HorizontalTabControl1, EventArgs.Empty);
            //        break;
            //    case 1:
            //        usT_HorizontalTabControl2_Click(this.usT_HorizontalTabControl2, EventArgs.Empty);
            //        break;
            //    case 2:
            //        usT_HorizontalTabControl3_Click(this.usT_HorizontalTabControl3, EventArgs.Empty);
            //        break;
            //    case 3:
            //        usT_HorizontalTabControl4_Click(this.usT_HorizontalTabControl4, EventArgs.Empty);
            //        break;
            //}
            usT_HorizontalTabControl1_Click(this.usT_HorizontalTabControl1, EventArgs.Empty);
            selBtn = index;

        }

        private void UpdateButtonsStatus(UST_HorizontalTabControl btn)
        {
            foreach (UST_HorizontalTabControl button in btnList)
            {
                if (button.TabIndex == btn.TabIndex)
                {
                    button.PressedStatus = true;
                    button.Invalidate();
                }
                else
                {
                    button.PressedStatus = false;
                    button.Invalidate();
                }
            }
        }

        public void CangeBtnVisible()
        {
            if (GlobalData.SelectedPosition != null 
                || (GlobalData.SelectedProduct != null && GlobalData.SelectedStage != null)
                || (GlobalData.SelectedTechSolution != null && GlobalData.SelectedStage != null))
            {
                usT_HorizontalTabControl2.Visible = true;
                
                if (GlobalData.SelectedPosition != null)
                {
                    usT_HorizontalTabControl3.Visible = true;
                    usT_HorizontalTabControl4.Visible = true;
                    usT_HorizontalTabControl5.Visible = true;
                }
                else
                {
                    usT_HorizontalTabControl3.Visible = false;
                    usT_HorizontalTabControl4.Visible = false;
                    usT_HorizontalTabControl5.Visible = false;
                }

            }
            else if (GlobalData.SelectedStage != null && GlobalData.SelectedPosition == null )
            {
                usT_HorizontalTabControl2.Visible = true;
                usT_HorizontalTabControl3.Visible = false;
                usT_HorizontalTabControl4.Visible = false;
                usT_HorizontalTabControl5.Visible = false;
            }
            else
            {
                usT_HorizontalTabControl2.Visible = false;
                usT_HorizontalTabControl3.Visible = false;
                usT_HorizontalTabControl4.Visible = false;
                usT_HorizontalTabControl5.Visible = false;
            }
            flowLayoutPanel1_SizeChanged(this, EventArgs.Empty);
        }

        public void BtnSetSchedule_Click()
        {
            usT_HorizontalTabControl2_Click(this.usT_HorizontalTabControl2, EventArgs.Empty);
        }

        private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            button_Left.Height = flowLayoutPanel1.Height;
            button_Left.Width = 15;
            button_Right.Height = flowLayoutPanel1.Height;
            button_Right.Width = 15;

            //if (btnList.Where(x => x.Visible).Count() > 2)
            //{
            //    button_Left.Visible = true;
            //    button_Right.Visible = true;
            //}
            //else
            //{
            //    button_Left.Visible = false;
            //    button_Right.Visible = false;
            //}

            int fi = 0;
            int li = 0;
            int btnsLength = GetVisibleBtnLength(out fi, out li);
            if (btnsLength < flowLayoutPanel1.Width)
            {
                button_Left.Visible = false;
                button_Right.Visible = false;

                if (GlobalData.SelectedPosition != null)
                {
                    foreach (var btn in btnList)
                    {
                        btn.Visible = true;
                    } 
                }
            }
            else
            {
                if (GlobalData.SelectedPosition != null)
                {
                    button_Left.Visible = true;
                    button_Right.Visible = true;
                    btnList[0].Visible = false;
                    btnList[1].Visible = false;
                    RefreshBtnVisible(selBtn, fi, li, 0);
                }
            }
        }

        private int GetVisibleBtnLength(out int firstIndex, out int lastindex)
        {
            int length = 0;
            firstIndex = btnList.Count-1;
            lastindex = 0;
            int index = 0;
            if (button_Left.Visible) length += 30;
            foreach(UST_HorizontalTabControl btn in btnList)
            {
                if (btn.Visible)
                {
                    length += btn.Width;
                    if (index < firstIndex) firstIndex = index;
                    if (index > lastindex) lastindex = index;
                }
                index += 1;
            }
            return length;
        }

        private void RefreshBtnVisible(int index, int firstIndex, int lastIndex, int rep)
        {
            if (!MainForm.minimized && rep < 10)
            {
                try
                {
                    for (int i = 0; i < btnList.Count; i++)
                    {
                        if (i < firstIndex) btnList[i].Visible = false;
                        else if (i > lastIndex) btnList[i].Visible = false;
                        else
                        {
                            try
                            {
                                btnList[i].Visible = true;
                            }
                            catch
                            {

                            }

                        }

                    }
                    int f = 0;
                    int l = 0;
                    if (GetVisibleBtnLength(out f, out l) > flowLayoutPanel1.Width)
                    {
                        if (index < lastIndex) lastIndex -= 1;
                        else if (index > firstIndex) firstIndex += 1;
                        rep += 1;
                        RefreshBtnVisible(index, firstIndex, lastIndex, rep);
                    }
                }
                catch
                {

                }
            }
        }

        private void button_Left_Click(object sender, EventArgs e)
        {
            int fIndex = 0;
            int lIndex = 0;
            GetVisibleBtnLength(out fIndex, out lIndex);
            if(fIndex > 0)
            {
                fIndex -= 1;
                lIndex -= 1;
                RefreshBtnVisible(selBtn, fIndex, lIndex, 0);
            }
        }

        private void button_Right_Click(object sender, EventArgs e)
        {
            int fIndex = 0;
            int lIndex = 0;
            GetVisibleBtnLength(out fIndex, out lIndex);
            if (lIndex < btnList.Count - 1)
            {
                fIndex += 1;
                lIndex += 1;
                RefreshBtnVisible(selBtn, fIndex, lIndex, 0);
            }
        }
    }
}
