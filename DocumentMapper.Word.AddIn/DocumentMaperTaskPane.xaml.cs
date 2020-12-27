using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DocumentMapper.Models;
using Microsoft.Office.Interop.Word;
using controls = System.Windows.Controls;

namespace DocumentMapper.Word.AddIn
{
    /// <summary>
    /// Interaction logic for DocumentMaperTaskPane.xaml
    /// </summary>
    public partial class DocumentMaperTaskPane : System.Windows.Controls.UserControl
    {
        TreeViewItem _SelectedTreeViewItem;
        MappedItem _SelectedMappedItem;

        public DocumentMaperTaskPane()
        {
            InitializeComponent();
            DocumentMapTreeView.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(TreeViewItemChanged);
        }

        #region Methods

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            PopulateTreeView().Await();
        }

        private async System.Threading.Tasks.Task PopulateTreeView()
        {
            DocumentMapTreeView.Items.Clear();
            //await TreeViewController.CreateTreeViewItems(DocumentMapTreeView.Items, DocumentMapping.Current.MappedItems, MappedTreeViewItem_Click);
        }

        private static void CreateMapedItemRangeControl(MappedItem mappedItem)
        {
            try
            {
                var range = Globals.ThisAddIn.Application.ActiveDocument.Range(0, 0);

                var vstoDocument = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveDocument);
                range.Text = mappedItem.Name;
                var bookmark = vstoDocument.Bookmarks.Add(mappedItem.Id.ToString(), range);

            }
            catch (Exception)
            {

            }
        }

        private static void CreateMapedItemTextControl(MappedItem mappedItem)
        {
            try
            {
                var selectedText = Globals.ThisAddIn.Application.Selection;
                selectedText.InsertBefore($"{mappedItem.Name} ");
                Microsoft.Office.Tools.Word.PlainTextContentControl textControl;
                
                var vstoDocument = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveDocument);
                textControl = vstoDocument.Controls.AddPlainTextContentControl(mappedItem.Name);
                textControl.Text = mappedItem.Name;
                textControl.LockContents = true;
                textControl.Title = string.Empty;
                textControl.Tag = mappedItem.Id.ToString();
                
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Events

        #region Treeview 

        private void TreeViewItemSelected(TreeViewItem selectedTreeViewItem)
        {
            _SelectedTreeViewItem = selectedTreeViewItem;
           // _SelectedMappedItem = DocumentMapping.Current.MappedItemDictionary[_SelectedTreeViewItem.Tag.ToString()];

            MappedItemNotesTex.Text = _SelectedMappedItem.Notes;

            var sp = (StackPanel)_SelectedTreeViewItem.Header;
            sp.Children[1].Visibility = Visibility.Visible;
            EditMappedItemsBtn.Visibility = Visibility.Visible;
        }

        public void TreeViewItemChanged<T>(object sender, RoutedPropertyChangedEventArgs<T> e)
        {
            var treeView = (TreeView)sender;
            if (_SelectedTreeViewItem != null)
            {
                var header = (StackPanel)_SelectedTreeViewItem.Header;
                header.Children[1].Visibility = Visibility.Hidden;
            }

            TreeViewItemSelected((TreeViewItem)treeView.SelectedItem);

        }

        public void TreeViewItemFocusChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Button)
            {
                var treeViewItemButton = (Button)sender;
                treeViewItemButton.Visibility = Visibility.Hidden;
            }
        }

        #endregion 

        void ThisDocument_SelectionChange(object sender, Microsoft.Office.Tools.Word.SelectionEventArgs e)
        {
            if (e.Selection.Characters.Count > 0)
            {
                System.Windows.Forms.MessageBox.Show("The selection in the document has changed.");
            }
        }

        #endregion

        #region Clicks

        private void RefreashBtn_Click(object sender, RoutedEventArgs e)
        {
            PopulateTreeView().Await();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedText = Globals.ThisAddIn.Application.Selection;
            var treeItem = (TreeViewItem)DocumentMapTreeView.SelectedItem;
            var mappedItem = default(MappedItem);

            if (treeItem == null)
            {
                mappedItem = DocumentMapping.AddMappedItem(selectedText.Text);
            }
            else
            {
                //if (!DocumentMapping.Current.MappedItemDictionary.ContainsKey(treeItem.Tag.ToString()))
                //{
                //    throw new Exception($"Document map does not contain the item {treeItem.Tag.ToString()}");
                //}

                //var parentMappedItem = (MappedItem)DocumentMapping.Current.MappedItemDictionary[treeItem.Tag.ToString()];
                //mappedItem = DocumentMapping.AddMappedItem(selectedText.Text, parentMappedItem);
            }

            if (mappedItem != default(MappedItem))
            {
                controls.ItemCollection itemCollection = treeItem != null ? treeItem.Items : DocumentMapTreeView.Items;
                TreeViewController.CreateTreeViewItems(itemCollection, new List<MappedItem> { mappedItem }, MappedTreeViewItem_Click).Await();
            }
        }

        private void EditMappedTreeViewItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var editItemsWindow = new EditMappedItemsControl();
            editItemsWindow.Show();
        }

        private void MappedTreeViewItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Button)
            {
                var button = (Button)sender;

               // var mappedItem = DocumentMapping.Current.MappedItemDictionary[button.Tag.ToString()];

              //  CreateMapedItemTextControl(mappedItem);

                var parent = (StackPanel)button.Parent;

                button.Visibility = Visibility.Hidden;
            }
        }

        private void AddMappingBtn_Click(object sender, RoutedEventArgs e)
        {
            EditMappedItemWindow window;
            var currentSelection = Globals.ThisAddIn.Application.Selection;
            var selectedText = currentSelection != null ? currentSelection.Text : string.Empty;
            var selectedItem = (TreeViewItem)DocumentMapTreeView.SelectedItem;

            window = new EditMappedItemWindow(selectedText, selectedItem);
            window.Closed += new EventHandler(XYZ);

            window.Show();
        }


        public void XYZ(object obj, EventArgs e)
        {
            PopulateTreeView().Await();
        }

        #endregion

    }
}
