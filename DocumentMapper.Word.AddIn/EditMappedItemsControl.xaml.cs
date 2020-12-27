using DocumentMapper.Models;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace DocumentMapper.Word.AddIn
{
    /// <summary>
    /// Interaction logic for EditMappedItemsControl.xaml
    /// </summary>
    public partial class EditMappedItemsControl : Window
    {
        MappedItem _selectedMappedItem = null;
        TreeViewItem _selectedTreeView = null;
        string _selectedText = string.Empty;

        const string _parentRootLavel = "ROOT";

        public EditMappedItemsControl()
        {
            InitializeComponent();
            PopulateTreeView().Await();
            MappedItemTreeView.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(SelectedTreeViewItem);
        }

        #region Add Items

        private void AddMappedItemtn_Click(object sender, RoutedEventArgs e)
        {
            var newMappedItem = new MappedItem()
            {
                Name = MappedItemText.Text,
                Notes = new TextRange(ItemMapNotesText.Document.ContentStart, ItemMapNotesText.Document.ContentEnd).Text
            };

            if (_selectedTreeView == null || _selectedTreeView.Header.ToString() == _parentRootLavel)
            {
                MappedItemTreeView.Items.Add(CreateTreeViewItem(newMappedItem));
            }
            else
            {
                newMappedItem.ParentMappedItemId = _selectedMappedItem.Id;
                _selectedTreeView.Items.Add(CreateTreeViewItem(newMappedItem));
            }

            ///DocumentMapping.Current.AddMappedItem(newMappedItem);
            ChangeSelectedTreeViewItem(_selectedTreeView);
        }

        private TreeViewItem CreateTreeViewItem(MappedItem newMappedItem)
        {
            return new TreeViewItem()
            {
                Header = newMappedItem.Name,
                Tag = newMappedItem.Id.ToString()
            };
        }

        private void SaveAndCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            var path = Utils.DocumentMapperFilelocation();
            //Utils.SaveDocumentMap(DocumentMapping.Current, path);
            this.Close();
        }

        private void UpdateMappedItemtn_Click(object sender, RoutedEventArgs e)
        {
            _selectedMappedItem.Name = MappedItemText.Text;
            _selectedTreeView.Header = MappedItemText.Text;
            _selectedMappedItem.Notes = new TextRange(ItemMapNotesText.Document.ContentStart, ItemMapNotesText.Document.ContentEnd).Text;
        }

        private void AddNewMappedItemBtn_Click(object sender, RoutedEventArgs e)
        {
            AddMappedItemtn.Visibility = Visibility.Visible;
            UpdateMappedItemtn.Visibility = Visibility.Hidden;
            MappedItemText.Text = string.Empty;
        }

        private void SelectedTreeViewItem<T>(object sender, RoutedPropertyChangedEventArgs<T> e)
        {
            var treeView = (TreeView)sender;
            ChangeSelectedTreeViewItem((TreeViewItem)treeView.SelectedItem);
        }

        private void ChangeSelectedTreeViewItem(TreeViewItem treeViewItem)
        {
            _selectedTreeView = treeViewItem;
            ParentNodesLabel.Content = _selectedTreeView.Header.ToString();
            if (_selectedTreeView.Tag != null)
            {
               // _selectedMappedItem = DocumentMapping.Current.MappedItemDictionary[_selectedTreeView.Tag.ToString()];
                MappedItemText.Text = _selectedMappedItem.Name;
                AddMappedItemtn.Visibility = Visibility.Hidden;
                UpdateMappedItemtn.Visibility = Visibility.Visible;
            }
            else
            {
                AddMappedItemtn.Visibility = Visibility.Visible;
                UpdateMappedItemtn.Visibility = Visibility.Hidden;
            }
        }
        
        #endregion

        #region Treeview Events

        private async Task PopulateTreeView()
        {
            MappedItemTreeView.Items.Add(new TreeViewItem()
            {
                Header = "ROOT"
            });
            //await TreeViewController.CreateTreeViewItems(MappedItemTreeView.Items, DocumentMapping.Current.MappedItems);
        }

        private void TreeViewItemChanged<T>(object sender, RoutedPropertyChangedEventArgs<T> e)
        {
            var treeView = (TreeView)sender;
            _selectedTreeView = (TreeViewItem)treeView.SelectedItem;
          //  _selectedMappedItem = _selectedTreeView.Tag != null ? DocumentMapping.Current.MappedItemDictionary[_selectedTreeView.Tag.ToString()] : null;
            ParentNodesLabel.Content = _selectedMappedItem != null ? _selectedMappedItem.Name : _parentRootLavel;

            if (_selectedMappedItem != null)
            {
                ItemMapNotesText.Document.Blocks.Add(new Paragraph(new Run(_selectedMappedItem.Notes)));
            }

        }

        #endregion

        #region TreeView Drag and Drop

        Point _lastMouseDown;
        TreeViewItem draggedItem, _target;


        private void TreeView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _lastMouseDown = e.GetPosition(MappedItemTreeView);
            }

        }
        private void treeView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point currentPosition = e.GetPosition(MappedItemTreeView);


                    if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                        (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                    {
                        draggedItem = (TreeViewItem)MappedItemTreeView.SelectedItem;
                        if (draggedItem != null)
                        {
                            DragDropEffects finalDropEffect = DragDrop.DoDragDrop(MappedItemTreeView, MappedItemTreeView.SelectedValue,
                                DragDropEffects.Move);
                            //Checking target is not null and item is dragging(moving)
                            if ((finalDropEffect == DragDropEffects.Move) && (_target != null))
                            {
                                // A Move drop was accepted
                                if (!draggedItem.Header.ToString().Equals(_target.Header.ToString()))
                                {
                                    CopyItem(draggedItem, _target);
                                    _target = null;
                                    draggedItem = null;
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            try
            {

                Point currentPosition = e.GetPosition(MappedItemTreeView);


                if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                    (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                {
                    // Verify that this is a valid drop and then store the drop target
                    TreeViewItem item = GetNearestContainer(e.OriginalSource as UIElement);
                    if (CheckDropTarget(draggedItem, item))
                    {
                        e.Effects = DragDropEffects.Move;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.None;
                    }
                }
                e.Handled = true;
            }
            catch (Exception)
            {
            }
        }
        private void treeView_Drop(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

                // Verify that this is a valid drop and then store the drop target
                TreeViewItem TargetItem = GetNearestContainer(e.OriginalSource as UIElement);
                if (TargetItem != null && draggedItem != null)
                {
                    _target = TargetItem;
                    e.Effects = DragDropEffects.Move;

                }
            }
            catch (Exception)
            {
            }



        }
        private bool CheckDropTarget(TreeViewItem _sourceItem, TreeViewItem _targetItem)
        {
            //Check whether the target item is meeting your condition
            bool _isEqual = false;
            if (!_sourceItem.Header.ToString().Equals(_targetItem.Header.ToString()))
            {
                _isEqual = true;
            }
            return _isEqual;

        }
        private void CopyItem(TreeViewItem _sourceItem, TreeViewItem _targetItem)
        {
            //Asking user wether he want to drop the dragged TreeViewItem here or not
            if (MessageBox.Show("Would you like to drop " + _sourceItem.Header.ToString() + " into " + _targetItem.Header.ToString() + "", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    //adding dragged TreeViewItem in target TreeViewItem
                    addChild(_sourceItem, _targetItem);

                    //finding Parent TreeViewItem of dragged TreeViewItem 
                    TreeViewItem ParentItem = FindVisualParent<TreeViewItem>(_sourceItem);
                    // if parent is null then remove from TreeView else remove from Parent TreeViewItem
                    if (ParentItem == null)
                    {
                        MappedItemTreeView.Items.Remove(_sourceItem);
                    }
                    else
                    {
                        ParentItem.Items.Remove(_sourceItem);
                    }
                }
                catch
                {

                }
            }

        }
        public void addChild(TreeViewItem _sourceItem, TreeViewItem _targetItem)
        {
           // var sourceMappedItem = DocumentMapping.Current.MappedItemDictionary[_sourceItem.Tag.ToString()];
           // var parentMappedItem = DocumentMapping.Current.MappedItemDictionary[_targetItem.Tag.ToString()];
          //  DocumentMapping.Current.ChangeParentItem(sourceMappedItem, parentMappedItem);

            // add item in target TreeViewItem 
            TreeViewItem item1 = new TreeViewItem();
            item1.Header = _sourceItem.Header;
            _targetItem.Items.Add(item1);
            foreach (TreeViewItem item in _sourceItem.Items)
            {
                addChild(item, item1);
            }
        }
        static TObject FindVisualParent<TObject>(UIElement child) where TObject : UIElement
        {
            if (child == null)
            {
                return null;
            }

            UIElement parent = VisualTreeHelper.GetParent(child) as UIElement;

            while (parent != null)
            {
                TObject found = parent as TObject;
                if (found != null)
                {
                    return found;
                }
                else
                {
                    parent = VisualTreeHelper.GetParent(parent) as UIElement;
                }
            }

            return null;
        }
        private TreeViewItem GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            TreeViewItem container = element as TreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }
            return container;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        #endregion

    }
}
