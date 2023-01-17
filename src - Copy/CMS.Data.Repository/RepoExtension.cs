using AutoMapper;
using CMS.Data.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Data.Repository
{
    public static class RepoExtension
    {
        public static List<VM> ToViewModelList<VM, DM>(this List<DM> source, IMapper autoMapper)
        {
            try
            {
                return autoMapper.Map<List<DM>, List<VM>>(source);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public static VM ToViewModel<VM, DM>(this DM source, IMapper autoMapper)
        {
            try
            {
                return autoMapper.Map<DM, VM>(source);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public static void AddParameters(this NpgsqlParameterCollection queryParam, Dictionary<string, object> prms)
        {
            if (prms != null)
            {
                foreach (var item in prms)
                {
                    queryParam.Add(new NpgsqlParameter { ParameterName = item.Key, Value = item.Value });
                }
            }

        }

    }
}
