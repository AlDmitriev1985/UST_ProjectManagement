using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UST_DataGridLibrary;
using LibraryDB.DB;

namespace UST_ProjectManagement
{
    public partial class UC_StageSchedulePanel : UserControl
    {
        DataGridView dataGrid;

        bool updateControls = true;
        int clickedIndex = -1;
        bool filterEromLib = true;
        int start_Width = 145;
        int start_X = 5;
        int start_Y = 7;

        private Dictionary<int, string> filters = new Dictionary<int, string>();
        private Dictionary<int, int> filterColumns = new Dictionary<int, int>()
        {
            {0, 8}, { 1, 5 }
        };
        private Dictionary<int, List<string>> filterItems = new Dictionary<int, List<string>>();

        public UC_StageSchedulePanel()
        {
            InitializeComponent();

            dataGrid = new DataGridView();
            dataGrid.Dock = DockStyle.Fill;
            dataGrid.BorderStyle = BorderStyle.None;
            dataGrid.Margin = new Padding(0);
            dataGrid.ScrollBars = ScrollBars.Vertical;
            dataGrid.BorderStyle = BorderStyle.None;
            dataGrid.BackgroundColor = Color.WhiteSmoke;
            dataGrid.SizeChanged += new EventHandler(DG_SizeChanged);
            tableLayoutPanel3.Controls.Add(dataGrid, 0, 1);
            List<string> header = new List<string>() { "Шифр", "Обозначение", "Наименование", "Разработал", "% готовности", "Статус","Дата", "Изменил", "Position" };
            Methodes.CreateDataGrid(dataGrid, header);
            for (int i = dataGrid.Columns.Count-1; i < dataGrid.Columns.Count; i++)
            {
                dataGrid.Columns[i].Visible = false;
            }
            dataGrid.MultiSelect = false;
            dataGrid.ColumnHeadersVisible = true;
            dataGrid.ColumnHeadersHeight = 50;
        }

        public void DG_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                int num = 0;
                for (int i = 0; i < dataGrid.Columns.Count; i++)
                {
                    if (dataGrid.Columns[i].Visible) num++;
                }
                int summ = 0;
                for (int i = 0; i < num; i++)
                {
                    if (i == 0 || i == 4  || i == 6 )
                    {
                        dataGrid.Columns[i].Width = 100;
                    }
                    else if (i == 1 || i == 3 || i == 7)
                    {
                        dataGrid.Columns[i].Width = 150;
                    }
                    else if (i == 5)
                    {
                        dataGrid.Columns[i].Width = 150;
                    }
                    else
                    {
                        dataGrid.Columns[i].Width = 0;
                    }
                    summ += dataGrid.Columns[i].Width;
                }
                dataGrid.Columns[2].Width = dataGrid.Width - summ;
            }
            catch
            {
            }
        }


        public void CreatePanels()
        {
            panel_Filters.Visible = false;
            panel_Filters.Controls.Clear();
            updateControls = true;
            try
            {
                List<string>Filters = new List<string>() { "Поз.по ГП", "Статус"};

                int X = start_X;
                for (int i = 0; i < Filters.Count; i++)
                {
                    GroupBox groupBox = new GroupBox();
                    groupBox.TabIndex = i;
                    groupBox.Text = Filters[i];
                    groupBox.Margin = new Padding(5);
                    panel_Filters.Controls.Add(groupBox);

                    ComboBox comboBox = new ComboBox();
                    comboBox.TabIndex = i;
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    comboBox.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
                    comboBox.Click += new EventHandler(ComboBox_Click);
                    groupBox.Controls.Add(comboBox);
                }

                SerchPanel_SelectedIndexChanged(this, EventArgs.Empty);
                UpdateControls();
                //IsCreatePanels = false;
                panel_Filters.Visible = true;
            }
            catch
            {
            }
        }
        

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (clickedIndex == comboBox.TabIndex)
            {
                if (!filters.ContainsKey(comboBox.TabIndex))
                {
                    filters.Add(comboBox.TabIndex, comboBox.Text);
                }
                else
                {
                    filters[comboBox.TabIndex] = comboBox.Text;
                }

                //updateControls = false;
                UpdateDataGrid();
            }
            else
            {
                //updateControls = false;
                return;
            }
        }

        private void ComboBox_Click(object sender, EventArgs e)
        {
            filterEromLib = false;
            //updateControls = false;
            clickedIndex = (sender as ComboBox).TabIndex;
        }

        public void UpdateControls()
        {
            if (updateControls)
            {
                for (int i = 0; i < panel_Filters.Controls.Count; i++)
                {
                    try
                    {
                        ComboBox comboBox = panel_Filters.Controls[i].Controls[0] as ComboBox;
                        comboBox.Items.Add("<Нет>");
                        filterItems[i].Sort();
                        comboBox.Items.AddRange(filterItems[i].ToArray());
                        try
                        {
                            comboBox.SelectedIndex = 0;
                        }
                        catch { }
                    }
                    catch { }
                }
                updateControls = false;
            }
            else
            {
                updateControls = false;
                return;
            }
        }

        private void SerchPanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Height > 0)
            {
                int X = start_X;
                foreach (Control control in panel_Filters.Controls)
                {
                    try
                    {
                        GroupBox groupBox = control as GroupBox;
                        groupBox.Location = new Point(X, start_Y);
                        groupBox.Height = panel_Filters.Height - 12;
                        groupBox.Width = start_Width;
                        X += groupBox.Width + 5;
                        GroupBox_SizeChanged(groupBox, EventArgs.Empty);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void GroupBox_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                GroupBox groupBox = sender as GroupBox;
                foreach (Control control in groupBox.Controls)
                {
                    if (groupBox.Width < start_Width)
                    {
                        control.Width = start_Width - 10;
                    }
                    else
                    {
                        control.Width = groupBox.Width - 10;
                    }
                    control.Location = new Point(5, groupBox.Height / 2 - control.Height / 2 + 2);
                }
            }
            catch
            {
            }
        }

        public void UpdateDataGrid()
        {
            dataGrid.Rows.Clear();
            List<PositionInfo> infos = GetPositions();
            if (infos != null)
            {
                foreach(PositionInfo position in infos)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGrid);
                    row.Height = 30;
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    //row.DefaultCellStyle.ForeColor = Color.White;
                    row.Cells[1].Value = position.Code;
                    row.Cells[2].Value = position.PositionName;
                    row.Cells[8].Value = position.Code;
                    dataGrid.Rows.Add(row);

                    var groupSecOne = position.scheduleItems.GroupBy(x => x.SecOneId);
                    foreach(var group in groupSecOne)
                    {
                        DataGridViewRow rowG = new DataGridViewRow();
                        rowG.CreateCells(dataGrid);
                        rowG.Height = 30;
                        rowG.DefaultCellStyle.BackColor = Color.LightBlue;
                        SectionsOne sectionsOne = RequestInfo.lb.SectionsOnes.FirstOrDefault(x => x.SectionOneId == group.Key);
                        try
                        {
                            rowG.Cells[0].Value = sectionsOne.SectionOneNum;
                            if (position.LanguageId == 2)
                            {
                                rowG.Cells[2].Value = sectionsOne.SectionOneNameEng;
                            }
                            else
                            {
                                rowG.Cells[2].Value = sectionsOne.SectionOneNameRus;
                            }
                            rowG.Cells[8].Value = position.Code;


                            dataGrid.Rows.Add(rowG);
                        }
                        catch
                        {
                        }

                        foreach(var section in group)
                        {
                            DataGridViewRow rowS = new DataGridViewRow();
                            rowS.CreateCells(dataGrid);
                            rowS.Height = 30;
                            rowS.Cells[0].Value = section.SecThreeNum;
                            rowS.Cells[1].Value = position.Code + " " + section.SecThreeTag;
                            rowS.Cells[2].Value = section.SecThreeName;
                            rowS.Cells[3].Value = "";
                            int progress = 0;
                            try
                            {
                                progress = section.Progress.Value;
                            }
                            catch { }
                            rowS.Cells[4].Value = progress.ToString() + "%";
                            rowS.Cells[4].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            rowS.Cells[5].Value = section.Status.StatusName;
                            Color color = Color.White;
                            switch (section.Status.StatusId)
                            {
                                case 0: color = Color.Gainsboro; break;
                                //case 1: color = Color.LightBlue; break;
                                case 2: color = Color.Orange; break;
                                case 3: color = Color.LightCoral; break;
                                case 4: color = Color.MediumSeaGreen; break;
                                case 11: color = Color.MediumSeaGreen; break;
                                case 21: color = Color.YellowGreen; break;
                            }
                            rowS.Cells[5].Style.BackColor = color;
                            string date = "";
                            string user = "";
                            GlobalMethodes.GetHistory(section.History, out date, out user);

                            rowS.Cells[6].Value = date;
                            rowS.Cells[7].Value = user;
                            rowS.Cells[8].Value = position.Code;

                            dataGrid.Rows.Add(rowS);

                            for (int f = 0; f < filterColumns.Count; f++)
                            {
                                int index = filterColumns[f];
                                if (filters.ContainsKey(f))
                                {
                                    if (filters[f] != "" && filters[f] != "<Нет>")
                                    {
                                        if (rowS.Cells[index].Value.ToString() != filters[f])
                                        {
                                            rowS.Visible = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (row.Visible)
                            {
                                for (int f = 0; f < filterColumns.Count; f++)
                                {
                                    int index = filterColumns[f];
                                    if (!filterItems.ContainsKey(f))
                                    {
                                        filterItems.Add(f, new List<string>());
                                        //Filters.filterItems[Mode][f] = new List<string>();
                                    }

                                    try
                                    {
                                        if (!filterItems[f].Contains(rowS.Cells[index].Value.ToString()))
                                        {
                                            filterItems[f].Add(rowS.Cells[index].Value.ToString());
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
            }
            clickedIndex = -1;

        }

        public void RefreshFilters()
        {
            filters.Clear();
        }

        private List<PositionInfo> GetPositions()
        {
            List<PositionInfo> positionInfos = new List<PositionInfo>();
            List<Position> positions = new List<Position>();

            try
            {
                positions = RequestInfo.lb.Positions.Where(x => x.ProjectId == GlobalData.SelectedProject.ProjectId).Where(x => x.StageId == GlobalData.SelectedStage.StageId).ToList();
            }
            catch
            {
            }
            List<Project> linkedProjects = RequestInfo.lb.Projects.Where(x => x.ProjectLinkId == GlobalData.SelectedProject.ProjectId).ToList();
            foreach(Project project in linkedProjects)
            {
                try
                {
                    positions.AddRange(RequestInfo.lb.Positions.Where(x => x.ProjectId ==project.ProjectId).Where(x => x.StageId == GlobalData.SelectedStage.StageId).ToArray());
                }
                catch
                {
                }
            }
            foreach(Position position in positions)
            {
                Project project = RequestInfo.lb.Projects.FirstOrDefault(x => x.ProjectId == position.ProjectId);
                if (project != null)
                {
                    PositionInfo positionInfo = new PositionInfo(project, GlobalData.SelectedStage, position);
                    positionInfos.Add(positionInfo);
                }
            }

            return positionInfos;
        }
    }
}
