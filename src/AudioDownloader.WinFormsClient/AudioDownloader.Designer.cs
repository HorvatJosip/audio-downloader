using System.Threading.Tasks;

namespace AudioDownloader.WinFormsClient
{
    partial class AudioDownloader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AudioDownloader));
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtLink = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStart = new System.Windows.Forms.TextBox();
            this.txtEnd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSplitRanges = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSaveLocation = new System.Windows.Forms.TextBox();
            this.btnBrowseForSaveLocation = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbOpenContainingDirectory = new System.Windows.Forms.CheckBox();
            this.cbRunAudioFile = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 30.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(311, 23);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(362, 55);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Audio Downloader";
            // 
            // txtLink
            // 
            this.txtLink.Location = new System.Drawing.Point(195, 121);
            this.txtLink.Name = "txtLink";
            this.txtLink.Size = new System.Drawing.Size(735, 33);
            this.txtLink.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "YouTube link";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(89, 320);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Start time";
            // 
            // txtStart
            // 
            this.txtStart.Location = new System.Drawing.Point(195, 317);
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(211, 33);
            this.txtStart.TabIndex = 4;
            // 
            // txtEnd
            // 
            this.txtEnd.Location = new System.Drawing.Point(719, 317);
            this.txtEnd.Name = "txtEnd";
            this.txtEnd.Size = new System.Drawing.Size(211, 33);
            this.txtEnd.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(618, 320);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 25);
            this.label3.TabIndex = 5;
            this.label3.Text = "End time";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(48, 379);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(883, 60);
            this.label4.TabIndex = 7;
            this.label4.Text = "To input time, use HH:MM:SS format. You can also just input SS or just MM:SS.\r\nEx" +
    "amples: 2:13:02 -> 2 hours 13 minutes 2 seconds, 25:34 -> 25 minutes 34 seconds," +
    " 17 -> 17 seconds";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 471);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 25);
            this.label5.TabIndex = 9;
            this.label5.Text = "Other sections";
            // 
            // txtSplitRanges
            // 
            this.txtSplitRanges.Location = new System.Drawing.Point(195, 468);
            this.txtSplitRanges.Name = "txtSplitRanges";
            this.txtSplitRanges.Size = new System.Drawing.Size(735, 33);
            this.txtSplitRanges.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(53, 522);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(883, 85);
            this.label6.TabIndex = 10;
            this.label6.Text = resources.GetString("label6.Text");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(92, 779);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 25);
            this.label7.TabIndex = 12;
            this.label7.Text = "Location";
            // 
            // txtSaveLocation
            // 
            this.txtSaveLocation.Location = new System.Drawing.Point(196, 775);
            this.txtSaveLocation.Name = "txtSaveLocation";
            this.txtSaveLocation.Size = new System.Drawing.Size(614, 33);
            this.txtSaveLocation.TabIndex = 11;
            this.txtSaveLocation.Leave += new System.EventHandler(this.txtSaveLocation_Leave);
            // 
            // btnBrowseForSaveLocation
            // 
            this.btnBrowseForSaveLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseForSaveLocation.Location = new System.Drawing.Point(824, 773);
            this.btnBrowseForSaveLocation.Name = "btnBrowseForSaveLocation";
            this.btnBrowseForSaveLocation.Size = new System.Drawing.Size(107, 43);
            this.btnBrowseForSaveLocation.TabIndex = 13;
            this.btnBrowseForSaveLocation.Text = "Browse";
            this.btnBrowseForSaveLocation.UseVisualStyleBackColor = true;
            this.btnBrowseForSaveLocation.Click += new System.EventHandler(this.btnBrowseForSaveLocation_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Location = new System.Drawing.Point(409, 178);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(167, 64);
            this.btnDownload.TabIndex = 14;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(317, 619);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(167, 64);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(88, 268);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 25);
            this.label8.TabIndex = 16;
            this.label8.Text = "File name";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(195, 264);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(735, 33);
            this.txtFileName.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 30.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(408, 706);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(169, 55);
            this.label9.TabIndex = 18;
            this.label9.Text = "Settings";
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(501, 619);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(167, 64);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbOpenContainingDirectory
            // 
            this.cbOpenContainingDirectory.AutoSize = true;
            this.cbOpenContainingDirectory.Location = new System.Drawing.Point(222, 838);
            this.cbOpenContainingDirectory.Name = "cbOpenContainingDirectory";
            this.cbOpenContainingDirectory.Size = new System.Drawing.Size(540, 29);
            this.cbOpenContainingDirectory.TabIndex = 20;
            this.cbOpenContainingDirectory.Text = "Open the location where audio file resides after it gets saved";
            this.cbOpenContainingDirectory.UseVisualStyleBackColor = true;
            this.cbOpenContainingDirectory.CheckedChanged += new System.EventHandler(this.cbOpenContainingDirectory_CheckedChanged);
            // 
            // cbRunAudioFile
            // 
            this.cbRunAudioFile.AutoSize = true;
            this.cbRunAudioFile.Location = new System.Drawing.Point(326, 887);
            this.cbRunAudioFile.Name = "cbRunAudioFile";
            this.cbRunAudioFile.Size = new System.Drawing.Size(333, 29);
            this.cbRunAudioFile.TabIndex = 21;
            this.cbRunAudioFile.Text = "Run the audio file after it gets saved";
            this.cbRunAudioFile.UseVisualStyleBackColor = true;
            this.cbRunAudioFile.CheckedChanged += new System.EventHandler(this.cbRunAudioFile_CheckedChanged);
            // 
            // AudioDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(35)))), ((int)(((byte)(46)))));
            this.ClientSize = new System.Drawing.Size(984, 944);
            this.Controls.Add(this.cbRunAudioFile);
            this.Controls.Add(this.cbOpenContainingDirectory);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnBrowseForSaveLocation);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtSaveLocation);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSplitRanges);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtEnd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtStart);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLink);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(240)))), ((int)(((byte)(234)))));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "AudioDownloader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Audio Downloader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtLink;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.TextBox txtEnd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSplitRanges;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSaveLocation;
        private System.Windows.Forms.Button btnBrowseForSaveLocation;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox cbOpenContainingDirectory;
        private System.Windows.Forms.CheckBox cbRunAudioFile;
    }
}