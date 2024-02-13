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
    public partial class UC_SearchPanel : UserControl
    {
        public delegate void PrjectPanel_Click();
        public event PrjectPanel_Click OpenProjectPanel;

        public delegate void SelectObject_Click();
        public event SelectObject_Click SearchThreeNode;

        ToolStripMenuItem toolStripItem1 = new ToolStripMenuItem();
        private DataGridViewCellEventArgs mouseLocation;

        public string SelectedObjectShortPath = "";

        List<ClassSearchPanelRow> SearchRowList = new List<ClassSearchPanelRow>();
        List<ClassSearchPanelRow> SearchedList = new List<ClassSearchPanelRow>();
        List<TextBox> TextBoxList = new List<TextBox>();
        List<MaskedTextBox> MaskedTextBoxList = new List<MaskedTextBox>();

        bool StartCalendar = false;
        bool EndCalendar = false;
        bool ReliseCalendar = false;

        public UC_SearchPanel()
        {
            InitializeComponent();
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Red;
            AddContextMenu();
            uC_CalendarPanel1.ApplyDate += ApplyDate;
        }

        private void UC_SearchPanel_Load(object sender, EventArgs e)
        {
            panelColor.BackColor = MainForm.HeaderColor;
            TextBoxList.Add(textBox_pNumber);
            TextBoxList.Add(textBox_pName);
            TextBoxList.Add(textBox_pStage);
            TextBoxList.Add(textBox_pGIP);

            MaskedTextBoxList.Add(maskedTextBox_Start);
            MaskedTextBoxList.Add(maskedTextBox_End);
            MaskedTextBoxList.Add(maskedTextBox_Approve);
        }

        private void UC_SearchPanel_SizeChanged(object sender, EventArgs e)
        {
            tableLayoutPanel_Search.ColumnStyles[0].Width = 130;
            //tableLayoutPanel9.ColumnStyles[2].Width = tableLayoutPanel9.Width - 850;
            tableLayoutPanel_Search.ColumnStyles[4].Width = 100;
            tableLayoutPanel_Search.ColumnStyles[6].Width = 130;
            tableLayoutPanel_Search.ColumnStyles[8].Width = 130;
            tableLayoutPanel_Search.ColumnStyles[10].Width = 130;
            tableLayoutPanel_Search.ColumnStyles[12].Width = 185;
            tableLayoutPanel_Search.ColumnStyles[14].Width = 50;
            tableLayoutPanel_Search.ColumnStyles[16].Width = 50;

            if (StartCalendar) GetCalendarLocation(1);
            if (EndCalendar) GetCalendarLocation(2);
            if (ReliseCalendar) GetCalendarLocation(3);
        }

        private void dataGridView1_SizeChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns[0].Width = 130;
            dataGridView1.Columns[1].Width = Convert.ToInt32((dataGridView1.Size.Width - 530) * 0.75);
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 100;
            dataGridView1.Columns[5].Width = 100;
            dataGridView1.Columns[6].Width = Convert.ToInt32((dataGridView1.Size.Width - 530) * 0.25 - 2);
        }

        /// <summary>
        /// Добавть контекстное меню
        /// </summary>
        private void AddContextMenu()
        {
            toolStripItem1.Text = "Открыть проект";
            toolStripItem1.Click += new EventHandler(toolStripItem1_Click);
            ContextMenuStrip strip = new ContextMenuStrip();
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {

                column.ContextMenuStrip = strip;
                column.ContextMenuStrip.Items.Add(toolStripItem1);
            }
        }

        /// <summary>
        /// Выбор проекта по двойному щелчку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            toolStripItem1_Click(this, EventArgs.Empty);
        }

        /// <summary>
        /// Перейти в проект
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void toolStripItem1_Click(object sender, EventArgs args)
        {
            int rIndex = mouseLocation.RowIndex;
            GlobalData.SelectedProject = RequestInfo.lb.Projects.Find(id => id.ProjectId == dataGridView1.Rows[rIndex].Cells[0].Value.ToString());
            List<ClassPosition> pList = GlobalData.PositionList.Where(id => id.ID == dataGridView1.Rows[rIndex].Cells[0].Value.ToString()).ToList();
            //List<ClassPosition> pList = GlobalData.PositionList.Where(id => id.ID == dataGridView1.Rows[rIndex].Cells[0].Value.ToString()).Where(s => s.ParantStageTag == dataGridView1.Rows[rIndex].Cells[2].Value.ToString()).ToList();
            //if (pList.Count > 0) GlobalData.SelectedPosition = pList.Find(s => s.ParantStageTag == dataGridView1.Rows[rIndex].Cells[2].Value.ToString());

            //GlobalMethodes.SearchSelectedProject(dataGridView1.Rows[rIndex].Cells[0].Value.ToString());
 
            //GlobalMethodes.SearchSelectedPosition(dataGridView1.Rows[rIndex].Cells[0].Value.ToString());

            //UC_NavigationPanel.SelectTreeNode(GlobalData.SelectedProject.prjShortDirPath);
            OpenProjectPanel?.Invoke();

            //if (GlobalData.SelectedPosition == null)
            //{
            //    SelectedObjectShortPath = RequestInfo.lb.prjShortDirPath;
            //}
            //else
            //{
            //    SelectedObjectShortPath = GlobalData.SelectedPosition.ShortPath;
            //}

            foreach (ClassFolder _folder in GlobalData.NaviTreeView.FullFoldersList)
            {
                if (_folder.ShortPath == SelectedObjectShortPath)
                {
                    GlobalData.CreatedFolder = _folder;
                    break;
                }
            }

            SearchThreeNode?.Invoke();
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs location)
        {
            mouseLocation = location;
            ///MessageBox.Show(mouseLocation.ToString());
        }

        
        /// <summary>
        /// Обновлене списка проектов
        /// </summary>
        public void UpdateSearchView()
        {
            dataGridView1.Rows.Clear();
            SearchRowList.Clear();
            //MessageBox.Show(GlobalData.ProjectList.Count.ToString());
            foreach (ClassProject prj in GlobalData.ProjectList)
            {
                string pNumber;
                string pShortName;
                string pStage;
                string pStartDate;
                string pEndDate;
                string pRelDate;
                string pRespons;

                if (prj.prjID != null)
                {
                    pNumber = prj.prjID;
                }
                else
                {
                    pNumber = "";
                }

                if (prj.prjShortName != null)
                {
                    pShortName = prj.prjShortName;
                }
                else
                {
                    pShortName = "";
                }

                if (prj.prjStage != null)
                {
                    pStage = prj.prjStage;
                }
                else
                {
                    pStage = "";
                }

                if (prj.prjStartDate != null)
                {
                    pStartDate = prj.prjStartDate;
                }
                else
                {
                    pStartDate = "";
                }

                if (prj.prjEndDate != null)
                {
                    pEndDate = prj.prjEndDate;
                }
                else
                {
                    pEndDate = "";
                }

                if (prj.prjReleaseDate != null)
                {
                    pRelDate = prj.prjReleaseDate;
                }
                else
                {
                    pRelDate = "";
                }

                if (prj.prjResponsieble != null)
                {
                    pRespons = prj.prjResponsieble;
                }
                else
                {
                    pRespons = "";
                }

                ClassSearchPanelRow shRow = new ClassSearchPanelRow(pNumber, pStage);
                shRow.pShortName = pShortName;
                shRow.pStartDate = pStartDate;
                shRow.pEndDate = pEndDate;
                shRow.pRelDate = pRelDate;
                shRow.pRespons = pRespons;
                SearchRowList.Add(shRow);

            }

            foreach (ClassPosition pos in GlobalData.PositionList)
            {
                string pNumber;
                string pShortName;
                string pStage;
                string pStartDate;
                string pEndDate;
                string pRelDate ="";
                string pRespons;

                if (pos.ID != null)
                {
                    pNumber = pos.ID;
                }
                else
                {
                    pNumber = "";
                }

                if (pos.Name != null)
                {
                    pShortName = pos.Name;
                }
                else
                {
                    pShortName = "";
                }

                if (pos.ParantStageTag != null)
                {
                    pStage = pos.ParantStageTag;
                }
                else
                {
                    pStage = "";
                }

                if (pos.prjStartDate != null)
                {
                    pStartDate = pos.prjStartDate;
                }
                else
                {
                    pStartDate = "";
                }

                if (pos.prjEndDate != null)
                {
                    pEndDate = pos.prjEndDate;
                }
                else
                {
                    pEndDate = "";
                }

                //if (pos.Re != null)
                //{
                //    pRelDate = prj.prjReleaseDate;
                //}
                //else
                //{
                //    pRelDate = "";
                //}

                if (pos.prjGIP != null)
                {
                    pRespons = pos.prjGIP;
                }
                else
                {
                    pRespons = "";
                }

                ClassSearchPanelRow shRow = new ClassSearchPanelRow(pNumber, pStage);
                shRow.pShortName = pShortName;
                shRow.pStartDate = pStartDate;
                shRow.pEndDate = pEndDate;
                shRow.pRelDate = pRelDate;
                shRow.pRespons = pRespons;
                SearchRowList.Add(shRow);

            }


            List<ClassSearchPanelRow> SortList = SearchRowList.OrderBy(Name => Name.pNumber).ToList();
            SearchRowList = SortList;


            UpdateSerchListView(SearchRowList);
            UpdateAutoComplete();
        }

        /// <summary>
        /// Обновление DataGrid
        /// </summary>
        /// <param name="searchlist"></param>
        private void UpdateSerchListView (List<ClassSearchPanelRow> searchlist)
        {
            dataGridView1.Rows.Clear();
            foreach (ClassSearchPanelRow row in searchlist)
            {
                object[] tt = new object[7];
                tt[0] = row.pNumber;
                tt[1] = row.pShortName;
                tt[2] = row.pStage;
                tt[3] = row.pStartDate;
                tt[4] = row.pEndDate;
                tt[5] = row.pRelDate;
                tt[6] = row.pRespons;
                dataGridView1.Rows.Add(tt);
            }
        }

        /// <summary>
        /// Добавление контекстных подсказок
        /// </summary>
        private void UpdateAutoComplete()
        {

            AutoCompleteStringCollection _pNumber = new AutoCompleteStringCollection();
            AutoCompleteStringCollection _pName = new AutoCompleteStringCollection();
            AutoCompleteStringCollection _pStage = new AutoCompleteStringCollection();
            AutoCompleteStringCollection _pGIP = new AutoCompleteStringCollection();

            List<string> StageList = new List<string>();
            List<string> GIPList = new List<string>();

            foreach (ClassSearchPanelRow row in SearchRowList)
            {            
                _pNumber.Add(row.pNumber);
                _pName.Add(row.pShortName);
                if (!StageList.Contains(row.pStage))
                {
                    StageList.Add(row.pStage);
                }
                if (!GIPList.Contains(row.pRespons))
                {
                    GIPList.Add(row.pRespons);
                }
            }

            textBox_pNumber.AutoCompleteCustomSource = _pNumber;
            textBox_pNumber.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox_pNumber.AutoCompleteSource = AutoCompleteSource.CustomSource;

            textBox_pName.AutoCompleteCustomSource = _pName;
            textBox_pName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox_pName.AutoCompleteSource = AutoCompleteSource.CustomSource;

            foreach(string stage in StageList)
            {
                _pStage.Add(stage);
            }

            textBox_pStage.AutoCompleteCustomSource = _pStage;
            textBox_pStage.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox_pStage.AutoCompleteSource = AutoCompleteSource.CustomSource;

            foreach (string GIP in GIPList)
            {
                _pGIP.Add(GIP);
            }

            textBox_pGIP.AutoCompleteCustomSource = _pGIP;
            textBox_pGIP.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox_pGIP.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        /// <summary>
        /// Нажатие кнопки Очистить контрол
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClearTextBox_Click(object sender, EventArgs e)
        {
            int index = 100;
            Button btn = sender as Button;
            if (btn.Tag.ToString() != "")
            {
                index = Convert.ToInt32(btn.Tag);
            }
            ClearTextBox(index);
        }

        /// <summary>
        /// Очистить все
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ClearAll_Click(object sender, EventArgs e)
        {
            ClearAllControls();
            buttonSearch_Click(this.button11, EventArgs.Empty);
        }

        public void ClearAllControls()
        {
            foreach (TextBox tb in TextBoxList)
            {
                tb.Text = "";
            }
            foreach (MaskedTextBox mtb in MaskedTextBoxList)
            {
                mtb.Text = "";
            }
        }

        /// <summary>
        /// Очистить TextBox
        /// </summary>
        /// <param name="index"></param>
        private void ClearTextBox(int index)
        {
            foreach(TextBox tb in TextBoxList)
            {
                if (tb.Tag.ToString() != "")
                {
                    if (Convert.ToInt32(tb.Tag) == index)
                    {
                        tb.Text = "";
                        break;
                    }
                }
                
            }
        }

        /// <summary>
        /// Дата начала
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCalendar_Click(object sender, EventArgs e)
        {
            int index = 100;
            Button btn = sender as Button;
            if (btn.Tag.ToString() != "")
            {
                index = Convert.ToInt32(btn.Tag);
            }
            switch (index)
            {
                case 1:
                    if (StartCalendar == false)
                    {
                        StartCalendar = true;
                        EndCalendar = false;
                        ReliseCalendar = false;
                        uC_CalendarPanel1.UpdatePanelSize();
                        GetCalendarLocation(index);
                        uC_CalendarPanel1.BringToFront();
                    }
                    else
                    {
                        StartCalendar = false;
                        uC_CalendarPanel1.SendToBack();
                    }
                    break;
                case 2:
                    if (EndCalendar == false)
                    {
                        StartCalendar = false;
                        EndCalendar = true;
                        ReliseCalendar = false;
                        uC_CalendarPanel1.UpdatePanelSize();
                        GetCalendarLocation(index);
                        uC_CalendarPanel1.BringToFront();
                    }
                    else
                    {
                         EndCalendar = false;
                        uC_CalendarPanel1.SendToBack();
                    }
                    break;
                case 3:
                    if (ReliseCalendar == false)
                    {
                        StartCalendar = false;
                        EndCalendar = false;
                        ReliseCalendar = true;
                        uC_CalendarPanel1.UpdatePanelSize();
                        GetCalendarLocation(index);
                        uC_CalendarPanel1.BringToFront();
                    }
                    else
                    {
                        ReliseCalendar = false;
                        uC_CalendarPanel1.SendToBack();
                    }
                    break;
            }
            
        }

        /// <summary>
        /// Очистить фильтр по дате
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClearCalendar_Click(object sender, EventArgs e)
        {
            int index = 100;
            Button btn = sender as Button;
            if (btn.Tag.ToString() != "")
            {
                index = Convert.ToInt32(btn.Tag);
            }
            switch (index)
            {
                case 1:
                    maskedTextBox_Start.Text = "";
                    break;
                case 2:
                    maskedTextBox_End.Text = "";
                    break;
                case 3:
                    maskedTextBox_Approve.Text = "";
                    break;
            }
        }

        /// <summary>
        /// Обновление положения панели календаря
        /// </summary>
        /// <param name="index"></param>
        private void GetCalendarLocation(int index)
        {
            int X = 0;
            switch (index)
            {
                case 1:
                    X = Convert.ToInt32(tableLayoutPanel5.Location.X - 25);
                    break;
                case 2:
                    X = Convert.ToInt32(tableLayoutPanel6.Location.X - 25);
                    break;
                case 3:
                    X = Convert.ToInt32(tableLayoutPanel7.Location.X - 25);
                    break;
            }
            
            
            Point pp = new Point(X, -5);
            uC_CalendarPanel1.Location = pp;
        }

        /// <summary>
        /// Получение даты
        /// </summary>
        private void ApplyDate()
        {
            if (StartCalendar == true)
            {
                maskedTextBox_Start.Text = uC_CalendarPanel1.Date;
                StartCalendar = false;
            }
            if (EndCalendar == true)
            {
                maskedTextBox_End.Text = uC_CalendarPanel1.Date;
                EndCalendar = false;
            }
            if (ReliseCalendar == true)
            {
                maskedTextBox_Approve.Text = uC_CalendarPanel1.Date;
                ReliseCalendar = false;
            }
            uC_CalendarPanel1.SendToBack();
        }


        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (StartSearch() == true)
            {
                UpdateSerchListView(SearchedList);
            }           
        }

        /// <summary>
        /// Сбор фильтров
        /// </summary>
        /// <returns></returns>
        private bool StartSearch()
        {
            bool result = false;
            SearchedList.Clear();

            Dictionary<int, string> Filter = new Dictionary<int, string>();

            //List<string> SaercParamList = new List<string>();

            foreach (TextBox tb in TextBoxList)
            {
                if (tb.Text != "")
                {
                    //SaercParamList.Add(tb.Text);
                    result = true;
                }
                Filter.Add(Convert.ToInt32(tb.Tag), tb.Text);
            }

            foreach (MaskedTextBox mtb in MaskedTextBoxList)
            {
                if (mtb.Text != "  .  .")
                {
                    //SaercParamList.Add(mtb.Text);
                    Filter.Add(Convert.ToInt32(mtb.Tag), mtb.Text);
                    result = true;
                }
                else
                {
                    Filter.Add(Convert.ToInt32(mtb.Tag), "");
                }
               
            }

            UpdateSerchedList(Filter);
            //foreach (string filter in SaercParamList)
            //{
            //    UpdateSerchedList(filter);
            //}

            return result;
        }

        /// <summary>
        /// Поиск
        /// </summary>
        /// <param name="filter"></param>
        private void UpdateSerchedList(Dictionary<int, string> filter)
        {
            foreach (ClassSearchPanelRow row in SearchRowList)
            {
                if (row.pNumber.IndexOf(filter[0]) != -1 &&
                    row.pShortName.IndexOf(filter[1]) != -1 &&
                    row.pStage.IndexOf(filter[2]) != -1 &&
                    row.pStartDate.IndexOf(filter[3]) != -1 &&
                    row.pEndDate.IndexOf(filter[4]) != -1 &&
                    row.pRelDate.IndexOf(filter[5]) != -1 &&
                    row.pRespons.IndexOf(filter[6]) != -1)
                {
                    SearchedList.Add(row);
                }
            }
        }
    }
}
