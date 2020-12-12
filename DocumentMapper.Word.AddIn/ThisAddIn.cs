using Office = Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
using System.Windows.Forms.Integration;
using Microsoft.Office.Tools.Word;
using System.Windows;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using DocumentMapper.Models;

namespace DocumentMapper.Word.AddIn
{
    public partial class ThisAddIn
    {

        private Microsoft.Office.Tools.CustomTaskPane myCustomTaskPane;
        
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            this.Application.DocumentOpen += new ApplicationEvents4_DocumentOpenEventHandler(OnDocumentOpen);
            this.Application.DocumentBeforeSave += new ApplicationEvents4_DocumentBeforeSaveEventHandler(DocumentBeforeSave);
           
        }



        private void CreateTaskPane()
        {

            if (Utils.ActiveDocumentLinkedToDocumentMap())
            {
                var taskPaneContainer = new TaskPaneContainer();
                var elementHost = new ElementHost() { Child = new DocumentMaperTaskPane() };
                taskPaneContainer.Controls.Add(elementHost);
                elementHost.Dock = System.Windows.Forms.DockStyle.Fill;

                myCustomTaskPane = this.CustomTaskPanes.Add(taskPaneContainer, "taskPaneContainer");
                myCustomTaskPane.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionRight;
                myCustomTaskPane.Visible = true;
                myCustomTaskPane.Width = 300;
            }
            else
            {

            }
        }

        public void InitializeDocumentMapper()
        {
            ShowDocumentMapperRibbon();
            CreateTaskPane();
            if (Utils.ActiveDocumentLinkedToDocumentMap())
            {
                //DocumentMapping.MapDocumentControlToMappedItem().Await();
            }
        }

        private void ShowDocumentMapperRibbon()
        {
            var ribbon = Globals.Ribbons.GetRibbon<DocumentMapperRibbon>();

            if (Utils.ActiveDocumentLinkedToDocumentMap())
            {
                ribbon.UnLinkDocumentMapBtn.Visible = true;
                ribbon.LinkDocumentMapGroup.Visible = false;
            }
            else
            {
                ribbon.UnLinkDocumentMapBtn.Visible = false;
                ribbon.LinkDocumentMapGroup.Visible = true;
            }
        }

        private void OnDocumentOpen(Microsoft.Office.Interop.Word.Document Doc)
        {
            InitializeDocumentMapper();
        }

        private void DocumentBeforeSave(Microsoft.Office.Interop.Word.Document Doc, ref bool SaveAsUI, ref bool Cancel)
        {
            if (Utils.ActiveDocumentLinkedToDocumentMap())
            {
                var path = Utils.DocumentMapperFilelocation();
                
                Utils.SaveDocumentMap(DocumentMapping.Current, path);
            }
        }


        #region Document Ribbon


        #endregion


        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
        }
        
        #endregion
    }
}
