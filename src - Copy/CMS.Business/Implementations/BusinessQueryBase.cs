using AutoMapper;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CMS.Business
{
    public class BusinessQueryBase<V> : IBusinessQueryBase<V> where V : class, new()
    {
        private readonly IRepositoryQueryBase<V> _repo;
        private readonly IMapper _autoMapper;
        public BusinessQueryBase(IRepositoryQueryBase<V> repo, IMapper autoMapper)
        {
            _repo = repo;
            _autoMapper = autoMapper;
        }

        public async Task ExecuteCommand(string query, Dictionary<string, object> prms)
        {
            await _repo.ExecuteCommand(query, prms);
        }

        public async Task<DataTable> ExecuteQueryDataTable(string query, Dictionary<string, object> prms)
        {
            return await _repo.ExecuteQueryDataTable(query, prms);
        }

        public async Task<List<V>> ExecuteQueryList(string query, Dictionary<string, object> prms)
        {
            return await this.ExecuteQueryList<V>(query, prms);
        }

        public async Task<List<VM>> ExecuteQueryList<VM>(string query, Dictionary<string, object> prms) where VM : class, new()
        {
            return await _repo.ExecuteQueryList<VM>(query, prms);
        }

        public async Task<V> ExecuteQuerySingle(string query, Dictionary<string, object> prms)
        {
            return await this.ExecuteQuerySingle<V>(query, prms);
        }

        public async Task<VM> ExecuteQuerySingle<VM>(string query, Dictionary<string, object> prms) where VM : class, new()
        {
            return await _repo.ExecuteQuerySingle<VM>(query, prms);
        }

        public async Task<VM> ExecuteScalar<VM>(string query, Dictionary<string, object> prms)
        {
            return await _repo.ExecuteScalar<VM>(query, prms);
        }
    }
}
