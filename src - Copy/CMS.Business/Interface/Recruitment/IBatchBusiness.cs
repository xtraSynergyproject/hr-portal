using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IBatchBusiness : IBusinessBase<BatchViewModel,Batch>
    {
        Task<List<BatchViewModel>> GetBatchData(string jobid, BatchTypeEnum type,string orgId);
        Task<List<BatchViewModel>> GetWorkerBatchData(BatchTypeEnum type);
        Task<List<BatchViewModel>> GetBatchHmData(string jobid, string orgId, string HmId, BatchTypeEnum type,string batchId);
        Task UpdateStatus(string batchId, string code);
        Task UpdateBatchStatus(string batchId, string code);
        Task<List<BatchViewModel>> GetActiveBatchList(string JobAdvertisementId);
        Task<BatchViewModel> GetBatchApplicantCount(string Id);
        Task<BatchViewModel> GetWorkerBatchApplicantCount(string Id);
        Task<List<BatchViewModel>> GetActiveBatchListByJobAdvOrg(string JobAdvertisementId, string organizationId);
        Task<string> GenerateNextBatchName(string Name);
        Task<string> GenerateNextBatchNameUsingOrg(string Name);
        //Task<string> GenerateNextBatchName(string JobName);
        Task<List<BatchViewModel>> GetActiveBatchHm(string JobAdvertisementId, string organizationId, string HmId);
    }
}
