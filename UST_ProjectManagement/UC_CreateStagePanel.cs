using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using LibraryDB.DB;
using System.Threading;

namespace UST_ProjectManagement
{
    public partial class UC_CreateStagePanel : UserControl
    {
        public delegate void ButtonCancel_Click(byte mode);
        public event ButtonCancel_Click CancelStagePanel;

        public delegate void ButtonCreate_Click();
        public event ButtonCreate_Click CreateStage;

        public delegate void StartProcess_Start();
        public event StartProcess_Start StartProcess;

        int LanguageId = 1;
        List<Stage> Stages = new List<Stage>();
        public UC_CreateStagePanel()
        {
            InitializeComponent();
            UpdateStageComboBox();


            //panel1.BackColor = MainForm.HeaderColor;
            tableLayoutPanel2.BackColor = MainForm.HeaderColor;
            tableLayoutPanel6.BackColor = MainForm.HeaderColor;
        }

        private void UC_CreateStagePanel_Load(object sender, EventArgs e)
        {
            tableLayoutPanel1.RowStyles[0].Height = 25;
            tableLayoutPanel1.RowStyles[5].Height = 25;
            tableLayoutPanel6.ColumnStyles[1].Width = 25;
        }

        public void UpdateLanguageComboBox()
        {
            comboBoxLanguage.Items.Clear();
            //comboBoxLanguage.Items.Add("-");
            foreach (var lang in RequestInfo.lb.Languages)
            {
                comboBoxLanguage.Items.Add(lang.LanguageTag);
            }

            Language language = null;

            //MessageBox.Show(GlobalData.SelectedProject.prjLanguage);
            if (GlobalData.SelectedProject != null && GlobalData.SelectedProject.LanguageId != null)
            {
                comboBoxStages.Enabled = true;
                Stages.Clear();

                List<StageProject> stageProjects = RequestInfo.lb.StageProjects.FindAll(x => x.ProjectId == GlobalData.SelectedProject.ProjectId);
                if (stageProjects.Count > 0)
                {  
                    foreach(StageProject sp in stageProjects)
                    {
                        Stages.Add(RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == sp.StageId));
                    }
                    
                    Stage stage = RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == stageProjects[0].StageId);
                    language = RequestInfo.lb.Languages.FirstOrDefault(x => x.LanguageId == stage.LanguageId);
                }
                if (language != null)
                {
                    LanguageId = language.LanguageId;
                    comboBoxLanguage.Text = language.LanguageTag;
                }
                else if (comboBoxLanguage.Items.Count > 0)
                {
                    comboBoxLanguage.SelectedIndex = 0;
                }
                if (comboBoxLanguage.SelectedIndex > 0)
                {
                    comboBoxLanguage.Enabled = false;
                }
                else
                {
                    comboBoxLanguage.Enabled = true;
                }
            }
            else if (GlobalData.SelectedProduct != null)
            {
                comboBoxLanguage.Enabled = false;
                comboBoxStages.Enabled = false;
                comboBoxLanguage.Text = "Рус";
                Stages.Clear();
                List<StageProduct> stageProducts = RequestInfo.lb.StageProducts.FindAll(x => x.ProductId == GlobalData.SelectedProduct.ProductId);
                if (stageProducts.Count > 0)
                {
                    Stages.Add(RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == 5));
                    Stages.Add(RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == 7));
                    foreach (StageProduct sp in stageProducts)
                    {
                        Stages.Add(RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == sp.StageId));
                    }
                }
            }
            else if (GlobalData.SelectedTechSolution != null)
            {
                comboBoxLanguage.Enabled = false;
                comboBoxStages.Enabled = false;
                comboBoxLanguage.Text = "Рус";
                Stages.Clear();
                List<StageTechSolution> stages = RequestInfo.lb.StageTechSolutions.FindAll(x => x.TechSolutionId == GlobalData.SelectedTechSolution.TechSolutionId);
                if (stages.Count > 0)
                {
                    Stages.Add(RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == 5));
                    Stages.Add(RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == 7));
                    foreach (StageTechSolution sp in stages)
                    {
                        Stages.Add(RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == sp.StageId));
                    }
                }
            }
            else
            {
                comboBoxLanguage.Enabled = true;
                comboBoxLanguage.Text = "-";
            }
        }


        public void UpdateStageComboBox()
        {
            comboBoxStages.Items.Clear();
            comboBoxStages.Items.Add("-");
            try
            {
                List<Stage> stages = RequestInfo.lb.Stages.FindAll(x => x.LanguageId == LanguageId);
                stages = stages.OrderBy(x => x.StageIndex).ToList();
                foreach (Stage stage in stages)
                {
                    if (!Stages.Contains(stage))
                    {
                        comboBoxStages.Items.Add(stage.StageTag);
                    }
                }
            }
            catch
            {
            }
            if ((GlobalData.SelectedProduct != null || GlobalData.SelectedTechSolution != null) && comboBoxStages.Items.Count > 1)
            {
                comboBoxStages.SelectedIndex = 1;
            }
            else
            {
                comboBoxStages.SelectedIndex = 0;
            }           
        }

        private void comboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Language language = RequestInfo.lb.Languages.FirstOrDefault(x => x.LanguageTag == (sender as ComboBox).Text);
            if (language != null) LanguageId = language.LanguageId;
            else LanguageId = 1;
            UpdateStageComboBox();
        }


        private void comboBoxStages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxStages.Text != "" && comboBoxStages.Text != "-")
            {
                foreach (ClassStageType stage in GlobalData.StageTypeList)
                {
                    //MessageBox.Show(stage.Name);
                    if (stage.Tag == comboBoxStages.Text)
                    {
                        labelStageName.Text = stage.Name;
                        break;
                    }
                }
                
            }
            else
            {
                labelStageName.Text = "";
            }
            
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            CancelStagePanel?.Invoke(0);
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {

            if (comboBoxStages.Text != "" && comboBoxStages.Text != "-")
            {
                Stage stage = RequestInfo.lb.Stages.FirstOrDefault(x => x.StageTag == comboBoxStages.Text);
                if (GlobalData.SelectedProject != null)
                {
                    GlobalData.loadInfo = "Создание стадии..";
                    StartProcess?.Invoke();

                    string StageId = "";
                    string StageTag = "";
                    string StageIndex = "";
                    string StageLable = "";
                    string DirPath = "";

                    if (stage != null)
                    {
                        StageId = stage.StageId.ToString();
                        StageTag = stage.StageTag;
                        StageIndex = stage.StageIndex;
                        StageLable = stage.StageIndex + "_" + stage.StageTag;

                        string[] sPath = GlobalData.SelectedDirPath.Split('\\');
                        for (int i = 3; i < sPath.Length - 1; i++)
                        {
                            DirPath += sPath[i] + "\\";
                        }

                        //DirPath += stage.StageIndex + "_" + stage.StageTag;

                        if (GlobalMethodes.CreateStage(DirPath, StageTag, StageId, StageIndex) == true)
                        {
                            GlobalData.SelectedDirPath += StageLable + "\\";
                            GlobalData.BuferDirPath = GlobalData.SelectedDirPath;
                            CreateStage?.Invoke();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось создать Стадию проекта.\nОбратитесь к администратору", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не удалось создать стадию", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }                  
                }
                else if (GlobalData.SelectedProduct != null)
                {
                    if (stage != null) //
                    {
                        GlobalData.loadInfo = "Создание стадии..";
                        StartProcess?.Invoke();

                        ClassCreatedProdStage prodStage = new ClassCreatedProdStage();
                        //ClassStageType sStage = (GlobalData.StageTypeList.Where(t => t.Tag == comboBoxStages.Text).First());
                        prodStage.StageId = stage.StageId.ToString();
                        prodStage.ProductId = GlobalData.SelectedProduct.ProductId.ToString();
                        //ClassUser pResp = (GlobalData.User_FullList.Where(n => n.FullName == GlobalData.SelectedProduct.User.UserName).First());
                        prodStage.UserId = GlobalData.user.UserId.ToString();
                        prodStage.StageProductDataStart = GlobalData.SelectedProduct.ProductDataStart;
                        prodStage.StageProductDataEnd = GlobalData.SelectedProduct.ProductDataEnd;
                        prodStage.ProductAuthor = GlobalData.user.UserAccount;
                        string[] sPath = GlobalData.SelectedDirPath.Split('\\');
                        string DirPath = "";
                        for(int i = 3; i < sPath.Length - 1; i++)
                        {
                            DirPath += sPath[i] + "\\";
                        }

                        prodStage.Path = DirPath + stage.StageIndex + "_" + stage.StageTag;


                        //ClassFolder sFolder = null;
                        //foreach (ClassFolder _folder in GlobalData.NaviTreeView.FullFoldersList)
                        //{
                        //    if (_folder.Name == GlobalData.SelectedProduct.ProductCode)
                        //    {
                        //        sFolder = _folder;
                        //        break;
                        //    }
                        //}

                        if (GlobalMethodes.CreateProductStage(JsonConvert.SerializeObject(prodStage, Formatting.Indented)) == true) //
                        {
                            GlobalData.BuferDirPath = GlobalData.SelectedDirPath + stage.StageIndex + "_" + stage.StageTag + "\\";
                            CreateStage?.Invoke();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось создать Стадию продукта.\nОбратитесь к администратору", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"В указанном продукте уже создана стадия {comboBoxStages.Text}.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (GlobalData.SelectedTechSolution != null)
                {
                    if (stage != null) //
                    {
                        GlobalData.loadInfo = "Создание стадии..";
                        StartProcess?.Invoke();

                        ClassCreatedTechSolutionStage tsStage = new ClassCreatedTechSolutionStage();
                        tsStage.StageId = stage.StageId.ToString();
                        tsStage.TechSolutionId = GlobalData.SelectedTechSolution.TechSolutionId.ToString();
                        tsStage.UserId = GlobalData.user.UserId.ToString();
                        tsStage.StageTechSolutionDataStart = GlobalData.SelectedTechSolution.TechSolutionDataStart;
                        tsStage.StageTechSolutionDataEnd = GlobalData.SelectedTechSolution.TechSolutionDataEnd;
                        tsStage.TechSolutionAuthor = GlobalData.user.UserAccount;
                        string[] sPath = GlobalData.SelectedDirPath.Split('\\');
                        string DirPath = "";
                        for (int i = 2; i < sPath.Length - 1; i++)
                        {
                            DirPath += sPath[i] + "\\";
                        }

                        tsStage.Path = DirPath + stage.StageIndex + "_" + stage.StageTag;

                        if (GlobalMethodes.CreateTechSolutionStage(JsonConvert.SerializeObject(tsStage, Formatting.Indented)) == true) //
                        {
                            GlobalData.BuferDirPath = GlobalData.SelectedDirPath + stage.StageIndex + "_" + stage.StageTag + "\\";
                            Thread.Sleep(3000);
                            CreateStage?.Invoke();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось создать Стадию продукта.\nОбратитесь к администратору", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"В указанном продукте уже создана стадия {comboBoxStages.Text}.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Стадия не указана. \nУкажите стадию проектирования.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        
    }
}
