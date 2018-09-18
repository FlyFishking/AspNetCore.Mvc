using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace Microsoft.EFCore.Infrustructure
{
    [Serializable]
    public abstract class EntityBaseWithDynamic<TPrimaryKey> : EntityBase<TPrimaryKey>
    {
        protected EntityBaseWithDynamic()
        {
            DynamicObj = new ExpandoObject();
        }

        [NotMapped]
        public dynamic DynamicObj { get; set; }
    }

    [Serializable]
    public abstract class EntityBaseWithDynamic : EntityBaseWithDynamic<int> { }

}