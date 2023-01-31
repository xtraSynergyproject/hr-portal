using CMS.Common;
using CMS.Data.Model;
using MongoDB.Driver;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CMS.Data.Repository
{
    public interface IRepositoryQueryBase<V> where V : class, new()
    {
        Task<V> ExecuteQuerySingle(string query, Dictionary<string, object> prms);
        Task<List<V>> ExecuteQueryList(string query, Dictionary<string, object> prms);
        Task<DataTable> ExecuteQueryDataTable(string query, Dictionary<string, object> prms);
        Task<dynamic> ExecuteQuerySingleDynamicObject(string query, Dictionary<string, object> prms);
        Task ExecuteCommand(string query, Dictionary<string, object> prms);

        Task<VM> ExecuteQuerySingle<VM>(string query, Dictionary<string, object> prms) where VM : class, new();
        Task<List<VM>> ExecuteQueryList<VM>(string query, Dictionary<string, object> prms) where VM : class, new();


        Task<VM> ExecuteScalar<VM>(string query, Dictionary<string, object> prms);
        Task<List<VM>> ExecuteScalarList<VM>(string query, Dictionary<string, object> prms);
        Task<DataRow> ExecuteQueryDataRow(string query, Dictionary<string, object> prms);



    }
}
