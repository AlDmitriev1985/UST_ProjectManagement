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
using LibraryDB;
using LibraryDB.DB;

namespace UST_ProjectManagement
{
    public partial class UC_ProjectPanel : UserControl
    {
        bool EditInfirmation = false;
        List<Control> leftcontrols;
        List<Control> rightcontrols;
        public UC_ProjectPanel()
        {
            InitializeComponent();
            panelLine01.BackColor = MainForm.HeaderColor;
            flowLayoutPanel1.BackColor = MainForm.HeaderColor;

            leftcontrols = new List<Control>() { label2, label3, label12, label13, label14, label10, label15, label11, label4, label5, label6 };
            rightcontrols = new List<Control>() { label1, labelStage, richTextBoxProjectName, richTextBoxPositionName, textBoxGIP, 
                textBoxGAP, maskedTextBox1, maskedTextBox2, richTextBox1, textBox1, textBox2 };
        }

        public void UpdateProjectPanel()
        {
            List<Project> projects = new List<Project>();          
            List<Project> subprojects = new List<Project>();
            List<Position> positions = new List<Position>();

            List<ProductGroup> productGroups = new List<ProductGroup>();
            List<Product> products = new List<Product>();

            List<TechSolutionGroup> solutionGroups = new List<TechSolutionGroup>();
            List<TechSolution> solutions = new List<TechSolution>();

            List<Stage> stages = new List<Stage>();

            List<DateTime> startDates = new List<DateTime>();
            List<DateTime> endDates = new List<DateTime>();
            startDates.Clear();
            endDates.Clear();
            if (GlobalData.SelectedMainFolderName != null)
            {
                string[] sName = GlobalData.SelectedMainFolderName.Split('_');
                if (sName[0] == "01")
                {
                    projects = RequestInfo.lb.Projects.FindAll(l => l.ProjectLinkId == null);
                    subprojects = RequestInfo.lb.Projects.FindAll(l => l.ProjectLinkId != null);
                    foreach (Project p in projects)
                    {
                        positions.AddRange(RequestInfo.lb.Positions.FindAll(x => x.ProjectId == p.ProjectId).FindAll(s => s.PositionPos != "00").ToArray());
                    }
                    foreach (Project p in subprojects)
                    {
                        positions.AddRange(RequestInfo.lb.Positions.FindAll(x => x.ProjectId == p.ProjectId).FindAll(s => s.PositionPos != "00").ToArray());
                    }             
                }
                else if (sName[0] == "02")
                {
                    products = RequestInfo.lb.Products;
                    productGroups = RequestInfo.lb.ProductGroups;               
                }
                else
                {
                    solutions = RequestInfo.lb.TechSolutions;
                    solutionGroups = RequestInfo.lb.TechSolutionGroups;
                }

                foreach (Control control in leftcontrols)
                {
                    switch (control.TabIndex)
                    {
                        case 1:
                            control.Text = "Шифр:";
                            break;
                        case 2:
                            control.Text = "Наименование:";
                            break;
                        case 3:
                            control.Text = "Общее количество:";
                            break;
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            control.Visible = false;
                            break;
                    }
                }

                foreach (Control control in rightcontrols)
                {
                    switch (control.TabIndex)
                    {
                        case 1:
                            control.Text = sName[0];
                            control.Visible = true;
                            break;
                        case 2:
                            control.Text = sName[1];
                            control.Visible = true;
                            break;
                        case 3:
                            if (sName[0] == "01")
                            {
                                control.Text = "Проектов - " + projects.Count.ToString() + " шт.\n" +
                                                "Поз.по ГП - " + positions.Count.ToString() + " шт.";
                            }
                            else if (sName[0] == "02")
                            {
                                control.Text = "Групп продуктов - " + productGroups.Count.ToString() + " шт." + Environment.NewLine +
                                                "Продуктов - " + products.Count.ToString() + " шт.";
                            }
                            else
                            {
                                control.Text = "Групп тех.решений - " + solutionGroups.Count.ToString() + " шт." + Environment.NewLine +
                                               "Тех.решений - " + solutions.Count.ToString() + " шт.";
                            }
                            control.Visible = true;
                            break;
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            control.Visible = false;
                            break;
                    }
                }
            }

            else if (GlobalData.SelectedCountry != null)
            {
                projects = RequestInfo.lb.Projects.FindAll(p => p.NationId == GlobalData.SelectedCountry.NationId).FindAll(l => l.ProjectLinkId == null);
                subprojects = RequestInfo.lb.Projects.FindAll(p => p.NationId == GlobalData.SelectedCountry.NationId).FindAll(l => l.ProjectLinkId != null);
                foreach (Project p in projects)
                {
                    positions.AddRange(RequestInfo.lb.Positions.FindAll(x => x.ProjectId == p.ProjectId).FindAll(s => s.PositionPos != "00").ToArray());
                }
                foreach (Project p in subprojects)
                {
                    positions.AddRange(RequestInfo.lb.Positions.FindAll(x => x.ProjectId == p.ProjectId).FindAll(s => s.PositionPos != "00").ToArray());
                }
                
                foreach (Control control in leftcontrols)
                {
                    switch (control.TabIndex)
                    {
                        case 1:
                            control.Text = "Шифр:";
                            break;
                        case 2:
                            control.Text = "Наименование:";
                            break;
                        case 3:
                            control.Text = "Общее количество:";
                            break;
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            control.Visible = false;
                            break;
                    }
                }

                foreach (Control control in rightcontrols)
                {
                    switch (control.TabIndex)
                    {
                        case 1:
                            control.Text = GlobalData.SelectedCountry.NationId;
                            control.Visible = true;
                            break;
                        case 2:
                            control.Text = GlobalData.SelectedCountry.NationFullName;
                            control.Visible = true;
                            break;
                        case 3:
                            control.Text = "Проектов - " + projects.Count.ToString() + " шт.\n" +
                                           "Поз.по ГП - " + positions.Count.ToString() + " шт.";
                            control.Visible = true;
                            break;
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            control.Visible = false;
                            break;
                    }
                }
            }

            else if (GlobalData.SelectedProductCatalog != null)
            {
                ProductCatalogInfo productCatalogInfo = new ProductCatalogInfo(GlobalData.SelectedProductCatalog);
                foreach (Control control in leftcontrols)
                {
                    switch (control.TabIndex)
                    {
                        case 1:
                            control.Text = "Шифр:";
                            break;
                        case 2:
                            control.Text = "Наименование:";
                            break;
                        case 3:
                            control.Text = "Общее количество:";
                            break;
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            control.Visible = false;
                            break;
                    }
                }

                foreach (Control control in rightcontrols)
                {
                    switch (control.TabIndex)
                    {
                        case 1:
                            control.Text = productCatalogInfo.CatalogCode;
                            control.Visible = true;
                            break;
                        case 2:
                            control.Text = productCatalogInfo.CatalogName;
                            control.Visible = true;
                            break;
                        case 3:
                            control.Text = "Продуктов - " + productCatalogInfo.Products.Count.ToString() + " шт.";
                            control.Visible = true;
                            break;
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            control.Visible = false;
                            break;
                    }
                }
            }

            else if (GlobalData.SelectedProject != null && GlobalData.SelectedStage == null && GlobalData.SelectedPosition == null)
            {
                var sp = RequestInfo.lb.StageProjects.Where(x => x.ProjectId == GlobalData.SelectedProject.ProjectId).ToList();
                List<Project> linkedProjects = RequestInfo.lb.Projects.FindAll(x => x.ProjectLinkId == GlobalData.SelectedProject.ProjectId);
                string stagesTags = "";
                if (linkedProjects.Count == 0)
                {
                    foreach (var s in sp)
                    {
                        Stage stage = RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == s.StageId);
                        if (stage != null)
                        {
                            if (stagesTags != "")
                            {
                                stagesTags += "; ";
                            }
                            stagesTags += stage.StageTag;
                        }
                    } 
                }
                else
                {
                    for(int i = 0; i < linkedProjects.Count; i++)
                    {
                        stagesTags += linkedProjects[i].ProjectId;
                        if (linkedProjects.Count > 1 && i < linkedProjects.Count - 1)
                        {
                            stagesTags += "; ";
                        }
                    }
                }
                positions = RequestInfo.lb.Positions.FindAll(x => x.ProjectId == GlobalData.SelectedProject.ProjectId);

                foreach (Position position in positions)
                {
                    try
                    {
                        startDates.Add(Convert.ToDateTime(position.PositionDataStart));
                        endDates.Add(Convert.ToDateTime(position.PositionDataEnd));
                    }
                    catch
                    {
                    }
                }

                foreach (Control control in leftcontrols)
                {
                    switch (control.TabIndex)
                    {
                        case 1:
                            control.Text = "Шифр:";
                            control.Visible = true;
                            break;
                        case 2:
                            if (linkedProjects.Count == 0)
                            {
                                control.Text = "Стадии:";
                            }
                            else
                            {
                                control.Text = "Подпроекты:";
                            }
                            control.Visible = true;
                            break;
                        case 3:
                            control.Text = "Описание:";
                            control.Visible = true;
                            break;
                        case 4:
                            control.Text = "Наименование здания:";
                            control.Visible = true;
                            break;
                        case 5:
                            control.Text = "ГИП:";
                            control.Visible = true;
                            break;
                        case 6:
                            control.Text = "ГАП:";
                            control.Visible = true;
                            break;
                        case 7:
                            control.Text = "Дата начала:";
                            control.Visible = true;
                            break;
                        case 8:
                            control.Text = "Дата завершения:";
                            control.Visible = true;
                            break;
                        case 9:
                        case 10:
                        case 11:
                            control.Visible = false;
                            break;
                    }
                }

                foreach (Control control in rightcontrols)
                {
                    switch (control.TabIndex)
                    {
                        case 1:
                            control.Text = GlobalData.SelectedProject.ProjectId;
                            control.Visible = true;
                            break;
                        case 2:
                            control.Text = stagesTags;
                            control.Visible = true;
                            break;
                        case 3:
                            control.Text = GlobalData.SelectedProject.ProjectFullName;
                            control.Visible = true;
                            break;
                        case 4:
                            control.Text = "-";
                            control.Visible = true;
                            break;
                        case 5:
                            try
                            {
                                control.Text = GlobalData.SelectedProject.ProjectGIP;
                            }
                            catch
                            {
                                control.Text = "-";
                            }
                            control.Visible = true;
                            break;
                        case 6:
                            control.Text = "-";
                            control.Visible = true;
                            break;                    
                        case 7:
                            try
                            {
                                control.Text = startDates.Min().ToString();
                            }
                            catch
                            {
                                control.Text = GlobalData.SelectedProject.ProjectDataStart;
                            }
                           
                            control.Visible = true;
                            break;
                        case 8:
                            try
                            {
                                control.Text = endDates.Max().ToString();
                            }
                            catch
                            {
                                control.Text = GlobalData.SelectedProject.ProjectDataEnd;
                            }
                            
                            control.Visible = true;
                            break;
                        case 9:
                        case 10:
                        case 11:
                            control.Visible = false;
                            break;
                    }
                }
            }

            else if (GlobalData.SelectedStage != null && GlobalData.SelectedPosition == null)
            {
                if (GlobalData.SelectedProject == null)
                {
                    GlobalData.SelectedProject = RequestInfo.lb.Projects.FirstOrDefault(p => p.ProjectId == GlobalData.SelectedNode.Parent.Text); 
                }
                if (GlobalData.SelectedProject == null)
                {
                    string productCode = "";
                    productCode += GlobalData.SelectedNode.Parent.Parent.Text;
                    productCode += "-" + GlobalData.SelectedNode.Parent.Text;

                    GlobalData.SelectedProduct = RequestInfo.lb.Products.FirstOrDefault(p => p.ProductCode == productCode);
                    if (GlobalData.SelectedProduct == null)
                    {
                        productCode = GlobalData.SelectedNode.Parent.Text;
                        GlobalData.SelectedTechSolution = RequestInfo.lb.TechSolutions.FirstOrDefault(p => p.TechSolutionCode == productCode);
                    }

                    foreach (Control control in leftcontrols)
                    {
                        switch (control.TabIndex)
                        {
                            case 10:
                                control.Visible = false;
                                break;
                        }
                    }
                    foreach (Control control in rightcontrols)
                    {
                        switch (control.TabIndex)
                        {
                            case 10:
                                control.Visible = false;
                                break;
                        }
                    }

                }
                if (GlobalData.SelectedProject != null)
                {
                    ProjectStageInfo prjInfo = new ProjectStageInfo(GlobalData.SelectedProject, GlobalData.SelectedStage);

                    foreach (Control control in leftcontrols)
                    {
                        switch (control.TabIndex)
                        {
                            case 1:
                                control.Text = "Шифр:";
                                control.Visible = true;
                                break;
                            case 2:
                                control.Text = "Стадия:";
                                control.Visible = true;
                                break;
                            case 3:
                                control.Text = "Описание:";
                                control.Visible = true;
                                break;
                            case 4:
                                control.Text = "Позиции по ГП:";
                                control.Visible = true;
                                break;
                            case 5:
                                control.Text = "ГИП:";
                                control.Visible = true;
                                break;
                            case 6:
                                control.Text = "ГАП:";
                                control.Visible = true;
                                break;
                            case 7:
                                control.Text = "Дата начала:";
                                control.Visible = true;
                                break;
                            case 8:
                                control.Text = "Дата завершения:";
                                control.Visible = true;
                                break;
                            case 9:
                                control.Visible = false;
                                break;
                            case 10:
                                control.Visible = false;
                                break;
                        }
                    }
                    foreach (Control control in rightcontrols)
                    {
                        switch (control.TabIndex)
                        {
                            case 1:
                                control.Text = prjInfo.ProjectId;
                                control.Visible = true;
                                break;
                            case 2:
                                control.Text = prjInfo.StageTag;
                                control.Visible = true;
                                break;
                            case 3:
                                control.Text = prjInfo.StageName;
                                control.Visible = true;
                                break;
                            case 4:
                                control.Text = prjInfo.PositionsTxt;
                                if (control.Text == "") control.Text = "-";
                                control.Visible = true;
                                break;
                            case 5:
                                control.Text = prjInfo.GIPs;
                                if (control.Text == "") control.Text = "-";
                                control.Visible = true;
                                break;
                            case 6:
                                control.Text = prjInfo.GAPs;
                                if (control.Text == "") control.Text = "-";
                                control.Visible = true;
                                break;
                            case 7:
                                control.Text = prjInfo.StartDate;
                                control.Visible = true;
                                break;
                            case 8:
                                control.Text = prjInfo.EndDate;
                                control.Visible = true;
                                break;
                            case 9:
                                control.Visible = false;
                                break;
                            case 10:
                                control.Visible = false;
                                break;
                        }
                    } 
                }
                else if (GlobalData.SelectedProduct != null)
                {
                    ProductStageInfo productStageInfo = new ProductStageInfo(GlobalData.SelectedProduct, GlobalData.SelectedStage);
                    foreach (Control control in leftcontrols)
                    {
                        switch (control.TabIndex)
                        {
                            case 1:
                                control.Text = "Шифр:";
                                control.Visible = true;
                                break;
                            case 2:
                                control.Text = "Стадия:";
                                control.Visible = true;
                                break;
                            case 3:
                                control.Text = "Описание:";
                                control.Visible = true;
                                break;
                            case 4:
                                control.Text = "Разделы:";
                                control.Visible = true;
                                break;
                            case 5:
                                control.Text = "Ответственный:";
                                control.Visible = true;
                                break;
                            case 6:
                                control.Text = "Выполнено:";
                                control.Visible = false;
                                break;
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                            case 11:
                                control.Visible = false;
                                break;
                        }
                    }

                    foreach (Control control in rightcontrols)
                    {
                        switch (control.TabIndex)
                        {
                            case 1:
                                control.Text = productStageInfo.Code;
                                control.Visible = true;
                                break;
                            case 2:
                                control.Text = productStageInfo.StageTag;
                                control.Visible = true;
                                break;
                            case 3:
                                control.Text = productStageInfo.ProductName;
                                control.Visible = true;
                                break;
                            case 4:
                                control.Text = productStageInfo.SectionsTxt;
                                control.Visible = true;
                                break;
                            case 5:
                                control.Text = productStageInfo.GIP;
                                control.Visible = true;
                                break;
                            case 6:
                                control.Text = productStageInfo.PersentComplete;
                                control.Visible = false;
                                break;
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                            case 11:
                                control.Visible = false;
                                break;
                        }
                    }
                }
                else if (GlobalData.SelectedTechSolution != null)
                {
                    TechSulutionStageInfo solutionStageInfo = new TechSulutionStageInfo(GlobalData.SelectedTechSolution, GlobalData.SelectedStage);
                    foreach (Control control in leftcontrols)
                    {
                        switch (control.TabIndex)
                        {
                            case 1:
                                control.Text = "Шифр:";
                                control.Visible = true;
                                break;
                            case 2:
                                control.Text = "Стадия:";
                                control.Visible = true;
                                break;
                            case 3:
                                control.Text = "Описание:";
                                control.Visible = true;
                                break;
                            case 4:
                                control.Text = "Разделы:";
                                control.Visible = true;
                                break;
                            case 5:
                                control.Text = "Ответственный:";
                                control.Visible = true;
                                break;
                            case 6:
                                control.Text = "Выполнено:";
                                control.Visible = false;
                                break;
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                            case 11:
                                control.Visible = false;
                                break;
                        }
                    }

                    foreach (Control control in rightcontrols)
                    {
                        switch (control.TabIndex)
                        {
                            case 1:
                                control.Text = solutionStageInfo.Code;
                                control.Visible = true;
                                break;
                            case 2:
                                control.Text = solutionStageInfo.StageTag;
                                control.Visible = true;
                                break;
                            case 3:
                                control.Text = solutionStageInfo.ProductName;
                                control.Visible = true;
                                break;
                            case 4:
                                control.Text = solutionStageInfo.SectionsTxt;
                                control.Visible = true;
                                break;
                            case 5:
                                control.Text = solutionStageInfo.GIP;
                                control.Visible = true;
                                break;
                            case 6:
                                control.Text = solutionStageInfo.PersentComplete;
                                control.Visible = false;
                                break;
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                            case 11:
                                control.Visible = false;
                                break;
                        }
                    }
                }
            }

            else if (GlobalData.SelectedPosition != null)
            {
                GlobalData.SelectedStage = RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == GlobalData.SelectedPosition.StageId);
                GlobalData.SelectedProject = RequestInfo.lb.Projects.FirstOrDefault(x => x.ProjectId == GlobalData.SelectedPosition.ProjectId);
                PositionInfo positionInfo = new PositionInfo(GlobalData.SelectedProject, GlobalData.SelectedStage, GlobalData.SelectedPosition); 

                foreach (Control control in leftcontrols)
                {
                    switch (control.TabIndex)
                    {
                        case 1:
                            control.Text = "Шифр:";
                            control.Visible = true;
                            break;
                        case 2:
                            control.Text = "Стадия:";
                            control.Visible = true;
                            break;
                        case 3:
                            control.Text = "Описание:";
                            control.Visible = true;
                            break;
                        case 4:
                            control.Text = "Наименование здания:";
                            control.Visible = true;
                            break;
                        case 5:
                            control.Text = "ГИП:";
                            control.Visible = true;
                            break;
                        case 6:
                            control.Text = "ГАП:";
                            control.Visible = true;
                            break;
                        case 7:
                            control.Text = "Дата начала:";
                            control.Visible = true;
                            break;
                        case 8:
                            control.Text = "Дата завершения:";
                            control.Visible = true;
                            break;
                        case 9:
                            control.Text = "Разрабатываемые разделы:";
                            control.Visible = true;
                            break;
                        case 10:
                            control.Text = "Выполнено:";
                            control.Visible = true;
                            break;
                        case 11:
                            control.Text = "Координация:";
                            control.Visible = true;
                            break;
                    }
                }

                foreach (Control control in rightcontrols)
                {
                    switch (control.TabIndex)
                    {
                        case 1:
                            try
                            {
                                control.Text = positionInfo.Code;
                            }
                            catch
                            {
                                control.Text = "-";
                            }
                            control.Visible = true;
                            break;
                        case 2:
                            try
                            {
                                control.Text = positionInfo.StageTag;
                            }
                            catch
                            {
                                control.Text = "-";
                            } 
                            control.Visible = true;
                            break;
                        case 3:
                            try
                            {
                                control.Text = positionInfo.ProjectName;
                            }
                            catch
                            {
                                control.Text = "-";
                            }
                            control.Visible = true;
                            break;
                        case 4:
                            try
                            {
                                control.Text = positionInfo.PositionName;
                            }
                            catch
                            {
                                control.Text = "-";
                            }
                            control.Visible = true;
                            break;
                        case 5:
                            try
                            {
                                control.Text = positionInfo.GIP;
                            }
                            catch
                            {
                                control.Text = "-";
                            }
                            control.Visible = true;
                            break;
                        case 6:
                            try
                            {
                                control.Text = positionInfo.GAP;
                            }
                            catch
                            {
                                control.Text = "-";
                            }
                            control.Visible = true;
                            break;
                        case 7:
                            try
                            {
                                control.Text = positionInfo.StartDate;
                            }
                            catch
                            {
                                control.Text = "";
                            }

                            control.Visible = true;
                            break;
                        case 8:
                            try
                            {
                                control.Text = positionInfo.EndDate;
                            }
                            catch
                            {
                                control.Text = "";
                            }

                            control.Visible = true;
                            break;
                        case 9:
                            control.Text = positionInfo.SectionsTxt;
                            control.Visible = true;
                            break;
                        case 10:
                            control.Text = positionInfo.PersentComplete;
                            if (control.Text == "")
                            {
                                control.Text = "-";
                            }
                            control.Visible = true;
                            break;
                        case 11:
                            control.Text = positionInfo.CoordinationStatus;
                            control.Visible = true;
                            break;
                    }
                }
            }

            else if (GlobalData.SelectedProduct != null)
            {
                var st = RequestInfo.lb.StageProducts.Where(x => x.ProductId == GlobalData.SelectedProduct.ProductId).ToList();
                string txt = "";
                for(int i = 0; i < st.Count; i++)
                {
                    Stage stage = RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == st[i].StageId);
                    if (stage != null)
                    {
                        txt += stage.StageTag;
                        if (i < st.Count - 1)
                        {
                            txt += "; ";
                        }
                    }
                }
                foreach (Control control in leftcontrols)
                {
                    switch (control.TabIndex)
                    {
                        case 1:
                            control.Text = "Шифр:";
                            break;
                        case 2:
                            control.Text = "Наименование:";
                            break;
                        case 3:
                            control.Text = "Описание:";
                            break;
                        case 4:
                            control.Text = "Стадии:";
                            control.Visible = true;
                            break;
                        case 5:
                            control.Text = "Ответственный:";
                            control.Visible = true;
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            control.Visible = false;
                            break;
                    }
                }

                foreach (Control control in rightcontrols)
                {
                    switch (control.TabIndex)
                    {
                        case 1:
                            control.Text = GlobalData.SelectedNode.Text;
                            control.Visible = true;
                            break;
                        case 2:
                            control.Text = GlobalData.SelectedProduct.ProductFullName;
                            control.Visible = true;
                            break;
                        case 3:
                            control.Text = GlobalData.SelectedProduct.ProductDescription;
                            if (control.Text == "") control.Text = "-";
                            control.Visible = true;
                            break;
                        case 4:
                            control.Text = txt;
                            control.Visible = true;
                            break;
                        case 5:
                            control.Text = GetUserFullNameById(GlobalData.SelectedProduct.UserId);
                            control.Visible = true;
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            control.Visible = false;
                            break;
                    }
                }
            }

            else if (GlobalData.SelectedTechSolutionCatalog != null)
            {
                TechSolutionCatalogInfo solutionCatalogInfo = new TechSolutionCatalogInfo(GlobalData.SelectedTechSolutionCatalog);
                foreach (Control control in leftcontrols)
                {
                    switch (control.TabIndex)
                    {
                        case 1:
                            control.Text = "Шифр:";
                            break;
                        case 2:
                            control.Text = "Наименование:";
                            break;
                        case 3:
                            control.Text = "Общее количество:";
                            break;
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            control.Visible = false;
                            break;
                    }
                }

                foreach (Control control in rightcontrols)
                {
                    switch (control.TabIndex)
                    {
                        case 1:
                            control.Text = solutionCatalogInfo.CatalogCode;
                            control.Visible = true;
                            break;
                        case 2:
                            control.Text = solutionCatalogInfo.CatalogName;
                            control.Visible = true;
                            break;
                        case 3:
                            control.Text = "Тех.решений - " + solutionCatalogInfo.TechSolutions.Count.ToString() + " шт.";
                            control.Visible = true;
                            break;
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            control.Visible = false;
                            break;
                    }
                }
            }

            else if (GlobalData.SelectedTechSolution != null)
            {
                var st = RequestInfo.lb.StageTechSolutions.Where(x => x.TechSolutionId == GlobalData.SelectedTechSolution.TechSolutionId).ToList();
                string txt = "";
                for (int i = 0; i < st.Count; i++)
                {
                    Stage stage = RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == st[i].StageId);
                    if (stage != null)
                    {
                        txt += stage.StageTag;
                        if (i < st.Count - 1)
                        {
                            txt += "; ";
                        }
                    }
                }
                if (txt == "") txt = "-";
                foreach (Control control in leftcontrols)
                {
                    switch (control.TabIndex)
                    {
                        case 1:
                            control.Text = "Шифр:";
                            break;
                        case 2:
                            control.Text = "Наименование:";
                            break;
                        case 3:
                            control.Text = "Описание:";
                            break;
                        case 4:
                            control.Text = "Стадии:";
                            control.Visible = true;
                            break;
                        case 5:
                            control.Text = "Ответственный:";
                            control.Visible = true;
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            control.Visible = false;
                            break;
                    }
                }

                foreach (Control control in rightcontrols)
                {
                    switch (control.TabIndex)
                    {
                        case 1:
                            control.Text = GlobalData.SelectedNode.Text;
                            control.Visible = true;
                            break;
                        case 2:
                            control.Text = GlobalData.SelectedTechSolution.TechSolutionFullName;
                            control.Visible = true;
                            break;
                        case 3:
                            control.Text = GlobalData.SelectedTechSolution.TechSolutionDescription;
                            if (control.Text == "") control.Text = "-";
                            control.Visible = true;
                            break;
                        case 4:
                            control.Text = txt;
                            control.Visible = true;
                            break;
                        case 5:
                            control.Text = GetUserFullNameById(GlobalData.SelectedTechSolution.UserId);
                            control.Visible = true;
                            break;
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            control.Visible = false;
                            break;
                    }
                }
            }

            else
            {
                foreach (Control control in leftcontrols)
                {
                    control.Text = "";
                    control.Visible = false;
                }

                foreach (Control control in rightcontrols)
                {
                    control.Text = "";
                    control.Visible = false;
                }
            }       
        }

        private string GetUserFullNameById(int id)
        {
            User user = RequestInfo.lb.Users.FirstOrDefault(x => x.UserId == id);
            if (user != null) return user.UserSurname + " " + user.UserName + " " + user.UserMidlName;
            else return null;
        }

        public void UpdateProjectPanel_Old()
        {
            if (GlobalData.SelectedMainFolderName != null)
            {
                string[] sName = GlobalData.SelectedMainFolderName.Split('_');
                label1.Text = sName[0];
                label3.Text = "Наименование";
                labelStage.Text = sName[1];
                //labelStage.Text = GlobalData.ProjectList.Count.ToString() + " шт.";

                label12.Visible = true; label12.Text = "Общее количество:";
                richTextBoxProjectName.Visible = true;
                if (sName[0] == "01")
                {
                    richTextBoxProjectName.Text = "Проектов - " + GlobalData.ProjectList.Count.ToString() + " шт.\n" +
                                          "Поз.по ГП - " + GlobalData.PositionList.Count.ToString() + " шт.";
                }
                else
                {
                    richTextBoxProjectName.Text = "Продуктов - " + GlobalData.ProductList.Count.ToString() + " шт.";
                }
                label13.Visible = false; richTextBoxPositionName.Visible = false;
                label14.Visible = false; textBoxGIP.Visible = false;
                label10.Visible = false; textBoxGAP.Visible = false;
                label15.Visible = false; maskedTextBox1.Visible = false;
                label11.Visible = false; maskedTextBox2.Visible = false;
                label4.Visible = false; richTextBox1.Visible = false;
            }
            else if (GlobalData.SelectedCountry != null)
            {
                label1.Text = GlobalData.SelectedCountry.NationId;
                label3.Text = "Наименование";
                labelStage.Text = GlobalData.SelectedCountry.NationFullName;

                label12.Visible = false; richTextBoxProjectName.Visible = false;
                label13.Visible = false; richTextBoxPositionName.Visible = false;
                label14.Visible = false; textBoxGIP.Visible = false;
                label10.Visible = false; textBoxGAP.Visible = false;
                label15.Visible = false; maskedTextBox1.Visible = false;
                label11.Visible = false; maskedTextBox2.Visible = false;
                label4.Visible = false; richTextBox1.Visible = false;
            }
            else
            {
                if (GlobalData.SelectedProject != null)
                {
                    label3.Text = "Стадия";
                    label12.Text = "Описание";
                    label14.Text = "ГИП";
                    textBoxGIP.Text = GlobalData.SelectedProject.ProjectGIP;
                    textBoxGAP.Text = "-";
                    maskedTextBox1.Text = GlobalData.SelectedProject.ProjectDataStart;
                    maskedTextBox2.Text = GlobalData.SelectedProject.ProjectDataEnd;

                    label12.Visible = true; richTextBoxProjectName.Visible = true;
                    label13.Visible = true; richTextBoxPositionName.Visible = true;
                    label14.Visible = true; textBoxGIP.Visible = true;
                    label10.Visible = true; textBoxGAP.Visible = true;
                    label15.Visible = true; maskedTextBox1.Visible = true;
                    label11.Visible = true; maskedTextBox2.Visible = true;
                    label4.Visible = true; richTextBox1.Visible = true;


                    label4.Visible = false;

                    if (GlobalData.SelectedStage != null)
                    {
                        labelStage.Text = GlobalData.SelectedStage.StageTag;
                    }
                    else
                    {
                        labelStage.Text = "-";

                    }

                    richTextBoxProjectName.Text = GlobalData.SelectedProject.ProjectFullName;

                    if (GlobalData.SelectedPosition != null)
                    {
                        label1.Text = GlobalData.SelectedPosition.PositionCode;
                        Stage stage = RequestInfo.lb.Stages.FirstOrDefault(x => x.StageId == GlobalData.SelectedPosition.StageId);
                        string stageTag = "-";
                        if (stage != null)
                        {
                            stageTag = stage.StageTag;
                        }
                        labelStage.Text = stageTag;

                        string userGAP = "-";
                        User GAP = RequestInfo.lb.Users.FirstOrDefault(id => id.UserId == GlobalData.SelectedPosition.PositionUserIdGAP);
                        if (GAP != null)
                        {
                            userGAP = GAP.UserName + " " + GAP.UserSurname;
                        }
                        textBoxGAP.Text = userGAP;
                        richTextBoxPositionName.Text = GlobalData.SelectedPosition.PositionName;
                    }
                    else
                    {
                        label1.Text = GlobalData.SelectedProject.ProjectId;
                        richTextBoxPositionName.Text = "-";

                    }

                }
                else if (GlobalData.SelectedProductCatalog != null)
                {
                    if (GlobalData.SelectedProduct != null)
                    {
                        if (GlobalData.SelectedStage != null)
                        {
                            label1.Text = GlobalData.SelectedStage.StageId.ToString();
                            label3.Text = "Стадия";
                            label12.Text = "Наименование";
                            label13.Text = "Описание";
                            label13.Visible = true;
                            label14.Text = "Ответственный";
                            richTextBoxPositionName.Visible = true;
                            labelStage.Text = GlobalData.SelectedStage.StageTag;
                            //try
                            //{
                            //    GlobalData.SelectedProduct = GlobalData.ProductList.Where(id => id.pId == GlobalData.SelectedStage.Pa).First();
                            //}
                            //catch { }

                            if (GlobalData.SelectedProduct != null)
                            {
                                richTextBoxProjectName.Text = GlobalData.SelectedProduct.ProductFullName;
                                richTextBoxPositionName.Text = GlobalData.SelectedProduct.ProductDescription;
                                textBoxGIP.Text = GlobalData.SelectedProduct.User.UserName;
                            }
                        }
                        else
                        {
                            label1.Text = GlobalData.SelectedProduct.ProductCode;
                            label3.Text = "Наименование";
                            labelStage.Text = GlobalData.SelectedProduct.ProductFullName;
                            label12.Text = "Описание";
                            richTextBoxProjectName.Text = GlobalData.SelectedProduct.ProductDescription;
                            label14.Text = "Ответственный";
                            textBoxGIP.Text = GlobalData.SelectedProduct.User.UserName;

                            label12.Visible = true; richTextBoxProjectName.Visible = true;
                            label13.Visible = false; richTextBoxPositionName.Visible = false;
                            label14.Visible = true; textBoxGIP.Visible = true;
                            label10.Visible = false; textBoxGAP.Visible = false;
                            label15.Visible = false; maskedTextBox1.Visible = false;
                            label11.Visible = false; maskedTextBox2.Visible = false;
                            label4.Visible = false; richTextBox1.Visible = false;
                        }
                    }
                    else
                    {
                        label1.Text = GlobalData.SelectedProductCatalog.ProductGroupCode;
                        label3.Text = "Наименование";
                        labelStage.Text = GlobalData.SelectedProductCatalog.ProductGroupFullName;
                        label14.Text = "Ответственный";
                        textBoxGIP.Text = GlobalData.SelectedProductCatalog.User.UserName + " " + GlobalData.SelectedProductCatalog.User.UserSurname;

                        label12.Visible = false; richTextBoxProjectName.Visible = false;
                        label13.Visible = false; richTextBoxPositionName.Visible = false;
                        label14.Visible = true; textBoxGIP.Visible = true;
                        label10.Visible = false; textBoxGAP.Visible = false;
                        label15.Visible = false; maskedTextBox1.Visible = false;
                        label11.Visible = false; maskedTextBox2.Visible = false;
                        label4.Visible = false; richTextBox1.Visible = false;
                    }
                }
                else if (GlobalData.SelectedStage != null)
                {
                    label1.Text = GlobalData.SelectedStage.StageName;
                    label3.Text = "Стадия";
                    label12.Text = "Наименование";
                    label13.Text = "Описание";
                    label13.Visible = true;
                    label14.Text = "Ответственный";
                    richTextBoxPositionName.Visible = true;
                    labelStage.Text = GlobalData.SelectedStage.StageTag;
                    try
                    {
                        //GlobalData.SelectedProduct = GlobalData.ProductList.Where(id => id.ID == GlobalData.SelectedStage.ParantProductID).First();
                    }
                    catch { }

                    if (GlobalData.SelectedProduct != null)
                    {
                        richTextBoxProjectName.Text = GlobalData.SelectedProduct.ProductFullName;
                        richTextBoxPositionName.Text = GlobalData.SelectedProduct.ProductDescription;
                        textBoxGIP.Text = GlobalData.SelectedProduct.User.UserName;
                    }
                }
                else
                {
                    label14.Text = "ГИП";
                    label1.Text = "";
                    richTextBoxProjectName.Text = "";
                    textBoxGIP.Text = "";
                    textBoxGAP.Text = "";
                }
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (EditInfirmation == false)
            {
                EditInfirmation = true;
                richTextBoxPositionName.ReadOnly = false;
                richTextBoxPositionName.BackColor = Color.White;
                richTextBoxPositionName.BorderStyle = BorderStyle.FixedSingle;
                richTextBoxPositionName.Margin = new Padding(3, 3, 3, 3);
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (EditInfirmation == true)
            {
                EditInfirmation = false;
                richTextBoxPositionName.ReadOnly = true;
                richTextBoxPositionName.BackColor = Color.WhiteSmoke;
                richTextBoxPositionName.BorderStyle = BorderStyle.None;
                richTextBoxPositionName_TextChanged(this.richTextBoxPositionName, EventArgs.Empty);
            }           
        }

        private void richTextBoxProjectName_TextChanged(object sender, EventArgs e)
        {
            RichTextBox control = sender as RichTextBox;
            int rows = 0;
            double cL = control.Width;

            foreach(var line in control.Lines)
            {
                double lL = line.Length * 8;
                rows += Convert.ToInt32(Math.Ceiling(lL/cL));
            }

            tableLayoutPanel2.RowStyles[control.TabIndex - 1].Height = 40 + 10 * (rows - 1);
            tableLayoutPanel3.RowStyles[control.TabIndex - 1].Height = 40 + 10 * (rows - 1);
            if (rows < 2)
            {
                control.Margin = new Padding(3, 12, 3, 3);
            }
            else if (rows < 3)
            {
                control.Margin = new Padding(3, 6, 3, 3);
            }
            else
            {
                control.Margin = new Padding(3, 3, 3, 3);
            }
        }

        private void richTextBoxPositionName_TextChanged(object sender, EventArgs e)
        {
            if (EditInfirmation == false)
            {
                if (richTextBoxPositionName.Text.Length > 140)
                {
                    if (richTextBoxPositionName.Text.Length > 280)
                    {
                        richTextBoxPositionName.Margin = new Padding(3, 3, 3, 3);
                    }
                    else
                    {
                        richTextBoxPositionName.Margin = new Padding(3, 15, 3, 3);
                    }

                }
                else
                {
                    richTextBoxPositionName.Margin = new Padding(3, 21, 3, 3);
                }
            }          
        }

    }
}
