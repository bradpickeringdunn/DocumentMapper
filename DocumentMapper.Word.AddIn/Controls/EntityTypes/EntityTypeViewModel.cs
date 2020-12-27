using DocumentMapper.Models.AuthorsAid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DocumentMapper.Word.AddIn.Controls.EntityTypes
{
    public class EntityTypeViewModel
    {
        public EntityTypeViewModel(EntityType entityType)
        {
            this.EntityTypeName = entityType.TypeName;

            IList<EntityViewModel> entityReferences = new List<EntityViewModel>();
            foreach (var reference in entityType.EntityReferences.Values)
            {
                entityReferences.Add(new EntityViewModel(reference));
            }

            this.EntityReferences = (ReadOnlyCollection<EntityViewModel>)entityReferences;
        }

        public string EntityTypeName { get; }
        public ReadOnlyCollection<EntityViewModel> EntityReferences{get;}
    }
}
