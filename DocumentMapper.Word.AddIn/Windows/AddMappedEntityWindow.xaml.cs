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
            AddEntityTreeViewSource();

            SetEntityTypeList();
        }

        private void AddEntityTreeViewSource()
        {
            var entitySource = new List<EntityViewModel>()
            {
                _setAsRootEntity
            };

            entitySource.AddRange(_viewModel.RootEntities);
            entitiesTreView.ItemsSource = entitySource;
            entitiesTreView.Items.Refresh();
            entitiesTreView.UpdateLayout();
        }

        private void AddMappedItem_Click(object sender, RoutedEventArgs e)
        {

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

        private void SetEntityTypeList()
        {
            entityTypesList.ItemsSource = _viewModel.EntityTypes;
            if (_viewModel.EntityTypes.Any())
            {
                _viewModel.SelecedEntityType = _viewModel.EntityTypes.First();
                entityTypesList.SelectedItem = _viewModel.EntityTypes;
            }
        }

        private void entityTypesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox)
            {
                var listBox = sender as ListBox;
                var selectedItem = listBox.SelectedItem as EntityType;

                _viewModel.ChangeSelectedEntityType(selectedItem.Id);
                AddEntityTreeViewSource();
            }
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


    }
}
