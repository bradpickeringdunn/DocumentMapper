namespace DocumentMapper.Word.AddIn
{
    partial class LinkToDocumentMapperWindow
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
            this.DocumentMapPathTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LinkNewDocumentMapBtn = new System.Windows.Forms.Button();
            this.OpenDocumentMapFile = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BrowseFilePathBtn);
            this.groupBox1.Controls.Add(this.DocumentMapPathTxt);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.LinkNewDocumentMapBtn);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 84);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Document Map File Location";
            // 
            // BrowseFilePathBtn
            // 
            this.BrowseFilePathBtn.Location = new System.Drawing.Point(191, 19);
            this.BrowseFilePathBtn.Name = "BrowseFilePathBtn";
            this.BrowseFilePathBtn.Size = new System.Drawing.Size(53, 20);
            this.BrowseFilePathBtn.TabIndex = 7;
            this.BrowseFilePathBtn.Text = "Browse";
            this.BrowseFilePathBtn.UseVisualStyleBackColor = true;
            this.BrowseFilePathBtn.Click += new System.EventHandler(this.BrowseFilePathBtn_Click);
            // 
            // DocumentMapPathTxt
            // 
            this.DocumentMapPathTxt.Location = new System.Drawing.Point(77, 19);
            this.DocumentMapPathTxt.Name = "DocumentMapPathTxt";
            this.DocumentMapPathTxt.ReadOnly = true;
            this.DocumentMapPathTxt.Size = new System.Drawing.Size(107, 20);
            this.DocumentMapPathTxt.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "File Path";
            // 
            // LinkNewDocumentMapBtn
            // 
            this.LinkNewDocumentMapBtn.Enabled = false;
            this.LinkNewDocumentMapBtn.Location = new System.Drawing.Point(4, 42);
            this.LinkNewDocumentMapBtn.Name = "LinkNewDocumentMapBtn";
            this.LinkNewDocumentMapBtn.Size = new System.Drawing.Size(157, 32);
            this.LinkNewDocumentMapBtn.TabIndex = 0;
            this.LinkNewDocumentMapBtn.Text = "Link Document Map";
            this.LinkNewDocumentMapBtn.UseVisualStyleBackColor = true;
            this.LinkNewDocumentMapBtn.Click += new System.EventHandler(this.CreateNewDocumentMap_Click);
            // 
            // OpenDocumentMapFile
            // 
            this.OpenDocumentMapFile.FileName = "OpenDocumentMapFile";
            // 
            // LinkToDocumentMapperWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox1);
            this.Name = "LinkToDocumentMapperWindow";
            this.Text = "LinkToDocumentMapperWindow";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BrowseFilePathBtn;
        private System.Windows.Forms.TextBox DocumentMapPathTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button LinkNewDocumentMapBtn;
        private System.Windows.Forms.OpenFileDialog OpenDocumentMapFile;
    }
}