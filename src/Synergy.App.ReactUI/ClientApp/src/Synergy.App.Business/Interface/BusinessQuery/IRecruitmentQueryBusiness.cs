using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Common;

namespace Synergy.App.Business
{
    public interface IRecruitmentQueryBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<IList<JobAdvertisementViewModel>> GetJobAdvertisement(string jobid, string rolid, StatusEnum status);
        Task<IList<JobAdvertisementViewModel>> GetSelectedJobAdvertisement(string vacancyId);
        Task<IList<JobAdvertisementViewModel>> GetJobAdvList();
    }
}
