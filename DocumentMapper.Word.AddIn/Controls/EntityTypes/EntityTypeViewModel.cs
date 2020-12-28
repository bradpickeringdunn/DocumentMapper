using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using DocumentMapper.Models.AuthorsAid;

namespace TreeViewWithViewModelDemo.TextSearch
{
    /// <summary>
    /// This is the view-model of the UI.  It provides a data source
    /// for the TreeView (the FirstGeneration property), a bindable
    /// SearchText property, and the SearchCommand to perform a search.
    /// </summary>
    public class EntityTypeViewModel
    {
        #region Data

        readonly ReadOnlyCollection<EntityViewModel> _firstGeneration;
        readonly IList<EntityViewModel> _rootEntities = new List<EntityViewModel>();
        readonly ICommand _searchCommand;

        IEnumerator<EntityViewModel> _matchingPeopleEnumerator;
        string _searchText = String.Empty;

        #endregion // Data

        #region Constructor

        public EntityTypeViewModel(ICollection<EntityReference> rootEntities)
        {
            foreach (var entity in rootEntities)
            {
                _rootEntities.Add(new EntityViewModel(entity));
            }

            _firstGeneration = new ReadOnlyCollection<EntityViewModel>(_rootEntities);

            _searchCommand = new SearchFamilyTreeCommand(this);
        }

        #endregion // Constructor

        #region Properties

        #region FirstGeneration

        /// <summary>
        /// Returns a read-only collection containing the first person 
        /// in the family tree, to which the TreeView can bind.
        /// </summary>
        public ReadOnlyCollection<EntityViewModel> FirstGeneration
        {
            get { return _firstGeneration; }
        }

        #endregion // FirstGeneration

        #region SearchCommand

        /// <summary>
        /// Returns the command used to execute a search in the family tree.
        /// </summary>
        public ICommand SearchCommand
        {
            get { return _searchCommand; }
        }

        private class SearchFamilyTreeCommand : ICommand
        {
            readonly EntityTypeViewModel _familyTree;

            public SearchFamilyTreeCommand(EntityTypeViewModel familyTree)
            {
                _familyTree = familyTree;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            event EventHandler ICommand.CanExecuteChanged
            {
                // I intentionally left these empty because
                // this command never raises the event, and
                // not using the WeakEvent pattern here can
                // cause memory leaks.  WeakEvent pattern is
                // not simple to implement, so why bother.
                add { }
                remove { }
            }

            public void Execute(object parameter)
            {
                _familyTree.PerformSearch();
            }
        }

        #endregion // SearchCommand

        #region SearchText

        /// <summary>
        /// Gets/sets a fragment of the name to search for.
        /// </summary>
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (value == _searchText)
                    return;

                _searchText = value;

                _matchingPeopleEnumerator = null;
            }
        }

        #endregion // SearchText

        #endregion // Properties

        #region Search Logic

        void PerformSearch()
        {
            if (_matchingPeopleEnumerator == null || !_matchingPeopleEnumerator.MoveNext())
                this.VerifyMatchingPeopleEnumerator();

            var person = _matchingPeopleEnumerator.Current;

            if (person == null)
                return;

            // Ensure that this person is in view.
            if (person.Parent != null)
                person.Parent.IsExpanded = true;

            person.IsSelected = true;
        }

        void VerifyMatchingPeopleEnumerator()
        {
            var matches = this.FindMatches(_searchText, _rootEntities.First());
            _matchingPeopleEnumerator = matches.GetEnumerator();

            if (!_matchingPeopleEnumerator.MoveNext())
            {
                MessageBox.Show(
                    "No matching names were found.",
                    "Try Again",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );
            }
        }

        IEnumerable<EntityViewModel> FindMatches(string searchText, EntityViewModel person)
        {
            if (person.NameContainsText(searchText))
                yield return person;

            foreach (EntityViewModel child in person.Children)
                foreach (EntityViewModel match in this.FindMatches(searchText, child))
                    yield return match;
        }

        #endregion // Search Logic
    }
}