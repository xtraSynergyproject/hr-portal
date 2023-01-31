using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
////using Kendo.Mvc.UI;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Synergy.App.Business
{
    public class IipBusiness : BusinessBase<NoteViewModel, NtsNote>, IIipBusiness
    {
        private readonly IIipQueryBusiness _iipQueryBusiness;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IServiceProvider _serviceProvider;
        private readonly IFileBusiness _fileBusiness;
        private readonly IRepositoryQueryBase<NtsLogViewModel> _queryNtsLog;
        private readonly IUserContext _userContex;
        private readonly ILOVBusiness _lOVBusiness;
        public IipBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IMapper autoMapper, IRepositoryQueryBase<NoteViewModel> queryRepo,
            IServiceProvider serviceProvider, IFileBusiness fileBusiness,
        IRepositoryQueryBase<NtsLogViewModel> queryNtsLog, ILOVBusiness lOVBusiness,
            IUserContext userContex, IIipQueryBusiness iipQueryBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _serviceProvider = serviceProvider;
            _userContex = userContex;
            _fileBusiness = fileBusiness;
            _queryNtsLog = queryNtsLog;
            _lOVBusiness = lOVBusiness;
            _iipQueryBusiness = iipQueryBusiness;
        }
        public async Task<IList<DashboardMasterViewModel>> GetAllReportDashboard()
        {

            var querydata = await _iipQueryBusiness.GetAllReportDashboardData();
            return querydata;
        }
        public async Task<DashboardMasterViewModel> GetReportDashboard(string id)
        {

            var querydata = await _iipQueryBusiness.GetReportDashboardData(id);
            return querydata;
        }
        public async Task<IList<TrendingLocationViewModel>> GetAllTrendingLocation()
        {

            var querydata = await _iipQueryBusiness.GetAllTrendingLocationData();
            return querydata;
        }
        public async Task<IList<IdNameViewModel>> GetAllCommonChartTemplate()
        {

            var querydata = await _iipQueryBusiness.GetAllCommonChartTemplateData();
            return querydata;
        }
        public async Task<IdNameViewModel> GetRoipChannelByNo(string chanelNo)
        {

            var querydata = await _iipQueryBusiness.GetRoipChannelByNo(chanelNo);
            return querydata;
        }
        public async Task<IList<IdNameViewModel>> GetAllRoipChannel()
        {

            var querydata = await _iipQueryBusiness.GetAllRoipChannel();
            return querydata;
        }

    }
}
