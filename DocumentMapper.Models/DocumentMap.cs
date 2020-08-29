using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DocumentMapper.Models
{
    [Serializable]
    [XmlRoot("DocumentMap")]
    public class DocumentMap
    {
        public DocumentMap(){}

        public DocumentMap(string linkedDocumentPath)
        {
            LinkedDocumentPaths = new List<string>() { linkedDocumentPath };
            Id = Guid.NewGuid();
        }

        [XmlIgnoreAttribute]
        public Dictionary<string, MappedItem> MappedItemDictionary
        {
            get
            {
                if (!_mappedItemDictionary.Any())
                {
                    foreach(var item in MappedItems)
                    {
                        _mappedItemDictionary.Add(item.Id.ToString(), item);
                    }
                }

                return _mappedItemDictionary;
            }
        }

        [XmlIgnoreAttribute]
        Dictionary<string, MappedItem> _mappedItemDictionary = new Dictionary<string, MappedItem>();

        [XmlArray("MappedItemArray")]
        [XmlArrayItem("MappedItemsObjekt")]
        public List<MappedItem> MappedItems { get; set; }

        [XmlElement("Id")]
        public Guid Id { get; set; }

        public void AddMappedItem(MappedItem mappedItem)
        {
            mappedItem.Name = mappedItem.Name.Trim();
            if (!MappedItemDictionary.ContainsKey(mappedItem.Id.ToString()))
            {
                if (mappedItem.ParentMappedItemId.HasValue)
                {
                    var parentItem = MappedItemDictionary[mappedItem.ParentMappedItemId.Value.ToString()];
                    if (parentItem != null)
                    {
                        mappedItem.Position = parentItem.ChildMappedItems.Count > 0 ? parentItem.ChildMappedItems.Count + 1 : 0;
                        parentItem.ChildMappedItems.Add(mappedItem);
                    }
                }
                else
                {
                    mappedItem.Position = this.MappedItems.Count > 0 ? this.MappedItems.Count + 1 : 0;
                    mappedItem.IsRootItem = true;
                    MappedItems.Add(mappedItem);
                }
            }

            if (!MappedItemDictionary.ContainsKey(mappedItem.Id.ToString()))
            {
                MappedItemDictionary.Add(mappedItem.Id.ToString(), mappedItem);
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

        /// <summary>
        /// Get's and sets all the documents that are linked to this map.
        /// </summary>
        public List<string> LinkedDocumentPaths { get; set; }

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

        public async Task DeleteMappedItem(string mappedItemId)
        {
            var item = MappedItemDictionary[mappedItemId];
            if (item.ParentMappedItemId.HasValue)
            {
                var parent = MappedItemDictionary[item.ParentMappedItemId.Value.ToString()];
                parent.DeleteChildMappedItem(item);
            }
            else
            {
                this.MappedItems.Remove(item);
            }

            await Task.FromResult(MappedItemDictionary.Remove(mappedItemId));
        }
    }
}
