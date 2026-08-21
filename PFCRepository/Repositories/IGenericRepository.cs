using Microsoft.EntityFrameworkCore.Query;
using System.Data;
using System.Linq.Expressions;

namespace PFCRepository.Repositories
{
    /// <summary>
    /// GenericRepository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// GetAllEntities
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAllEntities(bool disabledTracking = true);

        /// <summary>
        /// GetAllEntitiesAsync
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllEntitiesAsync(bool disabledTracking = true);

        /// <summary>
        /// GetAllEntitiesAsync
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllEntitiesAsync(string SqlQuery, bool disabledTracking = true);

        /// <summary>
        /// GetAllEntities
        /// </summary>
        /// <param name="SqlQuery"></param>
        /// <param name="disabledTracking"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAllEntities(string SqlQuery, bool disabledTracking = true);

        /// <summary>
        /// GetAllEntities
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <param name="disabledTracking"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAllEntities(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disabledTracking = true);

        /// <summary>
        /// GetEntityByID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetEntityByID(object id);

        /// <summary>
        /// GetAllEntitiesByIDAsync
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="include"></param>
        /// <param name="disabledTracking"></param>
        /// <returns></returns>
        Task<TEntity> GetAllEntitiesByIDAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disabledTracking = true);

        /// <summary>
        /// InsertEntity
        /// </summary>
        /// <param name="entityToAdd"></param>
        /// <returns></returns>
        TEntity InsertEntity(TEntity entityToAdd);

        /// <summary>
        /// InsertMultipleEntities
        /// </summary>
        /// <param name="entitiesToAdd"></param>
        /// <returns></returns>
        void InsertMultipleEntities(List<TEntity> entitiesToAdd);

        /// <summary>
        /// InsertMultipleEntitiesAsync
        /// </summary>
        /// <param name="entitiesToAdd"></param>
        /// <returns></returns>
        void InsertMultipleEntitiesAsync(List<TEntity> entitiesToAdd);

        /// <summary>
        /// InsertEntityAsync
        /// </summary>
        /// <param name="entityToAdd"></param>
        /// <returns></returns>
        Task<TEntity> InsertEntityAsync(TEntity entityToAdd);

        /// <summary>
        /// UpdateEntity
        /// </summary>
        /// <param name="entityToUpdate"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        int UpdateEntity(TEntity entityToUpdate, object key);

        /// <summary>
        /// UpdateEntity
        /// </summary>
        /// <param name="entityToUpdate"></param>
        /// <returns></returns>
        int UpdateEntity(TEntity entityToUpdate);

        /// <summary>
        /// UpdateEntityAsync
        /// </summary>
        /// <param name="entityToUpdate"></param>
        /// <returns></returns>
        Task<int> UpdateEntityAsync(TEntity entityToUpdate);

        /// <summary>
        /// UpdateEntityAsync
        /// </summary>
        /// <param name="entityToUpdate"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<int> UpdateEntityAsync(TEntity entityToUpdate, object key);

        /// <summary>
        /// DeleteEntityByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> DeleteEntityByIdAsync(object id);

        /// <summary>
        /// DeleteEntity
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <returns></returns>
        TEntity DeleteEntity(TEntity entityToDelete);

        /// <summary>
        /// DeleteEntityAsync
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <returns></returns>
        Task<TEntity> DeleteEntityAsync(TEntity entityToDelete);

        /// <summary>
        /// GetSingle
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity GetSingle(Expression<Func<TEntity, bool>> predicate, bool disabledTracking = true);

        /// <summary>
        /// GetFirst
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity GetFirst(Expression<Func<TEntity, bool>> predicate, bool disabledTracking = true);

        /// <summary>
        /// GetSingleAsync
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool disabledTracking = true);

        /// <summary>
        /// GetFirstAsync
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, bool disabledTracking = true);

        /// <summary>
        /// GetMany
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> predicate, bool disabledTracking = true);

        /// <summary>
        /// GetManyQueryable
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetManyQueryable(Func<TEntity, bool> predicate, bool disabledTracking = true);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="predicate"></param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// GetManyAsync
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> predicate, bool disabledTracking = true);

        /// <summary>
        /// GetQueryable
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetQueryable(Func<TEntity, bool> predicate, bool disabledTracking = true);

        /// <summary>
        /// GetManyQueryable
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetManyQueryable(bool disabledTracking = true);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="entityObject"></param>
        void Delete(TEntity entityObject);

        /// <summary>
        /// GetWithInclude
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetWithInclude(Expression<Func<TEntity, bool>> predicate, bool disabledTracking = true, params string[] include);

        /// <summary>
        /// ExecuteQuery
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        int ExecuteQuery(string commandText, params object[] sqlParameters);

        /// <summary>
        /// GetDataWithDataTable
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        DataTable GetDataWithDataTable(string commandText);

        /// <summary>
        /// GetDataWithDataTable
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        DataTable GetDataWithDataTable(string sqlQuery, string connectionString);
    }
}
