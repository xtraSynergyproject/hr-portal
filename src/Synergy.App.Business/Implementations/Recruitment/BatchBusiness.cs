using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class BatchBusiness : BusinessBase<BatchViewModel, Batch>, IBatchBusiness
    {
        private IUserBusiness _userBusiness;
        private IApplicationBusiness _applicationBusiness;
        private readonly IRepositoryQueryBase<BatchViewModel> _queryRepo;
        private readonly IRecTaskBusiness _taskBusiness;

        public BatchBusiness(IRepositoryBase<BatchViewModel, Batch> repo
            , IMapper autoMapper, IUserBusiness userBusiness
            , IRepositoryQueryBase<BatchViewModel> queryRepo
            , IApplicationBusiness applicationBusiness
            , IRecTaskBusiness taskBusiness) : base(repo, autoMapper)
        {
            _userBusiness = userBusiness;
            _queryRepo = queryRepo;
            _applicationBusiness = applicationBusiness;
            _taskBusiness = taskBusiness;
        }

        public async override Task<CommandResult<BatchViewModel>> Create(BatchViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<BatchViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<BatchViewModel>.Instance(model, false, validateName.Messages);
            }

            var result = await base.Create(data, autoCommit);
            if(result.IsSuccess && model.TaskId.IsNotNullAndNotEmpty())
            {
                var task = await _taskBusiness.UpdateTaskBatchId(model.TaskId, result.Item.Id);
            }
            return CommandResult<BatchViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<BatchViewModel>> Edit(BatchViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<BatchViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<BatchViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(data,autoCommit);

            return CommandResult<BatchViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        private async Task<CommandResult<BatchViewModel>> IsNameExists(BatchViewModel model)
        {
            var errorList = new Dictionary<string, string>();

            if (model.Name != null || model.Name != "")
            {
                var name = await _repo.GetSingle(x => x.Name == model.Name && x.Id != model.Id && x.JobId == model.JobId);
                if (name != null)
                {
                    errorList.Add("Name", "Batch name already exist.");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<BatchViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<BatchViewModel>.Instance();
        }

        public async Task<List<BatchViewModel>> GetBatchData(string jobid, BatchTypeEnum type,string orgId)
        {
            var batchtype = (int)((BatchTypeEnum)Enum.Parse(typeof(BatchTypeEnum), type.ToString()));
            string query = @$"SELECT c.*, o.""Name"" as Organization,l.""Code"" as BatchStatusCode, l.""Name"" as BatchStatusName,hm.""Name"" as HiringManagerName,hod.""Name"" as HeadOfDepartmentName,
                            sum(case when q.""Id"" is not null then 1 else 0 end) as NoOfApplication
                            FROM rec.""Batch"" as c                           
                            LEFT JOIN rec.""Application"" as q ON q.""BatchId"" = c.""Id""
                            LEFT JOIN cms.""Organization"" as o ON o.""Id"" = c.""OrganizationId""
                            LEFT JOIN rec.""ListOfValue"" as l ON l.""Id"" = c.""BatchStatus""
                            left join  rec.""HiringManager"" as hm on c.""HiringManager""=hm.""UserId""
                            left join  rec.""HeadOfDepartment"" as hod on c.""HeadOfDepartment""=hod.""UserId""
                            where c.""JobId""='{jobid}' and c.""BatchType""={batchtype} 
                            --and c.""OrganizationId""='{orgId}'
                            group by c.""Id"",o.""Name"",l.""Name"",l.""Code"",hm.""Name"",hod.""Name""
                            ";
            var list = await _queryRepo.ExecuteQueryList(query, null);
            return list;
        }
        public async Task<List<BatchViewModel>> GetWorkerBatchData(BatchTypeEnum type)
        {
            var batchtype = (int)((BatchTypeEnum)Enum.Parse(typeof(BatchTypeEnum), type.ToString()));
            string query = @$"SELECT c.*, o.""Name"" as Organization, l.""Name"" as BatchStatusName,
                            sum(case when q.""Id"" is not null then 1 else 0 end) as NoOfApplication
                            FROM rec.""Batch"" as c    
                             LEFT JOIN cms.""Organization"" as o ON o.""Id"" = c.""OrganizationId""
                            LEFT JOIN rec.""Application"" as q ON q.""WorkerBatchId"" = c.""Id""
                            LEFT JOIN rec.""ListOfValue"" as l ON l.""Id"" = c.""BatchStatus""
                            where  c.""BatchType""={batchtype}
                            group by c.""Id"",l.""Name"",o.""Name""
                            ";
            var list = await _queryRepo.ExecuteQueryList(query, null);
            return list;
        }
        public async Task<List<BatchViewModel>> GetBatchHmData(string jobid, string orgId, string HmId, BatchTypeEnum type,string batchId)
        {
            var batchtype = (int)((BatchTypeEnum)Enum.Parse(typeof(BatchTypeEnum), type.ToString()));
            string query = @$"SELECT c.*,j.""Name"" as JobName, o.""Name"" as Organization, l.""Name"" as BatchStatusName,l.""Code"" as BatchStatusCode,
                            sum(case when q.""Id"" is not null then 1 else 0 end) as NoOfApplication,
							(sum(case when (aps.""Code"" = 'ShortListByHm' AND appst.""Code""='NotShortlisted') then 1 else 0 end)) as NotShortlistByHM,
							(sum(case when (aps.""Code"" = 'ShortListByHm' AND appst.""Code""='ShortlistedHM') then 1 else 0 end)) as ShortlistByHM,
							(sum(case when (aps.""Code"" = 'ShortListByHm' AND appst.""Code""='Interview') then 1 else 0 end)) as ConfirmInterview
							--,(sum(case when task3.""TaskStatusCode"" = 'COMPLETED' then 1 else 0 end)) as Evaluated
                            ,hm.""Name"" as HiringManagerName,hod.""Name"" as HeadOfDepartmentName
                            FROM rec.""Batch"" as c                           
                            JOIN rec.""Application"" as q ON q.""BatchId"" = c.""Id""
                            left join  rec.""HiringManager"" as hm on c.""HiringManager""=hm.""UserId""
                            left join  rec.""HeadOfDepartment"" as hod on c.""HeadOfDepartment""=hod.""UserId""
                            JOIN rec.""ApplicationState"" as aps on aps.""Id"" = q.""ApplicationState""
                            LEFT JOIN rec.""ApplicationStatus"" as appst on appst.""Id"" = q.""ApplicationStatus""
                           -- LEFT JOIN public.""RecTask"" as service ON service.""ReferenceTypeId""=q.""Id"" AND service.""NtsType"" = 2
                           -- LEFT JOIN public.""RecTask"" as task2 ON task2.""ReferenceTypeId""=service.""Id"" AND task2.""TemplateCode""='SCHEDULE_INTERVIEW_CANDIDATE'
							--LEFT JOIN public.""RecTask"" as task3 ON task3.""ReferenceTypeId""=service.""Id"" AND task3.""TemplateCode""='INTERVIEW_EVALUATION_HM'

                            LEFT JOIN cms.""Job"" as j ON j.""Id"" = c.""JobId""
                            LEFT JOIN cms.""Organization"" as o ON o.""Id"" = c.""OrganizationId""
                            LEFT JOIN rec.""ListOfValue"" as l ON l.""Id"" = c.""BatchStatus""
                            #WHERE#
                            group by c.""Id"",j.""Name"",o.""Name"",l.""Name"",l.""Code"",hm.""Name"",hod.""Name""
                            ";
            var where = $@" where c.""HiringManager""='{HmId}' and c.""BatchType""={batchtype} ";
            if (jobid.IsNotNullAndNotEmpty())
            {
                where = $@" where c.""JobId""='{jobid}' and c.""HiringManager""='{HmId}' and c.""BatchType""={batchtype} ";
            }
            if (orgId.IsNotNullAndNotEmpty())
            {
                where = $@" where c.""JobId""='{jobid}' and c.""OrganizationId""='{orgId}'  and c.""HiringManager""='{HmId}' and c.""BatchType""={batchtype} ";
            }
            if(batchId.IsNotNullAndNotEmpty())
            {
                where = $@" where c.""Id""='{batchId}' ";
            }
            query = query.Replace("#WHERE#", where);
            var list = await _queryRepo.ExecuteQueryList(query, null);
            foreach(var item in list)
            {
                item.Evaluated = item.NoOfApplication - (item.NotShortlistByHM + item.ShortlistByHM + item.ConfirmInterview);
            }
            return list;
        }
        private async Task<List<ApplicationViewModel>> GetBatchApplicantList(string batchId)
        {
            string query = @$"SELECT q.*
                            FROM rec.""Batch"" as c
                            JOIN rec.""Application"" as q ON q.""BatchId"" = c.""Id""
                            where c.""Id"" = '{batchId}' ";
            var list = await _queryRepo.ExecuteQueryList<ApplicationViewModel>(query, null);
            return list;
        }
        public async Task<BatchViewModel> GetBatchApplicantCount(string Id)
        {
            string query = @$"SELECT c.*,count( q.""Id"" ) as NoOfApplication
                            FROM rec.""Batch"" as c
                            left JOIN rec.""Application"" as q ON q.""BatchId"" = c.""Id""
                            where c.""Id"" = '{Id}'
                            group by c.""Id""
                            ";
            var list = await _queryRepo.ExecuteQuerySingle(query, null);
            return list;
        }
        public async Task<BatchViewModel> GetWorkerBatchApplicantCount(string Id)
        {
            string query = @$"SELECT c.*,count( q.""Id"" ) as NoOfApplication
                            FROM rec.""Batch"" as c
                            left JOIN rec.""Application"" as q ON q.""WorkerBatchId"" = c.""Id""
                            where c.""Id"" = '{Id}'
                            group by c.""Id""
                            ";
            var list = await _queryRepo.ExecuteQuerySingle(query, null);
            return list;
        }

        public async Task UpdateStatus(string batchId, string code)
        {
            var status = await _repo.GetSingleGlobal<IdNameViewModel, ListOfValue>(x => x.ListOfValueType == "BatchStatus" && x.Code == "PendingwithHM");
            if (code.IsNotNullAndNotEmpty())
            {
                status = await _repo.GetSingleGlobal<IdNameViewModel, ListOfValue>(x => x.ListOfValueType == "BatchStatus" && x.Code == code);
            }
            string query = @$"update rec.""Batch"" set ""BatchStatus""='{status.Id}' where ""Id""='{batchId}'";

            var result = await _queryRepo.ExecuteScalar<bool?>(query, null);
            if (code != "Close")
            {
                var apps = await GetBatchApplicantList(batchId);
                if (apps != null && apps.Count > 0)
                {
                    foreach (var item in apps)
                    {
                        await _applicationBusiness.CreateApplicationStatusTrack(item.Id, "SL_BATCH_SEND");
                        if (code == "Close")
                        {
                            //var appstate = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Id == item.ApplicationState);
                            //var appstatus = await _repo.GetSingleGlobal<IdNameViewModel, ApplicationStatus>(x => x.Id == item.ApplicationStatus);

                            //if (appstate.Code == "ShortListByHr" && appstatus.Code == "SHORTLISTED")
                            //{
                            //    await _applicationBusiness.UpdateApplicationState(item.Id, "Rereviewed");
                            //}
                            //else if (appstate.Code == "ShortListByHm" && appstatus.Code == "SHORTLISTED")
                            //{
                            //    await _applicationBusiness.UpdateApplicationState(item.Id, "Rereviewed");
                            //}
                            ////else if (appstate.Code == "InterviewsCompleted" && appstatus.Code == "WAITLISTED")
                            ////{
                            ////    await _applicationBusiness.UpdateApplicationState(item.Id, "Rereviewed");
                            ////}
                            //else if (appstate.Code == "InterviewsCompleted" && appstatus.Code == "REJECTED")
                            //{
                            //    await _applicationBusiness.UpdateApplicationState(item.Id, "Rereviewed");
                            //}

                            //noor
                            //if (appstate.Code == "ShortListByHm" && appstatus.Code == "NotShortlisted")
                            //{
                            //    await _applicationBusiness.UpdateApplicationState(item.Id, "Rereviewed");
                            //}
                            //else if (appstate.Code == "ShortListByHm" && appstatus.Code == "ShortlistedHM")
                            //{
                            //    await _applicationBusiness.UpdateApplicationState(item.Id, "Rereviewed");
                            //}
                            //else if (appstate.Code == "ShortListByHm" && appstatus.Code == "REJECTED")
                            //{
                            //    await _applicationBusiness.UpdateApplicationState(item.Id, "Rereviewed");
                            //}
                            //var batchmodel = await _taskBusiness.GetSingle(x=>x.ReferenceTypeId==batchId && x.ReferenceTypeCode== ReferenceTypeEnum.REC_Batch);
                            //if (batchmodel!=null)
                            //{
                            //    batchmodel.DataAction = DataActionEnum.Edit;
                            //    batchmodel.TemplateAction = NtsActionEnum.Complete;
                            //    batchmodel.TaskStatusName = "Completed";
                            //    batchmodel.TaskStatusCode = "COMPLETED";
                            //    batchmodel.CompletionDate = System.DateTime.Now;
                            //    var batchresult = await _taskBusiness.Edit(batchmodel);

                            //}
                        }
                        else
                        {
                            await _applicationBusiness.UpdateApplicationState(item.Id, "ShortListByHm");
                            await _applicationBusiness.UpdateApplicationtStatus(item.Id, "NotShortlisted");
                        }
                    }
                }
            }
        }
        public async Task UpdateBatchStatus(string batchId, string code)
        {
            var status = await _repo.GetSingleGlobal<IdNameViewModel, ListOfValue>(x => x.ListOfValueType == "BatchStatus" && x.Code == code);

            string query = @$"update rec.""Batch"" set ""BatchStatus""='{status.Id}' where ""Id""='{batchId}'";

            var result = await _queryRepo.ExecuteScalar<bool?>(query, null);
        }

        public async Task<List<BatchViewModel>> GetActiveBatchList(string JobAdvertisementId)
        {
            string query = @$"Select b.*
                                FROM rec.""Batch"" as b
                                INNER JOIN rec.""ListOfValue"" as lov ON lov.""Id"" = b.""BatchStatus"" AND lov.""Code"" <> 'Draft'
                                WHERE b.""JobAdvertisementId"" = '{JobAdvertisementId}'
                            ";
            var list = await _queryRepo.ExecuteQueryList(query, null);
            return list;
        }

        public async Task<List<BatchViewModel>> GetActiveBatchListByJobAdvOrg(string JobAdvertisementId, string organizationId)
        {
            string query = @$"Select b.*
                                FROM rec.""Batch"" as b
                                INNER JOIN rec.""ListOfValue"" as lov ON lov.""Id"" = b.""BatchStatus"" AND lov.""Code"" <> 'Draft'
                                #WHERE#
                            ";
            var where = @$" WHERE b.""JobAdvertisementId"" = '{JobAdvertisementId}' ";
            if (organizationId.IsNotNullAndNotEmpty())
            {
                where = @$" WHERE b.""JobAdvertisementId"" = '{JobAdvertisementId}' AND b.""OrganizationId"" = '{organizationId}' ";
            }
            query = query.Replace("#WHERE#", where);
            var list = await _queryRepo.ExecuteQueryList(query, null);
            return list;
        }
        public async Task<List<BatchViewModel>> GetActiveBatchHm(string JobAdvertisementId, string organizationId, string HmId)
        {
            string query = @$"Select b.*
                                FROM rec.""Batch"" as b
                                INNER JOIN rec.""ListOfValue"" as lov ON lov.""Id"" = b.""BatchStatus"" AND lov.""Code"" <> 'Draft'
                                #WHERE#
                            ";
            var where = @$" WHERE b.""JobId"" = '{JobAdvertisementId}' AND b.""HiringManager""='{HmId}' ";
            if (organizationId.IsNotNullAndNotEmpty())
            {
                where = @$" WHERE b.""JobId"" = '{JobAdvertisementId}' AND b.""OrganizationId"" = '{organizationId}' AND b.""HiringManager""='{HmId}'";
            }
            query = query.Replace("#WHERE#", where);
            var list = await _queryRepo.ExecuteQueryList(query, null);
            return list;
        }
        //public async Task<string> GenerateNextBatchName(string JobName)
        public async Task<string> GenerateNextBatchName(string Name)
        {
            var date = DateTime.Now.Date;
            var id = await GenerateNextDatedBatchName();
            return string.Concat(Name, String.Format("{0:dd-MM-yyyy}", date), "-", id);
        }
        public async Task<string> GenerateNextBatchNameUsingOrg(string Name)
        {
            var id = 0;
            var date = DateTime.Now.Date;
            var batchList = await GenerateNextBatchNo(Name);
            if (batchList.Count > 0)
            {
                for (var i = 0; i < batchList.Count; i++)
                {
                    var lastNo = batchList[i].Name.Split('_')[^1];
                    lastNo = lastNo.Replace("-",string.Empty);
                    var lastNo1 = Convert.ToInt32(lastNo);
                    if (id < lastNo1)
                    {
                        id = lastNo1;
                    }
                }
                return string.Concat(Name, "-", ++id);
            }
            else 
            {
                return string.Concat(Name, "-", id);
            }            
        }
        public async Task<IList<BatchViewModel>> GenerateNextBatchNo(string Name)
        {
            string query = @$"SELECT *  FROM Rec.""Batch"" as app
                                where app.""Name"" like '{Name}%' COLLATE ""tr-TR-x-icu""
                            ";
            var result = await _queryRepo.ExecuteQueryList<BatchViewModel>(query, null);
            return result;
        }
        public async Task<long> GenerateNextDatedBatchName()
        {
            string query = @$"SELECT  count(*) as cc FROM Rec.""Batch"" as app
                                where Date(app.""CreatedDate"")=Date('{ToDD_MMM_YYYY_HHMMSS(DateTime.Now)}')
                            ";
            var result = await _queryRepo.ExecuteScalar<long>(query, null);
            return result;
        }
        public string ToDD_MMM_YYYY_HHMMSS(DateTime value)
        {
            return String.Format("{0:yyyy-MM-dd}", value);
        }
    }
}