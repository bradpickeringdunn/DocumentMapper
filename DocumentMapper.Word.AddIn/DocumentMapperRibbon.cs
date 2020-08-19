using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Tools.Ribbon;

namespace DocumentMapper.Word.AddIn
{
    public partial class DocumentMapperRibbon
    {
        private void DocumentMapperRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void NewDocumentMapBtn_Click(object sender, RibbonControlEventArgs e)
        {
            var documentMapperSettings = new NewDocumentMapperWindow();

            documentMapperSettings.ShowDialog();
        }
    }
}
