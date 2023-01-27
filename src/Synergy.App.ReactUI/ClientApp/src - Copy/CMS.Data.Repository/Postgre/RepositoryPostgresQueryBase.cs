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

namespace CMS.Data.Repository
{
    public class RepositoryPostgresQueryBase<V> : IRepositoryQueryBase<V> where V : class, new()
    {

        private readonly IConfiguration _configuration;
        private readonly IMapper _autoMapper;
        private readonly string _connection = "PostgreConnection";
        public IUserContext UserContext { get; set; }

        public RepositoryPostgresQueryBase(PostgreDbContext context, IUserContext userContext, IConfiguration configuration, IMapper autoMapper)
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

        public async Task<DataTable> ExecuteQueryDataTable(string query, Dictionary<string, object> prms)
        {
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString(_connection)))
            {
                var cmd = new NpgsqlCommand(query, connection);
                try
                {
                    await connection.OpenAsync();
                    cmd.Parameters.AddParameters(prms);
                    cmd.CommandType = CommandType.Text;
                    var da = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    da.Dispose();

                    return dt;

                }

                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    await cmd.DisposeAsync();
                    await connection.CloseAsync();
                    await connection.DisposeAsync();
                }
            }


        }

        public async Task ExecuteCommand(string query, Dictionary<string, object> prms)
        {
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString(_connection)))
            {
                var cmd = new NpgsqlCommand(query, connection);
                try
                {
                    await connection.OpenAsync();

                    cmd.Parameters.AddParameters(prms);
                    cmd.CommandType = CommandType.Text;
                    await cmd.ExecuteNonQueryAsync();

                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    await cmd.DisposeAsync();
                    await connection.CloseAsync();
                    await connection.DisposeAsync();
                }
            }


        }

        public async Task<VM> ExecuteQuerySingle<VM>(string query, Dictionary<string, object> prms)
            where VM : class, new()

        {
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString(_connection)))
            {
                var cmd = new NpgsqlCommand(query, connection);
                try
                {
                    await connection.OpenAsync();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddParameters(prms);
                    var reader = await cmd.ExecuteReaderAsync();
                    var data = await ConvertToObject<VM>(reader);
                    await reader.CloseAsync();
                    if (data == null || !data.Any())
                    {
                        return await Task.FromResult<VM>(null);
                    }
                    return await Task.FromResult<VM>(data.FirstOrDefault());
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    await cmd.DisposeAsync();
                    await connection.CloseAsync();
                    await connection.DisposeAsync();
                }
            }
        }

        public async Task<List<VM>> ExecuteQueryList<VM>(string query, Dictionary<string, object> prms)
          where VM : class, new()
        {
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString(_connection)))
            {
                var cmd = new NpgsqlCommand(query, connection);
                try
                {
                    await connection.OpenAsync();

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddParameters(prms);
                    var reader = await cmd.ExecuteReaderAsync();
                    var data = await ConvertToObject<VM>(reader);
                    // var data = _autoMapper.Map<IDataReader, List<VM>>(reader);
                    await reader.CloseAsync();
                    return data;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    await cmd.DisposeAsync();
                    await connection.CloseAsync();
                    await connection.DisposeAsync();
                }
            }
        }

        private async Task<List<T>> ConvertToObject<T>(NpgsqlDataReader rd) where T : class, new()
        {
            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var list = new List<T>();
            while (await rd.ReadAsync())
            {
                var t = new T();

                for (int i = 0; i < rd.FieldCount; i++)
                {
                    if (!rd.IsDBNull(i))
                    {
                        string fieldName = rd.GetName(i);
                        var member = members.FirstOrDefault(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase));
                        if (member != null)
                        {
                            try
                            {
                                var mt = member.Type.FullName;

                                if (mt.Contains("System.Int32"))
                                {
                                    var val = rd.GetValue(i);
                                    if (val != null)
                                    {
                                        var actualVal = int.Parse(Convert.ToString(val));
                                        accessor[t, member.Name] = actualVal;
                                    }

                                }
                                else if (mt.Contains("System.Double"))
                                {
                                    var val = rd.GetValue(i);
                                    if (val != null)
                                    {
                                        var actualVal = double.Parse(Convert.ToString(val));
                                        accessor[t, member.Name] = actualVal;
                                    }

                                }
                                else if (mt.Contains("System.DateTime"))
                                {
                                    var val = rd.GetValue(i);
                                    if (val != null)
                                    {
                                        var actualVal = Convert.ToDateTime(val);
                                        accessor[t, member.Name] = actualVal;
                                    }

                                }
                                else if (mt.EndsWith("Enum"))
                                {
                                    var val = rd.GetValue(i);
                                    if (val != null)
                                    {
                                        var actualVal = int.Parse(Convert.ToString(val));
                                        accessor[t, member.Name] = actualVal;
                                    }

                                }
                                else if (mt.Contains("System.Int16"))
                                {
                                    var val = rd.GetValue(i);
                                    if (val != null)
                                    {
                                        var actualVal = short.Parse(Convert.ToString(val));
                                        accessor[t, member.Name] = actualVal;
                                    }

                                }
                                else if (mt.Contains("System.Boolean"))
                                {
                                    var val = rd.GetValue(i);
                                    if (val != null)
                                    {
                                        var actualVal = bool.Parse(Convert.ToString(val));
                                        accessor[t, member.Name] = actualVal;
                                    }

                                }
                                else if (mt.StartsWith("System.Nullable`1[[CMS.Common"))
                                {
                                    var splits = member.Type.FullName.Split(',');
                                    if (splits.Length > 0)
                                    {
                                        var typText = $"{splits[0].Replace("System.Nullable`1[[", "")}, CMS.Common";
                                        var val = rd.GetValue(i);
                                        if (val != null)
                                        {
                                            var typ = Type.GetType(typText);
                                            var enumVal = Enum.Parse(typ, Convert.ToString(val));
                                            accessor[t, member.Name] = enumVal;
                                        }

                                    }

                                }
                                else if (mt.StartsWith("System.Nullable`1[[") && mt.Contains("System.Int64"))
                                {
                                    var val = rd.GetValue(i);
                                    if (val != null)
                                    {
                                        var actualVal = long.Parse(Convert.ToString(val));
                                        accessor[t, member.Name] = actualVal;
                                    }

                                }
                                else if (mt.StartsWith("System.Nullable`1[") && mt.Contains("System.TimeSpan"))
                                {
                                    var val = rd.GetValue(i);
                                    if (val != null)
                                    {
                                        TimeSpan timeSpan;
                                        if (TimeSpan.TryParse(Convert.ToString(val), out timeSpan))
                                        {
                                            //var actualVal = long.Parse(Convert.ToString(val));
                                            accessor[t, member.Name] = timeSpan;
                                        }
                                        else
                                        {
                                            accessor[t, member.Name] = null;
                                        }
                                        

                                    }

                                }
                                else if (mt.StartsWith("System.Nullable`1[[") && mt.Contains("System.Int32"))
                                {
                                    var val = rd.GetValue(i);
                                    if (val != null)
                                    {
                                        var actualVal = int.Parse(Convert.ToString(val));
                                        accessor[t, member.Name] = actualVal;
                                    }

                                }
                                else
                                {
                                    var temp = rd.GetValue(i);
                                    if (temp != null)
                                    {
                                        accessor[t, member.Name] = rd.GetValue(i);
                                    }

                                }


                            }
                            catch (Exception ex)
                            {

                                throw;
                            }

                        }
                    }
                }
                list.Add(t);
            }



            return list;
        }



        public async Task<VM> ExecuteScalar<VM>(string query, Dictionary<string, object> prms)
        {
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString(_connection)))
            {
                var cmd = new NpgsqlCommand(query, connection);
                try
                {
                    await connection.OpenAsync();
                    cmd.Parameters.AddParameters(prms);
                    cmd.CommandType = CommandType.Text;
                    var result = await cmd.ExecuteScalarAsync();
                    return (VM)result;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    await cmd.DisposeAsync();
                    await connection.CloseAsync();
                    await connection.DisposeAsync();
                }
            }
        }

        public async Task<dynamic> ExecuteQuerySingleDynamicObject(string query, Dictionary<string, object> prms)
        {
            var dt = await ExecuteQueryDataTable(query, prms);
            if (dt != null && dt.Rows.Count > 0)
            {
                dynamic dy = dt.Rows[0].ToDynamicObject();
                string hello1 = dy.Id;
                return dy;
            }
            return null;
        }

        public async Task<List<VM>> ExecuteScalarList<VM>(string query, Dictionary<string, object> prms)
        {
            throw new NotImplementedException();
        }

        public Task<DataRow> ExecuteQueryDataRow(string query, Dictionary<string, object> prms)
        {
            throw new NotImplementedException();
        }
    }
}
