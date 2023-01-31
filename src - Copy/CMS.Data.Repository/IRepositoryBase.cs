using CMS.Common;
using CMS.Data.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CMS.Data.Repository
{
    public interface IRepositoryBase<V, D> where V : DataModelBase where D : DataModelBase
    {
        IUserContext UserContext { get; set; }
        PostgreDbContext GetDbContext();
        Task DisposeDbContext(PostgreDbContext context);
        //PostgreDbContext DbContext { get; set; }

        // IMongoDatabase Mongo { get; set; }

        Task<List<V>> GetList();
        Task<List<V>> GetActiveList();
        Task<List<V>> GetList(Expression<Func<D, bool>> where, params Expression<Func<D, object>>[] include);
        //Task<List<V>> GetListIgnoreCase(Expression<Func<D, bool>> where);
        Task<V> GetSingle(Expression<Func<D, bool>> where, params Expression<Func<D, object>>[] include);
        // Task<V> GetSingleIgnoreCase(Expression<Func<D, bool>> where, params Expression<Func<D, object>>[] properties);
        Task<V> GetSingleGlobal(Expression<Func<D, bool>> where, params Expression<Func<D, object>>[] include);
        // Task<V> GetSingleGlobalIgnoreCase(Expression<Func<D, bool>> where, params Expression<Func<D, object>>[] properties);
        Task<V> GetSingleById(string id, params Expression<Func<D, object>>[] include);
        Task<V> Create(V model);
        Task<V> CreateMigrate(V model);
        Task<V> CreateGlobal(V model);
        Task<List<V>> CreateMany(List<V> models);
        Task<V> Edit(V model);
        Task<V> EditGlobal(V model);
        Task Delete(string id);
        Task<List<V>> GetListGlobal();
        Task<List<V>> GetListGlobal(Expression<Func<D, bool>> where, params Expression<Func<D, object>>[] include);
        // Task<List<V>> GetListGlobalIgnoreCase(Expression<Func<D, bool>> where);


        Task<List<VM>> GetList<VM, DM>() where VM : DataModelBase where DM : DataModelBase;
        Task<List<VM>> GetActiveList<VM, DM>() where VM : DataModelBase where DM : DataModelBase;
        Task<List<VM>> GetList<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] include) where VM : DataModelBase where DM : DataModelBase;
        // Task<List<VM>> GetListIgnoreCase<VM, DM>(Expression<Func<DM, bool>> where) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> GetSingle<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] include) where VM : DataModelBase where DM : DataModelBase;
        // Task<VM> GetSingleIgnoreCase<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] properties) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> GetSingleGlobal<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] include) where VM : DataModelBase where DM : DataModelBase;
        // Task<VM> GetSingleGlobalIgnoreCase<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] properties) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> GetSingleById<VM, DM>(string id, params Expression<Func<DM, object>>[] include) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> Create<VM, DM>(VM model) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> CreateMigrate<VM, DM>(VM model) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> CreateGlobal<VM, DM>(VM model) where VM : DataModelBase where DM : DataModelBase;

        Task<List<VM>> CreateMany<VM, DM>(List<VM> models) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> Edit<VM, DM>(VM model) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> EditGlobal<VM, DM>(VM model) where VM : DataModelBase where DM : DataModelBase;
        Task Delete<VM, DM>(string id) where VM : DataModelBase where DM : DataModelBase;
        Task<List<VM>> GetListGlobal<VM, DM>() where VM : DataModelBase where DM : DataModelBase;
        Task<List<VM>> GetListGlobal<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] include) where VM : DataModelBase where DM : DataModelBase;
        Task<string> UploadMongoFile(string fileName, byte[] bytes);
        Task<byte[]> DownloadMongoFile(string id);
        Task<string> DeleteMongoFile(string id);


        //  Task<List<VM>> GetListGlobalIgnoreCase<VM, DM>(Expression<Func<DM, bool>> where) where VM : DataModelBase where DM : DataModelBase;


    }
}
