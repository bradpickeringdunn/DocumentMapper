namespace DocumentMapper.Word.AddIn
{
    partial class NewDocumentMapperWindow
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BrowseFilePathBtn = new System.Windows.Forms.Button();
            this.FilePathTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DocumentMapFilenameTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CreateNewDocumentMap = new System.Windows.Forms.Button();
            this.SaveNewDocumentMapDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BrowseFilePathBtn);
            this.groupBox1.Controls.Add(this.FilePathTxt);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.DocumentMapFilenameTxt);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.CreateNewDocumentMap);
            this.groupBox1.Location = new System.Drawing.Point(12, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 119);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Document Map File Location";
            // 
            // BrowseFilePathBtn
            // 
            this.BrowseFilePathBtn.Location = new System.Drawing.Point(187, 51);
            this.BrowseFilePathBtn.Name = "BrowseFilePathBtn";
            this.BrowseFilePathBtn.Size = new System.Drawing.Size(53, 20);
            this.BrowseFilePathBtn.TabIndex = 7;
            this.BrowseFilePathBtn.Text = "Browse";
            this.BrowseFilePathBtn.UseVisualStyleBackColor = true;
            this.BrowseFilePathBtn.Click += new System.EventHandler(this.BrowseFilePathBtn_Click);
            // 
            // FilePathTxt
            // 
            this.FilePathTxt.Location = new System.Drawing.Point(73, 51);
            this.FilePathTxt.Name = "FilePathTxt";
            this.FilePathTxt.ReadOnly = true;
            this.FilePathTxt.Size = new System.Drawing.Size(107, 20);
            this.FilePathTxt.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "File Path";
            // 
            // DocumentMapFilenameTxt
            // 
            this.DocumentMapFilenameTxt.Location = new System.Drawing.Point(73, 24);
            this.DocumentMapFilenameTxt.Name = "DocumentMapFilenameTxt";
            this.DocumentMapFilenameTxt.Size = new System.Drawing.Size(108, 20);
            this.DocumentMapFilenameTxt.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "File Name";
            // 
            // CreateNewDocumentMap
            // 
            this.CreateNewDocumentMap.Enabled = false;
            this.CreateNewDocumentMap.Location = new System.Drawing.Point(0, 74);
            this.CreateNewDocumentMap.Name = "CreateNewDocumentMap";
            this.CreateNewDocumentMap.Size = new System.Drawing.Size(157, 32);
            this.CreateNewDocumentMap.TabIndex = 0;
            this.CreateNewDocumentMap.Text = "Create New Document Map";
            this.CreateNewDocumentMap.UseVisualStyleBackColor = true;
            this.CreateNewDocumentMap.Click += new System.EventHandler(this.button1_Click);
            // 
            // NewDocumentMapperWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 127);
            this.Controls.Add(this.groupBox1);
            this.Name = "NewDocumentMapperWindow";
            this.Text = "NewDocumentMapperWindow";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox DocumentMapFilenameTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button CreateNewDocumentMap;
        private System.Windows.Forms.FolderBrowserDialog SaveNewDocumentMapDialog;
        private System.Windows.Forms.Button BrowseFilePathBtn;
        private System.Windows.Forms.TextBox FilePathTxt;
        private System.Windows.Forms.Label label1;
    }
}