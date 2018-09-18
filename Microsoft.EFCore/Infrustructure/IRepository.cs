using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microsoft.EFCore.Infrustructure
{
    public interface IRepository<TEntity, in TKey>
        where TEntity : EntityBase<TKey>
    {
        //
        // 摘要:
        //     Occurs when a property of this collection (such as Microsoft.EntityFrameworkCore.ChangeTracking.LocalView`1.Count)
        //     is changing.
        event PropertyChangingEventHandler PropertyChanging;
        //
        // 摘要:
        //     Occurs when a property of this collection (such as Microsoft.EntityFrameworkCore.ChangeTracking.LocalView`1.Count)
        //     changes.
        event PropertyChangedEventHandler PropertyChanged;
        //
        // 摘要:
        //     Occurs when the contents of the collection changes, either because an entity
        //     has been directly added or removed from the collection, or because an entity
        //     starts being tracked, or because an entity is marked as Deleted.
        event NotifyCollectionChangedEventHandler CollectionChanged;
        IQueryable<TEntity> Entities { get; }
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        TEntity GetByKey(TKey key);
        List<TEntity> GetAll();
        List<TEntity> GetMany(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get Query as no tracking
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetQuery();

        /// <summary>
        /// Get Query as no tracking
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="includePath"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetQuery<TProperty>(Expression<Func<TEntity, TProperty>> includePath);

        ObservableCollection<TEntity> GetObservable();

        /// <summary>
        /// Execute original sql comman
        /// <example>
        /// <![CDATA[
        /// 1、FormattableString sqlQuery = $"update student set name ='{"new name"}'";
        ///    DbContext.Database.ExecuteSqlCommand(sqlQuery);
        /// 2、ExecuteSqlCommand("update student set name=name where id = @id", new SqlParameter[] {new SqlParameter("@id", 10)}
        /// ]]>
        /// </example>
        /// </summary>
        /// <param name="sqlQuery">sql command</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sqlQuery, params object[] parameters);
        Task<int> ExecuteSqlCommandAsync(string sqlQuery, params object[] parameters);
        IEnumerable<TModel> SqlQuery<TModel>(string sql, params object[] parameters);
        IEnumerable SqlQuery(Type elementType, string sql, params object[] parameters);
        int Insert(TEntity entity, bool doSave = true);
        int Insert(IList<TEntity> entities, bool doSave = true);
        int Update(TEntity entity, bool isSave = true);
        int Delete(TEntity entity, bool isSave = true);
        int Delete(Expression<Func<TEntity, bool>> predicate, bool isSave = true);
    }

    public interface IRepository<TEntity> : IRepository<TEntity, int>
        where TEntity : EntityBase<int>
    {

    }

}