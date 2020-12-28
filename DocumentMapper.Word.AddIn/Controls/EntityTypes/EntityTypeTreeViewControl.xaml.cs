using DocumentMapper.Models.AuthorsAid;
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
using TreeViewWithViewModelDemo.TextSearch;

namespace DocumentMapper.Word.AddIn.Controls.EntityTypes
{
    /// <summary>
    /// Interaction logic for EntityTypeTreeViewControl.xaml
    /// </summary>
    public partial class EntityTypeTreeViewControl : UserControl
    {
        readonly EntityTypeViewModel _entityRefs;

        public EntityTypeTreeViewControl()
        {
            InitializeComponent();

            // Get raw family tree data from a database.
            var entityType = DocumentMapping.CurrentBook.EntityTypes.First();

            // Create UI-friendly wrappers around the 
            // raw data objects (i.e. the view-model).
            _entityRefs = new EntityTypeViewModel(entityType.EntityReferences.Values);

            // Let the UI bind to the view-model.
            base.DataContext = _entityRefs;

        }

        void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                _entityRefs.SearchCommand.Execute(null);
        }
    }
}
