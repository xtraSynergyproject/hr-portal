﻿using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IBusinessBase<V, D> where V : DataModelBase where D : DataModelBase
    {
        Task<List<V>> GetList();
        Task<List<V>> GetActiveList();
        Task<List<V>> GetList(Expression<Func<D, bool>> where, params Expression<Func<D, object>>[] include);
        Task<V> GetSingle(Expression<Func<D, bool>> where, params Expression<Func<D, object>>[] include);
        Task<V> GetSingleGlobal(Expression<Func<D, bool>> where);
        Task<V> GetSingleById(string id, params Expression<Func<D, object>>[] include);
        Task<CommandResult<V>> Create(V model, bool autoCommit = true);
        Task<CommandResult<V>> CreateMigrate(V model, bool autoCommit = true);
        Task<CommandResult<V>> CreateGlobal(V model, bool autoCommit = true);
        Task<List<V>> CreateMany(List<V> models, bool autoCommit = true);
        Task<CommandResult<V>> Edit(V model, bool autoCommit = true);
        Task<CommandResult<V>> EditGlobal(V model, bool autoCommit = true);
        Task Delete(string id, bool autoCommit = true);
        Task<List<V>> GetListGlobal();
        Task<List<V>> GetListGlobal(Expression<Func<D, bool>> where);


        Task<List<VM>> GetList<VM, DM>() where VM : DataModelBase where DM : DataModelBase;
        Task<List<VM>> GetActiveList<VM, DM>() where VM : DataModelBase where DM : DataModelBase;
        Task<List<VM>> GetList<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] include) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> GetSingle<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] include) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> GetSingleGlobal<VM, DM>(Expression<Func<DM, bool>> where) where VM : DataModelBase where DM : DataModelBase;
        Task<VM> GetSingleById<VM, DM>(string id, params Expression<Func<DM, object>>[] include) where VM : DataModelBase where DM : DataModelBase;
        Task<CommandResult<VM>> Create<VM, DM>(VM model, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase;
        Task<CommandResult<VM>> CreateMigrate<VM, DM>(VM model, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase;
        Task<CommandResult<VM>> CreateGlobal<VM, DM>(VM model, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase;
        Task<List<VM>> CreateMany<VM, DM>(List<VM> models, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase;
        Task<CommandResult<VM>> Edit<VM, DM>(VM model, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase;
        Task<CommandResult<VM>> EditGlobal<VM, DM>(VM model, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase;
        Task Delete<VM, DM>(string id, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase;
        Task<List<VM>> GetListGlobal<VM, DM>() where VM : DataModelBase where DM : DataModelBase;
        Task<List<VM>> GetListGlobal<VM, DM>(Expression<Func<DM, bool>> where) where VM : DataModelBase where DM : DataModelBase;
    }
}
