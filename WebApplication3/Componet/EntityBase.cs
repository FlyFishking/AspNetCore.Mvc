using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace WebApplication3.Componet
{
    /// <summary>
    /// 可持久到数据库的领域模型的基类。
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    [Serializable]
    public abstract class EntityBase<TPrimaryKey>
    {
//        [NotMapped]
//        public virtual TPrimaryKey Key { get; set; }
    }

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
    public abstract class EntityBase : EntityBase<int> { }

    [Serializable]
    public abstract class EntityBaseWithDynamic : EntityBaseWithDynamic<int> { }
}