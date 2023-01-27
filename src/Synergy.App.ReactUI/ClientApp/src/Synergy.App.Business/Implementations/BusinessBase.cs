using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class BusinessBase<V, D> : IBusinessBase<V, D> where V : DataModelBase where D : DataModelBase
    {
        internal IRepositoryBase<V, D> _repo;
        internal IMapper _autoMapper;
        public BusinessBase(IRepositoryBase<V, D> repo, IMapper autoMapper)
        {
            _repo = repo;
            _autoMapper = autoMapper;
        }
        public async virtual Task<CommandResult<V>> Create(V model, bool autoCommit = true)
        {
            return await this.Create<V, D>(model, autoCommit);
        }
        public async virtual Task<CommandResult<V>> CreateMigrate(V model, bool autoCommit = true)
        {
            return await this.CreateMigrate<V, D>(model, autoCommit);
        }
        public async virtual Task<List<V>> CreateMany(List<V> models, bool autoCommit = true)
        {
            return await this.CreateMany<V, D>(models, autoCommit);
        }
        public async virtual Task Delete(string id, bool autoCommit = true)
        {
            await this.Delete<V, D>(id, autoCommit);
        }

        public async virtual Task<CommandResult<V>> Edit(V model, bool autoCommit = true)
        {
            return await this.Edit<V, D>(model, autoCommit);
        }

        public virtual async Task<V> GetSingle(Expression<Func<D, bool>> where, params Expression<Func<D, object>>[] include)
        {
            return await this.GetSingle<V, D>(where, include);
        }

        public virtual async Task<V> GetSingleById(string id, params Expression<Func<D, object>>[] include)
        {
            return await this.GetSingleById<V, D>(id, include);
        }

        public async virtual Task<List<V>> GetList()
        {
            return await this.GetList<V, D>();
        }

        public virtual async Task<List<V>> GetList(Expression<Func<D, bool>> where, params Expression<Func<D, object>>[] include)
        {
            return await this.GetList<V, D>(where, include);
        }


        public virtual async Task<List<VM>> GetList<VM, DM>()
            where VM : DataModelBase
            where DM : DataModelBase
        {
            return await _repo.GetList<VM, DM>();
        }

        public virtual async Task<List<VM>> GetList<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] include)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            return await _repo.GetList<VM, DM>(where, include);
        }

        public virtual async Task<VM> GetSingle<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] include)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            return await _repo.GetSingle<VM, DM>(where, include);
        }

        public async Task<VM> GetSingleGlobal<VM, DM>(Expression<Func<DM, bool>> where)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            return await _repo.GetSingleGlobal<VM, DM>(where);
        }

        public virtual async Task<VM> GetSingleById<VM, DM>(string id, params Expression<Func<DM, object>>[] include)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            return await _repo.GetSingleById<VM, DM>(id, include);
        }

        public virtual async Task<CommandResult<VM>> Create<VM, DM>(VM model, bool autoCommit = true)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            var result = await _repo.Create<VM, DM>(model, autoCommit);
            return CommandResult<VM>.Instance(result);
        }

        public virtual async Task<CommandResult<VM>> CreateMigrate<VM, DM>(VM model, bool autoCommit = true)
           where VM : DataModelBase
           where DM : DataModelBase
        {
            var result = await _repo.CreateMigrate<VM, DM>(model, autoCommit);
            return CommandResult<VM>.Instance(result);
        }

        public virtual async Task<List<VM>> CreateMany<VM, DM>(List<VM> models, bool autoCommit = true)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            return await _repo.CreateMany<VM, DM>(models, autoCommit);
        }

        public virtual async Task<CommandResult<VM>> Edit<VM, DM>(VM model, bool autoCommit = true)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            var result = await _repo.Edit<VM, DM>(model, autoCommit);
            return CommandResult<VM>.Instance(result);
        }

        public virtual async Task Delete<VM, DM>(string id, bool autoCommit = true)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            await _repo.Delete<VM, DM>(id, autoCommit);
        }

        public virtual async Task<List<VM>> GetListGlobal<VM, DM>()
            where VM : DataModelBase
            where DM : DataModelBase
        {
            return await _repo.GetListGlobal<VM, DM>();
        }

        public virtual async Task<List<VM>> GetListGlobal<VM, DM>(Expression<Func<DM, bool>> where)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            return await _repo.GetListGlobal<VM, DM>(where);
        }



        public virtual async Task<V> GetSingleGlobal(Expression<Func<D, bool>> where)
        {
            return await this.GetSingleGlobal<V, D>(where);
        }

        public virtual async Task<List<V>> GetListGlobal()
        {
            return await this.GetListGlobal<V, D>();
        }

        public virtual async Task<List<V>> GetListGlobal(Expression<Func<D, bool>> where)
        {
            return await this.GetListGlobal<V, D>(where);
        }

        public virtual async Task<CommandResult<V>> CreateGlobal(V model, bool autoCommit = true)
        {
            return await this.CreateGlobal<V, D>(model, autoCommit);
        }

        public virtual async Task<CommandResult<V>> EditGlobal(V model, bool autoCommit = true)
        {
            return await this.EditGlobal<V, D>(model, autoCommit);
        }

        public virtual async Task<CommandResult<VM>> CreateGlobal<VM, DM>(VM model, bool autoCommit = true)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            var result = await _repo.CreateGlobal<VM, DM>(model, autoCommit);
            return CommandResult<VM>.Instance(result);
        }

        public virtual async Task<CommandResult<VM>> EditGlobal<VM, DM>(VM model, bool autoCommit = true)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            var result = await _repo.EditGlobal<VM, DM>(model, autoCommit);
            return CommandResult<VM>.Instance(result);
        }

        public virtual async Task<List<V>> GetActiveList()
        {
            return await this.GetActiveList<V, D>();
        }

        public virtual async Task<List<VM>> GetActiveList<VM, DM>()
            where VM : DataModelBase
            where DM : DataModelBase
        {
            return await this.GetActiveList<VM, DM>();
        }
    }
}
