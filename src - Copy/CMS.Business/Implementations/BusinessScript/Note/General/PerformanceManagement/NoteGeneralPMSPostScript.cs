using CMS.Business.Interface.BusinessScript.Note.General.PerformanceManagement;
using CMS.Common;
using CMS.UI.ViewModel;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Note.General.PerformanceManagement
{
    public class NoteGeneralPMSPostScript : INoteGeneralPMSPostScript
    {
        public async Task<CommandResult<NoteTemplateViewModel>> MapDepartmentUser(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _performanceManagementBusiness = sp.GetService<IPerformanceManagementBusiness>();
            //BackgroundJob.Enqueue<HangfireScheduler>(x => x.MapDepartmentUser(viewModel, uc.ToIdentityUser()));
            await _performanceManagementBusiness.MapDepartmentUser(viewModel);
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }

       
    }

}
