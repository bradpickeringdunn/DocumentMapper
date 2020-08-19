using DocumentMapper.Models;
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
using System.Xml.Serialization;

namespace DocumentMapper.Word.AddIn
{
    public partial class NewDocumentMapperWindow : Form
    {
        string _Filepath { get; set; }

        public NewDocumentMapperWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(DocumentMapFilenameTxt.Text) || String.IsNullOrEmpty(FilePathTxt.Text)){
                MessageBox.Show("You must provide a file name and a file path", "File path error");
            }
            else {
                var newMap = new DocumentMap();
                newMap.LinkedDocuments.Add("some document path.");

                var writer = new XmlSerializer(typeof(DocumentMap));
                var path = $"{FilePathTxt.Text}/{DocumentMapFilenameTxt.Text}.xml";

                FileStream file = System.IO.File.Create(path);
                writer.Serialize(file, newMap);
                file.Close();
            }

        }

        private void BrowseFilePathBtn_Click(object sender, EventArgs e)
        {
            SaveNewDocumentMapDialog.ShowDialog();
            _Filepath = SaveNewDocumentMapDialog.SelectedPath;
            FilePathTxt.Text = _Filepath;
        }
    }
}
