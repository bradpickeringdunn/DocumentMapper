using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using DocumentMapper.Models.AuthorsAid;

namespace TreeViewWithViewModelDemo.TextSearch
{
    /// <summary>
    /// A UI-friendly wrapper around a Person object.
    /// </summary>
    public class EntityViewModel : INotifyPropertyChanged
    {
        #region Data

        readonly ReadOnlyCollection<EntityViewModel> _entities;
        readonly EntityViewModel _parent;
        readonly EntityReference _entity;

        bool _isExpanded;
        bool _isSelected;

        #endregion // Data

        #region Constructors

        public EntityViewModel(EntityReference entity)
            : this(entity, null)
        {
        }

        private EntityViewModel(EntityReference entity, EntityViewModel parent)
        {
            _entity = entity;
            _parent = parent;

            _entities = new ReadOnlyCollection<EntityViewModel>(
                    (from child in _entity.ChildReferences
                     select new EntityViewModel(child.Value, this))
                     .ToList<EntityViewModel>());
        }

        #endregion // Constructors

        #region Person Properties

        public ReadOnlyCollection<EntityViewModel> Children
        {
            get { return _entities; }
        }

        public string Name
        {
            get { return _entity.Title; }
        }

        #endregion // Person Properties

        #region Presentation Members

        #region IsExpanded

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (_isExpanded && _parent != null)
                    _parent.IsExpanded = true;
            }
        }

        #endregion // IsExpanded

        #region IsSelected

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        #endregion // IsSelected

        #region NameContainsText

        public bool NameContainsText(string text)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(this.Name))
                return false;

            return this.Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        #endregion // NameContainsText

        #region Parent

        public EntityViewModel Parent
        {
            get { return _parent; }
        }

        #endregion // Parent

        #endregion // Presentation Members        

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members
    }
}