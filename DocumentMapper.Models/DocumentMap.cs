using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace DocumentMapper.Models
{
    [Serializable]
    public class DocumentMap
    {
        public DocumentMap()
        {}

        public DocumentMap(string linkedDocument)
        {
            LinkedDocuments = new List<string>() { linkedDocument };
            Id = Guid.NewGuid();
        }

        public List<MappedItem> MappedItems
        {
            get
            {
                return _mappedItems;
            }
        }

        public void AddMappedItem(MappedItem mappedItem)
        {
            if (mappedItem.ParentMappedItemId.HasValue)
            {
                var parentItem = FindMappedItem(mappedItem.ParentMappedItemId.Value);
                if(parentItem != null)
                {
                    mappedItem.Position = parentItem.ChildMappedItems.Count > 0 ? parentItem.ChildMappedItems.Count + 1 : 0;
                    parentItem.ChildMappedItems.Add(mappedItem);
                }
            }
            else
            {
                mappedItem.Position = this.MappedItems.Count > 0 ? this.MappedItems.Count + 1 : 0;
                mappedItem.IsRootItem = true;
                this.MappedItems.Add(mappedItem);
            }
        }

        private MappedItem FindMappedItem(Guid mappedItemId)
        {
            var foundItem = default(MappedItem);

            FindInChildItems(this.MappedItems);

            return foundItem;

            void FindInChildItems(IList<MappedItem> childMappedItems)
            {
                foreach (var childitem in childMappedItems)
                {
                    if (childitem.Id == mappedItemId)
                    {
                        foundItem = childitem;
                        break;
                    }
                    else if (childitem.ChildMappedItems.Any())
                    {
                        FindInChildItems(childitem.ChildMappedItems);
                    }
                }
            }
        }

        public Guid Id { get; set; }

        List<MappedItem> _mappedItems = new List<MappedItem>();

        /// <summary>
        /// Get's and sets all the documents that are linked to this map.
        /// </summary>
        public List<string> LinkedDocuments { get; set; }

        public MappedItem FindMappedItem(Guid mappedItemId, IList<MappedItem> childMappedItems = null)
        {
            var foundItem = default(MappedItem);

            childMappedItems = childMappedItems ?? MappedItems;
            foreach (var childitem in childMappedItems)
            {
                if (childitem.Id == mappedItemId)
                {
                    foundItem = childitem;
                    break;
                }
                else if (childitem.ChildMappedItems.Any())
                {
                    foundItem = FindMappedItem(mappedItemId, childitem.ChildMappedItems);
                }
            }

            return foundItem;
        }
    }
}
