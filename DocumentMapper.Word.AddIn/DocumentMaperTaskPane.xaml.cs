using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        
        private void EnableDisableItemButtons(bool enabled)
        {
            DeleteMappedItemBtn.IsEnabled = enabled;
            EditMapping.IsEnabled = enabled;
            AddNewMapping.IsEnabled = enabled;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {                        
            DocumentMapTreeView.Items.Clear();
            PopulateTreeView();
        }

        private TreeViewItem CreateTreeViewItem(MappedItem mappedItem)
        {
            var treeItem= new TreeViewItem()
            {
                Tag = mappedItem.Id.ToString()
            };
            treeItem.Selected += new RoutedEventHandler(SelectedTreeViewItem);
            treeItem.FocusableChanged += new DependencyPropertyChangedEventHandler(TreeViewItemFocusChanged);

            return treeItem;
        }

        private void AddTreeNode(ref TreeViewItem treeViewItem, MappedItem mappedItem)
        {
            var button = new Button()
            {
                Content = "Add",
                Visibility = System.Windows.Visibility.Visible,
                Tag = mappedItem.Id.ToString(),
                Height = 10,
                Margin = new System.Windows.Thickness(10, 0, 0, 0)
            };

            button.Click += new System.Windows.RoutedEventHandler(MappedTreeViewItem_Click);
            
            var sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Children.Add(new Label() { Content = mappedItem.Name });
            sp.Children.Add(button);
                        
            treeViewItem.Header = sp;
        }

        private void PopulateTreeView()
        {
            //DocumentMapTreeView.Items.Clear();
            foreach (var item in DocumentMapping.Current.MappedItems)
            {
                var treeItem = CreateTreeViewItem(item);
                AddTreeNode(ref treeItem, item);
                

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
                var treeViewItem = CreateTreeViewItem(item);
                AddTreeNode(ref treeViewItem, item);

                treeItem.Items.Add(treeViewItem);

                if (item.ChildMappedItems.Any())
                {
                    BindTreeViewItem(ref treeViewItem, item.ChildMappedItems);
                }

            }
        }
               
        private static void CreateMapedItemTextControl(MappedItem mappedItem)
        {
            Microsoft.Office.Tools.Word.PlainTextContentControl textControl;
            var vstoDocument = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveDocument);
            textControl = vstoDocument.Controls.AddPlainTextContentControl(mappedItem.Name);
            //textControl.DataBindings.Add("Text", mappedItem, "Name");
            textControl.Text = mappedItem.Name;
            textControl.LockContents = true;
            textControl.Tag = mappedItem.Id.ToString();
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

        #region Events

        #region Treeview 

        public void SelectedTreeViewItem(object sender, RoutedEventArgs e)
        {
            if(sender is TreeViewItem)
            {
                var treeViewItem = (TreeViewItem)sender;
                EnableDisableItemButtons(true);
                var mapedItem = DocumentMapping.Current.MappedItemDictionary[treeViewItem.Tag.ToString()];

                MappedItemNotesTex.Text = mapedItem.Notes;

            }
        }

        public void TreeViewItemFocusChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(sender is Button)
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

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var currentSelection = Globals.ThisAddIn.Application.Selection;

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
                    if (!DocumentMapping.Current.MappedItemDictionary.ContainsKey(treeItem.Tag.ToString()))
                    {
                        throw new Exception($"Document map does not contain the item {treeItem.Tag.ToString()}");
                    }

                    var parentMappedItem = (MappedItem)DocumentMapping.Current.MappedItemDictionary[treeItem.Tag.ToString()];
                    mappedItem = new MappedItem(selectedText, DocumentMapping.Current, parentMappedItem);
                }

                DocumentMapping.Current.AddMappedItem(mappedItem);
                CreateMapedItemTextControl(mappedItem);
                AddItemToTree(mappedItem, treeItem);
            }
            catch (Exception ex)
            {

            }
        }

        private void EditMappedTreeViewItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DocumentMapTreeView.SelectedItem != null)
            {
                var selectedItem = (TreeViewItem)DocumentMapTreeView.SelectedItem;
                var editItemmappingWindow = new EditMappedItemWindow(ref selectedItem);
                editItemmappingWindow.Show();
            }
            EnableDisableItemButtons(false);
        }

        private void MappedTreeViewItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Button)
            {
                var button = (Button)sender;
                button.Visibility = Visibility.Hidden;

                var mappedItem = DocumentMapping.Current.MappedItemDictionary[button.Tag.ToString()];

                CreateMapedItemTextControl(mappedItem);
            }
        }

        private void DeleteMappedTreeViewItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DocumentMapTreeView.SelectedItem is TreeViewItem)
            {
                var selectedItem = (TreeViewItem)DocumentMapTreeView.SelectedItem;

                if (selectedItem.Items.Count == 0)
                {
                    DocumentMapping.RemoveMappedControls(selectedItem.Tag.ToString()).Await();
                    DocumentMapping.Current.DeleteMappedItem(selectedItem.Tag.ToString()).Await();
                    DocumentMapTreeView.Items.Remove(selectedItem);
                }
                else
                {

                }
            }

            EnableDisableItemButtons(false);
        }

        private void AddMappingBtn_Click(object sender, RoutedEventArgs e)
        {
            EditMappedItemWindow window;
            var currentSelection = Globals.ThisAddIn.Application.Selection;
            if (currentSelection != null && !string.IsNullOrEmpty(currentSelection.Text))
            {
                window = new EditMappedItemWindow(ref DocumentMapTreeView, currentSelection.Text);
            }
            else
            {
                window = new EditMappedItemWindow(ref DocumentMapTreeView);
            }

            window.Show();
        }


        #endregion


    }
}
