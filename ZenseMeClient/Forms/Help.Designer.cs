namespace ZenseMe.Client.Forms
{
    partial class Help
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Help));
            this.lbl_HelpTitle = new System.Windows.Forms.Label();
            this.txtbox_HelpText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbl_HelpTitle
            // 
            this.lbl_HelpTitle.AutoSize = true;
            this.lbl_HelpTitle.Font = new System.Drawing.Font("Segoe UI Light", 20.25F);
            this.lbl_HelpTitle.Location = new System.Drawing.Point(12, 9);
            this.lbl_HelpTitle.Name = "lbl_HelpTitle";
            this.lbl_HelpTitle.Size = new System.Drawing.Size(70, 37);
            this.lbl_HelpTitle.TabIndex = 18;
            this.lbl_HelpTitle.Text = "Help";
            // 
            // txtbox_HelpText
            // 
            this.txtbox_HelpText.BackColor = System.Drawing.Color.White;
            this.txtbox_HelpText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtbox_HelpText.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtbox_HelpText.Location = new System.Drawing.Point(16, 49);
            this.txtbox_HelpText.Multiline = true;
            this.txtbox_HelpText.Name = "txtbox_HelpText";
            this.txtbox_HelpText.ReadOnly = true;
            this.txtbox_HelpText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtbox_HelpText.Size = new System.Drawing.Size(449, 397);
            this.txtbox_HelpText.TabIndex = 19;
            this.txtbox_HelpText.TabStop = false;
            this.txtbox_HelpText.Text = "Loading help text...";
            // 
            // Help
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(484, 464);
            this.Controls.Add(this.txtbox_HelpText);
            this.Controls.Add(this.lbl_HelpTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 502);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 502);
            this.Name = "Help";
            this.Text = "ZenseMe - Help";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_HelpTitle;
        private System.Windows.Forms.TextBox txtbox_HelpText;
    }
}