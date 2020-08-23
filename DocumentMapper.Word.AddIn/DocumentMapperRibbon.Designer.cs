using Microsoft.Office.Interop.Word;

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
            this.LinkDocumentMapGroup = this.Factory.CreateRibbonGroup();
            this.NewDocumentMapBtn = this.Factory.CreateRibbonButton();
            this.LinkDocumentMapBtn = this.Factory.CreateRibbonButton();
            this.UnlinkDocumentMapGroup = this.Factory.CreateRibbonGroup();
            this.UnLinkDocumentMapBtn = this.Factory.CreateRibbonButton();
            this.DocumentMapper.SuspendLayout();
            this.LinkDocumentMapGroup.SuspendLayout();
            this.UnlinkDocumentMapGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // DocumentMapper
            // 
            this.DocumentMapper.Groups.Add(this.LinkDocumentMapGroup);
            this.DocumentMapper.Groups.Add(this.UnlinkDocumentMapGroup);
            this.DocumentMapper.Label = "Document Mapper";
            this.DocumentMapper.Name = "DocumentMapper";
            this.DocumentMapper.Position = this.Factory.RibbonPosition.AfterOfficeId("DocumentMapperTab");
            // 
            // LinkDocumentMapGroup
            // 
            this.LinkDocumentMapGroup.Items.Add(this.NewDocumentMapBtn);
            this.LinkDocumentMapGroup.Items.Add(this.LinkDocumentMapBtn);
            this.LinkDocumentMapGroup.Name = "LinkDocumentMapGroup";
            // 
            // NewDocumentMapBtn
            // 
            this.NewDocumentMapBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.NewDocumentMapBtn.Label = "Create Document Map";
            this.NewDocumentMapBtn.Name = "NewDocumentMapBtn";
            this.NewDocumentMapBtn.ShowImage = true;
            this.NewDocumentMapBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.NewDocumentMapBtn_Click);
            // 
            // LinkDocumentMapBtn
            // 
            this.LinkDocumentMapBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.LinkDocumentMapBtn.Label = "Link Document map";
            this.LinkDocumentMapBtn.Name = "LinkDocumentMapBtn";
            this.LinkDocumentMapBtn.ShowImage = true;
            // 
            // UnlinkDocumentMapGroup
            // 
            this.UnlinkDocumentMapGroup.Items.Add(this.UnLinkDocumentMapBtn);
            this.UnlinkDocumentMapGroup.Name = "UnlinkDocumentMapGroup";
            // 
            // UnLinkDocumentMapBtn
            // 
            this.UnLinkDocumentMapBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.UnLinkDocumentMapBtn.Label = "Un Link Document map";
            this.UnLinkDocumentMapBtn.Name = "UnLinkDocumentMapBtn";
            this.UnLinkDocumentMapBtn.ShowImage = true;
            this.UnLinkDocumentMapBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.UnLinkDocumentMapBtn_Click);
            // 
            // DocumentMapperRibbon
            // 
            this.Name = "DocumentMapperRibbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.DocumentMapper);
            this.DocumentMapper.ResumeLayout(false);
            this.DocumentMapper.PerformLayout();
            this.LinkDocumentMapGroup.ResumeLayout(false);
            this.LinkDocumentMapGroup.PerformLayout();
            this.UnlinkDocumentMapGroup.ResumeLayout(false);
            this.UnlinkDocumentMapGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab DocumentMapper;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup LinkDocumentMapGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton NewDocumentMapBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton LinkDocumentMapBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup UnlinkDocumentMapGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton UnLinkDocumentMapBtn;
    }

    partial class ThisRibbonCollection
    {
        internal DocumentMapperRibbon DocumentMapperRibbon
        {
            get { return this.GetRibbon<DocumentMapperRibbon>(); }
        }

    }
}
