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

namespace Synergy.App.Repository
{
    public class RepositoryBase<V, D> : IRepositoryBase<V, D> where V : DataModelBase where D : DataModelBase
    {
        public IMongoDatabase Mongo { get; set; }
        //IMongoCollection<T> _collection;
        private readonly IConfiguration _configuration;
        private readonly IMapper _autoMapper;
        public IUserContext UserContext { get; set; }

        public RepositoryBase(IUserContext userContext, IConfiguration configuration, IMapper autoMapper)
        {
            _configuration = configuration;
            UserContext = userContext;
            _autoMapper = autoMapper;
            var cs = this._configuration.GetConnectionString("MongoDbConnection");
            //var client = new MongoClient("mongodb://SynergyAdmin:SynergyAdm1n2020@95.111.235.64:31889");
            var client = new MongoClient(cs);
            Mongo = client.GetDatabase("CMS");
            // var name = typeof(T).BaseType.Name;
            //_collection = Mongo.GetCollection<T>(name);
        }
        public async Task<V> Create(V model)
        {
            return await Create<V, D>(model);

        }
        public async Task<List<V>> CreateMany(List<V> models)
        {
            return await CreateMany<V, D>(models);

        }

        public async Task Delete(string id)
        {
            await Delete<V, D>(id);
        }

        public async Task<V> Edit(V model)
        {
            return await Edit<V, D>(model);
        }

        public async Task<V> GetSingle(Expression<Func<V, bool>> where)
        {
            return await GetSingle<V, D>(where);
        }

        public async Task<V> GetSingleById(string id)
        {
            return await GetSingleById<V, D>(id);
        }

        public async Task<List<V>> GetList()
        {
            return await GetList<V, D>();
        }

        public async Task<List<V>> GetList(Expression<Func<V, bool>> where)
        {
            return await GetList<V, D>(where);
        }

        public async Task<List<VM>> GetList<VM, DM>() where VM : DataModelBase where DM : DataModelBase
        {
            var c = Mongo.GetCollection<VM>(typeof(DM).Name);
            var model = await c.FindAsync(x => x.IsDeleted == false && x.CompanyId == UserContext.CompanyId);
            return model.ToList();

        }

        public async Task<List<VM>> GetList<VM, DM>(Expression<Func<VM, bool>> where) where VM : DataModelBase where DM : DataModelBase
        {
            where = AndAlso<VM>(x => x.IsDeleted == false && x.CompanyId == UserContext.CompanyId, where);
            var c = Mongo.GetCollection<VM>(typeof(DM).Name);
            var model = await c.FindAsync(where);
            return model.ToList();
        }

        public async Task<VM> GetSingle<VM, DM>(Expression<Func<VM, bool>> where) where VM : DataModelBase where DM : DataModelBase
        {
            where = AndAlso<VM>(x => x.IsDeleted == false && x.CompanyId == UserContext.CompanyId, where);
            var c = Mongo.GetCollection<VM>(typeof(DM).Name);
            var model = await c.Find(where).FirstOrDefaultAsync();
            return model;
        }

        public async Task<VM> GetSingleById<VM, DM>(string id) where VM : DataModelBase where DM : DataModelBase
        {
            try
            {
                var c = Mongo.GetCollection<VM>(typeof(DM).Name);
                var model = await c.Find(x => x.Id == id && x.IsDeleted == false && x.CompanyId == UserContext.CompanyId).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {

                throw;
            }

        }



        public async Task<VM> Create<VM, DM>(VM model) where VM : DataModelBase where DM : DataModelBase
        {
            try
            {

                var baseModel = _autoMapper.Map<VM, DM>(model);
                var c = Mongo.GetCollection<DM>(typeof(DM).Name);
                baseModel.CreatedDate = DateTime.Now;
                baseModel.CreatedBy = UserContext.UserId;
                baseModel.LastUpdatedDate = DateTime.Now;
                baseModel.LastUpdatedBy = UserContext.UserId;
                baseModel.CompanyId = UserContext.CompanyId;
                await c.InsertOneAsync(baseModel);
                model.Id = baseModel.Id;
                return model;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        //private dynamic GetMongoCollection<D>()
        //{
        //    var baseType = typeof(D).BaseType;
        //    MethodInfo method = Mongo.GetType().GetMethod(nameof(Mongo.GetCollection));
        //    MethodInfo generic = method.MakeGenericMethod(baseType);
        //    return generic.Invoke(Mongo, new object[] { baseType.Name, null });
        //}

        //private dynamic GetBaseModel<D>(D model)
        //{
        //    MethodInfo method = this.GetType().GetMethod(nameof(Map));
        //    MethodInfo generic = method.MakeGenericMethod(typeof(D), typeof(D).BaseType);
        //    dynamic result = generic.Invoke(this, new object[] { model });
        //    return result;
        //}
        //public D Map<S, D>(S source)
        //{
        //    try
        //    {

        //        return _autoMapper.Map<S, D>(source);

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        public async Task<List<VM>> CreateMany<VM, DM>(List<VM> models) where VM : DataModelBase where DM : DataModelBase
        {
            var baseModels = _autoMapper.Map<List<VM>, List<DM>>(models);
            var c = Mongo.GetCollection<DM>(typeof(DM).Name);
            foreach (var baseModel in baseModels)
            {
                baseModel.CreatedDate = DateTime.Now;
                baseModel.CreatedBy = UserContext.UserId;
                baseModel.LastUpdatedDate = DateTime.Now;
                baseModel.LastUpdatedBy = UserContext.UserId;
                baseModel.CompanyId = UserContext.CompanyId;

            }
            await c.InsertManyAsync(baseModels);
            return models;
        }

        public async Task<VM> Edit<VM, DM>(VM model) where VM : DataModelBase where DM : DataModelBase
        {
            var baseModel = _autoMapper.Map<VM, DM>(model);
            var c = Mongo.GetCollection<DM>(typeof(DM).Name);
            var existingItem = await GetSingleById(model.Id);
            if (existingItem != null)
            {
                baseModel.CreatedDate = existingItem.CreatedDate;
                baseModel.CreatedBy = existingItem.CreatedBy;
                baseModel.CompanyId = existingItem.CompanyId;
            }
            baseModel.LastUpdatedBy = UserContext.UserId;
            baseModel.LastUpdatedDate = DateTime.Now;
            await c.ReplaceOneAsync(x => x.Id == model.Id, baseModel);
            return model;
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

        public Task<V> GetSingleGlobal(Expression<Func<V, bool>> where)
        {
            return GetSingleGlobal<V, D>(where);
        }

        public async Task<VM> GetSingleGlobal<VM, DM>(Expression<Func<VM, bool>> where) where VM : DataModelBase where DM : DataModelBase
        {
            var name = typeof(DM).Name;
            var c = Mongo.GetCollection<VM>(name);
            var model = await c.FindAsync(where);
            return model.FirstOrDefault();
        }

        public async Task Delete<VM, DM>(string id) where VM : DataModelBase where DM : DataModelBase
        {
            var c = Mongo.GetCollection<DM>(typeof(DM).Name);
            await c.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<List<V>> GetListGlobal(Expression<Func<V, bool>> where)
        {
            return await GetListGlobal<V, D>(where);
        }

        public async Task<List<VM>> GetListGlobal<VM, DM>(Expression<Func<VM, bool>> where) where VM : DataModelBase where DM : DataModelBase
        {
            where = AndAlso<VM>(x => x.IsDeleted == false, where);
            var c = Mongo.GetCollection<VM>(typeof(DM).Name);
            var model = await c.FindAsync(where);
            return model.ToList();
        }

        public async Task<List<V>> GetListGlobal()
        {
            return await GetListGlobal<V, D>();
        }

        public async Task<List<VM>> GetListGlobal<VM, DM>() where VM : DataModelBase where DM : DataModelBase
        {
            var c = Mongo.GetCollection<VM>(typeof(DM).Name);
            var model = await c.FindAsync(x => x.IsDeleted == false);
            return model.ToList();
        }

        public async Task<V> GetSingleIgnoreCase(string key, object value)
        {
            return await GetSingleIgnoreCase<V, D>(key, value);
        }

        public async Task<VM> GetSingleIgnoreCase<VM, DM>(string key, object value)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            var filter = Builders<VM>.Filter.Where(x => x.CompanyId == UserContext.CompanyId && x.IsDeleted == false)
                & Builders<VM>.Filter.Regex(key, new BsonRegularExpression(Convert.ToString(value), "i"));
            var model = await Mongo.GetCollection<VM>(typeof(DM).Name).Find(filter).FirstOrDefaultAsync();
            return model;
        }

        public async Task<V> GetSingleIgnoreCase(string key, object value, Expression<Func<V, bool>> where = null)
        {
            return await GetSingleIgnoreCase<V, D>(key, value, where);
        }

        public async Task<VM> GetSingleIgnoreCase<VM, DM>(string key, object value, Expression<Func<VM, bool>> where)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            var filter = Builders<VM>.Filter.Where(x => x.CompanyId == UserContext.CompanyId && x.IsDeleted == false)
                & Builders<VM>.Filter.Regex(key, new BsonRegularExpression(Convert.ToString(value), "i"));
            if (where != null)
            {
                filter = filter & Builders<VM>.Filter.Where(where);
            }
            var model = await Mongo.GetCollection<VM>(typeof(DM).Name).Find(filter).FirstOrDefaultAsync();
            return model;
        }

        public async Task<V> GetSingleGlobalIgnoreCase(string key, object value, Expression<Func<V, bool>> where = null)
        {
            return await GetSingleGlobalIgnoreCase<V, D>(key, value, where);
        }

        public async Task<VM> GetSingleGlobalIgnoreCase<VM, DM>(string key, object value, Expression<Func<VM, bool>> where = null)
            where VM : DataModelBase
            where DM : DataModelBase
        {
            var filter = Builders<VM>.Filter.Where(x => x.IsDeleted == false) &
                   Builders<VM>.Filter.Regex(key, new BsonRegularExpression(Convert.ToString(value), "i"));
            if (where != null)
            {
                filter = filter & Builders<VM>.Filter.Where(where);
            }
            var model = await Mongo.GetCollection<VM>(typeof(DM).Name).Find(filter).FirstOrDefaultAsync();
            return model;
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
