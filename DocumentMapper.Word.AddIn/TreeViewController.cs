using DocumentMapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace DocumentMapper.Word.AddIn
{
    public static class TreeViewController
    {
        public async static Task<List<System.Windows.Controls.TreeViewItem>> CreateTreeViewItems(IList<MappedItem> mappedItems)
        {
            List<TreeViewItem> treeViewItems = new List<TreeViewItem>();
            foreach (var item in mappedItems)
            {
                var treeItem = CreateTreeViewItem(item);
                treeViewItems.Add(treeItem);
            }

            return await Task.FromResult(treeViewItems);
        }

        public static TreeViewItem CreateTreeViewItem(MappedItem mappedItem, Action<object, RoutedEventArgs> selectedTreeViewItem = null, Action<object, DependencyPropertyChangedEventArgs> treeViewItemFocusChanged = null)
        {
            var treeItem = new TreeViewItem()
            {
                Tag = mappedItem.Id.ToString(),
                Header = mappedItem.Name
            };

            if (selectedTreeViewItem != null)
            {
                treeItem.Selected += new RoutedEventHandler(selectedTreeViewItem);
            }

            if (treeViewItemFocusChanged != null)
            {
                treeItem.FocusableChanged += new DependencyPropertyChangedEventHandler(treeViewItemFocusChanged);
            }

            if (mappedItem.ChildMappedItems.Any())
            {
                CreateTreeViewItem(mappedItem, selectedTreeViewItem, treeViewItemFocusChanged);
            }

            return treeItem;
        }

    }
}
