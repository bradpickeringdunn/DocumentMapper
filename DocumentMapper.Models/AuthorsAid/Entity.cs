using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DocumentMapper.Models.AuthorsAid
{
    public class Entity
    {
        public Entity(string name, string entityType,  Guid? parentId = null)
        {
            this.Name = name;
            this.EntityType = entityType;
            Id = Guid.NewGuid();
            ParentId = parentId;
            ChildEntities= new List<Entity>();
        }

        [JsonProperty("id")]
        public Guid Id { get; }

        [JsonProperty("parentId")]
        public Guid? ParentId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("entityType")]
        public string EntityType { get; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("isRootItem")]
        public bool IsRootItem { get; set; }

        [JsonProperty("childEntities")]
        public IList<Entity> ChildEntities { get; set; }

        [JsonProperty("Notes")]
        public string Notes { get; set; }

        public void AddParent(MappedItem parentMapItem)
        {
            if (parentMapItem == null)
            {
                this.Position = 0;
                IsRootItem = true;
            }
            else
            {
                ParentId = parentMapItem.Id;
                IsRootItem = false;
            }
        }

        public void AddChildEntity(Entity childEntity)
        {
            childEntity.ParentId = this.Id;
            ChildEntities.Add(childEntity);
        }
    }
}
