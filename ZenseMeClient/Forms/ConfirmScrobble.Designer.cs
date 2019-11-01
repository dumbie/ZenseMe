namespace ZenseMe.Client.Forms
{
    partial class ConfirmScrobble
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfirmScrobble));
            this.cs_ScrobbleList = new System.Windows.Forms.ListView();
            this._cTrackTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._cTrackArtist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._cTrackAlbum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._cScrobbleDateTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._cSplitContainer = new System.Windows.Forms.SplitContainer();
            this.lbl_ScrobbleStatus = new System.Windows.Forms.Label();
            this.cs_ScrobbleStatusLabel = new System.Windows.Forms.Label();
            this.btn_ScrobbleNow = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._cSplitContainer)).BeginInit();
            this._cSplitContainer.Panel1.SuspendLayout();
            this._cSplitContainer.Panel2.SuspendLayout();
            this._cSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // cs_ScrobbleList
            // 
            this.cs_ScrobbleList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._cTrackTitle,
            this._cTrackArtist,
            this._cTrackAlbum,
            this._cScrobbleDateTime});
            this.cs_ScrobbleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cs_ScrobbleList.FullRowSelect = true;
            this.cs_ScrobbleList.GridLines = true;
            this.cs_ScrobbleList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.cs_ScrobbleList.Location = new System.Drawing.Point(0, 0);
            this.cs_ScrobbleList.MultiSelect = false;
            this.cs_ScrobbleList.Name = "cs_ScrobbleList";
            this.cs_ScrobbleList.Size = new System.Drawing.Size(727, 314);
            this.cs_ScrobbleList.TabIndex = 0;
            this.cs_ScrobbleList.UseCompatibleStateImageBehavior = false;
            this.cs_ScrobbleList.View = System.Windows.Forms.View.Details;
            // 
            // _cTrackTitle
            // 
            this._cTrackTitle.Text = "Track";
            this._cTrackTitle.Width = 235;
            // 
            // _cTrackArtist
            // 
            this._cTrackArtist.Text = "Artist";
            this._cTrackArtist.Width = 165;
            // 
            // _cTrackAlbum
            // 
            this._cTrackAlbum.Text = "Album";
            this._cTrackAlbum.Width = 170;
            // 
            // _cScrobbleDateTime
            // 
            this._cScrobbleDateTime.Text = "Scrobble time";
            this._cScrobbleDateTime.Width = 136;
            // 
            // _cSplitContainer
            // 
            this._cSplitContainer.BackColor = System.Drawing.Color.White;
            this._cSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._cSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._cSplitContainer.IsSplitterFixed = true;
            this._cSplitContainer.Location = new System.Drawing.Point(0, 0);
            this._cSplitContainer.Name = "_cSplitContainer";
            this._cSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _cSplitContainer.Panel1
            // 
            this._cSplitContainer.Panel1.BackColor = System.Drawing.Color.White;
            this._cSplitContainer.Panel1.Controls.Add(this.lbl_ScrobbleStatus);
            this._cSplitContainer.Panel1.Controls.Add(this.cs_ScrobbleStatusLabel);
            this._cSplitContainer.Panel1.Controls.Add(this.btn_ScrobbleNow);
            // 
            // _cSplitContainer.Panel2
            // 
            this._cSplitContainer.Panel2.Controls.Add(this.cs_ScrobbleList);
            this._cSplitContainer.Size = new System.Drawing.Size(727, 368);
            this._cSplitContainer.TabIndex = 1;
            // 
            // lbl_ScrobbleStatus
            // 
            this.lbl_ScrobbleStatus.AutoSize = true;
            this.lbl_ScrobbleStatus.Location = new System.Drawing.Point(4, 34);
            this.lbl_ScrobbleStatus.Name = "lbl_ScrobbleStatus";
            this.lbl_ScrobbleStatus.Size = new System.Drawing.Size(0, 13);
            this.lbl_ScrobbleStatus.TabIndex = 2;
            // 
            // cs_ScrobbleStatusLabel
            // 
            this.cs_ScrobbleStatusLabel.AutoSize = true;
            this.cs_ScrobbleStatusLabel.Location = new System.Drawing.Point(4, 9);
            this.cs_ScrobbleStatusLabel.Name = "cs_ScrobbleStatusLabel";
            this.cs_ScrobbleStatusLabel.Size = new System.Drawing.Size(205, 13);
            this.cs_ScrobbleStatusLabel.TabIndex = 1;
            this.cs_ScrobbleStatusLabel.Text = "The following tracks will be scrobbled:";
            // 
            // btn_ScrobbleNow
            // 
            this.btn_ScrobbleNow.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_ScrobbleNow.Location = new System.Drawing.Point(636, 0);
            this.btn_ScrobbleNow.Name = "btn_ScrobbleNow";
            this.btn_ScrobbleNow.Size = new System.Drawing.Size(91, 50);
            this.btn_ScrobbleNow.TabIndex = 0;
            this.btn_ScrobbleNow.Text = "Scrobble Now!";
            this.btn_ScrobbleNow.UseVisualStyleBackColor = true;
            this.btn_ScrobbleNow.Click += new System.EventHandler(this.btn_scrobble_Click);
            // 
            // ConfirmScrobble
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 368);
            this.Controls.Add(this._cSplitContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(743, 406);
            this.Name = "ConfirmScrobble";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Confirm Scrobbles";
            this._cSplitContainer.Panel1.ResumeLayout(false);
            this._cSplitContainer.Panel1.PerformLayout();
            this._cSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._cSplitContainer)).EndInit();
            this._cSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView cs_ScrobbleList;
        private System.Windows.Forms.ColumnHeader _cTrackTitle;
        private System.Windows.Forms.ColumnHeader _cTrackArtist;
        private System.Windows.Forms.ColumnHeader _cTrackAlbum;
        private System.Windows.Forms.ColumnHeader _cScrobbleDateTime;
        private System.Windows.Forms.SplitContainer _cSplitContainer;
        private System.Windows.Forms.Button btn_ScrobbleNow;
        private System.Windows.Forms.Label cs_ScrobbleStatusLabel;
        private System.Windows.Forms.Label lbl_ScrobbleStatus;
    }
}