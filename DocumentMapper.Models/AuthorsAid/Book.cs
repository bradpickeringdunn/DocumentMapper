using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocumentMapper.Models.AuthorsAid
{
    public class Book
    {
        Dictionary<Guid, EntityType> _entityTypes;

        [JsonConstructor]
        public Book(string title)
        {
            Title = title;

            var entityType = new EntityType("");
            _entityTypes = new Dictionary<Guid, EntityType>();
            _entityTypes.Add(entityType.Id, entityType);
            
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
        public IReadOnlyDictionary<Guid, EntityType> EntityTypes {
            get
            {
                return _entityTypes;
            }
            private set
            {
                _entityTypes.Clear();
                foreach (var entitytype in value)
                {
                    _entityTypes.Add(entitytype.Value.Id, entitytype.Value);
                }
            }
        }

        public void AddChapter(string fileLocation, string title = "")
        {
            var chapterNumber = Chapters.Count + 1;
            title = !string.IsNullOrEmpty(title) ? title : $"Unnamed Chapter {chapterNumber}";
            Chapters.Add(new Chapter(this.EntityTypes, chapterNumber, title, fileLocation));
        }

        public void AddEntity(Entity newEntity, Entity parentEntity = null)
        {
            if(!EntityTypes.ContainsKey(newEntity.EntityTypeId))
            {
                // TODO: handle this
            }
            else if (!EntityManifest.ContainsKey(newEntity.Id))
            {
                if(parentEntity != null && EntityManifest.ContainsKey(parentEntity.Id))
                {
                    newEntity.ParentId = parentEntity.Id;
                }

                EntityManifest.Add(newEntity.Id, newEntity);
                UpdateEntityTypeReferences(new EntityReference(newEntity.Id, newEntity.Name, newEntity.EntityTypeId, newEntity.ParentId));
            }
        }

        private void UpdateEntityTypeReferences(EntityReference entityReference)
        {
            if (EntityTypes.TryGetValue(entityReference.EntityTypeId, out var entityType))
            {
                entityType.AddEntityReference(entityReference);
            }
        }

        public void AddParentToEntity(Guid entityId, Guid parentEntityId)
        {
            if(EntityManifest.TryGetValue(entityId, out var childEntity) && EntityManifest.TryGetValue(parentEntityId, out var parentEntity))
            {
                if (childEntity.EntityTypeId == parentEntity.EntityTypeId)
                {
                    childEntity.ParentId = parentEntity.Id;
                    var entityType = EntityTypes.First(x => x.Key == parentEntity.EntityTypeId);
                    entityType.Value.AddParentEntity(childEntity.Id, parentEntity.Id);
                }
            }
        }

        public EntityType AddEntityType(string text)
        {
            var entityType = new EntityType(text);
            _entityTypes.Add(entityType.Id, entityType);
            return entityType;
        }

        public void RemoveParentFromEntity(Guid childId1, Guid parentId)
        {
            if (EntityManifest.TryGetValue(childId1, out var childEntity) && EntityManifest.TryGetValue(parentId, out var parentEntity))
            {
                childEntity.ParentId = null;
                var entityType = EntityTypes.First(x => x.Key == parentEntity.EntityTypeId);
                entityType.Value.RemoveParentEntity(childEntity.Id, parentEntity.Id);
            }
        }

        public void UpdateEntityType(EntityType updateEntityType)
        {
            if (_entityTypes.TryGetValue(updateEntityType.Id, out var entityType))
            {
                entityType.TypeName = updateEntityType.TypeName;
            }
        }
    }
}
