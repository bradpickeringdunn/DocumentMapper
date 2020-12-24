using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocumentMapper.Models.AuthorsAid
{
    public class Book
    {
        public Book(string title)
        {
            Title = title;
            EntityTypes = new List<EntityType>();
            Chapters = new List<Chapter>();
            EntityManifest = new Dictionary<Guid, Entity>();
        }

        [JsonProperty("title")]
        public string Title { get; internal set; }

        [JsonProperty("wordCount")]
        public int WordCount { get; internal set; }

        [JsonProperty("pageCount")]
        public int PageCount { get; internal set; }

        [JsonProperty("chapters")]
        public IList<Chapter> Chapters { get; internal set; }

        [JsonProperty("entityManifest")]
        public IDictionary<Guid, Entity> EntityManifest { get; internal set; }

        [JsonProperty("entityTypes")]
        public IList<EntityType> EntityTypes { get; internal set; }

        public void AddChapter(string fileLocation, string title = "")
        {
            var chapterNumber = Chapters.Count + 1;
            title = !string.IsNullOrEmpty(title) ? title : $"Unnamed Chapter {chapterNumber}";
            Chapters.Add(new Chapter(this.EntityTypes, chapterNumber, title, fileLocation));
        }

        public void AddEntity(Entity newEntity)
        {
            EntityManifest.Add(newEntity.Id, newEntity);
            UpdateEntityType(new EntityReference(newEntity.Id, newEntity.Name, newEntity.EntityType, newEntity.ParentId));
        }

        private void UpdateEntityType(EntityReference entityReference)
        {
            var entityType = EntityTypes.FirstOrDefault(x => x.TypeName == entityReference.EntityType);
            
            if(entityType == default)
            {
                entityType = new EntityType(entityReference.EntityType);
                EntityTypes.Add(entityType);
            }

            entityType.AddEntityReference(entityReference);

        }

        public void AddParentToEntity(Guid entityId, Guid parentEntityId)
        {
            if(EntityManifest.TryGetValue(entityId, out var childEntity) && EntityManifest.TryGetValue(parentEntityId, out var parentEntity))
            {
                if (childEntity.EntityType == parentEntity.EntityType)
                {
                    childEntity.ParentId = parentEntity.Id;
                    var entityType = EntityTypes.First(x => x.TypeName == parentEntity.EntityType);
                    entityType.AddParentEntity(childEntity.Id, parentEntity.Id);
                }

            }
        }

        public void RemoveParentFromEntity(Guid childId1, Guid parentId)
        {
            if (EntityManifest.TryGetValue(childId1, out var childEntity) && EntityManifest.TryGetValue(parentId, out var parentEntity))
            {
                childEntity.ParentId = null;
                var entityType = EntityTypes.First(x => x.TypeName == parentEntity.EntityType);
                entityType.RemoveParentEntity(childEntity.Id, parentEntity.Id);
            }
        }
    }
}
