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
using System.Threading;
using LibraryDB.DB;

namespace UST_ProjectManagement
{
    public partial class UC_CreatePositionPanel : UserControl
    {
        bool StartCalendar = false;
        bool EndCalendar = false;
        List<string> ErroreList = new List<string>();

        public delegate void ButtonCreate_Click();
        public event ButtonCreate_Click CreatePosition;
        public event ButtonCreate_Click CreateProduct;

        public delegate void ButtonCancel_Click();
        public event ButtonCancel_Click CancelPosition;

        public delegate void StartProcess_Start();
        public event StartProcess_Start StartProcess;

        int Mode = 0;

        //public string SelectedDate;

        public UC_CreatePositionPanel()
        {
            InitializeComponent();

            tableLayoutPanel6.BackColor = MainForm.HeaderColor;
            tableLayoutPanelButtons.BackColor = MainForm.HeaderColor;
            uC_CalendarPanel1.ApplyDate += ApplyDate;

        }

        private void UC_CreatePositionPanel_Load(object sender, EventArgs e)
        {
            tableLayoutPanel1.RowStyles[0].Height = 25;
            tableLayoutPanel1.RowStyles[20].Height = 25;
        }

        public void FiilProjectInfo(byte mode = 0)
        {
            Mode = mode;
            textBoxProjectNo.Text = "";
            textBoxPositionNo.Text = "";
            textBoxStage.Text = "";
            richTextBox1.Text = "";
            maskedTextBoxStartDate.Text = "";
            maskedTextBoxEndDate.Text = "";
            labelDropOrNot.Text = "-";
            richTextBox2.Text = "";

            tableLayoutPanel2.ColumnStyles[1].Width = 106;
            tableLayoutPanel2.ColumnStyles[3].Width = 56;

            if (GlobalData.SelectedProject != null)
            {
                if (mode == 0)
                {
                    label1.Text = "Создать поз. по ГП";

                    textBoxProjectNo.Text = GlobalData.SelectedProject.ProjectId;
                    textBoxStage.Text = GlobalData.SelectedStage.StageTag;

                    maskedTextBoxStartDate.Text = GlobalData.SelectedProject.ProjectDataStart;
                    maskedTextBoxEndDate.Text = GlobalData.SelectedProject.ProjectDataEnd;

                    label8.Visible = true;
                    comboBoxGAP.Visible = true;

                    ProjectStageInfo projectStageInfo = new ProjectStageInfo(GlobalData.SelectedProject, GlobalData.SelectedStage);

                    if (GlobalData.SelectedProject.ProjectId.IndexOf('-') != -1)
                    {
                        labelDropOrNot.Text = ".";
                        var pos = RequestInfo.lb.Positions.Where(p => p.ProjectId == GlobalData.SelectedProject.ProjectId).ToList();
                        //textBoxPositionNo.Text = (pos.Count + 1).ToString();
                        textBoxPositionNo.Text = projectStageInfo.CreateNewPositionId(true);
                    }
                    else
                    {
                        labelDropOrNot.Text = "-";
                        textBoxPositionNo.Text = projectStageInfo.CreateNewPositionId(false);
                    }
                    textBoxPositionNo.Enabled = true;
                    //label2.Text = "Наименование:";

                    richTextBox2.Visible = false;
                    label9.Visible = false;
                    tableLayoutPanel1.RowStyles[8].Height = 0;
                    tableLayoutPanel1.RowStyles[9].Height = 0;

                    GlobalMethodes.UpdateCombaBox(GlobalData.User_GIPList, comboBoxGIP);
                    if (GlobalData.SelectedProject.ProjectGIP != "")
                    {
                        comboBoxGIP.Text = GlobalData.SelectedProject.ProjectGIP;
                    }
                    else
                    {
                        comboBoxGIP.SelectedIndex = 0;
                    }
                    GlobalMethodes.UpdateCombaBox(GlobalData.User_GAPList, comboBoxGAP);
                    comboBoxGAP.SelectedIndex = 0;  
                }
                else if (mode == 3)
                {
                    label1.Text = "Создать подпроекта";

                    textBoxProjectNo.Text = GlobalData.SelectedProject.ProjectId;
                    textBoxStage.Text = GlobalData.SelectedStage.StageTag;

                    maskedTextBoxStartDate.Text = GlobalData.SelectedProject.ProjectDataStart;
                    maskedTextBoxEndDate.Text = GlobalData.SelectedProject.ProjectDataEnd;

                    label8.Visible = true;
                    comboBoxGAP.Visible = true;

                    ProjectStageInfo projectStageInfo = new ProjectStageInfo(GlobalData.SelectedProject, GlobalData.SelectedStage);

                    if (GlobalData.SelectedProject.ProjectId.IndexOf('-') != -1)
                    {
                        labelDropOrNot.Text = ".";
                        var pos = RequestInfo.lb.Positions.Where(p => p.ProjectId == GlobalData.SelectedProject.ProjectId).ToList();
                        //textBoxPositionNo.Text = (pos.Count + 1).ToString();
                        textBoxPositionNo.Text = projectStageInfo.CreateNewPositionId(true);
                    }
                    else
                    {
                        labelDropOrNot.Text = "-";
                        textBoxPositionNo.Text = projectStageInfo.CreateNewPositionId(false);
                    }
                    textBoxPositionNo.Enabled = true;
                    //label2.Text = "Наименование:";

                    //richTextBox2.Visible = false;
                    //label9.Visible = false;
                    //tableLayoutPanel1.RowStyles[8].Height = 0;
                    //tableLayoutPanel1.RowStyles[9].Height = 0;

                    GlobalMethodes.UpdateCombaBox(GlobalData.User_GIPList, comboBoxGIP);
                    if (GlobalData.SelectedProject.ProjectGIP != "")
                    {
                        comboBoxGIP.Text = GlobalData.SelectedProject.ProjectGIP;
                    }
                    else
                    {
                        comboBoxGIP.SelectedIndex = 0;
                    }
                    GlobalMethodes.UpdateCombaBox(GlobalData.User_GAPList, comboBoxGAP);
                    comboBoxGAP.SelectedIndex = 0;
                }

            }
            else if (GlobalData.SelectedProductCatalog != null)
            {
                label1.Text = "Создать продукт";

                ProductCatalogInfo productCatalogInfo = new ProductCatalogInfo(GlobalData.SelectedProductCatalog);

                textBoxProjectNo.Text = "R" + DateTime.Now.Year.ToString().Substring(2,2);
                labelDropOrNot.Text = ".";
                textBoxPositionNo.Text = productCatalogInfo.GetNewProductIndex(DateTime.Now.Year.ToString().Substring(2, 2));
                textBoxPositionNo.Enabled = false;
                textBoxStage.Text = "-";

                richTextBox2.Visible = true;
                label9.Visible = true;
                tableLayoutPanel1.RowStyles[8].Height = 20;
                tableLayoutPanel1.RowStyles[9].Height = 100;

                List<User> users = RequestInfo.lb.Users.Where(x => x.FunctionId <= 2).Where(y => y.FunctionId > 0).ToList();
                List<string> usersList = new List<string>();
                foreach(User user in users)
                {
                    usersList.Add($"{user.UserSurname} {user.UserName} {user.UserMidlName}");
                }

                GlobalMethodes.UpdateCombaBox(usersList, comboBoxGIP);
                if (comboBoxGIP.Items.Count > 0) comboBoxGIP.SelectedIndex = 0;
                label8.Visible = false;
                comboBoxGAP.Visible = false;
            }
            else if (GlobalData.SelectedTechSolutionCatalog != null)
            {
                label1.Text = "Создать тех.решение";

                TechSolutionCatalogInfo techSolutionCatalogInfo = new TechSolutionCatalogInfo(GlobalData.SelectedTechSolutionCatalog);


                textBoxProjectNo.Text = "T" + DateTime.Now.Year.ToString().Substring(2, 2);
                labelDropOrNot.Text = ".";
                textBoxPositionNo.Text = techSolutionCatalogInfo.GetNewSolutionIndex(DateTime.Now.Year.ToString().Substring(2, 2));
                textBoxPositionNo.Enabled = false;
                textBoxStage.Text = "-";

                richTextBox2.Visible = true;
                label9.Visible = true;
                tableLayoutPanel1.RowStyles[8].Height = 20;
                tableLayoutPanel1.RowStyles[9].Height = 100;

                List<User> users = RequestInfo.lb.Users.Where(x => x.FunctionId <= 2).Where(y => y.FunctionId > 0).ToList();
                List<string> usersList = new List<string>();
                foreach (User user in users)
                {
                    usersList.Add($"{user.UserSurname} {user.UserName} {user.UserMidlName}");
                }

                GlobalMethodes.UpdateCombaBox(usersList, comboBoxGIP);
                if (comboBoxGIP.Items.Count > 0) comboBoxGIP.SelectedIndex = 0;
                label8.Visible = false;
                comboBoxGAP.Visible = false;
            }
            else if (GlobalData.SelectedCountry != null && (mode == 1 || mode == 2))
            {
                label1.Text = "Создать проект";
                tableLayoutPanel2.ColumnStyles[1].Width = 56;
                tableLayoutPanel2.ColumnStyles[3].Width = 56;

                if (mode == 1)
                {
                    textBoxProjectNo.Text = $"2.{GlobalData.SelectedCountry.NationId}"; 
                }
                else if (mode == 2)
                {
                    textBoxProjectNo.Text = $"T.{GlobalData.SelectedCountry.NationId}";
                }
                labelDropOrNot.Text = ".";
                textBoxPositionNo.Text = GetProjectNumber(mode);


                label8.Visible = true;
                comboBoxGAP.Visible = true;

                GlobalMethodes.UpdateCombaBox(GlobalData.User_GIPList, comboBoxGIP);
                comboBoxGIP.SelectedIndex = 0;
                GlobalMethodes.UpdateCombaBox(GlobalData.User_GAPList, comboBoxGAP);
                comboBoxGAP.SelectedIndex = 0;
            }
        }

        private string GetProjectNumber(int mode)
        {
            string result = "000";
            int maxnum = 0;
            var projects = RequestInfo.lb.Projects.Where(x => x.NationId == GlobalData.SelectedCountry.NationId).Select(z => z.ProjectId);
            foreach(string id in projects)
            {
                string[] _splitid = id.Split('.');
                if (mode == 2)
                {
                    if (_splitid[0].Length > 1 || (_splitid[0] != "T" && _splitid[0] != "Т"))
                    {
                        continue;
                    }
                }
                char[] _splitcode = null;
                try
                {
                    _splitcode = _splitid[2].ToCharArray();
                }
                catch
                {
                    _splitcode = _splitid[1].ToCharArray();
                }
                if (_splitcode != null)
                {
                    int selnum = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        int num = 0;
                        try
                        {
                            num = Convert.ToInt32(_splitcode[i].ToString());
                        }
                        catch { }

                        if (num > 0)
                        {
                            if (i == 0) selnum += num * 100;
                            else if (i == 1) selnum += num * 10;
                            else if (i == 2) selnum += num;
                        }
                    }
                    if (selnum > maxnum) maxnum = selnum; 
                }
            }
            maxnum += 1;
            if (maxnum >= 100)
            {
                result = maxnum.ToString();
            }
            else if (maxnum >= 10)
            {
                result = "0" + maxnum.ToString();
            }
            else
            {
                result = "00" + maxnum.ToString();
            }
            return result;
        }

        private void buttonStartDate_Click(object sender, EventArgs e)
        {
            if (StartCalendar == false && EndCalendar == false)
            {
                StartCalendar = true;
                int sX = 5;
                int sY = 0;
                for (int i = 0; i<12; i++)
                {
                    sY = sY + Convert.ToInt32(tableLayoutPanel1.RowStyles[i].Height);
                }
                uC_CalendarPanel1.Size = new Size(174, 202);
                uC_CalendarPanel1.Location = new Point(sX, sY);

                UpdateSelectedDate(maskedTextBoxStartDate);

                uC_CalendarPanel1.BringToFront();
                uC_CalendarPanel1.Visible = true;

            }
            else if (StartCalendar == true)
            {
                StartCalendar = false;
                uC_CalendarPanel1.SendToBack();
                uC_CalendarPanel1.Visible = false;
            }
        }

        private void buttonEndDate_Click(object sender, EventArgs e)
        {
            if (StartCalendar == false && EndCalendar == false)
            {
                EndCalendar = true;
                int sX = 0;
                int sY = 0;
                for (int i = 0; i < 12; i++)
                {
                    sY = sY + Convert.ToInt32(tableLayoutPanel1.RowStyles[i].Height);
                }
                sX = Convert.ToInt32(tableLayoutPanel1.Width - uC_CalendarPanel1.Width - 5);

                UpdateSelectedDate(maskedTextBoxEndDate);

                uC_CalendarPanel1.Location = new Point(sX, sY);
                uC_CalendarPanel1.BringToFront();
                uC_CalendarPanel1.Visible = true;
            }
            else if (EndCalendar == true)
            {
                EndCalendar = false;
                uC_CalendarPanel1.SendToBack();
                uC_CalendarPanel1.Visible = false;
            }
        }

        private void ApplyDate()
        {
            if (StartCalendar == true)
            {
                maskedTextBoxStartDate.Text = uC_CalendarPanel1.Date;
                buttonStartDate_Click(this.buttonStartDate, EventArgs.Empty);
            }
            else
            {
                maskedTextBoxEndDate.Text = uC_CalendarPanel1.Date;
                buttonEndDate_Click(this.buttonEndDate, EventArgs.Empty);
            }
        }

        private void UpdateSelectedDate(MaskedTextBox TB)
        {
            string[] seldate = TB.Text.Split('.');
            try
            {
                uC_CalendarPanel1.SelectedDate = new DateTime(Convert.ToInt32(seldate[2]), Convert.ToInt32(seldate[1]), Convert.ToInt32(seldate[0]));
            }
            catch
            {
                
            }
            //MessageBox.Show(TB.Text);
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            ErroreList.Clear();

            if (GlobalData.SelectedStage != null && Mode == 0)
            {
                ProjectStageInfo projectStageInfo = new ProjectStageInfo(GlobalData.SelectedProject, GlobalData.SelectedStage);
                string PositionCode = textBoxProjectNo.Text + labelDropOrNot.Text + textBoxPositionNo.Text;
                string ParantStageTag = projectStageInfo.StageTag;
                //Проверка на наличие создаваемого шифра 
                Position existingPos = projectStageInfo.Positions.FirstOrDefault(x => x.PositionCode == PositionCode);

                if (existingPos != null) ErroreList.Add("Позиция с таким номером по ГП уже существует");

                if (ValidatePositionPanel(0) == true)
                {
                    StartProcess?.Invoke();
                    string PositionName = richTextBox1.Text;
                    string PositionNumber = textBoxPositionNo.Text;
                    string PositionFullPath = GlobalData.SelectedDirPath;
                    GlobalData.BuferDirPath = GlobalData.SelectedDirPath + PositionCode + "\\";
                    string[] splitPath = PositionFullPath.Split('\\');
                    string PositionShortPath = "\\";
                    for (int i = 3; i < splitPath.Length - 1; i++)
                    {
                        PositionShortPath += splitPath[i] + "\\";
                    }
                    string PositionGIP = comboBoxGIP.Text;
                    string PositionGAP = comboBoxGAP.Text;
                    int gipId = 1;
                    try
                    {
                        gipId = RequestInfo.lb.Users.FirstOrDefault(x => $"{x.UserSurname} {x.UserName} {x.UserMidlName}" == PositionGIP).UserId;
                    }
                    catch { }
                    int gapId = 1;
                    try
                    {
                        gapId = RequestInfo.lb.Users.FirstOrDefault(x => $"{x.UserSurname} {x.UserName} {x.UserMidlName}" == PositionGAP).UserId;
                    }
                    catch { }
                    string StartDate = maskedTextBoxStartDate.Text;
                    string EndDate = maskedTextBoxEndDate.Text;

                    //ClassFolder sFolder = null;


                    if (GlobalMethodes.CreatePosition(projectStageInfo.Stage.StageId.ToString(),
                                                      PositionCode, PositionName, PositionNumber,
                                                      projectStageInfo.ProjectId,
                                                      PositionShortPath, 
                                                      gipId.ToString(), gapId.ToString(), StartDate, EndDate) == true)
                    {
                        CancelPosition?.Invoke();
                        CreatePosition?.Invoke();
                    }
                }
                else
                {
                    string Errores = "";
                    foreach (string err in ErroreList)
                    {
                        Errores = Errores + "\n" + err;
                    }
                    MessageBox.Show(Errores, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (GlobalData.SelectedProductCatalog !=  null)
            {
                ProductCatalogInfo productCatalogInfo = new ProductCatalogInfo(GlobalData.SelectedProductCatalog);

                string ProductID = textBoxProjectNo.Text + "." + textBoxPositionNo.Text;
                //Проверка на наличие создаваемого шифра 
                Product existingProduct = productCatalogInfo.Products.FirstOrDefault(x => x.ProductCode == ProductID);
                if (existingProduct != null)
                {
                    ErroreList.Add("Продукт с таким номером уже существует");
                }

                if (ValidatePositionPanel(1) == true)
                {
                    StartProcess?.Invoke();

                    CreateProduct newProduct = new CreateProduct();
                    newProduct.ProductGroupId = productCatalogInfo.CatalogId;
                    User user = RequestInfo.lb.Users.FirstOrDefault(x => $"{x.UserSurname} {x.UserName} {x.UserMidlName}" == comboBoxGIP.Text);
                    newProduct.UserId = user.UserId;         
                    newProduct.ProductCode = ProductID;
                    newProduct.ProductFullName = richTextBox1.Text;
                    newProduct.ProductDescription = richTextBox2.Text;
                    newProduct.ProductDataStart = maskedTextBoxStartDate.Text;
                    newProduct.ProductDataEnd = maskedTextBoxEndDate.Text;
                    newProduct.ProductAuthor = GlobalData.user.UserAccount;
                    newProduct.ProductGroupCode = productCatalogInfo.CatalogCode;
                    GlobalData.BuferDirPath = GlobalData.SelectedDirPath + ProductID + "\\";

                    if (GlobalMethodes.CreateProduct(JsonConvert.SerializeObject(newProduct, Formatting.Indented)) == true)//
                    {
                        Thread.Sleep(3000);
                        CancelPosition?.Invoke();
                        CreateProduct?.Invoke();
                    }
                }
            }
            else if (GlobalData.SelectedTechSolutionCatalog != null)
            {
                TechSolutionCatalogInfo techSolutionCatalogInfo = new TechSolutionCatalogInfo(GlobalData.SelectedTechSolutionCatalog);

                string ProductID = textBoxProjectNo.Text + "." + textBoxPositionNo.Text;
                //Проверка на наличие создаваемого шифра 
                TechSolution existingProduct = techSolutionCatalogInfo.TechSolutions.FirstOrDefault(x => x.TechSolutionCode == ProductID);
                if (existingProduct != null)
                {
                    ErroreList.Add("Тех.решение с таким номером уже существует");
                }

                if (ValidatePositionPanel(1) == true)
                {
                    StartProcess?.Invoke();

                    CreateTechSolution newSolution = new CreateTechSolution();
                    newSolution.TechSolutionGroupId = techSolutionCatalogInfo.CatalogId;
                    User user = RequestInfo.lb.Users.FirstOrDefault(x => $"{x.UserSurname} {x.UserName} {x.UserMidlName}" == comboBoxGIP.Text);
                    newSolution.UserId = user.UserId;
                    newSolution.TechSolutionCode = ProductID;
                    newSolution.TechSolutionFullName = richTextBox1.Text;
                    newSolution.TechSolutionDescription = richTextBox2.Text;
                    newSolution.TechSolutionDataStart = maskedTextBoxStartDate.Text;
                    newSolution.TechSolutionDataEnd = maskedTextBoxEndDate.Text;
                    newSolution.TechSolutionAuthor = GlobalData.user.UserAccount;
                    newSolution.TechSolutionGroupCode = techSolutionCatalogInfo.CatalogCode;
                    GlobalData.BuferDirPath = GlobalData.SelectedDirPath + ProductID + "\\";

                    if (GlobalMethodes.CreateTechSolution(JsonConvert.SerializeObject(newSolution, Formatting.Indented)) == true)//
                    {
                        Thread.Sleep(3000);
                        CancelPosition?.Invoke();
                        CreateProduct?.Invoke();
                    }
                }
            }
            else if (Mode == 1 || Mode == 2)
            {
                string projectId = textBoxProjectNo.Text + "." + textBoxPositionNo.Text;
                List<string> projectIds = RequestInfo.lb.Projects.Select(x => x.ProjectId).ToList();
                GlobalData.BuferDirPath = GlobalData.SelectedDirPath + projectId + "\\";
                if (projectIds.Contains(projectId)) ErroreList.Add("Проект с таким шифром уже существует");
                if (ValidatePositionPanel(2) == true)
                {
                    StartProcess?.Invoke();
                    string responsible = "";
                    if (comboBoxGIP.SelectedIndex > 0) responsible = comboBoxGIP.Text;

                    if (GlobalMethodes.CreateProject(maskedTextBoxStartDate.Text,
                                                    maskedTextBoxEndDate.Text,
                                                    responsible,
                                                    GlobalData.SelectedCountry.NationFullName,
                                                    GlobalData.SelectedCountry.NationName,
                                                    GlobalData.SelectedCountry.NationId,
                                                    richTextBox1.Text,
                                                    richTextBox2.Text,
                                                    projectId) == true)
                    {
                        CancelPosition?.Invoke();
                        CreatePosition?.Invoke();
                    }
                }
                else
                {
                    string Errores = "";
                    foreach (string err in ErroreList)
                    {
                        Errores = Errores + "\n" + err;
                    }
                    MessageBox.Show(Errores, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (Mode == 3)
            {
                try
                {
                    Project parentProject = RequestInfo.lb.Projects.FirstOrDefault(x => x.ProjectId == textBoxProjectNo.Text);
                    Nation nation = RequestInfo.lb.Nations.FirstOrDefault(x => x.NationId == parentProject.NationId);

                    string projectId = textBoxProjectNo.Text + "-" + textBoxPositionNo.Text;
                    List<string> projectIds = RequestInfo.lb.Projects.Select(x => x.ProjectId).ToList();
                    GlobalData.BuferDirPath = GlobalData.SelectedDirPath + projectId + "\\";
                    if (projectIds.Contains(projectId)) ErroreList.Add("Проект с таким шифром уже существует");
                    if (ValidatePositionPanel(2) == true)
                    {
                        StartProcess?.Invoke();
                        string responsible = "";
                        if (comboBoxGIP.SelectedIndex > 0) responsible = comboBoxGIP.Text;

                        if (GlobalMethodes.CreateProject(maskedTextBoxStartDate.Text,
                                                        maskedTextBoxEndDate.Text,
                                                        responsible,
                                                        nation.NationFullName,
                                                        nation.NationName,
                                                        nation.NationId,
                                                        richTextBox1.Text,
                                                        richTextBox2.Text,
                                                        projectId) == true)
                        {
                            if (GlobalMethodes.AddProjectLink(projectId, responsible, textBoxProjectNo.Text))
                            {
                                CancelPosition?.Invoke();
                                CreatePosition?.Invoke();
                            }
                        }
                    }
                    else
                    {
                        string Errores = "";
                        foreach (string err in ErroreList)
                        {
                            Errores = Errores + "\n" + err;
                        }
                        MessageBox.Show(Errores, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch
                {

                }
            }
        }


        private void buttonCancel_Click(object sender, EventArgs e)
        {
            CancelPosition?.Invoke();
        }

        private void usT_CloseButton1_Click(object sender, EventArgs e)
        {
            CancelPosition?.Invoke();
        }

        private string CountSubProjacts()
        {
            string result = "";


            int count = 0;
            foreach (ClassPosition position in GlobalData.PositionList)
            {
                if (GlobalData.SelectedProject.ProjectId == position.ParantProjectID && GlobalData.SelectedStage.StageTag == position.ParantStageTag)
                {
                    string[] pos = position.ID.Split('-');

                    if (pos.Length>1)
                    {
                        char[] posId = pos[1].ToCharArray();

                        if (posId[0] == '0')
                        {
                            if (count < Convert.ToInt32(posId[1].ToString())) count = Convert.ToInt32(posId[1].ToString());
                        }
                        else
                        {
                            if (count < Convert.ToInt32(posId[1].ToString())) count = Convert.ToInt32(posId[1].ToString());
                        } 
                    }
                    
                }

            }

            if (count > 0)
            {
                if (count < 9)
                {
                    result = "0" + (count + 1).ToString();
                }
                else if (count == 9)
                {
                    result = "10";
                }
                else
                {
                    result = (count + 1).ToString();
                }
            }
            else
            {
                result = "00";
            }


            return result;
        }

        private bool ValidatePositionPanel(byte mode)
        {
            bool result = false;
            if (mode == 0)
            {
                if (textBoxPositionNo.Text == "" || textBoxPositionNo.Text == " ")
                {
                    ErroreList.Add("Не указан номер поз.по ГП");
                }

                if (richTextBox1.Text == "")
                {
                    ErroreList.Add("Не указано Наименование");
                }

                if (maskedTextBoxStartDate.Text == "  .  .")
                {
                    ErroreList.Add("Не указана дата Начала");
                }

                if (maskedTextBoxEndDate.Text == "  .  .")
                {
                    ErroreList.Add("Не указана дата Завершения");
                }

                string dateErrore = ValidateDates(maskedTextBoxStartDate.Text, maskedTextBoxEndDate.Text);
                if (dateErrore != "")
                {
                    ErroreList.Add(dateErrore);
                }

                if ((comboBoxGIP.Text == "" || comboBoxGIP.SelectedIndex == 0) && (comboBoxGAP.Text == "" || comboBoxGAP.SelectedIndex == 0))
                {
                    ErroreList.Add("Не указан Руководитель проекта");
                }

                //if (comboBoxGIP.Text == "" || comboBoxGIP.Text == "<Не указано>")
                //{
                //    ErroreList.Add("Не указан ГИП");
                //}

                //if (comboBoxGAP.Text == "" || comboBoxGAP.Text == "<Не указано>")
                //{
                //    ErroreList.Add("Не указан ГАП");
                //}
            }
            else if (mode == 1)
            {
                if (richTextBox1.Text == "")
                {
                    ErroreList.Add("Не заполнено поле Описание");
                }

                if (maskedTextBoxStartDate.Text == "  .  .")
                {
                    ErroreList.Add("Не указана дата Начала");
                }

                if (maskedTextBoxEndDate.Text == "  .  .")
                {
                    ErroreList.Add("Не указана дата Завершения");
                }

                string dateErrore = ValidateDates(maskedTextBoxStartDate.Text, maskedTextBoxEndDate.Text);
                if (dateErrore != "")
                {
                    ErroreList.Add(dateErrore);
                }

                if (comboBoxGIP.Text == "" || comboBoxGIP.Text == "<Не указано>")
                {
                    ErroreList.Add("Не указан Ответственны");
                }
            }
            else if (mode == 2)
            {
                if (textBoxPositionNo.Text == "" || textBoxPositionNo.Text == " ")
                {
                    ErroreList.Add("Не указан Шифр проекта");
                }

                if (richTextBox1.Text == "")
                {
                    ErroreList.Add("Не указано Наименование");
                }

                if (richTextBox1.Text == "")
                {
                    ErroreList.Add("Не указано Описание");
                }

                if (maskedTextBoxStartDate.Text == "  .  ." || maskedTextBoxStartDate.Text == "")
                {
                    ErroreList.Add("Не указана дата Начала");
                }

                if (maskedTextBoxEndDate.Text == "  .  ." || maskedTextBoxStartDate.Text == "")
                {
                    ErroreList.Add("Не указана дата Завершения");
                }

                string dateErrore = ValidateDates(maskedTextBoxStartDate.Text, maskedTextBoxEndDate.Text);
                if (dateErrore != "")
                {
                    ErroreList.Add(dateErrore);
                }

                if (comboBoxGIP.Text == "" || comboBoxGIP.SelectedIndex == 0)
                {
                    ErroreList.Add("Не указан Руководитель проекта");
                }
            }

            if (ErroreList.Count == 0)
            {
                result = true;
            }

            return result;
        }

        private string ValidateDates(string start, string end)
        {
            DateTime startDate = Convert.ToDateTime("01.01.0001 0:00:00");
            DateTime endDate = Convert.ToDateTime("01.01.0001 0:00:00");

            try
            {
                startDate = Convert.ToDateTime(start);
            }
            catch { }
            try
            {
                endDate = Convert.ToDateTime(end);
            }
            catch { }

            if (startDate.ToString() != "01.01.0001 0:00:00" && endDate.ToString() != "01.01.0001 0:00:00")
            {
                int comp = DateTime.Compare(startDate, endDate);
                if (comp == 0)
                {
                    return "Дата завершения проекта совпадает с датой начала";
                }
                else if (comp > 0)
                {
                    return "Дата завершения проекта указана неверно";
                }
            }

            return "";
        }

        private string CountProductInYear()
        {
            string result = "";

            List<ClassProduct> yearProdList = new List<ClassProduct>(GlobalData.ProductList.Where(e => e.Year == DateTime.Now.Year.ToString().Substring(2, 2)));
            int maxNum = 1;
            if (yearProdList.Count > 0) maxNum = yearProdList.Max(n => n.Index) + 1;

            if (maxNum.ToString().Length < 3)
            {
                for(int i = 1; i <= (3-maxNum.ToString().Length); i++)
                {
                    result = result + "0";
                }
                result = result + maxNum.ToString();
            }
            else
            {
                result = maxNum.ToString();
            }
            return result;
        }

        private void textBoxPositionNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = SectionNumOnlyNumber(e);
        }

        private bool SectionNumOnlyNumber(KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && number != 46 && number != 40 && number != 41)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
