using DocumentMapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using controls = System.Windows.Controls;
using System.Windows.Forms;

namespace DocumentMapper.Word.AddIn
{
    public static class TreeViewController
    {
        public async static Task CreateTreeViewItems(controls.ItemCollection itemCollection, IList<MappedItem> mappedItems, Action<object, RoutedEventArgs> addItemClick)
        {
            itemCollection.Clear();
            foreach (var item in mappedItems)
            {
                var treeViewItem = new controls.TreeViewItem()
                {
                    Tag = item.Id,
                };

                var button = new controls.Button()
                {
                    Content = "Add",
                    Visibility = System.Windows.Visibility.Visible,
                    Tag = item.Id.ToString(),
                    Height = 10,
                    Margin = new System.Windows.Thickness(10, 0, 0, 0)
                };

                button.Click += new System.Windows.RoutedEventHandler(addItemClick);

                var sp = new controls.StackPanel();
                sp.Orientation = System.Windows.Controls.Orientation.Horizontal;
                sp.Children.Add(new System.Windows.Controls.Label() { Content = item.Name });
                sp.Children.Add(button);

                treeViewItem.Header = sp;

                if (item.ChildMappedItems.Any())
                {
                    await CreateTreeViewItems(treeViewItem.Items, item.ChildMappedItems, addItemClick);
                }

                itemCollection.Add(treeViewItem);
            }

        }

        public async static Task CreateTreeViewItems(controls.ItemCollection itemCollection, IList<MappedItem> mappedItems)
        {
            foreach (var item in mappedItems)
            {
                var treeViewItem = new controls.TreeViewItem()
                {
                    Header = item.Name,
                    Tag = item.Id,
                };
                
                if (item.ChildMappedItems.Any())
                {
                    await CreateTreeViewItems(treeViewItem.Items, item.ChildMappedItems);
                }

                itemCollection.Add(treeViewItem);
            }

        }

    }
}
