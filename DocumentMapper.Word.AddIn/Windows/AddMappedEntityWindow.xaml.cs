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

namespace DocumentMapper.Word.AddIn.Windows
{
    /// <summary>
    /// Interaction logic for AddMappedEntityWindow.xaml
    /// </summary>
    public partial class AddMappedEntityWindow : Window
    {
        readonly EntityTypeViewModel _entityRefs;

        public AddMappedEntityWindow()
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

        private void AddMappedItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
