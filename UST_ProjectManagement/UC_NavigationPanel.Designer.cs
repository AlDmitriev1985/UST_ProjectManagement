namespace UST_ProjectManagement
{
    partial class UC_NavigationPanel
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.treeViewNavigation = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.treeViewNavigation, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(230, 589);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // treeViewNavigation
            // 
            this.treeViewNavigation.BackColor = System.Drawing.Color.WhiteSmoke;
            this.treeViewNavigation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewNavigation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.treeViewNavigation.Location = new System.Drawing.Point(0, 5);
            this.treeViewNavigation.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.treeViewNavigation.Name = "treeViewNavigation";
            this.treeViewNavigation.Size = new System.Drawing.Size(230, 584);
            this.treeViewNavigation.TabIndex = 0;
            this.treeViewNavigation.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewNavigation_BeforeExpand);
            this.treeViewNavigation.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewNavigation_BeforeSelect);
            this.treeViewNavigation.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewNavigation_NodeMouseClick);
            // 
            // UC_NavigationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UC_NavigationPanel";
            this.Size = new System.Drawing.Size(230, 589);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.TreeView treeViewNavigation;
    }
}
