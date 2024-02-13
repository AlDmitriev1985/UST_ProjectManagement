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
using Newtonsoft.Json;

namespace UST_ProjectManagement
{
    public partial class UC_Search_Projects : UserControl
    {
        public delegate void StartProcaess();
        public event StartProcaess StartProcess;

        Dictionary<string, string> nations = new Dictionary<string, string>();
        Dictionary<string, string> projects = new Dictionary<string, string>();
        Dictionary<string, string> positions = new Dictionary<string, string>();
        Dictionary<string, string> stages = new Dictionary<string, string>();
        Dictionary<string, string> gips = new Dictionary<string, string>();
        Dictionary<string, string> gaps = new Dictionary<string, string>();
        Dictionary<string, string> sections = new Dictionary<string, string>();

        static List<string> Filters = new List<string>();
        //static List<string> Filters0 = new List<string>() { "Страна", "Шифр", "Стадия", "ГИП", "ГАП" };
        //static List<string> Filters3 = new List<string>() { "Страна", "Шифр", "Стадия", "От раздела", "Для раздела" };



        bool updateControls = true;
        public bool IsCreatePanels = true;
        public bool filterEromLib = true;
        public Button button;
        int Mode = 0;

        public delegate void Search_Click(string[,] filters);
        public event Search_Click SearchObjects;
        int clickedIndex = 0;
        int start_Width = 145;
        int start_X = 5;
        int start_Y = 7;

        public UC_Search_Projects()
        {
            InitializeComponent();
            Filters = GlobalData.Filters0;
            //GlobalData.FilterList.Clear();
            //if (GlobalData.FilterList == null || GlobalData.FilterList.Count == 0)
            //{
            //    for (int i = 0; i < 4; i++)
            //    {
            //        string[,] filter = new string[2, Filters.Count];
            //        for (int c = 0; c < filter.Length; c++)
            //        {
            //            try
            //            {
            //                filter[1, i] = "<Нет>";
            //            }
            //            catch
            //            {
            //            }
            //        }
            //        GlobalData.FilterList.Add(filter);
            //    }
            //}
            //else
            //{
            //    SearchObjects(GlobalData.FilterList[Mode]);
            //}
            //GlobalData.filters = new string[2, Filters.Count];
            
            SizeChanged += new EventHandler(SerchPanel_SelectedIndexChanged);
        }

        public void CreatePanels(int mode)
        {
            this.Visible = false;
            this.Controls.Clear();
            Mode = mode;
            switch (mode)
            {
                case 0: Filters = GlobalData.Filters0; break;
                case 3: Filters = GlobalData.Filters3; break;
            }

            int X = start_X;
            for (int i = 0; i < Filters.Count; i++)
            {
                GroupBox groupBox = new GroupBox();
                groupBox.TabIndex = i;
                groupBox.Text = Filters[i];
                groupBox.Margin = new Padding(5);
                //groupBox.Location = new Point(X, start_Y);
                //groupBox.Height = this.Height - 12;
                //groupBox.Width = start_Width;
                //X += groupBox.Width + 5;
                //groupBox.SizeChanged += new EventHandler(GroupBox_SizeChanged);
                this.Controls.Add(groupBox);

                ComboBox comboBox = new ComboBox();
                comboBox.TabIndex = i;
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
                comboBox.Click += new EventHandler(ComboBox_Click);
                //comboBox.Location = new Point(5, groupBox.Height / 2 - comboBox.Height / 2 + 2);
                //comboBox.Width = groupBox.Width - 10;
                groupBox.Controls.Add(comboBox);
            }

            button = new Button();
            
            button.Text = "Search";
            button.TabIndex = 0;
            button.Width = 70;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Margin = new Padding(0);
            button.Dock = DockStyle.Right;
            button.Click += new EventHandler(Button_Click);
            this.Controls.Add(button);

            SerchPanel_SelectedIndexChanged(this, EventArgs.Empty);
            UpdateDictionaries(mode);
            UpdateControls(0, mode);
            IsCreatePanels = false;
            this.Visible = true;
        }

        public void CleareFilters(int mode)
        {
            string[,] filter = GlobalData.FilterList[mode];

            for (int r = 0; r < 2; r++)
            {
                for (int i = 0; i < filter.Length; i++)
                {
                    try
                    {
                        filter[r, i] = "<Нет>";
                    }
                    catch
                    {

                    }
                } 
            }
        }

        private void UpdateControls(int index, int mode)
        {
            if (updateControls && index < this.Controls.Count - 1)
            {
                for (int i = index; i < this.Controls.Count-1; i++)
                {
                    try
                    {
                        ComboBox comboBox = this.Controls[i].Controls[0] as ComboBox;
                        comboBox.Items.Clear();
                        comboBox.Items.Add("<Нет>");
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                        switch (i)
                        {
                            case 0: keyValuePairs = nations; break;
                            case 1: keyValuePairs = projects; break;
                            case 2: keyValuePairs = stages; break;
                            case 3: keyValuePairs = positions; break;                          
                            case 4:
                                if (Mode == 0)
                                {
                                    keyValuePairs = gips;
                                }
                                else
                                {
                                    keyValuePairs = sections;
                                }
                                break;
                            case 5:
                                if (Mode == 0)
                                {
                                    keyValuePairs = gaps;
                                }
                                else
                                {
                                    keyValuePairs = sections;
                                }
                                break;
                        }
                        foreach (var pair in keyValuePairs)
                        {
                            comboBox.Items.Add(pair.Key);
                        }


                        string[,] filter = GlobalData.FilterList[mode];
                        if (filter != null && filter[1, i] != null && filter[1, i] != "" && filter[1, i] != "<Нет>")
                        {
                            if (comboBox.Text != filter[1, i])
                            {
                                comboBox.Text = filter[1, i]; 
                            }
                        }
                        else
                        {
                            comboBox.SelectedIndex = 0;
                        }
                        //updateControls = true;
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                updateControls = false;
                return;
            }
        }

        private void UpdateDictionaries(int mode)
        {
            nations.Clear();
            foreach (var nation in RequestInfo.lb.Nations)
            {
                nations.Add(nation.NationId + "_" + nation.NationName, nation.NationId);
            }

            projects.Clear();
            positions.Clear();
            stages.Clear();
            gips.Clear();
            gaps.Clear();

            List<Project> Projects = new List<Project>();
            List<Position> Positions = new List<Position>();
            //List<PositionInfo> positionInfos = new List<PositionInfo>();
            List<StageProject> stageProjects = new List<StageProject>();
            List<Stage> Stages = new List<Stage>();

            try
            {
                Projects = GetProjectsByFilter(GlobalData.FilterList[mode], RequestInfo.lb.Projects);
            }
            catch (Exception)
            {

            }
            
            List<User> GIPs = RequestInfo.lb.Users.Where(x => x.FunctionId == 1).Where(f => f.LifeId == 1).ToList();
            List<User> GAPs = RequestInfo.lb.Users.Where(x => x.FunctionId == 2).Where(f => f.LifeId == 1).ToList();

            foreach (var project in Projects)
            {
                projects.Add(project.ProjectId, project.ProjectId);
                try
                {
                    Positions.AddRange(RequestInfo.lb.Positions.Where(x => x.ProjectId == project.ProjectId));
                }
                catch
                {

                }
                try
                {
                    stageProjects.AddRange(RequestInfo.lb.StageProjects.Where(x => x.ProjectId == project.ProjectId).ToList().ToArray());
                }
                catch
                {

                }
            }

            try
            {
                var StageIdList = stageProjects.Select(x => x.StageId).Distinct().OrderBy(x => x.Value);
                foreach (var sp in StageIdList)
                {
                    var stage = RequestInfo.lb.Stages.OrderBy(s => s.LanguageId).FirstOrDefault(x => x.StageId == sp);
                    Stages.Add(stage);
                    stages.Add(stage.StageTag, stage.StageId.ToString());
                }
            }
            catch
            {
            }

            foreach(Position position in Positions)
            {
                try
                {
                    positions.Add(position.PositionCode, position.PositionId.ToString());
                }
                catch
                {

                } 
            }


            if (Mode == 0)
            {
                foreach (User gip in GIPs)
                {
                    try
                    {
                        gips.Add($"{gip.UserSurname} {gip.UserName}", gip.UserId.ToString());
                    }
                    catch
                    {

                    }
                }

                foreach (User gap in GAPs)
                {
                    try
                    {
                        gaps.Add($"{gap.UserSurname} {gap.UserName}", gap.UserId.ToString());
                    }
                    catch
                    {

                    }
                } 
            }
            else if (Mode == 3)
            {
                foreach(SectionsThree sectionsThree in RequestInfo.lb.SectionsThrees)
                {
                    try
                    {
                        sections.Add(sectionsThree.SectionThreeTagRus, sectionsThree.SectionThreeId.ToString());
                    }
                    catch
                    {

                    }
                }
            }
        }

        public List<Project> GetProjectsByFilter(string[,] filter, List<Project> projects)
        {
            List<Project> Projects = new List<Project>();
            if (filter[0, 0] != null && filter[0, 0] != "<Нет>" && filter[0, 0] != "")
            {
                Projects = projects.Where(x => x.NationId == filter[0, 0]).ToList();
            }
            else
            {
                Projects = projects;
            }

            if (filter[0, 1] != null && filter[0, 1] != "<Нет>" && filter[0, 1] != "")
            {
                Projects = Projects.Where(x => x.ProjectId == filter[0, 1]).ToList();
            }

            if (filter[0, 2] != null && filter[0, 2] != "<Нет>" && filter[0, 2] != "")
            {
                List<Project> sList = new List<Project>();
                foreach (Project project in Projects)
                {
                    try
                    {
                        var stageProjects = RequestInfo.lb.StageProjects.Where(x => x.ProjectId == project.ProjectId);
                        var currientStage = stageProjects.Where(x => x.StageId.Value.ToString() == filter[0, 2]);
                        if (currientStage != null && currientStage.Count() > 0)
                        {
                            sList.Add(project);
                        }
                    }
                    catch
                    {
                    }
                }
                Projects = sList;
            }
            return Projects;
        }

        private void UC_Search_Projects_SizeChanged(object sender, EventArgs e)
        {
            foreach(Control control in this.Controls)
            {
                control.Height = this.Height - 12;
                control.Location = new Point(5, 7);
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

        private void ComboBox_Click(object sender, EventArgs e)
        {
            filterEromLib = false;
            clickedIndex = (sender as ComboBox).TabIndex;
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (updateControls && clickedIndex == comboBox.TabIndex)
            {

                string code = "<Нет>";
                string[,] filter = GlobalData.FilterList[Mode];
                filter[0, comboBox.TabIndex] = code;
                filter[1, comboBox.TabIndex] = code;
                UpdateDictionaries(Mode);

                switch (comboBox.TabIndex)
                {
                    case 0:
                        {
                            try
                            {
                                code = nations[comboBox.Text];
                            }
                            catch
                            {
                            }
                        }
                        break;
                    case 1:
                        {
                            try
                            {
                                code = projects[comboBox.Text];
                            }
                            catch
                            {
                            }
                        }
                        break;
                    case 2:
                        {
                            try
                            {
                                code = stages[comboBox.Text];
                            }
                            catch
                            {
                            }
                        }
                        break;
                    case 3:
                        {
                            try
                            {
                                code = positions[comboBox.Text];
                            }
                            catch
                            {
                            }
                        }
                        break;
                    case 4:
                        {
                            if (Mode == 0)
                            {
                                try
                                {
                                    code = gips[comboBox.Text];
                                }
                                catch
                                {
                                } 
                            }
                            else if (Mode == 3)
                            {
                                try
                                {
                                    code = sections[comboBox.Text];
                                }
                                catch
                                {

                                }
                            }
                        }
                        break;
                    case 5:
                        {
                            if (Mode == 0)
                            {
                                try
                                {
                                    code = gaps[comboBox.Text];
                                }
                                catch
                                {
                                }
                            }
                            else if (Mode == 3)
                            {
                                try
                                {
                                    code = sections[comboBox.Text];
                                }
                                catch
                                {

                                }
                            }
                        }
                        break;
                }

                filter[0, comboBox.TabIndex] = code;
                filter[1, comboBox.TabIndex] = comboBox.Text;

                if (! filterEromLib)
                {
                    for (int i = comboBox.TabIndex + 1; i < Filters.Count; i++)
                    {
                        filter[0, comboBox.TabIndex + 1] = "<Нет>";
                        filter[1, comboBox.TabIndex + 1] = "<Нет>";
                    } 
                }
                UpdateDictionaries(Mode);
                UpdateControls(comboBox.TabIndex + 1, Mode); 
            }
            else
            {
                updateControls = true;
                return;
            }
        }

        private void SerchPanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Height > 0)
            {
                int X = start_X;
                foreach (Control control in this.Controls)
                {
                    try
                    {
                        GroupBox groupBox = control as GroupBox;
                        groupBox.Location = new Point(X, start_Y);
                        groupBox.Height = this.Height - 12;
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

        public void Button_Click(object sender, EventArgs e)
        {
            try
            {
                GlobalData.loadInfo = "Поиск...";
                string txt = JsonConvert.SerializeObject(GlobalData.FilterList, Formatting.Indented);
                GlobalMethodes.SaveToJSON(GlobalData.TempDirPath, GlobalData.FiltersFileName, txt);
                if (!MainForm.firstStart)StartProcess?.Invoke();
                SearchObjects?.Invoke(GlobalData.FilterList[Mode]);
            }
            catch
            {
            }
        }

    }
}
