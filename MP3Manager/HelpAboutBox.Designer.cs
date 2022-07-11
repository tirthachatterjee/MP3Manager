namespace MP3Manager
{
    partial class HelpAboutBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.okButton = new System.Windows.Forms.Button();
            this.labelProductName = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.linkLabelMail_ = new System.Windows.Forms.LinkLabel();
            this.pictureBoxGooglePlus_ = new System.Windows.Forms.PictureBox();
            this.pictureBoxFacebookIcon_ = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGooglePlus_)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFacebookIcon_)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Location = new System.Drawing.Point(234, 135);
            this.okButton.Margin = new System.Windows.Forms.Padding(4);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(78, 28);
            this.okButton.TabIndex = 30;
            this.okButton.Text = "&OK";
            // 
            // labelProductName
            // 
            this.labelProductName.Location = new System.Drawing.Point(16, 15);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.Size = new System.Drawing.Size(296, 23);
            this.labelProductName.TabIndex = 31;
            this.labelProductName.Text = "Product Name";
            this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelVersion
            // 
            this.labelVersion.Location = new System.Drawing.Point(16, 43);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(296, 23);
            this.labelVersion.TabIndex = 32;
            this.labelVersion.Text = "Version";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCopyright
            // 
            this.labelCopyright.Location = new System.Drawing.Point(16, 71);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(296, 23);
            this.labelCopyright.TabIndex = 33;
            this.labelCopyright.Text = "Copyright";
            this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabelMail_
            // 
            this.linkLabelMail_.Location = new System.Drawing.Point(16, 99);
            this.linkLabelMail_.Name = "linkLabelMail_";
            this.linkLabelMail_.Size = new System.Drawing.Size(296, 23);
            this.linkLabelMail_.TabIndex = 34;
            this.linkLabelMail_.TabStop = true;
            this.linkLabelMail_.Text = "chatterjee.tirtha@gmail.com";
            this.linkLabelMail_.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabelMail_.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelMail__LinkClicked);
            // 
            // pictureBoxGooglePlus_
            // 
            this.pictureBoxGooglePlus_.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxGooglePlus_.Image = global::MP3Manager.Properties.Resources.GooglePlusLogo64;
            this.pictureBoxGooglePlus_.Location = new System.Drawing.Point(64, 125);
            this.pictureBoxGooglePlus_.Margin = new System.Windows.Forms.Padding(5);
            this.pictureBoxGooglePlus_.Name = "pictureBoxGooglePlus_";
            this.pictureBoxGooglePlus_.Size = new System.Drawing.Size(41, 38);
            this.pictureBoxGooglePlus_.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxGooglePlus_.TabIndex = 36;
            this.pictureBoxGooglePlus_.TabStop = false;
            this.pictureBoxGooglePlus_.Click += new System.EventHandler(this.pictureBoxGooglePlus__Click);
            // 
            // pictureBoxFacebookIcon_
            // 
            this.pictureBoxFacebookIcon_.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxFacebookIcon_.Image = global::MP3Manager.Properties.Resources.FacebookIcon64;
            this.pictureBoxFacebookIcon_.Location = new System.Drawing.Point(16, 125);
            this.pictureBoxFacebookIcon_.Name = "pictureBoxFacebookIcon_";
            this.pictureBoxFacebookIcon_.Size = new System.Drawing.Size(41, 38);
            this.pictureBoxFacebookIcon_.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxFacebookIcon_.TabIndex = 35;
            this.pictureBoxFacebookIcon_.TabStop = false;
            this.pictureBoxFacebookIcon_.Click += new System.EventHandler(this.pictureBoxFacebookIcon__Click);
            // 
            // HelpAboutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 177);
            this.Controls.Add(this.pictureBoxGooglePlus_);
            this.Controls.Add(this.pictureBoxFacebookIcon_);
            this.Controls.Add(this.linkLabelMail_);
            this.Controls.Add(this.labelCopyright);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelProductName);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HelpAboutBox";
            this.Padding = new System.Windows.Forms.Padding(12, 11, 12, 11);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AboutBox";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGooglePlus_)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFacebookIcon_)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelCopyright;
        private System.Windows.Forms.LinkLabel linkLabelMail_;
        private System.Windows.Forms.PictureBox pictureBoxFacebookIcon_;
        private System.Windows.Forms.PictureBox pictureBoxGooglePlus_;


    }
}
