using DocumentMapper.Models.AuthorsAid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TreeViewWithViewModelDemo.TextSearch;

namespace DocumentMapper.Word.AddIn.Windows
{
    /// <summary>
    /// Interaction logic for AddMappedEntityWindow.xaml
    /// </summary>
    public partial class AddMappedEntityWindow : Window
    {
        EntityTypeViewModel _viewModel;
        readonly Book _bookMap;

        EntityViewModel _setAsRootEntity = new EntityViewModel(new EntityReference(Guid.NewGuid(), "Set as root entity", Guid.NewGuid(), null));

        EntityViewModel selectedParent;

        string SelectedParentName = string.Empty;

        EntityType _selectedEntityType;

        public AddMappedEntityWindow()
        {
            InitializeComponent();

            _bookMap = DocumentMapping.CurrentBook;
            selectedParent = _setAsRootEntity;
            SelectedParentName = _setAsRootEntity.Name;
            ParentNodesLabel.Content = _setAsRootEntity.Name;

            // Create UI-friendly wrappers around the 
            // raw data objects (i.e. the view-model).
            _viewModel = new EntityTypeViewModel(_bookMap);

            // Let the UI bind to the view-model.
            UpdateEntityTreeViewSource();

            SetEntityTypeList();
        }

        private void UpdateEntityTreeViewSource()
        {
            var entitySource = new List<EntityViewModel>()
            {
                _setAsRootEntity
            };

            _viewModel = new EntityTypeViewModel(_bookMap);
            entitySource.AddRange(_viewModel.RootEntities);
            entitiesTreView.ItemsSource = entitySource;
            entitiesTreView.Items.Refresh();
            entitiesTreView.UpdateLayout();
        }

        private void TreeView_ParentEntityChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(sender is TreeView)
            {
                var treeView = sender as TreeView;
                selectedParent = (EntityViewModel)treeView.SelectedItem;
                ParentNodesLabel.Content = selectedParent.Name;
            }
        }

        #region Entity Type List

        private void SetEntityTypeList(Guid? selectedEntityTypeId = null)
        {
            var entityTypeList = new List<ListBoxItem>();
            foreach(var entityType in _bookMap.EntityTypes.Values)
            {
                entityTypeList.Add(new ListBoxItem()
                {
                    Content = String.IsNullOrEmpty(entityType.TypeName) ? "Unnamed" : entityType.TypeName,
                    Tag = entityType.Id
                }); 
            }

            entityTypeList.FirstOrDefault().IsSelected = true;
            if(!selectedEntityTypeId.HasValue)
            {
                _selectedEntityType = _bookMap.EntityTypes.First().Value;
            }

            entityTypesList.ItemsSource = entityTypeList;
        }

        private void entityTypesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            var selectedItem = listBox.SelectedItem as ListBoxItem;

            _viewModel.ChangeSelectedEntityType(Guid.Parse(selectedItem.Tag.ToString()));
            UpdateEntityTreeViewSource();
        }

        private void showAddNewEntityBtn_Click(object sender, RoutedEventArgs e)
        {
            showAddNewEntityBtn.Visibility = Visibility.Collapsed;
            newEntityTypePanel.Visibility = Visibility.Visible;
        }

        private void hideAddNewEntityBtn_Click(object sender, RoutedEventArgs e)
        {
            showAddNewEntityBtn.Visibility = Visibility.Visible;
            newEntityTypePanel.Visibility = Visibility.Collapsed;
        }

        private void addNewEntityBtn_Click(object sender, RoutedEventArgs e)
        {
            if (newEntityTypeTxt.Text.Length > 0)
            {
                _bookMap.AddEntityType(newEntityTypeTxt.Text);
                hideAddNewEntityBtn_Click(sender, e);

            }
        }

        #endregion

        private void AddMappedItem_Click_1(object sender, RoutedEventArgs e)
        {
            AddNewEntity();
            UpdateEntityTreeViewSource();
        }

        private void AddnClose_Click(object sender, RoutedEventArgs e)
        {
            AddNewEntity();
            Utils.SaveBookMap(_bookMap);
        }

        private void AddNewEntity()
        {
            if (selectedParent == null)
            {
                // TODO: error message
            }
            else if (String.IsNullOrEmpty(MappedItemText.Text))
            {
                // TODO: Error message
            }
            else if (_selectedEntityType == null)
            {

            }
            else
            {
                Guid? parentId = null;
                if (selectedParent != _setAsRootEntity)
                {
                    parentId = selectedParent.Id;
                }

                _bookMap.AddEntity(new Entity(MappedItemText.Text, _selectedEntityType.Id, parentId));
            }
        }
    }
}
