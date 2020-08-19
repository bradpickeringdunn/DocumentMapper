namespace DocumentMapper.Word.AddIn
{
    partial class DocumentMapperRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public DocumentMapperRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DocumentMapper = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.NewDocumentMapBtn = this.Factory.CreateRibbonButton();
            this.DocumentMapper.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DocumentMapper
            // 
            this.DocumentMapper.Groups.Add(this.group1);
            this.DocumentMapper.Label = "Document Mapper";
            this.DocumentMapper.Name = "DocumentMapper";
            this.DocumentMapper.Position = this.Factory.RibbonPosition.AfterOfficeId("DocumentMapperTab");
            // 
            // group1
            // 
            this.group1.Items.Add(this.NewDocumentMapBtn);
            this.group1.Label = "group1";
            this.group1.Name = "group1";
            // 
            // NewDocumentMapBtn
            // 
            this.NewDocumentMapBtn.Label = "New Document Map";
            this.NewDocumentMapBtn.Name = "NewDocumentMapBtn";
            this.NewDocumentMapBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.NewDocumentMapBtn_Click);
            // 
            // DocumentMapperRibbon
            // 
            this.Name = "DocumentMapperRibbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.DocumentMapper);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.DocumentMapperRibbon_Load);
            this.DocumentMapper.ResumeLayout(false);
            this.DocumentMapper.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab DocumentMapper;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton NewDocumentMapBtn;
    }

    partial class ThisRibbonCollection
    {
        internal DocumentMapperRibbon DocumentMapperRibbon
        {
            get { return this.GetRibbon<DocumentMapperRibbon>(); }
        }
    }
}
