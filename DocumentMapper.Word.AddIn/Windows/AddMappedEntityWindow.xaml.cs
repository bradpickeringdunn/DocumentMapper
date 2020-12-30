using DocumentMapper.Models.AuthorsAid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        Entity _selectedEntity;

        public AddMappedEntityWindow()
        {
            InitializeComponent();

            DisplayEntityTypesControlPanel(true);

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

        #region Entity Tree View

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
                _selectedEntity = _bookMap.EntityManifest[selectedParent.Id];
                ParentNodesLabel.Content = selectedParent.Name;
                EntityNotesTextBox.Text = _selectedEntity.Notes;
            }
        }

        #endregion

        #region Entity Type List

        private void SetEntityTypeSetings(EntityType entityType)
        {
            _selectedEntityType = entityType;
        }

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
                SetEntityTypeSetings(_bookMap.EntityTypes.First().Value);
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
            DisplayAddEntityTypesPanel(true);
        }

        private void hideAddNewEntityBtn_Click(object sender, RoutedEventArgs e)
        {
            DisplayEntityTypesControlPanel(true);
        }

        private void addNewEntityBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(newEntityTypeTxt.Text))
            {
                _bookMap.AddEntityType(newEntityTypeTxt.Text);
                DisplayEntityTypesControlPanel(true);
            }
            else
            {
                var message = new StringBuilder("You need to supply a type / genre title.");
                message.AppendLine("E.g. Characters / locations");
                MessageBox.Show(message.ToString());
            }
        }

        private void EditEntityType_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEntityType != null)
            {
                DisplayEditEntityTypesBox(true);
                editTypeLabel.Content = $"Editing type: {_selectedEntityType.TypeName}";
                EditEntityTypeTxt.Text = _selectedEntityType.TypeName;
            }
            else
            {
                MessageBox.Show("You need to select a type to edit.");
            }
        }

        private void UpdateEntityType_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(EditEntityTypeTxt.Text))
            {
                MessageBox.Show("You need to enter a type name.");
                EditEntityTypeTxt.Focus();
            }
            else if (EditEntityTypeTxt.Text != _selectedEntityType.TypeName)
            {
                _selectedEntityType.TypeName = EditEntityTypeTxt.Text;
                _bookMap.UpdateEntityType(_selectedEntityType);
                entityTypesList.Items.Refresh();
                DisplayEntityTypesControlPanel(true);
            }
        }

        private void HideEditEntityTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            DisplayEntityTypesControlPanel(true);
        }

        private void DisplayAddEntityTypesPanel(bool visible)
        {
            if (visible)
            {
                DisplayEditEntityTypesBox(false);
                DisplayEntityTypesControlPanel(false);
                AddEntityTypePanel.Visibility = Visibility.Visible;
            }
            else
            {
                newEntityTypeTxt.Text = string.Empty;
                AddEntityTypePanel.Visibility = Visibility.Collapsed;
            }
        }

        private void DisplayEntityTypesControlPanel(bool visible)
        {
            if (visible)
            {
                DisplayAddEntityTypesPanel(false);
                DisplayEditEntityTypesBox(false);
                ListBoxControlPanel.Visibility = Visibility.Visible;
            }
            else
            {
                ListBoxControlPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void DisplayEditEntityTypesBox(bool visible)
        {
            if (visible)
            {
                DisplayEntityTypesControlPanel(false);
                DisplayAddEntityTypesPanel(false);
                ExitEntityTypePanel.Visibility = Visibility.Visible;
            }
            else
            {
                EditEntityTypeTxt.Text = string.Empty;
                ExitEntityTypePanel.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region Add Items

        private void AddMappedItem_Click_1(object sender, RoutedEventArgs e)
        {
            AddNewEntity();
            UpdateEntityTreeViewSource();
        }

        private void AddnClose_Click(object sender, RoutedEventArgs e)
        {
            AddNewEntity();
            Utils.SaveBookMap(_bookMap);
            MappedItemText.Text = string.Empty;
            this.Close();
        }

        private void AddNewEntity()
        {
            if (selectedParent == null)
            {
                MessageBox.Show("Select either root, or a parent entity entity must either make your eior have a parent.");
            }
            else if (String.IsNullOrEmpty(MappedItemText.Text))
            {
                MessageBox.Show("You need to enter a name for the entity you want to add.");
            }
            else if (_selectedEntityType == null)
            {
                MessageBox.Show("First you need to select an entity type / genre.");
            }
            else
            {
                Guid? parentId = null;
                if (selectedParent != _setAsRootEntity)
                {
                    parentId = selectedParent.Id;
                }

                var newEntity = new Entity(MappedItemText.Text, _selectedEntityType.Id, parentId);
                newEntity.Notes = EntityNotesTextBox.Text;
                _bookMap.AddEntity(newEntity);
            }
        }

        #endregion
    }
}
