using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;

namespace DocumentMapper.Models
{
    public class MappedItem
    {
        public MappedItem(){}

        public MappedItem(string name, DocumentMap documentMap, MappedItem parentMapItem = null)
        {
            this.Name = name;
            AddParent(documentMap, parentMapItem);
            DocumentMapId = documentMap.Id;
            Id = Guid.NewGuid();
        }

        private void AddParent(DocumentMap documentMap, MappedItem parentMapItem = null)
        {
            if(parentMapItem == null)
            {
                this.Position = documentMap.MappedItems.Count > 0 ? documentMap.MappedItems.Count + 1 : 0;
                IsRootItem = true;
            }
            else
            {
                ParentMappedItemId = parentMapItem.Id;
                IsRootItem = false;
            }
        }

        public Guid DocumentMapId { get; set; }

        public Guid Id { get; set; }

        public Guid? ParentMappedItemId { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }
        
        public bool IsRootItem { get; set; }
               
        public IList<MappedItem> FlattenChildItems(MappedItem item)
        {
            if(item == null)
            {
                FlattenChildItems(this);
            }

            var childMappedItems = new List<MappedItem>();

            foreach (var child in item.ChildMappedItems)
            {
                childMappedItems.Add(child);
                
                if (child.ChildMappedItems.Any())
                {
                    foreach(var childItem in child.ChildMappedItems)
                    {
                        childMappedItems.AddRange(FlattenChildItems(childItem));
                    }
                }
            }

            return childMappedItems;
        }

        public List<MappedItem> ChildMappedItems {
            get
            {
                return _childMappedItems;
            }
        }

        public string Notes { get; set; }

        private List<MappedItem> _childMappedItems = new List<MappedItem>();

        public void AddChildMappedItem(MappedItem newMappedItem, ref DocumentMap documentmap)
        {
            if(this.Name == newMappedItem.Name)
            {
                //Error message
            }

            newMappedItem.Position = _childMappedItems.Count > 0 ? _childMappedItems.Count + 1 : 0;
            _childMappedItems.Add(newMappedItem);
        }

        public string ThisMappedItemToXMLString()
        {
            MappedItem onlyThisMappedItem = this;
            onlyThisMappedItem.ChildMappedItems.Clear();
            using (StringWriter stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(onlyThisMappedItem.GetType());
                serializer.Serialize(stringwriter, onlyThisMappedItem);
                return stringwriter.ToString();
            }
        }

        internal void DeleteChildMappedItem(MappedItem mappedItem)
        {
            int? itemIdex = null;
            for(var i =0; i< ChildMappedItems.Count; i++)
            {
                if(ChildMappedItems[i].Id == mappedItem.Id)
                {
                    itemIdex = i;
                    break;
                }
            }

            if (itemIdex.HasValue)
            {
                ChildMappedItems.RemoveAt(itemIdex.Value);
            }
            
        }
    }
}
