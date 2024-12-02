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
using LibraryDB;
using LibraryDB.DB;
using UST_DataGridLibrary;

namespace UST_ProjectManagement
{
    public partial class UC_ProjectSchedulePanel : UserControl
    {
        public delegate void btnEdit_Click(PositionInfo positionInfo, ProductStageInfo productInfo, TechSulutionStageInfo solutionInfo);
        public event btnEdit_Click EditSetSchedule;

        public delegate void btnApprove_Click(PositionInfo positionInfo, ProductStageInfo productInfo, TechSulutionStageInfo solutionInfo);
        public event btnApprove_Click ApproveSetSchedule;

        public delegate void SetesApproved_True();
        public event SetesApproved_True SetesApproved;

        public delegate void StartProcess_Start();
        public event StartProcess_Start StartProcess;

        public delegate void btnCancel_Click();
        public event btnCancel_Click CancelChanges;

        public delegate void btnEditSetsList_Click(List<ScheduleItem> scheduleItems);
        public event btnEditSetsList_Click EditSetsList;

        Thread ProcasThread;
        static ProcessForm pForm;
        DataGridView dataGrid;

        //List<SectionsThree> secThree = new List<SectionsThree>();
        List<ScheduleItem> SecList = new List<ScheduleItem>();
        List<ScheduleItem> fullList = new List<ScheduleItem>();

        public PositionInfo positionInfo = null;
        public ProductStageInfo productInfo = null;
        public TechSulutionStageInfo solutionInfo = null;

        ContextMenuStrip contextMenu = new ContextMenuStrip();
        ToolStripMenuItem stripItemEdit = new ToolStripMenuItem();
        ToolStripMenuItem stripItemEditSetList = new ToolStripMenuItem();
        ToolStripMenuItem stripItemChangeDep = new ToolStripMenuItem();
        ToolStripMenuItem stripItemApply = new ToolStripMenuItem();
        ToolStripMenuItem stripItemCancel = new ToolStripMenuItem();
        ToolStripMenuItem stripItemPrint = new ToolStripMenuItem();
        ToolStripMenuItem stripItemHelp = new ToolStripMenuItem();

        EditSetForm setForm;
        byte Mode = 0;

        public UC_ProjectSchedulePanel(byte Mode = 0)
        {
            InitializeComponent();
            UpdateTopBtnEnabled();
            //flowLayoutPanel1.BackColor = MainForm.HeaderColor;

            dataGrid = new DataGridView();
            dataGrid.Dock = DockStyle.Fill;
            dataGrid.BorderStyle = BorderStyle.None;
            dataGrid.Margin = new Padding(0);
            dataGrid.ScrollBars = ScrollBars.Vertical;
            dataGrid.BorderStyle = BorderStyle.None;
            dataGrid.BackgroundColor = Color.WhiteSmoke;    
            dataGrid.SizeChanged += new EventHandler(DG_SizeChanged);
            dataGrid.MouseDown += dataGridView_MouseDown;
            tableLayoutPanel3.Controls.Add(dataGrid, 0, 1);
            List<string> header = new List<string>() { "Шифр", "Обозначение", "Наименование", "Разработал", "Статус", "Tag","Postfix","Department" };
            Methodes.CreateDataGrid(dataGrid, header);
            for(int i = 5; i < dataGrid.Columns.Count; i++)
            {
                dataGrid.Columns[i].Visible = false;
            }
            dataGrid.MultiSelect = false;
            dataGrid.ColumnHeadersVisible = true;
            dataGrid.ColumnHeadersHeight = 50;


            stripItemEdit.Text = "Изменить";
            stripItemEdit.Image = Properties.Resources.btn_Edit_30x30;
            stripItemEdit.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemEdit);

            stripItemApply.Text = "Сохранить изменения";
            stripItemApply.Image = Properties.Resources.Btn_Approve_Grey_30x30;
            stripItemApply.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemApply);

            stripItemEditSetList.Text = "Добавить/Удалить раздел";
            stripItemEditSetList.Image = Properties.Resources.btnAdd_RemoveSett_30x30;
            stripItemEditSetList.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemEditSetList);

            stripItemChangeDep.Text = "Свойства раздела";
            stripItemChangeDep.Image = Properties.Resources.btnEditSelected_30x30;
            stripItemChangeDep.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemChangeDep);

            stripItemCancel.Text = "Отменить";
            stripItemCancel.Image = Properties.Resources.Btn_Cancel_30x30;
            stripItemCancel.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemCancel);

            stripItemPrint.Text = "Экспортировать в Word";
            stripItemPrint.Image = Properties.Resources.btn_Print_30x30;
            stripItemPrint.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemPrint);

            stripItemHelp.Text = "Справка";
            stripItemHelp.Image = Properties.Resources.Btn_Help_30x30;
            stripItemHelp.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemHelp);
        }

        private void UC_ProjectSchedulePanel_Load(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(buttonEdit, stripItemEdit.Text);
            TT.SetToolTip(buttonApprove, stripItemApply.Text);
            TT.SetToolTip(buttonEditeSchedule, stripItemEditSetList.Text);
            TT.SetToolTip(buttonChangeDepartment, stripItemChangeDep.Text);
            TT.SetToolTip(buttonCancel, stripItemCancel.Text);
            TT.SetToolTip(button_Print, stripItemPrint.Text);
            TT.SetToolTip(button_Help, stripItemHelp.Text);
        }

        public void UpdateTopBtnEnabled(bool edit = false, byte mode = 0)
        {
            if (GlobalData.UserRole == "Admin" || GlobalData.UserRole == "Manager")
            {
                buttonEdit.Visible = true;
                buttonApprove.Visible = true;
                buttonEditeSchedule.Visible = true;
                buttonChangeDepartment.Visible = true;
                button_Print.Visible = true;
                buttonCancel.Visible = true;

                if (edit)
                {
                    foreach (Control control in flowLayoutPanel1.Controls)
                    {
                        control.Enabled = true;
                    }
                    buttonEdit.Visible = false;
                    button_Print.Visible = false;
                    if (GlobalData.SelectedPosition == null)
                    {
                        buttonChangeDepartment.Visible = false;
                        //buttonEditeSchedule_Click(buttonEditeSchedule, EventArgs.Empty);
                    }
                    if (mode == 0)
                    {
                        if (GlobalData.SelectedPosition != null)
                        {
                            if (GlobalData.addSubSetList.Count == 0)
                            {
                                buttonChangeDepartment.Enabled = false;
                            }
                            else
                            {
                                buttonChangeDepartment.Enabled = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (Control control in flowLayoutPanel1.Controls)
                        {
                            control.Enabled = false;
                        }
                    }
                }
                else
                {
                    buttonApprove.Visible = false;
                    buttonEditeSchedule.Visible = false;
                    buttonChangeDepartment.Visible = false;
                    buttonCancel.Visible = false;
                } 
            }
            else
            {
                buttonEdit.Visible = false;
                buttonApprove.Visible = false;
                buttonEditeSchedule.Visible = false;
                buttonChangeDepartment.Visible = false;
                button_Print.Visible = true;
                buttonCancel.Visible = false;
            }
        }
       

        private void toolStripItem_Click(object sender, EventArgs args)
        {
            if(sender.ToString() == stripItemEdit.Text)
            {
                buttonEdit_Click(buttonEdit, EventArgs.Empty);
            }
            else if (sender.ToString() == stripItemApply.Text)
            {
                buttonApprove_Click(buttonApprove, EventArgs.Empty);
            }
            else if (sender.ToString() == stripItemChangeDep.Text)
            {
                buttonChangeDepartment_Click(buttonChangeDepartment, EventArgs.Empty);
            }
            else if (sender.ToString() == stripItemEditSetList.Text)
            {
                buttonEditeSchedule_Click(buttonEditeSchedule, EventArgs.Empty);
            }
            else if (sender.ToString() == stripItemCancel.Text)
            {
                buttonCancel_Click(buttonCancel, EventArgs.Empty);
            }
            else if (sender.ToString() == stripItemPrint.Text)
            {
                button_Print_Click(button_Print, EventArgs.Empty);
            }
            else if (sender.ToString() == stripItemHelp.Text)
            {
                button_Help_Click(button_Help, EventArgs.Empty);
            }
        }

        public void DG_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                int columnWidth = 0;
                int summ = 0;
                for (int i = 0; i < dataGrid.Columns.Count; i++)
                {
                    columnWidth = 0;
                    switch (i)
                    {
                        case 0: columnWidth = 100; break;
                        case 1: columnWidth = 200; break;
                        case 4: columnWidth = 150; break;
                    }
                    dataGrid.Columns[i].Width = columnWidth;
                    summ += columnWidth;
                }
                columnWidth = Convert.ToInt32((dataGrid.Width - summ) * 0.3);
                dataGrid.Columns[3].Width = columnWidth;
                summ += columnWidth;
                dataGrid.Columns[2].Width = dataGrid.Width - summ;
            }
            catch
            {
            }
        }

        private void dataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var ht = dataGrid.HitTest(e.X, e.Y);
                foreach (DataGridViewCell cell in dataGrid.SelectedCells)
                {
                    if (ht.RowIndex == cell.RowIndex)
                    {
                        for (int i = 0; i < contextMenu.Items.Count; i++)
                        {
                            if (contextMenu.Items[i].Text == stripItemEdit.Text)
                            {
                                contextMenu.Items[i].Visible = buttonEdit.Visible;
                                contextMenu.Items[i].Enabled = buttonEdit.Enabled;
                            }
                            else if (contextMenu.Items[i].Text == stripItemApply.Text)
                            {
                                contextMenu.Items[i].Visible = buttonApprove.Visible;
                                contextMenu.Items[i].Enabled = buttonApprove.Enabled;
                            }
                            else if (contextMenu.Items[i].Text == stripItemEditSetList.Text)
                            {
                                contextMenu.Items[i].Visible = buttonEditeSchedule.Visible;
                                contextMenu.Items[i].Enabled = buttonEditeSchedule.Enabled;
                            }
                            else if (contextMenu.Items[i].Text == stripItemChangeDep.Text)
                            {
                                contextMenu.Items[i].Visible = buttonChangeDepartment.Visible;
                                contextMenu.Items[i].Enabled = buttonChangeDepartment.Enabled;
                            }
                            else if (contextMenu.Items[i].Text == stripItemCancel.Text)
                            {
                                contextMenu.Items[i].Visible = buttonCancel.Visible;
                                contextMenu.Items[i].Enabled = buttonCancel.Enabled;
                            }
                            else if (contextMenu.Items[i].Text == stripItemPrint.Text)
                            {
                                contextMenu.Items[i].Visible = button_Print.Visible;
                                contextMenu.Items[i].Enabled = button_Print.Enabled;
                            }
                        }

                        contextMenu.Show(MousePosition);
                        break;
                    }
                }
            }
        }

        public void UpdatePrjSchedule()
        {
            if (GlobalData.SelectedPosition == null) positionInfo = null;
            if (GlobalData.SelectedProduct == null) productInfo = null;
            if (GlobalData.SelectedTechSolution == null) solutionInfo = null;

            dataGrid.Rows.Clear();
            SecList.Clear();
            fullList.Clear();

            string Code = "";
            string Discription = "";
            string CreatedBy = "";
            string Status = "";            

            bool languageRus = true;

            List<ScheduleItem> scheduleItems1 = new List<ScheduleItem>();

            if (GlobalData.SelectedPosition != null)
            {
                if (positionInfo == null || positionInfo.Code != GlobalData.SelectedPosition.PositionCode || !positionInfo.SetListeInRelease)
                { 
                    positionInfo = new PositionInfo(GlobalData.SelectedProject, GlobalData.SelectedStage, GlobalData.SelectedPosition); 
                }
                if (positionInfo.LanguageId == 2) languageRus = false;

                foreach (var item in positionInfo.scheduleItems)
                {
                    SecList.Add(item);
                    fullList.Add(item);
                }
                foreach (var additem in GlobalData.addSubSetList)
                {
                    //additem.PositionCode = positionInfo.Code;
                    fullList.Add(additem);
                }

            }
            else if (GlobalData.SelectedProduct != null)
            {
                if (productInfo == null || productInfo.Code != GlobalData.SelectedProduct.ProductCode || !productInfo.SetListeInRelease)
                { 
                    productInfo = new ProductStageInfo(GlobalData.SelectedProduct, GlobalData.SelectedStage); 
                }

                foreach (var item in productInfo.scheduleItems)
                {
                    SecList.Add(item);
                    fullList.Add(item);
                }
                foreach (var additem in GlobalData.addSubSetList)
                {
                    fullList.Add(additem);
                }
            }
            else if (GlobalData.SelectedTechSolution != null)
            {
                if(solutionInfo == null || solutionInfo.Code != GlobalData.SelectedTechSolution.TechSolutionCode || !solutionInfo.SetListeInRelease) 
                {
                    solutionInfo = new TechSulutionStageInfo(GlobalData.SelectedTechSolution, GlobalData.SelectedStage);
                }

                foreach (var item in solutionInfo.scheduleItems)
                {
                    SecList.Add(item);
                    fullList.Add(item);
                }
                foreach (var additem in GlobalData.addSubSetList)
                {
                    fullList.Add(additem);
                }
            }

            List<ScheduleItem> sorted = fullList.OrderBy(x => x.SecThreeNum).ToList();
            var secOneGroup = sorted.GroupBy(x => x.SecOneId);

            foreach (var gr in secOneGroup)
            {
                if (gr.Key != 1 && gr.Key != 17)
                {
                    var secOne = RequestInfo.lb.SectionsOnes.First(x => x.SectionOneId == gr.Key);
                    Code = secOne.SectionOneNum;

                    if (languageRus)
                    {
                        Discription = secOne.SectionOneNameRus;
                    }
                    else
                    {
                        Discription = secOne.SectionOneNameEng;
                    }

                    DataGridViewRow groupRow = new DataGridViewRow();
                    groupRow.CreateCells(dataGrid);
                    groupRow.SetValues(new string[] { Code, "", Discription });
                    groupRow.Height = Settings.RowHeight;
                    Methodes.ApplyRowBackColor(groupRow, Color.LightBlue);
                    dataGrid.Rows.Add(groupRow);
                    List<ScheduleItem> Sets = new List<ScheduleItem>();
                    try
                    {
                        Sets = gr.OrderBy(x => x.SecThreeNum).ThenBy(y => y.SecThreePostfix).ToList();
                    }
                    catch
                    {
                        Sets = gr.OrderBy(x => x.SecThreeNum).ToList();
                    }

                    foreach (var secThr in Sets)
                    {
                        if (SecList.Contains(secThr)) Status = "Утверждено";
                        else Status = "В работе";

                        try
                        {
                            var dep = RequestInfo.lb.Departments.FirstOrDefault(x => x.DepartmentId == secThr.DelegatedDepId);
                            CreatedBy = dep.DepartmentName;
                        }
                        catch
                        {
                            CreatedBy = "";
                        }

                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(dataGrid);
                        row.Height = Settings.RowHeight;
                        row.SetValues(new string[] { secThr.SecThreeNum, $"{secThr.PositionCode} {secThr.SecThreeTag}{secThr.SecThreePostfix}", secThr.SecThreeName, CreatedBy, Status, secThr.SecThreeTag, secThr.SecThreePostfix, secThr.DelegatedDepId.ToString() });
                        dataGrid.Rows.Add(row);
                    } 
                }
            }
        }

        public void ApproveSets()
        {
            if (GlobalData.SelectedPosition != null)
            {
                if (positionInfo == null) positionInfo = new PositionInfo(GlobalData.SelectedProject, GlobalData.SelectedStage, GlobalData.SelectedPosition);
                String PosPath = "";
                String PositionId = positionInfo.ID.ToString();
                String StageId = positionInfo.StageId.ToString();
                String UserId = GlobalData.user.UserId.ToString();
                List<ScheduleItem> SetIdList = new List<ScheduleItem>();
                foreach (var _set in GlobalData.addSubSetList)
                {
                    SetIdList.Add(_set);
                }

                string[] splitPath = GlobalData.SelectedDirPath.Split('\\');
                for (int i = 3; i < splitPath.Length - 1; i++)
                {
                    PosPath += splitPath[i] + "\\";
                }
                int Mode = 0;
                if (positionInfo.SectionsDict.Count > 0) Mode = 1;


                GlobalData.BuferDirPath = GlobalData.SelectedDirPath;
                ///MessageBox.Show($"{SetIdList.Count}; {UserId}");

                if (SetIdList.Count > 0 && UserId != "")
                {
                    if (GlobalMethodes.CreatePositionSets(PosPath, StageId, PositionId, UserId, SetIdList, Mode))
                    {
                        SetesApproved?.Invoke();
                        GlobalData.addSubSetList.Clear();
                    }
                }
            }
            else if (GlobalData.SelectedProduct != null && GlobalData.SelectedStage != null)
            {
                ProductStageInfo productStageInfo = new ProductStageInfo(GlobalData.SelectedProduct, GlobalData.SelectedStage);
                oldSectionsProduct _SecP = new oldSectionsProduct();
                foreach (var _set in GlobalData.addSubSetList)
                {
                    OldSection _Set = new OldSection();
                    _Set.SectionThreeId = _set.SecThreeId;
                    _SecP.spSection.Add(_Set);
                }

                _SecP.Path = "";
                GlobalData.BuferDirPath = GlobalData.SelectedDirPath;
                string[] splitPath = GlobalData.SelectedDirPath.Split('\\');
                for (int i = 3; i < splitPath.Length - 1; i++)
                {
                    _SecP.Path += splitPath[i] + "\\";
                }
                _SecP.ProductId = productStageInfo.ID;
                _SecP.StageId = productStageInfo.StageId;
                _SecP.UserId = GlobalData.user.UserId;
                _SecP.Rus = true;
                _SecP.Mode = 0;
                if (productStageInfo.SectionsDict.Count > 0) _SecP.Mode = 1;


                if (GlobalMethodes.CreateProductSet(JsonConvert.SerializeObject(_SecP, Formatting.Indented)) == true)
                {
                    SetesApproved?.Invoke();
                    GlobalData.addSubSetList.Clear();
                }
            }
            else if (GlobalData.SelectedTechSolution != null && GlobalData.SelectedStage != null)
            {
                TechSulutionStageInfo Info = new TechSulutionStageInfo(GlobalData.SelectedTechSolution, GlobalData.SelectedStage);
                oldSectionsSolution _SecP = new oldSectionsSolution();
                foreach (var _set in GlobalData.addSubSetList)
                {
                    OldSection _Set = new OldSection();
                    _Set.SectionThreeId = _set.SecThreeId;
                    _SecP.spSection.Add(_Set);
                }

                _SecP.Path = "";
                GlobalData.BuferDirPath = GlobalData.SelectedDirPath;
                string[] splitPath = GlobalData.SelectedDirPath.Split('\\');
                for (int i = 3; i < splitPath.Length - 1; i++)
                {
                    _SecP.Path += splitPath[i] + "\\";
                }
                _SecP.TechSolutionId = Info.ID;
                _SecP.StageId = Info.StageId;
                _SecP.UserId = GlobalData.user.UserId;
                _SecP.Rus = true;
                _SecP.Mode = 0;
                if (Info.SectionsDict.Count > 0) _SecP.Mode = 1;


                if (GlobalMethodes.CreateTechSolutionSet(JsonConvert.SerializeObject(_SecP, Formatting.Indented)) == true)
                {
                    SetesApproved?.Invoke();
                    GlobalData.addSubSetList.Clear();
                }
            }
        }

        public void buttonEdit_Click(object sender, EventArgs e)
        {
            EditSetSchedule?.Invoke(positionInfo, productInfo, solutionInfo);
        }

        private void buttonApprove_Click(object sender, EventArgs e)
        {
            ApproveSetSchedule?.Invoke(positionInfo, productInfo, solutionInfo);
        }

        private static string folderpath;
        private void button_Print_Click(object sender, EventArgs e)
        {
            string stageTag = "-";
            string Code = "";

            if (positionInfo != null)
            {
                stageTag = positionInfo.StageTag;
                Code = positionInfo.Code;
            }
            else if (productInfo != null)
            {
                stageTag = productInfo.StageTag;
                Code = productInfo.Code;
            }
            else if (solutionInfo != null)
            {
                stageTag = solutionInfo.StageTag;
                Code = solutionInfo.Code;
            }
            var secOneGroup = SecList.GroupBy(x => x.SecOneId).OrderBy(x => x.Key);
            string filename = "";
            filename = Code + "_" + stageTag + "_СП.doc";
            string filepath = "";
            string temppath = @"Z:\BIM01\01_Библиотеки\07_Word\GEN_Temp.doc";

            using (var dialog = new FolderBrowserDialog())
            {
                if (folderpath != null) dialog.SelectedPath = folderpath;
                dialog.Description = "Укажите папку в которой хотите сохранить состав проекта";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    folderpath = dialog.SelectedPath;
                    filepath = dialog.SelectedPath + @"\" + filename;

                }
            }

            string newfilepath = Class_PrintSetsList.CreateWordFile(filepath, temppath);
            if (newfilepath != "")
            {
                GlobalMethodes._stop = false;
                GlobalData.loadInfo = "Выгузка состава проекта...";
                ProcasThread = new Thread(StartProcessPanel);
                ProcasThread.IsBackground = true;
                ProcasThread.Name = "Start";
                ProcasThread.IsBackground = true;
                ProcasThread.Priority = ThreadPriority.Lowest;
                ProcasThread.Start();

                Class_PrintSetsList.OpenWordFile(newfilepath);
                Class_PrintSetsList.CreateGrid(newfilepath, 4, SecList.Count + 1 + secOneGroup.Count(), SecList);
                Class_PrintSetsList.UpdateStemp();
                GlobalMethodes._stop = true;
            }
        }

        private void CloseProcessForm()
        {
            ProcasThread.Abort();
            ProcasThread.Join();
        }

        private void StartProcessPanel()
        {
            pForm = new ProcessForm();
            pForm._Close += CloseProcessForm;
            pForm.ShowDialog();
        }

        private void buttonChangeDepartment_Click(object sender, EventArgs e)
        {
            if ((positionInfo != null && positionInfo.SetListeInRelease) || (productInfo != null && productInfo.SetListeInRelease) || (solutionInfo != null && solutionInfo.SetListeInRelease))
            {
                if (dataGrid.SelectedRows.Count > 0)
                {
                    int rowindex = 0;
                    DataGridViewRow row = dataGrid.SelectedRows[0];
                    rowindex = row.Index;
                    ScheduleItem item = null;
                    try
                    {
                        item = GlobalData.addSubSetList.Where(x => x.SecThreeTag == row.Cells[5].Value.ToString()).FirstOrDefault(i => i.SecThreePostfix == row.Cells[6].Value.ToString());
                    }
                    catch 
                    {
                        item = GlobalData.addSubSetList.Where(x => x.SecThreeTag == row.Cells[5].Value.ToString()).First();
                    }
                    if (item != null)
                    {
                        setForm = new EditSetForm();
                        setForm.existingitems = fullList.Where(x => x.SecThreeTag == row.Cells[5].Value).ToList();
                        setForm.selecteditem = item;
                        setForm.RefreshControls();
                        setForm.StartPosition = FormStartPosition.CenterParent;
                        DialogResult dialogResult = setForm.ShowDialog();
                        if (dialogResult == DialogResult.OK)
                        {
                            UpdatePrjSchedule();
                            try
                            {
                                dataGrid.Rows[0].Selected = false;
                                dataGrid.Rows[rowindex].Selected = true;
                            }
                            catch
                            {

                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Изменения можно внести только в раздел, который находится в статусе \"В работе\"", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Не выбран раздел проекта, в который нужно внести изменения", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }


            
        }
        /// <summary>
        /// CancelChanges
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            GlobalData.addSubSetList.Clear();
            CancelChanges?.Invoke();
        }

        private void buttonEditeSchedule_Click(object sender, EventArgs e)
        {
            if (positionInfo != null) EditSetsList?.Invoke(SecList);
            else if (productInfo != null) EditSetsList?.Invoke(SecList);
            else if (solutionInfo != null) EditSetsList?.Invoke(SecList);
            UpdateTopBtnEnabled(true, 1);
        }

        private void button_Help_Click(object sender, EventArgs e)
        {
            var url = @"https://kb.unitsky.com/pages/viewpage.action?pageId=69960919";
            System.Diagnostics.Process.Start(url);
        }
    }
}
