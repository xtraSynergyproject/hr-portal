using Synergy.App.Business.Interface.BusinessScript.Note.General.SwacchSanjy;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Note.General.SwacchSanjy
{
    public class NoteGeneralSwacchSanjyPreScript : INoteGeneralSwacchSanjyPreScript
    {
        /// <summary>
        /// This method for to Create Position Hierarchy
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<NoteTemplateViewModel>> SaveUdfData(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
           
            var data = JsonConvert.DeserializeObject<dynamic>(viewModel.Json);
            data["InspectedBy"] = uc.UserId;
            if (!data["InspectedDate"].IsNotNull()) 
            {
                data["InspectedDate"] = DateTime.Now;
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }

       
    }
}
