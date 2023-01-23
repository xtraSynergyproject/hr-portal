using AutoMapper;
using AutoMapper.Configuration;
using CMS.Common;
using CMS.Common.Utilities;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SpreadsheetLight;
using SpreadsheetLight.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CMS.Business
{
    public class TalentAssessmentBusiness : BusinessBase<NoteViewModel, NtsNote>, ITalentAssessmentBusiness
    {
        private readonly IRepositoryQueryBase<AssessmentViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<AssessmentDetailViewModel> _queryAssDetail;
        private readonly IRepositoryQueryBase<AssessmentQuestionsViewModel> _queryAssQues;
        private readonly IRepositoryQueryBase<AssessmentResultViewModel> _queryAssRes;
        private readonly IRepositoryQueryBase<AssessmentInterviewViewModel> _queryAssInterView;
        private readonly IRepositoryQueryBase<AssessmentAnswerViewModel> _queryAssAns;
        private IUserContext _userContext;
        private IServiceBusiness _serviceBusiness;
        private INoteBusiness _noteBusiness;
        private readonly IServiceProvider _sp;
        private readonly ILOVBusiness _lOVBusiness;
        private readonly IRepositoryQueryBase<AssessmentQuestionsOptionViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<AssignmentViewModel> _queryAssignment;
        private readonly IRepositoryQueryBase<TeamWorkloadViewModel> _queryTWRepo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _idNameRepo;

        private readonly IRepositoryQueryBase<InterViewQuestionViewModel> _QueryInterviewquestion;
        private IUserBusiness _userBusiness;
        private readonly IFileBusiness _fileBusiness;
        public TalentAssessmentBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper
            , IRepositoryQueryBase<AssessmentQuestionsOptionViewModel> queryRepo1, IServiceProvider sp,
            IUserContext userContext, IServiceBusiness serviceBusiness, INoteBusiness noteBusiness,
            IRepositoryQueryBase<AssessmentViewModel> queryRepo, IRepositoryQueryBase<AssignmentViewModel> queryAssignment
           , IRepositoryQueryBase<AssessmentQuestionsViewModel> queryAssQues,
                IRepositoryQueryBase<TeamWorkloadViewModel> queryTWRepo, IRepositoryQueryBase<AssessmentDetailViewModel> queryAssDetail
            , IRepositoryQueryBase<IdNameViewModel> idNameRepo, IRepositoryQueryBase<AssessmentResultViewModel> queryAssRes,
                ILOVBusiness lOVBusiness, IRepositoryQueryBase<AssessmentInterviewViewModel> queryAssInterView,
                IRepositoryQueryBase<AssessmentAnswerViewModel> queryAssAns,
                IRepositoryQueryBase<InterViewQuestionViewModel> QueryInterviewquestion
            , IUserBusiness userBusiness
            , IFileBusiness fileBusiness) : base(repo, autoMapper)
        {
            _queryRepo1 = queryRepo1;
            _sp = sp;
            _userContext = userContext;
            _serviceBusiness = serviceBusiness;
            _noteBusiness = noteBusiness;
            _queryRepo = queryRepo;
            _queryAssignment = queryAssignment;
            _queryAssQues = queryAssQues;
            _queryTWRepo = queryTWRepo;
            _queryAssDetail = queryAssDetail;
            _idNameRepo = idNameRepo;
            _queryAssRes = queryAssRes;
            _lOVBusiness = lOVBusiness;
            _queryAssInterView = queryAssInterView;
            _queryAssAns = queryAssAns;
            _QueryInterviewquestion = QueryInterviewquestion;
            _userBusiness = userBusiness;
            _fileBusiness = fileBusiness;
        }
        public async Task<List<AssessmentQuestionsOptionViewModel>> GetOptionsForQuestion(string questionNoteId)
        {
            var query = $@"select q.*,q.""Id"" as OptionId,q.""NtsNoteId"" as NoteId 
from cms.""N_TAS_QuestionOption"" as q
join public.""NtsNote"" as n on n.""Id""=q.""NtsNoteId"" and q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}'
where n.""ParentNoteId""='{questionNoteId}'  and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}' ";
            return await _queryRepo1.ExecuteQueryList(query, null);
        }

        public async Task<List<AssessmentQuestionsOptionViewModel>> GetOptionsByQuestionId(string questionId)
        {
            var query = $@"select q.*,q.""Id"" as OptionId,q.""NtsNoteId"" as NoteId 
from cms.""N_TAS_QuestionOption"" as q
join public.""NtsNote"" as n on q.""NtsNoteId""=n.""Id"" and n.""IsDeleted"" = false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_TAS_Question"" as que on n.""ParentNoteId""=que.""NtsNoteId""
where que.""Id""='{questionId}' and que.""IsDeleted""=false and que.""CompanyId""='{_repo.UserContext.CompanyId}'  ";
            return await _queryRepo1.ExecuteQueryList(query, null);
        }


        public async Task<List<AssessmentQuestionsViewModel>> GetQuestion()
        {
            var query = $@"select q.*,q.""Id"" as QuestionId,q.""NtsNoteId"" as NoteId, i.""IndicatorName"" from cms.""N_TAS_Question"" as q
                            join cms.""N_TAS_Indicator"" as i on q.""IndicatorId""=i.""Id"" and i.""IsDeleted""=false and i.""CompanyId""='{_repo.UserContext.CompanyId}'
             where q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}'";
            return await _queryRepo1.ExecuteQueryList<AssessmentQuestionsViewModel>(query, null);
        }

        public async Task<List<AssessmentQuestionsViewModel>> GetTreeListQuestion()
        {
            var query = $@"select nt.""Id"",nt.""ParentNoteId"" as ParentId,nt.""NoteSubject"",nt.""Id"" as NoteId,
null as Question,null as QuestionArabic,null as QuestionDescription,null as QuestionDescriptionArabic,'Topic' as Type,
null as CompetencyLevel,null as IndicatorName,null as AssessmentType,true as lazy,nt.""NoteSubject"" as title,nt.""Id"" as key
from public.""NtsNote"" nt
 where nt.""TemplateCode""='TAS_TOPIC' and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
 
 --union all
--select nt.""Id"",nt.""ParentNoteId"" as ParentId,nt.""NoteSubject"",nt.""Id"" as NoteId,
--null as Question,null as QuestionArabic,null as QuestionDescription,null as QuestionDescriptionArabic,'Level' as Type
--from public.""NtsNote"" nt
-- where nt.""TemplateCode""='TAS_COMPETENCY_LEVEL' and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
 
  union all
select 
q.""Id"",nq.""ParentNoteId"" as ParentId,null,q.""NtsNoteId"" as NoteId,
q.""Question"",q.""QuestionArabic"",q.""QuestionDescription"",q.""QuestionDescriptionArabic"",'Question' as Type,
l.""CompetencyLevel"" as CompetencyLevel,i.""IndicatorName"" as IndicatorName, at.""Name"" as AssessmentType,false as lazy,'' as title,q.""Id"" as key
from cms.""N_TAS_Question"" as q
join public.""NtsNote"" nq on nq.""Id""=q.""NtsNoteId"" and nq.""IsDeleted""=false and nq.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_CompetencyLevel"" l on l.""Id""=q.""CompentencyLevelId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_Indicator"" i on i.""Id""=q.""IndicatorId"" and i.""IsDeleted""=false and i.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as at on q.""AssessmentTypeId""=at.""Id"" and at.""IsDeleted""=false and at.""CompanyId""='{_repo.UserContext.CompanyId}'
where q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}'";
            return await _queryRepo1.ExecuteQueryList<AssessmentQuestionsViewModel>(query, null);
        }

        public async Task<IList<AssessmentQuestionsViewModel>> GetAssessmentQuestions(string topic, string level)
        {
            var query = $@"select q.""Id"",q.""NtsNoteId"" as NoteId,q.""Question"",q.""QuestionArabic"",n.""ParentNoteId"",
t.""NoteSubject"" as Topic,i.""IndicatorName"",c.""CompetencyLevel"",at.""Name"" as AssessmentType
from cms.""N_TAS_Question"" as q
left join public.""NtsNote"" as n on q.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false
left join public.""NtsNote"" as t on n.""ParentNoteId""=t.""Id"" and t.""IsDeleted""=false
left join cms.""N_TAS_CompetencyLevel"" as c on q.""CompentencyLevelId""=c.""Id"" and c.""IsDeleted""=false
left join cms.""N_TAS_Indicator"" as i on q.""IndicatorId""=i.""Id"" and i.""IsDeleted""=false
left join public.""LOV"" as at on q.""AssessmentTypeId""=at.""Id"" and at.""IsDeleted""=false
where q.""IsDeleted""=false #WHERE# ";

            var where = "";
            if (level.IsNotNull())
            {
                where = $@"  and ""n"".""ParentNoteId"" = '{topic}' and ""q"".""CompentencyLevelId"" = '{level}'";
            }
            else if (level == null && topic.IsNotNull())
            {
                where = $@" and ""n"".""ParentNoteId"" = '{topic}'";
            }
            query = query.Replace("#WHERE#", where);

            var result = await _queryRepo1.ExecuteQueryList<AssessmentQuestionsViewModel>(query, null);

            return result;
        }

        public async Task<AssessmentQuestionsOptionViewModel> GetOptionsById(string Id)
        {
            var query = $@"select q.*,q.""Id"" as OptionId,q.""NtsNoteId"" as NoteId from cms.""N_TAS_QuestionOption"" as q

where q.""Id""='{Id}' and q.""CompanyId""='{_repo.UserContext.CompanyId}'";
            return await _queryRepo1.ExecuteQuerySingle(query, null);
        }
        public async Task DeleteOption(string id)
        {
            var _tableMetadataBusiness = _sp.GetService<ITableMetadataBusiness>();
            var _noteBusiness = _sp.GetService<INoteBusiness>();
            var sal = await _tableMetadataBusiness.GetTableDataByColumn("TAS_QUESTION_OPTION", "", "Id", id);
            if (sal != null)
            {
                var query = $@"update  cms.""N_TAS_QuestionOption"" set ""IsDeleted""=true 
where ""Id""='{id}'";
                await _queryRepo1.ExecuteCommand(query, null);
                await _noteBusiness.Delete(sal["NtsNoteId"].ToString());
            }
        }

        public async Task<List<AssessmentViewModel>> GetAssessmentsList(string type, string searchtext)
        {

            var query = $@"select a.*,a.""NtsNoteId"" as NoteId, at.""Name"" as AssessmentType, s.""ServiceNo"",s.""TemplateCode"",s.""Id"" as ServiceId, ss.""Code"" as ServiceStatusCode, u.""Name"" as CreatedBy
							from cms.""N_TAS_Assessment"" as a
							join public.""NtsNote"" as n on a.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
							join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
							join public.""LOV"" as at on a.""AssessmentTypeId""=at.""Id"" and at.""IsDeleted""=false and at.""CompanyId""='{_repo.UserContext.CompanyId}'
							join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_repo.UserContext.CompanyId}'
							join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
							where a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'  #SearchWhere# order by a.""CreatedDate"" desc ";

            var questionWhere = "";
            if (type.IsNotNullAndNotEmpty() && searchtext.IsNotNullAndNotEmpty() && type == "Question")
            {
                questionWhere = $@" and a.""Id"" in (select aq.""AssessmentId"" from cms.""N_TAS_AssessmentQuestion"" as aq
									join cms.""N_TAS_Question"" as q on aq.""QuestionId"" = q.""Id"" and q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}'
									where lower(q.""Question"") like lower('%{searchtext}%') COLLATE ""tr-TR-x-icu"" and aq.""IsDeleted""=false and aq.""CompanyId""='{_repo.UserContext.CompanyId}') ";
            }
            else if (type.IsNotNullAndNotEmpty() && searchtext.IsNotNullAndNotEmpty() && type == "Option")
            {
                questionWhere = $@" and a.""Id"" in (select aq.""AssessmentId"" from cms.""N_TAS_AssessmentQuestion"" as aq
									join cms.""N_TAS_Question"" as q on aq.""QuestionId"" = q.""Id"" and q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}'
									join public.""NtsNote"" as n on q.""NtsNoteId""=n.""ParentNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'						
									join cms.""N_TAS_QuestionOption"" as qo on n.""Id""=qo.""NtsNoteId"" and qo.""IsDeleted""=false and qo.""CompanyId""='{_repo.UserContext.CompanyId}'
									where lower(qo.""Option"") like lower('%{searchtext}%') COLLATE ""tr-TR-x-icu"" and aq.""IsDeleted""=false  and aq.""CompanyId""='{_repo.UserContext.CompanyId}') ";
            }
            query = query.Replace("#SearchWhere#", questionWhere);

            var result = await _queryRepo.ExecuteQueryList<AssessmentViewModel>(query, null);
            return result;

        }
        public async Task<bool> DeleteAssessment(string NoteId)
        {
            var note = await GetSingleById(NoteId);
            if (note != null)
            {
                var query = $@"Update cms.""N_TAS_Assessment"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
                await _queryRepo.ExecuteCommand(query, null);

                await Delete(NoteId);
                return true;
            }
            return false;
        }

        public async Task<AssessmentViewModel> GetAssessmentDetailsById(string assmntId)
        {
            var query = $@"Select * from cms.""N_TAS_Assessment"" where ""Id""='{assmntId}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var result = await _queryRepo.ExecuteQuerySingle(query, null);
            return result;
        }

        public async Task CopyAssessment(AssessmentViewModel model)
        {
            ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
            serviceModel.ActiveUserId = _userContext.UserId;
            serviceModel.TemplateCode = "TAS_ASSESSMENT";
            var service = await _serviceBusiness.GetServiceDetails(serviceModel);

            //var template = await _templateBusiness.GetSingle(x => x.Id == viewModel.TemplateId);

            service.ServiceSubject = model.AssessmentName;
            service.OwnerUserId = _userContext.UserId;
            service.StartDate = DateTime.Now;
            service.DueDate = DateTime.Now.AddDays(10);
            service.ActiveUserId = _userContext.UserId;
            service.DataAction = DataActionEnum.Create;

            service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

            service.Json = JsonConvert.SerializeObject(model);

            var res = await _serviceBusiness.ManageService(service);

            if (res.IsSuccess)
            {
                var questions = await GetQuestionsByAssessmentId(model.AssessmentId);

                foreach (var qId in questions)
                {
                    var noteTemplate = new NoteTemplateViewModel();
                    noteTemplate.ActiveUserId = _userContext.UserId;
                    noteTemplate.TemplateCode = "TAS_ASSESSMENT_QUESTION";
                    noteTemplate.DataAction = DataActionEnum.Create;
                    var assQues = await _noteBusiness.GetNoteDetails(noteTemplate);
                    assQues.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

                    var assessmentQuesModel = new AssessmentViewModel();
                    assessmentQuesModel.AssessmentId = res.Item.UdfNoteTableId;
                    assessmentQuesModel.QuestionId = qId.QuestionId;

                    assQues.Json = JsonConvert.SerializeObject(assessmentQuesModel);

                    var assQuesResult = await _noteBusiness.ManageNote(assQues);
                }

            }

        }

        public async Task<List<AssessmentViewModel>> GetQuestionsByAssessmentId(string assessmentId)
        {
            var query = $@"Select aq.""QuestionId"", q.""Question"" from cms.""N_TAS_AssessmentQuestion"" as aq 
                        join cms.""N_TAS_Question"" as q on aq.""QuestionId""=q.""Id"" and q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}'
where aq.""AssessmentId""='{assessmentId}' and aq.""IsDeleted""=false and aq.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var result = await _queryRepo.ExecuteQueryList(query, null);
            return result;
        }

        public async Task<List<CapacityRiskViewModel>> GetJobCapacityData(string departmentId)
        {
            var query = $@"select distinct j.""JobTitle"" as Name,j.""Id"" as Id
FROM cms.""N_CoreHR_HRDepartment"" as d
 join cms.""N_CoreHR_HRPosition"" as p on d.""Id"" = p.""DepartmentId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
  join cms.""N_CoreHR_HRJob"" as j on j.""Id"" = p.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
  where d.""Id""='{departmentId}' and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var dept = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            var deptids = "'" + string.Join("','", dept.Select(x => x.Id)) + "'";

            var query1 = $@"select j.""JobTitle"" as JobName,j.""Id"" as JobId,cr.""ExternalAvailability""::int,cr.""InternalAvailability""::int
from cms.""N_CoreHR_HRJob"" as j 
left join cms.""N_TAS_JobCapacityRisk"" as cr on cr.""JobId""=j.""Id"" and cr.""IsDeleted""=false and cr.""CompanyId""='{_repo.UserContext.CompanyId}'
where j.""Id"" in ({deptids}) and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'";
            return await _queryRepo1.ExecuteQueryList<CapacityRiskViewModel>(query1, null);
        }

        public async Task<List<CapacityRiskViewModel>> GetJobCapacityChartData(string jobId)
        {

            var query1 = $@"select j.""JobTitle"" as JobName,j.""Id"" as JobId,100 as Size,cr.""ExternalAvailability""::int,cr.""InternalAvailability""::int
from cms.""N_TAS_JobCapacityRisk"" as cr
join cms.""N_CoreHR_HRJob"" as j on cr.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
where cr.""IsDeleted""=false and cr.""CompanyId""='{_repo.UserContext.CompanyId}' #where#";
            var where = "";
            if (jobId.IsNotNullAndNotEmpty())
            {
                where = $@"and cr.""JobId""='{jobId}'";
            }
            query1 = query1.Replace("#where#", where);
            return await _queryRepo1.ExecuteQueryList<CapacityRiskViewModel>(query1, null);
        }
        public async Task<AssignmentViewModel> GetAssignmentDetails(string userId)
        {
            string query = $@"select CONCAT( hp.""FirstName"",' ',hp.""LastName"") as PersonFullName,hd.""DepartmentName"",hj.""JobTitle"" as JobName,hpos.""PositionName"",hl.""LocationName"",hg.""GradeName"",
substring(assi.""DateOfJoin"",0,11) as DateOfJoin,lovs.""Name"" as AssignmentStatusName,case when hp.""Status""=1 then 'Active' else 'InActive' end as PersonStatus,
case when u.""Status""=1 then 'Active' else 'InActive' end as UserStatus,u.""Id"" as UserId,nt.""Id"" as NoteId,assi.""Id"" as AssignmentId,
cont.""Id"" as ContractId,ntc.""Id"" as NoteContractId,nta.""Id"" as NoteAssignmentId,hpos.""Id"" as PositionId,
ph.""Id"" as PositionHierarchyId,hm.""Id"" as HierarchyId,
ntp.""Id"" as NotePositionHierarchyId,
si.""Id"" as SalaryInfoId,ntsi.""Id"" as NoteSalaryInfoId,u.""PhotoId"" as PhotoId,
hd.""Id"" as DepartmentId,hj.""Id"" as JobId,hp.""Id"" as PersonId,u.""Email"" as Email,hp.""PersonNo"" as PersonNo,
b.""Name"" as ""BadgeName"",b.""BadgeDescription"" as ""BadgeDescription"",b.""ImageId"" as ""BadgeImage"",
ass.""AssessmentName"" as ""AssessmentName"",ra.""Score"" as ""AssessmentScore"",sa.""AssessmentStartDate"" as ""AssessmentStartTime""
from cms.""N_CoreHR_HRPerson"" as hp
left join public.""NtsNote"" as nt on nt.""Id""=hp.""NtsNoteId"" and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id""=assi.""EmployeeId"" and assi.""IsDeleted""=false and assi.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsNote"" as nta on nta.""Id""=assi.""NtsNoteId"" and nta.""IsDeleted""=false and nta.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""User"" as u on u.""Id""=hp.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_UserBadges"" as ub on ub.""UserId""=hp.""UserId"" and ub.""IsDeleted""=false and ub.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_Badges"" as b on b.""Id"" = ub.""BadgeId"" and b.""IsDeleted"" = false and b.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_UserAssessmentSchedule"" as sa on sa.""UserId""=hp.""UserId"" and sa.""IsDeleted""=false and sa.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_UserAssessmentResult"" as ra on ra.""UserId"" = hp.""UserId"" and ra.""IsDeleted"" = false and ra.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_Assessment"" as ass on ass.""Id"" = sa.""AssessmentId"" and ass.""IsDeleted"" = false and ass.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""HierarchyMaster"" as hm on hm.""Code""='POS_HIERARCHY' and hm.""IsDeleted""=false and hm.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id""=assi.""DepartmentId"" and hd.""IsDeleted""=false and hd.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false and hj.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRPosition"" as hpos on hpos.""Id""=assi.""PositionId"" and hpos.""IsDeleted""=false and hpos.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRLocation"" as hl on hl.""Id""=assi.""LocationId"" and hl.""IsDeleted""=false and hl.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRGrade"" as hg on hg.""Id""=assi.""AssignmentGradeId"" and hg.""IsDeleted""=false and hg.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as lovs on lovs.""Id""=assi.""AssignmentStatusId"" and lovs.""IsDeleted""=false and lovs.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRContract"" as cont on hp.""Id""=cont.""EmployeeId"" and cont.""IsDeleted""=false and cont.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsNote"" as ntc on ntc.""Id""=cont.""NtsNoteId"" and ntc.""IsDeleted""=false and ntc.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_PositionHierarchy"" as ph on ph.""PositionId""=assi.""PositionId"" and ph.""IsDeleted""=false and ph.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsNote"" as ntp on ntp.""Id""=ph.""NtsNoteId"" and ntp.""IsDeleted""=false and ntp.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_PayrollHR_SalaryInfo"" as si on hp.""Id""=si.""PersonId"" and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsNote"" as ntsi on ntsi.""Id""=si.""NtsNoteId"" and ntsi.""IsDeleted""=false and ntsi.""CompanyId""='{_repo.UserContext.CompanyId}'
where hp.""IsDeleted""=false and hp.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""Id"" = '{userId}' ";


            var queryData = await _queryAssignment.ExecuteQuerySingle(query, null);
            return queryData;
        }
        public async Task<List<AssignmentViewModel>> ReadBadgeData(string userId)
        {
            var query = $@"select b.""Name"" as ""BadgeName"",b.""BadgeDescription"" as ""BadgeDescription"",b.""ImageId"" as ""BadgeImage"",
				hp.""Id"",u.""Id"",ub.""AwardDate"" as ""BadgeAwardDate""
				from cms.""N_CoreHR_HRPerson"" as hp
				left join public.""User"" as u on u.""Id""=hp.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                 left join cms.""N_TAS_UserBadges"" as ub on ub.""UserId""=hp.""UserId"" and ub.""IsDeleted""=false and ub.""CompanyId""='{_repo.UserContext.CompanyId}'
                 left join cms.""N_TAS_Badges"" as b on b.""Id"" = ub.""BadgeId"" and b.""IsDeleted"" = false and b.""CompanyId""='{_repo.UserContext.CompanyId}'
				where hp.""IsDeleted""=false and hp.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""Id"" = '{userId}'";
            return await _queryAssignment.ExecuteQueryList<AssignmentViewModel>(query, null);
        }
        public async Task<List<AssignmentViewModel>> ReadAssessmentData(string userId)
        {
            var query = $@"select distinct  ass.""AssessmentName"" as ""AssessmentName""
,ra.""Score"" as ""AssessmentScore"",sa.""AssessmentStartDate"" as ""AssessmentStartTime""
,ass.""Id"" as AssessmentId,sa.""Id"",u.""Id""

                from cms.""N_CoreHR_HRPerson"" as hp
                join public.""User"" as u on u.""Id""=hp.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                
                 join cms.""N_TAS_UserAssessmentResult"" as ra on ra.""UserId"" = hp.""UserId"" and ra.""IsDeleted"" = false and ra.""CompanyId""='{_repo.UserContext.CompanyId}'

                 join cms.""N_TAS_UserAssessmentSchedule"" as sa on sa.""UserId""=hp.""UserId"" and sa.""IsDeleted""=false and sa.""CompanyId""='{_repo.UserContext.CompanyId}'
                  join cms.""N_TAS_Assessment"" as ass on ass.""Id"" = ra.""AssessmentId"" and ass.""IsDeleted"" = false and ass.""CompanyId""='{_repo.UserContext.CompanyId}'

                  and ass.""Id"" = sa.""AssessmentId""

                where hp.""IsDeleted""=false and hp.""CompanyId""='{_repo.UserContext.CompanyId}' and  u.""Id"" = '{userId}'";
            return await _queryAssignment.ExecuteQueryList<AssignmentViewModel>(query, null);
        }


        public async Task<List<CapacityRiskViewModel>> GetJobCapacityChartData(string jobId, string departmentId)
        {

            var query = "";
            var jobids = "";
            if (departmentId.IsNotNullAndNotEmpty())
            {
                query = $@"select distinct j.""JobTitle"" as Name,j.""Id"" as Id
FROM cms.""N_CoreHR_HRDepartment"" as d
 join cms.""N_CoreHR_HRPosition"" as p on d.""Id"" = p.""DepartmentId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
  join cms.""N_CoreHR_HRJob"" as j on j.""Id"" = p.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
  where d.""Id""='{departmentId}' and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'";
                var dept = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
                jobids = "'" + string.Join("','", dept.Select(x => x.Id)) + "'";

            }


            var query1 = $@"select j.""JobTitle"" as JobName,j.""Id"" as JobId,(cr.""ExternalAvailability""::int + cr.""InternalAvailability""::int) as Size,
cr.""ExternalAvailability""::int,cr.""InternalAvailability""::int
from cms.""N_TAS_JobCapacityRisk"" as cr
join cms.""N_CoreHR_HRJob"" as j on cr.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
where cr.""IsDeleted""=false and cr.""CompanyId""='{_repo.UserContext.CompanyId}' #where#";
            var where = "";
            if (jobId.IsNotNullAndNotEmpty())
            {
                where = $@"and cr.""JobId""='{jobId}'";
            }
            else if (jobids.IsNotNullAndNotEmpty())
            {
                where = $@"and cr.""JobId"" in ({jobids})";
            }
            query1 = query1.Replace("#where#", where);
            return await _queryRepo1.ExecuteQueryList<CapacityRiskViewModel>(query1, null);
        }

        public async Task<List<AssessmentQuestionsViewModel>> GetMappedQuestionsList(string assessmentId)
        {
            var query = $@"Select q.""Question"",q.""QuestionDescription"",q.""QuestionArabic"",q.""QuestionDescriptionArabic"", aq.""Id"",aq.""AssessmentId"",
                            aq.""SequenceOrder"", aq.""NtsNoteId"" as NoteId,nt.""NoteSubject"" as Topic, c.""CompetencyLevel"" as CompetencyLevel, i.""IndicatorName""
							From cms.""N_TAS_AssessmentQuestion"" as aq
							Join cms.""N_TAS_Question"" as q on aq.""QuestionId""=q.""Id"" and q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join public.""NtsNote"" as n on q.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
							left Join public.""NtsNote"" as nt on n.""ParentNoteId""=nt.""Id"" and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'							
							left Join cms.""N_TAS_CompetencyLevel"" as c on q.""CompentencyLevelId""=c.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
							left Join public.""NtsNote"" as nc on c.""NtsNoteId""=nc.""Id"" and nc.""IsDeleted""=false and nc.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left join cms.""N_TAS_Indicator"" as i on q.""IndicatorId""=i.""Id"" and i.""IsDeleted""=false and i.""CompanyId""='{_repo.UserContext.CompanyId}'
							where aq.""AssessmentId""='{assessmentId}' and aq.""IsDeleted""=false and aq.""CompanyId""='{_repo.UserContext.CompanyId}' order by aq.""CreatedDate"" desc";

            var result = await _queryAssQues.ExecuteQueryList(query, null);
            return result;

        }

        public async Task<bool> UpdateSequenceNo(string Id, long? sequenceNo)
        {
            var query = $@"Update cms.""N_TAS_AssessmentQuestion"" set ""SequenceOrder""='{sequenceNo}' where ""Id""='{Id}' ";
            await _queryAssQues.ExecuteCommand(query, null);
            return true;
        }

        public async Task<List<AssessmentQuestionsViewModel>> GetAllQuestions(string assessmentTypeId)
        {
            var query = $@"select q.*,q.""Id"" as QuestionId,q.""NtsNoteId"" as NoteId, nt.""NoteSubject"" as Topic, c.""CompetencyLevel"" as CompetencyLevel, i.""IndicatorName"",at.""Name"" as AssessmentType
							from cms.""N_TAS_Question"" as q
                            join public.""NtsNote"" as n on q.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left Join public.""NtsNote"" as nt on n.""ParentNoteId""=nt.""Id"" and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'							
							left Join cms.""N_TAS_CompetencyLevel"" as c on q.""CompentencyLevelId""=c.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
							left Join public.""NtsNote"" as nc on c.""NtsNoteId""=nc.""Id"" and nc.""IsDeleted""=false and nc.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left join cms.""N_TAS_Indicator"" as i on q.""IndicatorId""=i.""Id"" and i.""IsDeleted""=false and i.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left join public.""LOV"" as at on q.""AssessmentTypeId""=at.""Id"" and at.""IsDeleted""=false and at.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where q.""AssessmentTypeId""='{assessmentTypeId}' and q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}' order by q.""CreatedDate"" desc";
            return await _queryRepo1.ExecuteQueryList<AssessmentQuestionsViewModel>(query, null);
        }

        public async Task<bool> DeleteAssessmentQuestion(string NoteId)
        {
            var note = await GetSingleById(NoteId);
            if (note != null)
            {
                var query = $@"Update cms.""N_TAS_AssessmentQuestion"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
                await _queryRepo.ExecuteCommand(query, null);

                await Delete(NoteId);
                return true;
            }
            return false;
        }

        public async Task<bool> MapQuestionsToAssessment(string assessmentId, string qIds)
        {

            string[] QId = qIds.Trim(',').Split(",").ToArray();

            var mappedquelist = await GetMappedQuestionsList(assessmentId);
            var quecount = mappedquelist.Count();
            var seq = quecount > 0 ? quecount : 0;

            foreach (var id in QId)
            {
                var noteTemplate = new NoteTemplateViewModel();
                noteTemplate.ActiveUserId = _userContext.UserId;
                noteTemplate.TemplateCode = "TAS_ASSESSMENT_QUESTION";
                noteTemplate.DataAction = DataActionEnum.Create;
                var assQues = await _noteBusiness.GetNoteDetails(noteTemplate);
                assQues.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

                var assessmentQuesModel = new AssessmentViewModel();
                assessmentQuesModel.AssessmentId = assessmentId;
                assessmentQuesModel.QuestionId = id;
                assessmentQuesModel.SequenceOrder = seq + 1;

                assQues.Json = JsonConvert.SerializeObject(assessmentQuesModel);

                var assQuesResult = await _noteBusiness.ManageNote(assQues);

                seq++;
            }
            return true;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadPerformanceTaskViewData(string projectId)
        {
            var query = @$"             
            select st.""SubTaskCount"" as SubTaskCount,
st.""Pending"" as PendingCount,
 st.""Complete"" as TotalCompletedCount,
nts.""TemplateCode"" as TemplateCode,tt.""Code"" as Code,'{projectId}' as ProjectId,t.""TaskStatusId"" as TaskStatusId,l.""Name"" as TaskStatus,u.""PhotoId"" as PhotoId,
u.""Id"" as UserId,o.""Id"" as ProjectOwnerId,r.""Id"" as RequestedByUserId,u.""Name"" as UserName,t.""Id"" as TaskId,SUBSTRING (t.""TaskSubject"", 1, 30)  as TaskName,t.""StartDate"" as StartDate,t.""DueDate"" as DueDate,u.""Name"" as UserName,t.""TaskPriorityId"" as Priority,l.""Code"" as TaskStatusCode
               FROM public.""NtsTask"" as t
                 left join public.""User"" as u on u.""Id""=t.""AssignedToUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
 left join public.""User"" as o on o.""Id""=t.""OwnerUserId"" and o.""IsDeleted""=false and o.""CompanyId""='{_repo.UserContext.CompanyId}'
 left join public.""User"" as r on r.""Id""=t.""RequestedByUserId""  and r.""IsDeleted""=false and r.""CompanyId""='{_repo.UserContext.CompanyId}'
                    left join(
			with recursive task as(
            select nt.""TaskSubject"" as TaskSubject,nt.""Id"",nt.""Id"" as ""TaskId"",tl.""Code"",nt.""TaskStatusId"",'parent' as type
            from public.""NtsTask"" as nt 
	         left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false and tl.""CompanyId""='{_repo.UserContext.CompanyId}'
			where nt.""ParentServiceId""='{projectId}' and nt.""ParentTaskId"" is null  and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
            union all
            select nt.""TaskSubject"" as TaskSubject,nt.""Id"",em.""TaskId"",tl.""Code"",nt.""TaskStatusId"" ,'child' as type
            from public.""NtsTask"" as nt 
            join task as em on em.""Id""=nt.""ParentTaskId"" and em.""IsDeleted""=false and em.""CompanyId""='{_repo.UserContext.CompanyId}'
	        left join public.""LOV"" as tl on tl.""Id"" = nt.""TaskStatusId"" and tl.""IsDeleted""=false and tl.""CompanyId""='{_repo.UserContext.CompanyId}'
            )select count(""Id"") as ""SubTaskCount"",
			sum(case when ""Code""='TASK_STATUS_INPROGRESS'  then 1 else 0 end) as ""Pending"",
			sum(case when ""Code""='TASK_STATUS_COMPLETE' then 1 else 0 end) as ""Complete"",""TaskId""
						  from task
						  where type='child'
					group by ""TaskId""
					
					)as st on st.""TaskId""=t.""Id""
                      left join public.""LOV"" as l on l.""Id""=t.""TaskStatusId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
                    left join public.""NtsService"" as nts on nts.""Id""=t.""ParentServiceId"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
   join public.""Template"" as tt on tt.""Id""=nts.""TemplateId"" and tt.""IsDeleted""=false and tt.""CompanyId""='{_repo.UserContext.CompanyId}'
                 where t.""ParentServiceId""='{projectId}' and t.""ParentTaskId"" is null and nts.""OwnerUserId""='{_repo.UserContext.UserId}' and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
   group by t.""Id"",t.""TaskSubject"",t.""StartDate"",t.""DueDate"",u.""Name"",t.""TaskPriorityId"",u.""PhotoId"",
   u.""Id"",u.""Name"",l.""Name"",tt.""Code"",o.""Id"",r.""Id"",l.""Code"",nts.""TemplateCode"",st.""SubTaskCount"",st.""Pending"",st.""Complete""
";





            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }
        public async Task<IList<TeamWorkloadViewModel>> ReadPerformanceDevelopmentViewData()
        {
            var query = @$"
               select s.""Id"" as Id,u.""Id"" as ProjectOwnerId,u.""Name"" as ProjectOwnerName,s.""ServiceSubject"" as StageName,
              sp.""Name"" as ""Priority"",ss.""Name"" as ""NtsStatus"",s.""StartDate"",s.""DueDate"",s.""ParentServiceId"",r.""Id"" as RequestedByUserId,
               u.""PhotoId"" as PhotoId, s.""ServiceDescription"" as ""ServiceDescription""

                                       FROM public.""NtsService"" as s 
									left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left join public.""LOV"" as ss on ss.""Id"" = s.""ServiceStatusId"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_repo.UserContext.CompanyId}'
                         left join public.""User"" as u on u.""Id"" = s.""OwnerUserId""  and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
 left join public.""User"" as r on r.""Id"" = s.""RequestedByUserId"" and r.""IsDeleted""=false and r.""CompanyId""='{_repo.UserContext.CompanyId}'
                where s.""TemplateCode""='PMS_DEVELOPMENT' and s.""OwnerUserId""='{_repo.UserContext.UserId}' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}' ";


            var queryData = await _queryTWRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<IdNameViewModel>> GetCompetencyList()
        {
            var query = $@"Select c.""Id"", c.""CompetencyLevel"" as Name
                            From public.""NtsNote"" as n
                            join cms.""N_TAS_CompetencyLevel"" as c on n.""Id""=c.""NtsNoteId"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}' ";

            var result = await _idNameRepo.ExecuteQueryList(query, null);
            return result;
        }

        public async Task<List<AssignmentViewModel>> ReadCertificationData(string userId)
        {
            var query = $@"select hp.""Id"",u.""Id"", crf.""CertificationName"" as ""CertificationName"",crf.""CertificateReferenceNo"" as ""CertificateReferenceNo"",crf.""ExpiryLicenseDate"" as ""ExpiryLicenseDate""
				from cms.""N_CoreHR_HRPerson"" as hp
				left join public.""User"" as u on u.""Id""=hp.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join cms.""N_TAS_Certification"" as crf on crf.""UserId""=hp.""UserId"" and crf.""IsDeleted""=false and crf.""CompanyId""='{_repo.UserContext.CompanyId}'
				where hp.""IsDeleted""=false and u.""Id"" = '{userId}'  and hp.""CompanyId""='{_repo.UserContext.CompanyId}'";
            return await _queryAssignment.ExecuteQueryList<AssignmentViewModel>(query, null);
        }
        public async Task<List<AssignmentViewModel>> ReadSkillData(string userId)
        {
            var query = $@"select UA.""Score"" as ""ResultScore"",CM.""CompetencyName"" as ""CompetencyName""
from cms.""N_TAS_UserAssessmentResult"" as UA
left join public.""User"" as U on U.""Id""=UA.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
Left join cms.""N_TAS_UserAssessmentAnswer"" as UAA on UAA.""AssessmentId"" =UA.""AssessmentId"" and UAA.""IsDeleted""=false  and UAA.""CompanyId""='{_repo.UserContext.CompanyId}'
Left join cms.""N_TAS_Question"" as Q on Q.""Id"" = UAA.""QuestionId"" and Q.""IsDeleted""=false  and Q.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_PerformanceDocument_CompetencyMaster"" as CM on CM.""Id""=Q.""CompetencyId"" and CM.""IsDeleted""=false  and CM.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_PerformanceDocument_PMSCompentency"" as PC on PC.""CompetencyMasterId""=CM.""Id"" and PC.""IsDeleted""=false  and PC.""CompanyId""='{_repo.UserContext.CompanyId}'
 left join public.""LOV"" as LOV on PC.""PmsTypeId""=LOV.""Id"" and LOV.""IsDeleted""=false  and LOV.""CompanyId""='{_repo.UserContext.CompanyId}'
 where LOV.""Code""='SKILL' and UA.""IsDeleted""=false and u.""Id"" = '{userId}'  and UA.""CompanyId""='{_repo.UserContext.CompanyId}'";
            return await _queryAssignment.ExecuteQueryList<AssignmentViewModel>(query, null);
        }

        public async Task<List<AssessmentDetailViewModel>> GetAssessmentListByUserId(string userId, string statusCode)
        {
            var query = $@"Select s.*,ars.""Id"" as ServiceId, a.""Id"" as AssessmentId,a.""AssessmentName"",udf.""AssessmentStartDate"" as ScheduledStartDate,udf.""EndDate"" as ScheduledEndDate
,at.""Name"" as AssessmentType, ar.""StartDate"" as ActualStartDate,ar.""EndDate"" as ActualEndDate,pl.""Code"" as PreferredLanguageCode, arss.""Name"" as AssessmentStatus, arss.""Code"", ar.""IsAssessmentStopped"",udf.""Url"" as AssessmentUrl
from public.""NtsService"" as s
Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""Code""='TAS_USER_ASSESSMENT_SCHEDULE' and t.""IsDeleted""=false  and t.""CompanyId""='{_repo.UserContext.CompanyId}'
Join public.""NtsNote"" as nts on s.""UdfNoteId""=nts.""Id"" and nts.""IsDeleted""=false  and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_UserAssessmentSchedule"" as udf on nts.""Id""=udf.""NtsNoteId"" and udf.""UserId""='{userId}' and udf.""IsDeleted""=false and udf.""SlotType""='0' and udf.""CompanyId""='{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as pl on udf.""PreferedLanguageId""=pl.""Id""  and pl.""IsDeleted""=false  and pl.""CompanyId""='{_repo.UserContext.CompanyId}'
left Join cms.""N_TAS_Assessment"" as a on udf.""AssessmentId""=a.""Id"" and a.""IsDeleted""=false  and a.""CompanyId""='{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as at on a.""AssessmentTypeId""=at.""Id"" and at.""IsDeleted""=false  and at.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_UserAssessmentResult"" as ar on a.""Id""=ar.""AssessmentId"" and ar.""ScheduledAssessmentId""= udf.""Id"" and ar.""IsDeleted""=false  and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsNote"" as arn on ar.""NtsNoteId""=arn.""Id"" and arn.""IsDeleted""=false  and arn.""CompanyId""='{_repo.UserContext.CompanyId}'
left Join public.""NtsService"" as ars on arn.""Id""=ars.""UdfNoteId"" and ars.""IsDeleted""=false  and ars.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as arss on ars.""ServiceStatusId""=arss.""Id"" and arss.""IsDeleted""=false  and arss.""CompanyId""='{_repo.UserContext.CompanyId}'
where(arss.""Code""!='SERVICE_STATUS_COMPLETE' or arss.""Code"" is null) and s.""IsDeleted""=false  and s.""CompanyId""='{_repo.UserContext.CompanyId}' order by udf.""AssessmentStartDate"" ";

            //var join = "";
            //if(statusCode == "SERVICE_STATUS_INPROGRESS")
            //{
            //    join = $@" Left";
            //}
            //query = query.Replace("#Join#", join);

            var result = await _queryAssDetail.ExecuteQueryList(query, null);
            foreach (var i in result)
            {
                i.DisplayScheduledStartDate = i.ScheduledStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayScheduledEndDate = i.ScheduledEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayActualStartDate = i.ActualStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayActualEndDate = i.ActualEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
            }
            return result;
        }

        public async Task<List<AssessmentDetailViewModel>> GetCompletedAssessmentListByUserId(string userId)
        {
            var query = $@"Select s.*,udf.""Id"" as AssessmentId,a.""AssessmentName"",udf.""AssessmentStartDate"" as ScheduledStartDate,udf.""EndDate"" as ScheduledEndDate
,udf1.""StartDate"" as ActualStartDate,udf1.""EndDate"" as ActualEndDate,pl.""Code"" as PreferredLanguageCode, ss.""Name"" as AssessmentStatus,udf.""Url"" as AssessmentUrl,at.""Name"" as AssessmentType
from public.""NtsService"" as s
Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""Code""='TAS_USER_ASSESSMENT_SCHEDULE' and t.""IsDeleted""=false  and t.""CompanyId""='{_repo.UserContext.CompanyId}'
Join public.""NtsNote"" as nts on s.""UdfNoteId""=nts.""Id"" and nts.""IsDeleted""=false  and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_UserAssessmentSchedule"" as udf on nts.""Id""=udf.""NtsNoteId"" and udf.""UserId""='{userId}' and udf.""IsDeleted""=false and udf.""SlotType""='0' and udf.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_Assessment"" as a on udf.""AssessmentId""=a.""Id"" and a.""IsDeleted""=false  and a.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_UserAssessmentResult"" as udf1 on a.""Id""=udf1.""AssessmentId"" and udf1.""ScheduledAssessmentId""= udf.""Id"" and udf1.""IsDeleted""=false  and udf1.""CompanyId""='{_repo.UserContext.CompanyId}'
Join public.""NtsNote"" as arn on udf1.""NtsNoteId""=arn.""Id"" and arn.""IsDeleted""=false  and arn.""CompanyId""='{_repo.UserContext.CompanyId}'
Join public.""NtsService"" as ars on arn.""Id""=ars.""UdfNoteId"" and ars.""IsDeleted""=false  and ars.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as ss on ars.""ServiceStatusId""=ss.""Id"" and ss.""Code""='SERVICE_STATUS_COMPLETE' and ss.""IsDeleted""=false  and ss.""CompanyId""='{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as pl on udf.""PreferedLanguageId""=pl.""Id""  and pl.""IsDeleted""=false  and pl.""CompanyId""='{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as at on a.""AssessmentTypeId""=at.""Id"" and at.""IsDeleted""=false  and at.""CompanyId""='{_repo.UserContext.CompanyId}'
where  s.""IsDeleted""=false  and s.""CompanyId""='{_repo.UserContext.CompanyId}' order by udf.""AssessmentStartDate"" ";

            var result = await _queryAssDetail.ExecuteQueryList(query, null);
            foreach (var i in result)
            {
                i.DisplayScheduledStartDate = i.ScheduledStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayScheduledEndDate = i.ScheduledEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayActualStartDate = i.ActualStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayActualEndDate = i.ActualEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
            }
            return result;
        }
        public async Task<List<IdNameViewModel>> GetAssessmentSetListIdName()
        {
            //var query = @"match (t:NTS_Template{IsDeleted:0})-[:R_TemplateRoot]->
            //        (tr: NTS_TemplateMaster{ Code: 'ASSESSMENT_SET',IsDeleted: 0})
            //    match(t) < -[:R_Note_Template] - (n: NTS_Note{ IsDeleted: 0})   return n.Id as Id, n.Subject as Name";

            var query = $@" select ast.""Id"" as Id,ast.""AssessmentSetName"" as Name 
                            from cms.""N_TAS_AssessmentSet"" as ast
                            where ast.""IsDeleted"" = false and ast.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _idNameRepo.ExecuteQueryList(query, null);
            return result.ToList();
        }

        public async Task<List<IdNameViewModel>> GetAssessmentListIdName(string assSetId)
        {
            //var query = @"match (t:NTS_Template{IsDeleted:0})-[:R_TemplateRoot]->
            //        (tr: NTS_TemplateMaster{ Code: 'ASSESSMENT_SET',IsDeleted: 0})
            //    match(t) < -[:R_Note_Template] - (n: NTS_Note{ IsDeleted: 0})   return n.Id as Id, n.Subject as Name";

            var query = $@"select a.""Id"", a.""AssessmentName"" as Name
from cms.""N_TAS_AssessmentSet"" as aset
join cms.""N_TAS_AssessmentSetAssessment"" as asa on asa.""AssessmentSetId"" = aset.""Id"" and asa.""IsDeleted"" = false and asa.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join cms.""N_TAS_Assessment"" as a on a.""Id"" = asa.""AssessmentId"" and a.""IsDeleted"" = false and a.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where aset.""Id"" = '{assSetId}' and aset.""IsDeleted"" = false and aset.""CompanyId"" = '{_repo.UserContext.CompanyId}'";

            var result = await _idNameRepo.ExecuteQueryList(query, null);
            return result.ToList();
        }

        public async Task<List<IdNameViewModel>> GetAssessmentProctorList()
        {
            //var cypher = @"match(u:ADM_User)-[:R_User_UserRole]->(r:ADM_UserRole{Name:'Proctor'}) return u.Id as Id, u.UserName as Name";
            var query = $@"select u.""Id"" as Id, u.""Name"" as Name
                            from public.""User"" as u
                            join public.""UserRoleUser"" as uru on uru.""UserId""=u.""Id"" and uru.""IsDeleted""=false  and uru.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join public.""UserRole"" as ur on ur.""Id""=uru.""UserRoleId"" and ur.""IsDeleted""=false  and ur.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where  u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}' and ur.""Code""='PROCTOR' ";
            var result = await _idNameRepo.ExecuteQueryList(query, null);
            return result.ToList();
        }

        public async Task<IList<IdNameViewModel>> GetIndicatorList(string levelId = null)
        {
            var query = $@"Select ind.""Id"", ind.""IndicatorName"" as Name
                            From cms.""N_TAS_Indicator"" as ind
                            join public.""NtsNote"" as n on ind.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where ind.""IsDeleted""=false and ind.""CompanyId""='{_repo.UserContext.CompanyId}' #LEVELWHERE# ";

            var levelwhere = "";
            if (levelId.IsNotNullAndNotEmpty())
            {
                levelwhere = $@" and ind.""CompetencyLevelId""='{levelId}' ";
            }
            query = query.Replace("#LEVELWHERE#", levelwhere);

            var result = await _idNameRepo.ExecuteQueryList(query, null);
            return result.ToList();
        }

        public async Task<IList<IdNameViewModel>> GetCompetencyLevelList(string noteId = null)
        {
            var query = $@"Select ind.""Id"", ind.""CompetencyLevel"" as Name
                            From cms.""N_TAS_CompetencyLevel"" as ind
                            join public.""NtsNote"" as n on ind.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where ind.""IsDeleted""=false and ind.""CompanyId""='{_repo.UserContext.CompanyId}' ";

            var result = await _idNameRepo.ExecuteQueryList(query, null);
            return result.ToList();
        }

        public async Task<IList<IdNameViewModel>> GetCompetencyLevelListByTopic(string topicId)
        {
            var query = $@"SELECT count(q.*) as Count,l.""CompetencyLevel"" as Name,l.""Id"" FROM cms.""N_TAS_CompetencyLevel"" l
join public.""NtsNote"" nl  on nl.""Id""=l.""NtsNoteId"" and nl.""IsDeleted""=false and nl.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_TAS_Question"" q on q.""CompentencyLevelId""=l.""Id"" and q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" n  on n.""Id""=q.""NtsNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
where n.""ParentNoteId""='{topicId}' and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
group by l.""CompetencyLevel"",l.""Id"" ";

            var result = await _idNameRepo.ExecuteQueryList(query, null);
            return result.ToList();
        }

        public async Task<IdNameViewModel> GetQuestionCountByTopic(string topicId)
        {
            var query = $@"SELECT count(q.*) as Count FROM cms.""N_TAS_CompetencyLevel"" l
join public.""NtsNote"" nl  on nl.""Id""=l.""NtsNoteId"" and nl.""IsDeleted""=false and nl.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_TAS_Question"" q on q.""CompentencyLevelId""=l.""Id"" and q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" n  on n.""Id""=q.""NtsNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
where n.""ParentNoteId"" in ('{topicId}') and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
 ";

            var result = await _idNameRepo.ExecuteQuerySingle(query, null);
            return result;
        }

        public async Task<List<IdNameViewModel>> GetJobTitle(string loggedInUserId)
        {
            //var cypher = @"match (t:NTS_Template{IsDeleted:0})-[:R_TemplateRoot]->
            //(tr: NTS_TemplateMaster{ Code: 'ASSESSMENT_SLOT',IsDeleted: 0})
            //match(t) < -[:R_Note_Template] - (n: NTS_Note{ IsDeleted: 0})
            //match(n)< -[:R_NoteFieldValue_Note] - (nfv2: NTS_NoteFieldValue) 
            //-[:R_NoteFieldValue_TemplateField]->(tf2: NTS_TemplateField{ FieldName: 'jobTitle'})
            //where nfv2.Code is not null
            //return case when nfv2.Code is null then '' else nfv2.Code end as Name, 0 as Id";
            //var data = ExecuteCypherList<IdNameViewModel>(cypher, prms).ToList();
            //return data.Where(x => x.Name.IsNotNullAndNotEmpty()).ToList();

            var query = $@" select uas.""Id"" as ""Id"",uas.""JobTitle"" as ""Name""
                            from cms.""N_TAS_UserAssessmentSchedule"" as uas
                            where uas.""IsDeleted""=false  and uas.""CompanyId""='{_repo.UserContext.CompanyId}' and uas.""JobTitle"" is not null #WHEREUSER#
                        ";
            var whereuser = "";
            //if (loggedInUserId.IsNotNullAndNotEmpty())
            //{
            //    whereuser = $@" and uas.""UserId""='{loggedInUserId}' ";
            //}
            query = query.Replace("#WHEREUSER#", whereuser);

            var result = await _idNameRepo.ExecuteQueryList(query, null);
            result = result.Where(x => x.Name.IsNotNullAndNotEmpty()).ToList();
            return result;
        }
        public async Task<List<AssessmentDetailViewModel>> GetAllAssessmentsList()
        {
            var query = $@"Select s.*,a.""Id"" as AssessmentId,a.""AssessmentName"",udf.""UserId"",udf.""PreferedLanguageId"" as PreferredLanguageId,udf.""AssessmentStartDate"" as ScheduledStartDate,udf.""EndDate"" as ScheduledEndDate
, ar.""StartDate"" as ActualStartDate,ar.""EndDate"" as ActualEndDate,pl.""Code"" as PreferredLanguageCode,ar.""IsAssessmentStopped"",
arss.""Name"" as AssessmentStatus, arss.""Code"", u.""Name"" as UserName, u.""Email"",at.""Name"" as AssessmentType,udf.""Id"" as ScheduledAssessmentId
from public.""NtsService"" as s
Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""Code""='TAS_USER_ASSESSMENT_SCHEDULE' and t.""IsDeleted""=false  and t.""CompanyId""='{_repo.UserContext.CompanyId}'
Join public.""NtsNote"" as nts on s.""UdfNoteId""=nts.""Id"" and nts.""IsDeleted""=false  and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_UserAssessmentSchedule"" as udf on nts.""Id""=udf.""NtsNoteId"" and udf.""IsDeleted""=false and udf.""SlotType""='0' and udf.""CompanyId""='{_repo.UserContext.CompanyId}'
Join public.""User"" as u on udf.""UserId""=u.""Id"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left Join cms.""N_TAS_Assessment"" as a on udf.""AssessmentId""=a.""Id"" and a.""IsDeleted""=false  and a.""CompanyId""='{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as at on a.""AssessmentTypeId""=at.""Id"" and at.""IsDeleted""=false  and at.""CompanyId""='{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as pl on udf.""PreferedLanguageId""=pl.""Id""  and pl.""IsDeleted""=false  and pl.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_UserAssessmentResult"" as ar on a.""Id""=ar.""AssessmentId"" and udf.""Id""=ar.""ScheduledAssessmentId"" and ar.""IsDeleted""=false  and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsNote"" as arn on ar.""NtsNoteId""=arn.""Id"" and arn.""IsDeleted""=false  and arn.""CompanyId""='{_repo.UserContext.CompanyId}'
left Join public.""NtsService"" as ars on arn.""Id""=ars.""UdfNoteId"" and ars.""IsDeleted""=false  and ars.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as arss on ars.""ServiceStatusId""=arss.""Id"" and arss.""IsDeleted""=false  and arss.""CompanyId""='{_repo.UserContext.CompanyId}'
where  s.""IsDeleted""=false  and s.""CompanyId""='{_repo.UserContext.CompanyId}'
order by udf.""AssessmentStartDate"" desc ";

            var result = await _queryAssDetail.ExecuteQueryList(query, null);

            return result;
        }

        public async Task<AssessmentResultViewModel> GetQuestion(string serviceId)
        {
            var query = $@"Select n.""Id"" as ServiceId, ar.""TimeElapsed"",ar.""AssessmentId"" as AssessmentId, ar.""CurrentQuestionId"", ss.""Code"" as ServiceStatusCode
                            From public.""NtsService"" as n
                            Join public.""Template"" as t on n.""TemplateId""=t.""Id"" and t.""Code""='TAS_USER_ASSESSMENT_RESULT' and t.""IsDeleted""=false  and t.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join public.""NtsNote"" as nts on n.""UdfNoteId""=nts.""Id"" and nts.""IsDeleted""=false  and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_TAS_UserAssessmentResult"" as ar on nts.""Id""=ar.""NtsNoteId"" and ar.""IsDeleted""=false  and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join public.""LOV"" as ss on n.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false  and ss.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where n.""Id""='{serviceId}' and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _queryAssRes.ExecuteQuerySingle(query, null);
            return result;
        }
        public async Task<SurveyTopicViewModel> GetSurveyQuestion(string serviceId)
        {
            var query = $@"Select n.""Id"" as ServiceId,ar.""SurveyId"" as SurveyId, ar.""CurrentTopicId"", ss.""Code"" as ServiceStatusCode
                            From public.""NtsService"" as n
                            Join public.""Template"" as t on n.""TemplateId""=t.""Id"" and t.""Code""='S_SURVEY_RESULT' and t.""IsDeleted""=false  and t.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join public.""NtsNote"" as nts on n.""UdfNoteId""=nts.""Id"" and nts.""IsDeleted""=false  and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_SURVEY_S_SurveyResult"" as ar on nts.""Id""=ar.""NtsNoteId"" and ar.""IsDeleted""=false  and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join public.""LOV"" as ss on n.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false  and ss.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where n.""Id""='{serviceId}' and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _queryAssRes.ExecuteQuerySingle<SurveyTopicViewModel>(query, null);
            return result;
        }

        public async Task<SurveyScheduleViewModel> GetSurveyClosingInstruction(string surveyId)
        {
            var query = $@"select ss.*
                            from cms.""N_SURVEY_SurveySchedule"" as ss
                            WHERE ss.""IsDeleted""=false and ss.""SurveyId""='{surveyId}'  and ss.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _queryAssRes.ExecuteQuerySingle<SurveyScheduleViewModel>(query, null);
            return result;
        }

        public async Task<AssessmentDetailViewModel> GetTryoutQuestion(string assessmentId, string currQueId, string lang = null)
        {
            return await GetAssessmentTryoutQuestion(assessmentId, currQueId, lang);

        }
        public async Task<AssessmentDetailViewModel> GetAssessmentTryoutQuestion(string assessmentId, string curQueId, string lang)
        {
            var result = new AssessmentDetailViewModel();

            var query = $@"select aq.""QuestionId"" as Id,aq.""SequenceOrder"" as SequenceNo, #QUESTIONLANGUAGE#,q.""NtsNoteId"" as NoteId
From cms.""N_TAS_Assessment"" as a
Join cms.""N_TAS_AssessmentQuestion"" as aq on a.""Id""=aq.""AssessmentId"" and aq.""IsDeleted""=false and aq.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_Question"" as q on aq.""QuestionId""=q.""Id""  and q.""IsDeleted""=false  and q.""CompanyId""='{_repo.UserContext.CompanyId}'
where a.""Id""='{assessmentId}' and a.""IsDeleted""=false  and a.""CompanyId""='{_repo.UserContext.CompanyId}' order by aq.""SequenceOrder"" ";

            var queslang = "";
            if (lang.IsNotNullAndNotEmpty())
            {
                if (lang == "ENGLISH")
                {
                    queslang = $@"q.""Question"", q.""QuestionDescription"",q.""QuestionAttachmentId"" as FileId ";
                }
                else if (lang == "ARABIC")
                {
                    queslang = $@"q.""QuestionArabic"" as Question, q.""QuestionDescriptionArabic"" as QuestionDescription,q.""QuestionArabicAttachmentId"" as FileId ";
                }
            }
            else
            {
                queslang = $@"q.""Question"", q.""QuestionDescription"",q.""QuestionAttachmentId"" as FileId ";
            }
            query = query.Replace("#QUESTIONLANGUAGE#", queslang);

            var list = await _queryAssDetail.ExecuteQueryList(query, null);

            if (list != null && list.Count != 0)
            {
                if (curQueId.IsNullOrEmpty())
                {
                    result.NoteId = list.Where(x => x.SequenceNo == 1).Select(x => x.Id).FirstOrDefault();
                    result.NextId = list.Where(x => x.SequenceNo == 2).Select(x => x.Id).FirstOrDefault();
                }
                else
                {
                    result.NoteId = curQueId;
                    var seq = list.Where(x => x.Id == curQueId).Select(x => x.SequenceNo).FirstOrDefault();
                    var next = seq + 1;
                    var prev = seq - 1;
                    result.PreviousId = list.Where(x => x.SequenceNo == prev).Select(x => x.Id).FirstOrDefault();
                    result.NextId = list.Where(x => x.SequenceNo == next).Select(x => x.Id).FirstOrDefault();
                }
                result.FirstId = list.First().Id;
                result.LastId = list.Last().Id;

                result.OptionList = new List<AssessmentQuestionsOptionViewModel>();

                if (result.NoteId.IsNotNull())
                {
                    var question = list.Where(x => x.Id == result.NoteId).FirstOrDefault();
                    if (question != null)
                    {
                        result.Subject = HttpUtility.HtmlDecode(question.Question);
                        result.Description = HttpUtility.HtmlDecode(question.QuestionDescription);
                        result.TotalCount = list.Count;
                        result.SequenceNo = list.Where(x => x.Id == result.NoteId).Select(x => x.SequenceNo).FirstOrDefault();
                        result.AssessmentDuration = list.Where(x => x.Id == result.NoteId).Select(x => x.AssessmentDuration).FirstOrDefault();

                        result.FileId = list.Where(x => x.Id == result.NoteId).Select(x => x.FileId).FirstOrDefault();

                    }
                    result.OptionList = await GetTryoutQuestionOptions(result.NoteId, lang);
                    //if (result.OptionList != null && result.OptionList.Count > 0)
                    //{
                    //    var answer = result.OptionList.FirstOrDefault();
                    //    if (answer != null)
                    //    {
                    //        //result.CandidateAnswer = answer.CandidateAnswerComment;
                    //        result.CandidateAnswerComment = answer.AnswerComment;
                    //        result.Score = answer.Score;
                    //        //result.InterviewerComment = answer.InterviewerComment;
                    //        result.AnswerId = answer.AnswerId;
                    //    }
                    //}
                }
            }
            return result;
        }

        public async Task<List<AssessmentQuestionsOptionViewModel>> GetTryoutQuestionOptions(string noteId, string lang)
        {
            var query = $@"Select o.""Id"", #OPTIONLANGUAGE#, o.""IsRightAnswer"",o.""Score""
From cms.""N_TAS_Question"" as q
join public.""NtsNote"" as n on q.""NtsNoteId""=n.""ParentNoteId"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_QuestionOption"" as o on n.""Id""=o.""NtsNoteId"" and o.""IsDeleted""=false  and o.""CompanyId""='{_repo.UserContext.CompanyId}'
where q.""Id""='{noteId}' and q.""IsDeleted""=false  and q.""CompanyId""='{_repo.UserContext.CompanyId}' order by o.""SequenceOrder"" ";

            var optlang = "";
            if (lang.IsNotNullAndNotEmpty())
            {
                if (lang == "ENGLISH")
                {
                    optlang = $@"o.""Option"" ";
                }
                else if (lang == "ARABIC")
                {
                    optlang = $@"o.""OptionArabic"" as Option";
                }
            }
            else
            {
                optlang = $@"o.""Option"" ";
            }
            query = query.Replace("#OPTIONLANGUAGE#", optlang);
            var result = await _queryRepo1.ExecuteQueryList(query, null);
            return result;
        }

        public async Task<AssessmentDetailViewModel> GetAssessmentQuestionEnglish(string serviceId, string currQueId)
        {
            if (currQueId.IsNullOrEmpty())
            {
                var que = $@"Select ar.""CurrentQuestionId""
                           From public.""NtsService"" as s
                           Join public.""NtsNote"" as n on s.""UdfNoteId""=n.""Id"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                           Join cms.""N_TAS_UserAssessmentResult"" as ar on n.""Id""=ar.""NtsNoteId"" and ar.""IsDeleted""=false and ar.""UserId""='{_repo.UserContext.UserId}' and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
                           where s.""Id""='{serviceId}' and s.""IsDeleted""=false  and s.""CompanyId""='{_repo.UserContext.CompanyId}'";

                var res = await _queryAssDetail.ExecuteQuerySingle(que, null);

                return await GetAssessmentQuestionParent(serviceId, res.CurrentQuestionId, "ENGLISH");
            }
            else
            {
                return await GetAssessmentQuestionParent(serviceId, currQueId, "ENGLISH");
            }

        }
        public async Task<AssessmentDetailViewModel> GetAssessmentQuestionArabic(string serviceId, string currQueId)
        {
            if (currQueId.IsNullOrEmpty())
            {
                var que = $@"Select ar.""CurrentQuestionId""
                           From public.""NtsService"" as s
                           Join public.""NtsNote"" as n on s.""UdfNoteId""=n.""Id"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                           Join cms.""N_TAS_UserAssessmentResult"" as ar on n.""Id""=ar.""NtsNoteId"" and ar.""IsDeleted""=false  and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
                           where s.""Id""='{serviceId}'  and s.""IsDeleted""=false  and s.""CompanyId""='{_repo.UserContext.CompanyId}'";

                var res = await _queryAssDetail.ExecuteQuerySingle(que, null);

                return await GetAssessmentQuestionParent(serviceId, res.CurrentQuestionId, "ARABIC");
            }
            else
            {
                return await GetAssessmentQuestionParent(serviceId, currQueId, "ARABIC");
            }

        }

        public async Task<AssessmentDetailViewModel> GetAssessmentQuestionParent(string serviceId, string curQueId, string lang)
        {
            var result = new AssessmentDetailViewModel();

            var query = $@"select aq.""QuestionId"" as Id,aq.""SequenceOrder"" as SequenceNo, #QUESTIONLANGUAGE#,q.""NtsNoteId"" as NoteId, a.""AssessmentDuration"", ar.""TimeElapsed"", ar.""TimeElapsedSec"", ar.""IsAssessmentStopped""
      ,ar.""AssessmentId""  as AssessmentId From public.""NtsService"" as s
Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""Code""='TAS_USER_ASSESSMENT_RESULT' and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
Join public.""NtsNote"" as nts on s.""UdfNoteId""=nts.""Id"" and nts.""IsDeleted""=false  and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_UserAssessmentResult"" as ar on nts.""Id""=ar.""NtsNoteId"" and ar.""IsDeleted""=false and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_Assessment"" as a on ar.""AssessmentId""=a.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_AssessmentQuestion"" as aq on a.""Id""=aq.""AssessmentId"" and aq.""IsDeleted""=false and aq.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_Question"" as q on aq.""QuestionId""=q.""Id"" and q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}'
where s.""Id""='{serviceId}' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}' order by aq.""SequenceOrder"" ";

            var queslang = "";
            if (lang.IsNotNullAndNotEmpty())
            {
                if (lang == "ENGLISH")
                {
                    queslang = $@"q.""Question"", q.""QuestionDescription"", q.""QuestionAttachmentId"" as FileId ";
                }
                else if (lang == "ARABIC")
                {
                    queslang = $@"q.""QuestionArabic"" as Question, q.""QuestionDescriptionArabic"" as QuestionDescription, q.""QuestionArabicAttachmentId"" as FileId ";
                }
            }
            else
            {
                queslang = $@"q.""Question"", q.""QuestionDescription""";
            }
            query = query.Replace("#QUESTIONLANGUAGE#", queslang);

            var list = await _queryAssDetail.ExecuteQueryList(query, null);

            if (list != null && list.Count != 0)
            {
                if (curQueId.IsNullOrEmpty())
                {
                    result.NoteId = list.Where(x => x.SequenceNo == 1).Select(x => x.Id).FirstOrDefault();
                    result.NextId = list.Where(x => x.SequenceNo == 2).Select(x => x.Id).FirstOrDefault();
                }
                else
                {
                    result.NoteId = curQueId;
                    var seq = list.Where(x => x.Id == curQueId).Select(x => x.SequenceNo).FirstOrDefault();
                    var next = seq + 1;
                    var prev = seq - 1;
                    result.PreviousId = list.Where(x => x.SequenceNo == prev).Select(x => x.Id).FirstOrDefault();
                    result.NextId = list.Where(x => x.SequenceNo == next).Select(x => x.Id).FirstOrDefault();
                }
                result.FirstId = list.First().Id;
                result.LastId = list.Last().Id;
                result.AssessmentId = list.First().AssessmentId;
                result.OptionList = new List<AssessmentQuestionsOptionViewModel>();

                if (result.NoteId.IsNotNull())
                {
                    var question = list.Where(x => x.Id == result.NoteId).FirstOrDefault();
                    if (question != null)
                    {
                        result.Subject = HttpUtility.HtmlDecode(question.Question);
                        result.Description = HttpUtility.HtmlDecode(question.QuestionDescription);
                        result.TotalCount = list.Count;
                        result.SequenceNo = list.Where(x => x.Id == result.NoteId).Select(x => x.SequenceNo).FirstOrDefault();
                        result.AssessmentDuration = list.Where(x => x.Id == result.NoteId).Select(x => x.AssessmentDuration).FirstOrDefault();
                        result.TimeElapsed = list.Where(x => x.Id == result.NoteId).Select(x => x.TimeElapsed).FirstOrDefault();
                        result.TimeElapsedSec = list.Where(x => x.Id == result.NoteId).Select(x => x.TimeElapsedSec).FirstOrDefault();
                        result.FileId = list.Where(x => x.Id == result.NoteId).Select(x => x.FileId).FirstOrDefault();
                        result.IsAssessmentStopped = list.Where(x => x.Id == result.NoteId).Select(x => x.IsAssessmentStopped).FirstOrDefault();
                    }
                    result.OptionList = await GetOptions(result.NoteId, serviceId, lang, result.AssessmentId);
                    if (result.OptionList != null && result.OptionList.Count > 0)
                    {
                        var answer = result.OptionList.FirstOrDefault();
                        if (answer != null)
                        {
                            //result.CandidateAnswer = answer.CandidateAnswerComment;
                            result.CandidateAnswerComment = answer.AnswerComment;
                            result.Score = answer.Score;
                            //result.InterviewerComment = answer.InterviewerComment;
                            result.AnswerId = answer.AnswerId;
                        }
                    }

                }
            }

            return result;
        }

        public async Task<AssessmentAnswerViewModel> GetQuestionByAnswerId(string noteId)
        {
            var query = $@"select * from cms.""N_TAS_UserAssessmentAnswer"" where ""NtsNoteId""='{noteId}' ";
            var result = await _queryAssAns.ExecuteQuerySingle(query, null);
            return result;
        }

        public async Task<bool> UpdateScoreComment(List<AssessmentAnswerViewModel> modelList)
        {
            string query = "";
            foreach (var item in modelList)
            {
                if (item.CaseStudyComment.IsNotNullAndNotEmpty())
                {
                    item.CaseStudyComment = item.CaseStudyComment.Replace("'", "''");
                }
                query = string.Concat(query, $@"Update cms.""N_TAS_UserAssessmentAnswer"" set ""Score""={item.Score}, ""CaseStudyComment""='{item.CaseStudyComment}'
                            where ""NtsNoteId""='{item.Id}'", ";");
            }

            await _queryAssAns.ExecuteCommand(query, null);

            return true;

        }

        public async Task<List<AssessmentQuestionsOptionViewModel>> GetOptions(string noteId, string serviceId, string lang, string assessmentId)
        {
            //var query = $@"Select o.""Id"", o.""Option"", o.""IsRightAnswer"",o.""Score""
            //From cms.""N_TAS_Question"" as q 
            //join public.""NtsNote"" as n on q.""NtsNoteId""=n.""ParentNoteId""
            //Join cms.""N_TAS_QuestionOption"" as o on n.""Id""=o.""NtsNoteId""
            //where q.""Id""='{noteId}' ";

            var query = $@"Select o.""Id"", #OPTIONLANGUAGE#, o.""IsRightAnswer"",o.""Score"",ans.""OptionId"" as AnswerId,ans.""Comment"" as AnswerComment
From cms.""N_TAS_Question"" as q
join public.""NtsNote"" as n on q.""NtsNoteId""=n.""ParentNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_QuestionOption"" as o on n.""Id""=o.""NtsNoteId"" and o.""IsDeleted""=false and o.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_UserAssessmentAnswer"" as ans on q.""Id""=ans.""QuestionId"" and ans.""IsDeleted""=false and ans.""UserId""='{_repo.UserContext.UserId}' and ans.""AssessmentId""='{assessmentId}' and ans.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsNote"" as nts on ans.""NtsNoteId""=nts.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsService"" as s on nts.""Id""=s.""UdfNoteId"" and s.""ParentServiceId""='{serviceId}' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
where q.""Id""='{noteId}' and q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}' order by o.""SequenceOrder""";

            var optlang = "";
            if (lang.IsNotNullAndNotEmpty())
            {
                if (lang == "ENGLISH")
                {
                    optlang = $@"o.""Option"" ";
                }
                else if (lang == "ARABIC")
                {
                    optlang = $@"o.""OptionArabic"" as Option";
                }
            }
            else
            {
                optlang = $@"o.""Option"" ";
            }
            query = query.Replace("#OPTIONLANGUAGE#", optlang);
            var result = await _queryRepo1.ExecuteQueryList(query, null);
            return result;
        }

        public async Task SubmitAssessmentAnswer(string serviceId, bool isSubmit, string currentQuestionId, string multipleFieldValueId, string comment, int? timeElapsed, int? timeElapsedSec, string nextQuestionId, string currentAnswerId, string assessmentType, string userId, string assessmentId)
        {

            if (currentAnswerId.IsNotNullAndNotEmpty())
            {
                //if(currentAnswerId != multipleFieldValueId)
                //{
                var arquery = $@"select ans.* from public.""NtsService"" as s
join public.""NtsNote"" as n on s.""UdfNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_TAS_UserAssessmentAnswer"" as ans on n.""Id""=ans.""NtsNoteId"" and ans.""QuestionId""='{currentQuestionId}' and ans.""IsDeleted""=false and ans.""CompanyId""='{_repo.UserContext.CompanyId}'
where s.""ParentServiceId""='{serviceId}' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'";

                var result = await _queryAssAns.ExecuteQuerySingle(arquery, null);
                if (result != null)
                {
                    var serdetail = await _serviceBusiness.GetSingle(x => x.UdfNoteId == result.NtsNoteId);

                    ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
                    serviceModel.ActiveUserId = _userContext.UserId;
                    serviceModel.TemplateCode = "TAS_USER_ASSESSMENT_ANSWER";
                    serviceModel.ServiceId = serdetail.Id;
                    var service = await _serviceBusiness.GetServiceDetails(serviceModel);
                    service.DataAction = DataActionEnum.Edit;
                    service.AllowPastStartDate = true;

                    result.OptionId = multipleFieldValueId;
                    result.Comment = comment;

                    service.Json = JsonConvert.SerializeObject(result);

                    var res = await _serviceBusiness.ManageService(service);
                }
                //}
            }
            else
            {
                var model = new AssessmentAnswerViewModel()
                {
                    QuestionId = currentQuestionId,
                    OptionId = multipleFieldValueId,
                    Comment = comment,
                    UserId = userId,
                    AssessmentId = assessmentId
                };

                ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
                serviceModel.ActiveUserId = _userContext.UserId;
                serviceModel.TemplateCode = "TAS_USER_ASSESSMENT_ANSWER";
                var service = await _serviceBusiness.GetServiceDetails(serviceModel);

                //service.ServiceSubject = ;
                service.OwnerUserId = _userContext.UserId;
                service.StartDate = DateTime.Now;
                service.DueDate = DateTime.Now.AddDays(10);
                service.ActiveUserId = _userContext.UserId;
                service.DataAction = DataActionEnum.Create;
                service.ParentServiceId = serviceId;
                service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

                service.Json = JsonConvert.SerializeObject(model);

                var res = await _serviceBusiness.ManageService(service);
            }

            ServiceTemplateViewModel serviceModel1 = new ServiceTemplateViewModel();
            serviceModel1.ActiveUserId = _userContext.UserId;
            serviceModel1.TemplateCode = "TAS_USER_ASSESSMENT_RESULT";
            serviceModel1.ServiceId = serviceId;
            var service1 = await _serviceBusiness.GetServiceDetails(serviceModel1);

            var query = $@"Select * from cms.""N_TAS_UserAssessmentResult"" where ""NtsNoteId""='{service1.UdfNoteId}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var assResult = await _queryAssRes.ExecuteQuerySingle(query, null);

            if (isSubmit)
            {
                service1.DataAction = DataActionEnum.Edit;
                service1.ServiceStatusCode = "SERVICE_STATUS_COMPLETE";
                service1.AllowPastStartDate = true;
                await _serviceBusiness.ManageService(service1);

                var query1 = $@"Update cms.""N_TAS_UserAssessmentResult"" set ""EndDate""='{DateTime.Now}'
                                where ""Id""='{service1.UdfNoteTableId}'";
                await _queryAssRes.ExecuteCommand(query1, null);

                await UpdateAssessment(serviceId, timeElapsed, timeElapsedSec, nextQuestionId);
            }
            else
            {
                await UpdateAssessment(serviceId, timeElapsed, timeElapsedSec, nextQuestionId);
            }

        }

        public async Task SubmitSurveyAnswer(SurveyTopicViewModel model, bool isSubmit, string nextTopicId)
        {

            model.QuestionList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SurveyQuestionViewModel>>(model.QuestionAnswerList);
            var updateQuery = "";
            foreach (var item in model.QuestionList)
            {
                if (item.AnswerItemId.IsNotNullAndNotEmpty())
                {
                    //if(currentAnswerId != multipleFieldValueId)
                    //{
                    //                    var arquery = $@"select ans.""Id"" as Id from public.""NtsService"" as s
                    //join public.""NtsNote"" as n on s.""UdfNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                    //join cms.""N_SURVEY_S_SurveyResultAnswer"" as ans on n.""Id""=ans.""NtsNoteId"" and ans.""QuestionId""='{item.QuestionId}' and ans.""IsDeleted""=false and ans.""CompanyId""='{_repo.UserContext.CompanyId}'
                    //where s.""ParentServiceId""='{model.ServiceId}' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'";

                    //                    var result = await _queryAssAns.ExecuteQuerySingle(arquery, null);
                    if (item.AnswerComment.IsNotNullAndNotEmpty())
                    {
                        item.AnswerComment = item.AnswerComment.Replace("'", "''");

                    }
                    updateQuery = $@"{updateQuery}Update cms.""N_SURVEY_S_SurveyResultAnswer"" 
                        set ""OptionId""='{item.AnswerId}',""Comment""='{item.AnswerComment}'
                        ,""LastUpdatedDate""='{DateTime.Now.ToDatabaseDateFormat()}',""LastUpdatedBy""='{_userContext.UserId}'
                        where ""Id""='{item.AnswerItemId}';";
                    //if (result != null)
                    //{

                    //    await _queryAssRes.ExecuteCommand(query1, null);

                    //    //var serdetail = await _serviceBusiness.GetSingle(x => x.UdfNoteId == result.NtsNoteId);

                    //    //ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
                    //    //serviceModel.ActiveUserId = _userContext.UserId;
                    //    //serviceModel.TemplateCode = "TAS_USER_ASSESSMENT_ANSWER";
                    //    //serviceModel.ServiceId = serdetail.Id;
                    //    //var service = await _serviceBusiness.GetServiceDetails(serviceModel);
                    //    //service.DataAction = DataActionEnum.Edit;
                    //    //service.AllowPastStartDate = true;

                    //    //result.OptionId = multipleFieldValueId;
                    //    //result.Comment = comment;

                    //    //service.Json = JsonConvert.SerializeObject(result);

                    //    //var res = await _serviceBusiness.ManageService(service);
                    //}
                    //}
                }
                else
                {
                    ServiceTemplateViewModel serviceModel1 = new ServiceTemplateViewModel();
                    serviceModel1.ServiceId = model.ServiceId;
                    serviceModel1.TemplateCode = "S_SURVEY_RESULT";
                    var service1 = await _serviceBusiness.GetServiceDetails(serviceModel1);
                    dynamic exo = new System.Dynamic.ExpandoObject();

                    ((IDictionary<String, Object>)exo).Add("SurveyId", model.SurveyId);
                    ((IDictionary<String, Object>)exo).Add("SurveyResultId", service1.UdfNoteTableId);
                    ((IDictionary<String, Object>)exo).Add("QuestionId", item.QuestionId);
                    ((IDictionary<String, Object>)exo).Add("OptionId", item.AnswerId);
                    ((IDictionary<String, Object>)exo).Add("Comment", item.AnswerComment);
                    ((IDictionary<String, Object>)exo).Add("UserId", _repo.UserContext.UserId);

                    ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
                    serviceModel.ActiveUserId = _userContext.UserId;
                    serviceModel.TemplateCode = "S_SURVEY_RESULT_ANSWER";
                    var service = await _serviceBusiness.GetServiceDetails(serviceModel);

                    //service.ServiceSubject = ;
                    service.OwnerUserId = _userContext.UserId;
                    service.StartDate = DateTime.Now;
                    service.DueDate = DateTime.Now.AddDays(10);
                    service.ActiveUserId = _userContext.UserId;
                    service.DataAction = DataActionEnum.Create;
                    service.ParentServiceId = model.ServiceId;
                    service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

                    service.Json = JsonConvert.SerializeObject(exo);

                    var res = await _serviceBusiness.ManageService(service);
                }
            }
            if (updateQuery.IsNotNullAndNotEmpty())
            {
                await _queryAssRes.ExecuteCommand(updateQuery, null);
            }
            if (isSubmit)
            {
                ServiceTemplateViewModel serviceModel1 = new ServiceTemplateViewModel();
                serviceModel1.ActiveUserId = _userContext.UserId;
                serviceModel1.TemplateCode = "S_SURVEY_RESULT";
                serviceModel1.ServiceId = model.ServiceId;
                var service1 = await _serviceBusiness.GetServiceDetails(serviceModel1);

                service1.DataAction = DataActionEnum.Edit;
                service1.ServiceStatusCode = "SERVICE_STATUS_COMPLETE";
                service1.AllowPastStartDate = true;
                await _serviceBusiness.ManageService(service1);

                var query1 = $@"Update cms.""N_SURVEY_S_SurveyResult"" set ""SurveyEndDate""='{DateTime.Now}'
                                where ""Id""='{service1.UdfNoteTableId}'";
                await _queryAssRes.ExecuteCommand(query1, null);


            }

            await UpdateSurvey(model.ServiceId, nextTopicId);


        }

        public async Task<bool> UpdateAssessment(string serviceId, int? timeElapsed, int? timeElapsedSec, string nextQuestionId = null, DateTime? startDate = null)
        {
            ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
            serviceModel.ActiveUserId = _userContext.UserId;
            serviceModel.TemplateCode = "TAS_USER_ASSESSMENT_RESULT";
            serviceModel.ServiceId = serviceId;
            var service = await _serviceBusiness.GetServiceDetails(serviceModel);

            var query1 = $@"Update cms.""N_TAS_UserAssessmentResult"" set ""TimeElapsed""='{timeElapsed}', ""TimeElapsedSec""='{timeElapsedSec}', ""CurrentQuestionId""='{nextQuestionId}' #SETDATE#
                           where ""Id""='{service.UdfNoteTableId}'";

            var setdate = "";
            if (startDate.IsNotNull())
            {
                setdate = $@", ""StartDate""='{startDate}' ";
            }
            query1 = query1.Replace("#SETDATE#", setdate);
            await _queryAssRes.ExecuteCommand(query1, null);

            var query = $@"Select ""IsAssessmentStopped"" from public.""NtsService"" as s
                        Join public.""NtsNote"" as n on s.""UdfNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_TAS_UserAssessmentResult"" as ar on n.""Id""=ar.""NtsNoteId"" and ar.""IsDeleted""=false and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
                        where s.""Id""='{serviceId}' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var result = await _queryAssDetail.ExecuteScalar<bool>(query, null);
            return result;

        }
        public async Task<bool> UpdateSurvey(string serviceId, string nextQTopicId = null, DateTime? startDate = null)
        {
            ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
            serviceModel.ActiveUserId = _userContext.UserId;
            serviceModel.TemplateCode = "S_SURVEY_RESULT";
            serviceModel.ServiceId = serviceId;
            var service = await _serviceBusiness.GetServiceDetails(serviceModel);

            var query1 = $@"Update cms.""N_SURVEY_S_SurveyResult"" set ""CurrentTopicId""='{nextQTopicId}' 
                           where ""Id""='{service.UdfNoteTableId}'";

            //var setdate = "";
            //if (startDate.IsNotNull())
            //{
            //    setdate = $@", ""StartDate""='{startDate}' ";
            //}
            //query1 = query1.Replace("#SETDATE#", setdate);
            await _queryAssRes.ExecuteCommand(query1, null);

            //var query = $@"Select ""IsAssessmentStopped"" from public.""NtsService"" as s
            //            Join public.""NtsNote"" as n on s.""UdfNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
            //            join cms.""N_TAS_UserAssessmentResult"" as ar on n.""Id""=ar.""NtsNoteId"" and ar.""IsDeleted""=false and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
            //            where s.""Id""='{serviceId}' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'";
            //var result = await _queryAssDetail.ExecuteScalar<bool>(query, null);
            return true;

        }
        public async Task ManageAssessmentStatus(string serviceId, bool isAssessmentStopped)
        {
            var query = "";
            var sermodel = await _serviceBusiness.GetSingle(x => x.ParentServiceId == serviceId);
            if (!isAssessmentStopped)
            {
                query = $@"Update cms.""N_TAS_UserAssessmentResult"" set ""IsAssessmentStopped""='{isAssessmentStopped.ToString()}'
                                where ""Id""='{sermodel.UdfNoteTableId}' ";

                await _queryAssRes.ExecuteCommand(query, null);
            }
            else
            {
                query = $@"Update cms.""N_TAS_UserAssessmentResult"" set ""IsAssessmentStopped""='{isAssessmentStopped.ToString()}'
                                where ""Id""='{sermodel.UdfNoteTableId}' ";

                await _queryAssRes.ExecuteCommand(query, null);
            }
        }

        public async Task<List<AssessmentInterviewViewModel>> GetAssessmentInterview(string UserID, bool isarchived = false, string source = null)
        {
            var Query = "";
            if (_userContext.IsSystemAdmin || source == "VIEWALL")
            {
                Query = $@"select  u.""Id"" as Id,sr.""Id"" as ServiceId, sr.""ServiceSubject"" as Subject,sr.""IsArchived"" as IsRowArchived, u.""Name""
 as OwnerUserName, L.""Name"" as AssessmentStatus,u.""Id"" as OwnerUserId,sc.""AttachmentId"" as ""CandidateCVId"",
'AssessmentInterview' as AssessmentType,false as IsAssessmentStopped,i.""Id"" as ""TableId"",u.""JobTitle"" as JobTitle,
sc.""AssessmentStartDate"" as ScheduledStartDate,sc.""Id"" as ""InterviewScheduleId"" ,u.""Email"" as Email,t.""Name"" as PanelTeamName,sc.""Url"" as AssessmentInterviewUrl,c.""InterviewSheetId""
from public.""User"" as u
inner join cms.""N_TAS_UserAssessmentSchedule"" as sc on sc.""UserId""=u.""Id"" and sc.""IsDeleted""=false and sc.""CompanyId""='{_repo.UserContext.CompanyId}'
--left join public.""TeamUser""  as tm on  u.""Id""=tm.""UserId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""Team""  as t on  t.""Id""=sc.""InterviewerId""  and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsService"" as sr on u.""Id"" =sr.""OwnerUserId"" and sr.""IsDeleted""=false and sr.""CompanyId""='{_repo.UserContext.CompanyId}' and sr.""TemplateId"" =(select ""Id"" from public.""Template""  where ""Code""='TAS_Interview')
left join public.""NtsNote"" as n on n.""Id""=sr.""UdfNoteId"" and  n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_AssessmentInterview"" as i on i.""NtsNoteId""=n.""Id"" and i.""IsDeleted""=false and i.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as L on L.""Id""=sr.""ServiceStatusId"" and L.""IsDeleted""=false and L.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_CandidateId"" as c on u.""Id""=c.""UserId"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
where  u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and sc.""SlotType""='1'
";
            }
            else if (_userContext.UserRoleCodes.Contains("INTERVIEWER"))
            {
                Query = $@"select distinct u.""Id"" as Id,sr.""Id"" as ServiceId, sr.""ServiceSubject"" as Subject,sr.""IsArchived"" as IsRowArchived, u.""Name""
 as OwnerUserName, L.""Name"" as AssessmentStatus,u.""Id"" as OwnerUserId,sc.""AttachmentId"" as ""CandidateCVId"",
'AssessmentInterview' as AssessmentType,false as IsAssessmentStopped,i.""Id"" as ""TableId"",u.""JobTitle"" as JobTitle,
sc.""AssessmentStartDate"" as ScheduledStartDate,sc.""Id"" as ""InterviewScheduleId"" ,u.""Email"" as Email,t.""Name"" as PanelTeamName,sc.""Url"" as AssessmentInterviewUrl,c.""InterviewSheetId""
from public.""User"" as u
inner join cms.""N_TAS_UserAssessmentSchedule"" as sc on sc.""UserId""=u.""Id"" and sc.""IsDeleted""=false and sc.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join public.""Team""  as t on  t.""Id""=sc.""InterviewerId""  and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join public.""TeamUser""  as tm on t.""Id""=tm.""TeamId"" and tm.""UserId""='{_userContext.UserId}' and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsService"" as sr on u.""Id"" =sr.""OwnerUserId"" and sr.""IsDeleted""=false and sr.""CompanyId""='{_repo.UserContext.CompanyId}' and sr.""TemplateId"" =(select ""Id"" from public.""Template""  where ""Code""='TAS_Interview')
left join public.""NtsNote"" as n on n.""Id""=sr.""UdfNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_AssessmentInterview"" as i on i.""NtsNoteId""=n.""Id"" and i.""IsDeleted""=false and i.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as L on L.""Id""=sr.""ServiceStatusId""
left join cms.""N_TAS_CandidateId"" as c on u.""Id""=c.""UserId"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and sc.""SlotType""='1'";

            }
            else { Query = $@"select distinct u.""Id"" as Id,sr.""Id"" as ServiceId, sr.""ServiceSubject"" as Subject,sr.""IsArchived"" as IsRowArchived, u.""Name""
 as OwnerUserName, L.""Name"" as AssessmentStatus,u.""Id"" as OwnerUserId,sc.""AttachmentId"" as ""CandidateCVId"",
'AssessmentInterview' as AssessmentType,false as IsAssessmentStopped,i.""Id"" as ""TableId"",u.""JobTitle"" as JobTitle,
sc.""AssessmentStartDate"" as ScheduledStartDate,sc.""Id"" as ""InterviewScheduleId"" ,u.""Email"" as Email,t.""Name"" as PanelTeamName,sc.""Url"" as AssessmentInterviewUrl,c.""InterviewSheetId""
from public.""User"" as u
inner join cms.""N_TAS_UserAssessmentSchedule"" as sc on sc.""UserId""=u.""Id"" and sc.""IsDeleted""=false and sc.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join public.""TeamUser""  as tm on  u.""Id""=tm.""UserId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join public.""Team""  as t on  t.""Id""=sc.""InterviewerId""  and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsService"" as sr on u.""Id"" =sr.""OwnerUserId"" and sr.""IsDeleted""=false and sr.""CompanyId""='{_repo.UserContext.CompanyId}' and sr.""TemplateId"" =(select ""Id"" from public.""Template""  where ""Code""='TAS_Interview')
left join public.""NtsNote"" as n on n.""Id""=sr.""UdfNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_AssessmentInterview"" as i on i.""NtsNoteId""=n.""Id"" and i.""IsDeleted""=false and i.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as L on L.""Id""=sr.""ServiceStatusId""
left join cms.""N_TAS_CandidateId"" as c on u.""Id""=c.""UserId"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""Id""='{_userContext.UserId}' and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and sc.""SlotType""='1'"; }

            var result = await _queryAssInterView.ExecuteQueryList<AssessmentInterviewViewModel>(Query, null);
            return result;


        }

        public async Task UpdateCandidateId(string userId, string fileId, string type)
        {
            string query;
            if (type == "InterviewSheet")
            {
                query = $@"Update cms.""N_TAS_CandidateId"" set ""InterviewSheetId""='{fileId}' where ""UserId""='{userId}' ";
            }
            else
            {
                query = $@"Update cms.""N_TAS_CandidateId"" set ""CandidateId""='{fileId}' where ""UserId""='{userId}' ";
            }
            await _queryAssInterView.ExecuteCommand(query, null);
        }

        public async Task<AssessmentInterviewViewModel> GetInterviewService(string UserID)
        {

            var Query = $@"select  u.""Id"" as Id,sr.""Id"" as ServiceId, sr.""ServiceSubject"" as Subject,'' as IsArchived, u.""Name""
as OwnerUserName, L.""Name"" as AssessmentStatus,u.""Id"" as OwnerUserId,
'AssessmentInterview' as AssessmentType,false as IsAssessmentStopped,
case when uas.""AssessmentStartDate"" is not null  then uas.""AssessmentStartDate"" else i.""ScheduledStartDate"" end as ScheduledStartDate,
case when uas.""EndDate"" is not null  then uas.""EndDate"" else i.""ScheduledEndDate"" end as ScheduledEndDate,
--i.""ScheduledStartDate"" as ScheduledStartDate,
i.""ActualStartDate"" as ActualStartDate ,i.""ActualEndDate"" as ActualEndDate ,
u.""Email"" as Email,t.""Name"" as PanelTeamName,uas.""Url"" as AssessmentInterviewUrl
,u.""JobTitle"" as JobTitle
from public.""User"" as u
left join public.""TeamUser""  as tm on  u.""Id""=tm.""UserId"" and tm.""IsDeleted""=false  and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""Team""  as t on  t.""Id""=tm.""TeamId""  and t.""IsDeleted""=false  and t.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsService"" as sr on u.""Id"" =sr.""OwnerUserId"" and sr.""IsDeleted""=false  and sr.""CompanyId""='{_repo.UserContext.CompanyId}' and sr.""TemplateId"" =(select ""Id"" from public.""Template""  where ""Code""='TAS_Interview')
left join public.""NtsNote"" as n on n.""Id""=sr.""UdfNoteId"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_AssessmentInterview"" as i on i.""NtsNoteId""=n.""Id"" and i.""IsDeleted""=false  and i.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as L on L.""Id""=sr.""ServiceStatusId"" 
left join cms.""N_TAS_UserAssessmentSchedule"" as uas on uas.""Id""=i.""InterviewScheduleId"" and uas.""UserId""=u.""Id"" and uas.""IsDeleted""=false and uas.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""Id""='{UserID}' and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
";

            var result = await _queryAssInterView.ExecuteQuerySingle<AssessmentInterviewViewModel>(Query, null);
            return result;
        }

        public async Task<List<IdNameViewModel>> GetInterviewAssessorList(string UserId)
        {
            var query = $@"Select ituser.""Id"",ituser.""Name"" from 
 cms.""N_TAS_UserAssessmentSchedule"" as uas  
left join public.""Team"" as it on uas.""InterviewerId""=it.""Id"" and it.""IsDeleted""=false
left join public.""TeamUser"" as itu on it.""Id"" = itu.""TeamId"" and itu.""IsDeleted""=false
left join public.""User"" as ituser on itu.""UserId""=ituser.""Id"" and ituser.""IsDeleted""=false 
    where uas.""UserId""='{UserId}' and uas.""SlotType"" = '1' and uas.""IsDeleted""=false ";

            var result = await _idNameRepo.ExecuteQueryList(query, null);
            return result;
        }


        public async Task<MemoryStream> GetInterViewQuestionExcel(string UserId, string TableId)



        {
            var model = await GetQuestionListofUser(UserId, TableId);
            int row = 1;
            var ms = new MemoryStream();
            var reportList = new List<string>() { "Sheet1" };
            var modelData = new AssessmentViewModel();
            using (var sl = new SLDocument())
            {
                foreach (var rep in reportList)
                {
                    sl.AddWorksheet(rep);

                    SLPageSettings pageSettings = new SLPageSettings();
                    pageSettings.ShowGridLines = true;
                    sl.SetPageSettings(pageSettings);

                    sl.SetColumnWidth("A", 20);
                    sl.SetColumnWidth("B", 20);
                    sl.SetColumnWidth("C", 20);
                    sl.SetColumnWidth("D", 20);
                    sl.SetColumnWidth("E", 20);
                    sl.SetColumnWidth("F", 20);
                    sl.SetColumnWidth("G", 20);
                    sl.SetColumnWidth("G", 20);
                    sl.SetColumnWidth("H", 20);
                    sl.SetColumnWidth("I", 20);
                    sl.SetColumnWidth("J", 20);
                    sl.SetColumnWidth("K", 20);
                    sl.SetColumnWidth("L", 20);
                    sl.SetColumnWidth("M", 20);
                    sl.SetColumnWidth("N", 20);


                    sl.SetCellValue("A1", "NoteId");
                    sl.SetCellValue("B1", "SeraillNo");
                    sl.SetCellValue("C1", "Subject");
                    sl.SetCellValue("D1", "Description");
                    sl.SetCellValue("E1", "Level");
                    sl.SetCellValue("F1", "Indicatior");
                    sl.SetCellValue("G1", "Remarks");
                    sl.SetCellValue("H1", "Question");
                    sl.SetCellValue("I1", "CandidateAnswer");
                    sl.SetCellValue("J1", "Score");
                    sl.SetCellValue("K1", "InterviewerComment");

                    if (rep == "Sheet1")







                        row++;


                    if (model.IsNotNull())
                    {

                        foreach (var item in model)

                        {



                            //sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                            sl.SetCellValue(string.Concat("A", row), item.NoteId);
                            sl.SetCellValue(string.Concat("B", row), item.SlNo);
                            sl.SetCellValue(string.Concat("C", row), item.NoteSubject);
                            sl.SetCellValue(string.Concat("D", row), item.NoteDescription);
                            sl.SetCellValue(string.Concat("E", row), item.ProficiencyLevel);
                            sl.SetCellValue(string.Concat("F", row), item.Indicator);
                            sl.SetCellValue(string.Concat("G", row), item.Remark);
                            sl.SetCellValue(string.Concat("H", row), item.Question);
                            sl.SetCellValue(string.Concat("I", row), item.CandidateAnswer);
                            sl.SetCellValue(string.Concat("J", row), item.Score);
                            sl.SetCellValue(string.Concat("K", row), item.InterviewerComment);






                            row++;
                        }
                    }










                }

                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);


            }


            ms.Position = 0;
            return ms;
        }



        public async Task<List<InterViewQuestionViewModel>> GetRandamQuestions()
        {

            var Query = $@"select n.""NoteSubject"",q.""Question"",n.""NoteDescription"" from cms.""N_TAS_Question"" q inner join public.""NtsNote"" n on q.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
             where  q.""IsDeleted""=false  and q.""CompanyId""='{_repo.UserContext.CompanyId}' order by random()  limit 3";

            var result = await _QueryInterviewquestion.ExecuteQueryList<InterViewQuestionViewModel>(Query, null);
            return result;


        }


        public async Task<List<InterViewQuestionViewModel>> GetQuestionDetails(string UserId, string TableId)
        {

            var Query = $@"select n.""NoteSubject"",q.""Question"",n.""NoteDescription"" 
    from cms.""N_TAS_Question"" q inner join public.""NtsNote"" n on q.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
where  q.""IsDeleted""=false  and q.""CompanyId""='{_repo.UserContext.CompanyId}'
   order by random()  limit 3";

            var result = await _QueryInterviewquestion.ExecuteQueryList<InterViewQuestionViewModel>(Query, null);
            return result;


        }


        public async Task<List<InterViewQuestionViewModel>> GetQuestionListofUser(string UserId, string TableId)
        {

            var Query = $@"select n.""Id"" as NoteId,  n.""NoteSubject"",n.""NoteDescription"", q.* 
from cms.""N_TAS_InterviewQuestion"" q inner join public.""NtsNote"" n on q.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
where q.""UserId"" = '{UserId}' and n.""ParentServiceId"" = '{TableId}' and q.""IsDeleted""=false  and q.""CompanyId""='{_repo.UserContext.CompanyId}'
order by q.""SequenceOrder"" ";

            var result = await _QueryInterviewquestion.ExecuteQueryList<InterViewQuestionViewModel>(Query, null);
            return result;


        }

        public async Task<AssessmentReportViewModel> GetCandidateCaseStudyReport(string userId)
        {
            var model = new AssessmentReportViewModel();
            //var userModel =
            //if (userModel != null)
            //{
            //    model.OwnerUserName = userModel.UserName;
            //    model.Email = userModel.Email;
            //}

            //model.TechnicalAssessment = GetAssessmentReport(userId, "S_ASSESSMENT_QUISTIONNAIRE_RESULT");
            model.CaseStudyAssessment = await GetAssessmentReport(userId);

            if (model.CaseStudyAssessment != null && model.CaseStudyAssessment.AnswerList != null && model.CaseStudyAssessment.AnswerList.Count > 0)
            {
                model.CaseStudyText = model.CaseStudyAssessment.AnswerList.FirstOrDefault()?.Description;

            }
            return model;
        }
        public async Task<AssessmentDetailViewModel> GetAssessmentReport(string UserID)
        {
            var query = $@"select Ass.""Id"", Ass.""AssessmentName"" as Title, S.""AssessmentStartDate"" as ScheduledStartDate,S.""EndDate"" as ScheduledEndDate,R.""StartDate"" as ActualStartDate,R.""EndDate"" as ActualEndDate,lovl.""Code""  as PreferredLanguageCode 
from public.""NtsService"" as sr inner join public.""NtsNote"" N on sr.""UdfNoteId""=N.""Id""  and N.""IsDeleted""=false  and N.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join cms.""N_TAS_UserAssessmentSchedule"" S on N.""Id"" = S.""NtsNoteId"" and S.""IsDeleted"" = false and  S.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_UserAssessmentResult"" R on S.""AssessmentId"" = R.""AssessmentId"" and R.""IsDeleted"" = false  and R.""UserId"" = '{UserID}'  and R.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_Assessment"" as Ass on Ass.""Id"" = S.""AssessmentId"" and Ass.""IsDeleted"" = false and Ass.""CompanyId""='{_repo.UserContext.CompanyId}'
left join Public.""LOV"" as lov  on lov.""Id"" = Ass.""AssessmentTypeId"" and lov.""IsDeleted"" = false   and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
left join Public.""LOV"" as lovl  on lovl.""Id"" =S.""PreferedLanguageId"" and lovl.""IsDeleted"" = false   and lovl.""CompanyId""='{_repo.UserContext.CompanyId}'

where lov.""Code"" = 'CASE_STUDY' and S.""UserId"" = '{UserID}' and sr.""IsDeleted""=false  and sr.""CompanyId""='{_repo.UserContext.CompanyId}'";


            var result = await _queryAssDetail.ExecuteQuerySingle<AssessmentDetailViewModel>(query, null);

            if (result.IsNotNull())
            {
                result.AnswerList = await GetQuestionOptionList(result.Id, UserID);
            }
            return result;



        }
        public async Task<List<AssessmentDetailViewModel>> GetAssessmentReporTechnical(string UserID)
        {
            var query = $@"select Ass.""AssessmentName"" as Title , Ass.""Id"", S.""AssessmentStartDate"" as ScheduledStartDate,S.""EndDate"" as ScheduledEndDate,R.""StartDate"" as ActualStartDate,R.""EndDate"" as ActualEndDate,lovl.""Code"" as PreferredLanguageCode 
from public.""NtsService"" as sr inner join public.""NtsNote"" N on sr.""UdfNoteId""=N.""Id"" and N.""IsDeleted""=false  and N.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join cms.""N_TAS_UserAssessmentSchedule"" S on N.""Id"" = S.""NtsNoteId"" and S.""IsDeleted"" = false   and S.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_UserAssessmentResult"" R on S.""AssessmentId"" = R.""AssessmentId"" and R.""IsDeleted"" = false  and R.""UserId"" = '{UserID}' and R.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_Assessment"" as Ass on Ass.""Id"" = S.""AssessmentId"" and Ass.""IsDeleted"" = false   and Ass.""CompanyId""='{_repo.UserContext.CompanyId}'
left join Public.""LOV"" as lov  on lov.""Id"" = Ass.""AssessmentTypeId"" and lov.""IsDeleted"" = false   and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
left join Public.""LOV"" as lovl  on lovl.""Id"" =S.""PreferedLanguageId"" and lovl.""IsDeleted"" = false   and lovl.""CompanyId""='{_repo.UserContext.CompanyId}'
where lov.""Code"" = 'TECHNICAL' and S.""UserId"" = '{UserID}' and sr.""IsDeleted""=false  and sr.""CompanyId""='{_repo.UserContext.CompanyId}'";


            var result = await _queryAssDetail.ExecuteQueryList<AssessmentDetailViewModel>(query, null);

            if (result.IsNotNull())
            {
                foreach (var item in result)
                {
                    item.AnswerList = await GetQuestionOptionList(item.Id, UserID);

                }


            }
            return result;



        }




        public async Task<List<AssessmentAnswerViewModel>> GetQuestionOptionList(string AssesmentId, string UserId)
        {

            var query = $@"select an.""NtsNoteId"" as NtsNoteId, Q.""Question"",Q.""QuestionArabic"" as QuestionAr, Q.""NtsNoteId"" as QuestionId,Aq.""Id"",o.""Option"" as CandidateAnswer,
o.""OptionArabic"" as CandidateAnswerAr,an.""Comment"" as CandidateAnswerComment,an.""Comment"" as CandidateAnswerCommentAr, N.""NoteDescription"" as Description,an.""Score"",an.""CaseStudyComment"" as InterviewerComment
,Q.""QuestionAttachmentId"" as FileId,Q.""QuestionArabicAttachmentId"" as FileIdAr
from cms.""N_TAS_AssessmentQuestion"" as Aq inner join
cms.""N_TAS_Question"" Q on Q.""Id"" = Aq.""QuestionId"" and Q.""IsDeleted""=false  and Q.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join public.""NtsNote"" as N on N.""Id""=Q.""NtsNoteId"" and N.""IsDeleted""=false  and N.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_UserAssessmentAnswer"" as  an on an.""AssessmentId""=Aq.""AssessmentId"" and Q.""Id""=an.""QuestionId"" and an.""UserId""='{UserId}' and an.""IsDeleted""=false  and an.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_QuestionOption"" as o on o.""Id""=an.""OptionId"" and o.""IsDeleted""=false  and o.""CompanyId""='{_repo.UserContext.CompanyId}'
where Aq.""AssessmentId""='{AssesmentId}' and Aq.""IsDeleted""=false  and Aq.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var res = await _queryAssAns.ExecuteQueryList<AssessmentAnswerViewModel>(query, null);


            Int32 row = 0, MaxNo = 0;
            foreach (var item in res)
            {

                row++;
                item.SeraillNo = row;
                var options = await GetOptionList(item.QuestionId);

                if (MaxNo < options.Count)
                {
                    MaxNo = Convert.ToInt32(options.Count);
                    item.MaxoptionNo = MaxNo;
                }
                if (options.IsNotNull())
                {
                    foreach (var optionitem in options)
                    {
                        if (optionitem.IsRightAnswer == true)
                        {
                            item.RightAnswer = optionitem.Option;
                            item.RightAnswerAr = optionitem.OptionArabic;

                        }


                        // switch (optionitem.SequenceOrder)
                        // {
                        //     case 1:
                        //         item.Option1 = optionitem.Option;
                        //         item.Option1Ar = optionitem.OptionArabic;
                        //         item.ScoreOption1 = Convert.ToString(optionitem.Score);
                        //         if (MaxNo < 1)
                        //         {
                        //             MaxNo = 1;
                        //         }
                        //         
                        //         break;
                        //     case 2:
                        //         item.Option2 = optionitem.Option;
                        //         item.Option2Ar = optionitem.OptionArabic;
                        //         item.ScoreOption2 = Convert.ToString(optionitem.Score);
                        //         if (MaxNo < 1)
                        //         {
                        //             MaxNo = 1;
                        //         }
                        //         break;
                        //     case 3:
                        //         item.Option3 = optionitem.Option;
                        //         item.Option3Ar = optionitem.OptionArabic;
                        //         item.ScoreOption3 = Convert.ToString(optionitem.Score);
                        //         break;
                        //     case 4:
                        //         item.Option4 = optionitem.Option;
                        //         item.Option4Ar = optionitem.OptionArabic;
                        //         item.ScoreOption4 = Convert.ToString(optionitem.Score);
                        //         break;
                        //     case 5:
                        //         item.Option5 = optionitem.Option;
                        //         item.ScoreOption5 = Convert.ToString(optionitem.Score);
                        //         break;
                        //
                        //     case 6:
                        //         item.Option6 = optionitem.Option;
                        //         item.ScoreOption6 = Convert.ToString(optionitem.Score);
                        //         break;
                        //     case 7:
                        //         item.Option7= optionitem.Option;
                        //         item.ScoreOption7 = Convert.ToString(optionitem.Score);
                        //         break;
                        //     case 8:
                        //         item.Option8 = optionitem.Option;
                        //         item.ScoreOption8 = Convert.ToString(optionitem.Score);
                        //         break;
                        //     case 9:
                        //         item.Option9 = optionitem.Option;
                        //         item.ScoreOption9 = Convert.ToString(optionitem.Score);
                        //         break;
                        //     case 10:
                        //         item.Option10 = optionitem.Option;
                        //         item.ScoreOption10 = Convert.ToString(optionitem.Score);
                        //         break;
                        //     default:
                        //         break;
                        // }




                    }
                    if (item.RightAnswer.IsNotNull() && item.CandidateAnswer.IsNotNull())
                    {
                        if (item.RightAnswer == item.CandidateAnswer)
                        {
                            item.Accurate = BoolStatus.Yes;
                        }
                        else { item.Accurate = BoolStatus.No; }
                    }
                }

            }
            return res.OrderBy(x => x.SeraillNo).ToList();
        }



        public async Task<List<AssessmentQuestionsOptionViewModel>> GetOptionList(string QuestionId)
        {

            var query = $@"select o.""SequenceOrder"",o.""Id"" as OptionId,o.""Option"",o.""OptionArabic"",o.""IsRightAnswer"",o.""Score"" 
from cms.""N_TAS_QuestionOption"" o
inner join public.""NtsNote"" N on N.""Id""=o.""NtsNoteId""  and N.""IsDeleted""=false  and N.""CompanyId""='{_repo.UserContext.CompanyId}'
where ""ParentNoteId""='{QuestionId}' and o.""IsDeleted""=false  and o.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var res = await _queryRepo1.ExecuteQueryList<AssessmentQuestionsOptionViewModel>(query, null);
            return res;


        }



        public async Task<MemoryStream> GetInterviewerCaseStudyDataExcel(AssessmentReportViewModel model)
        {
            var ms = new MemoryStream();
            var reportList = new List<string>() { "Case Study Report" };
            var modelData = new AssessmentDetailViewModel();
            using (var sl = new SLDocument())
            {
                foreach (var rep in reportList)
                {
                    sl.AddWorksheet(rep);

                    SLPageSettings pageSettings = new SLPageSettings();
                    pageSettings.ShowGridLines = true;
                    sl.SetPageSettings(pageSettings);

                    sl.SetColumnWidth("A", 20);
                    sl.SetColumnWidth("B", 20);
                    sl.SetColumnWidth("C", 20);
                    sl.SetColumnWidth("D", 20);
                    sl.SetColumnWidth("E", 20);
                    sl.SetColumnWidth("F", 20);
                    sl.SetColumnWidth("G", 20);
                    sl.SetColumnWidth("H", 20);
                    sl.SetColumnWidth("I", 20);
                    sl.SetColumnWidth("J", 20);
                    sl.SetColumnWidth("K", 20);
                    sl.SetColumnWidth("L", 20);
                    sl.SetColumnWidth("M", 20);
                    sl.SetColumnWidth("N", 20);
                    sl.SetColumnWidth("O", 20);
                    sl.SetColumnWidth("P", 20);
                    sl.SetColumnWidth("Q", 20);
                    sl.SetColumnWidth("R", 20);
                    sl.SetColumnWidth("S", 20);

                    // sl.HideColumn("S");

                    sl.MergeWorksheetCells("A1", "B1");
                    sl.SetCellValue("A1", "");
                    sl.SetCellStyle("A1", "B1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                    sl.MergeWorksheetCells("I1", "J1");
                    sl.SetCellValue("I1", DateTime.Today.ToDefaultDateFormat());
                    sl.SetCellStyle("I1", "J1", ExcelHelper.GetHeaderRowDateStyle(sl));

                    sl.MergeWorksheetCells("B2", "I3");
                    sl.SetCellValue("B2", rep + " - Interviewer");
                    sl.SetCellStyle("B2", "I3", ExcelHelper.GetReportHeadingStyle(sl));

                    //sl.MergeWorksheetCells("B4", "K4");
                    //sl.SetCellStyle("B4", "K4", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));
                    //sl.SetCellStyle("B4", "K4", ExcelHelper.GetNoBorderCentreAlignStyle(sl)); 
                    if (rep != "Case Study Data")
                    {
                        if (rep == "Case Study Report")
                            modelData = model.CaseStudyAssessment;

                        if (modelData.IsNotNull())
                        {
                            sl.SetCellStyle("A5", "R5", ExcelHelper.GetTopBorderStyle(sl));
                            sl.SetCellStyle("A5", "A7", ExcelHelper.GetLeftBorderStyle(sl));
                            sl.SetCellStyle("R5", "R7", ExcelHelper.GetRightBorderStyle(sl));
                            sl.SetCellStyle("A7", "R7", ExcelHelper.GetBottomBorderOnlyStyle(sl));
                            sl.SetCellStyle("A5", "R7", ExcelHelper.FillColorStyle(sl));

                            sl.MergeWorksheetCells("A5", "B5");
                            sl.SetCellValue("A5", "Name");
                            sl.SetCellStyle("A5", "B5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("A6", "B7");
                            sl.SetCellValue("A6", model.OwnerUserName);
                            sl.SetCellStyle("A6", "B7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("C5", "C5");
                            sl.SetCellValue("C5", "Job Title");
                            sl.SetCellStyle("C5", "C5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("C6", "C7");
                            sl.SetCellValue("C6", model.JobTitle);
                            sl.SetCellStyle("C6", "C7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("D5", "E5");
                            sl.SetCellValue("D5", "Email ID");
                            sl.SetCellStyle("D5", "E5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("D6", "E7");
                            sl.SetCellValue("D6", model.Email);
                            sl.SetCellStyle("D6", "E7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("F5", "F5");
                            sl.SetCellValue("F5", "Assessment Name");
                            sl.SetCellStyle("F5", "F5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("F6", "F7");
                            sl.SetCellValue("F6", modelData.Title);
                            sl.SetCellStyle("F6", "F7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("G5", "G5");
                            sl.SetCellValue("G5", "Scheduled Start Date");
                            sl.SetCellStyle("G5", "G5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("G6", "G7");
                            sl.SetCellValue("G6", modelData.ScheduledStartDate.IsNotNull() ? modelData.ScheduledStartDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("G6", "G7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("H5", "H5");
                            sl.SetCellValue("H5", "Scheduled End Date");
                            sl.SetCellStyle("H5", "H5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("H6", "H7");
                            sl.SetCellValue("H6", modelData.ScheduledEndDate.IsNotNull() ? modelData.ScheduledEndDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("H6", "H7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("I5", "I5");
                            sl.SetCellValue("I5", "Actual Start Date");
                            sl.SetCellStyle("I5", "I5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("I6", "I7");
                            sl.SetCellValue("I6", modelData.ActualStartDate.IsNotNull() ? modelData.ActualStartDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("I6", "I7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("J5", "J5");
                            sl.SetCellValue("J5", "Actual End Date");
                            sl.SetCellStyle("J5", "J5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("J6", "J7");
                            sl.SetCellValue("J6", modelData.ActualEndDate.IsNotNull() ? modelData.ActualEndDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("J6", "J7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            int row = 9;


                            sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                            sl.SetCellValue(string.Concat("A", row), "NtsNoteId");
                            sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                            sl.SetCellValue(string.Concat("B", row), "Sl No");
                            sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                            sl.SetCellValue(string.Concat("C", row), "Question");
                            sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetShiftEntryDateStyle(sl));



                            Int32 maxNo = modelData.AnswerList.Count > 0 ? modelData.AnswerList.Max(x => x.MaxoptionNo) : 0;
                            char first = 'C';

                            for (int i = 1; i <= maxNo; i++)
                            {
                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), "Option" + i);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            }

                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Right Answer");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Candidate Answer");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Accurate");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Answer Comment");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));



                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Score");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));



                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Interviewer Comment");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));



                            for (int i = 1; i <= maxNo; i++)
                            {
                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), "Score Option" + i);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            }


                            row++;
                            if (modelData.AnswerList != null)
                            {
                                foreach (var ans in modelData.AnswerList)
                                {
                                    sl.SetRowHeight(row, 30);
                                    sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                                    sl.SetCellValue(string.Concat("A", row), ans.NtsNoteId);
                                    sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                    sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                                    sl.SetCellValue(string.Concat("B", row), ans.SeraillNo.IsNotNull() ? ans.SeraillNo.ToString() : "");
                                    sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                                    sl.SetCellValue(string.Concat("C", row), model.CaseStudyAssessment.PreferredLanguageCode == "ARABIC" ? ans.QuestionAr : ans.Question);
                                    sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                    var options = await GetOptionList(ans.QuestionId);
                                    options = options.OrderBy(x => x.SequenceOrder).ToList();
                                    first = 'C';
                                    foreach (var item in options)
                                    {
                                        first = (char)((int)first + 1);
                                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                        sl.SetCellValue(string.Concat(first, row), model.CaseStudyAssessment.PreferredLanguageCode == "ARABIC" ? item.OptionArabic : item.Option);
                                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    }
                                    if (maxNo > options.Count)
                                    {
                                        Int32 k = maxNo - options.Count;

                                        for (int i = 0; i < k; i++)
                                        {
                                            first = (char)((int)first + 1);
                                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                            sl.SetCellValue(string.Concat(first, row), "");
                                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                        }
                                    }



                                    first = (char)((int)first + 1);

                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), model.CaseStudyAssessment.PreferredLanguageCode == "ARABIC" ? ans.RightAnswerAr : ans.RightAnswer);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                    first = (char)((int)first + 1);

                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), model.CaseStudyAssessment.PreferredLanguageCode == "ARABIC" ? ans.CandidateAnswerAr : ans.CandidateAnswer);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), ans.Accurate.ToString());
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), model.CaseStudyAssessment.PreferredLanguageCode == "ARABIC" ? ans.CandidateAnswerCommentAr : ans.CandidateAnswerComment);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), ans.Score.IsNotNull() ? ans.Score.ToString() : "");
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), ans.InterviewerComment);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), "");
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));






                                    if (ans.AnswerId.HasValue)
                                    {
                                        first = (char)((int)first + 1);
                                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                        sl.SetCellValue(string.Concat(first, row), ans.AnswerId.Value);
                                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    }


                                    foreach (var item in options)
                                    {
                                        first = (char)((int)first + 1);
                                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                        sl.SetCellValue(string.Concat(first, row), item.Score);
                                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    }


                                    if (maxNo > options.Count)
                                    {
                                        Int32 k = maxNo - options.Count;

                                        for (int i = 0; i < k; i++)
                                        {
                                            first = (char)((int)first + 1);
                                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                            sl.SetCellValue(string.Concat(first, row), "");
                                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                        }
                                    }


                                    //    sl.MergeWorksheetCells(string.Concat("T", row), string.Concat("T", row));
                                    //    sl.SetCellValue(string.Concat("T", row), ans.ScoreOption1);
                                    //    sl.SetCellStyle(string.Concat("T", row), string.Concat("T", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //    sl.MergeWorksheetCells(string.Concat("U", row), string.Concat("U", row));
                                    //    sl.SetCellValue(string.Concat("U", row), ans.ScoreOption2);
                                    //    sl.SetCellStyle(string.Concat("U", row), string.Concat("U", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //    sl.MergeWorksheetCells(string.Concat("V", row), string.Concat("V", row));
                                    //    sl.SetCellValue(string.Concat("V", row), ans.ScoreOption3);
                                    //    sl.SetCellStyle(string.Concat("V", row), string.Concat("V", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //    sl.MergeWorksheetCells(string.Concat("W", row), string.Concat("W", row));
                                    //    sl.SetCellValue(string.Concat("W", row), ans.ScoreOption4);
                                    //    sl.SetCellStyle(string.Concat("W", row), string.Concat("W", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //    sl.MergeWorksheetCells(string.Concat("X", row), string.Concat("X", row));
                                    //    sl.SetCellValue(string.Concat("X", row), ans.ScoreOption5);
                                    //    sl.SetCellStyle(string.Concat("X", row), string.Concat("X", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //    sl.MergeWorksheetCells(string.Concat("Y", row), string.Concat("Y", row));
                                    //    sl.SetCellValue(string.Concat("Y", row), ans.ScoreOption6);
                                    //    sl.SetCellStyle(string.Concat("Y", row), string.Concat("Y", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //    sl.MergeWorksheetCells(string.Concat("Z", row), string.Concat("Z", row));
                                    //    sl.SetCellValue(string.Concat("Z", row), ans.ScoreOption7);
                                    //    sl.SetCellStyle(string.Concat("Z", row), string.Concat("Z", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //    sl.MergeWorksheetCells(string.Concat("AA", row), string.Concat("AA", row));
                                    //    sl.SetCellValue(string.Concat("AA", row), ans.ScoreOption8);
                                    //    sl.SetCellStyle(string.Concat("AA", row), string.Concat("L", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //    sl.MergeWorksheetCells(string.Concat("AB", row), string.Concat("AB", row));
                                    //    sl.SetCellValue(string.Concat("AB", row), ans.ScoreOption9);
                                    //    sl.SetCellStyle(string.Concat("AB", row), string.Concat("AB", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //    sl.MergeWorksheetCells(string.Concat("AC", row), string.Concat("AC", row));
                                    //    sl.SetCellValue(string.Concat("AC", row), ans.ScoreOption10);
                                    //    sl.SetCellStyle(string.Concat("AC", row), string.Concat("AC", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    row++;
                                }
                            }

                        }

                    }
                    else
                    {
                        sl.MergeWorksheetCells(string.Concat("A", 5), string.Concat("J", 30));
                        sl.SetCellValue(string.Concat("A", 5), model.CaseStudyText);
                        sl.SetCellStyle(string.Concat("A", 5), string.Concat("J", 30), ExcelHelper.GetTextCaseStudyStyle(sl));

                        sl.SetCellStyle("A5", "J5", ExcelHelper.GetTopBorderStyle(sl));
                        sl.SetCellStyle("A5", "A30", ExcelHelper.GetLeftBorderStyle(sl));
                        sl.SetCellStyle("J5", "J30", ExcelHelper.GetRightBorderStyle(sl));
                        sl.SetCellStyle("A30", "J30", ExcelHelper.GetBottomBorderOnlyStyle(sl));
                    }

                }

                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);


            }


            ms.Position = 0;
            return ms;
        }



        public async Task<MemoryStream> GetCandidateCaseStudyDataExcel(AssessmentReportViewModel model)
        {
            var ms = new MemoryStream();
            var reportList = new List<string>() { "Case Study Data", "Case Study Report" };
            var modelData = new AssessmentDetailViewModel();
            using (var sl = new SLDocument())
            {
                foreach (var rep in reportList)
                {
                    sl.AddWorksheet(rep);

                    SLPageSettings pageSettings = new SLPageSettings();
                    pageSettings.ShowGridLines = true;
                    sl.SetPageSettings(pageSettings);

                    sl.SetColumnWidth("A", 20);
                    sl.SetColumnWidth("B", 20);
                    sl.SetColumnWidth("C", 20);
                    sl.SetColumnWidth("D", 20);
                    sl.SetColumnWidth("E", 20);
                    sl.SetColumnWidth("F", 20);
                    sl.SetColumnWidth("G", 20);
                    sl.SetColumnWidth("H", 20);
                    sl.SetColumnWidth("I", 20);
                    sl.SetColumnWidth("J", 20);
                    sl.SetColumnWidth("K", 20);
                    sl.SetColumnWidth("L", 20);
                    sl.SetColumnWidth("M", 20);
                    sl.SetColumnWidth("N", 20);

                    sl.MergeWorksheetCells("A1", "B1");
                    sl.SetCellValue("A1", "");
                    sl.SetCellStyle("A1", "B1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                    sl.MergeWorksheetCells("I1", "J1");
                    sl.SetCellValue("I1", DateTime.Today.ToDefaultDateFormat());
                    sl.SetCellStyle("I1", "J1", ExcelHelper.GetHeaderRowDateStyle(sl));

                    sl.MergeWorksheetCells("B2", "I3");
                    sl.SetCellValue("B2", rep + " - Candidate");
                    sl.SetCellStyle("B2", "I3", ExcelHelper.GetReportHeadingStyle(sl));

                    //sl.MergeWorksheetCells("B4", "K4");
                    //sl.SetCellStyle("B4", "K4", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));
                    //sl.SetCellStyle("B4", "K4", ExcelHelper.GetNoBorderCentreAlignStyle(sl)); 
                    if (rep != "Case Study Data")
                    {
                        if (rep == "Case Study Report")
                            modelData = model.CaseStudyAssessment;

                        if (modelData.IsNotNull())
                        {
                            sl.SetCellStyle("A5", "N5", ExcelHelper.GetTopBorderStyle(sl));
                            sl.SetCellStyle("A5", "A7", ExcelHelper.GetLeftBorderStyle(sl));
                            sl.SetCellStyle("N5", "N7", ExcelHelper.GetRightBorderStyle(sl));
                            sl.SetCellStyle("A7", "N7", ExcelHelper.GetBottomBorderOnlyStyle(sl));
                            sl.SetCellStyle("A5", "N7", ExcelHelper.FillColorStyle(sl));

                            sl.MergeWorksheetCells("A5", "B5");
                            sl.SetCellValue("A5", "Name");
                            sl.SetCellStyle("A5", "B5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("A6", "B7");
                            sl.SetCellValue("A6", model.OwnerUserName);
                            sl.SetCellStyle("A6", "B7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("C5", "C5");
                            sl.SetCellValue("C5", "Job Title");
                            sl.SetCellStyle("C5", "C5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("C6", "C7");
                            sl.SetCellValue("C6", model.JobTitle);
                            sl.SetCellStyle("C6", "C7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("D5", "E5");
                            sl.SetCellValue("D5", "Email ID");
                            sl.SetCellStyle("D5", "E5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("D6", "E7");
                            sl.SetCellValue("D6", model.Email);
                            sl.SetCellStyle("D6", "E7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("F5", "F5");
                            sl.SetCellValue("F5", "Assessment Name");
                            sl.SetCellStyle("F5", "F5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("F6", "F7");
                            sl.SetCellValue("F6", modelData.Title);
                            sl.SetCellStyle("F6", "F7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("G5", "G5");
                            sl.SetCellValue("G5", "Scheduled Start Date");
                            sl.SetCellStyle("G5", "G5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("G6", "G7");
                            sl.SetCellValue("G6", modelData.ScheduledStartDate.IsNotNull() ? modelData.ScheduledStartDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("G6", "G7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("H5", "H5");
                            sl.SetCellValue("H5", "Scheduled End Date");
                            sl.SetCellStyle("H5", "H5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("H6", "H7");
                            sl.SetCellValue("H6", modelData.ScheduledEndDate.IsNotNull() ? modelData.ScheduledEndDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("H6", "H7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("I5", "I5");
                            sl.SetCellValue("I5", "Actual Start Date");
                            sl.SetCellStyle("I5", "I5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("I6", "I7");
                            sl.SetCellValue("I6", modelData.ActualStartDate.IsNotNull() ? modelData.ActualStartDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("I6", "I7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("J5", "J5");
                            sl.SetCellValue("J5", "Actual End Date");
                            sl.SetCellStyle("J5", "J5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("J6", "J7");
                            sl.SetCellValue("J6", modelData.ActualEndDate.IsNotNull() ? modelData.ActualEndDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("J6", "J7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));


                            int row = 9;

                            sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                            sl.SetCellValue(string.Concat("A", row), "Sl No");
                            sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                            sl.SetCellValue(string.Concat("B", row), "Question");
                            sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            Int32 maxNo = modelData.AnswerList.Count > 0 ? modelData.AnswerList.Max(x => x.MaxoptionNo) : 0;
                            char first = 'B';

                            for (int i = 1; i <= maxNo; i++)
                            {
                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), "Option" + i);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            }

                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Candidate Answer");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Answer Comment");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            //sl.MergeWorksheetCells(string.Concat("M", row), string.Concat("M", row));
                            //sl.SetCellValue(string.Concat("M", row), "Candidate Answer");
                            //sl.SetCellStyle(string.Concat("M", row), string.Concat("M", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            //sl.MergeWorksheetCells(string.Concat("N", row), string.Concat("N", row));
                            //sl.SetCellValue(string.Concat("N", row), "Answer Comment");
                            //sl.SetCellStyle(string.Concat("N", row), string.Concat("N", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                            //sl.SetCellValue(string.Concat("C", row), "Option 1");
                            //sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                            //sl.SetCellValue(string.Concat("D", row), "Option 2");
                            //sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                            //sl.SetCellValue(string.Concat("E", row), "Option 3");
                            //sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("F", row));
                            //sl.SetCellValue(string.Concat("F", row), "Option 4");
                            //sl.SetCellStyle(string.Concat("F", row), string.Concat("F", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("G", row), string.Concat("G", row));
                            //sl.SetCellValue(string.Concat("G", row), "Option 5");
                            //sl.SetCellStyle(string.Concat("G", row), string.Concat("G", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("H", row), string.Concat("H", row));
                            //sl.SetCellValue(string.Concat("H", row), "Option 6");
                            //sl.SetCellStyle(string.Concat("H", row), string.Concat("H", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("I", row), string.Concat("I", row));
                            //sl.SetCellValue(string.Concat("I", row), "Option 7");
                            //sl.SetCellStyle(string.Concat("I", row), string.Concat("I", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("J", row), string.Concat("J", row));
                            //sl.SetCellValue(string.Concat("J", row), "Option 8");
                            //sl.SetCellStyle(string.Concat("J", row), string.Concat("J", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("K", row), string.Concat("K", row));
                            //sl.SetCellValue(string.Concat("K", row), "Option 9");
                            //sl.SetCellStyle(string.Concat("K", row), string.Concat("K", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("L", row), string.Concat("L", row));
                            //sl.SetCellValue(string.Concat("L", row), "Option 10");
                            //sl.SetCellStyle(string.Concat("L", row), string.Concat("L", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            //sl.MergeWorksheetCells(string.Concat("I", row), string.Concat("I", row));
                            //sl.SetCellValue(string.Concat("I", row), "");
                            //sl.SetCellStyle(string.Concat("I", row), string.Concat("I", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            //sl.MergeWorksheetCells(string.Concat("J", row), string.Concat("J", row));
                            //sl.SetCellValue(string.Concat("J", row), "");
                            //sl.SetCellStyle(string.Concat("J", row), string.Concat("J", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            row++;
                            if (modelData.AnswerList != null)
                            {
                                foreach (var ans in modelData.AnswerList)
                                {
                                    sl.SetRowHeight(row, 30);
                                    sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                                    sl.SetCellValue(string.Concat("A", row), ans.SeraillNo.IsNotNull() ? ans.SeraillNo.ToString() : "");
                                    sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                                    sl.SetCellValue(string.Concat("B", row), model.CaseStudyAssessment.PreferredLanguageCode == "ARABIC" ? ans.QuestionAr : ans.Question);
                                    sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    var options = await GetOptionList(ans.QuestionId);
                                    options = options.OrderBy(x => x.SequenceOrder).ToList();
                                    first = 'B';
                                    foreach (var item in options)
                                    {
                                        first = (char)((int)first + 1);
                                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                        sl.SetCellValue(string.Concat(first, row), model.CaseStudyAssessment.PreferredLanguageCode == "ARABIC" ? item.OptionArabic : item.Option);
                                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    }
                                    if (maxNo > options.Count)
                                    {
                                        Int32 k = maxNo - options.Count;

                                        for (int i = 0; i < k; i++)
                                        {
                                            first = (char)((int)first + 1);
                                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                            sl.SetCellValue(string.Concat(first, row), "");
                                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                        }
                                    }
                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), model.CaseStudyAssessment.PreferredLanguageCode == "ARABIC" ? ans.CandidateAnswerAr : ans.CandidateAnswer);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), model.CaseStudyAssessment.PreferredLanguageCode == "ARABIC" ? ans.CandidateAnswerCommentAr : ans.CandidateAnswerComment);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    //sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                                    //sl.SetCellValue(string.Concat("C", row), ans.Option1);
                                    //sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                                    //sl.SetCellValue(string.Concat("D", row), ans.Option2);
                                    //sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                                    //sl.SetCellValue(string.Concat("E", row), ans.Option3);
                                    //sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("F", row));
                                    //sl.SetCellValue(string.Concat("F", row), ans.Option4);
                                    //sl.SetCellStyle(string.Concat("F", row), string.Concat("F", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("G", row), string.Concat("G", row));
                                    //sl.SetCellValue(string.Concat("G", row), ans.Option5);
                                    //sl.SetCellStyle(string.Concat("G", row), string.Concat("G", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("H", row), string.Concat("H", row));
                                    //sl.SetCellValue(string.Concat("H", row), ans.Option6);
                                    //sl.SetCellStyle(string.Concat("H", row), string.Concat("H", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("I", row), string.Concat("I", row));
                                    //sl.SetCellValue(string.Concat("I", row), ans.Option7);
                                    //sl.SetCellStyle(string.Concat("I", row), string.Concat("I", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("J", row), string.Concat("J", row));
                                    //sl.SetCellValue(string.Concat("J", row), ans.Option8);
                                    //sl.SetCellStyle(string.Concat("J", row), string.Concat("J", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("K", row), string.Concat("K", row));
                                    //sl.SetCellValue(string.Concat("K", row), ans.Option9);
                                    //sl.SetCellStyle(string.Concat("K", row), string.Concat("K", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("L", row), string.Concat("L", row));
                                    //sl.SetCellValue(string.Concat("L", row), ans.Option10);
                                    //sl.SetCellStyle(string.Concat("L", row), string.Concat("L", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                    row++;
                                }
                            }

                        }

                    }
                    else
                    {
                        sl.MergeWorksheetCells(string.Concat("A", 5), string.Concat("J", 30));
                        sl.SetCellValue(string.Concat("A", 5), model.CaseStudyText);
                        sl.SetCellStyle(string.Concat("A", 5), string.Concat("J", 30), ExcelHelper.GetTextCaseStudyStyle(sl));

                        sl.SetCellStyle("A5", "J5", ExcelHelper.GetTopBorderStyle(sl));
                        sl.SetCellStyle("A5", "A30", ExcelHelper.GetLeftBorderStyle(sl));
                        sl.SetCellStyle("J5", "J30", ExcelHelper.GetRightBorderStyle(sl));
                        sl.SetCellStyle("A30", "J30", ExcelHelper.GetBottomBorderOnlyStyle(sl));
                    }

                }

                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);


            }


            ms.Position = 0;
            return ms;
        }

        public async Task<MemoryStream> GetAssessmentReportDataExcel(AssessmentReportViewModel model)
        {
            var ms = new MemoryStream();
            var reportList = new List<string>();
            //{ "Assessment Report", "Case Study Data", "Case Study Report" };

            reportList.Add("Case Study Data");
            reportList.Add("Case Study Report");

            using (var sl = new SLDocument())
            {
                foreach (var modelData in model.TechnicalAssessment)
                {
                    var Title = modelData.Title;
                    if (Title.IsNotNull())
                    {
                        if (modelData.Title.Length > 30)
                            Title = modelData.Title.Substring(0, 29);
                    }

                    sl.AddWorksheet(Title);

                    SLPageSettings pageSettings = new SLPageSettings();
                    pageSettings.ShowGridLines = true;
                    sl.SetPageSettings(pageSettings);

                    sl.SetColumnWidth("A", 20);
                    sl.SetColumnWidth("B", 20);
                    sl.SetColumnWidth("C", 20);
                    sl.SetColumnWidth("D", 20);
                    sl.SetColumnWidth("E", 20);
                    sl.SetColumnWidth("F", 20);
                    sl.SetColumnWidth("G", 20);
                    sl.SetColumnWidth("H", 20);
                    sl.SetColumnWidth("I", 20);
                    sl.SetColumnWidth("J", 20);
                    sl.SetColumnWidth("K", 20);
                    sl.SetColumnWidth("L", 20);
                    sl.SetColumnWidth("M", 20);
                    sl.SetColumnWidth("N", 20);
                    sl.SetColumnWidth("O", 20);
                    sl.SetColumnWidth("P", 20);
                    sl.SetColumnWidth("R", 20);

                    sl.MergeWorksheetCells("A1", "B1");
                    sl.SetCellValue("A1", "");
                    sl.SetCellStyle("A1", "B1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                    sl.MergeWorksheetCells("I1", "J1");
                    sl.SetCellValue("I1", DateTime.Today.ToDefaultDateFormat());
                    sl.SetCellStyle("I1", "J1", ExcelHelper.GetHeaderRowDateStyle(sl));

                    sl.MergeWorksheetCells("B2", "I3");
                    sl.SetCellValue("B2", modelData.Title);
                    sl.SetCellStyle("B2", "I3", ExcelHelper.GetReportHeadingStyle(sl));

                    //sl.MergeWorksheetCells("B4", "K4");
                    //sl.SetCellStyle("B4", "K4", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));
                    //sl.SetCellStyle("B4", "K4", ExcelHelper.GetNoBorderCentreAlignStyle(sl)); 


                    if (modelData.IsNotNull())
                    {
                        sl.SetCellStyle("A5", "N5", ExcelHelper.GetTopBorderStyle(sl));
                        sl.SetCellStyle("A5", "A7", ExcelHelper.GetLeftBorderStyle(sl));
                        sl.SetCellStyle("N5", "N7", ExcelHelper.GetRightBorderStyle(sl));
                        sl.SetCellStyle("A7", "N7", ExcelHelper.GetBottomBorderOnlyStyle(sl));
                        sl.SetCellStyle("A5", "N7", ExcelHelper.FillColorStyle(sl));

                        sl.MergeWorksheetCells("A5", "B5");
                        sl.SetCellValue("A5", "Name");
                        sl.SetCellStyle("A5", "B5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("A6", "B7");
                        sl.SetCellValue("A6", model.OwnerUserName);
                        sl.SetCellStyle("A6", "B7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("C5", "C5");
                        sl.SetCellValue("C5", "Job Title");
                        sl.SetCellStyle("C5", "C5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("C6", "C7");
                        sl.SetCellValue("C6", model.JobTitle);
                        sl.SetCellStyle("C6", "C7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("D5", "E5");
                        sl.SetCellValue("D5", "Email ID");
                        sl.SetCellStyle("D5", "E5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("D6", "E7");
                        sl.SetCellValue("D6", model.Email);
                        sl.SetCellStyle("D6", "E7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("F5", "F5");
                        sl.SetCellValue("F5", "Assessment Name");
                        sl.SetCellStyle("F5", "F5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("F6", "F7");
                        sl.SetCellValue("F6", modelData.Subject);
                        sl.SetCellStyle("F6", "F7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("G5", "G5");
                        sl.SetCellValue("G5", "Scheduled Start Date");
                        sl.SetCellStyle("G5", "G5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("G6", "G7");
                        sl.SetCellValue("G6", modelData.ScheduledStartDate.IsNotNull() ? modelData.ScheduledStartDate.ToDefaultDateFormat() : "");
                        sl.SetCellStyle("G6", "G7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("H5", "H5");
                        sl.SetCellValue("H5", "Scheduled End Date");
                        sl.SetCellStyle("H5", "H5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("H6", "H7");
                        sl.SetCellValue("H6", modelData.ScheduledEndDate.IsNotNull() ? modelData.ScheduledEndDate.ToDefaultDateFormat() : "");
                        sl.SetCellStyle("H6", "H7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("I5", "I5");
                        sl.SetCellValue("I5", "Actual Start Date");
                        sl.SetCellStyle("I5", "I5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("I6", "I7");
                        sl.SetCellValue("I6", modelData.ActualStartDate.IsNotNull() ? modelData.ActualStartDate.ToDefaultDateFormat() : "");
                        sl.SetCellStyle("I6", "I7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("J5", "J5");
                        sl.SetCellValue("J5", "Actual End Date");
                        sl.SetCellStyle("J5", "J5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("J6", "J7");
                        sl.SetCellValue("J6", modelData.ActualEndDate.IsNotNull() ? modelData.ActualEndDate.ToDefaultDateFormat() : "");
                        sl.SetCellStyle("J6", "J7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        sl.MergeWorksheetCells("K5", "N7");
                        sl.SetCellStyle("K5", "N7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                        int row = 9;

                        sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                        sl.SetCellValue(string.Concat("A", row), "Sl No");
                        sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                        sl.SetCellValue(string.Concat("B", row), "Question");
                        sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetShiftEntryDateStyle(sl));



                        Int32 maxNo = modelData.AnswerList.Count > 0 ? modelData.AnswerList.Max(x => x.MaxoptionNo) : 0;
                        char first = 'B';

                        for (int i = 1; i <= maxNo; i++)
                        {
                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Option" + i);
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                        }



                        //sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                        //sl.SetCellValue(string.Concat("C", row), "Option 1");
                        //sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                        //sl.SetCellValue(string.Concat("D", row), "Option 2");
                        //sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                        //sl.SetCellValue(string.Concat("E", row), "Option 3");
                        //sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("F", row));
                        //sl.SetCellValue(string.Concat("F", row), "Option 4");
                        //sl.SetCellStyle(string.Concat("F", row), string.Concat("F", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("G", row), string.Concat("G", row));
                        //sl.SetCellValue(string.Concat("G", row), "Option 5");
                        //sl.SetCellStyle(string.Concat("G", row), string.Concat("G", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("H", row), string.Concat("H", row));
                        //sl.SetCellValue(string.Concat("H", row), "Option 6");
                        //sl.SetCellStyle(string.Concat("H", row), string.Concat("H", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("I", row), string.Concat("I", row));
                        //sl.SetCellValue(string.Concat("I", row), "Option 7");
                        //sl.SetCellStyle(string.Concat("I", row), string.Concat("I", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("J", row), string.Concat("J", row));
                        //sl.SetCellValue(string.Concat("J", row), "Option 8");
                        //sl.SetCellStyle(string.Concat("J", row), string.Concat("J", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("K", row), string.Concat("K", row));
                        //sl.SetCellValue(string.Concat("K", row), "Option 9");
                        //sl.SetCellStyle(string.Concat("K", row), string.Concat("K", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("L", row), string.Concat("L", row));
                        //sl.SetCellValue(string.Concat("L", row), "Option 10");
                        //sl.SetCellStyle(string.Concat("L", row), string.Concat("L", row), ExcelHelper.GetShiftEntryDateStyle(sl));


                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), "Right Answer");
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));

                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), "Candidate Answer");
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), "Accurate");
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));

                        //first = (char)((int)first + 1);
                        //sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        //sl.SetCellValue(string.Concat(first, row), "");
                        //sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));

                        first = (char)((int)first + 1);
                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                        sl.SetCellValue(string.Concat(first, row), "Answer Comment");
                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));




                        for (int i = 1; i <= maxNo; i++)
                        {
                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Score Option" + i);
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                        }




                        //sl.MergeWorksheetCells(string.Concat("Q", row), string.Concat("Q", row));
                        //sl.SetCellValue(string.Concat("Q", row), "Score Option 1");
                        //sl.SetCellStyle(string.Concat("Q", row), string.Concat("Q", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("R", row), string.Concat("R", row));
                        //sl.SetCellValue(string.Concat("R", row), "Score Option 2");
                        //sl.SetCellStyle(string.Concat("R", row), string.Concat("R", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("S", row), string.Concat("S", row));
                        //sl.SetCellValue(string.Concat("S", row), "Score Option 3");
                        //sl.SetCellStyle(string.Concat("S", row), string.Concat("S", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("T", row), string.Concat("T", row));
                        //sl.SetCellValue(string.Concat("T", row), "Score Option 4");
                        //sl.SetCellStyle(string.Concat("T", row), string.Concat("T", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("U", row), string.Concat("U", row));
                        //sl.SetCellValue(string.Concat("U", row), "Score Option 5");
                        //sl.SetCellStyle(string.Concat("U", row), string.Concat("U", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("V", row), string.Concat("V", row));
                        //sl.SetCellValue(string.Concat("V", row), "Score Option 6");
                        //sl.SetCellStyle(string.Concat("V", row), string.Concat("V", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("W", row), string.Concat("W", row));
                        //sl.SetCellValue(string.Concat("W", row), "Score Option 7");
                        //sl.SetCellStyle(string.Concat("W", row), string.Concat("W", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //
                        //sl.MergeWorksheetCells(string.Concat("X", row), string.Concat("X", row));
                        //sl.SetCellValue(string.Concat("X", row), "Score Option 8");
                        //sl.SetCellStyle(string.Concat("X", row), string.Concat("X", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("Y", row), string.Concat("Y", row));
                        //sl.SetCellValue(string.Concat("Y", row), "Score Option 9");
                        //sl.SetCellStyle(string.Concat("Y", row), string.Concat("Y", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        //
                        //sl.MergeWorksheetCells(string.Concat("Z", row), string.Concat("Z", row));
                        //sl.SetCellValue(string.Concat("Z", row), "Score Option 10");
                        //sl.SetCellStyle(string.Concat("Z", row), string.Concat("Z", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                        row++;
                        if (modelData.AnswerList != null)
                        {
                            foreach (var ans in modelData.AnswerList)
                            {
                                sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                                sl.SetCellValue(string.Concat("A", row), ans.SeraillNo.IsNotNull() ? ans.SeraillNo.ToString() : "");
                                sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                                sl.SetCellValue(string.Concat("B", row), modelData.PreferredLanguageCode == "ARABIC" ? ans.QuestionAr : ans.Question);
                                sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                var options = await GetOptionList(ans.QuestionId);
                                options = options.OrderBy(x => x.SequenceOrder).ToList();
                                first = 'B';
                                foreach (var item in options)
                                {
                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), modelData.PreferredLanguageCode == "ARABIC" ? item.OptionArabic : item.Option);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                }

                                if (maxNo > options.Count)
                                {
                                    Int32 k = maxNo - options.Count;

                                    for (int i = 0; i < k; i++)
                                    {
                                        first = (char)((int)first + 1);
                                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                        sl.SetCellValue(string.Concat(first, row), "");
                                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    }
                                }

                                // sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                                // sl.SetCellValue(string.Concat("C", row), ans.Option1);
                                // sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                // sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                                // sl.SetCellValue(string.Concat("D", row), ans.Option2);
                                // sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                // sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                                // sl.SetCellValue(string.Concat("E", row), ans.Option3);
                                // sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                // sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("F", row));
                                // sl.SetCellValue(string.Concat("F", row), ans.Option4);
                                // sl.SetCellStyle(string.Concat("F", row), string.Concat("F", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                // sl.MergeWorksheetCells(string.Concat("G", row), string.Concat("G", row));
                                // sl.SetCellValue(string.Concat("G", row), ans.Option5);
                                // sl.SetCellStyle(string.Concat("G", row), string.Concat("G", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                // sl.MergeWorksheetCells(string.Concat("H", row), string.Concat("H", row));
                                // sl.SetCellValue(string.Concat("H", row), ans.Option6);
                                // sl.SetCellStyle(string.Concat("H", row), string.Concat("H", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                // sl.MergeWorksheetCells(string.Concat("I", row), string.Concat("I", row));
                                // sl.SetCellValue(string.Concat("I", row), ans.Option7);
                                // sl.SetCellStyle(string.Concat("I", row), string.Concat("I", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                // sl.MergeWorksheetCells(string.Concat("J", row), string.Concat("J", row));
                                // sl.SetCellValue(string.Concat("J", row), ans.Option8);
                                // sl.SetCellStyle(string.Concat("J", row), string.Concat("J", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                // sl.MergeWorksheetCells(string.Concat("K", row), string.Concat("K", row));
                                // sl.SetCellValue(string.Concat("K", row), ans.Option9);
                                // sl.SetCellStyle(string.Concat("K", row), string.Concat("K", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                // sl.MergeWorksheetCells(string.Concat("L", row), string.Concat("L", row));
                                // sl.SetCellValue(string.Concat("L", row), ans.Option10);
                                // sl.SetCellStyle(string.Concat("L", row), string.Concat("L", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                first = (char)((int)first + 1);

                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), modelData.PreferredLanguageCode == "ARABIC" ? ans.RightAnswerAr : ans.RightAnswer);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), modelData.PreferredLanguageCode == "ARABIC" ? ans.CandidateAnswerAr : ans.CandidateAnswer);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), ans.Accurate.ToString());
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                //first = (char)((int)first + 1);
                                //sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                //sl.SetCellValue(string.Concat(first, row), "");
                                //sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), modelData.PreferredLanguageCode == "ARABIC" ? ans.CandidateAnswerCommentAr : ans.CandidateAnswerComment);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                foreach (var item in options)
                                {
                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), item.Score);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                }


                                //sl.MergeWorksheetCells(string.Concat("Q", row), string.Concat("Q", row));
                                //sl.SetCellValue(string.Concat("Q", row), ans.ScoreOption1);
                                //sl.SetCellStyle(string.Concat("Q", row), string.Concat("Q", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                //sl.MergeWorksheetCells(string.Concat("R", row), string.Concat("R", row));
                                //sl.SetCellValue(string.Concat("R", row), ans.ScoreOption2);
                                //sl.SetCellStyle(string.Concat("R", row), string.Concat("R", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                //sl.MergeWorksheetCells(string.Concat("S", row), string.Concat("S", row));
                                //sl.SetCellValue(string.Concat("S", row), ans.ScoreOption3);
                                //sl.SetCellStyle(string.Concat("S", row), string.Concat("S", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                //sl.MergeWorksheetCells(string.Concat("T", row), string.Concat("T", row));
                                //sl.SetCellValue(string.Concat("T", row), ans.ScoreOption4);
                                //sl.SetCellStyle(string.Concat("T", row), string.Concat("T", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                //sl.MergeWorksheetCells(string.Concat("U", row), string.Concat("U", row));
                                //sl.SetCellValue(string.Concat("U", row), ans.ScoreOption5);
                                //sl.SetCellStyle(string.Concat("U", row), string.Concat("U", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                //sl.MergeWorksheetCells(string.Concat("V", row), string.Concat("V", row));
                                //sl.SetCellValue(string.Concat("V", row), ans.ScoreOption6);
                                //sl.SetCellStyle(string.Concat("V", row), string.Concat("V", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                //sl.MergeWorksheetCells(string.Concat("W", row), string.Concat("W", row));
                                //sl.SetCellValue(string.Concat("W", row), ans.ScoreOption7);
                                //sl.SetCellStyle(string.Concat("W", row), string.Concat("W", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                //sl.MergeWorksheetCells(string.Concat("X", row), string.Concat("X", row));
                                //sl.SetCellValue(string.Concat("X", row), ans.ScoreOption8);
                                //sl.SetCellStyle(string.Concat("X", row), string.Concat("X", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                //sl.MergeWorksheetCells(string.Concat("Y", row), string.Concat("Y", row));
                                //sl.SetCellValue(string.Concat("Y", row), ans.ScoreOption9);
                                //sl.SetCellStyle(string.Concat("Y", row), string.Concat("Y", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                //
                                //sl.MergeWorksheetCells(string.Concat("Z", row), string.Concat("Z", row));
                                //sl.SetCellValue(string.Concat("Z", row), ans.ScoreOption10);
                                //sl.SetCellStyle(string.Concat("Z", row), string.Concat("Z", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));



                                row++;
                            }
                        }

                    }



                }
                foreach (var rep in reportList)
                {
                    sl.AddWorksheet(rep);
                    var modelData = new AssessmentDetailViewModel();
                    SLPageSettings pageSettings = new SLPageSettings();
                    pageSettings.ShowGridLines = true;
                    sl.SetPageSettings(pageSettings);

                    sl.SetColumnWidth("A", 20);
                    sl.SetColumnWidth("B", 20);
                    sl.SetColumnWidth("C", 20);
                    sl.SetColumnWidth("D", 20);
                    sl.SetColumnWidth("E", 20);
                    sl.SetColumnWidth("F", 20);
                    sl.SetColumnWidth("G", 20);
                    sl.SetColumnWidth("H", 20);
                    sl.SetColumnWidth("I", 20);
                    sl.SetColumnWidth("J", 20);
                    sl.SetColumnWidth("K", 20);
                    sl.SetColumnWidth("L", 20);
                    sl.SetColumnWidth("M", 20);
                    sl.SetColumnWidth("N", 20);
                    sl.SetColumnWidth("O", 20);
                    sl.SetColumnWidth("P", 20);

                    sl.MergeWorksheetCells("A1", "B1");
                    sl.SetCellValue("A1", "");
                    sl.SetCellStyle("A1", "B1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                    sl.MergeWorksheetCells("I1", "J1");
                    sl.SetCellValue("I1", DateTime.Today.ToDefaultDateFormat());
                    sl.SetCellStyle("I1", "J1", ExcelHelper.GetHeaderRowDateStyle(sl));

                    sl.MergeWorksheetCells("B2", "I3");
                    sl.SetCellValue("B2", rep);
                    sl.SetCellStyle("B2", "I3", ExcelHelper.GetReportHeadingStyle(sl));

                    //sl.MergeWorksheetCells("B4", "K4");
                    //sl.SetCellStyle("B4", "K4", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));
                    //sl.SetCellStyle("B4", "K4", ExcelHelper.GetNoBorderCentreAlignStyle(sl)); 
                    if (rep != "Case Study Data")
                    {
                        //if (rep == "Assessment Report")
                        //    modelData = model.TechnicalAssessment;
                        //else 
                        if (rep == "Case Study Report")
                            modelData = model.CaseStudyAssessment;

                        if (modelData.IsNotNull())
                        {
                            sl.SetCellStyle("A5", "N5", ExcelHelper.GetTopBorderStyle(sl));
                            sl.SetCellStyle("A5", "A7", ExcelHelper.GetLeftBorderStyle(sl));
                            sl.SetCellStyle("N5", "N7", ExcelHelper.GetRightBorderStyle(sl));
                            sl.SetCellStyle("A7", "N7", ExcelHelper.GetBottomBorderOnlyStyle(sl));
                            sl.SetCellStyle("A5", "N7", ExcelHelper.FillColorStyle(sl));

                            sl.MergeWorksheetCells("A5", "B5");
                            sl.SetCellValue("A5", "Name");
                            sl.SetCellStyle("A5", "B5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("A6", "B7");
                            sl.SetCellValue("A6", model.OwnerUserName);
                            sl.SetCellStyle("A6", "B7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("C5", "C5");
                            sl.SetCellValue("C5", "Job Title");
                            sl.SetCellStyle("C5", "C5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("C6", "C7");
                            sl.SetCellValue("C6", model.JobTitle);
                            sl.SetCellStyle("C6", "C7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("D5", "E5");
                            sl.SetCellValue("D5", "Email ID");
                            sl.SetCellStyle("D5", "E5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("D6", "E7");
                            sl.SetCellValue("D6", model.Email);
                            sl.SetCellStyle("D6", "E7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("F5", "F5");
                            sl.SetCellValue("F5", "Assessment Name");
                            sl.SetCellStyle("F5", "F5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("F6", "F7");
                            sl.SetCellValue("F6", modelData.Subject);
                            sl.SetCellStyle("F6", "F7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("G5", "G5");
                            sl.SetCellValue("G5", "Scheduled Start Date");
                            sl.SetCellStyle("G5", "G5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("G6", "G7");
                            sl.SetCellValue("G6", modelData.ScheduledStartDate.IsNotNull() ? modelData.ScheduledStartDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("G6", "G7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("H5", "H5");
                            sl.SetCellValue("H5", "Scheduled End Date");
                            sl.SetCellStyle("H5", "H5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("H6", "H7");
                            sl.SetCellValue("H6", modelData.ScheduledEndDate.IsNotNull() ? modelData.ScheduledEndDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("H6", "H7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("I5", "I5");
                            sl.SetCellValue("I5", "Actual Start Date");
                            sl.SetCellStyle("I5", "I5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("I6", "I7");
                            sl.SetCellValue("I6", modelData.ActualStartDate.IsNotNull() ? modelData.ActualStartDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("I6", "I7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("J5", "J5");
                            sl.SetCellValue("J5", "Actual End Date");
                            sl.SetCellStyle("J5", "J5", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("J6", "J7");
                            sl.SetCellValue("J6", modelData.ActualEndDate.IsNotNull() ? modelData.ActualEndDate.ToDefaultDateFormat() : "");
                            sl.SetCellStyle("J6", "J7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            sl.MergeWorksheetCells("K6", "N7");
                            sl.SetCellStyle("K6", "N7", ExcelHelper.GetTextBoldwithoutBorderStyle(sl));

                            int row = 9;

                            sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                            sl.SetCellValue(string.Concat("A", row), "Sl No");
                            sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                            sl.SetCellValue(string.Concat("B", row), "Question");
                            sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            Int32 maxNo = modelData.AnswerList.Count > 0 ? modelData.AnswerList.Max(x => x.MaxoptionNo) : 0;
                            char first = 'B';

                            for (int i = 1; i <= maxNo; i++)
                            {
                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), "Option" + i);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            }

                            //sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                            //sl.SetCellValue(string.Concat("C", row), "Option 1");
                            //sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                            //sl.SetCellValue(string.Concat("D", row), "Option 2");
                            //sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                            //sl.SetCellValue(string.Concat("E", row), "Option 3");
                            //sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("F", row));
                            //sl.SetCellValue(string.Concat("F", row), "Option 4");
                            //sl.SetCellStyle(string.Concat("F", row), string.Concat("F", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("G", row), string.Concat("G", row));
                            //sl.SetCellValue(string.Concat("G", row), "Option 5");
                            //sl.SetCellStyle(string.Concat("G", row), string.Concat("G", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("H", row), string.Concat("H", row));
                            //sl.SetCellValue(string.Concat("H", row), "Option 6");
                            //sl.SetCellStyle(string.Concat("H", row), string.Concat("H", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("I", row), string.Concat("I", row));
                            //sl.SetCellValue(string.Concat("I", row), "Option 7");
                            //sl.SetCellStyle(string.Concat("I", row), string.Concat("I", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("J", row), string.Concat("J", row));
                            //sl.SetCellValue(string.Concat("J", row), "Option 8");
                            //sl.SetCellStyle(string.Concat("J", row), string.Concat("J", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("K", row), string.Concat("K", row));
                            //sl.SetCellValue(string.Concat("K", row), "Option 9");
                            //sl.SetCellStyle(string.Concat("K", row), string.Concat("K", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("L", row), string.Concat("L", row));
                            //sl.SetCellValue(string.Concat("L", row), "Option 10");
                            //sl.SetCellStyle(string.Concat("L", row), string.Concat("L", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Right Answer");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Candidate Answer");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            //first = (char)((int)first + 1);
                            //sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            //sl.SetCellValue(string.Concat(first, row), "");
                            //sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Accurate");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));

                            first = (char)((int)first + 1);
                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                            sl.SetCellValue(string.Concat(first, row), "Answer Comment");
                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            for (int i = 1; i <= maxNo; i++)
                            {
                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), "Score Option" + i);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetShiftEntryDateStyle(sl));


                            }

                            //sl.MergeWorksheetCells(string.Concat("Q", row), string.Concat("Q", row));
                            //sl.SetCellValue(string.Concat("Q", row), "Score Option 1");
                            //sl.SetCellStyle(string.Concat("Q", row), string.Concat("Q", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("R", row), string.Concat("R", row));
                            //sl.SetCellValue(string.Concat("R", row), "Score Option 2");
                            //sl.SetCellStyle(string.Concat("R", row), string.Concat("R", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("S", row), string.Concat("S", row));
                            //sl.SetCellValue(string.Concat("S", row), "Score Option 3");
                            //sl.SetCellStyle(string.Concat("S", row), string.Concat("S", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("T", row), string.Concat("T", row));
                            //sl.SetCellValue(string.Concat("T", row), "Score Option 4");
                            //sl.SetCellStyle(string.Concat("T", row), string.Concat("T", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("U", row), string.Concat("U", row));
                            //sl.SetCellValue(string.Concat("U", row), "Score Option 5");
                            //sl.SetCellStyle(string.Concat("U", row), string.Concat("U", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("V", row), string.Concat("V", row));
                            //sl.SetCellValue(string.Concat("V", row), "Score Option 6");
                            //sl.SetCellStyle(string.Concat("V", row), string.Concat("V", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("W", row), string.Concat("W", row));
                            //sl.SetCellValue(string.Concat("W", row), "Score Option 7");
                            //sl.SetCellStyle(string.Concat("W", row), string.Concat("W", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //
                            //sl.MergeWorksheetCells(string.Concat("X", row), string.Concat("X", row));
                            //sl.SetCellValue(string.Concat("X", row), "Score Option 8");
                            //sl.SetCellStyle(string.Concat("X", row), string.Concat("X", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("Y", row), string.Concat("Y", row));
                            //sl.SetCellValue(string.Concat("Y", row), "Score Option 9");
                            //sl.SetCellStyle(string.Concat("Y", row), string.Concat("Y", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            //
                            //sl.MergeWorksheetCells(string.Concat("Z", row), string.Concat("Z", row));
                            //sl.SetCellValue(string.Concat("Z", row), "Score Option 10");
                            //sl.SetCellStyle(string.Concat("Z", row), string.Concat("Z", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                            row++;
                            if (modelData.AnswerList != null)
                            {
                                foreach (var ans in modelData.AnswerList)
                                {
                                    sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                                    sl.SetCellValue(string.Concat("A", row), ans.SeraillNo.IsNotNull() ? ans.SeraillNo.ToString() : "");
                                    sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                                    sl.SetCellValue(string.Concat("B", row), modelData.PreferredLanguageCode == "ARABIC" ? ans.QuestionAr : ans.Question);
                                    sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));



                                    var options = await GetOptionList(ans.QuestionId);
                                    options = options.OrderBy(x => x.SequenceOrder).ToList();
                                    first = 'B';
                                    foreach (var item in options)
                                    {
                                        first = (char)((int)first + 1);
                                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                        sl.SetCellValue(string.Concat(first, row), modelData.PreferredLanguageCode == "ARABIC" ? item.OptionArabic : item.Option);
                                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    }

                                    if (maxNo > options.Count)
                                    {
                                        Int32 k = maxNo - options.Count;

                                        for (int i = 0; i < k; i++)
                                        {
                                            first = (char)((int)first + 1);
                                            sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                            sl.SetCellValue(string.Concat(first, row), "");
                                            sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                        }
                                    }
                                    //sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                                    //sl.SetCellValue(string.Concat("C", row), ans.Option1);
                                    //sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                                    //sl.SetCellValue(string.Concat("D", row), ans.Option2);
                                    //sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                                    //sl.SetCellValue(string.Concat("E", row), ans.Option3);
                                    //sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("F", row));
                                    //sl.SetCellValue(string.Concat("F", row), ans.Option4);
                                    //sl.SetCellStyle(string.Concat("F", row), string.Concat("F", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("G", row), string.Concat("G", row));
                                    //sl.SetCellValue(string.Concat("G", row), ans.Option5);
                                    //sl.SetCellStyle(string.Concat("G", row), string.Concat("G", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("H", row), string.Concat("H", row));
                                    //sl.SetCellValue(string.Concat("H", row), ans.Option6);
                                    //sl.SetCellStyle(string.Concat("H", row), string.Concat("H", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("I", row), string.Concat("I", row));
                                    //sl.SetCellValue(string.Concat("I", row), ans.Option7);
                                    //sl.SetCellStyle(string.Concat("I", row), string.Concat("I", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("J", row), string.Concat("J", row));
                                    //sl.SetCellValue(string.Concat("J", row), ans.Option8);
                                    //sl.SetCellStyle(string.Concat("J", row), string.Concat("J", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("K", row), string.Concat("K", row));
                                    //sl.SetCellValue(string.Concat("K", row), ans.Option9);
                                    //sl.SetCellStyle(string.Concat("K", row), string.Concat("K", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("L", row), string.Concat("L", row));
                                    //sl.SetCellValue(string.Concat("L", row), ans.Option10);
                                    //sl.SetCellStyle(string.Concat("L", row), string.Concat("L", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), modelData.PreferredLanguageCode == "ARABIC" ? ans.RightAnswerAr : ans.RightAnswer);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), modelData.PreferredLanguageCode == "ARABIC" ? ans.CandidateAnswerAr : ans.CandidateAnswer);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    //first = (char)((int)first + 1);
                                    //sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    //sl.SetCellValue(string.Concat(first, row),"");
                                    //sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), ans.Accurate.ToString());
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                    first = (char)((int)first + 1);
                                    sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                    sl.SetCellValue(string.Concat(first, row), modelData.PreferredLanguageCode == "ARABIC" ? ans.CandidateAnswerCommentAr : ans.CandidateAnswerComment);
                                    sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));



                                    foreach (var item in options)
                                    {
                                        first = (char)((int)first + 1);
                                        sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                        sl.SetCellValue(string.Concat(first, row), item.Score);
                                        sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    }

                                    //sl.MergeWorksheetCells(string.Concat("Q", row), string.Concat("Q", row));
                                    //sl.SetCellValue(string.Concat("Q", row), ans.ScoreOption1);
                                    //sl.SetCellStyle(string.Concat("Q", row), string.Concat("Q", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("R", row), string.Concat("R", row));
                                    //sl.SetCellValue(string.Concat("R", row), ans.ScoreOption2);
                                    //sl.SetCellStyle(string.Concat("R", row), string.Concat("R", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("S", row), string.Concat("S", row));
                                    //sl.SetCellValue(string.Concat("S", row), ans.ScoreOption3);
                                    //sl.SetCellStyle(string.Concat("S", row), string.Concat("S", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("T", row), string.Concat("T", row));
                                    //sl.SetCellValue(string.Concat("T", row), ans.ScoreOption4);
                                    //sl.SetCellStyle(string.Concat("T", row), string.Concat("T", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("U", row), string.Concat("U", row));
                                    //sl.SetCellValue(string.Concat("U", row), ans.ScoreOption5);
                                    //sl.SetCellStyle(string.Concat("U", row), string.Concat("U", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("V", row), string.Concat("V", row));
                                    //sl.SetCellValue(string.Concat("V", row), ans.ScoreOption6);
                                    //sl.SetCellStyle(string.Concat("V", row), string.Concat("V", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("W", row), string.Concat("W", row));
                                    //sl.SetCellValue(string.Concat("W", row), ans.ScoreOption7);
                                    //sl.SetCellStyle(string.Concat("W", row), string.Concat("W", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("X", row), string.Concat("X", row));
                                    //sl.SetCellValue(string.Concat("X", row), ans.ScoreOption8);
                                    //sl.SetCellStyle(string.Concat("X", row), string.Concat("X", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("Y", row), string.Concat("Y", row));
                                    //sl.SetCellValue(string.Concat("Y", row), ans.ScoreOption9);
                                    //sl.SetCellStyle(string.Concat("Y", row), string.Concat("Y", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                    //
                                    //sl.MergeWorksheetCells(string.Concat("Z", row), string.Concat("Z", row));
                                    //sl.SetCellValue(string.Concat("Z", row), ans.ScoreOption10);
                                    //sl.SetCellStyle(string.Concat("Z", row), string.Concat("Z", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));


                                    row++;
                                }
                            }

                        }

                    }
                    else
                    {
                        if (model.FileId.IsNotNullAndNotEmpty())
                        {
                            //var img = _fileBusiness.GetSingleById(model.FileId.Value);
                            //if (img != null)
                            //{
                            var bytes = await _fileBusiness.GetFileByte(model.FileId);
                            if (bytes.Count() > 0)
                            {
                                SLPicture pic = new SLPicture(bytes, DocumentFormat.OpenXml.Packaging.ImagePartType.Jpeg);
                                pic.SetPosition(3, 0);
                                pic.LockWithSheet = true;
                                pic.ResizeInPixels(800, 400);
                                sl.InsertPicture(pic);
                                sl.SetRowHeight(4, 250);
                                sl.MergeWorksheetCells(string.Concat("A", 4), string.Concat("J", 4));
                            }
                            //}
                        }

                        sl.MergeWorksheetCells(string.Concat("A", 5), string.Concat("J", 30));
                        sl.SetCellValue(string.Concat("A", 5), model.CaseStudyText);
                        sl.SetCellStyle(string.Concat("A", 5), string.Concat("J", 30), ExcelHelper.GetTextCaseStudyStyle(sl));

                        sl.SetCellStyle("A5", "J5", ExcelHelper.GetTopBorderStyle(sl));
                        sl.SetCellStyle("A5", "A30", ExcelHelper.GetLeftBorderStyle(sl));
                        sl.SetCellStyle("J5", "J30", ExcelHelper.GetRightBorderStyle(sl));
                        sl.SetCellStyle("A30", "J30", ExcelHelper.GetBottomBorderOnlyStyle(sl));
                    }

                }

                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);


            }


            ms.Position = 0;
            return ms;
        }

        public async Task<AssessmentReportViewModel> GetAssessmentReportexcel(string userId)
        {
            var model = new AssessmentReportViewModel();


            model.TechnicalAssessment = await GetAssessmentReporTechnical(userId);
            model.CaseStudyAssessment = await GetAssessmentReport(userId);

            if (model.CaseStudyAssessment != null && model.CaseStudyAssessment.AnswerList != null && model.CaseStudyAssessment.AnswerList.Count > 0)
            {
                model.CaseStudyText = model.CaseStudyAssessment.AnswerList.FirstOrDefault()?.Description;
                if (model.CaseStudyAssessment.PreferredLanguageCode == "ENGLISH")
                {
                    model.FileId = model.CaseStudyAssessment.AnswerList.FirstOrDefault()?.FileId;
                }
                else
                {
                    model.FileId = model.CaseStudyAssessment.AnswerList.FirstOrDefault()?.FileIdAr;
                    if (model.FileId.IsNullOrEmpty())
                    {
                        model.FileId = model.CaseStudyAssessment.AnswerList.FirstOrDefault()?.FileId;
                    }
                }


            }
            return model;
        }


        public async Task ChangeUserProfilePhoto(string photoId, string userId)
        {
            //var userb = _sp.GetService<IUserBusiness>();
            //var user = await userb.GetSingleById(userId);
            //if (user != null)
            //{
            //    user.PhotoId = photoId;
            //    await userb.Edit(user);
            //}

            var query = $@"update  public.""User"" set ""PhotoId""='{photoId}' where ""Id""='{userId}'";
            await _queryRepo1.ExecuteCommand(query, null);
        }



        public async Task<AssessmentInterviewViewModel> GetserviceScore(string ServiceID)
        {

            var Query = $@"select  u.""Id"" as Id,sr.""Id"" as ServiceId, sr.""ServiceSubject"" as Subject,'' as IsArchived, u.""Name""
as OwnerUserName, L.""Name"" as AssessmentStatus,u.""Id"" as OwnerUserId,
'AssessmentInterview' as AssessmentType,false as IsAssessmentStopped,
i.""ScheduledStartDate"" as ScheduledStartDate ,u.""Email"" as Email,t.""Name"" as PanelTeamName,'' as AssessmentInterviewUrl,I.""Score""
from public.""User"" as u
left join public.""TeamUser""  as tm on  u.""Id""=tm.""UserId"" and tm.""IsDeleted""=false  and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""Team""  as t on  t.""Id""=tm.""TeamId""  and t.""IsDeleted""=false  and t.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsService"" as sr on u.""Id"" =sr.""OwnerUserId"" and sr.""IsDeleted""=false  and sr.""CompanyId""='{_repo.UserContext.CompanyId}' and sr.""TemplateId"" =(select ""Id"" from public.""Template""  where ""Code""='TAS_Interview')
left join public.""NtsNote"" as n on n.""Id""=sr.""UdfNoteId"" and n.""IsDeleted""=false and i.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_AssessmentInterview"" as i on i.""NtsNoteId""=n.""Id"" and i.""IsDeleted""=false and i.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as L on L.""Id""=sr.""ServiceStatusId"" and L.""IsDeleted""=false  and L.""CompanyId""='{_repo.UserContext.CompanyId}'
where sr.""Id""='{ServiceID}' and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _queryAssInterView.ExecuteQuerySingle<AssessmentInterviewViewModel>(Query, null);
            return result;
        }

        public async Task<List<InterViewQuestionViewModel>> GetInterviewAssessmentQuestions(string userId, string Tableid)
        {


            var query = $@"select i.""SequenceOrder"",""Score"" from  cms.""N_TAS_InterviewQuestion"" as i
                         join public.""NtsNote"" as n on n.""Id""=i.""NtsNoteId"" and n.""ParentServiceId""='{Tableid}' and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'   
                        where i.""UserId""='{userId}' and  i.""IsDeleted""=false  and i.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var result = await _QueryInterviewquestion.ExecuteQueryList<InterViewQuestionViewModel>(query, null);
            return result;
        }



        public async Task<bool> CalculateQuestionaireResultScore(string UserId)
        {
            var questionaireResultServices = await GetAssessmentQuistionNaireResultByUser(UserId);
            var questionLists = await GetAssessmentReporTechnical(UserId);
            foreach (var questionaireResultService in questionaireResultServices)
            {

                //var technicalScore = questionaireResultService.Score;
                //if (technicalScore != null)
                //{
                double technicalScore = 0.0;
                double totalQuestions = 0.0;
                double correctAnswer = 0.0;
                var score = "0.0";
                var questionList = questionLists.Where(x => x.Id == questionaireResultService.Id).FirstOrDefault();
                if (questionList != null)
                {

                    totalQuestions = questionList.AnswerList.Count();
                    var maximumoption = questionList.MaximumOption ?? 3;
                    var rightAnsCount = questionList.AnswerList.Where(e => e.RightAnswer.IsNotNullAndNotEmpty()).Count();
                    if (rightAnsCount <= 0)
                    {
                        if (totalQuestions > 0)
                        {
                            var optionscoretotal = questionList.AnswerList.Sum(e => e.ScoreOption ?? 0);

                            score = ((optionscoretotal / (totalQuestions * maximumoption)) * 100).RoundPayrollSummaryAmount().ToString();
                            technicalScore = Convert.ToDouble(score);
                        }
                        else
                        {
                            technicalScore = 0.0;
                        }
                    }
                    else
                    {
                        if (totalQuestions > 0)
                        {
                            correctAnswer = questionList.AnswerList.Where(e => e.Accurate == BoolStatus.Yes).Count();

                            score = ((correctAnswer / totalQuestions) * 100).RoundPayrollSummaryAmount().ToString();
                            technicalScore = Convert.ToDouble(score);
                        }
                        else
                        {
                            technicalScore = 0.0;
                        }
                    }


                    await UpdateUserResult(questionaireResultService.ServiceId, technicalScore.ToString());
                }

            }

            // }

            return true;
        }


        public async Task<bool> CalculateCaseStudyResultScore(string userId)
        {
            var caseStudyResultService = await GetAssessmentCaseStudyResultByUser(userId);
            if (caseStudyResultService != null)
            {

                double caseStudyScore = 0.0;

                double totalCaseStudyQuestions = 0.0;
                double correctCaseStudyAnswer = 0.0;
                var score = "0.0";
                var caseStudyQuestionList = await GetAssessmentReport(userId);
                if (caseStudyQuestionList != null)
                {
                    totalCaseStudyQuestions = caseStudyQuestionList.AnswerList.Count();
                    if (totalCaseStudyQuestions > 0)
                    {
                        foreach (var item in caseStudyQuestionList.AnswerList)
                        {
                            if (item.Accurate == BoolStatus.Yes && item.Score.IsNotNull())
                            {
                                correctCaseStudyAnswer = correctCaseStudyAnswer + item.Score.Value;
                            }
                        }
                        score = ((correctCaseStudyAnswer / (totalCaseStudyQuestions * 4)) * 100).RoundPayrollSummaryAmount().ToString();
                        caseStudyScore = Convert.ToDouble(score);
                    }
                    else
                    {
                        caseStudyScore = 0.0;
                    }

                }
                else
                {
                    caseStudyScore = 0.0;
                }

                await UpdateUserResult(caseStudyResultService.ServiceId, caseStudyScore.ToString());
                //var cypher = @"match (s:NTS_Service{Id:{serviceId},IsDeleted:0}) 
                // optional match(s)<-[:R_ServiceFieldValue_Service]-(nfv1:NTS_ServiceFieldValue{IsDeleted:0})-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'score'})

                //set nfv1.Code={code},nfv1.Value={value} return 1";
                //var prms = new Dictionary<string, object>();
                //prms.Add("Status", StatusEnum.Active.ToString());
                //prms.Add("CompanyId", CompanyId);
                //prms.Add("serviceId", caseStudyResultService.Id);
                //prms.Add("code", score);
                //prms.Add("value", score);

                //ExecuteCypherList<long>(cypher, prms);

            }

            return true;
        }

        public async Task<List<AssessmentDetailViewModel>> GetAssessmentQuistionNaireResultByUser(string UserID)
        {
            var query = $@"select Ass.""AssessmentName"" as Title , Ass.""Id"",
R.""StartDate"" as ActualStartDate,R.""EndDate"" as ActualEndDate ,R.""Id"" as ""ServiceId""
from public.""NtsService"" as sr inner join public.""NtsNote"" N on sr.""UdfNoteId""=N.""Id""  and N.""IsDeleted""=false  and N.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join cms.""N_TAS_UserAssessmentResult"" R on N.""Id"" =  R.""NtsNoteId"" and R.""IsDeleted"" = false  and R.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_Assessment"" as Ass on Ass.""Id"" = R.""AssessmentId"" and Ass.""IsDeleted"" = false  and Ass.""CompanyId""='{_repo.UserContext.CompanyId}'
left join Public.""LOV"" as lov on lov.""Id"" = Ass.""AssessmentTypeId"" and lov.""IsDeleted"" = false  and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
where lov.""Code"" = 'TECHNICAL' and R.""UserId"" = '{UserID}' and sr.""IsDeleted""=false  and sr.""CompanyId""='{_repo.UserContext.CompanyId}'";


            var result = await _queryAssDetail.ExecuteQueryList<AssessmentDetailViewModel>(query, null);
            return result;

        }


        public async Task<AssessmentDetailViewModel> GetAssessmentCaseStudyResultByUser(string UserID)
        {
            var query = $@"select Ass.""AssessmentName"" as Title , Ass.""Id"",
R.""StartDate"" as ActualStartDate,R.""EndDate"" as ActualEndDate ,R.""Id"" as ""ServiceId""
from public.""NtsService"" as sr inner join public.""NtsNote"" N on sr.""UdfNoteId""=N.""Id"" and N.""IsDeleted""=false  and N.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join cms.""N_TAS_UserAssessmentResult"" R on N.""Id"" =  R.""NtsNoteId"" and R.""IsDeleted"" = false   and R.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_Assessment"" as Ass on Ass.""Id"" = R.""AssessmentId"" and Ass.""IsDeleted"" = false  and Ass.""CompanyId""='{_repo.UserContext.CompanyId}'
left join Public.""LOV"" as lov on lov.""Id"" = Ass.""AssessmentTypeId"" and lov.""IsDeleted"" = false   and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
where lov.""Code"" = 'CASE_STUDY' and R.""UserId"" = '{UserID}' and sr.""IsDeleted""=false  and sr.""CompanyId""='{_repo.UserContext.CompanyId}'";


            var result = await _queryAssDetail.ExecuteQuerySingle<AssessmentDetailViewModel>(query, null);
            return result;

        }


        public async Task<bool> UpdateUserResult(string Id, string Scores)
        {
            var query = $@"Update cms.""N_TAS_UserAssessmentResult"" set ""Score""='{Scores}' where ""Id""='{Id}' ";

            await _queryAssAns.ExecuteCommand(query, null);


            return true;

        }

        public async Task<bool> UpdateInterViewScore(string Id, string Scores, string ActualStartDate, string ActualEndDate)
        {
            var query = $@"Update cms.""N_TAS_AssessmentInterview"" set ""Score""='{Scores}',""ActualStartDate""='{ActualStartDate}',""ActualEndDate""='{ActualEndDate}' where ""Id""='{Id}' ";

            await _queryAssAns.ExecuteCommand(query, null);


            return true;

        }

        public async Task<IdNameViewModel> GetCanidateId(string UserId)
        {
            var Query = $@"select n.""Id"" from cms.""N_TAS_CandidateId"" as c inner join public.""NtsNote"" As n  on n.""Id""=c.""NtsNoteId"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
             where c.""UserId"" = '{UserId}' and c.""IsDeleted""=false  and c.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _idNameRepo.ExecuteQuerySingle<IdNameViewModel>(Query, null);
            return result;
        }

        public async Task<bool> UpdateAssessmentArchived(string serviceIds, bool archive = false)
        {
            var cypher = $@"Update Public.""NtsService"" as s set ""IsArchived""={archive} where s.""Id"" in ({serviceIds})";

            await _idNameRepo.ExecuteCommand(cypher, null);
            return true;
        }


        public async Task<IdNameViewModel> Getinterviewid(string ServiceId)
        {

            var query = $@"select I.* from public.""NtsService"" as sr
inner join public.""NtsNote"" as n on n.""Id""=sr.""UdfNoteId"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join cms.""N_TAS_AssessmentInterview"" as I on I.""NtsNoteId""=n.""Id"" and I.""IsDeleted""=false  and I.""CompanyId""='{_repo.UserContext.CompanyId}'
where sr.""Id""='{ServiceId}' and sr.""IsDeleted""=false  and sr.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var res = await _idNameRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return res;

        }

        public async Task<List<AssessmentCalendarViewModel>> GetCalendarScheduleList(string loggedInUserId = null, string role = null)
        {
            var query = $@"select uas.""Id"" as ""Id"", uas.""Status"" as ""Status"", uas.""AssessmentStartDate"" as ""Start"", uas.""EndDate"" as ""End"", uas.""JobTitle"" as ""JobTitle"",
                        uas.""InterviewerId"" as ""InterviewPanelId"", uas.""ProctorId"" as ""ProctorId"", uas.""Url"" as ""Url"", uas.""InterviewWeightage"" as ""InterviewWeightage"",uas.""AttachmentId"" as ""AttachmentId"",uas.""AssessmentId"",uas.""MonitoringTypeId"",
                        uas.""UserId"" as ""CandidateId"",u.""Email"" as ""Email"",u.""Name"" as ""CandidateName"", u.""Mobile"" as ""MobileNo"",
                        case when uas.""SlotType""='0' then 'Proctor' else 'Interviewer'end as ""Description"", uas.""IsAssessmentCreated"" as ""IsAssessmentCreated"",
                        uas.""AssessmentSetId"" as ""AssessmentSetId"", uas.""SlotType"" as ""SlotType"",'' as ""Remarks"", uas.""PreferedLanguageId"" as ""PreferredLanguageId"", lg.""Name"" as ""PreferredLanguageText"",
                        s.""Id"" as ""ServiceId"", s.""ServiceNo"" as ""ServiceNo"",a.""AssessmentName"" as AssessmentName,at.""Name"" as AssessmentTypeName,uas.""Location"" as ""Location""
                        from cms.""N_TAS_UserAssessmentSchedule"" as uas
                        join public.""User"" as u on u.""Id""=uas.""UserId"" and u.""IsDeleted""=false   and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join public.""NtsService"" as s on s.""UdfNoteTableId""=uas.""Id"" and s.""IsDeleted""=false   and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left join public.""LOV"" as lg on lg.""Id""=uas.""PreferedLanguageId"" and lg.""IsDeleted""=false  and lg.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left Join cms.""N_TAS_Assessment"" as a on a.""Id""=uas.""AssessmentId"" and a.""IsDeleted""=false  and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left Join public.""LOV"" as at on at.""Id""=a.""AssessmentTypeId"" and at.""IsDeleted""=false  and at.""CompanyId""='{_repo.UserContext.CompanyId}'

                        where uas.""IsDeleted""=false  and uas.""CompanyId""='{_repo.UserContext.CompanyId}' --#WHEREUSER#
                        ";
            //var whereuser = "";
            //if (loggedInUserId.IsNotNullAndNotEmpty())
            //{
            //    whereuser = $@" and u.""Id""='{loggedInUserId}' ";
            //}
            //query = query.Replace("#WHEREUSER#", whereuser);
            var result = await _queryRepo.ExecuteQueryList<AssessmentCalendarViewModel>(query, null);
            return result;
        }
        public async Task<AssessmentCalendarViewModel> GetCalendarScheduleById(string id)
        {
            var query = $@"select uas.""Id"" as ""Id"", uas.""Status"" as ""Status"", uas.""AssessmentStartDate"" as ""Start"", uas.""EndDate"" as ""End"", uas.""JobTitle"" as ""JobTitle"",
                        uas.""InterviewerId"" as ""InterviewPanelId"", uas.""ProctorId"" as ""ProctorId"", uas.""Url"" as ""Url"",uas.""InterviewWeightage"" as ""InterviewWeightage"", uas.""AttachmentId"" as ""AttachmentId"",uas.""AssessmentId"",uas.""MonitoringTypeId"",
                        uas.""UserId"" as ""CandidateId"",u.""Email"" as ""Email"",u.""Name"" as ""CandidateName"", u.""Mobile"" as ""MobileNo"",
                        case when uas.""SlotType""='0' then 'Proctor' else 'Interviewer'end as ""Description"", uas.""IsAssessmentCreated"" as ""IsAssessmentCreated"",
                        uas.""AssessmentSetId"" as ""AssessmentSetId"", uas.""SlotType"" as ""SlotType"",'' as ""Remarks"", uas.""PreferedLanguageId"" as ""PreferredLanguageId"",
                         lg.""Name"" as ""PreferredLanguageText"",
                        s.""Id"" as ""ServiceId"", s.""ServiceNo"" as ""ServiceNo""
                        from cms.""N_TAS_UserAssessmentSchedule"" as uas
                        join public.""User"" as u on u.""Id""=uas.""UserId"" and u.""IsDeleted""=false
                        join public.""NtsService"" as s on s.""UdfNoteTableId""=uas.""Id"" and s.""IsDeleted""=false
                        left join public.""LOV"" as lg on lg.""Id""=uas.""PreferedLanguageId"" and lg.""IsDeleted""=false
                        where uas.""IsDeleted""=false  and uas.""Id""='{id}'
                        ";
            var result = await _queryRepo.ExecuteQuerySingle<AssessmentCalendarViewModel>(query, null);
            return result;
        }
        public async Task<List<EmailViewModel>> GetSlotUserEmail(string slotId)
        {
            var query = $@"select e.*
                            FROM public.""Email"" as e
                            where e.""IsDeleted""=false and e.""ReferenceId""='{slotId}' ";
            var result = await _queryRepo.ExecuteQueryList<EmailViewModel>(query, null);
            return result.ToList();
        }
        public async Task<EmailViewModel> GetEmailById(string emailId)
        {
            var query = $@"select e.*
                            FROM public.""Email"" as e
                            where e.""IsDeleted""=false and e.""Id""='{emailId}' ";
            var result = await _queryRepo.ExecuteQuerySingle<EmailViewModel>(query, null);
            return result;
        }
        public async Task DeleteAssessmentSchedule(string id)
        {
            var _tableMetadataBusiness = _sp.GetService<ITableMetadataBusiness>();
            var _noteBusiness = _sp.GetService<INoteBusiness>();
            var _serviceBusiness = _sp.GetService<IServiceBusiness>();

            var res = await _tableMetadataBusiness.GetTableDataByColumn("TAS_USER_ASSESSMENT_SCHEDULE", "", "Id", id);
            if (res != null)
            {
                var query = $@"Update cms.""N_TAS_UserAssessmentSchedule"" set ""IsDeleted""=true where ""Id""='{id}'";
                await _queryAssDetail.ExecuteCommand(query, null);

                await _noteBusiness.Delete(res["NtsNoteId"].ToString());
                var ser = await _serviceBusiness.GetSingle(x => x.UdfNoteTableId == id);
                await _serviceBusiness.Delete(ser.Id);
            }
        }
        public async Task<AssessmentPDFReportViewModel> GetAssessmentReportDataPDFForUser(string userId)
        {
            //var _userbusiness = BusinessHelper.GetInstance<IUserBusiness>();
            AssessmentPDFReportViewModel model = new AssessmentPDFReportViewModel();
            //var TechnicalAssessment = await GetAssessmentReportByUserId(userId, "S_ASSESSMENT_QUISTIONNAIRE_RESULT");
            //var CaseStudyAssessment = await GetAssessmentReportByUserId(userId, "S_ASSESSMENT_CASESTUDY_RESULT");
            var TechnicalAssessment = await GetAssessmentReportByUserId(userId, "TECHNICAL");
            var CaseStudyAssessment = await GetAssessmentReportByUserId(userId, "CASE_STUDY");
            var InterviewAssessment = await GetInterviewAssessmentReportByUserId(userId);
            model.CaseAnalysisScore = 0.0;
            if (userId.IsNotNullAndNotEmpty())
            {
                var user = await _userBusiness.GetSingleById(userId);
                if (user != null)
                {
                    model.UserName = user.Name;
                    model.JobName = user.JobTitle;
                }
            }
            if (TechnicalAssessment != null)
            {
                model.JobName = TechnicalAssessment.JobTitle;
                //model.UserName = TechnicalAssessment.OwnerUserName;
                model.TechnicalQuestionCount = TechnicalAssessment.TotalCount.ToString();
                model.TechnicalQuestionnaireScore = TechnicalAssessment.Score ?? 0.0;
                model.AssessmentTechnicalDuratrion = TechnicalAssessment.AssessmentDuration;
                model.TechnicalWeightage = TechnicalAssessment.Weightage;
            }
            if (CaseStudyAssessment != null)
            {
                model.CaseStudyQuestionCount = CaseStudyAssessment.TotalCount.ToString();
                model.CaseStudyWeightage = CaseStudyAssessment.Weightage;
                model.CaseAnalysisScore = CaseStudyAssessment.Score ?? 0.0;
                model.AssessmentCaseStudyDuratrion = CaseStudyAssessment.AssessmentDuration;
            }
            if (InterviewAssessment != null)
            {
                model.TechnicalInterviewScore = InterviewAssessment.Score ?? 0.0;
                model.InterviewWeightage = InterviewAssessment.AssessmentInterviewWeightage;
                model.AssessmentInterviewDuratrion = InterviewAssessment.AssessmentInterviewDuratrion;
            }
            return model;
        }
        public async Task<AssessmentViewModel> GetAssessmentReportByUserId(string userId, string masterCode)
        {
            //var cypher = @"match (u:ADM_User {Id:{UserId},IsDeleted:0})<-[:R_Service_Owner_User]-(rs:NTS_Service{IsDeleted:0})
            //-[:R_Service_Template]-(t:NTS_Template{IsDeleted:0})-[:R_TemplateRoot]-(tm:NTS_TemplateMaster{IsDeleted:0,Code:{MasterCode}})
            // optional match(rs)-[:R_Service_Reference]->(s:NTS_Service{IsDeleted:0})
            //  optional match(s)<-[:R_Note_Reference]-(n:NTS_Note{IsDeleted:0})
            // optional match(s)<-[:R_ServiceFieldValue_Service]-(nfv3: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(tf3:NTS_TemplateField{FieldName:'assessmentDuration'})  
            //   optional match(s)<-[:R_ServiceFieldValue_Service]-(nfv4: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(tf4:NTS_TemplateField{FieldName:'weightage'}) 
            //optional match(s)<-[:R_ServiceFieldValue_Service]-(nfv5: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(tf5:NTS_TemplateField{FieldName:'jobName'})
            // optional match(rs)<-[:R_ServiceFieldValue_Service]-(nfv6: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(tf6:NTS_TemplateField{FieldName:'score'})
            //return u.JobTitle as JobTitle,count(n) as TotalCount,toFloat(nfv4.Code) as Weightage,nfv4.Code as AssessmentDuration,u.UserName as OwnerUserName,toFloat(nfv6.Code) as Score";
            //var prms = new Dictionary<string, object>
            //{
            //    { "UserId", userId },
            //    { "MasterCode", masterCode }

            //};
            //var assessment = ExecuteCypher<AssessmentViewModel>(cypher, prms);
            //return assessment;

            //var query = $@" select u.""JobTitle"" as ""JobTitle"", u.""Name"" as ""OwnerUserName"", a.""Weightage"" as ""Weightage""
            //            , a.""AssessmentDuration"" as ""AssessmentDuration"", uar.""Score"" as ""Score""
            //            from public.""User"" as u
            //            join public.""NtsService"" as rs on rs.""OwnerUserId""=u.""Id"" and rs.""IsDeleted""=false
            //            join cms.""N_TAS_UserAssessmentResult"" as uar on uar.""NtsNoteId""=rs.""UdfNoteId"" and uar.""IsDeleted""=false
            //            join cms.""N_TAS_Assessment"" as a on a.""Id""=uar.""AssessmentId"" and a.""IsDeleted""=false
            //            join public.""LOV"" as lov on lov.""Id""=a.""AssessmentTypeId"" and lov.""IsDeleted""=false
            //            left join public.""NtsService"" as s on s.""Id""=rs.""ParentServiceId"" and s.""IsDeleted""=false
            //            left join cms.""N_TAS_UserAssessmentAnswer"" as uaa on uaa.""NtsNoteId""=s.""UdfNoteId"" and uaa.""IsDeleted""=false
            //            where u.""IsDeleted""=false and u.""Id""='{userId}' and lov.""Code""='{masterCode}' ";


            var query = $@" select u.""JobTitle"" as ""JobTitle"", u.""Name"" as ""OwnerUserName"", a.""Weightage"" as ""Weightage""
                        , a.""AssessmentDuration"" as ""AssessmentDuration"", uar.""Score"" as ""Score""
                        , count(uaa.""Id"") ""TotalCount""
                        from public.""User"" as u
                        join cms.""N_TAS_UserAssessmentResult"" as uar on uar.""UserId""=u.""Id"" and uar.""IsDeleted""=false
                        join cms.""N_TAS_Assessment"" as a on a.""Id""=uar.""AssessmentId"" and a.""IsDeleted""=false
                        join public.""LOV"" as lov on lov.""Id""=a.""AssessmentTypeId"" and lov.""IsDeleted""=false
                        left join cms.""N_TAS_UserAssessmentAnswer"" as uaa on uaa.""UserId""=u.""Id"" and uaa.""AssessmentId""=a.""Id"" and uaa.""IsDeleted""=false
                        where u.""IsDeleted""=false and u.""Id""='{userId}' and lov.""Code""='{masterCode}' 
                        group by u.""JobTitle"",u.""Name"",a.""Weightage"",a.""AssessmentDuration"",uar.""Score"" ";

            var result = await _queryRepo.ExecuteQuerySingle<AssessmentViewModel>(query, null);
            return result;
        }
        public async Task<AssessmentDetailViewModel> GetInterviewAssessmentReportByUserId(string userId)
        {
            //            Dictionary<string, object> parameters = new Dictionary<string, object>();
            //            parameters.Add("Status", StatusEnum.Active);
            //            parameters.Add("CompanyId", CompanyId);
            //            parameters.Add("UserId", userId);
            //            var match = string.Concat(@"match (u:ADM_User{Id:{UserId}})-[r:R_User_Interview_Service]->(s:NTS_Service{IsDeleted: 0,Status:{Status}})
            //            optional match (s)-[:R_Service_Status_ListOfValue]->(ns: GEN_ListOfValue{ IsDeleted: 0})
            //optional match(s)<-[:R_ServiceFieldValue_Service]-(nfv1: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(tf1:NTS_TemplateField{FieldName:'score'})
            //            return u.AssessmentInterviewDuratrion as AssessmentInterviewDuratrion,u.AssessmentInterviewWeightage as AssessmentInterviewWeightage,toFloat(nfv1.Code) as Score,u.JobTitle as JobTitle");
            //            var result = _repository.ExecuteCypher<AssessmentDetailViewModel>(match, parameters);
            //            return result;

            var query = $@" Select u.""Name"" as ""OwnerUserName"",u.""JobTitle"" as ""JobTitle"", ai.""Weightage"" as ""AssessmentInterviewWeightage"", ai.""Score"" as ""Score""
                            From public.""User"" as u
                            join public.""NtsService"" as s on s.""OwnerUserId"" = u.""Id"" and s.""IsDeleted""=false
                            join cms.""N_TAS_AssessmentInterview"" as ai on ai.""NtsNoteId""=s.""UdfNoteId"" and ai.""IsDeleted""=false
                            left join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false
                            Where u.""IsDeleted""=false and u.""Id""='{userId}' ";
            var result = await _queryRepo.ExecuteQuerySingle<AssessmentDetailViewModel>(query, null);
            return result;

        }
        public async Task<AssessmentResultViewModel> GetManageAssessmentResult(string userId)
        {
            //var leb = BusinessHelper.GetInstance<ILegalEntityBusiness>();
            //var le = leb.GetDefaultLegalEntityByUser(userId);
            //var assessmentReportSummary = "";
            //var reportColor = "blue";
            //if (le != null)
            //{
            //    assessmentReportSummary = le.AssessmentReportSummary;
            //    reportColor = le.AssessmentReportColor;
            //}
            //var prms = new Dictionary<string, object>
            //{
            //        { "userId",userId},
            //        { "assessmentReportSummary",assessmentReportSummary},
            //        { "reportColor",reportColor},

            //};
            //var cypher = @"match (u:ADM_User{Id:{userId}}) 
            // return u.Id as UserId,u.UserName as OwnerUserName,u.JobTitle as JobTitle";
            //var data = ExecuteCypher<AssessmentResultViewModel>(cypher, prms);

            var query = $@" Select u.""Id"" as ""UserId"", u.""Name"" as ""OwnerUserName"",u.""JobTitle"" as ""JobName"",c.""Name"" as ""OrganizationName""
                            from public.""User"" as u
                            left join public.""Company"" as c on c.""Id""=u.""CompanyId"" and c.""IsDeleted""=false
                            where u.""IsDeleted""=false and u.""Id""='{userId}' ";
            var data = await _queryRepo.ExecuteQuerySingle<AssessmentResultViewModel>(query, null);

            if (data != null)
            {
                data.Type = "";
                var assessmentlist = await GetManageAssessmentResultList(userId);
                var assessments = assessmentlist.Where(x => (x.IncludeInReport == null || x.IncludeInReport == true)).ToList();
                if (assessments.Count > 0)
                {
                    var a = assessments.FirstOrDefault();
                    if (a.ReportSummary.IsNotNullAndNotEmpty())
                    {
                        data.ReportSummary = a.ReportSummary;
                    }
                    data.Type = a.Type;
                }
                //if (data.ReportSummary.IsNullOrEmpty())
                //{
                //    data.ReportSummary = assessmentReportSummary;
                //}
                //data.ReportColor = reportColor;
                data.Score = (assessments.Sum(x => x.ReportScore * (x.ReportWeightage / 100.0)) ?? 0).RoundToTwoDecimal();
            }
            //var data = new AssessmentResultViewModel();
            return data;
        }
        public async Task<List<AssessmentResultViewModel>> GetManageAssessmentResultList(string userId)
        {

            //var cypher = @"match (u:ADM_User {Id:{UserId},IsDeleted:0})<-[:R_Service_Owner_User]-(rs:NTS_Service{IsDeleted:0})
            //-[:R_Service_Template]-(t:NTS_Template{IsDeleted:0})-[:R_TemplateRoot]-(tm:NTS_TemplateMaster{IsDeleted:0})
            // match(rs)-[:R_Service_Reference]->(s:NTS_Service{IsDeleted:0})
            // where tm.Code in ['S_ASSESSMENT_QUISTIONNAIRE_RESULT','S_ASSESSMENT_CASESTUDY_RESULT']
            // optional match(s)<-[:R_ServiceFieldValue_Service]-(wt: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'weightage'}) 
            // optional match(s)<-[:R_ServiceFieldValue_Service]-(agn: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'assessmentGroupName'}) 
            // optional match(rs)<-[:R_ServiceFieldValue_Service]-(sc: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'score'})
            // optional match(rs)<-[:R_ServiceFieldValue_Service]-(rgn: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'reportGroupName'})
            // optional match(rs)<-[:R_ServiceFieldValue_Service]-(rn: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'reportName'})
            // optional match(rs)<-[:R_ServiceFieldValue_Service]-(rwt: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'reportWeightage'})
            // optional match(rs)<-[:R_ServiceFieldValue_Service]-(rsc: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'reportScore'})
            // optional match(rs)<-[:R_ServiceFieldValue_Service]-(iir: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'includeInReport'})
            // optional match(rs)<-[:R_ServiceFieldValue_Service]-(ars: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'assessmentReportSummary'}) 
            // return rs.Id as Id,COALESCE(rn.Code,s.Subject) as ReportName ,COALESCE(rgn.Code,agn.Code) as ReportGroupName 
            // ,COALESCE(case when rwt.Code ='' then null else rwt.Code end,case when wt.Code ='' then null else wt.Code end,'20') as ReportWeightage
            // ,COALESCE(sc.Code) as ReportScore ,iir.Code as IncludeInReport,ars.Code as ReportSummary,'Assessment' as Type
            // union 
            // match (u:ADM_User {Id:{UserId},IsDeleted:0})<-[:R_Service_Owner_User]-(rs:NTS_Service{IsDeleted:0})
            //-[:R_Service_Template]-(t:NTS_Template{IsDeleted:0})-[:R_TemplateRoot]-(tm:NTS_TemplateMaster{IsDeleted:0})
            // where tm.Code in ['S_ASSESSMENT_INTERVIEW']
            // optional match(rs)<-[:R_ServiceFieldValue_Service]-(wt: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'weightage'}) 
            // optional match(rs)<-[:R_ServiceFieldValue_Service]-(agn: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'assessmentGroupName'}) 
            // optional match(rs)<-[:R_ServiceFieldValue_Service]-(sc: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'score'})
            // optional match(rs)<-[:R_ServiceFieldValue_Service]-(rgn: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'reportGroupName'})
            // optional match(rs)<-[:R_ServiceFieldValue_Service]-(rn: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'reportName'})
            // optional match(rs)<-[:R_ServiceFieldValue_Service]-(rwt: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'reportWeightage'})
            // optional match(rs)<-[:R_ServiceFieldValue_Service]-(rsc: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'reportScore'})
            // optional match(rs)<-[:R_ServiceFieldValue_Service]-(iir: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'includeInReport'})
            // optional match(rs)<-[:R_ServiceFieldValue_Service]-(ars: NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(:NTS_TemplateField{FieldName:'assessmentReportSummary'}) 
            // return rs.Id as Id,COALESCE(rn.Code,tm.Name) as ReportName ,COALESCE(rgn.Code,agn.Code,tm.Description) as ReportGroupName 
            //,COALESCE(case when rwt.Code ='' then null else rwt.Code end,wt.Code,'60') as ReportWeightage
            // ,COALESCE(sc.Code) as ReportScore ,iir.Code as IncludeInReport,ars.Code as ReportSummary,'Interview' as Type";
            //var prms = new Dictionary<string, object>
            //{
            //    { "UserId", userId }

            //};
            //var assessment = ExecuteCypherList<AssessmentResultViewModel>(cypher, prms).ToList();
            var query = $@"select rs.""Id"" as ""Id"",coalesce(uar.""ReportName"",rs.""ServiceSubject"") as ""ReportName"", uar.""ReportGroupName"" as ""ReportGroupName"",
                        COALESCE(case when uar.""ReportWeightage"" = '' then null else uar.""ReportWeightage"" end,case when a.""Weightage"" = '' then null else a.""Weightage"" end,'20') as ""ReportWeightage"",
                        COALESCE(uar.""Score"",'0') as ""ReportScore"", uar.""IncludeInReport"" as ""IncludeInReport"", uar.""AssessmentReportSummary"" as ""ReportSummary"", 'Assessment' as ""Type""
                        from public.""User"" as u
                        join public.""NtsService"" as rs on rs.""OwnerUserId""=u.""Id"" and rs.""IsDeleted""=false
                        join cms.""N_TAS_UserAssessmentResult"" as uar on uar.""NtsNoteId""=rs.""UdfNoteId"" and uar.""IsDeleted""=false
                        join cms.""N_TAS_Assessment"" as a on a.""Id""=uar.""AssessmentId"" and a.""IsDeleted""=false
                        join public.""LOV"" as lov on lov.""Id""=a.""AssessmentTypeId"" and lov.""IsDeleted""=false
                        left join public.""NtsService"" as s on s.""Id""=rs.""ParentServiceId"" and s.""IsDeleted""=false
                        left join cms.""N_TAS_UserAssessmentAnswer"" as uaa on uaa.""NtsNoteId""=s.""UdfNoteId"" and uaa.""IsDeleted""=false
                        where u.""IsDeleted""=false and u.""Id""='{userId}' and lov.""Code"" IN ('TECHNICAL','CASE_STUDY')

                        union
                        select rs.""Id"" as ""Id"",coalesce(uar.""ReportName"",rs.""ServiceSubject"") as ""ReportName"", uar.""ReportGroupName"" as ""ReportGroupName"",
                        COALESCE(case when uar.""ReportWeightage"" = '' then null else uar.""ReportWeightage"" end,'60') as ""ReportWeightage"", coalesce(uar.""Score"",'0') as ""ReportScore"",
                        uar.""IncludeInReport"" as ""IncludeInReport"", uar.""AssessmentReportSummary"" as ""ReportSummary"",'Interview' as ""Type""
                        from public.""User"" as u
                        join public.""NtsService"" as rs on rs.""OwnerUserId""=u.""Id"" and rs.""IsDeleted""=false
                        join cms.""N_TAS_AssessmentInterview"" as uar on uar.""NtsNoteId""=rs.""UdfNoteId"" and uar.""IsDeleted""=false
                        where u.""IsDeleted""=false and u.""Id""='{userId}'
                        ";

            var assessment = await _queryRepo.ExecuteQueryList<AssessmentResultViewModel>(query, null);
            return assessment.ToList();
        }


        public async Task<List<AssessmentViewModel>> GetAssessmentSetList()
        {
            var Query = @$"select a.*,a.""NtsNoteId"" as NoteId, s.""ServiceNo"",s.""TemplateCode"",s.""Id"" as ServiceId, ss.""Code"" as ServiceStatusCode, u.""Name"" as CreatedBy
							from cms.""N_TAS_AssessmentSet"" as a
							join public.""NtsNote"" as n on a.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
							join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
							
							join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_repo.UserContext.CompanyId}'
							join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
							where a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'   order by a.""CreatedDate"" desc ";

            var Result = await _queryRepo.ExecuteQueryList<AssessmentViewModel>(Query, null);
            return Result;

        }

        public async Task<List<AssessmentViewModel>> GetAssessmentSetAssessmentList(string AssessmentSetId)
        {
            var Query = @$"select Asa.""Id"",Asan.""Id"" as ""NoteId"",  LOV.""Name"" as ""AssessmentType"",a.""AssessmentName"" ,Asa.""SequenceOrder"" from  cms.""N_TAS_Assessment"" as a
                            join public.""NtsNote"" as n on a.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false  and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_TAS_AssessmentSetAssessment"" as Asa on Asa.""AssessmentId""=a.""Id"" and Asa.""IsDeleted""=false  and Asa.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join public.""NtsNote"" as Asan on Asa.""NtsNoteId""=Asan.""Id"" and Asan.""IsDeleted""=false  and Asan.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_TAS_AssessmentSet"" as Aset on Aset.""Id""=Asa.""AssessmentSetId"" and Aset.""IsDeleted""=false and Aset.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join public.""LOV"" as LOV on a.""AssessmentTypeId""=LOV.""Id"" and LOV.""IsDeleted""=false and LOV.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where Asa.""AssessmentSetId""='{AssessmentSetId}' and a.""IsDeleted""=false  and a.""CompanyId""='{_repo.UserContext.CompanyId}' order by Asa.""SequenceOrder""";

            var Result = await _queryRepo.ExecuteQueryList<AssessmentViewModel>(Query, null);
            return Result;

        }

        public async Task<bool> DeleteAssessmentSet(string NoteId)
        {
            var note = await GetSingleById(NoteId);
            if (note != null)
            {
                var query = $@"Update cms.""N_TAS_AssessmentSet"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
                await _queryRepo.ExecuteCommand(query, null);

                await Delete(NoteId);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAssessmentSetAsessment(string NoteId)
        {
            var note = await GetSingleById(NoteId);
            if (note != null)
            {
                var query = $@"Update cms.""N_TAS_AssessmentSetAssessment"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
                await _queryRepo.ExecuteCommand(query, null);

                await Delete(NoteId);
                return true;
            }
            return false;
        }

        public async Task<List<IdNameViewModel>> GetAssessmentListToSet(string TypeId)
        {

            var Query = @$"select a.""Id"",a.""AssessmentName"" as Name
							from cms.""N_TAS_Assessment"" as a
							join public.""NtsNote"" as n on a.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
							join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
							
							join public.""LOV"" as ss on s.""ServiceStatusId""=ss.""Id"" and ss.""IsDeleted""=false and ss.""CompanyId""='{_repo.UserContext.CompanyId}'
							join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
							where a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}' and  a.""AssessmentTypeId""='{TypeId}'";

            var Result = await _idNameRepo.ExecuteQueryList<IdNameViewModel>(Query, null);
            return Result;

        }


        public async Task<AssessmentViewModel> GetAssessmentSetAssessmentByid(string NoteId)
        {
            var Query = @$"select Asa.""Id"",Asan.""Id"" as ""NoteId"",  LOV.""Id"" as ""AssessmentType"",a.""Id"" as AssessmentName ,Asa.""SequenceOrder"" from  cms.""N_TAS_Assessment"" as a
                            join public.""NtsNote"" as n on a.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false  and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_TAS_AssessmentSetAssessment"" as Asa on Asa.""AssessmentId""=a.""Id"" and Asa.""IsDeleted""=false  and Asa.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join public.""NtsNote"" as Asan on Asa.""NtsNoteId""=Asan.""Id"" and Asan.""IsDeleted""=false  and Asan.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_TAS_AssessmentSet"" as Aset on Aset.""Id""=Asa.""AssessmentSetId"" and Aset.""IsDeleted""=false and Aset.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join public.""LOV"" as LOV on a.""AssessmentTypeId""=LOV.""Id"" and LOV.""IsDeleted""=false and LOV.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where Asa.""NtsNoteId""='{NoteId}' and a.""IsDeleted""=false  and a.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var Result = await _queryRepo.ExecuteQuerySingle<AssessmentViewModel>(Query, null);
            return Result;

        }

        public async Task<IdNameViewModel> GetSquenceNoExistAssessmentSet(string AssessmentSetId, string SequenceNo)
        {
            var Query = $@"select ""Id"" from cms.""N_TAS_AssessmentSetAssessment"" where ""AssessmentSetId""='{AssessmentSetId}' and  ""SequenceOrder""='{SequenceNo}' 
                            and ""IsDeleted"" = false  and ""CompanyId"" = '{_repo.UserContext.CompanyId}'";

            var Result = await _idNameRepo.ExecuteQuerySingle<IdNameViewModel>(Query, null);
            return Result;
        }

        public async Task<IdNameViewModel> GetAssessmentExistAssessmentSet(string AssessmentSetId, string AssessmentId, string Id)
        {
            var Query = $@"select ""Id"" from cms.""N_TAS_AssessmentSetAssessment"" where ""AssessmentSetId""='{AssessmentSetId}' and  ""AssessmentId""='{AssessmentId}' 
                            and ""IsDeleted"" = false  and ""CompanyId"" = '{_repo.UserContext.CompanyId}' #Id#";

            if (Id.IsNotNull())
            {
                var vl = $@" and ""Id""!='{Id}'";
                Query = Query.Replace("#Id#", vl);
            }
            else { Query = Query.Replace("#Id#", ""); }

            var Result = await _idNameRepo.ExecuteQuerySingle<IdNameViewModel>(Query, null);
            return Result;
        }


        public async Task<List<AssessmentInterviewViewModel>> GetAssessmentReportMinistryList(string SponsorId = null, string userId = null)
        {
            var Query = "";

            //                 Query = $@"Select U.""Id"",U.""Name"",sr.""Id"" as ServiceId,sr.""ServiceSubject"" as Subject,u.""Name"" as OwnerUserName,u.""Id"" as OwnerUserId,
            //'AssessmentInterview' as AssessmentType,LOVI.""Name"" as AssessmentStatus,false as IsAssessmentStopped,u.""JobTitle"" as MinistryName,
            //i.""ScheduledStartDate"" as ScheduledStartDate,u.""Email"" as Email,i.""ScheduledEndDate"" as ScheduledEndDate,
            //i.""ActualStartDate"" as ActualStartDate, i.""ActualEndDate"" as ActualEndDate, true as IsAdmin,
            //u.""Mobile"" as Mobile,lov1.""Name"" as CSStatus,lov3.""Name"" as MCQStatus, u.""PhotoId""
            //from Public.""User"" u
            //inner join ""NtsService"" as sr on sr.""OwnerUserId"" = u.""Id"" and sr.""IsDeleted"" = false and sr.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            //inner join ""NtsNote"" as n on n.""Id"" = sr.""UdfNoteId""  and n.""IsDeleted"" = false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            //inner join cms.""N_TAS_AssessmentInterview"" as i on i.""NtsNoteId"" = n.""Id"" and i.""IsDeleted"" = false and i.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            //left join public.""LOV"" as LOVI on LOVI.""Id""=sr.""ServiceStatusId"" and LOVI.""IsDeleted""=false and LOVI.""CompanyId""='{_repo.UserContext.CompanyId}'
            //left join  cms.""N_TAS_UserAssessmentResult"" as r1 on r1.""UserId""=sr.""OwnerUserId"" and r1.""IsDeleted""=false and r1.""CompanyId""='{_repo.UserContext.CompanyId}'
            //left join cms.""N_TAS_Assessment"" as Ass on Ass.""Id""=r1.""AssessmentId"" and Ass.""IsDeleted""=false and Ass.""CompanyId""='{_repo.UserContext.CompanyId}'
            //left join public.""NtsNote""  as n1 on n1.""Id""=Ass.""NtsNoteId"" and n1.""IsDeleted""=false and n1.""CompanyId""='{_repo.UserContext.CompanyId}'
            //left join public.""NtsService"" as sr1 on sr1.""UdfNoteId""=n1.""Id"" and sr.""IsDeleted""=false and sr.""CompanyId""='{_repo.UserContext.CompanyId}'
            //left join public.""LOV"" as lov2 on lov2.""Id""=Ass.""AssessmentTypeId""  and lov2.""IsDeleted""=false and lov2.""CompanyId""='{_repo.UserContext.CompanyId}'
            //left join public.""LOV"" as lov1 on lov1.""Id""=sr1.""ServiceStatusId"" and lov2.""Code"" ='CASE_STUDY' and lov1.""IsDeleted""=false  and lov1.""CompanyId""='{_repo.UserContext.CompanyId}'
            //left join public.""LOV"" as lov3 on lov3.""Id""=sr1.""ServiceStatusId"" and lov2.""Code"" !='CASE_STUDY' and lov3.""IsDeleted""=false  and lov3.""CompanyId""='{_repo.UserContext.CompanyId}'
            //left join public.""NtsNote""  as n2 on n2.""Id""=r1.""NtsNoteId""  and n2.""IsDeleted""=false  and n2.""CompanyId""='{_repo.UserContext.CompanyId}'
            //left join public.""NtsService"" as sr2 on sr2.""UdfNoteId""=n2.""Id""  and sr2.""IsDeleted""=false  and sr2.""CompanyId""='{_repo.UserContext.CompanyId}'
            //left join public.""LOV"" as lovassR on lovassR.""Id""=sr2.""ServiceStatusId"" and lovassR.""IsDeleted""=false  and lovassR.""CompanyId""='{_repo.UserContext.CompanyId}'
            //where LOVI.""Code""='SERVICE_STATUS_COMPLETE' and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}' #SponsorId# #USERWHERE# ";

            Query = $@"Select U.""Id"",U.""Name"",sr.""Id"" as ServiceId,sr.""ServiceSubject"" as Subject,u.""Name"" as OwnerUserName,u.""Id"" as OwnerUserId,
'AssessmentInterview' as AssessmentType,LOVI.""Name"" as AssessmentStatus,false as IsAssessmentStopped,u.""JobTitle"" as MinistryName,
i.""ScheduledStartDate"" as ScheduledStartDate,u.""Email"" as Email,i.""ScheduledEndDate"" as ScheduledEndDate,
i.""ActualStartDate"" as ActualStartDate, i.""ActualEndDate"" as ActualEndDate, true as IsAdmin,
pl.""Name"" as PreferredLanguage,uas.""Url"" as AssessmentZoomUrl,uas.""Url"" as CaseStudyZoomUrl,uas.""Url"" as AssessmentInterviewUrl,
u.""Mobile"" as Mobile, u.""PhotoId""
,(select string_agg(arss.""Name""::text, ',') from cms.""N_TAS_Assessment"" as a
Join public.""LOV"" as at on a.""AssessmentTypeId""=at.""Id"" and at.""Code""='CASE_STUDY' and at.""IsDeleted""=false and at.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_TAS_UserAssessmentResult"" as ar on a.""Id""=ar.""AssessmentId"" and ar.""IsDeleted""=false and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsNote"" as arn on ar.""NtsNoteId""=arn.""Id"" and arn.""IsDeleted""=false and arn.""CompanyId""='{_repo.UserContext.CompanyId}'
left Join public.""NtsService"" as ars on arn.""Id""=ars.""UdfNoteId"" and ars.""IsDeleted""=false and ars.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as arss on ars.""ServiceStatusId""=arss.""Id"" and arss.""IsDeleted""=false and arss.""CompanyId""='{_repo.UserContext.CompanyId}'
where ar.""UserId""=u.""Id"" and a.""IsDeleted""=false  and a.""CompanyId""='{_repo.UserContext.CompanyId}'
) as CSStatus
,(select string_agg(arss.""Name""::text, ',') from cms.""N_TAS_Assessment"" as a
Join public.""LOV"" as at on a.""AssessmentTypeId""=at.""Id"" and at.""Code""!='CASE_STUDY' and at.""IsDeleted""=false and at.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_TAS_UserAssessmentResult"" as ar on a.""Id""=ar.""AssessmentId"" and ar.""IsDeleted""=false and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsNote"" as arn on ar.""NtsNoteId""=arn.""Id"" and arn.""IsDeleted""=false and arn.""CompanyId""='{_repo.UserContext.CompanyId}'
left Join public.""NtsService"" as ars on arn.""Id""=ars.""UdfNoteId"" and ars.""IsDeleted""=false and ars.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as arss on ars.""ServiceStatusId""=arss.""Id"" and arss.""IsDeleted""=false and arss.""CompanyId""='{_repo.UserContext.CompanyId}'
where ar.""UserId""=u.""Id"" and a.""IsDeleted""=false  and a.""CompanyId""='{_repo.UserContext.CompanyId}'
) as MCQStatus
from Public.""User"" u
inner join ""NtsService"" as sr on sr.""OwnerUserId"" = u.""Id"" and sr.""IsDeleted"" = false and sr.""CompanyId"" = '{_repo.UserContext.CompanyId}'
inner join ""NtsNote"" as n on n.""Id"" = sr.""UdfNoteId""  and n.""IsDeleted"" = false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
inner join cms.""N_TAS_AssessmentInterview"" as i on i.""NtsNoteId"" = n.""Id"" and i.""IsDeleted"" = false and i.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOVI on LOVI.""Id""=sr.""ServiceStatusId"" and LOVI.""IsDeleted""=false and LOVI.""CompanyId""='{_repo.UserContext.CompanyId}'

left join cms.""N_TAS_UserAssessmentSchedule"" as uas on uas.""Id""=i.""InterviewScheduleId"" and uas.""UserId""=u.""Id"" and uas.""IsDeleted""=false and uas.""CompanyId""='{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as pl on pl.""Id""=uas.""PreferedLanguageId""  and pl.""IsDeleted""=false  and pl.""CompanyId""='{_repo.UserContext.CompanyId}'

where LOVI.""Code""='SERVICE_STATUS_COMPLETE' and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}' #SponsorId# #USERWHERE# ";

            if (_repo.UserContext.IsSystemAdmin)
            {
                Query = Query.Replace("#SponsorId#", "");
            }
            else if (SponsorId.IsNotNull())
            {
                var qr = $@" and u.""SponsorId""='{SponsorId}'";
                Query = Query.Replace("#SponsorId#", qr);
            }
            var userwhere = "";
            if (userId.IsNotNullAndNotEmpty())
            {
                userwhere = $@" and u.""Id""='{userId}' ";
            }
            Query = Query.Replace("#USERWHERE#", userwhere);

            var Result = await _queryAssInterView.ExecuteQueryList<AssessmentInterviewViewModel>(Query, null);
            return Result;

        }

        public async Task<IdNameViewModel> GetSponsorDetailsByUserId(string UserId)
        {
            var Query = $@"Select ""SponsorId"" as Id,""Name"" from  public.""User"" where ""Id""='{UserId}'";
            var result = await _idNameRepo.ExecuteQuerySingle<IdNameViewModel>(Query, null);
            return result;
        }
        public async Task<List<AssessmentDetailViewModel>> GetAssessmentListByUserId(string userId, string statusCode, string typeId)
        {
            var query = $@"Select s.*,ars.""Id"" as ServiceId, a.""Id"" as AssessmentId,a.""AssessmentName"",udf.""AssessmentStartDate"" as ScheduledStartDate,udf.""EndDate"" as ScheduledEndDate
,at.""Name"" as AssessmentType, ar.""StartDate"" as ActualStartDate,ar.""EndDate"" as ActualEndDate,pl.""Code"" as PreferredLanguageCode, arss.""Name"" as AssessmentStatus, arss.""Code"", ar.""IsAssessmentStopped""
from public.""NtsService"" as s
Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""Code""='TAS_USER_ASSESSMENT_SCHEDULE' and t.""IsDeleted""=false  and t.""CompanyId""='{_repo.UserContext.CompanyId}'
Join public.""NtsNote"" as nts on s.""UdfNoteId""=nts.""Id"" and nts.""IsDeleted""=false  and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_UserAssessmentSchedule"" as udf on nts.""Id""=udf.""NtsNoteId"" and udf.""UserId""='{userId}' and udf.""IsDeleted""=false  and udf.""CompanyId""='{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as pl on udf.""PreferedLanguageId""=pl.""Id""  and pl.""IsDeleted""=false  and pl.""CompanyId""='{_repo.UserContext.CompanyId}'
left Join cms.""N_TAS_Assessment"" as a on udf.""AssessmentId""=a.""Id""  and a.""IsDeleted""=false  and a.""CompanyId""='{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as at on a.""AssessmentTypeId""=at.""Id"" and at.""Id""='{typeId}' and at.""IsDeleted""=false  and at.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAS_UserAssessmentResult"" as ar on a.""Id""=ar.""AssessmentId""  and ar.""ScheduledAssessmentId""= udf.""Id""  and ar.""IsDeleted""=false  and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsNote"" as arn on ar.""NtsNoteId""=arn.""Id"" and arn.""IsDeleted""=false  and arn.""CompanyId""='{_repo.UserContext.CompanyId}'
left Join public.""NtsService"" as ars on arn.""Id""=ars.""UdfNoteId"" and ars.""IsDeleted""=false  and ars.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as arss on ars.""ServiceStatusId""=arss.""Id"" and arss.""IsDeleted""=false  and arss.""CompanyId""='{_repo.UserContext.CompanyId}'
where #WHERE#
--(case when arss.""Code""='SERVICE_STATUS_INPROGRESS' then arss.""Code""='{statusCode}' or arss.""Code"" is null else arss.""Code""='{statusCode}') 
and s.""IsDeleted""=false  and s.""CompanyId""='{_repo.UserContext.CompanyId}' order by udf.""AssessmentStartDate"" ";

            var search = "";
            if (statusCode == "SERVICE_STATUS_INPROGRESS")
            {
                search = $@"  (arss.""Code""!='SERVICE_STATUS_COMPLETE' or arss.""Code"" is null ) ";
            }
            else
            {
                search = $@"  arss.""Code""='{statusCode}' ";
            }

            query = query.Replace("#WHERE#", search);

            var result = await _queryAssDetail.ExecuteQueryList(query, null);
            foreach (var i in result)
            {
                i.DisplayScheduledStartDate = i.ScheduledStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayScheduledEndDate = i.ScheduledEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayActualStartDate = i.ActualStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayActualEndDate = i.ActualEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
            }
            return result;
        }

        public async Task<string> GetUserIdentificationByUserId(string userId)
        {
            var userqry = $@"select f.""Id"" from cms.""N_TAS_CandidateId"" as c 
join public.""File"" as f on f.""Id""=c.""CandidateId"" and f.""IsDeleted""=false where c.""UserId""='{userId}' and c.""IsDeleted""=false
";



            var model = await _queryRepo.ExecuteScalar<string>(userqry, null);
            return model;
        }
        public async Task<List<AssessmentInterviewViewModel>> GetInterviewList(string UserID, string status)
        {

            var Query = $@"select  u.""Id"" as Id,sr.""Id"" as ServiceId, sr.""ServiceSubject"" as Subject,sr.""IsArchived"" as IsRowArchived, u.""Name""
 as OwnerUserName, L.""Name"" as AssessmentStatus,u.""Id"" as OwnerUserId,
'AssessmentInterview' as AssessmentType,false as IsAssessmentStopped,i.""Id"" as ""TableId"",
i.""ScheduledStartDate"" as ScheduledStartDate,i.""ScheduledEndDate"" as ScheduledEndDate
,i.""ActualStartDate"" as ActualStartDate,i.""ActualEndDate"" as ActualEndDate
,u.""Email"" as Email
--,t.""Name"" as PanelTeamName
,'' as AssessmentInterviewUrl
from public.""User"" as u
--left join public.""TeamUser""  as tm on  u.""Id""=tm.""UserId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
--left join public.""Team""  as t on  t.""Id""=tm.""TeamId""  and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
 join public.""NtsService"" as sr on u.""Id"" =sr.""OwnerUserId"" and sr.""IsDeleted""=false and sr.""CompanyId""='{_repo.UserContext.CompanyId}' and sr.""TemplateId"" =(select ""Id"" from public.""Template""  where ""Code""='TAS_Interview')
 join public.""NtsNote"" as n on n.""Id""=sr.""UdfNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
 join cms.""N_TAS_AssessmentInterview"" as i on i.""NtsNoteId""=n.""Id"" and i.""IsDeleted""=false and i.""CompanyId""='{_repo.UserContext.CompanyId}'
 join public.""LOV"" as L on L.""Id""=sr.""ServiceStatusId"" and L.""Code""='{status}'
where u.""Id""='{UserID}' and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
";

            var result = await _queryAssInterView.ExecuteQueryList<AssessmentInterviewViewModel>(Query, null);
            foreach (var i in result)
            {
                i.DisplayScheduledStartDate = i.ScheduledStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayScheduledEndDate = i.ScheduledEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayActualStartDate = i.ActualStartDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
                i.DisplayActualEndDate = i.ActualEndDate?.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateTimeFormat);
            }
            return result;
        }

        public async Task<IdNameViewModel> GetIndicatorbyName(string Name)
        {
            var query = @$"select I.""Id"",I.""IndicatorName"" as Name 
from cms.""N_TAS_Indicator"" I inner join public.""NtsNote"" N on I.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false
where I.""IndicatorName""='{Name}' and I.""IsDeleted""=false";


            var result = await _idNameRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return result;
        }

        public async Task<IdNameViewModel> GetCompetencybyName(string Name)
        {
            var query = @$"select I.""Id"",I.""CompetencyLevel"" as Name 
from cms.""N_TAS_CompetencyLevel"" I inner join public.""NtsNote"" N on I.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false
where I.""CompetencyLevel""='{Name}' and I.""IsDeleted""=false";


            var result = await _idNameRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return result;
        }


        public async Task<IdNameViewModel> GetAssessmentbyName(string Name)
        {
            var query = @$"select ""Id"" ,""Name"" from public.""LOV""  where ""LOVType""='Assessment_Type' and ""Name""='{Name}'";


            var result = await _idNameRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return result;
        }
        public async Task<SurveyScheduleViewModel> GetSurveyDetails(string surveyScheduleUserId, string surveyCode = null)
        {
            var query = @$"select s.""Id"" as SurveyScheduleUserId,s.""SurveyUserId"",ss.""Id"" as SurveyScheduleId,ss.""SurveyScheduleName"",s.""SurveyCode"",
            case when sr.""PreferredLanguage"" is not null then sr.""PreferredLanguage"" else ss.""PreferredLanguage"" end as ""PreferredLanguage"",ss.""SurveyId"",(s.""SurveyExpiryDate"")::TIMESTAMP::DATE,ss.""StartingInstruction"",sr.""Id"" as SurveyResultId,
            sr.""CurrentTopicId"",case when srlov.""Code"" is not null then srlov.""Code"" else lov.""Code"" end as LanguageCode,ser.""Id"" as ServiceId,s.""SurveyLink"" as SurveyLink,sun.""PortalId"" as PortalId
            ,sr.""SurveyEndDate"" as SurveyEndDate,sr.""SurveyStartDate"" as SurveyStartDate,survey.""AssessmentName"" as SurveyName
            from cms.""N_SURVEY_SurveyScheduleUser"" as s
            join public.""NtsNote"" as sun on s.""NtsNoteId""=sun.""Id"" and sun.""IsDeleted""=false and sun.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join cms.""N_SURVEY_SurveySchedule"" as ss on s.""SurveyScheduleId"" = ss.""Id"" and ss.""IsDeleted"" = false and ss.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join cms.""N_TAS_Assessment"" as survey on ss.""SurveyId""=survey.""Id"" and survey.""IsDeleted""=false and survey.""CompanyId""='{_repo.UserContext.CompanyId}'            
            left join public.""LOV"" as lov on ss.""PreferredLanguage""=lov.""Id"" and lov.""IsDeleted""=false
            left join cms.""N_SURVEY_S_SurveyResult"" as sr on s.""Id"" = sr.""SurveyScheduleUserId"" and sr.""IsDeleted"" = false and sr.""CompanyId""='{_repo.UserContext.CompanyId}'
             left join public.""LOV"" as srlov on sr.""PreferredLanguage""=srlov.""Id"" and srlov.""IsDeleted""=false
            left join public.""NtsNote"" as n on sr.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""NtsService"" as ser on n.""Id""=ser.""UdfNoteId"" and ser.""IsDeleted""=false and ser.""CompanyId""='{_repo.UserContext.CompanyId}'
            where <<SEARCH>>  and s.""IsDeleted"" = false and s.""CompanyId""='{_repo.UserContext.CompanyId}' ";
            if (surveyScheduleUserId.IsNotNullAndNotEmpty())
            {
                query = query.Replace("<<SEARCH>>", @$"s.""Id"" = '{surveyScheduleUserId}'");
            }
            else
            {
                query = query.Replace("<<SEARCH>>", @$"s.""SurveyCode"" = '{surveyCode}'");
            }
            var result = await _idNameRepo.ExecuteQuerySingle<SurveyScheduleViewModel>(query, null);
            return result;
        }
        //public async Task<SurveyScheduleViewModel> GetSurveyDetailsByCode(string surveyCode)
        //{
        //    var query = @$"select s.""Id"" as SurveyScheduleUserId,s.""SurveyUserId"",ss.""Id"" as SurveyScheduleId,ss.""SurveyScheduleName"",
        //    ss.""PreferredLanguage"",ss.""SurveyId"",(s.""SurveyExpiryDate"")::TIMESTAMP::DATE,ss.""StartingInstruction"",sr.""Id"" as SurveyResultId,
        //    sr.""CurrentTopicId"",lov.""Code"" as LanguageCode,ser.""Id"" as ServiceId,s.""SurveyLink"" as SurveyLink,sun.""PortalId"" as PortalId
        //    from cms.""N_SURVEY_SurveyScheduleUser"" as s
        //    join public.""NtsNote"" as sun on s.""NtsNoteId""=sun.""Id"" and sun.""IsDeleted""=false and sun.""CompanyId""='{_repo.UserContext.CompanyId}'
        //    left join cms.""N_SURVEY_SurveySchedule"" as ss on s.""SurveyScheduleId"" = ss.""Id"" and ss.""IsDeleted"" = false and ss.""CompanyId""='{_repo.UserContext.CompanyId}'
        //    left join public.""LOV"" as lov on ss.""PreferredLanguage""=lov.""Id"" and lov.""IsDeleted""=false
        //    left join cms.""N_SURVEY_S_SurveyResult"" as sr on s.""Id"" = sr.""SurveyScheduleUserId"" and sr.""IsDeleted"" = false and sr.""CompanyId""='{_repo.UserContext.CompanyId}'
        //    left join public.""NtsNote"" as n on sr.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
        //    left join public.""NtsService"" as ser on n.""Id""=ser.""UdfNoteId"" and ser.""IsDeleted""=false and ser.""CompanyId""='{_repo.UserContext.CompanyId}'
        //    where s.""SurveyCode"" = '{surveyCode}' and s.""IsDeleted"" = false and s.""CompanyId""='{_repo.UserContext.CompanyId}' ";

        //    var result = await _idNameRepo.ExecuteQuerySingle<SurveyScheduleViewModel>(query, null);
        //    return result;
        //}

        public async Task UpdatePreferredLanguage(string surveyScheduleId, string lang,string surveyResultId)
        {
            var query = $@"Update cms.""N_SURVEY_S_SurveyResult"" set ""PreferredLanguage""='{lang}' where ""Id""='{surveyResultId}';
            Update cms.""N_SURVEY_S_SurveyResult"" set ""SurveyStartDate""='{DateTime.Now.ToDatabaseDateFormat()}' where ""Id""='{surveyResultId}' and ""SurveyStartDate"" is null";
            await _queryRepo1.ExecuteCommand(query, null);
        }

        //Survey

        public async Task<SurveyTopicViewModel> GetSurveyAssessmentQuestion(string serviceId, string currTopicId, string lang)
        {
            if (currTopicId.IsNullOrEmpty())
            {
                var que = $@"Select ar.""CurrentTopicId""
                           From public.""NtsService"" as s
                           Join public.""NtsNote"" as n on s.""UdfNoteId""=n.""Id"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                           Join cms.""N_SURVEY_S_SurveyResult"" as ar on n.""Id""=ar.""NtsNoteId"" and ar.""IsDeleted""=false  and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
                           where s.""Id""='{serviceId}'  and s.""IsDeleted""=false  and s.""CompanyId""='{_repo.UserContext.CompanyId}'";

                var res = await _queryAssDetail.ExecuteQuerySingle(que, null);

                return await GetSurveyAssessmentQuestionParent(serviceId, res.CurrentTopicId, lang);
            }
            else
            {
                return await GetSurveyAssessmentQuestionParent(serviceId, currTopicId, lang);
            }

        }

        public async Task<SurveyTopicViewModel> GetSurveyAssessmentQuestionParent(string serviceId, string currTopicId, string lang)
        {
            var result = new SurveyTopicViewModel();

            var query = $@"select nt.""Id"" as ""TopicId"",nt.""NoteSubject"" as TopicName,aq.""QuestionId"" as Id,aq.""SequenceOrder"" as SequenceNo,nt.""SequenceOrder"" as TopicSequenceNo, #QUESTIONLANGUAGE#,q.""NtsNoteId"" as NoteId
      ,ar.""SurveyId""  as AssessmentId,ar.""SurveyScheduleUserId"" as SurveyScheduleUserId From public.""NtsService"" as s
Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""Code""='S_SURVEY_RESULT' and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
Join public.""NtsNote"" as nts on s.""UdfNoteId""=nts.""Id"" and nts.""IsDeleted""=false  and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_SURVEY_S_SurveyResult"" as ar on nts.""Id""=ar.""NtsNoteId"" and ar.""IsDeleted""=false and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_Assessment"" as a on ar.""SurveyId""=a.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_AssessmentQuestion"" as aq on a.""Id""=aq.""AssessmentId"" and aq.""IsDeleted""=false and aq.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_Question"" as q on aq.""QuestionId""=q.""Id"" and q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as n on q.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
Join public.""NtsNote"" as nt on n.""ParentNoteId""=nt.""Id"" and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'	
where s.""Id""='{serviceId}' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}' order by aq.""SequenceOrder"" ";

            var queslang = "";
            if (lang.IsNotNullAndNotEmpty())
            {
                if (lang == "ENGLISH")
                {
                    queslang = $@"q.""Question"", q.""QuestionDescription"", q.""QuestionAttachmentId"" as FileId ";
                }
                else if (lang == "ARABIC")
                {
                    queslang = $@"q.""QuestionArabic"" as Question, q.""QuestionDescriptionArabic"" as QuestionDescription, q.""QuestionArabicAttachmentId"" as FileId ";
                }
            }
            else
            {
                queslang = $@"q.""Question"", q.""QuestionDescription""";
            }
            query = query.Replace("#QUESTIONLANGUAGE#", queslang);

            var list = await _queryAssDetail.ExecuteQueryList(query, null);

            if (list != null && list.Count != 0)
            {
                if (currTopicId.IsNullOrEmpty())
                {
                    result.TopicId = list.Where(x => x.TopicSequenceNo == 1).Select(x => x.TopicId).FirstOrDefault();
                    result.NextId = list.Where(x => x.TopicSequenceNo == 2).Select(x => x.TopicId).FirstOrDefault();
                    result.TopicName = list.Where(x => x.TopicSequenceNo == 1).Select(x => x.TopicName).FirstOrDefault();
                }
                else
                {
                    result.TopicId = currTopicId;
                    var seq = list.Where(x => x.TopicId == currTopicId).Select(x => x.TopicSequenceNo).FirstOrDefault();
                    var next = seq + 1;
                    var prev = seq - 1;
                    result.PreviousId = list.Where(x => x.TopicSequenceNo == prev).Select(x => x.TopicId).FirstOrDefault();
                    result.NextId = list.Where(x => x.TopicSequenceNo == next).Select(x => x.TopicId).FirstOrDefault();
                    result.TopicName = list.Where(x => x.TopicId == currTopicId).Select(x => x.TopicName).FirstOrDefault();

                }
                result.FirstId = list.First().TopicId;
                result.LastId = list.Last().TopicId;
                // result.AssessmentId = list.First().AssessmentId;
                //result.OptionList = new List<AssessmentQuestionsOptionViewModel>();

                if (result.TopicId.IsNotNull())
                {
                    result.QuestionList = new List<SurveyQuestionViewModel>();
                    var listQuestion = list.Where(x => x.TopicId == result.TopicId).ToList();
                    foreach (var question in listQuestion)
                    {
                        var survey = new SurveyQuestionViewModel();
                        survey.QuestionId = question.Id;
                        survey.QuestionName = HttpUtility.HtmlDecode(question.Question);


                        survey.OptionList = await GetSurveyOptions(question.Id, serviceId, lang, list.First().AssessmentId, question.SurveyScheduleUserId);
                        if (survey.OptionList != null && survey.OptionList.Count > 0)
                        {
                            var answer = survey.OptionList.Where(x => x.AnswerId != null).FirstOrDefault();
                            if (answer != null)
                            {
                                survey.AnswerComment = answer.AnswerComment;
                                //result.Score = answer.Score;
                                survey.AnswerId = answer.AnswerId;
                            }
                            survey.AnswerItemId = survey.OptionList.FirstOrDefault().AnswerItemId;
                        }
                        result.QuestionList.Add(survey);
                    }
                }
            }

            return result;
        }

        public async Task<List<SurveyScheduleViewModel>> GetSurveyScheduleList(string surveyId, string status)
        {
            var query = $@"select ss.*,n.""Id"" as NoteId, lovns.""Name"" as NoteStatusName,lovns.""Code"" as NoteStatusCode
                            ,a.""AssessmentName"" as SurveyName, lg.""Name"" as PreferredLanguageName,
                            case when ss.""SurveyStatus""='2' then 'Schedule Completed' else case when ss.""SurveyStatus""='1' then 'Schedule Inprogress' else 'Not Scheduled' end end as SurveyStatusName
                            from cms.""N_SURVEY_SurveySchedule"" as ss
                            left join public.""NtsNote"" as n on n.""Id""=ss.""NtsNoteId"" and n.""IsDeleted""=false
                            left join public.""LOV"" as lovns on lovns.""Id""=n.""NoteStatusId"" and lovns.""IsDeleted""=false
                            left join cms.""N_TAS_Assessment"" as a on a.""Id""=ss.""SurveyId""  and a.""IsDeleted""=false
                            left join public.""LOV"" as lg on lg.""Id""=ss.""PreferredLanguage"" and lg.""IsDeleted""=false
                            WHERE ss.""IsDeleted""=false #WHERE#
                        ";
            var replace = $@"";
            if (surveyId.IsNotNullAndNotEmpty())
            {
                replace = replace + $@" and a.""Id""='{surveyId}' ";
            }
            if (status.IsNotNullAndNotEmpty())
            {
                replace = replace + $@" and lovns.""Id""='{status}' ";
            }
            query = query.Replace("#WHERE#", replace);
            var data = await _queryRepo.ExecuteQueryList<SurveyScheduleViewModel>(query, null);
            return data;
        }
        public async Task<SurveyScheduleViewModel> GetSurveyScheduleDetails(string surveyScheduleId)
        {
            var query = $@"select ss.*,n.""Id"" as NoteId, lovns.""Name"" as NoteStatusName,lovns.""Code"" as NoteStatusCode
                            ,a.""AssessmentName"" as SurveyName, lg.""Name"" as PreferredLanguageName,
                            case when ss.""SurveyStatus""='2' then 'Schedule Completed' else case when ss.""SurveyStatus""='1' then 'Schedule Inprogress' else 'Not Scheduled' end end as SurveyStatusName
                            from cms.""N_SURVEY_SurveySchedule"" as ss
                            left join public.""NtsNote"" as n on n.""Id""=ss.""NtsNoteId"" and n.""IsDeleted""=false
                            left join public.""LOV"" as lovns on lovns.""Id""=n.""NoteStatusId"" and lovns.""IsDeleted""=false
                            left join cms.""N_TAS_Assessment"" as a on a.""Id""=ss.""SurveyId""  and a.""IsDeleted""=false
                            left join public.""LOV"" as lg on lg.""Id""=ss.""PreferredLanguage"" and lg.""IsDeleted""=false
                            WHERE ss.""IsDeleted""=false and ss.""Id""='{surveyScheduleId}' 
                        ";
            var data = await _queryRepo.ExecuteQuerySingle<SurveyScheduleViewModel>(query, null);
            return data;
        }
        public async Task<List<AssessmentQuestionsOptionViewModel>> GetSurveyOptions(string noteId, string serviceId, string lang, string assessmentId, string surveyScheduleUserId)
        {
            //var query = $@"Select o.""Id"", o.""Option"", o.""IsRightAnswer"",o.""Score""
            //From cms.""N_TAS_Question"" as q 
            //join public.""NtsNote"" as n on q.""NtsNoteId""=n.""ParentNoteId""
            //Join cms.""N_TAS_QuestionOption"" as o on n.""Id""=o.""NtsNoteId""
            //where q.""Id""='{noteId}' ";

            var query = $@"Select o.""Id"", #OPTIONLANGUAGE#, o.""IsRightAnswer"",o.""Score"",ans.""OptionId"" as AnswerId
            ,ans.""Comment"" as AnswerComment,ans.""Id"" as AnswerItemId
            From cms.""N_TAS_Question"" as q
            join public.""NtsNote"" as n on q.""NtsNoteId""=n.""ParentNoteId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
            Join cms.""N_TAS_QuestionOption"" as o on n.""Id""=o.""NtsNoteId"" and o.""IsDeleted""=false and o.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join cms.""N_SURVEY_S_SurveyResult"" as ser on ser.""SurveyScheduleUserId""='{surveyScheduleUserId}' and ser.""SurveyUserId""='{_repo.UserContext.UserId}' and ser.""IsDeleted""=false and ser.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join cms.""N_SURVEY_S_SurveyResultAnswer"" as ans on q.""Id""=ans.""QuestionId"" and ans.""SurveyResultId""=ser.""Id"" and ans.""IsDeleted""=false  and ans.""SurveyId""='{assessmentId}' and ans.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""NtsNote"" as nts on ans.""NtsNoteId""=nts.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
            left join public.""NtsService"" as s on nts.""Id""=s.""UdfNoteId"" and s.""ParentServiceId""='{serviceId}' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
            where q.""Id""='{noteId}' and q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}' order by o.""SequenceOrder""";

            var optlang = "";
            if (lang.IsNotNullAndNotEmpty())
            {
                if (lang == "ENGLISH")
                {
                    optlang = $@"o.""Option"" ";
                }
                else if (lang == "ARABIC")
                {
                    optlang = $@"o.""OptionArabic"" as Option";
                }
            }
            else
            {
                optlang = $@"o.""Option"" ";
            }
            query = query.Replace("#OPTIONLANGUAGE#", optlang);
            var result = await _queryRepo1.ExecuteQueryList(query, null);
            return result;
        }

        public async Task ExecuteSurveyForUsers(string NoteId, string PortalId)
        {
            var _userPortalBusiness = _sp.GetService<IUserPortalBusiness>();
            var _configuration = _sp.GetService<Microsoft.Extensions.Configuration.IConfiguration>();
            var _notificationBusiness = _sp.GetService<INotificationBusiness>();

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.NoteId = NoteId;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var LastUpdatedBy = notemodel.LastUpdatedBy; //rowData1.ContainsKey("LastUpdatedBy") ? Convert.ToString(rowData1["LastUpdatedBy"]) : "";
            var UserDetails = rowData1.ContainsKey("UserDetails") ? Convert.ToString(rowData1["UserDetails"]) : "";
            var SurveyExpiryDate = rowData1.ContainsKey("SurveyExpiryDate") ? Convert.ToString(rowData1["SurveyExpiryDate"]) : "";
            var SurveyScheduleId = notemodel.UdfNoteTableId; //rowData1.ContainsKey("Id") ? Convert.ToString(rowData1["Id"]) : "";
            // update existing survey schedule user Expiry date to New Expiry Date 
            await UpdateScheduleSurveyUserExpiryDate(SurveyScheduleId, SurveyExpiryDate);
            if (UserDetails.IsNotNull())
            {
                var users = UserDetails.Split(";");
                if (users.Count() > 0)
                {
                    foreach (var user in users)
                    {
                        if (user.IsNotNullAndNotEmpty())
                        {
                            string UserId = "";
                            var details = user.Split("|");
                            var email1 = details[0];
                            var Name = email1;
                            if (details.Length > 1)
                            {
                                Name = details[1];
                            }

                            // var email1 = email[1];
                            // check if user email exist
                            var exisitingUser = await _userBusiness.GetSingle(x => x.Email == email1);
                            if (exisitingUser != null)
                            {
                                UserId = exisitingUser.Id;
                                // check if user has access to survey portal
                                var allowedPortals = await _userPortalBusiness.GetList(x => x.UserId == exisitingUser.Id && x.PortalId == PortalId);
                                if (!allowedPortals.Any(x => x.PortalId == PortalId))
                                {
                                    var res1 = await _userPortalBusiness.Create(new UserPortalViewModel
                                    {
                                        UserId = exisitingUser.Id,
                                        PortalId = PortalId,
                                    });
                                }
                                // check survey is already scheduled for the user
                                var isExist = await GetSurveyScheduleUser(SurveyScheduleId, UserId);
                                if (!isExist)
                                {
                                    // Schedule the Survey for the User
                                    if (UserId.IsNotNullAndNotEmpty())
                                    {
                                        var TempModel = new NoteTemplateViewModel();
                                        TempModel.TemplateCode = "SURVEY_SCHEDULE_USER";
                                        TempModel.ActiveUserId = LastUpdatedBy;
                                        var model = await _noteBusiness.GetNoteDetails(TempModel);
                                        model.OwnerUserId = LastUpdatedBy;
                                        model.DataAction = DataActionEnum.Create;
                                        dynamic exo = new System.Dynamic.ExpandoObject();
                                        ((IDictionary<String, Object>)exo).Add("SurveyScheduleId", SurveyScheduleId);
                                        ((IDictionary<String, Object>)exo).Add("SurveyExpiryDate", SurveyExpiryDate);
                                        ((IDictionary<String, Object>)exo).Add("SurveyUserId", UserId);
                                        var surveyCode = await GetSurveyCode();
                                        ((IDictionary<String, Object>)exo).Add("SurveyCode", Convert.ToString(surveyCode));
                                        var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);

                                        var param = Helper.EncryptJavascriptAesCypher($"surveyCode={surveyCode}");
                                        var link = $"{baseurl}survey?enc={param}";
                                        ((IDictionary<String, Object>)exo).Add("SurveyLink", link);

                                        model.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                        model.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                                        var res = await _noteBusiness.ManageNote(model);


                                        //rowData1["SurveyLink"] = link;
                                        //var notificationTemplateModel = await _notificationBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.CompanyId == model.CompanyId && x.Code == "SURVEY_WELCOME_EMAIL");
                                        //if (notificationTemplateModel.IsNotNull())
                                        //{
                                        //    notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{link}}", link);
                                        //    var notificationModel = new NotificationViewModel()
                                        //    {
                                        //        To = email1,
                                        //        ToUserId = UserId,
                                        //        // FromUserId = uc.UserId,
                                        //        Subject = notificationTemplateModel.Subject,
                                        //        Body = notificationTemplateModel.Body,
                                        //        SendAlways = true,
                                        //        NotifyByEmail = true,
                                        //        DynamicObject = model
                                        //    };
                                        //    await _notificationBusiness.Create(notificationModel);
                                        //}
                                    }
                                }
                            }
                            else
                            {
                                // Create new User
                                var UserModel = new UserViewModel();
                                UserModel.Name = Name;
                                UserModel.Email = email1;
                                UserModel.Portal = new List<string>();
                                UserModel.Portal.Add(PortalId);
                                var result = await _userBusiness.Create(UserModel);
                                if (result.IsSuccess)
                                {
                                    UserId = result.Item.Id;
                                    var res1 = await _userPortalBusiness.Create(new UserPortalViewModel
                                    {
                                        UserId = result.Item.Id,
                                        PortalId = PortalId,
                                    });
                                    // Schedule the Survey for the User
                                    if (UserId.IsNotNullAndNotEmpty())
                                    {
                                        var TempModel = new NoteTemplateViewModel();
                                        TempModel.TemplateCode = "SURVEY_SCHEDULE_USER";
                                        TempModel.ActiveUserId = LastUpdatedBy;
                                        var model = await _noteBusiness.GetNoteDetails(TempModel);
                                        model.OwnerUserId = LastUpdatedBy;
                                        model.DataAction = DataActionEnum.Create;
                                        dynamic exo = new System.Dynamic.ExpandoObject();
                                        ((IDictionary<String, Object>)exo).Add("SurveyScheduleId", SurveyScheduleId);
                                        ((IDictionary<String, Object>)exo).Add("SurveyExpiryDate", SurveyExpiryDate);
                                        ((IDictionary<String, Object>)exo).Add("SurveyUserId", UserId);
                                        var surveyCode = await GetSurveyCode();
                                        ((IDictionary<String, Object>)exo).Add("SurveyCode", Convert.ToString(surveyCode));
                                        var baseurl = ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration);

                                        var param = Helper.EncryptJavascriptAesCypher($"surveyCode={surveyCode}");
                                        var link = $"{baseurl}survey?enc={param}";
                                        ((IDictionary<String, Object>)exo).Add("SurveyLink", link);

                                        model.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                        model.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                                        var res = await _noteBusiness.ManageNote(model);

                                        //var customUrl = HttpUtility.UrlEncode($"surveyScheduleUserId={res.Item.UdfNoteTableId}");
                                        //var param = Helper.EncryptJavascriptAesCypher($"page=surveyhome&customurl={customUrl}");
                                        //var portal = await _userPortalBusiness.GetSingleById<PortalViewModel, Portal>(PortalId);
                                        //var link = $"{baseurl}portal/{portal.Name}?enc={param}";
                                        //rowData1["SurveyLink"] = link;

                                        //var notificationTemplateModel = await _notificationBusiness.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.CompanyId == model.CompanyId && x.Code == "SURVEY_WELCOME_EMAIL");
                                        //if (notificationTemplateModel.IsNotNull())
                                        //{
                                        //    notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{link}}", link);
                                        //    var notificationModel = new NotificationViewModel()
                                        //    {
                                        //        To = email1,
                                        //        ToUserId = UserId,
                                        //        // FromUserId = uc.UserId,
                                        //        Subject = notificationTemplateModel.Subject,
                                        //        Body = notificationTemplateModel.Body,
                                        //        SendAlways = true,
                                        //        NotifyByEmail = true,
                                        //        DynamicObject = model
                                        //    };
                                        //    await _notificationBusiness.Create(notificationModel);
                                        //}
                                    }
                                }

                            }

                        }
                    }
                }

            }
            //Updated To Completed           
            var SurveyStatus = rowData1.ContainsKey("SurveyStatus") ? Convert.ToString(rowData1["SurveyStatus"]) : "";
            if (SurveyStatus.IsNotNull())
            {
                rowData1["SurveyStatus"] = (int)((SurveyStatusEnum)Enum.Parse(typeof(SurveyStatusEnum), SurveyStatusEnum.ScheduleCompleted.ToString()));
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

            }
        }
        public async Task<List<SurveyScheduleViewModel>> GetSurveyDetail(string id)
        {
            var query = $@"select  ss.""Id"" as ""Id"",u.""Name"" as ""SurveyUserName"",u.""Email"" as ""SurveyUserEmail"",ss.""SurveyExpiryDate"" as ""SurveyExpiryDate"" 
            ,sr.""SurveyStartDate"" as ""SurveyStartDate"",sr.""SurveyEndDate"" as ""SurveyEndDate"",su.""SurveyLink"" as ""SurveyLink"",su.""SurveyCode"" as ""SurveyCode"",sr.""Id"" as SurveyResultId,su.""Id"" as SurveyScheduleUserId,
            su.""SurveyUserId"" as SurveyUserId,ss.""SurveyId"" as SurveyId 
            from cms.""N_SURVEY_SurveySchedule"" as ss
            join cms.""N_SURVEY_SurveyScheduleUser"" as su on su.""SurveyScheduleId"" = ss.""Id""  and su.""IsDeleted"" = false

            join public.""User"" as u on u.""Id""=su.""SurveyUserId""
            left join cms.""N_SURVEY_S_SurveyResult"" as sr on sr.""SurveyScheduleUserId""=su.""Id""  and sr.""IsDeleted""=false
            WHERE ss.""IsDeleted""=false and ss.""Id""='{id}' ";
            var data = await _queryRepo.ExecuteQueryList<SurveyScheduleViewModel>(query, null);
            return data;
        }

        public async Task<CommandResult<List<SurveyScheduleViewModel>>> GetValidateSurveyScheduleUserDetails(string surveyId, string userDetails, string surveyScheduleId)
        {
            var query = $@"select ss.* from cms.""N_SURVEY_SurveySchedule"" as ss
                           WHERE ss.""IsDeleted""=false and ss.""SurveyId""='{surveyId}' #WHERE# ";
            var where = $@"";
            if (surveyScheduleId.IsNotNullAndNotEmpty())
            {
                where = $@" and ss.""Id""<>'{surveyScheduleId}' ";
            }
            query = query.Replace("#WHERE#", where);
            var result = await _queryRepo.ExecuteQueryList<SurveyScheduleViewModel>(query, null);
            var errorList = new Dictionary<string, string>();
            var userData = userDetails.Split(";");
            if (userData.Count() > 0)
            {
                var i = 1;
                foreach (var user in userData)
                {
                    var u = user.Split("|");
                    var uemail = u[0];
                    if (uemail.IsNotNullAndNotEmpty())
                    {
                        foreach (var rdata in result)
                        {
                            if (rdata.UserDetails.IsNotNullAndNotEmpty())
                            {
                                var rusers = rdata.UserDetails.Split(";");
                                if (rusers.Count() > 0)
                                {
                                    foreach (var ruser in rusers)
                                    {
                                        var ru = ruser.Split("|");
                                        var ruemail = ru[0];
                                        if (ruemail.IsNotNullAndNotEmpty() && ruemail == uemail)
                                        {
                                            errorList.Add("UserEmail" + i, "Survey Schedule : " + rdata.SurveyScheduleName + ", " + uemail + " email already exist.");
                                        }
                                        i++;
                                    }
                                }
                            }

                        }
                    }

                }
                if (errorList.Count > 0)
                {
                    return CommandResult<List<SurveyScheduleViewModel>>.Instance(result, false, errorList);
                }
            }
            return CommandResult<List<SurveyScheduleViewModel>>.Instance(result, true, errorList);
        }
        private async Task<long> GetSurveyCode()
        {
            var random = new Random();
            var isUniq = false;
            var surveyCode = random.Next(100000, 999999);
            while (isUniq == false)
            {
                var query = @$"select s.""SurveyCode"" from cms.""N_SURVEY_SurveyScheduleUser"" as s where s.""SurveyCode""='{surveyCode}'
                and s.""IsDeleted""=false";
                var result = await _queryRepo.ExecuteQuerySingle<SurveyScheduleViewModel>(query, null);
                if (result == null)
                {
                    isUniq = true;
                    return surveyCode;
                }
            }
            return surveyCode;

        }

        public async Task<ServiceTemplateViewModel> GetSurveyResultDetails(string surveyId, string surveyScheduleUserId, string surveyUserId)
        {
            var query = @$"select s.""Id"" as ""ServiceId"",s.*,n.""PreferredLanguage"" as PreferredLanguageId,n.""CurrentTopicId"" 
            ,n.""Id"" as ""UdfNoteTableId"",n.""SurveyStartDate"" as ""SurveyStartDate""
            FROM cms.""N_SURVEY_S_SurveyResult"" n
            join public.""NtsService"" s on n.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
            where n.""SurveyScheduleUserId"" ='{surveyScheduleUserId}' and n.""IsDeleted""=false";
            var result = await _queryRepo.ExecuteQuerySingle<ServiceTemplateViewModel>(query, null);
            return result;
        }
        public async Task<ServiceTemplateViewModel> CheckSurveyResultExist(string surveyScheduleUserId)
        {
            var query = @$"select s.""Id"" as ""ServiceId"",s.*,n.""PreferredLanguage"" as PreferredLanguageId,n.""CurrentTopicId"" FROM cms.""N_SURVEY_S_SurveyResult"" n
            join public.""NtsService"" s on n.""Id""=s.""UdfNoteTableId"" and s.""IsDeleted""=false
            where n.""SurveyScheduleUserId"" ='{surveyScheduleUserId}' and n.""IsDeleted""=false";
            var result = await _queryRepo.ExecuteQuerySingle<ServiceTemplateViewModel>(query, null);
            return result;
        }

        public async Task<List<SurveyQuestionViewModel>> GetSurveyResult(string surveyResultId, string lang)
        {
            var result = new List<SurveyQuestionViewModel>();

            var query = $@"SELECT sra.""OptionId"" as AnswerId,sra.""Comment"" as AnswerComment,q.""Question"" as QuestionName,o.""Option"" as AnswerName
FROM cms.""N_SURVEY_S_SurveyResult"" sr
join cms.""N_SURVEY_S_SurveyResultAnswer"" sra on sra.""SurveyResultId""=sr.""Id"" and sra.""IsDeleted""=false and sra.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_TAS_Question"" as q on q.""Id""=sra.""QuestionId"" and q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_QuestionOption"" as o on sra.""OptionId""=o.""Id"" and o.""IsDeleted""=false and o.""CompanyId""='{_repo.UserContext.CompanyId}'

where sr.""Id""='{surveyResultId}' and sr.""IsDeleted""=false and sr.""CompanyId""='{_repo.UserContext.CompanyId}' order by q.""SequenceOrder"" ";

            var queslang = "";
            if (lang.IsNotNullAndNotEmpty())
            {
                if (lang == "ENGLISH")
                {
                    queslang = $@"q.""Question"", q.""QuestionDescription"", q.""QuestionAttachmentId"" as FileId ";
                }
                else if (lang == "ARABIC")
                {
                    queslang = $@"q.""QuestionArabic"" as Question, q.""QuestionDescriptionArabic"" as QuestionDescription, q.""QuestionArabicAttachmentId"" as FileId ";
                }
            }
            else
            {
                queslang = $@"q.""Question"", q.""QuestionDescription""";
            }
            query = query.Replace("#QUESTIONLANGUAGE#", queslang);

            result = await _queryAssDetail.ExecuteQueryList<SurveyQuestionViewModel>(query, null);



            return result;
        }

        public async Task<bool> GetSurveyScheduleUser(string SurveyScheduleId, string UserId)
        {
            var query = $@"select * from cms.""N_SURVEY_SurveyScheduleUser"" where ""SurveyUserId""='{UserId}' and ""SurveyScheduleId""='{SurveyScheduleId}' and ""IsDeleted""=false";
            var data = await _queryRepo1.ExecuteQueryList<ServiceTemplateViewModel>(query, null);
            if (data.IsNotNull() && data.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task UpdateScheduleSurveyUserExpiryDate(string surveyScheduleId, string expiryDate)
        {
            var query = $@"Update cms.""N_SURVEY_SurveyScheduleUser"" set ""SurveyExpiryDate""='{expiryDate}' where ""SurveyScheduleId""='{surveyScheduleId}'";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task GenerateSurveyDetails(string surveyScheduleId)
        {
            var _tableMetadataBusiness = _sp.GetService<ITableMetadataBusiness>();
            var SurveyScheduleUserList = await GetSurveyDetail(surveyScheduleId);
            if (SurveyScheduleUserList.IsNotNull() && SurveyScheduleUserList.Count() > 0)
            {
                foreach (var user in SurveyScheduleUserList)
                {
                    // check if survey result exist or not     
                    var surveyresult = await CheckSurveyResultExist(user.SurveyScheduleUserId);
                    if (surveyresult.IsNotNull())
                    {
                        var existing = await _tableMetadataBusiness.GetTableDataByColumn("SN_S_SURVEY_RESULT", null, "SurveyScheduleUserId", user.SurveyScheduleUserId);
                        //check if end date is present
                        if (existing.IsNotNull()) 
                        {
                            
                            var EndDate =Convert.ToString(existing["SurveyEndDate"]);
                            if (EndDate.IsNullOrEmpty())
                            {
                                // get all Question and check if already have service for the answer result
                                var questionslist = await GetQuestionsForSurveyResult(user.SurveyId);
                                if (questionslist.IsNotNull())
                                {
                                    foreach (var question in questionslist)
                                    {
                                        var isAnswerResultExist = await CheckIfSurveyAnswerResultExist(question.Id, existing["Id"].ToString());
                                        if (!isAnswerResultExist)
                                        {
                                            // create Survey Answer result service                                       
                                            var serviceTempModel1 = new ServiceTemplateViewModel();
                                            serviceTempModel1.TemplateCode = "S_SURVEY_RESULT_ANSWER";
                                            serviceTempModel1.ActiveUserId = user.SurveyUserId;
                                            var servicemodel1 = await _serviceBusiness.GetServiceDetails(serviceTempModel1);
                                            servicemodel1.DataAction = DataActionEnum.Create;
                                            dynamic exo1 = new System.Dynamic.ExpandoObject();
                                            ((IDictionary<String, Object>)exo1).Add("SurveyId", user.SurveyId);
                                            ((IDictionary<String, Object>)exo1).Add("SurveyResultId", surveyresult.UdfNoteTableId);
                                            ((IDictionary<String, Object>)exo1).Add("QuestionId", question.Id);
                                            ((IDictionary<String, Object>)exo1).Add("UserId", user.SurveyUserId);
                                            servicemodel1.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                                            servicemodel1.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                                            var res1 = await _serviceBusiness.ManageService(servicemodel1);
                                        }
                                    }
                                }
                            }
                        }
                                         
                    }
                    // does not exist
                    else
                    {
                        // Create Survey Result service
                        var serviceTempModel = new ServiceTemplateViewModel();
                        serviceTempModel.TemplateCode = "S_SURVEY_RESULT";
                        serviceTempModel.ActiveUserId = user.SurveyUserId;
                        var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);
                        servicemodel.DataAction = DataActionEnum.Create;
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        ((IDictionary<String, Object>)exo).Add("SurveyScheduleId", user.Id);
                        ((IDictionary<String, Object>)exo).Add("PreferredLanguage", user.PreferredLanguage);
                        ((IDictionary<String, Object>)exo).Add("SurveyUserId", user.SurveyUserId);
                        ((IDictionary<String, Object>)exo).Add("SurveyId", user.SurveyId);
                        ((IDictionary<String, Object>)exo).Add("SurveyScheduleUserId", user.SurveyScheduleUserId);
                        servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                        servicemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        var res = await _serviceBusiness.ManageService(servicemodel);
                        // fetch all the Question and create answer service
                        var questionslist = await GetQuestionsForSurveyResult(user.SurveyId);
                        if (questionslist.IsNotNull())
                        {
                            foreach (var question in questionslist)
                            {
                                // Create Survey Result Answer service
                                var serviceTempModel1 = new ServiceTemplateViewModel();
                                serviceTempModel1.TemplateCode = "S_SURVEY_RESULT_ANSWER";
                                serviceTempModel1.ActiveUserId = _userContext.UserId;
                                var servicemodel1 = await _serviceBusiness.GetServiceDetails(serviceTempModel1);
                                servicemodel1.DataAction = DataActionEnum.Create;
                                dynamic exo1 = new System.Dynamic.ExpandoObject();
                                ((IDictionary<String, Object>)exo1).Add("SurveyId", user.SurveyId);
                                ((IDictionary<String, Object>)exo1).Add("SurveyResultId", res.Item.UdfNoteTableId);
                                ((IDictionary<String, Object>)exo1).Add("QuestionId", question.Id);
                                ((IDictionary<String, Object>)exo1).Add("UserId", user.SurveyUserId);
                                servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                                servicemodel1.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                                var res1 = await _serviceBusiness.ManageService(servicemodel1);
                            }
                        }
                    }
                }
            }
        }


        public async Task<List<SurveyTopicViewModel>> GetQuestionsForSurveyResult(string surveyId)
        {
            var result = new List<SurveyTopicViewModel>();

            var query = $@"select aq.""QuestionId"" as Id,aq.""SequenceOrder"" as SequenceNo     
From  cms.""N_TAS_Assessment"" as a 
Join cms.""N_TAS_AssessmentQuestion"" as aq on a.""Id""=aq.""AssessmentId"" and aq.""IsDeleted""=false and aq.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_Question"" as q on aq.""QuestionId""=q.""Id"" and q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}'
	
where a.""Id""='{surveyId}' and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'";

            //var queslang = "";
            //if (lang.IsNotNullAndNotEmpty())
            //{
            //    if (lang == "ENGLISH")
            //    {
            //        queslang = $@"q.""Question"", q.""QuestionDescription"", q.""QuestionAttachmentId"" as FileId ";
            //    }
            //    else if (lang == "ARABIC")
            //    {
            //        queslang = $@"q.""QuestionArabic"" as Question, q.""QuestionDescriptionArabic"" as QuestionDescription, q.""QuestionArabicAttachmentId"" as FileId ";
            //    }
            //}
            //else
            //{
            //    queslang = $@"q.""Question"", q.""QuestionDescription""";
            //}
            //query = query.Replace("#QUESTIONLANGUAGE#", queslang);

            var list = await _queryAssDetail.ExecuteQueryList<SurveyTopicViewModel>(query, null);

            //if (list != null && list.Count != 0)
            //{
            //    if (currTopicId.IsNullOrEmpty())
            //    {
            //        result.TopicId = list.Where(x => x.TopicSequenceNo == 1).Select(x => x.TopicId).FirstOrDefault();
            //        result.NextId = list.Where(x => x.TopicSequenceNo == 2).Select(x => x.TopicId).FirstOrDefault();
            //        result.TopicName = list.Where(x => x.TopicSequenceNo == 1).Select(x => x.TopicName).FirstOrDefault();
            //    }
            //    else
            //    {
            //        result.TopicId = currTopicId;
            //        var seq = list.Where(x => x.TopicId == currTopicId).Select(x => x.TopicSequenceNo).FirstOrDefault();
            //        var next = seq + 1;
            //        var prev = seq - 1;
            //        result.PreviousId = list.Where(x => x.TopicSequenceNo == prev).Select(x => x.TopicId).FirstOrDefault();
            //        result.NextId = list.Where(x => x.TopicSequenceNo == next).Select(x => x.TopicId).FirstOrDefault();
            //        result.TopicName = list.Where(x => x.TopicId == currTopicId).Select(x => x.TopicName).FirstOrDefault();

            //    }
            //    result.FirstId = list.First().TopicId;
            //    result.LastId = list.Last().TopicId;
            //    // result.AssessmentId = list.First().AssessmentId;
            //    //result.OptionList = new List<AssessmentQuestionsOptionViewModel>();

            //    if (result.TopicId.IsNotNull())
            //    {
            //        result.QuestionList = new List<SurveyQuestionViewModel>();
            //        var listQuestion = list.Where(x => x.TopicId == result.TopicId).ToList();
            //        foreach (var question in listQuestion)
            //        {
            //            var survey = new SurveyQuestionViewModel();
            //            survey.QuestionId = question.Id;
            //            survey.QuestionName = HttpUtility.HtmlDecode(question.Question);


            //            survey.OptionList = await GetSurveyOptions(question.Id, serviceId, lang, list.First().AssessmentId, question.SurveyScheduleUserId);
            //            if (survey.OptionList != null && survey.OptionList.Count > 0)
            //            {
            //                var answer = survey.OptionList.Where(x => x.AnswerId != null).FirstOrDefault();
            //                if (answer != null)
            //                {
            //                    survey.AnswerComment = answer.AnswerComment;
            //                    //result.Score = answer.Score;
            //                    survey.AnswerId = answer.AnswerId;
            //                }
            //            }
            //            result.QuestionList.Add(survey);
            //        }
            //    }
            //}

            return list;
        }
        public async Task<bool> CheckIfSurveyAnswerResultExist(string questionId, string surveyResultId)
        {
            var query = $@"select ar.*
From cms.""N_SURVEY_S_SurveyResultAnswer"" as ar 
where ar.""QuestionId""='{questionId}' and ar.""SurveyResultId""='{surveyResultId}' and ar.""IsDeleted""=false and ar.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var data = await _queryAssAns.ExecuteQuerySingle<SurveyTopicViewModel>(query, null);
            if (data.IsNotNull())
            {
                return true;
            }
            return false;
        }
        

        public async Task<MemoryStream> GetBusinessUserSurveyReportDataExcel()
        {

            var query = $@"Select ""Question"" as QuestionName, ""Answer"" as AnswerName, ""Comments"" as AnswerComment,
""Division"",""CareerLevel"",""Market"",""TypeofQuestion"" as HRFunction from public.""SurveyResultBusinessUser"" 
where ""IsDeleted""=false ";

            var model = await _queryRepo.ExecuteQueryList<SurveyQuestionViewModel>(query, null);

            var ms = new MemoryStream();
            var reportList = new List<string>();
            //{ "Assessment Report", "Case Study Data", "Case Study Report" };
            //reportList.Add("Survey Data");
            
            using (var sl = new SLDocument())
            {
                   //sl.AddWorksheet("Survey Data");

                   // SLPageSettings pageSettings = new SLPageSettings();
                   // pageSettings.ShowGridLines = true;
                   // sl.SetPageSettings(pageSettings);

                   // sl.SetColumnWidth("A", 20);
                   // sl.SetColumnWidth("B", 20);
                   // sl.SetColumnWidth("C", 20);                    

                   // sl.SetCellValue("A1", "Question");
                   // sl.SetCellStyle("A1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                   // sl.SetCellValue("B1", "Answer");
                   // sl.SetCellStyle("B1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                   // sl.SetCellValue("C1", "Comment");
                   // sl.SetCellStyle("C1", ExcelHelper.GetHeaderRowCayanStyle(sl));
                    
                
                    sl.AddWorksheet("Survey Data - Business User");
                    var modelData = new SurveyQuestionViewModel();
                    new SLPageSettings().ShowGridLines = true;
                    sl.SetPageSettings(new SLPageSettings());

                    sl.SetColumnWidth("A", 30);
                    sl.SetColumnWidth("B", 20);
                    sl.SetColumnWidth("C", 20);
                    sl.SetColumnWidth("D", 30);
                    sl.SetColumnWidth("E", 60);
                    sl.SetColumnWidth("F", 20);
                    sl.SetColumnWidth("G", 60);

                    sl.SetCellValue("A1", "Division");
                    sl.SetCellStyle("A1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                    sl.SetCellValue("B1", "Career Level");
                    sl.SetCellStyle("B1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                    sl.SetCellValue("C1", "Market");
                    sl.SetCellStyle("C1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                    sl.SetCellValue("D1", "HR Function");
                    sl.SetCellStyle("D1",  ExcelHelper.GetHeaderRowHeadingStyle(sl));
                    sl.SetCellValue("E1", "Question");
                    sl.SetCellStyle("E1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                    sl.SetCellValue("F1", "Answer");
                    sl.SetCellStyle("F1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                    sl.SetCellValue("G1", "Comment");
                    sl.SetCellStyle("G1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                                                         

                        if (model.Count > 0)
                        {   
                            int row = 2;
                                                        
                            Int32 maxNo = model.Count;                                                        
                            
                            foreach (var item in model)
                            {
                                char first = 'A';

                                first = (char)((int)first);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), item.Division);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), item.CareerLevel);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), item.Market);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), item.HRFunction);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), item.QuestionName);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), item.AnswerName);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), item.AnswerComment);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                row++;
                            }
                        }                                    
               
                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);

            }
            ms.Position = 0;
            return ms;
        }

        public async Task<MemoryStream> GetHRUserSurveyReportDataExcel()
        {

            var query = $@"Select ""Question"" as QuestionName, ""Answer"" as AnswerName, ""Comments"" as AnswerComment,
""Division"",""CareerLevel"",""Market"",""TypeofQuestion"" as HRFunction from public.""SurveyResultHRUser"" 
where ""IsDeleted""=false ";

            var model = await _queryRepo.ExecuteQueryList<SurveyQuestionViewModel>(query, null);

            var ms = new MemoryStream();
            var reportList = new List<string>();
            //{ "Assessment Report", "Case Study Data", "Case Study Report" };

            //reportList.Add("Survey Data");

            using (var sl = new SLDocument())
            {
                //sl.AddWorksheet("Survey Data");

                // SLPageSettings pageSettings = new SLPageSettings();
                // pageSettings.ShowGridLines = true;
                // sl.SetPageSettings(pageSettings);

                // sl.SetColumnWidth("A", 20);
                // sl.SetColumnWidth("B", 20);
                // sl.SetColumnWidth("C", 20);                    

                // sl.SetCellValue("A1", "Question");
                // sl.SetCellStyle("A1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                // sl.SetCellValue("B1", "Answer");
                // sl.SetCellStyle("B1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                // sl.SetCellValue("C1", "Comment");
                // sl.SetCellStyle("C1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                
                    sl.AddWorksheet("Survey Data - HR User");
                    var modelData = new SurveyQuestionViewModel();
                    new SLPageSettings().ShowGridLines = true;
                    sl.SetPageSettings(new SLPageSettings());

                    sl.SetColumnWidth("A", 30);
                    sl.SetColumnWidth("B", 20);
                    sl.SetColumnWidth("C", 20);
                    sl.SetColumnWidth("D", 30);
                    sl.SetColumnWidth("E", 60);
                    sl.SetColumnWidth("F", 20);
                    sl.SetColumnWidth("G", 60);

                    sl.SetCellValue("A1", "Division");
                    sl.SetCellStyle("A1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                    sl.SetCellValue("B1", "Career Level");
                    sl.SetCellStyle("B1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                    sl.SetCellValue("C1", "Market");
                    sl.SetCellStyle("C1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                    sl.SetCellValue("D1", "HR Function");
                    sl.SetCellStyle("D1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                    sl.SetCellValue("E1", "Question");
                    sl.SetCellStyle("E1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                    sl.SetCellValue("F1", "Answer");
                    sl.SetCellStyle("F1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                    sl.SetCellValue("G1", "Comment");
                    sl.SetCellStyle("G1", ExcelHelper.GetHeaderRowHeadingStyle(sl));
                                    
                        if (model.Count>0)
                        {
                            int row = 2;

                            Int32 maxNo = model.Count;

                            foreach (var item in model)
                            {
                                char first = 'A';

                                first = (char)((int)first);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), item.Division);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), item.CareerLevel);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), item.Market);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), item.HRFunction);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), item.QuestionName);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), item.AnswerName);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                first = (char)((int)first + 1);
                                sl.MergeWorksheetCells(string.Concat(first, row), string.Concat(first, row));
                                sl.SetCellValue(string.Concat(first, row), item.AnswerComment);
                                sl.SetCellStyle(string.Concat(first, row), string.Concat(first, row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                row++;
                            }
                        }

                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);

            }
            ms.Position = 0;
            return ms;
        }


    }
}

