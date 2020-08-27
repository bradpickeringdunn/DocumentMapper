using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
               
        public List<MappedItem> ChildMappedItems {
            get
            {
                return _childMappedItems;
            }
        }

        private List<MappedItem> _childMappedItems = new List<MappedItem>();

        public void AddChildMappedItem(MappedItem newMappedItem)
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

    }
}
