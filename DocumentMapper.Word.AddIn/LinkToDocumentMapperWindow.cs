using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocumentMapper.Word.AddIn
{
    public partial class LinkToDocumentMapperWindow : Form
    {
        public LinkToDocumentMapperWindow()
        {
            InitializeComponent();
        }

        private void CreateNewDocumentMap_Click(object sender, EventArgs e)
        {
            try
            {
                Utils.LinkDocumentMap(DocumentMapPathTxt.Text);
                this.Close();
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                _ = MessageBox.Show("YOur document needs to be saved before a document map can be created.  Save your document and try again.", "Create Document Map");
            }
        }

        private void BrowseFilePathBtn_Click(object sender, EventArgs e)
        {
            OpenDocumentMapFile.Filter = "xml files (*.xml)|*.xml";
            if (OpenDocumentMapFile.ShowDialog() == DialogResult.OK)
            {
                LinkNewDocumentMapBtn.Enabled = true;
                DocumentMapPathTxt.Text = OpenDocumentMapFile.FileName;
            }
        }
    }
}
