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
            this.BTN_search = new System.Windows.Forms.Button();
            this.txt_input_search = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BTN_search
            // 
            this.BTN_search.Location = new System.Drawing.Point(240, 7);
            this.BTN_search.Name = "BTN_search";
            this.BTN_search.Size = new System.Drawing.Size(150, 29);
            this.BTN_search.TabIndex = 0;
            this.BTN_search.Text = "Search";
            this.BTN_search.UseVisualStyleBackColor = true;
            this.BTN_search.Click += new System.EventHandler(this.BTN_search_Click);
            // 
            // txt_input_search
            // 
            this.txt_input_search.Location = new System.Drawing.Point(12, 12);
            this.txt_input_search.Name = "txt_input_search";
            this.txt_input_search.Size = new System.Drawing.Size(210, 20);
            this.txt_input_search.TabIndex = 1;
            this.txt_input_search.Text = "AAPL";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 479);
            this.Controls.Add(this.txt_input_search);
            this.Controls.Add(this.BTN_search);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BTN_search;
        private System.Windows.Forms.TextBox txt_input_search;
    }
}

