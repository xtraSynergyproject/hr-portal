using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class AppointmentApprovalRequestBusiness : BusinessBase<AppointmentApprovalRequestViewModel, AppointmentApprovalRequest>, IAppointmentApprovalRequestBusiness
    {
        private readonly IRepositoryQueryBase<AppointmentApprovalRequestViewModel> _queryRepo;
        public AppointmentApprovalRequestBusiness(IRepositoryBase<AppointmentApprovalRequestViewModel, AppointmentApprovalRequest> repo, IMapper autoMapper,
            IRepositoryQueryBase<AppointmentApprovalRequestViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<AppointmentApprovalRequestViewModel>> Create(AppointmentApprovalRequestViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<AppointmentApprovalRequestViewModel>(model);
            
            var result = await base.Create(data, autoCommit);

            return CommandResult<AppointmentApprovalRequestViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<AppointmentApprovalRequestViewModel>> Edit(AppointmentApprovalRequestViewModel model, bool autoCommit = true)
        {            
            var result = await base.Edit(model,autoCommit);

            return CommandResult<AppointmentApprovalRequestViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<AppointmentApprovalRequestViewModel>> IsNameExists(AppointmentApprovalRequestViewModel model)
        {                        
            return CommandResult<AppointmentApprovalRequestViewModel>.Instance();
        }
              
    }
}
