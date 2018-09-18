using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.EFCore.Infrustructure
{
    public class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : EntityBase<TKey>
    {
        public readonly DbContext DbContext;
        private DbSet<TEntity> DbSet { get; set; }
        public virtual IQueryable<TEntity> Entities => this.DbSet;

        //
        // 摘要:
        //     Occurs when a property of this collection (such as Microsoft.EntityFrameworkCore.ChangeTracking.LocalView`1.Count)
        //     is changing.
        public event PropertyChangingEventHandler PropertyChanging;
        //
        // 摘要:
        //     Occurs when a property of this collection (such as Microsoft.EntityFrameworkCore.ChangeTracking.LocalView`1.Count)
        //     changes.
        public event PropertyChangedEventHandler PropertyChanged;
        //
        // 摘要:
        //     Occurs when the contents of the collection changes, either because an entity
        //     has been directly added or removed from the collection, or because an entity
        //     starts being tracked, or because an entity is marked as Deleted.
        public event NotifyCollectionChangedEventHandler CollectionChanged;


        public RepositoryBase(DbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<TEntity>();

            DbSet.Local.PropertyChanging += PropertyChanging;
            DbSet.Local.PropertyChanged += PropertyChanged;
            DbSet.Local.CollectionChanged += CollectionChanged;
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }
        public TEntity GetByKey(TKey key)
        {
            return DbSet.Find(key);
        }
        public List<TEntity> GetAll()
        {
            return Entities.ToList();
        }
        public List<TEntity> GetMany(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.Where(predicate).ToList();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Entities.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Get Query as no tracking
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetQuery()
        {
            return GetQuery<TEntity>(null); // ((IQueryable<TEntity>)this.DbSet).AsNoTracking<TEntity>();
        }

        public virtual IQueryable<TEntity> GetQuery<TProperty>(Expression<Func<TEntity, TProperty>> includePath)
        {
            return includePath != null ?
                ((IQueryable<TEntity>)this.DbSet).AsNoTracking<TEntity>().Include<TEntity, TProperty>(includePath)
                : ((IQueryable<TEntity>)this.DbSet).AsNoTracking<TEntity>();
        }

        public virtual ObservableCollection<TEntity> GetObservable()
        {
            return this.DbSet.Local.ToObservableCollection();
        }

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
        public int ExecuteSqlCommand(string sqlQuery, params object[] parameters)
        {
            //reference Microsoft.EntityFrameworkCore.Relational.dll
            return DbContext.Database.ExecuteSqlCommand(sqlQuery, parameters);
        }

        public Task<int> ExecuteSqlCommandAsync(string sqlQuery, params object[] parameters)
        {
            //reference Microsoft.EntityFrameworkCore.Relational.dll
            return DbContext.Database.ExecuteSqlCommandAsync(sqlQuery, parameters);
        }

        public IEnumerable<TModel> SqlQuery<TModel>(string sql, params object[] parameters)
        {
            //            return (IEnumerable<TModel>)DbContext.Database.SqlQuery<TModel>(sql, parameters);
            throw new NotImplementedException();
        }

        public IEnumerable SqlQuery(Type elementType, string sql, params object[] parameters)
        {
            //            return DbContext.Database.SqlQuery(elementType, sql, parameters);
            throw new NotImplementedException();
        }

        public int Insert(TEntity entity, bool doSave = true)
        {
            if (entity == null) return 0;

            DbSet.Add(entity);
            return doSave ? SaveChanges() : 0;
        }
        public int Insert(IList<TEntity> entities, bool doSave = true)
        {
            if (entities == null || !entities.Any()) return 0;

            foreach (var entity in entities)
            {
                DbSet.Add(entity);
            }
            return doSave ? SaveChanges() : 0;
        }

        public int Update(TEntity entity, bool isSave = true)
        {
            if (entity == null) return 0;

            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            return isSave ? SaveChanges() : 0;
        }

        public int Delete(TEntity entity, bool isSave = true)
        {
            if (entity == null) return 0;

            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
            return isSave ? SaveChanges() : 0;
        }

        public int Delete(Expression<Func<TEntity, bool>> predicate, bool isSave = true)
        {
            if (predicate == null) return 0;

            var entity = GetMany(predicate);
            entity.ForEach(t => Delete(t, false));
            return isSave ? SaveChanges() : 0;
        }

        private int SaveChanges()
        {
            try
            {
//                var tracking = DbContext.ChangeTracker.Entries<EntityBase<TKey>>()
//                    .Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted ||
//                                p.State == EntityState.Modified).ToList();
//                foreach (var dbEntry in tracking)
//                {
//                    var auditableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(EditableAttribute), false)
//                        .SingleOrDefault() as EditableAttribute;
//                    if (auditableAttr == null)
//                        continue;
//
//                    Task.Factory.StartNew(() =>
//                    {
//                        var tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false)
//                            .SingleOrDefault() as TableAttribute;
//                        var tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;
//                        var moduleName = dbEntry.Entity.GetType().FullName.Split('.').Skip(1).FirstOrDefault();
//                        //Log Here
//
//                    });
//                }
                var result = DbContext.SaveChanges();
                return result;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("DbUpdateException occour", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception occour", ex);
            }
        }
    }

    public class RepositoryBase<TEntity> : RepositoryBase<TEntity, int>, IRepository<TEntity>
        where TEntity : EntityBase<int>
    {
        public RepositoryBase(DbContext dbContext)
            : base(dbContext)
        {

        }
    }
}