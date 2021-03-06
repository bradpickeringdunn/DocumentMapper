﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Tools.Ribbon;

namespace DocumentMapper.Word.AddIn
{
    public partial class DocumentMapperRibbon
    {
        private void NewDocumentMapBtn_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                Globals.ThisAddIn.Application.ActiveDocument.Save();
                var documentMapperSettings = new NewDocumentMapperWindow();
                documentMapperSettings.ShowDialog();
            }
            catch(System.Runtime.InteropServices.COMException)
            {
                _ = MessageBox.Show("YOur document needs to be saved before a document map can be created.  Save your document and try again.", "Create Document Map");
            }

        }

        private void UnLinkDocumentMapBtn_Click(object sender, RibbonControlEventArgs e)
        {
            Utils.UnLinkDocumentMap();
        }

        private void LinkDocumentMapBtn_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                Globals.ThisAddIn.Application.ActiveDocument.Save();
                var linkToDocumentMapper = new LinkToDocumentMapperWindow();
                linkToDocumentMapper.ShowDialog();
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                _ = MessageBox.Show("YOur document needs to be saved before a document map can be linked.  Save your document and try again.", "Link Document Map");
            }
        }
    }
}
