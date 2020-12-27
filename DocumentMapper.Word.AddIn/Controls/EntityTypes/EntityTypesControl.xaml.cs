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

namespace DocumentMapper.Word.AddIn.Controls.EntityTypes
{
    /// <summary>
    /// Interaction logic for EntityTypesControl.xaml
    /// </summary>
    public partial class EntityTypesControl : UserControl
    {
        public EntityTypesControl()
        {
            InitializeComponent();
            var book = DocumentMapping.CurrentBook;
            var viewModel = new EntityTypeViewModel(book.EntityTypes.First());
        }
    }
}
