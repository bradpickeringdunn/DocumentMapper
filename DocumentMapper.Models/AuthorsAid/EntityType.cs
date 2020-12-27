using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocumentMapper.Models.AuthorsAid
{
    public class EntityType
    {
        public EntityType(string name)
        {
            TypeName = name;
            EntityReferences = new Dictionary<Guid, EntityReference>();
        }

        [JsonProperty("typeName")]
        public string TypeName { get; set; }

        [JsonProperty("entityReferences")]
        public IDictionary<Guid, EntityReference> EntityReferences { get; set; }

        internal EntityReference FindEntityReference(Guid entityReferenceId)
        {
            var result = default(EntityReference);
            EntityReferences.TryGetValue(entityReferenceId, out result);
               
            if (result == default)
            {
                foreach (var entityReference in EntityReferences.Values)
                {
                    if (entityReference.ChildReferences.TryGetValue(entityReferenceId, out result))
                    {
                        break;
                    }
                }
            }

            return result;
        }

        internal void AddEntityReference(EntityReference entityReference)
        {
            if (!entityReference.ParentId.HasValue && !EntityReferences.ContainsKey(entityReference.Id))
            {
                EntityReferences.Add(entityReference.Id, entityReference);
            }
            else
            {
                var parentEntityReference = FindEntityReference(entityReference.ParentId.Value);
                if (!parentEntityReference.ChildReferences.ContainsKey(entityReference.Id))
                {
                    parentEntityReference.ChildReferences.Add(entityReference.Id, entityReference);
                }
            }

            void ValidateIsUnique(IDictionary<Guid, EntityReference> entityReferences, EntityReference reference)
            {
                if (!entityReferences.ContainsKey(reference.Id))
                {

                }
            }
        }

        internal void AddParentEntity(Guid childId1, Guid parentid)
        {
            var newparentReference = FindEntityReference(parentid);
            var childReference = FindEntityReference(childId1);
            var oldParentId = childReference.ParentId;
            
            childReference.ParentId = parentid;

            newparentReference.ChildReferences.Add(childReference.Id, childReference);
            RemoveReference(childReference.Id, oldParentId);
        }

        internal void RemoveParentEntity(Guid childId1, Guid parentid)
        {
            var oldparentReference = FindEntityReference(parentid);
            var childReference = FindEntityReference(childId1);
            
            childReference.ParentId = null;

            EntityReferences.Add(childReference.Id, childReference);
            RemoveReference(childReference.Id, oldparentReference.ParentId);
        }

        private void RemoveReference(Guid id, Guid? parentId)
        {
            if (!parentId.HasValue)
            {
                EntityReferences.Remove(id);
            }
            else
            {
                var oldParent = FindEntityReference(parentId.Value);
                oldParent.ChildReferences.Remove(id);
            }
        }
    }
}
