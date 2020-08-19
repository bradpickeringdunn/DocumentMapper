using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using Microsoft.Office.Interop.Word;

namespace DocumentMapper.Word.AddIn
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            Globals.ThisAddIn.Application.DocumentChange += new ApplicationEvents4_DocumentChangeEventHandler(OnDocumentChange);
        }

        void OnDocumentChange()
        {
            var filePath = ApplicationVariables.GetVariable(ApplicationVariables.DocumentMapFilePath);
            var ribbon = Globals.Ribbons.GetRibbon<DocumentMapperRibbon>();

            if (string.IsNullOrEmpty(filePath))
            {
                ribbon.UnLinkDocumentMapBtn.Visible = false;
            }
            else
            {
                ribbon.LinkDocumentMapGroup.Visible = false;
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
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
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
