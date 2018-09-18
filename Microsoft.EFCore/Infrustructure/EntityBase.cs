using System;

namespace Microsoft.EFCore.Infrustructure
{
    [Serializable]
    public abstract class EntityBase<TPrimaryKey>
    {
        //        [NotMapped]
        //        public virtual TPrimaryKey Key { get; set; }
    }

    [Serializable]
    public abstract class EntityBase : EntityBase<int> { }
}