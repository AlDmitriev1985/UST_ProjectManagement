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

        public delegate void OpenHistory_Click(bool open);
        public event OpenHistory_Click HistoryOpen;

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
                        if (cell.Style.BackColor == Color.White && UserRole != 3)
                        {
                            contextMenu.Show(MousePosition);
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
            dataGridView_Approvals.Columns[0].Width = 80;
            dataGridView_Approvals.Columns[1].Width = 80;
            dataGridView_Approvals.Columns[2].Width = dataGridView_Approvals.Width - (80 * 2 + 100 * 3 + 150 + 60);
            dataGridView_Approvals.Columns[3].Width = 0;
            dataGridView_Approvals.Columns[4].Width = 100;
            dataGridView_Approvals.Columns[5].Width = 150;
            dataGridView_Approvals.Columns[6].Width = 100;
            dataGridView_Approvals.Columns[7].Width = 100;
            dataGridView_Approvals.Columns[8].Width = 60;
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
                        dataGridView_Approvals.Rows.Add(secTowRow);

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
                                historyLog = JsonConvert.DeserializeObject<HistoryLog>(section.History);
                            }
                            if (historyLog != null)
                            {
                                POSTServer.History.HistoryInfo history = historyLog.spHistory.Last();
                                try
                                {
                                    row.Cells[6].Value = (history.Date.Split(' '))[0];
                                    row.Cells[7].Value = history.User;
                                }
                                catch
                                {
                                    row.Cells[6].Value = "";
                                    row.Cells[7].Value = "";
                                }
                            }
                            else
                            {
                                row.Cells[6].Value = "";
                                row.Cells[7].Value = "";
                            }
                            dataGridView_Approvals.Rows.Add(row);

                        }
                    }
                    catch
                    {

                    }
                }
            }
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
                    HistoryOpen?.Invoke(false);
                    EditStarted?.Invoke();
                }
            }
            else
            {
                MessageBox.Show("У вас нет прав на редактирование статуса разделов по данномк проекту", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    //UpdateDG_Approve();
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
                        //SP.Description = info.Comment;
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
                    spProgress.Add(SP);

                }

                #region --- OLD ---
                //foreach (DataGridViewRow _row in EditedRows)
                //{
                //    SectionsThree set = positionInfo.ListSecThree.FirstOrDefault(x => x.SectionThreeNum == _row.Cells[0].Value.ToString());

                //    foreach (KeyValuePair<string, string> _pair in dictEditedRows)
                //    {
                //        if (_pair.Key == _row.Cells[0].Value.ToString())
                //        {
                //            //ClassSubsetInfo info = GlobalData.SelectedPosition.SubSetInfoList.FirstOrDefault(x => x.SubSetId == _row.Cells[0].Value.ToString());
                //            string secthreeid = set.SectionThreeId.ToString();
                //            string persent = "0";
                //            //foreach (ClassSet _set in GlobalData.SelectedPosition.SetsList)
                //            //{
                //            //    if (_set.SubSetId == _row.Cells[0].Value.ToString())
                //            //    {
                //            //        secthreeid = _set.SubSetTreeId;
                //            //        break;
                //            //    }
                //            //}
                //            persent = _row.Cells[3].Value.ToString().Substring(0, _row.Cells[3].Value.ToString().Length - 1);

                //            KeyValuePair<string, string> statuspair = dictEditedRowsStatus.FirstOrDefault(k => k.Key == _pair.Key);

                //            if (_pair.Value != _row.Cells[3].Value.ToString() || statuspair.Value != _row.Cells[5].Value.ToString())
                //            {
                //                //GlobalMethodes.UpdatePositionProgress(persent, GlobalData.SelectedPosition.pId, secthreeid);
                //                SectionProgress SP = new SectionProgress();
                //                SP.PositionId = Convert.ToInt32(GlobalData.SelectedPosition.PositionId);
                //                SP.Progress = Convert.ToInt32(persent);
                //                SP.SectionThreeId = Convert.ToInt32(secthreeid);
                //                //SP.Description = info.Comment;
                //                if (Mode == 1 && SP.Progress < 100) SP.Status = 1;
                //                else if (Mode == 1 && SP.Progress == 100) SP.Status = 2;
                //                else if (Mode == 4 && SP.Progress == 100) SP.Status = 4;
                //                else SP.Status = 1;

                //                string[] spltid = _pair.Key.Split('.');
                //                string GroupId = spltid[0] + "." + spltid[1];
                //                if (!EditedGroupsList.Contains(GroupId)) EditedGroupsList.Add(GroupId);
                //                spProgress.Add(SP);
                //            }
                //            break;
                //        }
                //    }
                //} 
                #endregion
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
                    MessageBox.Show("Не выбрано ни одного раздела", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Изменение процента выполнения доступно только для задач со статусом \"В работе\"", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void button_Promote_Click(object sender, EventArgs e)
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

                    #region --- Old ---
                    //foreach (ScheduleItem item in editedItems)
                    //{
                    //    if (item.Progress != Convert.ToInt32(percentForm.numericUpDown1.Value))
                    //    {
                    //        item.Progress = Convert.ToInt32(percentForm.numericUpDown1.Value);
                    //        item.Status = GetStatus(item.Progress.Value);
                    //        if (!EditedItems.Contains(item)) EditedItems.Add(item);
                    //    }

                    //}




                    //foreach (DataGridViewRow _row in dataGridView_Approvals.SelectedRows)
                    //{

                    //    //if (UserRole > 0 && UserRole < 3 && EditedRows.Contains(_row))
                    //    //{
                    //    //    ClassSubsetInfo info = GlobalData.SelectedPosition.SubSetInfoList.FirstOrDefault(x => x.SubSetId == _row.Cells[0].Value.ToString());
                    //    //    _row.Cells[3].Value = "100%";
                    //    //    _row.Cells[5].Value = GlobalData.Statuses[4];
                    //    //    info.PercentComplete = 100;
                    //    //    info.Status = GlobalData.Statuses[4];
                    //    //    info.StatusId = 4;
                    //    //    Mode = 4;
                    //    //    if (!selectedid.Contains(_row.Cells[0].Value.ToString()))
                    //    //    {
                    //    //        selectedid.Add(_row.Cells[0].Value.ToString());
                    //    //    }
                    //    //}
                    //    //else if (UserRole == 3 && _row.Cells[5].Value.ToString() == GlobalData.Statuses[3])
                    //    //{
                    //    //    ClassSubsetInfo info = GlobalData.SelectedPosition.SubSetInfoList.FirstOrDefault(x => x.SubSetId == _row.Cells[0].Value.ToString());
                    //    //    _row.Cells[5].Value = GlobalData.Statuses[5];
                    //    //    info.Status = GlobalData.Statuses[5];
                    //    //    info.StatusId = 5;
                    //    //    Mode = 4;
                    //    //    if (!selectedid.Contains(_row.Cells[0].Value.ToString()))
                    //    //    {
                    //    //        selectedid.Add(_row.Cells[0].Value.ToString());
                    //    //    }
                    //    //}
                    //}
                    //UpdateProgress(); 
                    #endregion
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
                    MessageBox.Show("Для активации команды, перейдите в режим редактирования", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Команда доступна только ГИПам и Руководитлям отделов", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button_Demote_Click(object sender, EventArgs e)
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
                    MessageBox.Show("Для активации команды, перейдите в режим редактирования", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Команда доступна только ГИПам и Руководитлям отделов", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
                if (item.Status.StatusId != 3 && item.Status.StatusId != 4)
                {
                    return false;
                }
            }

            
            //foreach (DataGridViewRow _row in dataGridView_Approvals.Rows)
            //{
            //    if (_row.Cells[0].Value.ToString().Length > 5)
            //    {
            //        if (_row.Cells[5].Value.ToString() != GlobalData.Statuses[4] && _row.Cells[5].Value.ToString() != GlobalData.Statuses[3])
            //        {
            //            return false;
            //        }
            //    }
            //}
            return true;
        }
        









        //private void dataGridView_Approvals_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (Edit == true)
        //    {
        //        foreach (DataGridViewRow _row in dataGridView_Approvals.Rows)
        //        {
        //            foreach (ClassSubsetInfo _info in GlobalData.SelectedPosition.SubSetInfoList)
        //            {
        //                if (_info.SubSetId == _row.Cells[1].Value.ToString())
        //                {
        //                    _info.PercentComplete = Convert.ToInt32(_row.Cells[4].Value.ToString().Substring(0, _row.Cells[4].Value.ToString().Length - 1));
        //                    _info.Responsible = GlobalData.UserName;
        //                    break;
        //                }
        //            }

        //        }
        //        UpdateDG_Approve();
        //        UpdateEnabled();
        //    }
        //}
    }
}
