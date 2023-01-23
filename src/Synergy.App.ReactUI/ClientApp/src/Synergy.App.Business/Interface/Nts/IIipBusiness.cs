using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IIipBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<IList<DashboardMasterViewModel>> GetAllReportDashboard();
        Task<DashboardMasterViewModel> GetReportDashboard(string id);
        Task<IList<TrendingLocationViewModel>> GetAllTrendingLocation();
        Task<IList<IdNameViewModel>> GetAllCommonChartTemplate();
        Task<IdNameViewModel> GetRoipChannelByNo(string chanelNo);
        Task<IList<IdNameViewModel>> GetAllRoipChannel();
    }
}
