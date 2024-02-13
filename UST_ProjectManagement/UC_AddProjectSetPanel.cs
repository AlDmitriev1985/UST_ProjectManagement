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

namespace UST_ProjectManagement
{
    public partial class UC_AddProjectSetPanel : UserControl
    {
        List<string> SetList = new List<string>();

        List<ScheduleItem> fullSubSetList = new List<ScheduleItem>();
        List<ScheduleItem> existSubSetList = new List<ScheduleItem>();
        List<ScheduleItem> addSubSetList = new List<ScheduleItem>();
        List<ClassSet> lvSubSetList = new List<ClassSet>();
        

        public delegate void btnCreate_Click();
        public event btnCreate_Click createSetSchedule;

        public delegate void btnCancel_Click();
        public event btnCancel_Click cancelCreation;

        SetsInfo Info ;

        public UC_AddProjectSetPanel()
        {
            InitializeComponent();
            
            tableLayoutPanel2.BackColor = MainForm.HeaderColor;
            tableLayoutPanel6.BackColor = MainForm.HeaderColor;
            tableLayoutPanel1.RowStyles[0].Height = 25;
            tableLayoutPanel1.RowStyles[tableLayoutPanel1.RowCount - 1].Height = 25;
            tableLayoutPanel6.ColumnStyles[tableLayoutPanel6.ColumnCount - 1].Width = 20;
            //buttonAdd.ForeColor = MainForm.HeaderColor;
            listViewSetList.FullRowSelect = true;
            listViewAddSet.FullRowSelect = true;
        }

        #region --- Events ---

        private void comboBoxGenDis_SelectedIndexChanged(object sender, EventArgs e)
        {
            Info = new SetsInfo();
            GlobalMethodes.UpdateCombaBox(Info.GetSecTowList(comboBoxGenDis.Text, GlobalData.SelectedStage.LanguageId.Value), comboBoxGroupDis);
            comboBoxGroupDis.SelectedIndex = 0;
        }

        private void comboBoxGroupDis_SelectedIndexChanged(object sender, EventArgs e)
        {
            //UpdateListViewSets();
            UpdateSetListView();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (listViewSetList.SelectedItems.Count > 0)
            {
                foreach (ListViewItem _itm in listViewSetList.SelectedItems)
                {
                    ScheduleItem schitem = null;
                    try
                    {
                        schitem = Info.CtreateScheduleItem(_itm.SubItems[1].Text, GlobalData.SelectedStage.LanguageId.Value, fullSubSetList.First());
                    }
                    catch
                    {
                        schitem = Info.CtreateScheduleItem(_itm.SubItems[1].Text, GlobalData.SelectedStage.LanguageId.Value);
                    } 

                    if (schitem != null)
                    {
                        if (!fullSubSetList.Contains(schitem))
                        {
                            fullSubSetList.Add(schitem);
                            addSubSetList.Add(schitem);
                        }
                        else
                        {
                            if (GlobalData.SelectedPosition != null)
                            {
                                Info.GetNewScheduleItemIndex(schitem, fullSubSetList);
                                fullSubSetList.Add(schitem);
                                addSubSetList.Add(schitem); 
                            }
                            else
                            {
                                MessageBox.Show("Данный раздел уже добавлен в продукт/тех.решение.\n" +
                                    "Выберите дугой раздел.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        } 
                    }
                    else
                    {
                        return;
                    }
                }
            }
            UpdateAddListView();
            //UpdateSetListView();
        }

        //private string GetLastIndex(string tag)
        //{
        //    string newtag = "";
        //    int index = 1;
        //    while(newtag == "")
        //    {
        //        if (!fullSubSetDict.ContainsKey(tag + index.ToString()))
        //        {
        //            newtag = tag + index.ToString();
        //        }
        //        else
        //        {
        //            index += 1;
        //        }
        //    }
        //    return newtag;
        //}

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            List<string> errores = new List<string>();
            if (listViewAddSet.SelectedItems.Count > 0)
            {
                foreach (ListViewItem _itm in listViewAddSet.SelectedItems)
                {
                    ScheduleItem removeditem = addSubSetList.FirstOrDefault(x => x.SecThreeTag + x.SecThreePostfix == _itm.SubItems[0].Text);
                    if (removeditem != null)
                    {
                        
                        addSubSetList.Remove(removeditem);
                        fullSubSetList.Remove(removeditem);
                    }
                    else
                    {
                        errores.Add($"{_itm.SubItems[0].Text}");
                    }
                }
                
                if (errores.Count > 0)
                {
                    string txt = "";
                    string endtxt = "";
                    if (errores.Count == 1)
                    {
                        txt = "Раздел: ";
                        endtxt = " был создан ранее.\nЕго нельзя удалить.";
                    }
                    else
                    {
                        txt = "Разделы: ";
                        endtxt = " были созданы ранее.\nИх нельзя удалить.";
                    }
                    for(int i = 0; i < errores.Count; i++)
                    {
                        txt += errores[i];
                        if (i < errores.Count - 1) txt += "; ";
                    }
                    txt += endtxt;

                    MessageBox.Show(txt, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            UpdateAddListView();
            //UpdateSetListView();
        }

        private void listViewSetList_SizeChanged(object sender, EventArgs e)
        {
            listViewSetList.Columns[0].Width = 55;
            listViewSetList.Columns[1].Width = listViewSetList.Width - 55;
        }

        private void listViewAddSet_SizeChanged(object sender, EventArgs e)
        {
            listViewAddSet.Columns[0].Width = 55;
            listViewAddSet.Columns[1].Width = listViewAddSet.Width - 55;
        }
        
        /// <summary>
        /// Сформировать состав проекта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreate_Click(object sender, EventArgs e)
        {
            GlobalData.addSubSetList.Clear();
            foreach (var item in addSubSetList)
            {
                GlobalData.addSubSetList.Add(item);
            }
            createSetSchedule?.Invoke();
            buttonCancel_Click(this.buttonCreate, EventArgs.Empty);
            //if (addSubSetList.Count > 0)
            //{

            //    foreach(var item in addSubSetList)
            //    {
            //        GlobalData.addSubSetList.Add(item);
            //    }   
            //    createSetSchedule?.Invoke();
            //    buttonCancel_Click(this.buttonCreate, EventArgs.Empty); 
            //}
            //else
            //{
            //    DialogResult ask = MessageBox.Show("Не добавлено ни одного раздела.\n" +
            //        "Хотите завершить выбор разделов?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //    if (ask == DialogResult.No)
            //    {
            //        buttonCancel_Click(buttonCancel, EventArgs.Empty);
            //    }
            //}
        }
        
        /// <summary>
        /// Отменить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if ((sender as Button).TabIndex == 1) addSubSetList.Clear();
            }
            catch { }
            listViewAddSet.Items.Clear();
            cancelCreation?.Invoke();
        }

        #endregion

        #region --- Methodes ---
        public void UpdateControlsValues(List<ScheduleItem> existingitems)
        {
            addSubSetList.Clear();
            fullSubSetList.Clear();
            existSubSetList.Clear();

            foreach (var item in GlobalData.addSubSetList)
            {
                fullSubSetList.Add(item);
                addSubSetList.Add(item);
            }
            
            foreach(var item in existingitems)
            {
                fullSubSetList.Add(item);
                existSubSetList.Add(item);
            }

            Info = new SetsInfo();
            SetList = Info.GetSecOneList(GlobalData.SelectedStage.LanguageId.Value);

            GlobalMethodes.UpdateCombaBox(SetList, comboBoxGenDis);
            comboBoxGenDis.SelectedIndex = 0;
        }

        private void UpdateSetListView()
        {
            listViewSetList.Items.Clear();
            Dictionary<string, string> Sets = Info.GetSecThreeList(comboBoxGenDis.Text, comboBoxGroupDis.Text, GlobalData.SelectedStage.LanguageId.Value);

            foreach (var set in Sets)
            {
                ListViewItem lvi = new ListViewItem(set.Key);
                lvi.SubItems.Add(set.Value);
                listViewSetList.Items.Add(lvi);
            }
        }

        /// <summary>
        /// Обновление списка выбранных разделов
        /// </summary>
        public void UpdateAddListView()
        {
            listViewAddSet.Items.Clear();
            listViewAddSet.Groups.Clear();

            Dictionary<string, string> groupsdict = new Dictionary<string, string>();
            var addGroups = fullSubSetList.GroupBy(x => x.SecOneId);
            foreach (var group in addGroups)
            {
                SectionsOne One = RequestInfo.lb.SectionsOnes.FirstOrDefault(x => x.SectionOneId == group.Key);
                if (One != null)
                {
                    string groupName = "";
                    if (GlobalData.SelectedStage.LanguageId != 2)
                    {
                        groupName = One.SectionOneNameRus;
                    }
                    else
                    {
                        groupName = One.SectionOneNameEng;
                    }
                    ListViewGroup gr = new ListViewGroup(groupName);
                    gr.Name = groupName;
                    listViewAddSet.Groups.Add(gr);
                    var sort = group.OrderBy(x => x.SecThreeTag).ThenBy(y => y.SecThreePostfix);
                    
                    foreach (var set in sort)
                    {
                        string tag = set.SecThreeTag + set.SecThreePostfix;
                        string txt = set.SecThreeName;
                        ListViewItem lvi = new ListViewItem(tag);
                        lvi.SubItems.Add(txt);
                        lvi.Group = listViewAddSet.Groups[groupName];
                        listViewAddSet.Items.Add(lvi);

                        if (existSubSetList.Contains(set))
                        {
                            lvi.ForeColor = Color.DarkGray;
                        }
                    }
                }

            }
        }

        public void UpdateAddSetList()
        {
            //if (GlobalData.SelectedPosition != null) addSubSetList = GlobalData.SelectedPosition.SetsList.GetRange(0, GlobalData.SelectedPosition.SetsList.Count);
            //else if (GlobalData.SelectedProduct != null && GlobalData.SelectedStage != null)
            //{
            //    //addSubSetList = GlobalData.SelectedStage.SetsList.GetRange(0, GlobalData.SelectedStage.SetsList.Count);
            //}
            
        }




        #endregion

        
    }
}
