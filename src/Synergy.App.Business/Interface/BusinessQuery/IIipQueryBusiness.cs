using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Common;
using System.Data;


namespace Synergy.App.Business
{
    public interface IIipQueryBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<IList<DashboardMasterViewModel>> GetAllReportDashboardData();
        Task<DashboardMasterViewModel> GetReportDashboardData(string id);
        Task<IList<TrendingLocationViewModel>> GetAllTrendingLocationData();
        Task<IList<IdNameViewModel>> GetAllCommonChartTemplateData();
        Task<IdNameViewModel> GetRoipChannelByNo(string chanelNo);
        Task<List<IdNameViewModel>> GetAllRoipChannel();
    }
}
