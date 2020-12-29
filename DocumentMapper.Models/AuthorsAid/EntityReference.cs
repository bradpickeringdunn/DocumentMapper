using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentMapper.Models.AuthorsAid
{
    public class EntityReference
    {
        public EntityReference(Guid id, string title, Guid entityTypeId, Guid? parentId)
        {
            Id = id;
            Title = title;
            EntityTypeId = entityTypeId;
            ParentId = parentId;
            ChildReferences = new Dictionary<Guid, EntityReference>();
        }

        [JsonProperty("id")]
        public Guid Id { get; }

        [JsonProperty("title")]
        public string Title { get; }

        [JsonProperty("parentId")]
        public Guid? ParentId { get; set; }

        [JsonProperty("entityType")]
        public Guid EntityTypeId { get; }

        [JsonProperty]
        public IDictionary<Guid, EntityReference> ChildReferences { get; }
    }
}
