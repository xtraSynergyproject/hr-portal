using Synergy.App.Common;
using Synergy.App.DataModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using System.Reflection;
using System.Linq;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver.GridFS;
using Newtonsoft.Json;

namespace Synergy.App.Repository
{
    public class RepositoryPostgresBase<V, D> : IRepositoryBase<V, D> where V : DataModelBase where D : DataModelBase
    {

        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _sp;
        private readonly IMapper _autoMapper;
        private readonly string _mongoConnection = "MongoDbConnection";
        private PostgreDbContext _dbContext;
        private string _MongoDriverUrl { get { return _configuration.GetConnectionString(_mongoConnection); } }

        public MongoClient _mongoClient { get; set; }
        public GridFSBucket _gridFs { get; set; }
        public DbContextOptions<PostgreDbContext> _dbOptions { get; set; }
        public IUserContext UserContext { get; set; }

        public RepositoryPostgresBase(DbContextOptions<PostgreDbContext> dbOptions,
            IUserContext userContext, IConfiguration configuration, IMapper autoMapper
            , IServiceProvider sp)
        {
            _configuration = configuration;
            UserContext = userContext;
            _autoMapper = autoMapper;
            _dbOptions = dbOptions;
            _sp = sp;
            if (_MongoDriverUrl != null)
            {
                _mongoClient = new MongoClient(_MongoDriverUrl);
                _gridFs = new GridFSBucket(_mongoClient.GetDatabase(_configuration.GetConnectionString("MongoDbName")));
            }
        }
        public async Task<V> Create(V model, bool autoCommit = true)
        {

            return await Create<V, D>(model, autoCommit);

        }
        public async Task<V> CreateMigrate(V model, bool autoCommit = true)
        {
            return await CreateMigrate<V, D>(model, autoCommit);

        }
        public async Task<List<V>> CreateMany(List<V> models, bool autoCommit = true)
        {
            return await CreateMany<V, D>(models, autoCommit);

        }

        public async Task Delete(string id, bool autoCommit = true)
        {
            await Delete<V, D>(id, autoCommit);
        }

        public async Task<V> Edit(V model, bool autoCommit = true)
        {
            return await Edit<V, D>(model, autoCommit);
        }

        public async Task<V> GetSingle(Expression<Func<D, bool>> where, params Expression<Func<D, object>>[] include)
        {
            return await GetSingle<V, D>(where, include);
        }

        public async Task<V> GetSingleById(string id, params Expression<Func<D, object>>[] include)
        {
            return await GetSingleById<V, D>(id, include);
        }

        public async Task<List<V>> GetList()
        {
            return await GetList<V, D>();
        }

        public async Task<List<V>> GetList(Expression<Func<D, bool>> where, params Expression<Func<D, object>>[] include)
        {
            return await GetList<V, D>(where, include);
        }

        public async Task<string> UploadMongoFile(string fileName, byte[] bytes)
        {
            var result = await _gridFs.UploadFromBytesAsync(fileName, bytes);
            return result.ToString();
        }

        public async Task<byte[]> DownloadMongoFile(string id)
        {
            try
            {
                var objId = ObjectId.Parse(id);
                var x = await _gridFs.DownloadAsBytesAsync(objId);
                return x;
            }
            catch (Exception e)
            {
                return new byte[0];
            }

        }

        public async Task<string> DeleteMongoFile(string id)
        {
            try
            {
                var objId = ObjectId.Parse(id);
                await _gridFs.DeleteAsync(objId);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            return "Success";

        }

        public async Task<List<VM>> GetList<VM, DM>() where VM : DataModelBase where DM : DataModelBase
        {
            var context = GetDbContext();
            try
            {

                var data = await context.Set<DM>().AsNoTracking().Where(x => x.IsDeleted == false && x.CompanyId == UserContext.CompanyId).ToListAsync();
                return data.ToViewModelList<VM, DM>(_autoMapper);
            }
            catch (Exception e)
            {

                throw;
            }
            finally
            {
                await DisposeDbContext(context);
            }

        }

        public async Task<List<VM>> GetList<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] include) where VM : DataModelBase where DM : DataModelBase
        {

            var context = GetDbContext();
            try
            {
                where = AndAlso<DM>(x => x.IsDeleted == false && x.CompanyId == UserContext.CompanyId, where);
                if (include != null && include.Length > 0)
                {

                    var set = context.Set<DM>().Include(include[0]);
                    foreach (var item in include.Skip(1))
                    {
                        set = set.Include(item);
                    }
                    var data = await set.AsNoTracking().Where(where).ToListAsync();
                    return data.ToViewModelList<VM, DM>(_autoMapper);
                }

                var data2 = await context.Set<DM>().AsNoTracking().Where(where).ToListAsync();
                return data2.ToViewModelList<VM, DM>(_autoMapper);
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                await DisposeDbContext(context);
            }
        }

        public async Task<VM> GetSingle<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] include) where VM : DataModelBase where DM : DataModelBase
        {
            where = AndAlso<DM>(x => x.IsDeleted == false && x.CompanyId == UserContext.CompanyId, where);
            var context = GetDbContext();
            try
            {
                if (include != null && include.Length > 0)
                {
                    var set = context.Set<DM>().Include(include[0]);
                    foreach (var item in include.Skip(1))
                    {
                        set = set.Include(item);
                    }
                    var data = await set.AsNoTracking().FirstOrDefaultAsync(where);
                    return data.ToViewModel<VM, DM>(_autoMapper);
                }
                var result2 = await context.Set<DM>().AsNoTracking().FirstOrDefaultAsync(where);
                return result2.ToViewModel<VM, DM>(_autoMapper);
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                await DisposeDbContext(context);
            }


        }

        public async Task<VM> GetSingleById<VM, DM>(string id, params Expression<Func<DM, object>>[] include) where VM : DataModelBase where DM : DataModelBase
        {
            var context = GetDbContext();
            try
            {
                if (include != null && include.Length > 0)
                {
                    var set = context.Set<DM>().Include(include[0]);
                    foreach (var item in include.Skip(1))
                    {
                        set = set.Include(item);
                    }
                    var data = await set.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                    var result = data.ToViewModel<VM, DM>(_autoMapper);
                    return result;
                }
                var result2 = await context.Set<DM>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                return result2.ToViewModel<VM, DM>(_autoMapper);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await DisposeDbContext(context);
            }

        }
        private async Task<DM> GetSingleByIdWithTracking<DM>(string id) where DM : DataModelBase
        {
            var context = GetDbContext();
            try
            {

                var data = await context.Set<DM>().FirstOrDefaultAsync(x => x.Id == id);
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                await DisposeDbContext(context);
            }

        }

        public async Task<VM> Create<VM, DM>(VM model, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase
        {
            model.CreatedDate = DateTime.Now;
            model.LastUpdatedDate = DateTime.Now;
            if (model.CreatedBy.IsNullOrEmpty())
            {
                model.CreatedBy = UserContext.UserId;
            }
            if (model.LastUpdatedBy.IsNullOrEmpty())
            {
                model.LastUpdatedBy = UserContext.UserId;
            }
            if (model.CompanyId.IsNullOrEmpty())
            {
                model.CompanyId = UserContext.CompanyId;
            }
            if (model.LegalEntityId.IsNullOrEmpty())
            {
                model.LegalEntityId = UserContext.LegalEntityId;
            }
            if (model.PortalId.IsNullOrEmpty())
            {
                model.PortalId = UserContext.PortalId;
            }
            if (model.Id.IsNullOrEmpty())
            {
                model.Id = Guid.NewGuid().ToString();
            }
            DM baseModel = null;
            try
            {
                baseModel = _autoMapper.Map<VM, DM>(model);
            }
            catch (Exception ex)
            {

                throw;
            }


            var context = GetDbContext();
            try
            {


                //var result = await context.Set<DM>().AddAsync(baseModel);
                //await CreateLog(context, baseModel);
                //await context.SaveChangesAsync();
                //return model;



                Task.WaitAll(CreateMaster<DM>(context, baseModel), CreateLog<DM>(context, baseModel));
                if (autoCommit)
                {
                    await context.SaveChangesAsync();
                }

                return model;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                await DisposeDbContext(context);
            }

        }

        public static async Task CreateMaster<DM>(PostgreDbContext context, DM baseModel) where DM : DataModelBase
        {
            await context.Set<DM>().AddAsync(baseModel);
        }
        private async Task CreateLog<DM>(PostgreDbContext context, DM baseModel) where DM : DataModelBase
        {
            try
            {
                var source = baseModel.GetType();
                var destination = Type.GetType($"{source.FullName}Log, {source.Assembly.FullName}");
                if (destination != null)
                {
                    dynamic log = this.GetType().GetMethod("MapLog").MakeGenericMethod(source, destination)
                    .Invoke(this, new object[] { baseModel });
                    if (log != null)
                    {
                        log.RecordId = baseModel.Id;
                        log.Id = Guid.NewGuid().ToString();
                        log.LogVersionNo = 1;
                        log.IsLatest = true;
                        log.IsDatedLatest = true;
                        log.IsVersionLatest = true;
                        log.LogStartDate = DateTime.Today;
                        log.LogEndDate = ApplicationConstant.DateAndTime.MaxDate;
                        log.LogStartDateTime = DateTime.Now;
                        log.LogEndDateTime = ApplicationConstant.DateAndTime.MaxDate;
                        await context.AddAsync(log);
                    }

                }
            }
            catch (Exception)
            {

            }

        }

        private async Task EditMaster<DM>(PostgreDbContext context, DM existingItem) where DM : DataModelBase
        {
            context.Entry<DM>(existingItem).State = EntityState.Modified;
        }
        private async Task EditLog<DM>(PostgreDbContext context, DM baseModel) where DM : DataModelBase
        {
            try
            {
                var source = baseModel.GetType();
                var destination = Type.GetType($"{source.FullName}Log, {source.Assembly.FullName}");
                if (destination != null)
                {
                    dynamic log = this.GetType().GetMethod("MapLog").MakeGenericMethod(source, destination)
                    .Invoke(this, new object[] { baseModel });
                    if (log != null)
                    {

                        var repoQuery = _sp.GetService<IRepositoryQueryBase<NtsNote>>();
                        var exist = await repoQuery.ExecuteQueryDataRow(
                            @$"select * from log.""{source.Name}Log"" where ""RecordId""='{baseModel.Id}' and  ""IsLatest"" = true", null);

                        log.RecordId = baseModel.Id;
                        log.Id = Guid.NewGuid().ToString();
                        log.LogVersionNo = 1;
                        log.IsLatest = true;
                        log.IsVersionLatest = true;
                        log.IsDatedLatest = true;
                        log.LogStartDate = DateTime.Today;
                        log.LogEndDate = ApplicationConstant.DateAndTime.MaxDate;
                        log.LogStartDateTime = DateTime.Now;
                        log.LogEndDateTime = ApplicationConstant.DateAndTime.MaxDate;
                        if (exist != null)
                        {
                            if (log.LogStartDate == Convert.ToDateTime(exist["LogStartDate"]))
                            {
                                log.LogVersionNo = Convert.ToInt64(exist["LogVersionNo"]) + 1;
                                exist["IsDatedLatest"] = false;
                            }
                            else
                            {
                                exist["LogEndDate"] = Convert.ToDateTime(exist["LogStartDate"]).AddDays(-1);
                            }
                            if (log.VersionNo == Convert.ToInt64(exist["VersionNo"]))
                            {
                                exist["IsVersionLatest"] = false;
                            }
                            exist["IsLatest"] = false;
                            dynamic newLog = this.GetType().GetMethod("MapDictionaryLog").MakeGenericMethod(destination)
                            .Invoke(this, new object[] { exist.ToJson() });
                            context.Entry(newLog).State = EntityState.Modified;
                        }
                        await context.AddAsync(log);
                    }
                }
            }
            catch (Exception)
            {


            }

        }
        private DbSet<S> GetLogDbSet<S>(PostgreDbContext context) where S : LogModelBase
        {
            try
            {
                return context.Set<S>();
            }
            catch (Exception)
            {

                return null;
            }
        }
        public DL MapLog<SL, DL>(SL source) where SL : DataModelBase where DL : DataModelBase
        {
            try
            {
                return _autoMapper.Map<SL, DL>(source);
            }
            catch (Exception)
            {

                return null;
            }
        }
        public DL MapDictionaryLog<DL>(string json) where DL : DataModelBase
        {
            try
            {
                var result = JsonConvert.DeserializeObject<DL>(json);
                return result;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public async Task DisposeDbContext(PostgreDbContext context)
        {
            await context.Database.CloseConnectionAsync();
            await context.DisposeAsync();
        }

        public PostgreDbContext GetDbContext()
        {
            //if (_dbContext == null)
            //{
            //    _dbContext = new PostgreDbContext(_dbOptions);
            //}
            //return _dbContext;
            return new PostgreDbContext(_dbOptions);
        }

        public async Task<VM> CreateMigrate<VM, DM>(VM model, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase
        {

            model.CreatedDate = DateTime.Now;
            model.LastUpdatedDate = DateTime.Now;
            model.CreatedBy = "1";
            model.LastUpdatedBy = "1";
            model.CompanyId = "1";

            if (model.Id.IsNullOrEmpty())
            {
                model.Id = Guid.NewGuid().ToString();
            }
            var baseModel = _autoMapper.Map<VM, DM>(model);
            var context = GetDbContext();
            try
            {


                var result = await context.Set<DM>().AddAsync(baseModel);
                await context.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                await DisposeDbContext(context);
            }
        }


        public async Task<List<VM>> CreateMany<VM, DM>(List<VM> models, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase
        {
            foreach (var model in models)
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = UserContext.UserId;
                model.LastUpdatedDate = DateTime.Now;
                model.LastUpdatedBy = UserContext.UserId;
                model.CompanyId = UserContext.CompanyId;
                model.VersionNo = 1;
                if (model.CompanyId.IsNullOrEmpty())
                {
                    model.CompanyId = UserContext.CompanyId;
                }
                if (model.LegalEntityId.IsNullOrEmpty())
                {
                    model.LegalEntityId = UserContext.LegalEntityId;
                }
                if (model.PortalId.IsNullOrEmpty())
                {
                    model.PortalId = UserContext.PortalId;
                }
                if (model.Id.IsNullOrEmpty())
                {
                    model.Id = Guid.NewGuid().ToString();
                }

            }
            var baseModels = _autoMapper.Map<List<VM>, List<DM>>(models);
            var context = GetDbContext();
            try
            {


                await context.Set<DM>().AddRangeAsync(baseModels);
                await context.SaveChangesAsync();
                return models;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                await DisposeDbContext(context);
            }
        }

        public async Task<VM> Edit<VM, DM>(VM model, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase
        {
            var existingItem = await GetSingleById<DM, DM>(model.Id);
            if (existingItem != null)
            {
                model.CreatedDate = existingItem.CreatedDate;
                model.CreatedBy = existingItem.CreatedBy;
                model.CompanyId = existingItem.CompanyId;
            }
            model.LastUpdatedBy = UserContext.UserId;
            model.LastUpdatedDate = DateTime.Now;
            if (model.CompanyId.IsNullOrEmpty())
            {
                model.CompanyId = UserContext.CompanyId;
            }
            if (model.LegalEntityId.IsNullOrEmpty())
            {
                model.LegalEntityId = UserContext.LegalEntityId;
            }
            if (model.PortalId.IsNullOrEmpty())
            {
                model.PortalId = UserContext.PortalId;
            }
            var baseModel = _autoMapper.Map<VM, DM>(model, existingItem);
            var context = GetDbContext();
            try
            {

                //context.Entry<DM>(existingItem).State = EntityState.Modified;
                //await EditLog<DM>(context, existingItem);
                //await context.SaveChangesAsync();
                //return model;

                Task.WaitAll(EditMaster<DM>(context, baseModel), EditLog<DM>(context, baseModel));
                await context.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                await DisposeDbContext(context);
            }

        }

        private static Expression<Func<E, bool>> AndAlso<E>(Expression<Func<E, bool>> expr1, Expression<Func<E, bool>> expr2) where E : DataModelBase
        {
            var parameter = Expression.Parameter(typeof(E));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<E, bool>>(
                Expression.AndAlso(left, right), parameter);
        }

        public Task<V> GetSingleGlobal(Expression<Func<D, bool>> where, params Expression<Func<D, object>>[] include)
        {
            return GetSingleGlobal<V, D>(where, include);
        }

        public async Task<VM> GetSingleGlobal<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] include) where VM : DataModelBase where DM : DataModelBase
        {
            var context = GetDbContext();
            try
            {
                where = AndAlso<DM>(x => x.IsDeleted == false, where);
                if (include != null && include.Length > 0)
                {
                    var set = context.Set<DM>().Include(include[0]);
                    foreach (var item in include.Skip(1))
                    {
                        set = set.Include(item);
                    }

                    var data = await set.AsNoTracking().Where(where).FirstOrDefaultAsync();
                    return data.ToViewModel<VM, DM>(_autoMapper);
                }
                var data1 = await context.Set<DM>().FirstOrDefaultAsync(where);
                return data1.ToViewModel<VM, DM>(_autoMapper); ;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                await DisposeDbContext(context);
            }

        }

        public async Task Delete<VM, DM>(string id, bool autoCommit = true) where VM : DataModelBase where DM : DataModelBase
        {
            var model = await GetSingleById<VM, DM>(id);
            model.IsDeleted = true;
            await Edit<VM, DM>(model);
        }

        public async Task<List<V>> GetListGlobal(Expression<Func<D, bool>> where, params Expression<Func<D, object>>[] include)
        {
            return await GetListGlobal<V, D>(where, include);
        }

        public async Task<List<VM>> GetListGlobal<VM, DM>(Expression<Func<DM, bool>> where, params Expression<Func<DM, object>>[] include) where VM : DataModelBase where DM : DataModelBase
        {
            var context = GetDbContext();
            try
            {
                where = AndAlso<DM>(x => x.IsDeleted == false, where);
                if (include != null && include.Length > 0)
                {
                    var set = context.Set<DM>().Include(include[0]);
                    foreach (var item in include.Skip(1))
                    {
                        set = set.Include(item);
                    }
                    var data1 = await set.AsNoTracking().Where(where).ToListAsync();
                    return data1.ToViewModelList<VM, DM>(_autoMapper);
                }

                var data = await context.Set<DM>().Where(where).ToListAsync();
                return data.ToViewModelList<VM, DM>(_autoMapper);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                await DisposeDbContext(context);
            }

        }

        public async Task<List<V>> GetListGlobal()
        {
            return await GetListGlobal<V, D>();
        }

        public async Task<List<VM>> GetListGlobal<VM, DM>() where VM : DataModelBase where DM : DataModelBase
        {
            var context = GetDbContext();
            try
            {
                var data = await context.Set<DM>().Where(x => x.IsDeleted == false).ToListAsync();
                return data.ToViewModelList<VM, DM>(_autoMapper);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                await DisposeDbContext(context);
            }
        }

        public async Task<V> CreateGlobal(V model, bool autoCommit = true)
        {
            return await CreateGlobal<V, D>(model, autoCommit);
        }



        public async Task<VM> CreateGlobal<VM, DM>(VM model, bool autoCommit = true)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            var context = GetDbContext();

            try
            {

                model.CreatedDate = DateTime.Now;
                model.CreatedBy = UserContext.UserId;
                model.LastUpdatedDate = DateTime.Now;
                model.LastUpdatedBy = UserContext.UserId;
                model.CompanyId = UserContext.CompanyId;
                if (model.Id.IsNullOrEmpty())
                {
                    model.Id = Guid.NewGuid().ToString();
                }
                if (model.CompanyId.IsNullOrEmpty())
                {
                    model.CompanyId = UserContext.CompanyId;
                }
                var baseModel = _autoMapper.Map<VM, DM>(model);
                var result = await context.Set<DM>().AddAsync(baseModel);
                await context.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                await DisposeDbContext(context);
            }

        }
        public async Task<V> EditGlobal(V model, bool autoCommit = true)
        {
            return await Edit<V, D>(model, autoCommit);
        }
        public async Task<VM> EditGlobal<VM, DM>(VM model, bool autoCommit = true)
            where VM : DataModelBase
            where DM : DataModelBase
        {

            var existingItem = await GetSingleById<DM, DM>(model.Id);
            if (existingItem != null)
            {
                model.CreatedDate = existingItem.CreatedDate;
                model.CreatedBy = existingItem.CreatedBy;
                if (model.CompanyId.IsNullOrEmpty())
                {
                    model.CompanyId = existingItem.CompanyId;
                }
            }
            model.LastUpdatedBy = UserContext.UserId;
            model.LastUpdatedDate = DateTime.Now;

            var baseModel = _autoMapper.Map<VM, DM>(model, existingItem);
            var context = GetDbContext();
            try
            {
                var result = context.Set<DM>().Attach(baseModel);
                await context.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                await DisposeDbContext(context);
            }

        }

        public async Task<List<V>> GetActiveList()
        {
            return await GetActiveList<V, D>();
        }

        public async Task<List<VM>> GetActiveList<VM, DM>()
            where VM : DataModelBase
            where DM : DataModelBase
        {
            var context = GetDbContext();
            try
            {

                var data = await context.Set<DM>().AsNoTracking().Where(x => x.IsDeleted == false && x.Status == StatusEnum.Active && x.CompanyId == UserContext.CompanyId).ToListAsync();
                return data.ToViewModelList<VM, DM>(_autoMapper);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                await DisposeDbContext(context);
            }
        }

        public async Task CommitChanges()
        {
            var context = GetDbContext();
            await context.SaveChangesAsync();
        }

        private class ReplaceExpressionVisitor
           : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }
    }
}
