namespace UST_ProjectManagement
{
    partial class UC_StartPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_StartPanel));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.usT_HorizontalTabControl5 = new UST_ProjectManagement.UST_HorizontalTabControl();
            this.usT_HorizontalTabControl4 = new UST_ProjectManagement.UST_HorizontalTabControl();
            this.usT_HorizontalTabControl3 = new UST_ProjectManagement.UST_HorizontalTabControl();
            this.usT_HorizontalTabControl2 = new UST_ProjectManagement.UST_HorizontalTabControl();
            this.usT_HorizontalTabControl1 = new UST_ProjectManagement.UST_HorizontalTabControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Bahnschrift", 9.75F);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(847, 495);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 90);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(847, 4);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radioButton5);
            this.panel2.Controls.Add(this.radioButton4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.radioButton3);
            this.panel2.Controls.Add(this.radioButton2);
            this.panel2.Controls.Add(this.radioButton1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(847, 1);
            this.panel2.TabIndex = 1;
            this.panel2.Visible = false;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(412, 7);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(69, 20);
            this.radioButton5.TabIndex = 4;
            this.radioButton5.Text = "Сборки";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(329, 7);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(77, 20);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.Text = "Задания";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Silver;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(846, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1, 1);
            this.panel3.TabIndex = 4;
            this.panel3.Visible = false;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(216, 7);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(103, 20);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "Тех. решения";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(110, 7);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(86, 20);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Продукты";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(7, 7);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(79, 20);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Проекты";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(847, 90);
            this.panel4.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Silver;
            this.panel5.Controls.Add(this.usT_HorizontalTabControl5);
            this.panel5.Controls.Add(this.usT_HorizontalTabControl4);
            this.panel5.Controls.Add(this.usT_HorizontalTabControl3);
            this.panel5.Controls.Add(this.usT_HorizontalTabControl2);
            this.panel5.Controls.Add(this.usT_HorizontalTabControl1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(847, 25);
            this.panel5.TabIndex = 0;
            // 
            // usT_HorizontalTabControl5
            // 
            this.usT_HorizontalTabControl5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.usT_HorizontalTabControl5.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl5.Location = new System.Drawing.Point(600, 0);
            this.usT_HorizontalTabControl5.Name = "usT_HorizontalTabControl5";
            this.usT_HorizontalTabControl5.Size = new System.Drawing.Size(150, 25);
            this.usT_HorizontalTabControl5.TabIndex = 4;
            this.usT_HorizontalTabControl5.Text = "Сборки";
            this.usT_HorizontalTabControl5.UST_PressedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl5.UST_PressedForeColor = System.Drawing.Color.Black;
            this.usT_HorizontalTabControl5.UST_SelectedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl5.Click += new System.EventHandler(this.usT_HorizontalTabControl_Click);
            // 
            // usT_HorizontalTabControl4
            // 
            this.usT_HorizontalTabControl4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.usT_HorizontalTabControl4.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl4.Location = new System.Drawing.Point(450, 0);
            this.usT_HorizontalTabControl4.Name = "usT_HorizontalTabControl4";
            this.usT_HorizontalTabControl4.Size = new System.Drawing.Size(150, 25);
            this.usT_HorizontalTabControl4.TabIndex = 3;
            this.usT_HorizontalTabControl4.Text = "Задания";
            this.usT_HorizontalTabControl4.UST_PressedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl4.UST_PressedForeColor = System.Drawing.Color.Black;
            this.usT_HorizontalTabControl4.UST_SelectedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl4.Click += new System.EventHandler(this.usT_HorizontalTabControl_Click);
            // 
            // usT_HorizontalTabControl3
            // 
            this.usT_HorizontalTabControl3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.usT_HorizontalTabControl3.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl3.Location = new System.Drawing.Point(300, 0);
            this.usT_HorizontalTabControl3.Name = "usT_HorizontalTabControl3";
            this.usT_HorizontalTabControl3.Size = new System.Drawing.Size(150, 25);
            this.usT_HorizontalTabControl3.TabIndex = 2;
            this.usT_HorizontalTabControl3.Text = "Тех. решения";
            this.usT_HorizontalTabControl3.UST_PressedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl3.UST_PressedForeColor = System.Drawing.Color.Black;
            this.usT_HorizontalTabControl3.UST_SelectedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl3.Click += new System.EventHandler(this.usT_HorizontalTabControl_Click);
            // 
            // usT_HorizontalTabControl2
            // 
            this.usT_HorizontalTabControl2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.usT_HorizontalTabControl2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl2.Location = new System.Drawing.Point(150, 0);
            this.usT_HorizontalTabControl2.Name = "usT_HorizontalTabControl2";
            this.usT_HorizontalTabControl2.Size = new System.Drawing.Size(150, 25);
            this.usT_HorizontalTabControl2.TabIndex = 1;
            this.usT_HorizontalTabControl2.Text = "Продукты";
            this.usT_HorizontalTabControl2.UST_PressedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl2.UST_PressedForeColor = System.Drawing.Color.Black;
            this.usT_HorizontalTabControl2.UST_SelectedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl2.Click += new System.EventHandler(this.usT_HorizontalTabControl_Click);
            // 
            // usT_HorizontalTabControl1
            // 
            this.usT_HorizontalTabControl1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.usT_HorizontalTabControl1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl1.Location = new System.Drawing.Point(0, 0);
            this.usT_HorizontalTabControl1.Name = "usT_HorizontalTabControl1";
            this.usT_HorizontalTabControl1.Size = new System.Drawing.Size(150, 25);
            this.usT_HorizontalTabControl1.TabIndex = 0;
            this.usT_HorizontalTabControl1.Text = "Проекты";
            this.usT_HorizontalTabControl1.UST_PressedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl1.UST_PressedForeColor = System.Drawing.Color.Black;
            this.usT_HorizontalTabControl1.UST_SelectedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl1.Click += new System.EventHandler(this.usT_HorizontalTabControl_Click);
            // 
            // UC_StartPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UC_StartPanel";
            this.Size = new System.Drawing.Size(847, 495);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.RadioButton radioButton4;
        public System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton5;
        public UST_HorizontalTabControl usT_HorizontalTabControl1;
        public UST_HorizontalTabControl usT_HorizontalTabControl5;
        public UST_HorizontalTabControl usT_HorizontalTabControl4;
        public UST_HorizontalTabControl usT_HorizontalTabControl3;
        public UST_HorizontalTabControl usT_HorizontalTabControl2;
    }
}
