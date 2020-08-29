using DocumentMapper.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace DocumentMapper.Word.AddIn
{
    /// <summary>
    /// Interaction logic for EditMappedItemWindow.xaml
    /// </summary>
    public partial class EditMappedItemWindow : Window
    {
        MappedItem selectedMappedItem;
        TreeViewItem selectedTreeView;
        string selectedText = string.Empty;
        TreeView mainTreeView;

        public EditMappedItemWindow(ref TreeView treeView, string selectedText, TreeViewItem selectedTreeView = null)
        {
            this.selectedTreeView = selectedTreeView;
            InitializeComponent();
            mainTreeView = treeView;
            MappedItemText.Text = selectedText;
            AddUpdateMappedItemtn.Content = "Add Item";
            PopulateTreeView();
        }

        private void PopulateTreeView()
        {
            MappedItemTreeView.Items.Add(new TreeViewItem()
            {
                Header = "INSERT AT ROOT"
            });
            TreeViewController.CreateTreeViewItems(MappedItemTreeView.Items, DocumentMapping.Current.MappedItems).Await();
        }

        private void UpdateMappedItemtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MappedItemText.Text))
            {
                if (selectedMappedItem == null)
                {
                    selectedMappedItem = new MappedItem(MappedItemText.Text, DocumentMapping.Current);
                }
                else
                {
                    selectedMappedItem.Name = MappedItemText.Text;
                }

                selectedMappedItem.Notes = new TextRange(ItemMapNotesText.Document.ContentStart, ItemMapNotesText.Document.ContentEnd).Text;

                DocumentMapping.Current.AddMappedItem(selectedMappedItem);
                DocumentMapping.MapDocumentControlToMappedItem(selectedMappedItem.Id.ToString()).Await();

                this.Close();
            }
            else
            {

            }
        }
    }
}
