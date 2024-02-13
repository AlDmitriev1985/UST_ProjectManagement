namespace UST_ProjectManagement
{
    partial class UC_TopMainPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_TopMainPanel));
            this.buttonHome = new System.Windows.Forms.Button();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.buttonProject = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button_Update = new System.Windows.Forms.Button();
            this.buttonAdminPanel = new System.Windows.Forms.Button();
            this.buttonAdminNavis = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonHome
            // 
            this.buttonHome.BackColor = System.Drawing.Color.Gray;
            this.buttonHome.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.buttonHome.FlatAppearance.BorderSize = 0;
            this.buttonHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.CornflowerBlue;
            this.buttonHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonHome.Image = ((System.Drawing.Image)(resources.GetObject("buttonHome.Image")));
            this.buttonHome.Location = new System.Drawing.Point(0, 0);
            this.buttonHome.Margin = new System.Windows.Forms.Padding(0);
            this.buttonHome.Name = "buttonHome";
            this.buttonHome.Size = new System.Drawing.Size(60, 80);
            this.buttonHome.TabIndex = 0;
            this.buttonHome.Tag = "0";
            this.buttonHome.UseVisualStyleBackColor = false;
            this.buttonHome.Click += new System.EventHandler(this.buttonHome_Click);
            // 
            // buttonSearch
            // 
            this.buttonSearch.BackColor = System.Drawing.Color.Gray;
            this.buttonSearch.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.buttonSearch.FlatAppearance.BorderSize = 0;
            this.buttonSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSearch.Image = ((System.Drawing.Image)(resources.GetObject("buttonSearch.Image")));
            this.buttonSearch.Location = new System.Drawing.Point(60, 0);
            this.buttonSearch.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(60, 80);
            this.buttonSearch.TabIndex = 1;
            this.buttonSearch.Tag = "1";
            this.buttonSearch.UseVisualStyleBackColor = false;
            this.buttonSearch.Visible = false;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // buttonProject
            // 
            this.buttonProject.BackColor = System.Drawing.Color.Gray;
            this.buttonProject.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.buttonProject.FlatAppearance.BorderSize = 0;
            this.buttonProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonProject.Image = ((System.Drawing.Image)(resources.GetObject("buttonProject.Image")));
            this.buttonProject.Location = new System.Drawing.Point(120, 0);
            this.buttonProject.Margin = new System.Windows.Forms.Padding(0);
            this.buttonProject.Name = "buttonProject";
            this.buttonProject.Size = new System.Drawing.Size(60, 80);
            this.buttonProject.TabIndex = 2;
            this.buttonProject.Tag = "2";
            this.buttonProject.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonProject.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonProject.UseVisualStyleBackColor = false;
            this.buttonProject.Click += new System.EventHandler(this.buttonProject_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.buttonHome);
            this.flowLayoutPanel1.Controls.Add(this.buttonSearch);
            this.flowLayoutPanel1.Controls.Add(this.buttonProject);
            this.flowLayoutPanel1.Controls.Add(this.button_Update);
            this.flowLayoutPanel1.Controls.Add(this.buttonAdminPanel);
            this.flowLayoutPanel1.Controls.Add(this.buttonAdminNavis);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1488, 80);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // button_Update
            // 
            this.button_Update.BackColor = System.Drawing.Color.Gray;
            this.button_Update.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.button_Update.FlatAppearance.BorderSize = 0;
            this.button_Update.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Update.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_Update.Image = ((System.Drawing.Image)(resources.GetObject("button_Update.Image")));
            this.button_Update.Location = new System.Drawing.Point(180, 0);
            this.button_Update.Margin = new System.Windows.Forms.Padding(0);
            this.button_Update.Name = "button_Update";
            this.button_Update.Size = new System.Drawing.Size(60, 80);
            this.button_Update.TabIndex = 5;
            this.button_Update.Tag = "4";
            this.button_Update.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_Update.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button_Update.UseVisualStyleBackColor = false;
            this.button_Update.Click += new System.EventHandler(this.button_Update_Click);
            this.button_Update.MouseEnter += new System.EventHandler(this.button_Update_MouseEnter);
            // 
            // buttonAdminPanel
            // 
            this.buttonAdminPanel.BackColor = System.Drawing.Color.Gray;
            this.buttonAdminPanel.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.buttonAdminPanel.FlatAppearance.BorderSize = 0;
            this.buttonAdminPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAdminPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAdminPanel.Image = ((System.Drawing.Image)(resources.GetObject("buttonAdminPanel.Image")));
            this.buttonAdminPanel.Location = new System.Drawing.Point(240, 0);
            this.buttonAdminPanel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAdminPanel.Name = "buttonAdminPanel";
            this.buttonAdminPanel.Size = new System.Drawing.Size(60, 80);
            this.buttonAdminPanel.TabIndex = 3;
            this.buttonAdminPanel.Tag = "3";
            this.buttonAdminPanel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonAdminPanel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonAdminPanel.UseVisualStyleBackColor = false;
            this.buttonAdminPanel.Click += new System.EventHandler(this.buttonAdminPanel_Click);
            // 
            // buttonAdminNavis
            // 
            this.buttonAdminNavis.BackColor = System.Drawing.Color.Gray;
            this.buttonAdminNavis.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.buttonAdminNavis.FlatAppearance.BorderSize = 0;
            this.buttonAdminNavis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAdminNavis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAdminNavis.Image = ((System.Drawing.Image)(resources.GetObject("buttonAdminNavis.Image")));
            this.buttonAdminNavis.Location = new System.Drawing.Point(300, 0);
            this.buttonAdminNavis.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAdminNavis.Name = "buttonAdminNavis";
            this.buttonAdminNavis.Size = new System.Drawing.Size(60, 80);
            this.buttonAdminNavis.TabIndex = 4;
            this.buttonAdminNavis.Tag = "4";
            this.buttonAdminNavis.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonAdminNavis.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonAdminNavis.UseVisualStyleBackColor = false;
            this.buttonAdminNavis.Click += new System.EventHandler(this.buttonAdminNavis_Click);
            // 
            // UC_TopMainPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "UC_TopMainPanel";
            this.Size = new System.Drawing.Size(1488, 80);
            this.Load += new System.EventHandler(this.UC_TopMainPanel_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonProject;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Button buttonHome;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button buttonAdminPanel;
        private System.Windows.Forms.Button buttonAdminNavis;
        private System.Windows.Forms.Button button_Update;
    }
}
