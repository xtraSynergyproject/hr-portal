using Synergy.App.Business.Interface.BusinessScript.Task.Galfar.Recruitment;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Task.Galfar.Recruitment
{
    public class TaskGeneralRecPreScript : ITaskGeneralRecPreScript
    {
        /// <summary>
        /// Stop all action of task for Freezed Performance Document Master
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>

        public async Task<CommandResult<TaskTemplateViewModel>> ChangeAssigneeCandidate(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _recruitmentBusiness = sp.GetService<IRecruitmentTransactionBusiness>();
            var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            if (rowdata["ApplicationId"] != null)
            {
                var appId = rowdata["ApplicationId"].ToString();
                var usr = await _recruitmentBusiness.GetApplicationDetailsById(appId);
                if (usr!=null && usr.ApplicationUserId.IsNotNullAndNotEmpty())
                {
                    viewModel.AssignedToUserId = usr.ApplicationUserId;
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
    }
}
