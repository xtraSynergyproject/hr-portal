using Synergy.App.Common;
using Synergy.App.DataModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Synergy.App.Repository
{
    public interface IRepositoryBase<V, D> where V : DataModelBase where D : DataModelBase
    {
        IUserContext UserContext { get; set; }
        PostgreDbContext GetDbContext();
        Task CommitChanges();
        Task DisposeDbContext(PostgreDbContext context);

        Task<List<V>> GetList();
        Task<List<V>> GetActiveList();
        Task<List<V>> GetList(Expression<Func<D, bool>> where, params Expression<Func<D, object>>[] include);
        Task<V> GetSingle(Expression<Func<D, bool>> where, params Expression<Func<D, object>>[] include);
        Task<V> GetSingleGlobal(Expression<Func<D, bool>> where, params Expression<Func<D, object>>[] include);
        Task<V> GetSingleById(string id, params Expression<Func<D, object>>[] include);
        Task<V> Create(V model, bool autoCommit = true);
        Task<V> CreateMigrate(V model, bool autoCommit = true);
        Task<V> CreateGlobal(V model, bool autoCommit = true);
        Task<List<V>> CreateMany(List<V> models, bool autoCommit = true);
        Task<V> Edit(V model, bool autoCommit = true);
        Task<V> EditGlobal(V model, bool autoCommit = true);
        Task Delete(string id, bool autoCommit = true);
        Task<List<V>> GetListGlobal();
        Task<List<V>> GetListGlobal(Expression<Func<D, bool>> where, params Expression<Func<D, object>>[] include);


        Task<List<VM>> GetList<VM, DM>() where VM : DataModelBase where DM : DataModelBase;
        Task<List<VM>> GetActiveList<VM, DM>() where VM : DataModelBase where DM : DataModelBase;
        Task<List<VM>> GetList<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] include) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> GetSingle<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] include) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> GetSingleGlobal<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] include) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> GetSingleById<VM, DM>(string id, params Expression<Func<DM, object>>[] include) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> Create<VM, DM>(VM model, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> CreateMigrate<VM, DM>(VM model, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> CreateGlobal<VM, DM>(VM model, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase;

        Task<List<VM>> CreateMany<VM, DM>(List<VM> models, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> Edit<VM, DM>(VM model, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> EditGlobal<VM, DM>(VM model, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase;
        Task Delete<VM, DM>(string id, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase;
        Task<List<VM>> GetListGlobal<VM, DM>() where VM : DataModelBase where DM : DataModelBase;
        Task<List<VM>> GetListGlobal<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] include) where VM : DataModelBase where DM : DataModelBase;
        Task<string> UploadMongoFile(string fileName, byte[] bytes);
        Task<byte[]> DownloadMongoFile(string id);
        Task<string> DeleteMongoFile(string id);
    }
}
