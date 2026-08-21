using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using PFCRepository.Utilities;
using System.Data;
using System.Linq.Expressions;
using PFCRepository.DatabaseContext;

namespace PFCRepository.Repositories
{
    /// <summary>
    /// GenericRepository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        #region Private member variables...


        private PFCDBContext _dbContext;
        private DbSet<TEntity> _dbSet;

        #endregion

        #region Public Constructor...

        /// <summary>
        /// Public Constructor,initializes privately declared local variables.
        /// </summary>
        /// <param name="context"></param>
        public GenericRepository(PFCDBContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<TEntity>();
        }

        #endregion

        #region Public member methods...

        /// <summary>
        /// generic Get method for Entities
        /// <param name="disabledTracking"></param>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAllEntities(bool disabledTracking = true)
        {
            try
            {
                if (disabledTracking)
                {
                    return _dbSet.AsNoTracking().ToList();
                }
                else
                {
                    return _dbSet.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// generic Get async method for Entities
        /// <param name="disabledTracking"></param>
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllEntitiesAsync(bool disabledTracking = true)
        {
            try
            {
                if (disabledTracking)
                {
                    return await _dbSet.AsNoTracking().ToListAsync();
                }
                else
                {
                    return await _dbSet.ToListAsync();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// GetAllEntitiesAsync
        /// </summary>
        /// <param name="SqlQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllEntitiesAsync(string SqlQuery, bool disabledTracking = true)
        {
            try
            {
                //ExecuteSqlRawAsync use this for procedure as well...
                if (disabledTracking)
                {
                    return await _dbSet.FromSqlRaw(SqlQuery).AsNoTracking().ToListAsync();
                }
                else
                {
                    return await _dbSet.FromSqlRaw(SqlQuery).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public  IEnumerable<TEntity> GetAllEntities(string SqlQuery, bool disabledTracking = true)
        {
            try
            {
                //ExecuteSqlRawAsync use this for procedure as well...
                if (disabledTracking)
                {
                    return  _dbSet.FromSqlRaw(SqlQuery).AsNoTracking().ToList();
                }
                else
                {
                    return  _dbSet.FromSqlRaw(SqlQuery).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// GetAllEntities
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <param name="disabledTracking"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAllEntities(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disabledTracking = true)
        {
            try
            {
                IQueryable<TEntity> query = _dbSet;
                if (disabledTracking)
                {
                    query = query.AsNoTracking();
                }
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                if (include != null)
                {
                    query = include(query);
                }
                if (orderBy != null)
                {
                    return orderBy(query).ToList();
                }
                else
                {
                    return (query).ToList();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }





        /// <summary>
        /// GetEntityByID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetEntityByID(object id)
        {
            try
            {
                return _dbSet.Find(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// GetAllEntitiesByIDAsync
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="include"></param>
        /// <param name="disabledTracking"></param>
        /// <returns></returns>
        public async Task<TEntity> GetAllEntitiesByIDAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disabledTracking = true)
        {
            try
            {
                IQueryable<TEntity> query = _dbSet;
                if (disabledTracking)
                {
                    query = query.AsNoTracking();
                }
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                if (include != null)
                {
                    query = include(query);
                }


                return await query.FirstOrDefaultAsync();


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// generic Insert method for the entities
        /// </summary>
        /// <param name="entityToAdd"></param>
        public TEntity InsertEntity(TEntity entityToAdd)
        {
            if (entityToAdd == null) return null;
            try
            {
                TEntity entity = _dbSet.Add(entityToAdd).Entity;
                try
                {
                    _dbContext.SaveChanges();
                }

                catch (DbUpdateException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// generic Insert method for the entities
        /// </summary>
        /// <param name="entitiesToAdd"></param>
        /// 
        public void InsertMultipleEntities(List<TEntity> entitiesToAdd)
        {
            if (entitiesToAdd == null || entitiesToAdd.Count == 0) return;
            try
            {
                _dbSet.AddRange(entitiesToAdd);
                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        //public List<TEntity> InsertMultipleEntities(List<TEntity> entitiesToAdd)
        //{
        //    if (entitiesToAdd == null || entitiesToAdd.Count == 0) return null;
        //    try
        //    {
        //        List<TEntity> entities = _dbSet.AddRange(entitiesToAdd).Entity.ToList();
        //        try
        //        {
        //            _dbContext.SaveChanges();
        //        }
        //        catch (DbUpdateException)
        //        {
        //            throw;
        //        }
        //        //catch (DbEntityValidationException)
        //        //{
        //        //    throw;
        //        //}
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        return entities;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// generic Insert method for the entities
        /// </summary>
        /// <param name="entitiesToAdd"></param>
        /// 
        public async void InsertMultipleEntitiesAsync(List<TEntity> entitiesToAdd)
        {
            if (entitiesToAdd == null || entitiesToAdd.Count == 0) return;
            try
            {
                _dbSet.AddRange(entitiesToAdd);
                try
                {
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        //public async Task<List<TEntity>> InsertMultipleEntitiesAsync(List<TEntity> entitiesToAdd)
        //{
        //    if (entitiesToAdd == null || entitiesToAdd.Count == 0) return null;
        //    try
        //    {
        //        List<TEntity> entities = _dbSet.AddRange(entitiesToAdd).ToList();
        //        try
        //        {
        //            await _dbContext.SaveChangesAsync();
        //        }
        //        catch (DbUpdateException)
        //        {
        //            throw;
        //        }
        //        //catch (DbEntityValidationException)
        //        //{
        //        //    throw;
        //        //}
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        return entities;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}



        /// <summary>
        /// generic Insert async method for the entities
        /// </summary>
        /// <param name="entityToAdd"></param>
        public async Task<TEntity> InsertEntityAsync(TEntity entityToAdd)
        {
            if (entityToAdd == null) return null;
            try
            {
                TEntity entity = _dbSet.Add(entityToAdd).Entity;
                try
                {
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Generic update method for the entities
        /// </summary>
        /// <param name="entityToUpdate"></param>
        /// <param name="key"></param>
        public int UpdateEntity(TEntity entityToUpdate, object key)
        {
            int rowsUpdated = 0;
            if (entityToUpdate == null) return 0;
            try
            {
                TEntity existingEntity = _dbSet.Find(key);
                if (existingEntity == null) return 0;
                _dbContext.Entry(existingEntity).CurrentValues.SetValues(entityToUpdate);
                try
                {
                    rowsUpdated = _dbContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new Exception("Database_Record_Update");
                }
                catch (Exception)
                {
                    throw;
                }
                return rowsUpdated;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Generic update method for the entities
        /// </summary>
        /// <param name="entityToUpdate"></param>
        /// <param name="key"></param>
        public int UpdateEntity(TEntity entityToUpdate)
        {
            int rowsUpdated = 0;
            if (entityToUpdate == null) return 0;
            try
            {
                _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
                try
                {
                    rowsUpdated = _dbContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                catch (DbUpdateException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
                return rowsUpdated;
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// Generic update method for the entities
        /// </summary>
        /// <param name="entityToUpdate"></param>
        /// <param name="key"></param>
        public async Task<int> UpdateEntityAsync(TEntity entityToUpdate)
        {
            int rowsUpdated = 0;
            if (entityToUpdate == null) return 0;
            try
            {
                _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
                try
                {
                    rowsUpdated = await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new Exception("Database_Record_Update");
                }
                catch (Exception)
                {
                    throw;
                }
                return rowsUpdated;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Generic update async method for the entities
        /// </summary>
        /// <param name="entityToUpdate"></param>
        /// <param name="key"></param>
        public async Task<int> UpdateEntityAsync(TEntity entityToUpdate, object key)
        {
            int rowsUpdated = 0;
            if (entityToUpdate == null) return 0;
            try
            {
                TEntity existingEntity = await _dbSet.FindAsync(key);
                if (existingEntity == null) return 0;
                _dbContext.Entry(existingEntity).CurrentValues.SetValues(entityToUpdate);
                try
                {
                    rowsUpdated = await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new Exception("Database_Record_Update");
                }
                catch (Exception)
                {
                    throw;
                }
                return rowsUpdated;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Generic Delete method for deleting the entities
        /// </summary>
        /// <param name="entityToDelete"></param>
        public TEntity DeleteEntity(TEntity entityToDelete)
        {
            if (entityToDelete == null) return null;
            try
            {
                if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
                {
                    _dbSet.Attach(entityToDelete);
                }
                _dbContext.Entry(entityToDelete).State = EntityState.Modified;
                TEntity entity = _dbSet.Remove(entityToDelete).Entity;
                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new Exception("Database_Record_Delete");
                }
                catch (Exception)
                {
                    throw;
                }
                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Generic Delete async method for deleting an entity based on Id
        /// </summary>
        /// <param name="id"></param>
        public async Task<TEntity> DeleteEntityByIdAsync(object id)
        {
            try
            {
                TEntity entityToDelete = _dbSet.Find(id);
                return await DeleteEntityAsync(entityToDelete);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Generic Delete async method for deleting the entities
        /// </summary>
        /// <param name="entityToDelete"></param>
        public async Task<TEntity> DeleteEntityAsync(TEntity entityToDelete)
        {
            if (entityToDelete == null) return null;
            try
            {
                if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
                {
                    _dbSet.Attach(entityToDelete);
                }
                _dbContext.Entry(entityToDelete).State = EntityState.Modified;
                TEntity entity = _dbSet.Remove(entityToDelete).Entity;
                try
                {
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new Exception("Database_Record_Delete");
                }
                catch (Exception)
                {
                    throw;
                }
                return entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// generic get method , fetches data for the entities on the basis of condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity GetSingle(Expression<Func<TEntity, bool>> predicate, bool disabledTracking = true)
        {
            if (predicate == null) return null;
            try
            {
                if (disabledTracking)
                {
                    return _dbSet.AsNoTracking().SingleOrDefault(predicate);
                }
                else
                {
                    return _dbSet.SingleOrDefault(predicate);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// generic get method , fetches data for the entities on the basis of condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity GetFirst(Expression<Func<TEntity, bool>> predicate, bool disabledTracking = true)
        {
            if (predicate == null) return null;
            try
            {
                if (disabledTracking)
                {
                    return _dbSet.AsNoTracking().FirstOrDefault(predicate);
                }
                else
                {
                    return _dbSet.FirstOrDefault(predicate);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// generic get async method , fetches data for the entities on the basis of condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool disabledTracking = true)
        {
            if (predicate == null) return null;
            try
            {
                if (disabledTracking)
                {
                    return await _dbSet.AsNoTracking().SingleOrDefaultAsync(predicate);
                }
                else
                {
                    return await _dbSet.SingleOrDefaultAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// generic get async method , fetches data for the entities on the basis of condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, bool disabledTracking = true)
        {
            if (predicate == null) return null;
            try
            {
                if (disabledTracking)
                {
                    return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
                }
                else
                {
                    return await _dbSet.FirstOrDefaultAsync(predicate);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// generic method to get many records on the basis of a condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> predicate, bool disabledTracking = true)
        {
            if (predicate == null) return null;
            try
            {
                if (disabledTracking)
                {
                    return _dbSet.AsNoTracking().Where(predicate).ToList();
                }
                else
                {
                    return _dbSet.Where(predicate).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// generic method to get many record on the basis of a condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetManyQueryable(Func<TEntity, bool> predicate, bool disabledTracking = true)
        {
            if (predicate == null) return null;
            try
            {
                if (disabledTracking)
                {
                    return _dbSet.AsNoTracking().Where(predicate).ToList();
                }
                else
                {
                    return _dbSet.Where(predicate).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// generic method to get many record on the basis of a condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetManyQueryable(bool disabledTracking = true)
        {
            try
            {
                if (disabledTracking)
                {
                    return _dbSet.AsNoTracking();
                }
                else
                {
                    return _dbSet;
                }



            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// generic method to get many record on the basis of a condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetQueryable(Func<TEntity, bool> predicate, bool disabledTracking = true)
        {
            if (predicate == null) return null;
            try
            {
                if (disabledTracking)
                {
                    return _dbSet.AsNoTracking().Where(predicate).AsQueryable();
                }
                else
                {
                    return _dbSet.Where(predicate).AsQueryable();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// generic method to get many records asynchronously on the basis of a condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> predicate, bool disabledTracking = true)
        {
            if (predicate == null) return null;
            try
            {
                if (disabledTracking)
                {
                    return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
                }
                else
                {
                    return await _dbSet.Where(predicate).ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// generic delete method , deletes data for the entities on the basis of condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null) return;
            IQueryable<TEntity> objects = _dbSet.Where(predicate).AsQueryable();

            foreach (TEntity obj in objects)
            {
                _dbContext.Entry(obj).State = EntityState.Modified;
                _dbSet.Remove(obj);
            }
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Database_Record_Delete");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// generic delete method , deletes data for the entities on the basis of condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public void Delete(TEntity entityObject)
        {
            _dbContext.Entry(entityObject).State = EntityState.Modified;
            _dbSet.Remove(entityObject);
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("Database_Record_Delete");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// generic method to get many include multiple records on the basis of a condition but query able.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetWithInclude(Expression<Func<TEntity, bool>> predicate, bool disabledTracking = true, params string[] include)
        {
            if (predicate == null) return null;
            IQueryable<TEntity> query = _dbSet;
            if (disabledTracking)
            {
                query = query.AsNoTracking();
            }
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            try
            {
                return query.Where(predicate);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// ExecuteQuery with Sql Query
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public int ExecuteQuery(string commandText, params object[] sqlParameters)
        {
            int rowsUpdated = 0;
            try
            {
                try
                {
                    //rowsUpdated = _dbContext.Database.ExecuteSqlCommand(commandText, sqlParameters);
                    rowsUpdated = _dbContext.Database.ExecuteSqlRaw(commandText, sqlParameters);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
                return rowsUpdated;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// GetDataWithDataTable
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataTable GetDataWithDataTable(string commandText)
        {

            try
            {
                DataTable datatable = new DataTable();
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(commandText, AppConfig.ConnectionString))
                {

                    sqlDataAdapter.Fill(datatable);

                }

                return datatable;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// GetDataWithDataTable
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public DataTable GetDataWithDataTable(string sqlQuery, string connectionString)
        {
            DataTable dtRecords = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                if (connection.State == ConnectionState.Closed) connection.Open();

                using (SqlCommand command = new SqlCommand("SET ARITHABORT ON", connection))
                {
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 1200;
                    command.CommandText = sqlQuery;
                    SqlDataAdapter dtAdapter = new SqlDataAdapter(command);
                    dtAdapter.Fill(dtRecords);
                    command.Dispose();
                }
            }
            return dtRecords;
        }

        #endregion
    }
}
