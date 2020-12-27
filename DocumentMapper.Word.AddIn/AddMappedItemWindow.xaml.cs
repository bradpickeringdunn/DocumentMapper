using DocumentMapper.Models;
using System;
using System.Threading.Tasks;
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
        MappedItem _selectedMappedItem = null;
        TreeViewItem _selectedTreeView = null;
        string _selectedText = string.Empty;

        const string _parentRootLavel = "ROOT";

        public EditMappedItemWindow(string selectedText, TreeViewItem selectedTreeView = null)
        {
            _selectedTreeView = selectedTreeView;
            InitializeComponent();
            MappedItemText.Text = selectedText;
            PopulateTreeView().Await();
        }

        private async Task PopulateTreeView()
        {
            MappedItemTreeView.Items.Add(new TreeViewItem()
            {
                Header = "INSERT AT ROOT",
                IsSelected = true
            });
            var selecteMappedItemId = _selectedTreeView != null && _selectedTreeView.Tag !=null ? _selectedTreeView.Tag.ToString() : null;
            //await TreeViewController.CreateTreeViewItems(MappedItemTreeView.Items, DocumentMapping.Current.MappedItems, selecteMappedItemId);
            MappedItemTreeView.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(TreeViewItemChanged);
        }

        private void AddUpdateItem()
        {
            if (!string.IsNullOrEmpty(MappedItemText.Text))
            {
                MappedItem newMappedItem = null;
                if (_selectedMappedItem == null)
                {
                   // newMappedItem = new MappedItem(MappedItemText.Text, DocumentMapping.Current);
                }
                else
                {
                  //  newMappedItem = new MappedItem(MappedItemText.Text, DocumentMapping.Current, _selectedMappedItem);
                }

                newMappedItem.Notes = new TextRange(ItemMapNotesText.Document.ContentStart, ItemMapNotesText.Document.ContentEnd).Text;

              //  DocumentMapping.Current.AddMappedItem(newMappedItem);

               // Utils.SaveDocumentMap(DocumentMapping.Current);

                PopulateTreeView().Await();
            }
        }

        private void AddUpdateCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            AddUpdateItem();
            DocumentMapping.MapDocumentControlToMappedItem(_selectedMappedItem.Id.ToString()).Await();
            this.Close();
        }

        private void AddUpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            AddUpdateItem();
            PopulateTreeView().Await();
            MappedItemText.Text = string.Empty;
            ItemMapNotesText.Document.Blocks.Clear();
        }

        private void TreeViewItemChanged<T>(object sender, RoutedPropertyChangedEventArgs<T> e)
        {
            var treeView = (TreeView)sender;
            _selectedTreeView = (TreeViewItem)treeView.SelectedItem;
           // _selectedMappedItem = _selectedTreeView.Tag != null ? DocumentMapping.Current.MappedItemDictionary[_selectedTreeView.Tag.ToString()] : null;
            ParentNodesLabel.Content = _selectedMappedItem != null ? _selectedMappedItem.Name : _parentRootLavel;

            if (_selectedMappedItem != null)
            {
                ItemMapNotesText.Document.Blocks.Add(new Paragraph(new Run(_selectedMappedItem.Notes)));
            }
                        
        }
    }
}
