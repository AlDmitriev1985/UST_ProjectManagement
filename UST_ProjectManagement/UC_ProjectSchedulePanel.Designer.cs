namespace UST_ProjectManagement
{
    partial class UC_ProjectSchedulePanel
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
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button_Help = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonApprove = new System.Windows.Forms.Button();
            this.buttonEditeSchedule = new System.Windows.Forms.Button();
            this.buttonChangeDepartment = new System.Windows.Forms.Button();
            this.button_Print = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(921, 808);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(921, 808);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.flowLayoutPanel1.Controls.Add(this.buttonEdit);
            this.flowLayoutPanel1.Controls.Add(this.buttonApprove);
            this.flowLayoutPanel1.Controls.Add(this.buttonEditeSchedule);
            this.flowLayoutPanel1.Controls.Add(this.buttonChangeDepartment);
            this.flowLayoutPanel1.Controls.Add(this.button_Print);
            this.flowLayoutPanel1.Controls.Add(this.buttonCancel);
            this.flowLayoutPanel1.Controls.Add(this.button_Help);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(921, 35);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // button_Help
            // 
            this.button_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Help.FlatAppearance.BorderSize = 0;
            this.button_Help.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Help.Font = new System.Drawing.Font("Bahnschrift", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_Help.ForeColor = System.Drawing.Color.DimGray;
            this.button_Help.Image = global::UST_ProjectManagement.Properties.Resources.Btn_Help_30x30;
            this.button_Help.Location = new System.Drawing.Point(210, 0);
            this.button_Help.Margin = new System.Windows.Forms.Padding(0);
            this.button_Help.Name = "button_Help";
            this.button_Help.Size = new System.Drawing.Size(35, 35);
            this.button_Help.TabIndex = 7;
            this.button_Help.UseVisualStyleBackColor = true;
            this.button_Help.Click += new System.EventHandler(this.button_Help_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.FlatAppearance.BorderSize = 0;
            this.buttonEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEdit.Image = global::UST_ProjectManagement.Properties.Resources.btn_Edit_30x30;
            this.buttonEdit.Location = new System.Drawing.Point(0, 0);
            this.buttonEdit.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(35, 35);
            this.buttonEdit.TabIndex = 1;
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonApprove
            // 
            this.buttonApprove.FlatAppearance.BorderSize = 0;
            this.buttonApprove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonApprove.Image = global::UST_ProjectManagement.Properties.Resources.Btn_Approve_Grey_30x30;
            this.buttonApprove.Location = new System.Drawing.Point(35, 0);
            this.buttonApprove.Margin = new System.Windows.Forms.Padding(0);
            this.buttonApprove.Name = "buttonApprove";
            this.buttonApprove.Size = new System.Drawing.Size(35, 35);
            this.buttonApprove.TabIndex = 2;
            this.buttonApprove.UseVisualStyleBackColor = true;
            this.buttonApprove.Click += new System.EventHandler(this.buttonApprove_Click);
            // 
            // buttonEditeSchedule
            // 
            this.buttonEditeSchedule.FlatAppearance.BorderSize = 0;
            this.buttonEditeSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEditeSchedule.Image = global::UST_ProjectManagement.Properties.Resources.btnAdd_RemoveSett_30x30;
            this.buttonEditeSchedule.Location = new System.Drawing.Point(70, 0);
            this.buttonEditeSchedule.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEditeSchedule.Name = "buttonEditeSchedule";
            this.buttonEditeSchedule.Size = new System.Drawing.Size(35, 35);
            this.buttonEditeSchedule.TabIndex = 5;
            this.buttonEditeSchedule.UseVisualStyleBackColor = true;
            this.buttonEditeSchedule.Click += new System.EventHandler(this.buttonEditeSchedule_Click);
            // 
            // buttonChangeDepartment
            // 
            this.buttonChangeDepartment.FlatAppearance.BorderSize = 0;
            this.buttonChangeDepartment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChangeDepartment.Image = global::UST_ProjectManagement.Properties.Resources.btnEditSelected_30x30;
            this.buttonChangeDepartment.Location = new System.Drawing.Point(105, 0);
            this.buttonChangeDepartment.Margin = new System.Windows.Forms.Padding(0);
            this.buttonChangeDepartment.Name = "buttonChangeDepartment";
            this.buttonChangeDepartment.Size = new System.Drawing.Size(35, 35);
            this.buttonChangeDepartment.TabIndex = 4;
            this.buttonChangeDepartment.UseVisualStyleBackColor = true;
            this.buttonChangeDepartment.Click += new System.EventHandler(this.buttonChangeDepartment_Click);
            // 
            // button_Print
            // 
            this.button_Print.FlatAppearance.BorderSize = 0;
            this.button_Print.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Print.Image = global::UST_ProjectManagement.Properties.Resources.btn_Print_30x30;
            this.button_Print.Location = new System.Drawing.Point(140, 0);
            this.button_Print.Margin = new System.Windows.Forms.Padding(0);
            this.button_Print.Name = "button_Print";
            this.button_Print.Size = new System.Drawing.Size(35, 35);
            this.button_Print.TabIndex = 3;
            this.button_Print.UseVisualStyleBackColor = true;
            this.button_Print.Click += new System.EventHandler(this.button_Print_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Image = global::UST_ProjectManagement.Properties.Resources.Btn_Cancel_30x30;
            this.buttonCancel.Location = new System.Drawing.Point(175, 0);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(35, 35);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // UC_ProjectSchedulePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Bahnschrift", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "UC_ProjectSchedulePanel";
            this.Size = new System.Drawing.Size(921, 808);
            this.Load += new System.EventHandler(this.UC_ProjectSchedulePanel_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button button_Print;
        public System.Windows.Forms.Button buttonApprove;
        public System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonChangeDepartment;
        private System.Windows.Forms.Button buttonEditeSchedule;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button button_Help;
    }
}
