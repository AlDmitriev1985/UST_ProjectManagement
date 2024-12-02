using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POSTServer;
using Newtonsoft.Json;
using System.IO;
using LibraryDB;
using POSTServer.History;
using LibraryDB.DB;
using UST_DataGridLibrary;

namespace UST_ProjectManagement
{
    public partial class UC_Approvals : UserControl
    {
        bool Edit = false;
        //List<DataGridViewRow> EditedRows = new List<DataGridViewRow>();
        //Dictionary<string, string> dictEditedRows = new Dictionary<string, string>();
        //Dictionary<string, string> dictEditedRowsStatus = new Dictionary<string, string>();
        //List<string> EditedGroupsList = new List<string>();

        public delegate void ButtonEdit_Click();
        public event ButtonEdit_Click EditStarted;

        public delegate void ButtonFinish_Click();
        public event ButtonFinish_Click EditFinished;

        public delegate void OpenComment_Click();
        public event OpenComment_Click CommentOpen;

        public delegate void OpenHistory_Click(bool open, HistoryLog history);
        public event OpenHistory_Click HistoryOpen;

        public delegate void UpdateHistory_Click(HistoryLog history);
        public event UpdateHistory_Click HistoryUpdate;

        public delegate void StartProcess_Start();
        public event StartProcess_Start StartProcess;

        //List<SectionsThree> secThree = new List<SectionsThree>();

        ComboBox combo;
        int UserRole = 0;
        int Mode = 1;

        PositionInfo positionInfo;

        ContextMenuStrip contextMenu = new ContextMenuStrip();
        ToolStripMenuItem stripItemEdit = new ToolStripMenuItem();
        ToolStripMenuItem stripItemPromote = new ToolStripMenuItem();
        ToolStripMenuItem stripItemDemote = new ToolStripMenuItem();
        ToolStripMenuItem stripItemHistory = new ToolStripMenuItem();

        List<ScheduleItem> EditedItems = new List<ScheduleItem>();
        EditPercentForm percentForm;

        

        public UC_Approvals()
        {
            InitializeComponent();

            stripItemEdit.Text = "Изменить процент выполнения";
            stripItemEdit.Image = Properties.Resources.Btn_Percent_20x20;
            stripItemEdit.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemEdit);

            stripItemPromote.Text = "Согласовать";
            stripItemPromote.Image = Properties.Resources.Promote_25x25;
            stripItemPromote.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemPromote);

            stripItemDemote.Text = "Отклонить";
            stripItemDemote.Image = Properties.Resources.Demote_25x25;
            stripItemDemote.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemDemote);

            stripItemHistory.Text = "История изменений";
            stripItemHistory.Image = Properties.Resources.Btn_Help_25x25;
            stripItemHistory.Click += new EventHandler(toolStripItem_Click);
            contextMenu.Items.Add(stripItemHistory);

            dataGridView_Approvals.ScrollBars = ScrollBars.Vertical;
        }

        private void UC_Approvals_Load(object sender, EventArgs e)
        {
            ToolTip ttTop = new ToolTip();
            ttTop.SetToolTip(button_Edit, "Изменить\n\n" +
                "Изменить процент\n" +
                "выполнения по\n" +
                "закрепленным разделам\n");

            ttTop.SetToolTip(button_Promote, "Утвердить\n\n" +
                "Перевести раздел\n" +
                "из статуса На проверке\n" +
                "в статус Проверено\n");

            ttTop.SetToolTip(button_Demote, "На доработку\n\n" +
                "Вернуть разделы\n" +
                "на доработку\n");

            ttTop.SetToolTip(button_Print, "Выгрузить\n\n" +
                "Выгрузить данные\n" +
                "в формат Excel\n");

            ttTop.SetToolTip(button_Cancel, "Отменить\n\n" +
                "Отменить, внесенные\n" +
                "изменения\n");

            ttTop.SetToolTip(button_Save, "Сохранить\n\n" +
                "Сохранить, внесенные\n" +
                "изменения\n");

           
        }

        public void UpdateButtonsEnabled()
        {
            GetRole();
            if (Edit)
            {
                button_Edit.Visible = false;
                button_Cancel.Visible = true;
                button_Save.Visible = true;
                buttonPercent.Visible = true;
                if(UserRole == 0)
                {
                    button_Promote.Visible = false;
                    button_Demote.Visible = false;
                }
                else
                {
                    button_Promote.Visible = true;
                    button_Demote.Visible = true;
                }
                button_Print.Visible = false;
            }
            else
            {
                button_Edit.Visible = true;
                button_Cancel.Visible = false;
                buttonPercent.Visible = false;
                button_Save.Visible = false;
                button_Promote.Visible = false;
                button_Demote.Visible = false;
                button_Print.Visible = true;
            }
            //GetRole();
            //if (UserRole == 0)
            //{
            //    button_Promote.Visible = false;
            //    button_Demote.Visible = false;
            //}
            //else
            //{
            //    button_Promote.Visible = true;
            //    button_Demote.Visible = true;
            //    if (Edit == false)
            //    {
            //        button_Promote.Image = Properties.Resources.Btn_Promote_35x35_White as Bitmap;
            //        button_Demote.Image = Properties.Resources.Btn_Demote_White_35x35 as Bitmap;
            //    }
            //    else
            //    {
            //        button_Promote.Image = Properties.Resources.Btn_Promote_35x35 as Bitmap;
            //        button_Demote.Image = Properties.Resources.Btn_Demote_35x35 as Bitmap;
            //    }
            //}
        }

        private void dataGridView_Approvals_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var ht = dataGridView_Approvals.HitTest(e.X, e.Y);
                foreach (DataGridViewCell cell in dataGridView_Approvals.SelectedCells)
                {
                    if (ht.RowIndex == cell.RowIndex)
                    {
                        stripItemPromote.Visible = false;
                        stripItemDemote.Visible = false;
                        stripItemEdit.Visible = false;

                        contextMenu.Show(MousePosition);
                        if (cell.Style.BackColor == Color.White && UserRole != 3)
                        {
                            stripItemEdit.Visible = true;
                            if (GetRole() == 0)
                            {
                                stripItemPromote.Visible = false;
                                stripItemDemote.Visible = false;
                            }
                            else
                            {
                                stripItemPromote.Visible = true;
                                stripItemDemote.Visible = true;
                            }
                        }
                        stripItemHistory.Visible = true;
                    }
                }
            }
        }

        private void toolStripItem_Click(object sender, EventArgs args)
        {
            if (sender.ToString() == stripItemEdit.Text)
            {
                buttonPercent_Click(buttonPercent, EventArgs.Empty);
            }
            else if (sender.ToString() == stripItemPromote.Text)
            {
                button_Promote_Click(button_Promote, EventArgs.Empty);
            }
            else if (sender.ToString() == stripItemDemote.Text)
            {
                button_Demote_Click(button_Demote, EventArgs.Empty);
            }
            else if (sender.ToString() == stripItemHistory.Text)
            {
                HistoryLog historyLog = null;
                if (dataGridView_Approvals.SelectedRows.Count > 0 && dataGridView_Approvals.SelectedRows[0].Cells[9].Value != null)
                {
                    try
                    {
                        historyLog = JsonConvert.DeserializeObject<HistoryLog>(dataGridView_Approvals.SelectedRows[0].Cells[9].Value.ToString());
                    }
                    catch { }
                }

                HistoryOpen?.Invoke(true, historyLog);
            }
        }

        private List<ScheduleItem> GetEditedItems()
        {
            List<ScheduleItem> editedItems = new List<ScheduleItem>();
            foreach (DataGridViewRow row in dataGridView_Approvals.SelectedRows)
            {
                ScheduleItem item = positionInfo.scheduleItems.FirstOrDefault(x => x.SecThreeTag + x.SecThreePostfix == row.Cells[1].Value.ToString());
                if (item != null)
                {
                    if (GlobalData.user.FunctionId == 7 && item.Status.StatusId == 3)
                    {
                        editedItems.Add(item);
                    }
                    else if (GlobalData.user.FunctionId < 3 && item.Status.StatusId > 0 && item.Status.StatusId < 3)
                    {
                        editedItems.Add(item);
                    }
                    else
                    {
                        if (GlobalData.UserSets.Contains(row.Cells[0].Value))
                        {
                            editedItems.Add(item);
                        }
                    } 
                }
                
            }
            return editedItems;
        }

        private Status GetStatus(ScheduleItem item)
        {
            int id = -1;
            if (item.Progress < 100) id = 1;
            else if (item.Progress == 100 && item.Status.StatusId == 1) id = 2;
            else if (item.Progress == 100 && item.Status.StatusId == 2) id = 4;
            else if (item.Progress == 100 && item.Status.StatusId == 3 && GetRole() == 3)
            {
                Form_Approve form_Approve = new Form_Approve();
                form_Approve.StartPosition = FormStartPosition.CenterParent;
                if(form_Approve.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        id = RequestInfo.lb.Status.FirstOrDefault(x => x.StatusName == form_Approve.comboBox1.Text).StatusId;
                    }
                    catch 
                    {
                        id = item.Status.StatusId;
                    }
                }
                else
                {
                    id = item.Status.StatusId;
                }
            }
            else if (item.Progress == 100 && item.Status.StatusId == 4 && GetRole() == 2) id = 0;
            Status status = RequestInfo.lb.Status.FirstOrDefault(x => x.StatusId == id);
            return status;
        }


        private void dataGridView_Approvals_SizeChanged(object sender, EventArgs e)
        {
            int summ = 0;
            for(int c = 0; c < dataGridView_Approvals.Columns.Count - 1; c++)
            {
                int w = 0;
                if(c < 2)
                {
                    w = 80;
                }
                else if (c == 3)
                {
                    w = 0;
                }
                else if(c == 4 || c > 5)
                {
                    w = 120;
                }
                else if (c == 5)
                {
                    w = 150;
                }
                summ += w;
                dataGridView_Approvals.Columns[c].Width = w;
            }
            //dataGridView_Approvals.Columns[0].Width = 80;
            //dataGridView_Approvals.Columns[1].Width = 80;          
            //dataGridView_Approvals.Columns[3].Width = 0;
            //dataGridView_Approvals.Columns[4].Width = 100;
            //dataGridView_Approvals.Columns[5].Width = 150;
            //dataGridView_Approvals.Columns[6].Width = 100;
            //dataGridView_Approvals.Columns[7].Width = 100;
            //dataGridView_Approvals.Columns[8].Width = 100;
            
            dataGridView_Approvals.Columns[2].Width = dataGridView_Approvals.Width - summ;
        }

        public void UpdateDG()
        {
            dataGridView_Approvals.Rows.Clear();
            int rowh = 25;

            if (!Edit)
            {
                RequestInfo.requestInfoThree();
                positionInfo = new PositionInfo(GlobalData.SelectedProject, GlobalData.SelectedStage, GlobalData.SelectedPosition);
            }
            if (GlobalData.SelectedPosition != null)
            {
                List<ScheduleItem> sorted = positionInfo.scheduleItems.OrderBy(x => x.SecThreeNum).ToList();
                var secTowGroup = sorted.GroupBy(x => x.SecTowId);
                foreach (var group in secTowGroup)
                {
                    if (group.Key != 1 && group.Key != 38)
                    {
                        DataGridViewRow secTowRow = new DataGridViewRow();
                        secTowRow.CreateCells(dataGridView_Approvals);
                        secTowRow.ReadOnly = true;
                        secTowRow.Height = rowh;
                        var secTow = RequestInfo.lb.SectionsTwoes.FirstOrDefault(x => x.SectionTwoId == group.Key);
                        secTowRow.Cells[0].Value = secTow.SectionTwoNum;
                        if (positionInfo.LanguageId == 7)
                        {
                            secTowRow.Cells[2].Value = secTow.SectionTwoNameEng;
                        }
                        else
                        {
                            secTowRow.Cells[2].Value = secTow.SectionTwoNameRus;
                        }
                        DataGridViewComboBoxCell cbcell = new DataGridViewComboBoxCell();

                        int progress = 0;
                        try
                        {
                            progress = group.Sum(x => x.Progress.Value) / group.Count();
                        }
                        catch { }
                        secTowRow.Cells[4].Value = progress.ToString() + "%";
                        try
                        {
                            Status status = group.FirstOrDefault(x => x.Status.StatusId == group.Min(y => y.Status.StatusId)).Status;
                            secTowRow.Cells[5].Value = status.StatusName;

                            Methodes.ApplyRowBackColor(secTowRow, Color.LightBlue);

                            switch (status.StatusId)
                            {
                                case 0: Methodes.ApplyRowBackColor(secTowRow, Color.Gainsboro); break;
                                case 1: Methodes.ApplyRowBackColor(secTowRow, Color.LightBlue); break;
                                case 2: Methodes.ApplyRowBackColor(secTowRow, Color.Orange); break;
                                case 3: Methodes.ApplyRowBackColor(secTowRow, Color.LightCoral); break;
                                case 4: Methodes.ApplyRowBackColor(secTowRow, Color.MediumSeaGreen); break;
                                case 11: Methodes.ApplyRowBackColor(secTowRow, Color.MediumSeaGreen); break;
                                case 21: Methodes.ApplyRowBackColor(secTowRow, Color.YellowGreen); break;
                            }

                            //secTowRow.Visible = false;
                            dataGridView_Approvals.Rows.Add(secTowRow);

                            //dataGridView_Approvals.DataSource = group;

                            foreach (var section in group)
                            {
                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(dataGridView_Approvals);
                                row.ReadOnly = true;
                                row.Height = rowh;

                                row.Cells[0].Value = section.SecThreeNum;
                                row.Cells[1].Value = section.SecThreeTag + section.SecThreePostfix;
                                row.Cells[2].Value = section.SecThreeName;
                                row.Cells[4].Value = section.Progress + "%";
                                row.Cells[5].Value = section.Status.StatusName;
                                HistoryLog historyLog = null;
                                if (section.History != null)
                                {
                                    try
                                    {
                                        row.Cells[9].Value = section.History;
                                        historyLog = JsonConvert.DeserializeObject<HistoryLog>(section.History);
                                    }
                                    catch { }
                                }
                                if (historyLog != null)
                                {
                                    POSTServer.History.HistoryInfo history = historyLog.spHistory.Last();
                                    try
                                    {
                                        row.Cells[7].Value = (history.Date.Split(' '))[0];
                                        row.Cells[8].Value = history.User;
                                    }
                                    catch
                                    {
                                        row.Cells[7].Value = "";
                                        row.Cells[8].Value = "";
                                    }
                                }
                                else
                                {
                                    row.Cells[7].Value = "";
                                    row.Cells[8].Value = "";
                                }
                                //if(section.SecThreeId == 1 || section.SecThreeId == 65)
                                //{
                                //    row.Visible = false;
                                //}
                                dataGridView_Approvals.Rows.Add(row);

                            }
                        }
                        catch
                        {

                        } 
                    }
                }
            }

            try
            {
                dataGridView_Approvals.Rows[0].Selected = false;
            }
            catch { }
        }

        public int GetRole()
        {
            //0 - User
            //1 - Head
            //2 - GIP
            //GlobalData.UserName = "a.koshelev";
            //ClassUser user = GlobalData.User_FullList.Find(n => n.Account == GlobalData.UserName);
            if (GlobalData.user != null)
            {
                var tt = RequestInfo.lb.Users;
                if (GlobalData.user.FunctionId == 7)
                {
                    UserRole = 3;
                    return 3;
                }
                if (RequestInfo.lb.Users.Where(x => x.DepartmentId == 7 || x.DepartmentId == 17).Select(x => x.UserId).Contains(GlobalData.user.UserId))
                {
                    UserRole = 2;
                    return 2;
                }
                else if (RequestInfo.lb.Departments.Select(x => x.DepartmentHeade).Contains(GlobalData.user.UserId))
                {
                    UserRole = 1;
                    return 1;
                }
                else
                {
                    UserRole = 0;
                    return 0;
                }
            }
            UserRole = 0;
            return 0;        
        }


        private void button_Edit_Click(object sender, EventArgs e)
        {
            GlobalMethodes.ReadSQL_GetUserSets();
            if (GlobalData.UserSets != null && GlobalData.UserSets.Count > 0)
            {
                if (Edit == false)
                {
                    Edit = true;
                    EditedItems.Clear();
                    //UpdateProgressDict();
                    UpdateEnabled();
                    UpdateButtonsEnabled();
                    HistoryOpen?.Invoke(false, null);
                    EditStarted?.Invoke();
                }
            }
            else
            {
                MessageBox.Show("У вас нет прав на редактирование статуса разделов по данному проекту", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateEnabled()
        {
            if (Edit)
            {
                foreach (DataGridViewRow _row in dataGridView_Approvals.Rows)
                {
                    bool canedit = false;
                    string code = _row.Cells[0].Value.ToString();
                    string[] codesplit = code.Split('.');
                    foreach (string _code in GlobalData.UserSets)
                    {
                        if (code == _code)
                        {
                            canedit = true;
                            break;
                        }
                    }
                    if (UserRole == 3 && _row.Cells[5].Value.ToString() == GlobalData.Statuses[3] && codesplit.Length > 2)
                    {
                        canedit = true;
                    }
                    else if (UserRole == 2 && codesplit.Length > 2)
                    {
                        canedit = true;
                    }
                    if (canedit)
                    {
                        foreach (DataGridViewCell _cell in _row.Cells)
                        {
                            _cell.Style.BackColor = Color.White;
                        }
                    }
                    else
                    {
                        foreach (DataGridViewCell _cell in _row.Cells)
                        {
                            if (codesplit.Length > 2)
                            {
                                _cell.Style.BackColor = Color.Gainsboro;
                                _cell.Style.ForeColor = Color.Silver;
                            }
                            else
                            {
                                _cell.Style.BackColor = Color.Silver;
                                _cell.Style.ForeColor = Color.Black;
                            }
                            
                        }
                    }
                }
            }
            else
            {
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            if (Edit == true)
            {
                Edit = false;
                UpdateDG();
                //UpdateDG_Approve();
                UpdateEnabled();
                UpdateButtonsEnabled();
                EditFinished?.Invoke();
            }
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            if (Edit == true)
            {
                try
                {
                    GlobalData.loadInfo = "Обновление информации..";
                    StartProcess?.Invoke();
                    Edit = false;
                    UpdateProgress();
                    UpdateDG();
                    UpdateEnabled();
                    EditFinished();
                    GlobalMethodes._stop = true;
                }
                catch 
                {
                }
            }
        }

        private void UpdateProgress()
        {
            UpdateButtonsEnabled();
            //EditedGroupsList.Clear();
            List<SectionProgress> spProgress = new List<SectionProgress>();
            if (positionInfo == null) positionInfo = new PositionInfo(GlobalData.SelectedProject, GlobalData.SelectedStage, GlobalData.SelectedPosition);
            if (AllSetsFinished())
            {
                foreach (DataGridViewRow _row in dataGridView_Approvals.Rows)
                {
                    //SectionsTwo st = RequestInfo.lb.SectionsTwoes.FirstOrDefault(x => x.SectionTwoNum == _row.Cells[0].Value.ToString());
                    SectionsThree set = positionInfo.ListSecThree.FirstOrDefault(x => x.SectionThreeNum == _row.Cells[0].Value.ToString());
                    //ClassSet set = GlobalData.SelectedPosition.SetsList.FirstOrDefault(x => x.SubSetId == _row.Cells[0].Value.ToString());
                    //ClassSubsetInfo info = GlobalData.SelectedPosition.SubSetInfoList.FirstOrDefault(x => x.SubSetId == _row.Cells[0].Value.ToString());
                    if (set != null)
                    {
                        SectionProgress SP = new SectionProgress();
                        SP.PositionId = positionInfo.ID;
                        SP.Progress = 100;
                        SP.SectionThreeId = set.SectionThreeId;
                        SP.Status = 3;
                        SP.Description = "";
                        string GroupId = set.SectionTwoId.ToString();
                        //if (!EditedGroupsList.Contains(GroupId)) EditedGroupsList.Add(GroupId);
                        spProgress.Add(SP);
                    }
                }
            }
            else
            {
                foreach(ScheduleItem item in EditedItems)
                {
                    SectionProgress SP = new SectionProgress();
                    SP.PositionId = Convert.ToInt32(GlobalData.SelectedPosition.PositionId);
                    SP.Progress = item.Progress.Value;
                    SP.SectionThreeId = item.SecThreeId;
                    SP.Status = item.Status.StatusId;
                    SP.Description = item.Comment;
                    spProgress.Add(SP);

                }
            }

            string txt = JsonConvert.SerializeObject(spProgress, Newtonsoft.Json.Formatting.Indented);
            GlobalMethodes.UpdatePositionProgressByJson(txt);
        }

        /// <summary>
        /// DataGridViewCell выбор контролла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_Walls_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                combo = e.Control as ComboBox;

                if (combo != null)
                {
                    try
                    {
                        combo.SelectedIndexChanged -= new EventHandler(DG_SelectedIndexChanged);
                        combo.SelectedIndexChanged += DG_SelectedIndexChanged;
                    }
                    catch { }
                }
            }
            catch { }
        }

        /// <summary>
        /// Изменение значения ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DG_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Edit == true && combo != null && dataGridView_Approvals != null && dataGridView_Approvals.SelectedRows.Count > 0)
            {
                string id = "";
                string cellvalue = "";
                UpdateSubSetInfoList(out id, out cellvalue);

                if (combo.Text != cellvalue)
                {
                    UpdateDG();
                    UpdateEnabled();

                    foreach (DataGridViewRow _dgr in dataGridView_Approvals.Rows)
                    {
                        if (_dgr.Cells[0].Value.ToString() == id)
                        {
                            _dgr.Selected = true;
                            break;
                        }
                    }
                }

            }
        }

        private void UpdateSubSetInfoList(out string outid, out string outcellvalue)
        {
            outid = "";
            outcellvalue = "";
            foreach (DataGridViewRow _row in dataGridView_Approvals.SelectedRows)
            {
                //foreach (ClassSubsetInfo _info in GlobalData.SelectedPosition.SubSetInfoList)
                //{
                //    if (_info.SubSetId == _row.Cells[0].Value.ToString())
                //    {
                //        outid = _info.SubSetId;
                //        outcellvalue = _row.Cells[3].Value.ToString();
                //        _info.PercentComplete = Convert.ToInt32(combo.Text.Substring(0, combo.Text.Length - 1));
                //        _info.Responsible = GlobalData.UserName;
                //        break;
                //    }
                //}
            }
        }
        /// <summary>
        /// EditProgress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPercent_Click(object sender, EventArgs e)
        {
            try
            {
                List<ScheduleItem> editedItems = GetEditedItems();
                if (editedItems.Count > 0 && editedItems.Max(x => x.Progress) < 100)
                {
                    percentForm = new EditPercentForm();
                    percentForm.numericUpDown1.Value = editedItems.Min(x => x.Progress.Value);
                    percentForm.StartPosition = FormStartPosition.CenterParent;
                    if (percentForm.ShowDialog() == DialogResult.OK)
                    {
                        foreach (ScheduleItem item in editedItems)
                        {
                            if (item.Progress != Convert.ToInt32(percentForm.numericUpDown1.Value))
                            {
                                item.Progress = Convert.ToInt32(percentForm.numericUpDown1.Value);
                                item.Status = GetStatus(item);
                                if (!EditedItems.Contains(item)) EditedItems.Add(item);
                            }

                        }
                        UpdateDG();
                        UpdateEnabled();
                    }
                }
                else
                {
                    if (editedItems.Count == 0)
                    {
                        Form_MessageBox messageBox = new Form_MessageBox("Не выбрано ни одного раздела", "Предупреждение", 0);
                        messageBox.ShowDialog();
                    }
                    else
                    {
                        Form_MessageBox messageBox = new Form_MessageBox("Изменение процента выполнения доступно только для задач со статусом \"В работе\"", "Предупреждение", 0);
                        messageBox.ShowDialog();
                    }
                }
            }
            catch
            {
                Form_MessageBox messageBox = new Form_MessageBox("Что-то пошло нетак. \nОбратитесь в BIM-отдел.", "Ошибка", 0);
                messageBox.ShowDialog();
            }
        }

        private void button_Promote_Click(object sender, EventArgs e)
        {
            try
            {
                if (Edit == true)
                {
                    List<string> selectedid = new List<string>();
                    if (dataGridView_Approvals.SelectedRows.Count > 0)
                    {
                        List<ScheduleItem> editedItems = GetEditedItems();
                        int minprogress = editedItems.Min(x => x.Progress.Value);
                        if (minprogress < 100)
                        {
                            foreach (ScheduleItem item in editedItems)
                            {
                                if (item.Progress < 100)
                                {
                                    selectedid.Add(item.SecThreeTag + item.SecThreePostfix);
                                    item.Progress = 100;
                                    item.Status = GetStatus(item);
                                    if (!EditedItems.Contains(item)) EditedItems.Add(item);
                                }

                            }
                        }
                        else
                        {
                            foreach (ScheduleItem item in editedItems)
                            {
                                selectedid.Add(item.SecThreeTag + item.SecThreePostfix);
                                item.Status = GetStatus(item);
                                if (!EditedItems.Contains(item)) EditedItems.Add(item);
                            }
                        }
                        UpdateDG();
                        UpdateEnabled();
                        foreach (DataGridViewRow _row in dataGridView_Approvals.Rows)
                        {
                            foreach (string id in selectedid)
                            {
                                try
                                {
                                    if (_row.Cells[1].Value.ToString() == id)
                                    {
                                        _row.Selected = true;
                                    }
                                }
                                catch
                                {

                                    _row.Selected = false;
                                }
                            }
                        }
                    }

                }
                else
                {
                    if (UserRole != 0)
                    {
                        Form_MessageBox messageBox = new Form_MessageBox("Для активации команды, перейдите в режим редактирования", "Предупреждение", 0);
                        messageBox.ShowDialog();
                    }
                    else
                    {
                        Form_MessageBox messageBox = new Form_MessageBox("Команда доступна только ГИПам и Руководитлям отделов", "Предупреждение", 0);
                        messageBox.ShowDialog();
                    }
                }
            }
            catch
            {
                Form_MessageBox messageBox = new Form_MessageBox("Что-то пошло нетак. \nОбратитесь в BIM-отдел.", "Ошибка", 0);
                messageBox.ShowDialog();
            }
        }

        private void button_Demote_Click(object sender, EventArgs e)
        {
            try
            {
                if (Edit == true)
                {
                    GlobalData.Comment = "";
                    CommentOpen?.Invoke();
                }
                else
                {
                    if (UserRole != 0)
                    {
                        Form_MessageBox messageBox = new Form_MessageBox("Для активации команды, перейдите в режим редактирования", "Предупреждение", 0);
                        messageBox.ShowDialog();
                    }
                    else
                    {
                        Form_MessageBox messageBox = new Form_MessageBox("Команда доступна только ГИПам и Руководитлям отделов", "Предупреждение", 0);
                        messageBox.ShowDialog();
                    }
                }
            }
            catch
            {
                Form_MessageBox messageBox = new Form_MessageBox("Что-то пошло нетак. \nОбратитесь в BIM-отдел.", "Ошибка", 0);
                messageBox.ShowDialog();
            }
        }

        public void Demote()
        {
            List<string> selectedid = new List<string>();
            if (dataGridView_Approvals.SelectedRows.Count > 0)
            {
                List<ScheduleItem> editedItems = GetEditedItems();
                foreach (ScheduleItem item in editedItems)
                {
                    if (item.Progress == 100)
                    {
                        selectedid.Add(item.SecThreeTag + item.SecThreePostfix);
                        item.Progress = 90;
                        item.Status = GetStatus(item);
                        item.Comment = GlobalData.Comment;
                        if (!EditedItems.Contains(item)) EditedItems.Add(item);
                    }

                }
                UpdateDG();
                UpdateEnabled();
                foreach (DataGridViewRow _row in dataGridView_Approvals.Rows)
                {
                    foreach (string id in selectedid)
                    {
                        if (_row.Cells[0].Value.ToString() == id)
                        {
                            _row.Selected = true;
                        }
                    }
                }
            }
        }

        private bool AllSetsFinished()///ПРоверять только SubSet
        {
            foreach(ScheduleItem item in positionInfo.scheduleItems)
            {
                if (item.SecThreeId != 1 && item.SecThreeId != 65 && item.Status.StatusId != 3 && item.Status.StatusId != 4)
                {
                    return false;
                }
            }
            return true;
        }

        private void dataGridView_Approvals_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView_Approvals_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int index = e.RowIndex;
                string[] sp = dataGridView_Approvals.Rows[index].Cells[0].Value.ToString().Split('.');

                if (sp.Length < 3)
                {
                    dataGridView_Approvals.Rows[index].Selected = false;
                }
                else
                {
                    HistoryLog historyLog = null;
                    if (dataGridView_Approvals.SelectedRows.Count > 0 && dataGridView_Approvals.SelectedRows[0].Cells[9].Value != null)
                    {
                        try
                        {
                            historyLog = JsonConvert.DeserializeObject<HistoryLog>(dataGridView_Approvals.SelectedRows[0].Cells[9].Value.ToString());
                        }
                        catch { }
                    }
                    HistoryUpdate?.Invoke(historyLog);
                }
            }
            catch { }
        }
    }
}
