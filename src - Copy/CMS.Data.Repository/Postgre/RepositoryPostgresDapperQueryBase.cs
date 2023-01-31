using CMS.Common;
using CMS.Data.Model;
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
using System.Data;
using Npgsql;
using Newtonsoft.Json;
using FastMember;
using System.Dynamic;
using Newtonsoft.Json.Converters;
using Dapper;

namespace CMS.Data.Repository
{
    public class RepositoryPostgresDapperQueryBase<V> : IRepositoryQueryBase<V> where V : class, new()
    {

        private readonly IConfiguration _configuration;
        private readonly IMapper _autoMapper;
        private IDbConnection _dbConnection;
        private readonly string _connection = "PostgreConnection";
        public IUserContext UserContext { get; set; }

        public RepositoryPostgresDapperQueryBase(PostgreDbContext context, IUserContext userContext, IConfiguration configuration, IMapper autoMapper)
        {
            _configuration = configuration;
            UserContext = userContext;
            _autoMapper = autoMapper;
        }

        public async Task<V> ExecuteQuerySingle(string query, Dictionary<string, object> prms)
        {
            return await ExecuteQuerySingle<V>(query, prms);
        }

        public async Task<List<V>> ExecuteQueryList(string query, Dictionary<string, object> prms)
        {
            return await ExecuteQueryList<V>(query, prms);
        }
        private IDbConnection DbConnection()
        {
            SqlMapper.AddTypeHandler(typeof(TimeSpan), new TimeSpanHandler());
            var connStr = _configuration.GetConnectionString(_connection);
            if (connStr.IsNullOrEmpty())
            {
                connStr = _configuration.GetSection(_connection).Value;
            }
            var conn = new NpgsqlConnection(connStr);
            conn.Open();
            return conn;
        }
        //private IDbConnection DbConnection
        //{
        //    get
        //    {
        //        if (_dbConnection == null)
        //        {
        //            var connStr = _configuration.GetConnectionString(_connection);
        //            _dbConnection = new NpgsqlConnection(connStr);
        //        }
        //        if (_dbConnection.State != ConnectionState.Open)
        //        {
        //            _dbConnection.Open();
        //        }
        //        SqlMapper.AddTypeHandler(typeof(TimeSpan), new TimeSpanHandler());
        //        return _dbConnection;
        //    }
        //}
        public async Task<DataTable> ExecuteQueryDataTable(string query, Dictionary<string, object> prms)
        {
            using (var conn = DbConnection())
            {
                var result = await conn.QueryAsync(query);
                return result.ToList().ToDataTable();
            }
        }

        public async Task<DataRow> ExecuteQueryDataRow(string query, Dictionary<string, object> prms)
        {
            using (var conn = DbConnection())
            {
                var result = await conn.QueryAsync(query);
                var dt = result.ToList().ToDataTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0];
                }
                return null;
            }
        }

        public async Task ExecuteCommand(string query, Dictionary<string, object> prms)
        {
            using (var conn = DbConnection())
            {
                var result = await conn.ExecuteAsync(query);
                //return result.ToList().ToDataTable();
            }
            //var result = await DbConnection.ExecuteAsync(query);
            //return result.FirstOrDefault();
            //using (var connection = new NpgsqlConnection(_configuration.GetConnectionString(_connection)))
            //{
            //    var cmd = new NpgsqlCommand(query, connection);
            //    try
            //    {
            //        await connection.OpenAsync();

            //        cmd.Parameters.AddParameters(prms);
            //        cmd.CommandType = CommandType.Text;
            //        await cmd.ExecuteNonQueryAsync();

            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //    finally
            //    {
            //        await cmd.DisposeAsync();
            //        await connection.CloseAsync();
            //        await connection.DisposeAsync();
            //    }
            //}


        }

        public async Task<VM> ExecuteQuerySingle<VM>(string query, Dictionary<string, object> prms)
            where VM : class, new()
        {
            try
            {
                using (var conn = DbConnection())
                {
                    var result = await conn.QueryAsync<VM>(query);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<VM>> ExecuteQueryList<VM>(string query, Dictionary<string, object> prms)
          where VM : class, new()
        {
            try
            {
                using (var conn = DbConnection())
                {
                    var result = await conn.QueryAsync<VM>(query);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            //var result = await DbConnection.QueryAsync<VM>(query);
            //return result.ToList();
            //using (var connection = new NpgsqlConnection(_configuration.GetConnectionString(_connection)))
            //{
            //    var cmd = new NpgsqlCommand(query, connection);
            //    try
            //    {
            //        await connection.OpenAsync();

            //        cmd.CommandType = CommandType.Text;
            //        cmd.Parameters.AddParameters(prms);
            //        var reader = await cmd.ExecuteReaderAsync();
            //        var data = await ConvertToObject<VM>(reader);
            //        // var data = _autoMapper.Map<IDataReader, List<VM>>(reader);
            //        await reader.CloseAsync();
            //        return data;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //    finally
            //    {
            //        await cmd.DisposeAsync();
            //        await connection.CloseAsync();
            //        await connection.DisposeAsync();
            //    }
            //}
        }



        public async Task<VM> ExecuteScalar<VM>(string query, Dictionary<string, object> prms)
        {
            using (var conn = DbConnection())
            {
                var result = await conn.QueryAsync<VM>(query);
                return result.FirstOrDefault();
            }
            //var result = await DbConnection.QueryAsync<VM>(query);
            //return result.FirstOrDefault();

            //using (var connection = new NpgsqlConnection(_configuration.GetConnectionString(_connection)))
            //{
            //    var cmd = new NpgsqlCommand(query, connection);
            //    try
            //    {
            //        await connection.OpenAsync();
            //        cmd.Parameters.AddParameters(prms);
            //        cmd.CommandType = CommandType.Text;
            //        var result = await cmd.ExecuteScalarAsync();
            //        return (VM)result;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //    finally
            //    {
            //        await cmd.DisposeAsync();
            //        await connection.CloseAsync();
            //        await connection.DisposeAsync();
            //    }
            //}
        }

        public async Task<dynamic> ExecuteQuerySingleDynamicObject(string query, Dictionary<string, object> prms)
        {
            //using (var conn = DbConnection())
            //{
            //    dynamic result = conn.QuerySingle<dynamic>(query);
            //    return result;
            //}
            var dt = await ExecuteQueryDataTable(query, prms);
            if (dt != null && dt.Rows.Count > 0)
            {
                dynamic dy = dt.Rows[0].ToDynamicObject();
                return dy;
            }
            return null;
            //var result = await DbConnection.QueryAsync(query);
            //return result.FirstOrDefault();
            //var dt = await ExecuteQueryDataTable(query, prms);
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    dynamic dy = dt.Rows[0].ToDynamicObject();
            //    string hello1 = dy.Id;
            //    return dy;
            //}
            //return null;
        }

        public async Task<List<VM>> ExecuteScalarList<VM>(string query, Dictionary<string, object> prms)
        {
            using (var conn = DbConnection())
            {
                var result = await conn.QueryAsync<VM>(query);
                return result.ToList();
            }
            //var result = await DbConnection.QueryAsync<VM>(query);
            //return result.ToList();
        }
    }
    public class TimeSpanHandler : SqlMapper.TypeHandler<TimeSpan>
    {
        public override TimeSpan Parse(object value)
        {
            if (value.GetType().Name == "TimeSpan")
            {
                return (TimeSpan)value;
            }
            return TimeSpan.Parse((string)value);
        }

        public override void SetValue(IDbDataParameter parameter, TimeSpan value)
        {
            parameter.Value = Convert.ToString(value);
        }
    }
}
