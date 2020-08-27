using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml.Serialization;
using DocumentMapper.Models;
using Microsoft.Office.Interop.Word;

namespace DocumentMapper.Word.AddIn
{
    /// <summary>
    /// Interaction logic for DocumentMaperTaskPane.xaml
    /// </summary>
    public partial class DocumentMaperTaskPane : System.Windows.Controls.UserControl
    {

        public DocumentMaperTaskPane()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var documentMap = Utils.DocumentMap;
            //documentMap.LinkedDocuments

            foreach (var control in Globals.ThisAddIn.Application.ActiveWindow.Document.ContentControls)
                DocumentMapTreeView.Items.Clear();
            PopulateTreeView();
        }

        private void PopulateTreeView()
        {
            DocumentMapTreeView.Items.Clear();
            foreach (var item in DocumentMapping.Current.MappedItems)
            {
                var treeItem = new TreeViewItem()
                {
                    Header = item.Name,
                    Tag = item
                };

                if (item.ChildMappedItems.Any())
                {
                    BindTreeViewItem(ref treeItem, item.ChildMappedItems);
                }

                DocumentMapTreeView.Items.Add(treeItem);
            }
        }

        public void BindTreeViewItem(ref TreeViewItem treeItem, IList<MappedItem> mappedItems)
        {
            foreach (var item in mappedItems)
            {
                var treeViewItem = new TreeViewItem()
                {
                    Header = item.Name,
                    Tag = item
                };

                treeItem.Items.Add(treeViewItem);

                if (item.ChildMappedItems.Any())
                {
                    BindTreeViewItem(ref treeViewItem, item.ChildMappedItems);
                }

            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var currentSelection = Globals.ThisAddIn.Application.Selection;

            Microsoft.Office.Tools.Word.PlainTextContentControl textControl;
            var selectedText = currentSelection.Text.Trim();

            var treeItem = (TreeViewItem)DocumentMapTreeView.SelectedItem;
            try
            {
                MappedItem mappedItem;

                if (treeItem == null)
                {
                    mappedItem = new MappedItem(selectedText, DocumentMapping.Current);
                }
                else
                {
                    var parentMappedItem = (MappedItem)treeItem.Tag;
                    mappedItem = new MappedItem(selectedText, DocumentMapping.Current, (MappedItem)treeItem.Tag);
                }
                DocumentMapping.Current.AddMappedItem(mappedItem);

                Globals.ThisAddIn.Application.ActiveWindow.Document.CustomXMLParts.Add(mappedItem.ThisMappedItemToXMLString());
                var vstoDocument = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveDocument);
                textControl = vstoDocument.Controls.AddPlainTextContentControl(mappedItem.Name);
                //textControl.DataBindings.Add("Text", mappedItem, "Name");
                textControl.Text = mappedItem.Name;
                textControl.LockContents = true;
                textControl.Tag = mappedItem.Id.ToString();
                AddItemToTree(mappedItem, treeItem);
            }
            catch (Exception ex)
            {

            }
        }

        private void AddItemToTree(MappedItem mappedItem, TreeViewItem treeItem = null)
        {
            if (treeItem != null)
            {
                treeItem.Items.Add(new TreeViewItem()
                {
                    Header = mappedItem.Name,
                    Tag = mappedItem.Id.ToString()
                });;
            }
            else
            {
                treeItem = new TreeViewItem()
                {
                    Header = mappedItem.Name,
                    Tag = mappedItem.Id.ToString()
                };
                DocumentMapTreeView.Items.Add(treeItem);
            }
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            var treeItem = (TreeViewItem)DocumentMapTreeView.SelectedItem;
            if (treeItem != null)
            {
                var id = treeItem.Tag.ToString();
                if (DocumentMapping.FlattenedMappeditems.ContainsKey(id))
                {
                    var newName = "Some New name";
                    var mappedItem = (MappedItem)DocumentMapping.FlattenedMappeditems[id];
                    mappedItem.Name = newName;
                    treeItem.Header = newName;
                    DocumentMapping.MapDocumentControlToMappedItem(id).Await();
                }
            }
        }
    }
}
