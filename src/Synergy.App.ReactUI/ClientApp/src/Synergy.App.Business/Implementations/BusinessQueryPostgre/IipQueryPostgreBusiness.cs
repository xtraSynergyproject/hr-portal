using AutoMapper;
using MySql.Data.MySqlClient;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class IipQueryPostgreBusiness : BusinessBase<NoteViewModel, NtsNote>, IIipQueryBusiness
    {
        private readonly IUserContext _userContext;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<NtsLogViewModel> _queryNtsLog;        

        public IipQueryPostgreBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IMapper autoMapper
            , IUserContext userContext, IRepositoryQueryBase<NtsLogViewModel> queryNtsLog
            , IRepositoryQueryBase<NoteViewModel> queryRepo) : base(repo, autoMapper)
        {
            _userContext = userContext;
            _queryRepo = queryRepo;
            _queryNtsLog = queryNtsLog;           
        }
        public async Task<IList<DashboardMasterViewModel>> GetAllReportDashboardData()
        {
            var query = @$" select n.*,dm.""layoutMetadata"" as ""layoutMetadata"" 
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false  and n.""ParentNoteId"" is null
                        join cms.""N_CoreHR_DashboardMaster"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""isReportDashboard""='True' and dm.""IsDeleted""=false 
                        where t.""Name""='N_CoreHR_DashboardMaster' and t.""IsDeleted""=false  order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQueryList<DashboardMasterViewModel>(query, null);
            return querydata;
        }
        public async Task<DashboardMasterViewModel> GetReportDashboardData(string id)
        {
            var query = @$" select n.*,dm.""layoutMetadata"" as ""layoutMetadata"" 
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""Id""='{id}' and n.""IsDeleted""=false  and n.""ParentNoteId"" is null
                        join cms.""N_CoreHR_DashboardMaster"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""isReportDashboard""='True' and dm.""IsDeleted""=false 
                        where t.""Name""='N_CoreHR_DashboardMaster' and t.""IsDeleted""=false  order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQuerySingle<DashboardMasterViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<TrendingLocationViewModel>> GetAllTrendingLocationData()
        {
            var query = @$" select n.*,dm.""latitude"" as latitude,dm.""longitude"" as longitude, dm.""socialMediaType"" as socialMediaType
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id""  and n.""IsDeleted""=false  
                        join cms.""N_SWS_TrendingLocation"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Code""='TRENDING_LOCATION' and t.""IsDeleted""=false  order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQueryList<TrendingLocationViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<IdNameViewModel>> GetAllCommonChartTemplateData()
        {
            var query = @$" select n.""Id"" as Id,n.""NoteSubject"" as Name,dm.""Help"" as Code
                        from  public.""Template"" as t
                        join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false 
                        join cms.""N_CoreHR_ChartTemplate"" as dm on dm.""NtsNoteId""=n.""Id"" and dm.""IsDeleted""=false 
                        where t.""Name""='N_CoreHR_ChartTemplate' and t.""IsDeleted""=false and dm.""portal"" is null   order by n.""CreatedDate""  ";
            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<IdNameViewModel> GetRoipChannelByNo(string chanelNo)
        {
            var query = @$" select n.""Id"" as Id,dm.""chanelName"" as Name,dm.""chanelNo"" as Code
                        from public.""NtsNote"" as n 
                        join cms.""N_SWS_ROIPChanel"" as dm on dm.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false  and dm.""IsDeleted""=false and dm.""chanelNo""='{chanelNo}'
                        ";
            var querydata = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return querydata;
        }
        public async Task<List<IdNameViewModel>> GetAllRoipChannel()
        {
            var query = @$" select n.""Id"" as Id,dm.""chanelName"" as Name,dm.""chanelNo"" as Code
                        from public.""NtsNote"" as n 
                        join cms.""N_SWS_ROIPChanel"" as dm on dm.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and dm.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }
    }
}
