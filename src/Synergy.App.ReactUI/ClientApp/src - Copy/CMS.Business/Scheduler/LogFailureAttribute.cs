using CMS.Business;
using CMS.Common;
using Hangfire.Common;
using Hangfire.Logging;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace CMS.Business
{
    public class LogFailureAttribute : JobFilterAttribute, IApplyStateFilter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        void IApplyStateFilter.OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            try
            {
                var failedState = context.NewState as FailedState;
                if (failedState != null)
                {
                    Logger.ErrorException(
                        String.Format("Background job #{0} was failed with an exception.", context.BackgroundJob.Id),
                        failedState.Exception);
                }
            }catch(Exception e)
            {

            }
           
        }
        void IApplyStateFilter.OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }

    }

}
