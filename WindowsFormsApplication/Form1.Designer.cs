namespace WindowsFormsApplication
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tab_search = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txt_input_search = new System.Windows.Forms.TextBox();
            this.BTN_search = new System.Windows.Forms.Button();
            this.tab_admin = new System.Windows.Forms.TabPage();
            this.btn_delete = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tab_search.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tab_admin.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tab_search);
            this.tabControl1.Controls.Add(this.tab_admin);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1039, 550);
            this.tabControl1.TabIndex = 0;
            // 
            // tab_search
            // 
            this.tab_search.Controls.Add(this.dataGridView1);
            this.tab_search.Controls.Add(this.txt_input_search);
            this.tab_search.Controls.Add(this.BTN_search);
            this.tab_search.Location = new System.Drawing.Point(4, 22);
            this.tab_search.Name = "tab_search";
            this.tab_search.Padding = new System.Windows.Forms.Padding(3);
            this.tab_search.Size = new System.Drawing.Size(1031, 524);
            this.tab_search.TabIndex = 0;
            this.tab_search.Text = "Search";
            this.tab_search.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(7, 46);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1016, 469);
            this.dataGridView1.TabIndex = 5;
            // 
            // txt_input_search
            // 
            this.txt_input_search.Location = new System.Drawing.Point(7, 13);
            this.txt_input_search.Name = "txt_input_search";
            this.txt_input_search.Size = new System.Drawing.Size(210, 20);
            this.txt_input_search.TabIndex = 4;
            this.txt_input_search.Text = "AAPL";
            // 
            // BTN_search
            // 
            this.BTN_search.Location = new System.Drawing.Point(235, 8);
            this.BTN_search.Name = "BTN_search";
            this.BTN_search.Size = new System.Drawing.Size(150, 29);
            this.BTN_search.TabIndex = 3;
            this.BTN_search.Text = "Search";
            this.BTN_search.UseVisualStyleBackColor = true;
            this.BTN_search.Click += new System.EventHandler(this.BTN_search_Click_1);
            // 
            // tab_admin
            // 
            this.tab_admin.Controls.Add(this.btn_delete);
            this.tab_admin.Location = new System.Drawing.Point(4, 22);
            this.tab_admin.Name = "tab_admin";
            this.tab_admin.Padding = new System.Windows.Forms.Padding(3);
            this.tab_admin.Size = new System.Drawing.Size(1031, 524);
            this.tab_admin.TabIndex = 1;
            this.tab_admin.Text = "Admin";
            this.tab_admin.UseVisualStyleBackColor = true;
            // 
            // btn_delete
            // 
            this.btn_delete.Location = new System.Drawing.Point(310, 20);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(156, 22);
            this.btn_delete.TabIndex = 0;
            this.btn_delete.Text = "Drop all tables in database";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 549);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Ask Edgar";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tab_search.ResumeLayout(false);
            this.tab_search.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tab_admin.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tab_search;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txt_input_search;
        private System.Windows.Forms.Button BTN_search;
        private System.Windows.Forms.TabPage tab_admin;
        private System.Windows.Forms.Button btn_delete;
    }
}

