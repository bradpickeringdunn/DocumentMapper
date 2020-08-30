using DocumentMapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using controls = System.Windows.Controls;

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
                    Tag = item.Id.ToString(),
                    Height = 14,
                    Margin = new System.Windows.Thickness(10, 0, 0, 0)
                };

                button.Click += new System.Windows.RoutedEventHandler(addItemClick);

                var sp = new controls.StackPanel();
                sp.Height = 20;
                sp.Orientation = System.Windows.Controls.Orientation.Horizontal;
                sp.Children.Add(new System.Windows.Controls.Label() { Content = item.Name });
                sp.Children.Add(button);
                sp.Children[1].Visibility = Visibility.Hidden;

                treeViewItem.Header = sp;

                if (item.ChildMappedItems.Any())
                {
                    await CreateTreeViewItems(treeViewItem.Items, item.ChildMappedItems, addItemClick);
                }

                itemCollection.Add(treeViewItem);
            }

        }

        public async static Task CreateTreeViewItems(controls.ItemCollection itemCollection, IList<MappedItem> mappedItems, string selectedTreeItemId = null)
        {
            foreach (var item in mappedItems)
            {
                var treeViewItem = new controls.TreeViewItem()
                {
                    Header = item.Name,
                    Tag = item.Id,
                    IsSelected = string.IsNullOrEmpty(selectedTreeItemId) && selectedTreeItemId == item.Id.ToString() ? true : false
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
