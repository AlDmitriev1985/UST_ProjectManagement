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

        public delegate void ComboBoxSelectIndexChanged();
        public event ComboBoxSelectIndexChanged comboBoxSelectIndexChanged;

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
            Filters = UST_ProjectManagement.Filters.filtersSets[0];
            
            SizeChanged += new EventHandler(SerchPanel_SelectedIndexChanged);
        }

        public void CreatePanels(int mode)
        {
            this.Visible = false;
            this.Controls.Clear();
            Mode = mode;

            try
            {
                Filters = UST_ProjectManagement.Filters.filtersSets[mode];

                int X = start_X;
                for (int i = 0; i < Filters.Count; i++)
                {
                    GroupBox groupBox = new GroupBox();
                    groupBox.TabIndex = i;
                    groupBox.Text = Filters[i];
                    groupBox.Margin = new Padding(5);
                    this.Controls.Add(groupBox);

                    ComboBox comboBox = new ComboBox();
                    comboBox.TabIndex = i;
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    comboBox.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
                    comboBox.Click += new EventHandler(ComboBox_Click);
                    groupBox.Controls.Add(comboBox);
                }

                button = new Button();

                SerchPanel_SelectedIndexChanged(this, EventArgs.Empty);
                UpdateControls(0, mode);
                IsCreatePanels = false;
                this.Visible = true;
            }
            catch
            {
            }
        }


        private void UpdateControls(int index, int mode)
        {
            if (updateControls && index < this.Controls.Count)
            {
                for (int i = index; i < this.Controls.Count; i++)
                {
                    try
                    {
                        ComboBox comboBox = this.Controls[i].Controls[0] as ComboBox;
                        comboBox.Items.Add("<Нет>");
                        UST_ProjectManagement.Filters.filterItems[mode][i].Sort();
                        comboBox.Items.AddRange(UST_ProjectManagement.Filters.filterItems[mode][i].ToArray());
                        try
                        {
                            comboBox.SelectedIndex = 0;
                        }
                        catch { }
                    }
                    catch { }
                }
                
            }
            else
            {
                updateControls = false;
                return;
            }
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
                if (!UST_ProjectManagement.Filters.filters[Mode].ContainsKey(comboBox.TabIndex))
                {
                    UST_ProjectManagement.Filters.filters[Mode].Add(comboBox.TabIndex, comboBox.Text);
                }
                else
                {
                    UST_ProjectManagement.Filters.filters[Mode][comboBox.TabIndex] = comboBox.Text;
                }
                comboBoxSelectIndexChanged?.Invoke();
               
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
