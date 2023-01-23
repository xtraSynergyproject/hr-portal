using Synergy.App.Business.Interface.BusinessScript.Note.General.IIP;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Note.General.IIP
{
    public class NoteGeneralIip : INoteGeneralIip
    {
        public async Task<CommandResult<NoteTemplateViewModel>> TriggerHangfireForApiDataMigration(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _queryBusiness = sp.GetService<INtsQueryBusiness>();
            var api = await _queryBusiness.GetAllCCTNSApiMethodsDetails(viewModel.NoteId);
            RecurringJob.RemoveIfExists("MigrationJob-" + api.NoteSubject);
            RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>("MigrationJob-" + api.NoteSubject, x => x.ApiMasterDataMigrationToElasticDB(api), Cron.MinuteInterval(5));
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
    }
}
