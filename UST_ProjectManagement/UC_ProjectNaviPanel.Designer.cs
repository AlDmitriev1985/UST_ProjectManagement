namespace UST_ProjectManagement
{
    partial class UC_ProjectNaviPanel
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button_Left = new System.Windows.Forms.Button();
            this.button_Right = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.usT_HorizontalTabControl1 = new UST_ProjectManagement.UST_HorizontalTabControl();
            this.usT_HorizontalTabControl2 = new UST_ProjectManagement.UST_HorizontalTabControl();
            this.usT_HorizontalTabControl3 = new UST_ProjectManagement.UST_HorizontalTabControl();
            this.usT_HorizontalTabControl4 = new UST_ProjectManagement.UST_HorizontalTabControl();
            this.usT_HorizontalTabControl5 = new UST_ProjectManagement.UST_HorizontalTabControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.usT_HorizontalTabControl1);
            this.flowLayoutPanel1.Controls.Add(this.usT_HorizontalTabControl2);
            this.flowLayoutPanel1.Controls.Add(this.usT_HorizontalTabControl3);
            this.flowLayoutPanel1.Controls.Add(this.usT_HorizontalTabControl4);
            this.flowLayoutPanel1.Controls.Add(this.usT_HorizontalTabControl5);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Font = new System.Drawing.Font("Bahnschrift", 9.75F);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1550, 25);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.SizeChanged += new System.EventHandler(this.flowLayoutPanel1_SizeChanged);
            // 
            // button_Left
            // 
            this.button_Left.FlatAppearance.BorderSize = 0;
            this.button_Left.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Left.Image = global::UST_ProjectManagement.Properties.Resources.Btn_Left_Whit_15x15;
            this.button_Left.Location = new System.Drawing.Point(0, 0);
            this.button_Left.Margin = new System.Windows.Forms.Padding(0);
            this.button_Left.Name = "button_Left";
            this.button_Left.Size = new System.Drawing.Size(15, 25);
            this.button_Left.TabIndex = 0;
            this.button_Left.UseVisualStyleBackColor = true;
            this.button_Left.Click += new System.EventHandler(this.button_Left_Click);
            // 
            // button_Right
            // 
            this.button_Right.FlatAppearance.BorderSize = 0;
            this.button_Right.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Right.Image = global::UST_ProjectManagement.Properties.Resources.Btn_Right_Whit_15x15;
            this.button_Right.Location = new System.Drawing.Point(15, 0);
            this.button_Right.Margin = new System.Windows.Forms.Padding(0);
            this.button_Right.Name = "button_Right";
            this.button_Right.Size = new System.Drawing.Size(15, 25);
            this.button_Right.TabIndex = 1;
            this.button_Right.UseVisualStyleBackColor = true;
            this.button_Right.Click += new System.EventHandler(this.button_Right_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Шифр проекта";
            // 
            // usT_HorizontalTabControl1
            // 
            this.usT_HorizontalTabControl1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.usT_HorizontalTabControl1.Font = new System.Drawing.Font("Bahnschrift", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.usT_HorizontalTabControl1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl1.Location = new System.Drawing.Point(0, 0);
            this.usT_HorizontalTabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.usT_HorizontalTabControl1.Name = "usT_HorizontalTabControl1";
            this.usT_HorizontalTabControl1.Size = new System.Drawing.Size(167, 25);
            this.usT_HorizontalTabControl1.TabIndex = 0;
            this.usT_HorizontalTabControl1.Text = "Общее";
            this.usT_HorizontalTabControl1.UST_PressedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl1.UST_PressedForeColor = System.Drawing.Color.Black;
            this.usT_HorizontalTabControl1.UST_SelectedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl1.Click += new System.EventHandler(this.usT_HorizontalTabControl1_Click);
            // 
            // usT_HorizontalTabControl2
            // 
            this.usT_HorizontalTabControl2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.usT_HorizontalTabControl2.Font = new System.Drawing.Font("Bahnschrift", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.usT_HorizontalTabControl2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl2.Location = new System.Drawing.Point(167, 0);
            this.usT_HorizontalTabControl2.Margin = new System.Windows.Forms.Padding(0);
            this.usT_HorizontalTabControl2.Name = "usT_HorizontalTabControl2";
            this.usT_HorizontalTabControl2.Size = new System.Drawing.Size(203, 25);
            this.usT_HorizontalTabControl2.TabIndex = 1;
            this.usT_HorizontalTabControl2.Text = "Состав проекта";
            this.usT_HorizontalTabControl2.UST_PressedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl2.UST_PressedForeColor = System.Drawing.Color.Black;
            this.usT_HorizontalTabControl2.UST_SelectedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl2.Click += new System.EventHandler(this.usT_HorizontalTabControl2_Click);
            // 
            // usT_HorizontalTabControl3
            // 
            this.usT_HorizontalTabControl3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.usT_HorizontalTabControl3.Font = new System.Drawing.Font("Bahnschrift", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.usT_HorizontalTabControl3.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl3.Location = new System.Drawing.Point(370, 0);
            this.usT_HorizontalTabControl3.Margin = new System.Windows.Forms.Padding(0);
            this.usT_HorizontalTabControl3.Name = "usT_HorizontalTabControl3";
            this.usT_HorizontalTabControl3.Size = new System.Drawing.Size(203, 25);
            this.usT_HorizontalTabControl3.TabIndex = 2;
            this.usT_HorizontalTabControl3.Text = "Координация";
            this.usT_HorizontalTabControl3.UST_PressedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl3.UST_PressedForeColor = System.Drawing.Color.Black;
            this.usT_HorizontalTabControl3.UST_SelectedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl3.Click += new System.EventHandler(this.usT_HorizontalTabControl3_Click);
            // 
            // usT_HorizontalTabControl4
            // 
            this.usT_HorizontalTabControl4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.usT_HorizontalTabControl4.Font = new System.Drawing.Font("Bahnschrift", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.usT_HorizontalTabControl4.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl4.Location = new System.Drawing.Point(573, 0);
            this.usT_HorizontalTabControl4.Margin = new System.Windows.Forms.Padding(0);
            this.usT_HorizontalTabControl4.Name = "usT_HorizontalTabControl4";
            this.usT_HorizontalTabControl4.Size = new System.Drawing.Size(307, 25);
            this.usT_HorizontalTabControl4.TabIndex = 3;
            this.usT_HorizontalTabControl4.Text = "Утверждение/Согласование";
            this.usT_HorizontalTabControl4.UST_PressedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl4.UST_PressedForeColor = System.Drawing.Color.Black;
            this.usT_HorizontalTabControl4.UST_SelectedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl4.Click += new System.EventHandler(this.usT_HorizontalTabControl4_Click);
            // 
            // usT_HorizontalTabControl5
            // 
            this.usT_HorizontalTabControl5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.usT_HorizontalTabControl5.Font = new System.Drawing.Font("Bahnschrift", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.usT_HorizontalTabControl5.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl5.Location = new System.Drawing.Point(880, 0);
            this.usT_HorizontalTabControl5.Margin = new System.Windows.Forms.Padding(0);
            this.usT_HorizontalTabControl5.Name = "usT_HorizontalTabControl5";
            this.usT_HorizontalTabControl5.Size = new System.Drawing.Size(307, 25);
            this.usT_HorizontalTabControl5.TabIndex = 4;
            this.usT_HorizontalTabControl5.Text = "Передача заданий";
            this.usT_HorizontalTabControl5.UST_PressedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl5.UST_PressedForeColor = System.Drawing.Color.Black;
            this.usT_HorizontalTabControl5.UST_SelectedColor = System.Drawing.Color.WhiteSmoke;
            this.usT_HorizontalTabControl5.Click += new System.EventHandler(this.usT_HorizontalTabControl5_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1600, 50);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.button_Left);
            this.flowLayoutPanel2.Controls.Add(this.button_Right);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(1550, 25);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(50, 25);
            this.flowLayoutPanel2.TabIndex = 2;
            // 
            // UC_ProjectNaviPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Bahnschrift", 11.25F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UC_ProjectNaviPanel";
            this.Size = new System.Drawing.Size(1600, 50);
            this.Load += new System.EventHandler(this.UC_ProjectNaviPanel_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button_Left;
        private System.Windows.Forms.Button button_Right;
        public UST_HorizontalTabControl usT_HorizontalTabControl2;
        public UST_HorizontalTabControl usT_HorizontalTabControl1;
        public UST_HorizontalTabControl usT_HorizontalTabControl3;
        public UST_HorizontalTabControl usT_HorizontalTabControl4;
        public UST_HorizontalTabControl usT_HorizontalTabControl5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    }
}
