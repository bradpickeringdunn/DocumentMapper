using DocumentMapper.Models.AuthorsAid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentMapper.Word.AddIn.Controls.EntityTypes
{
    public class EntityViewModel
    {
        public IReadOnlyCollection< EntityViewModel> ChildEntityReferences { get; }

        public EntityViewModel(EntityReference entityReference)
        {
            EntityName = entityReference.Title;
            EntityId = entityReference.Id;

            var entityReferences = new List<EntityViewModel>();
            foreach(var reference in entityReference.ChildReferences.Values)
            {
                entityReferences.Add(new EntityViewModel(reference));
            }

            ChildEntityReferences = entityReferences;
        }

        public string EntityName { get; }
        public Guid EntityId { get; }
    }
}
