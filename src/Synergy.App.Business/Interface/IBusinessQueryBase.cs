using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IBusinessQueryBase<V> where V : class,new()
    {
        Task<V> ExecuteQuerySingle(string query, Dictionary<string, object> prms);
        Task<List<V>> ExecuteQueryList(string query, Dictionary<string, object> prms);
        Task<DataTable> ExecuteQueryDataTable(string query, Dictionary<string, object> prms);
        Task ExecuteCommand(string query, Dictionary<string, object> prms);

        Task<VM> ExecuteQuerySingle<VM>(string query, Dictionary<string, object> prms) where VM : class, new();
        Task<List<VM>> ExecuteQueryList<VM>(string query, Dictionary<string, object> prms) where VM : class,new();
        Task<VM> ExecuteScalar<VM>(string query, Dictionary<string, object> prms);

    }

}
