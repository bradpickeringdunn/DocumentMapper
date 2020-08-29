using DocumentMapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DocumentMapper.Word.AddIn
{
    /// <summary>
    /// Interaction logic for EditMappedItemWindow.xaml
    /// </summary>
    public partial class EditMappedItemWindow : Window
    {
        MappedItem mappedItem;
        TreeViewItem selectedItem;
        string selectedText = string.Empty;
        TreeView mainTreeView;

        public EditMappedItemWindow(ref TreeView treeView, string selectedText = null)
        {
            InitializeComponent();
            mainTreeView = treeView;
            MappedItemText.Text = selectedText != null ? selectedText : string.Empty;
            AddUpdateMappedItemtn.Content = "Add Item";

            var mappedItems = DocumentMapping.Current.MappedItems;
            PopulateTreeView(mappedItems).Await();
            
        }

        private async Task PopulateTreeView(IList<MappedItem> mappedItems)
        {
            var treeViewItems = await TreeViewController.CreateTreeViewItems(mappedItems);
            MappedItemTreeView.Items.Add(new TreeViewItem()
            {
                Header = "CREATE A ROOT ITEM"
            });

            foreach (var item in treeViewItems)
            {
                MappedItemTreeView.Items.Add(item);
            }
        }
               
        private void UpdateMappedItemtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MappedItemText.Text))
            {
                if (mappedItem == null)
                {
                    mappedItem = new MappedItem(MappedItemText.Text, DocumentMapping.Current);
                }
                else
                {
                    mappedItem.Name = MappedItemText.Text;
                }

                mappedItem.Notes = new TextRange(ItemMapNotesText.Document.ContentStart, ItemMapNotesText.Document.ContentEnd).Text;

                DocumentMapping.Current.AddMappedItem(mappedItem);
                DocumentMapping.MapDocumentControlToMappedItem(mappedItem.Id.ToString()).Await();
                this.Close();
            }
            else
            {

            }
        }
    }
}
