using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UILib = UST_UILibrary.UILibrary;
using LibraryDB.DB;
using System.Xml;
using System.Reflection;
using Newtonsoft.Json;

namespace UST_ProjectManagement
{
    
    public partial class MainForm : Form
    {
        #region --- Position ---
        Point sPoint;
        bool maxform = false;
        public static bool minimized = false;
        public byte windowstate = 0;
        #endregion

        #region --- Settings ---
        public static Color HeaderColor = Color.FromArgb(16, 110, 190);
        public static bool firstStart = true;
        #endregion

        List<Control> TopPanelTabControls = new List<Control>();
        List<Control> NaviPanelTabControls = new List<Control>();

        public static string MainPath = @"Z:\BIM02\";
        public static string MainPath_rezerv = @"z:\BIM02\";

        public static string ConnectionString = @"data source=SW-BIMserver.corp.sw-tech.by;Initial Catalog=Test3;MultipleActiveResultSets=True;User Id = bimworkset; Password = bimadmin";

        //List<string> AdminsList = new List<string>();

        #region  --- Variables ---
        bool NaviPanel = false;
        bool NaviSettPanel = false;
        float NaviPanelStartWidth = 0;
        Point NaviPanelStartPoin;
        int ProjectNaviPanelIndex = 0;

        public bool CreatePanel = false;
        public static int CreatePanelWidth = 250;

        bool CreateDisipline = false;
        #endregion

        public static bool CloseStart = false;

        Thread StratThread;
        Thread ProcasThread;        
        static StartForm sForm;
        static ProcessForm pForm;
        DateTime startTime;
        public static DateTime lastUpdate;

        UC_CoordinationPanel uC_Coordination = new UC_CoordinationPanel();
        UC_Approvals uC_Approve = new UC_Approvals();
        UC_Comments uC_Comments = new UC_Comments();
        UC_HistoryPanel UC_HistoryPanel = new UC_HistoryPanel();
        UC_TaskPanel uC_TaskPanel = new UC_TaskPanel();
        UC_TaskInfo uC_TaskInfo = new UC_TaskInfo();

        public string PathCheck = @"Z:\BIM01\01_Библиотеки\00_Autodesk Revit\13_SkyWay_вкладка\Version\";

        string PathBIMManager = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $@"\SkyWay\BIM Manager";
        //public static string request = @"ustpm:01_Проекты\112_БЕЛАРУСЬ\2.112.190\01_А\2.112.190-00\:4:169:188";
        public static string request;


        public MainForm()
        {
            firstStart = true;
            StartProcess();
            startTime = DateTime.Today;
            lastUpdate = DateTime.Now;


            InitializeComponent();
            System.Threading.Tasks.Task UpdateUpdate = System.Threading.Tasks.Task.Factory.StartNew(UpUpdate);

            try
            {
                RequestInfo.requestInfoThree();
            }
            catch
            {
                MessageBox.Show("Сервер не доступен!");
            }

            panel10.Controls.Add(uC_Coordination);
            uC_Coordination.Dock = DockStyle.Fill;

            panel10.Controls.Add(uC_Approve);
            uC_Approve.Dock = DockStyle.Fill;

            panel12.Controls.Add(uC_Comments);
            uC_Comments.Dock = DockStyle.Fill;

            panel12.Controls.Add(UC_HistoryPanel);
            UC_HistoryPanel.Dock = DockStyle.Fill;

            panel10.Controls.Add(uC_TaskPanel);
            uC_TaskPanel.Dock = DockStyle.Fill;

            panel12.Controls.Add(uC_TaskInfo);
            uC_TaskInfo.Dock = DockStyle.Fill;

            #region --- ReadFromSQL ---




            try
            {
                GlobalData.loadInfo = "Формирование диспетчера проектов...";
                GlobalData.NaviTreeView = new ClassNaviTreeView(MainPath, uC_NavigationPanel2.treeViewNavigation);
            }
            catch
            {
                GlobalData.loadInfo = "Формирование диспетчера проектов...";
                GlobalData.NaviTreeView = new ClassNaviTreeView(MainPath_rezerv, uC_NavigationPanel2.treeViewNavigation);
                ///GlobalMethodes.UpdateNavigationTreeView(uC_NavigationPanel2.treeViewNavigation, MainPath_rezerv);
                //MessageBox.Show($"ThreeViewErrore");

            }
            UpdateCreatePanel();
            if (!UpdateUserAxes(GetUserName()))
            {
                CloseProcessForm();
                this.Close();
            }
            GlobalData.User_GIPList = GetUsersFIObyFunctionId(1);
            GlobalData.User_GAPList = GetUsersFIObyFunctionId(2);


            #region -- Old ---

            ConnectionString = @"data source=SW-BIMserver.corp.sw-tech.by;Initial Catalog=Test3;MultipleActiveResultSets=True;User Id = bimworkset; Password = bimadmin";
            ReadFilters();

            #endregion
            #endregion


            #region --- Test ---
            //try
            //{
            //    GlobalMethodes.UpdateNavigationTreeView(uC_NavigationPanel2.treeViewNavigation, MainPath);
            //}
            //catch
            //{
            //    GlobalMethodes.UpdateNavigationTreeView(uC_NavigationPanel2.treeViewNavigation, MainPath_rezerv);
            //    //MessageBox.Show($"ThreeViewErrore");
            //}

            //UpdateCreatePanel();
            //UpdateUserAxes();
            #endregion

            uC_TopMainPanel1.OpenHomePanel += OpenHomePanel;
            uC_TopMainPanel1.OpenProjectPanel += OpenProjectPanel;
            uC_TopMainPanel1.OpenSearchPanel += OpenSearchPanel;
            uC_TopMainPanel1.OpenAdminPanel += OpenAdminPanel;
            uC_TopMainPanel1.OpenNavisPanel += OpenNavisPanel;
            uC_TopMainPanel1.UpdateForm += UpdateForm;

            uC_TopActionsPanel1.CreatePositionPanel += CreatePositionPanel;
            uC_TopActionsPanel1.CreateStagePanel += CreateStagePanel;
            //uC_TopActionsPanel1.CreateDisciplinePanel += CreateDisiplinePanel;
            uC_TopActionsPanel1.ApproveSetList += ApproveSetsList;
            uC_TopActionsPanel1.ValeraStart += ValeraStart;
            uC_TopActionsPanel1.NWFStart += NWFStart;
            uC_TopActionsPanel1.ValeraApprove += ValeraApprove;
            uC_TopActionsPanel1.NWFApprove += NWFApprove;
            uC_TopActionsPanel1.CancelSetChanges += CancelSetChanges;
            uC_TopActionsPanel1.EditSetsList += EditSetsList;

            uC_StartPanel1.UpdateForm += UpdateStartForm;

            uC_AdmiPanel1.ValeraStart += ValeraStart;
            uC_AdmiPanel1.NWFStart += NWFStart;
            uC_AdmiPanel1.ValeraApprove += ValeraApprove;
            uC_AdmiPanel1.NWFApprove += NWFApprove;

            uC_SearchPanel1.OpenProjectPanel += OpenProjectPanel;
            //uC_SearchPanel1.SearchThreeNode += SearchProjectThreeNode;

            uC_NavigationPanel2.OpenProjectPanel += OpenProjectPanel;
            //uC_NavigationPanel2.OpenProjectPanel += OpenActionPanel;
            uC_NavigationPanel2.OpenHomePanel += OpenHomePanel;
            uC_NavigationPanel2.ThreeNodeCick += uC_NavigationPanel2_MouseClick;
            uC_NavigationPanel2.CreatePosition += CreatePositionPanel;
            uC_NavigationPanel2.CreateStage += CreateStagePanel;
            uC_NavigationPanel2.CreateProduct += CreatePositionPanel;
            uC_NavigationPanel2.CreateTechSolution += CreatePositionPanel;
            uC_NavigationPanel2.CreateProject += CreatePositionPanel;
            uC_NavigationPanel2.EditSetsList += EditSetsList;
            uC_NavigationPanel2.CreateProductAsProject += CreatePositionPanel;

            uC_CreateStagePanel1.CancelStagePanel += CreateStagePanel;
            uC_CreateStagePanel1.CreateStage += CreateStageDirPath;
            uC_CreateStagePanel1.StartProcess += StartProcess;


            uC_CreatePositionPanel1.CreatePosition += CreatePosition;
            uC_CreatePositionPanel1.CancelPosition += CancelCreatePosition;
            uC_CreatePositionPanel1.CreateProduct += CreateProduct;
            uC_CreatePositionPanel1.StartProcess += StartProcess;

            uC_ProjectNaviPanel1.mainPanel += openPosMainPanel;
            uC_ProjectNaviPanel1.schedulePanel += openPosSchedulePanel;
            uC_ProjectNaviPanel1.coordinationPanel += openCoordoinationPanel;
            uC_ProjectNaviPanel1.approvepanel += openApprovalsPanel;
            uC_ProjectNaviPanel1.taskpanel += openTaskPanel;
            //uC_NaviSettingsPanel1.mainPanel += openPosMainPanel;
            //uC_NaviSettingsPanel1.infoPanel += openPosInfoPanel;
            //uC_NaviSettingsPanel1.schedulePanel += openPosSchedulePanel;

            uC_AddProjectSetPanel1.createSetSchedule += UpdateSetSchedule;
            uC_AddProjectSetPanel1.cancelCreation += CancelSetCreation;

            uC_ProjectSchedulePanel1.EditSetSchedule += EditSetsListBtnClick;
            uC_ProjectSchedulePanel1.ApproveSetSchedule += ApproveSetsListBtnClick;
            uC_ProjectSchedulePanel1.SetesApproved += SetesApproved;
            uC_ProjectSchedulePanel1.CancelChanges += CancelSetChanges;
            uC_ProjectSchedulePanel1.EditSetsList += EditeSetsList;

            uC_AdmiPanel1.SelectStartBtn += AdminCanStart;
            uC_AdmiPanel1.UnSelectStartBtn += AdminCanNotStart;

            uC_Approve.EditStarted += StartEdit;
            uC_Approve.EditFinished += FinishEdit;
            uC_Approve.CommentOpen += OpenCommentPanel;
            uC_Approve.HistoryOpen += OpenSetHistoryPanel;
            uC_Approve.StartProcess += StartProcess;
            UC_HistoryPanel.HistoryOpen += OpenSetHistoryPanel;

            uC_Comments.Cancel += CloseCommentPanel;
            uC_Comments.Apply += ApplyCommentPanel;

            uC_TaskPanel.TaskOpen += OpenTaskInfo;
            uC_TaskInfo.OpenTaskRoute += OpenTaskRoute;
            uC_TaskInfo.ChangeTaskStatus += ChangeTaskStatus;
            uC_TaskPanel.EditTask += EditTask;

            //try
            //{
            //    uC_StartPanel1.StartProcess += StartProcess;
            //}
            //catch
            //{
            //}

            //sForm._Close += CloseStartForm;
            

            #region --- StartValues ---
            tableLayoutPanel5.ColumnStyles[2].Width = 0;
            tableLayoutPanel5.ColumnStyles[3].Width = 0;
            usT_HorizontalTab_MainPanel_Click(this.usT_HorizontalTab_MainPanel, EventArgs.Empty);
            usT_VerticalTabControl1_Click(this.usT_VerticalTabControl1, EventArgs.Empty);

            #endregion

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);

            #region --- PanelesColor ---
            RefreshPanelsBackColor();

            #endregion

            #region --- ControlsLists ---
            UILib.GetAllTypedControls(tableLayoutPanel4, TopPanelTabControls, typeof(Control));
            UILib.GetAllTypedControls(tableLayoutPanel6, NaviPanelTabControls, typeof(Control));

            #endregion
           
;

            
        }

        private void UpUpdate()
        {
            Random chislo = new Random();
            try
            {
                string path1 = Path.Combine($@"Z:\BIM01\01_Библиотеки\00_Autodesk Revit\13_SkyWay_вкладка\Version", @"UpdateProg.exe");
                string path2 = Path.Combine(PathBIMManager, @"UpdateProg.exe");
                FileInfo fi1 = new FileInfo(path1);
                FileInfo fi2 = new FileInfo(path2);

                if (!fi2.Exists)
                {
                    fi1.CopyTo(Path.Combine(PathBIMManager, @"UpdateProg.exe"));
                }
                else
                {
                    DateTime t1 = fi1.LastWriteTime;
                    DateTime t2 = fi2.LastWriteTime;

                    if (t1 > t2)
                    {

                        fi1.CopyTo(Path.Combine(PathBIMManager, @"UpdateProg.exe"), true);
                    }
                }
            }
            catch
            {

            }

            ReadCheckXMLUpdateManager();
        }

        private void RefreshPanelsBackColor()
        {
            List<Control> controls = new List<Control>();
            GetControls(this, controls);
            foreach (Control control in controls)
            {
                try
                {
                    Panel panel = control as Panel;
                    if (panel.BackColor == Color.SteelBlue)
                    {
                        panel.BackColor = MainForm.HeaderColor;
                    }
                }
                catch
                {

                }
            }
        }

        private void GetControls(Control control, List<Control> controls)
        {

            if (control.Controls.Count > 0)
            {
                foreach (Control ctrl in control.Controls)
                {
                    controls.Add(ctrl);
                    if (ctrl.Controls.Count > 0)
                    {
                        GetControls(ctrl, controls);
                    }
                }
            }
        }

        private void ReadFilters()
        {
            string txt = GlobalMethodes.ReadFromJSON(GlobalData.TempDirPath, GlobalData.FiltersFileName);
            if (txt != null && txt != "")
            {
                GlobalData.FilterList.Clear();
                GlobalData.FilterList = JsonConvert.DeserializeObject<List<string[,]>>(txt);
                //uC_StartPanel1.UpdatePanels(GlobalData.FilterList[0]);
                //uC_StartPanel1.radioButton_CheckedChanged(uC_StartPanel1.radioButton1, EventArgs.Empty);
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    string[,] filter = new string[2, GlobalData.Filters0.Count];
                    for (int c = 0; c < filter.Length; c++)
                    {
                        try
                        {
                            filter[1, i] = "<Нет>";
                        }
                        catch
                        {
                        }
                    }
                    GlobalData.FilterList.Add(filter);
                }
            }
        }

        private void ReadCheckXMLUpdateManager()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(PathCheck + "check.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            //MessageBox.Show(xRoot.Attributes[0].InnerText.Replace(".", ""));
            int NewVer = Convert.ToInt32(xRoot.Attributes[0].InnerText.Replace(".", ""));
            //MessageBox.Show(NewVer + " " + ver);

            //AssemblyName ass = System.Reflection.Assembly.GetExecutingAssembly().GetName();
            Assembly ass = Assembly.Load(System.IO.File.ReadAllBytes(Path.Combine(PathBIMManager, @"SkyWayBIM Manager.exe")));
            var ver = Convert.ToInt32(ass.GetName().Version.ToString().Replace(".", ""));

            if (NewVer > ver)
            {
                StartUpdate();
            }
        }

        private void StartUpdate()
        {
            AssemblyName ass = Assembly.Load(System.IO.File.ReadAllBytes(Path.Combine(PathBIMManager, @"SkyWayBIM Manager.exe"))).GetName();
            string name = ass.Name + ".exe";
            FileInfo fi = new FileInfo(PathCheck + name);
            //MessageBox.Show(Application.StartupPath + @"\new." + name);
            fi.CopyTo(PathBIMManager + @"\new." + name, true);

            Process.Start(PathBIMManager + @"\UpdateProg.exe");
            //MessageBox.Show(Application.StartupPath + @"\UpdateProg.exe");
            //Process.GetCurrentProcess().Kill();

            System.Diagnostics.Process[] local_procs = System.Diagnostics.Process.GetProcesses();
            var target_proc = local_procs.Where(p => p.ProcessName == "SkyWayBIM Manager");
            foreach (var proc in target_proc)
            {
                proc.Kill();
            }
        }

        private List<string> GetUsersFIObyFunctionId(int id)
        {
            List<string> FIO = new List<string>();
            List<User> users = RequestInfo.lb.Users.Where(x => x.FunctionId == id).Where(l => l.LifeId != 2).OrderBy(fn => fn.UserSurname).ToList();
            foreach(User user in users)
            {
                FIO.Add($"{user.UserSurname} {user.UserName} {user.UserMidlName}");
            }
            return FIO;
        }


        private void CloseStartForm()
        {
            DateTime nowTime = DateTime.Today;
            while (startTime == nowTime)
            {
                Thread.Sleep(36000000);
                CloseStartForm();
            }

            System.Diagnostics.Process[] local_proc = System.Diagnostics.Process.GetProcesses();
            var target_proc = local_proc.Where(p => p.ProcessName == "UST_ProjectManagement");
            GlobalMethodes.CreateLog("Принудительное заткрытие программы");
            foreach (Process pr in target_proc)
            {
                pr.Kill();
            }
            
            //StratThread.Abort();
            //StratThread.Join();
        }
        
        public void CloseProcessForm()
        {
            try
            {
                ProcasThread.Abort();
                ProcasThread.Join();
            }
            catch
            {
            }
        }

        public void StartProcessPanel()
        {
            try
            {
                pForm = new ProcessForm();
                //pForm._Close += CloseProcessForm;
                pForm.ShowDialog();
            }
            catch
            {
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GlobalMethodes.CreateLog("Открытие программы");
            labelUserName.Text = GetUserName();
            OpenHomePanel();
            tableLayoutPanel2_Click(this.tableLayoutPanel2, EventArgs.Empty);
            RefreshForm();
            GlobalMethodes._stop = true;
            if(GlobalData.UserRole == "Admin" || GlobalData.UserRole == "Manager")
            {
                usT_HorizontalTab_ActionsPanel.Visible = true;
            }
            else
            {
                usT_HorizontalTab_ActionsPanel.Visible = false;
            }
            WindowState = FormWindowState.Normal;

            if (request != null && request != "")
            {
                //MessageBox.Show(request);
                try
                {
                    ExecuteRequest();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                    throw;
                }
            }
        }

        private void ExecuteRequest()
        {
            this.Visible = false;
            string[] spreques = request.Split(':');
            GlobalData.SelectedDirPath = spreques[1].Trim();
            if (GlobalData.NaviTreeView.SelectTreeNode_New(1) && GlobalData.SelectedPosition != null)
            {
                switch (spreques[2])
                {
                    case "0":
                        uC_ProjectNaviPanel1.usT_HorizontalTabControl1_Click(uC_ProjectNaviPanel1.usT_HorizontalTabControl1, EventArgs.Empty);
                        break;
                    case "1":
                        uC_ProjectNaviPanel1.usT_HorizontalTabControl2_Click(uC_ProjectNaviPanel1.usT_HorizontalTabControl2, EventArgs.Empty);
                        break;
                    case "2":
                        uC_ProjectNaviPanel1.usT_HorizontalTabControl3_Click(uC_ProjectNaviPanel1.usT_HorizontalTabControl3, EventArgs.Empty);
                        break;
                    case "3":
                        uC_ProjectNaviPanel1.usT_HorizontalTabControl4_Click(uC_ProjectNaviPanel1.usT_HorizontalTabControl4, EventArgs.Empty);
                        break;
                    case "4":
                        uC_ProjectNaviPanel1.usT_HorizontalTabControl5_Click(uC_ProjectNaviPanel1.usT_HorizontalTabControl5, EventArgs.Empty);

                        if (uC_TaskPanel.SelectTaskCell(spreques[3], spreques[4]))
                        {
                            try
                            {
                                if (uC_TaskInfo.SelectRowByTdIdAndOpenTask(spreques[4]))
                                {
                                    this.Visible = true;
                                    uC_TaskInfo.button4_Click(uC_TaskInfo.button4, EventArgs.Empty);
                                }
                            }
                            catch { }
                            
                        }
                        break;
                }

                
            }
            this.Visible = true;
            //List<TreeNode> existingNodes = uC_StartPanel1.SearchTreeNodes(spreques[1], GlobalData.NaviTreeView.View.Nodes[0], 0);
            //GlobalData.NaviTreeView.View.SelectedNode = existingNodes.First();
            //GlobalData.NaviTreeView.View.Focus();
        }

        private void RefreshForm()
        {
            this.BringToFront();
            //GlobalData.NaviTreeView.SelectTreeNode(GlobalData.NaviTreeView.mainFolders[0]);
            this.uC_NavigationPanel2.Focus();
        }

        private void UpdateForm()
        {
            StartProcess();
            lastUpdate = DateTime.Now;

            InitializeComponent();
            System.Threading.Tasks.Task UpdateUpdate = System.Threading.Tasks.Task.Factory.StartNew(UpUpdate);

            try
            {
                RequestInfo.requestInfoThree();
                try
                {
                    GlobalData.loadInfo = "Формирование диспетчера проектов...";
                    RequestInfo.requestInfoThree();
                    GlobalData.NaviTreeView.CreateTreeView_New();
                    GlobalData.SelectedDirPath = GlobalData.BuferDirPath;
                    GlobalData.NaviTreeView.SelectTreeNode_New();
                    GlobalMethodes._stop = true;
                }
                catch
                {

                }

            }
            catch
            {
                MessageBox.Show("Сервер не доступен!");
            }
            GlobalMethodes._stop = true;
        }


        #region --- FormEvents ---
        private void usT_CloseButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ThisClick(object sender, EventArgs e)
        {
            
            StratThread.Abort();
            MainForm.CheckForIllegalCrossThreadCalls = true;
        }
        private void usT_MaximizeButton1_Click(object sender, EventArgs e)
        {
            if (maxform == false)
            {
                Rectangle rect = Screen.FromHandle(this.Handle).WorkingArea;
                rect.Location = new Point(0, 0);
                this.MaximizedBounds = rect;
                this.WindowState = FormWindowState.Maximized;

                //his.WindowState = FormWindowState.Maximized;
                maxform = true;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                maxform = false;
            }
        }

        private void usT_MinimizeButton1_Click(object sender, EventArgs e)
        {
            minimized = true;
            this.WindowState = FormWindowState.Minimized;
        }

        private void tableLayoutPanel2_MouseDown(object sender, MouseEventArgs e)
        {
            sPoint = new Point(e.X, e.Y);
        }

        private void tableLayoutPanel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - sPoint.X;
                this.Top += e.Y - sPoint.Y;
            }
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            sPoint = new Point(e.X, e.Y);
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Width += e.X - sPoint.X;
            }
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            sPoint = new Point(e.X, e.Y);
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Height += e.Y - sPoint.Y;
            }
        }

        #endregion


        #region --- TopPanelEvents ---
        public static string GetUserName()
        {
            string[] user = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\');
            string account = user[user.Length - 1];
            return account.ToLower();
        }

        private void usT_HorizontalTab_MainPanel_Click(object sender, EventArgs e)
        {
            UpdateUSTButtonsStatus(usT_HorizontalTab_MainPanel);
            uC_TopMainPanel1.BringToFront();
            
        }

        private void usT_HorizontalTab_ActionsPanel_Click(object sender, EventArgs e)
        {
            UpdateUSTButtonsStatus(usT_HorizontalTab_ActionsPanel);
            uC_TopActionsPanel1.BringToFront();
            uC_TopActionsPanel1.UpdateButtonsEnabled();
            uC_TopActionsPanel1.UpdatePanelBubbtonsVisibleStatus();
        }

        private void OpenActionPanel()
        {
            usT_HorizontalTab_ActionsPanel_Click(this.usT_HorizontalTab_ActionsPanel, EventArgs.Empty);
        }
         private void StartEdit()
        {
            uC_NavigationPanel2.Enabled = false;
            uC_TopMainPanel1.Enabled = false;
            uC_TopActionsPanel1.Enabled = false;
        }

        private void FinishEdit()
        {
            uC_NavigationPanel2.Enabled = true;
            uC_TopMainPanel1.Enabled = true;
            uC_TopActionsPanel1.Enabled = true;
        }

        private void OpenCommentPanel()
        {
            CreatePanel = true;
            uC_Comments.BringToFront();
            UpdateCreatePanel();

        }

        private void CloseCommentPanel()
        {
            CreatePanel = false;
            UpdateCreatePanel();
        }

        private void ApplyCommentPanel()
        {
            CreatePanel = false;
            UpdateCreatePanel();
            uC_Approve.Demote();
        }

        public void OpenHomePanel()
        {
            OpenSetHistoryPanel(false);
            GlobalData.OpenPanelIndex = 0;
            if (firstStart)
            {
                firstStart = false;
                uC_StartPanel1.UpdatePanels();
            }
            else
            {
                uC_StartPanel1.ButtonSearch_Click();
            }
          
            
            //uC_StartPanel1.ButtonSearch_Click();
            uC_StartPanel1.BringToFront();
            tableLayoutPanel8.RowStyles[0].Height = 0;
            uC_TopMainPanel1.UpdateMainBtnColor();
        }

        public void OpenProjectPanel()
        {
            OpenSetHistoryPanel(false);
            GlobalData.OpenPanelIndex = 2;
            uC_ProjectPanel1.UpdateProjectPanel();
            uC_TopActionsPanel1.UpdateButtonsEnabled();
            //uC_NaviSettingsPanel1.ButtonMainClick();
            uC_ProjectNaviPanel1.ButtonClick(ProjectNaviPanelIndex);
            tableLayoutPanel8.RowStyles[0].Height = 25;
            uC_TopMainPanel1.UpdateMainBtnColor();
            uC_ProjectPanel1.BringToFront();
        }

        private void OpenSearchPanel()
        {
            GlobalData.OpenPanelIndex = 1;
            OpenSetHistoryPanel(false);        
            uC_SearchPanel1.ClearAllControls();
            uC_SearchPanel1.UpdateSearchView();
            tableLayoutPanel8.RowStyles[0].Height = 0;
            uC_TopMainPanel1.UpdateMainBtnColor();
            uC_SearchPanel1.BringToFront();
        }

        private void OpenAdminPanel()
        {
            GlobalData.OpenPanelIndex = 3;
            OpenSetHistoryPanel(false);
             
            usT_HorizontalTab_ActionsPanel_Click(this.usT_HorizontalTab_ActionsPanel, EventArgs.Empty);
            uC_AdmiPanel1.GetAllSetList();           
            tableLayoutPanel8.RowStyles[0].Height = 0;
            uC_TopMainPanel1.UpdateMainBtnColor();
            uC_AdmiPanel1.BringToFront();
        }

        private void OpenNavisPanel()
        {
            GlobalData.OpenPanelIndex = 4;
            uC_AdmiPanel1.GetAllNavisList();         
            usT_HorizontalTab_ActionsPanel_Click(this.usT_HorizontalTab_ActionsPanel, EventArgs.Empty);
            tableLayoutPanel8.RowStyles[0].Height = 0;
            uC_TopMainPanel1.UpdateMainBtnColor();
            uC_AdmiPanel1.BringToFront();
        }

        private void AdminCanStart()
        {
            uC_TopActionsPanel1.UpdatePanelBubbtonsEnabled(1);
        }
        private void AdminCanNotStart()
        {
            if (GlobalData.OpenPanelIndex == 3 || GlobalData.OpenPanelIndex == 4)
            {
                uC_TopActionsPanel1.UpdatePanelBubbtonsEnabled(1);
            }
            else
            {
                uC_TopActionsPanel1.UpdatePanelBubbtonsEnabled(0);
            }
            
        }
        #endregion

        public void StartProcess()
        {
            try
            {
                if (ProcasThread == null || (ProcasThread != null && !ProcasThread.IsAlive))
                {
                    try
                    {
                        GlobalMethodes._stop = false;
                        ProcasThread = new Thread(StartProcessPanel);
                        ProcasThread.IsBackground = true;
                        ProcasThread.Name = "Start";
                        ProcasThread.IsBackground = true;
                        ProcasThread.Priority = ThreadPriority.Lowest;
                        ProcasThread.Start();
                    }
                    catch 
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }

        }

        private void CreateStagePanel(byte mode = 0)
        {
            if (CreatePanel == false)
            {
                CreatePanel = true;
                uC_CreateStagePanel1.BringToFront();
                uC_CreateStagePanel1.UpdateLanguageComboBox();
                uC_CreateStagePanel1.UpdateStageComboBox();
                uC_NavigationPanel2.Enabled = false;
                uC_TopMainPanel1.Enabled = false;
            }
            else
            {
                CreatePanel = false;
                uC_NavigationPanel2.Enabled = true;
                uC_TopMainPanel1.Enabled = true;
            }
            UpdateCreatePanel();
            uC_NavigationPanel2.Focus();
            GlobalMethodes._stop = true;
        }

        private void CreatePositionPanel(byte mode)
        {
            if (CreatePanel == false)
            {
                CreatePanel = true;

                uC_CreatePositionPanel1.FiilProjectInfo(mode);
                uC_CreatePositionPanel1.BringToFront();
                uC_NavigationPanel2.Enabled = false;
                uC_TopMainPanel1.Enabled = false;
            }
            
            UpdateCreatePanel();
        }

        private void CancelCreatePosition()
        {
            if (CreatePanel == true)
            {
                CreatePanel = false;
                uC_NavigationPanel2.Enabled = true;
                uC_TopMainPanel1.Enabled = true;
            }
            UpdateCreatePanel();
        }

        private void OpenSetHistoryPanel(bool open)
        {
            CreatePanel = open;
            if (CreatePanel == true)
            {
                //CreatePanel = false;
                UC_HistoryPanel.BringToFront();
                UC_HistoryPanel.UpdateHistory();
                //uC_TopMainPanel1.Enabled = true;
            }
            UpdateCreatePanel();
        }

        private void OpenTaskInfo(bool open, List<int> ids, List<int> tdids, string from, string to, int row)
        {
            CreatePanelWidth = 400;
            CreatePanel = open;
            uC_TaskInfo.BringToFront();
            uC_TaskInfo.UpdateDG(ids, tdids, from, to);
            UpdateCreatePanel();
        }

        private void OpenTaskRoute(string txt, int statusId, int routetype)
        {
            uC_TaskPanel.UpdateTaskRoute(txt, statusId, routetype);
        }

        private void ChangeTaskStatus(string from, string to, int row)
        {
            RequestInfo.requestInfoThree();
            uC_TaskPanel.UpdateDG(from, to, row);
            uC_TaskPanel.DG_SizeChanged(this.uC_TaskPanel, EventArgs.Empty);
        }

        private void EditTask(byte mode)
        {
            switch (mode)
            {
                case 0:
                    uC_TaskInfo.button2_Click(uC_TaskInfo.button2, EventArgs.Empty);
                    break;
                case 1:
                    uC_TaskInfo.button4_Click(uC_TaskInfo.button4, EventArgs.Empty);
                    break;
                case 2:
                    uC_TaskInfo.button3_Click(uC_TaskInfo.button3, EventArgs.Empty);
                    break;
            }

            
        }

        #region --- NaviPanelEvents ---
        /// <summary>
        /// Диспетчер проектов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void usT_VerticalTabControl1_Click(object sender, EventArgs e)
        {
            if (NaviPanel == false)
            {
                tableLayoutPanel5.ColumnStyles[2].Width = 200;
                tableLayoutPanel5.ColumnStyles[3].Width = 5;

                NaviPanel = true;
                uC_NavigationPanel2.BringToFront();

                usT_VerticalTabControl1.PressedStatus = true;
                usT_VerticalTabControl1.Invalidate();
            }
            else
            {
                if (NaviSettPanel == true)
                {
                    NaviPanel = true;
                    NaviSettPanel = false;
                    uC_NavigationPanel2.BringToFront();

                    usT_VerticalTabControl1.PressedStatus = NaviPanel;
                    usT_VerticalTabControl1.Invalidate();

                }
                else
                {
                    tableLayoutPanel5.ColumnStyles[2].Width = 0;
                    tableLayoutPanel5.ColumnStyles[3].Width = 0;
                    NaviPanel = false;
                    NaviSettPanel = false;
                    usT_VerticalTabControl1.PressedStatus = false;
                    usT_VerticalTabControl1.Invalidate();
                }

            }
        }

        /// <summary>
        /// Панель свойств
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void usT_VerticalTabControl2_Click(object sender, EventArgs e)
        {
            if (NaviPanel == false)
            {
                tableLayoutPanel5.ColumnStyles[2].Width = 200;
                tableLayoutPanel5.ColumnStyles[3].Width = 5;

                NaviPanel = true;
                NaviSettPanel = true;
                usT_VerticalTabControl1.PressedStatus = false;
                usT_VerticalTabControl1.Invalidate();
            }
            else
            {
                if (NaviSettPanel == false)
                {
                    NaviPanel = true;
                    NaviSettPanel = true;

                    usT_VerticalTabControl1.PressedStatus = false;
                    usT_VerticalTabControl1.Invalidate();
                }
                else
                {
                    NaviPanel = false;
                    NaviSettPanel = false;

                    tableLayoutPanel5.ColumnStyles[2].Width = 0;
                    tableLayoutPanel5.ColumnStyles[3].Width = 0;

                    usT_VerticalTabControl1.PressedStatus = false;
                    usT_VerticalTabControl1.Invalidate();

                }
               
            }
        }

        private void panel6_MouseDown(object sender, MouseEventArgs e)
        {
            NaviPanelStartWidth = tableLayoutPanel5.ColumnStyles[2].Width;
            NaviPanelStartPoin = new Point(e.X, e.Y);

        }

        private void panel6_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                float delta = e.X - NaviPanelStartPoin.X;
                if (NaviPanelStartWidth + delta > 0)
                {
                    tableLayoutPanel5.ColumnStyles[2].Width = NaviPanelStartWidth + delta;
                }
            }
        }

        private void openPosMainPanel()
        {
            ProjectNaviPanelIndex = 0;
            OpenSetHistoryPanel(false);
            uC_ProjectPanel1.BringToFront();
            uC_ProjectPanel1.Focus();
        }

        private void openPosInfoPanel()
        {
            
        }

        private void openPosSchedulePanel()
        {
            ProjectNaviPanelIndex = 1;
            OpenSetHistoryPanel(false);
            uC_ProjectSchedulePanel1.UpdateTopBtnEnabled();
            uC_ProjectSchedulePanel1.UpdatePrjSchedule();
            uC_ProjectSchedulePanel1.BringToFront();

        }

        private void openCoordoinationPanel()
        {
            ProjectNaviPanelIndex = 2;
            OpenSetHistoryPanel(false);
            uC_Coordination.GetCoordinates();
            uC_Coordination.UpdateHistoryDG();
            uC_Coordination.UpdateCoordinates();
            uC_Coordination.BringToFront();
        }
        /// <summary>
        /// openApprovalsPanel
        /// </summary>
        /// <param name="btn"></param>
        /// 
        private void openApprovalsPanel()
        {
            ProjectNaviPanelIndex = 4;
            OpenSetHistoryPanel(false);
            GlobalMethodes.ReadSQL_GetUserSets();
            uC_Approve.GetRole();
            uC_Approve.UpdateButtonsEnabled();
            uC_Approve.UpdateDG();
            uC_Approve.BringToFront();
        }

        private void openTaskPanel()
        {
            ProjectNaviPanelIndex = 5;
            OpenSetHistoryPanel(false);
            uC_TaskPanel.UpdateControls();
            uC_TaskPanel.BringToFront();
        }
        #endregion

        #region --- Methodes ---
        private void UpdateUSTButtonsStatus(Control btn)
        {
            switch (btn.Name)
            {
                case "usT_HorizontalTab_MainPanel":
                    usT_HorizontalTab_MainPanel.PressedStatus = true;
                    usT_HorizontalTab_MainPanel.Invalidate();
                    usT_HorizontalTab_ActionsPanel.PressedStatus = false;
                    usT_HorizontalTab_ActionsPanel.Invalidate();
                    break;
                case "usT_HorizontalTab_ActionsPanel":
                    usT_HorizontalTab_MainPanel.PressedStatus = false;
                    usT_HorizontalTab_MainPanel.Invalidate();
                    usT_HorizontalTab_ActionsPanel.PressedStatus = true;
                    usT_HorizontalTab_ActionsPanel.Invalidate();
                    break;
            }
        }

        private void UpdateCreatePanel()
        {
            if (CreatePanel == true)
            {
                tableLayoutPanel5.ColumnStyles[5].Width = 5;
                tableLayoutPanel5.ColumnStyles[6].Width = CreatePanelWidth;

            }
            else
            {
                tableLayoutPanel5.ColumnStyles[5].Width = 0;
                tableLayoutPanel5.ColumnStyles[6].Width = 0;

            }
        }

        public void CreateStageDirPath()
        {
            Thread.Sleep(3000);
            RequestInfo.requestInfoThree();
            GlobalData.NaviTreeView.CreateTreeView_New();
            GlobalData.SelectedDirPath = GlobalData.BuferDirPath;
            GlobalData.NaviTreeView.SelectTreeNode_New();
            CreatePanel = true;
            CreateStagePanel();
            GlobalMethodes._stop = true;
        }

        public void CreatePosition()
        {
            Thread.Sleep(6000);
            RequestInfo.requestInfoThree();
            GlobalData.NaviTreeView.CreateTreeView_New();
            GlobalData.SelectedDirPath = GlobalData.BuferDirPath;
            GlobalData.NaviTreeView.SelectTreeNode_New();
            GlobalMethodes._stop = true;

        }

        public void CreateProduct()
        {
            Thread.Sleep(6000);
            RequestInfo.requestInfoThree();
            GlobalData.NaviTreeView.CreateTreeView_New();
            GlobalData.SelectedDirPath = GlobalData.BuferDirPath;
            GlobalData.NaviTreeView.SelectTreeNode_New();
            GlobalMethodes._stop = true;
        }


        #endregion

        #region --- DisciplinePanel ---

        private void UpdateSetSchedule()
        {
            //CreateDisiplinePanel(dict);
            openPosSchedulePanel();
            uC_ProjectSchedulePanel1.UpdatePrjSchedule();
            uC_ProjectSchedulePanel1.UpdateTopBtnEnabled(true, 0);
            uC_TopActionsPanel1.UpdateButtonsEnabled();
            NaviPanel = false;
            NaviSettPanel = false;

        }

        private void CancelSetCreation()
        {
            if (CreatePanel == true)
            {
                CreatePanel = false;
                uC_NavigationPanel2.Enabled = true;
                uC_TopMainPanel1.Enabled = true;
                CreatePanelWidth = 250;
            }
            uC_ProjectSchedulePanel1.UpdateTopBtnEnabled(true, 0);
            UpdateCreatePanel();
        }

        private void ApproveSetsList()
        {

            GlobalData.loadInfo = "Создание структуры проекта";
            OpenSetHistoryPanel(false);
            StartProcess();
            uC_NavigationPanel2.Enabled = false;          
            uC_ProjectSchedulePanel1.ApproveSets();
            uC_NavigationPanel2.Enabled = true;
            uC_TopMainPanel1.Enabled = true;  
            
            SetesApproved();
            
        }

        private void CancelSetChanges()
        {
            //CancelSetCreation();
            uC_ProjectSchedulePanel1.UpdateTopBtnEnabled(false);
            uC_NavigationPanel2.Enabled = true;
            uC_TopMainPanel1.Enabled = true;
            uC_ProjectSchedulePanel1.UpdatePrjSchedule();
        }

        private void ApproveSetsListBtnClick(PositionInfo positionInfo = null, ProductStageInfo productInfo = null, TechSulutionStageInfo solutionInfo = null)
        {  
            uC_TopActionsPanel1.btnApproveClick(positionInfo, productInfo, solutionInfo);
        }

        private void EditSetsList()
        {
            uC_ProjectNaviPanel1.usT_HorizontalTabControl2_Click(uC_ProjectNaviPanel1.usT_HorizontalTabControl2, EventArgs.Empty);
            //Thread.Sleep(1000);
            uC_ProjectSchedulePanel1.buttonEdit_Click(uC_ProjectSchedulePanel1.buttonEdit, EventArgs.Empty);
        }

        private void EditSetsListBtnClick(PositionInfo positionInfo = null, ProductStageInfo productStageInfo = null, TechSulutionStageInfo techSulutionStageInfo = null)
        {
            if (positionInfo != null)
            {
                if (!positionInfo.SetListeInRelease || GlobalData.addSubSetList.Count > 0)
                {
                    positionInfo.SetListeInRelease = true;
                    uC_ProjectSchedulePanel1.UpdateTopBtnEnabled(true);
                    uC_NavigationPanel2.Enabled = false;
                    uC_TopMainPanel1.Enabled = false;

                }
                else
                {
                    if (positionInfo.SetListeInRelease)
                    {
                        DialogResult relise = MessageBox.Show($"Cостав проекта {positionInfo.Code} \n" +
                                                              $"уже утвержден.\n\n" +
                                                              $"Хотите внести изменения?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (relise == DialogResult.Yes)
                        {
                            uC_NavigationPanel2.Enabled = false;
                            uC_TopMainPanel1.Enabled = false;

                            positionInfo.SetListeInRelease = true;
                            uC_ProjectSchedulePanel1.UpdateTopBtnEnabled(true);
                        }
                    }
                    else
                    {
                        //CreateDisiplinePanel(dict);
                    }
                }
            }
            else if (productStageInfo != null)
            {
                if (!productStageInfo.SetListeInRelease || GlobalData.addSubSetList.Count > 0)
                {
                    uC_ProjectSchedulePanel1.UpdateTopBtnEnabled(true);
                    uC_NavigationPanel2.Enabled = false;
                    uC_TopMainPanel1.Enabled = false;
                }
                else
                {
                    if (productStageInfo.SetListeInRelease)
                    {
                        DialogResult relise = MessageBox.Show($"Cостав продукта {productStageInfo.Code} \n" +
                                                              $"уже утвержден.\n\n" +
                                                              $"Хотите внести изменения?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (relise == DialogResult.Yes)
                        {
                            uC_NavigationPanel2.Enabled = false;
                            uC_TopMainPanel1.Enabled = false;

                            productStageInfo.SetListeInRelease = true;
                            uC_ProjectSchedulePanel1.UpdateTopBtnEnabled(true);
                            //GlobalData.addSubSetDict.Clear();
                            //CreateDisiplinePanel(dict);
                        }
                    }
                    else
                    {
                        uC_ProjectSchedulePanel1.UpdateTopBtnEnabled(true);
                        //CreateDisiplinePanel(dict);
                    }
                }
            }
            else if (techSulutionStageInfo != null)
            {
                if (!techSulutionStageInfo.SetListeInRelease || GlobalData.addSubSetList.Count > 0)
                {
                    uC_ProjectSchedulePanel1.UpdateTopBtnEnabled(true);
                    uC_NavigationPanel2.Enabled = false;
                    uC_TopMainPanel1.Enabled = false;
                }
                else
                {
                    if (techSulutionStageInfo.SetListeInRelease)
                    {
                        DialogResult relise = MessageBox.Show($"Cостав тех.решения {techSulutionStageInfo.Code} \n" +
                                                              $"уже утвержден.\n\n" +
                                                              $"Хотите внести изменения?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (relise == DialogResult.Yes)
                        {
                            uC_NavigationPanel2.Enabled = false;
                            uC_TopMainPanel1.Enabled = false;

                            techSulutionStageInfo.SetListeInRelease = true;
                            uC_ProjectSchedulePanel1.UpdateTopBtnEnabled(true);
                            //GlobalData.addSubSetDict.Clear();
                            //CreateDisiplinePanel(dict);
                        }
                    }
                    else
                    {
                        //CreateDisiplinePanel(dict);
                        uC_ProjectSchedulePanel1.UpdateTopBtnEnabled(true);
                    }
                }
            }
            else
            {
                MessageBox.Show("Функция Добавление разделов проекта доступна если\n" +
                                    "в Диспетчере проектов выбрана Позиция по ГП или\n" +
                                    "Стадия продукта. \n\n" +
                                    "Выберите Позицию по ГП или Стадию для продукта.",
                                    "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void EditeSetsList(List<ScheduleItem> existingitems)
        {
            //GlobalData.addSubSetList.Clear();
            CreateDisiplinePanel(existingitems);
        }

        private void CreateDisiplinePanel(List<ScheduleItem> existingitems)
        {
            if (CreatePanel == false)
            {
                uC_AddProjectSetPanel1.BringToFront();
                CreatePanelWidth = 700;
                uC_AddProjectSetPanel1.UpdateControlsValues(existingitems);
                uC_ProjectNaviPanel1.BtnSetSchedule_Click();
                CreatePanel = true;
            }
            uC_AddProjectSetPanel1.UpdateAddSetList();
            uC_AddProjectSetPanel1.UpdateAddListView();
            UpdateCreatePanel();
        }

        private void SetesApproved()
        {
            Thread.Sleep(3000);
            RequestInfo.requestInfoThree();
            GlobalData.NaviTreeView.CreateTreeView_New();
            GlobalData.SelectedDirPath = GlobalData.BuferDirPath;
            GlobalData.NaviTreeView.SelectTreeNode_New();
            //uC_ProjectSchedulePanel1.UpdatePrjSchedule();
            

            uC_ProjectNaviPanel1.BtnSetSchedule_Click();
            openPosSchedulePanel();
            GlobalMethodes._stop = true;
            CreatePanel = true;
            CancelSetCreation();
            uC_NavigationPanel2.Enabled = true;
            uC_TopMainPanel1.Enabled = true;
            uC_ProjectSchedulePanel1.UpdateTopBtnEnabled(false);
            uC_NavigationPanel2.Focus();

        }

        private void ValeraStart()
        {
            //GlobalData.loadInfo = "Запуск процесса создания RVT...";
            //StartProcess();

            string Code = uC_AdmiPanel1.GetSelectedCode();
            string Answer = "";

            if (Code != "")
            {
                if (GlobalMethodes.CreateRVT(Code, GlobalData.user.UserAccount, out Answer))
                {
                    MessageBox.Show("Выбранный проект взят в работу другим пользователем", "Предупреждение");
                    switch (Answer)
                    {
                        case "[В работе]": uC_AdmiPanel1.GetAllSetList(); break;
                        case "[Готово]": uC_AdmiPanel1.GetAllSetList(); break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Не выбран проект для которого\nнужно сформировать ЦФХ.\nВыберите проект из списка.", "Предупреждение", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void NWFStart()
        {
            //this.TopMost = false;
            GlobalMethodes._stop = false;

            string Code = uC_AdmiPanel1.GetSelectedCode();
            string Answer = "";

            if (Code != "")
            {
                GetLocation();
                GlobalData.loadInfo = "Запуск процесса создания NWF...";
                StartProcess();


                //ProcasThread = new Thread(StartProcessPanel);
                //ProcasThread.IsBackground = true;
                //ProcasThread.Name = "Start";
                //ProcasThread.IsBackground = true;
                //ProcasThread.Priority = ThreadPriority.Lowest;
                //ProcasThread.Start();

                if (GlobalMethodes.CreateNWF(Code, GlobalData.user.UserAccount, out Answer))
                {
                    MessageBox.Show("Выбранный проект взят в работу другим пользователем", "Предупреждение");
                    switch (Answer)
                    {
                        case "[В работе]": uC_AdmiPanel1.GetAllSetList(); break;
                        case "[Готово]": uC_AdmiPanel1.GetAllSetList(); break;
                    }
                }
                

                if (pForm.DialogResult == DialogResult.OK)
                {
                    ProcasThread.Abort();
                    ProcasThread.Join();
                    this.TopLevel = true;
                }

                MessageBox.Show("Процесс создания/обновления NWF завершен", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Form_Click(uC_AdmiPanel1, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Не выбран проект для которого\nнужно сформировать ЦФХ.\nВыберите проект из списка.", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //this.TopMost = true;
        }

        private void UpdateStartForm(bool status)
        {
            if (status)
            {
                GlobalData.loadInfo = "Сбор данных...";
                StartProcess();
            }
            else
            {
                CloseProcessForm();
            }
        }

        private void GetLocation()
        {
            GlobalData.X = this.Location.X;
            GlobalData.Y = this.Location.Y;
            GlobalData.Width = this.Width;
            GlobalData.Height = this.Height;
        }

        private void ValeraApprove()
        {
            string Code = uC_AdmiPanel1.GetSelectedCode();
            if (Code != "")
            {
                GlobalMethodes.requestSectionApproved(Code);
                uC_AdmiPanel1.GetAllSetList();
            }
            else
            {
                MessageBox.Show("Не выбран проект для утвердждения.\nВыберите проект из списка.", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void NWFApprove()
        {
            string Code = uC_AdmiPanel1.GetSelectedCode();
            if (Code != "")
            {
                GlobalMethodes.requestNWFApproved(Code);
                uC_AdmiPanel1.GetAllNavisList();
            }
            else
            {
                MessageBox.Show("Не выбран проект для утвердждения.\nВыберите проект из списка.", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        private void SearchProjectThreeNode()
        {
            GlobalData.NaviTreeView.SelectTreeNode(GlobalData.CreatedFolder);
        }

        public static bool UpdateUserAxes(string account)
        {
            //account = "a.garbachenok";
            //account = "s.gorodnik";
            GlobalData.user = RequestInfo.lb.Users.FirstOrDefault(x => x.UserAccount == account);

            try
            {
                if (GlobalData.user.FunctionId == 7 || GlobalData.user.UserAccount == "a.dmitriev" || GlobalData.user.UserAccount == "m.bardushko")
                {
                    GlobalData.UserRole = "Admin";
                }
                else if (GlobalData.user.FunctionId == 6 || GlobalData.user.FunctionId == 1 || GlobalData.user.FunctionId == 2)
                {
                    GlobalData.UserRole = "Manager";
                }
                else
                {
                    GlobalData.UserRole = "User";
                }
                return true;
            }
            catch
            {
                MessageBox.Show($"Пользователь {account} не найден. Обратитесь к Администратору.");
                return false;
            }
        }

        private void tableLayoutPanel5_MouseUp(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Click");
        }

        private void tableLayoutPanel2_Click(object sender, EventArgs e)
        {
            if (CloseStart == false)
            {
                //this.TopMost = true;
                this.BringToFront();
                //this.TopMost = false;
            }
            CloseStart = true;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalMethodes.CreateLog("Заткрытие программы");
        }

        private void uC_NavigationPanel2_MouseClick()
        {
            //this.TopMost = false;
        }

        private void tableLayoutPanel2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            usT_MaximizeButton1_Click(this.usT_MaximizeButton1, EventArgs.Empty);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) minimized = true;
            else minimized = false;


        }

        private void uC_StartPanel1_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal) windowstate = 0;
            else if (WindowState == FormWindowState.Maximized) windowstate = 1;
            else windowstate = 2;
        }
    }
}
