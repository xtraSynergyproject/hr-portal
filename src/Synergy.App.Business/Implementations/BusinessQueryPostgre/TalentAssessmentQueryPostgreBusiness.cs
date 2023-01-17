using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class TalentAssessmentQueryPostgreBusiness : BusinessBase<NoteViewModel, NtsNote>, ITalentAssessmentQueryBusiness
    {
        IUserContext _uc;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private IServiceBusiness _serviceBusiness;
        public TalentAssessmentQueryPostgreBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IMapper autoMapper
            , IUserContext uc
            , IServiceBusiness serviceBusiness
            , IRepositoryQueryBase<NoteViewModel> queryRepo) : base(repo, autoMapper)
        {
            _uc = uc;
            _queryRepo = queryRepo;
            _serviceBusiness = serviceBusiness;
        }

        public async Task<List<SurveyQuestionViewModel>> GetHRUserSurveyData()
        {

            var query = $@"Select ""Question"" as QuestionName, ""Answer"" as AnswerName, ""Comments"" as AnswerComment,
""Division"",""CareerLevel"",""Market"",""TypeofQuestion"" as HRFunction from public.""SurveyResultHRUser"" 
where ""IsDeleted""=false ";

            var model = await _queryRepo.ExecuteQueryList<SurveyQuestionViewModel>(query, null);
            return model;
        }

        public async Task<List<SurveyTopicViewModel>> GetQuestionsForSurveyResult(string surveyId)
        {


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

            var list = await _queryRepo.ExecuteQueryList<SurveyTopicViewModel>(query, null);

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
        public async Task<SurveyTopicViewModel> CheckIfSurveyAnswerResultExist(string questionId, string surveyResultId)
        {
            var query = $@"select ar.*
From cms.""N_SURVEY_S_SurveyResultAnswer"" as ar 
where ar.""QuestionId""='{questionId}' and ar.""SurveyResultId""='{surveyResultId}' and ar.""IsDeleted""=false and ar.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var data = await _queryRepo.ExecuteQuerySingle<SurveyTopicViewModel>(query, null);
            return data;
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

            var result = await _queryRepo.ExecuteQueryList<SurveyQuestionViewModel>(query, null);



            return result;
        }

        public async Task<List<ServiceTemplateViewModel>> GetSurveyScheduleUser(string SurveyScheduleId, string UserId)
        {
            var query = $@"select * from cms.""N_SURVEY_SurveyScheduleUser"" where ""SurveyUserId""='{UserId}' and ""SurveyScheduleId""='{SurveyScheduleId}' and ""IsDeleted""=false";
            var data = await _queryRepo.ExecuteQueryList<ServiceTemplateViewModel>(query, null);
            return data;
        }

        public async Task UpdateScheduleSurveyUserExpiryDate(string surveyScheduleId, string expiryDate)
        {
            var query = $@"Update cms.""N_SURVEY_SurveyScheduleUser"" set ""SurveyExpiryDate""='{expiryDate}' where ""SurveyScheduleId""='{surveyScheduleId}'";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task<SurveyScheduleViewModel> GetSurveyCode(int surveyCode)
        {
            var query = @$"select s.""SurveyCode"" from cms.""N_SURVEY_SurveyScheduleUser"" as s where s.""SurveyCode""='{surveyCode}'
                and s.""IsDeleted""=false";
            var result = await _queryRepo.ExecuteQuerySingle<SurveyScheduleViewModel>(query, null);
            return result;
        }

        public async Task<List<SurveyScheduleViewModel>> GetSurveyScheduleDetailsById(string surveyId, string surveyScheduleId)
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
            return result;
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
        public async Task<SurveyScheduleViewModel> GetSurveyScheduleById(string surveyId)
        {
            var query = @$"select * from cms.""N_SURVEY_SurveySchedule"" where ""SurveyId""='{surveyId}'";
           return await _queryRepo.ExecuteQuerySingle<SurveyScheduleViewModel>(query, null);
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
        public async Task<SurveyScheduleViewModel> AssignDummySurveyToUser(string surveyScheduleId, string surveyUserId)
        {
            var query = $@"select *,""Id"" as ""SurveyScheduleUserId"" from cms.""N_SURVEY_SurveyScheduleUser"" where ""SurveyUserId""='{surveyUserId}' 
            and ""SurveyScheduleId""='{surveyScheduleId}' and ""IsDeleted""=false";
            var surveyScheduleUser = await _queryRepo.ExecuteQuerySingle<SurveyScheduleViewModel>(query, null);
            if (surveyScheduleUser != null)
            {
                return surveyScheduleUser;
            }
            query = $@"update cms.""N_SURVEY_SurveyScheduleUser"" set ""SurveyUserId""='{surveyUserId}'
            where ""IsDeleted""=false and  ""SurveyUserId"" is null and ""SurveyScheduleId""='{surveyScheduleId}' 
           and ""Id""=(select ""Id"" from cms.""N_SURVEY_SurveyScheduleUser"" where ""IsDeleted""=false and 
              ""SurveyUserId"" is null and ""SurveyScheduleId"" = '{surveyScheduleId}' limit 1)
		      ";
            await _queryRepo.ExecuteCommand(query, null);

            query = $@"select *,""Id"" as ""SurveyScheduleUserId"" from cms.""N_SURVEY_SurveyScheduleUser"" where ""SurveyUserId""='{surveyUserId}' 
            and ""SurveyScheduleId""='{surveyScheduleId}'  and ""IsDeleted""=false";

            surveyScheduleUser = await _queryRepo.ExecuteQuerySingle<SurveyScheduleViewModel>(query, null);
            if (surveyScheduleUser != null)
            {
                query = $@"update cms.""N_SURVEY_S_SurveyResult"" set ""SurveyUserId""='{surveyUserId}'
                where ""IsDeleted""=false and  ""SurveyUserId"" is null and ""SurveyScheduleUserId""='{surveyScheduleUser.SurveyScheduleUserId}'";
                await _queryRepo.ExecuteCommand(query, null);

                query = $@"select * from cms.""N_SURVEY_S_SurveyResult"" 
                where ""SurveyUserId""='{surveyUserId}' and ""SurveyScheduleUserId""='{surveyScheduleUser.SurveyScheduleUserId}'
                and ""IsDeleted"" = false";
                var surveyResult = await _queryRepo.ExecuteQuerySingle<SurveyResultViewModel>(query, null);

                query = $@"update cms.""N_SURVEY_S_SurveyResultAnswer"" set ""UserId""='{surveyUserId}'
                where ""IsDeleted""=false and  ""UserId"" is null and ""SurveyResultId""='{surveyResult.Id}'";
                await _queryRepo.ExecuteCommand(query, null);
            }
            return surveyScheduleUser;
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
            var result = await _queryRepo.ExecuteQueryList<AssessmentQuestionsOptionViewModel>(query, null);
            return result;
        }

        public async Task<List<AssessmentDetailViewModel>> GetAssessmentData(string serviceId, string lang)
        {
            var user = await _serviceBusiness.GetSingleById<UserViewModel, User>(_repo.UserContext.UserId);
            string isodd = "true";
            if (user.SequenceOrder % 2==0)
            {
                isodd = "false";
            }
            var query = $@"select nt.""Id"" as ""TopicId"",nt.""NoteSubject"" as TopicName,aq.""QuestionId"" as Id,aq.""SequenceOrder"" as SequenceNo,
case when '"+ isodd + @$"' = 'false' then  top.""SequenceOrder""::int  else top.""ReverseSequenceOrder""::int end as TopicSequenceNo, #QUESTIONLANGUAGE#,q.""NtsNoteId"" as NoteId
      ,q.""EnableComment"" as EnableComment,ar.""SurveyId""  as AssessmentId,ar.""SurveyScheduleUserId"" as SurveyScheduleUserId From public.""NtsService"" as s
Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""Code""='S_SURVEY_RESULT' and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
Join public.""NtsNote"" as nts on s.""UdfNoteId""=nts.""Id"" and nts.""IsDeleted""=false  and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_SURVEY_S_SurveyResult"" as ar on nts.""Id""=ar.""NtsNoteId"" and ar.""IsDeleted""=false and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_Assessment"" as a on ar.""SurveyId""=a.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_AssessmentQuestion"" as aq on a.""Id""=aq.""AssessmentId"" and aq.""IsDeleted""=false and aq.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_Question"" as q on aq.""QuestionId""=q.""Id"" and q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as n on q.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
Join public.""NtsNote"" as nt on n.""ParentNoteId""=nt.""Id"" and nt.""IsDeleted""=false and nt.""CompanyId""='{_repo.UserContext.CompanyId}'
Join cms.""N_TAS_Topic"" as top on top.""NtsNoteId""=nt.""Id"" and top.""IsDeleted""=false and top.""CompanyId""='{_repo.UserContext.CompanyId}'
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

            var list = await _queryRepo.ExecuteQueryList<AssessmentDetailViewModel>(query, null);
            return list;
        }

        public async Task UpdatePreferredLanguage(string surveyScheduleId, string lang, string surveyResultId)
        {
            var query = $@"Update cms.""N_SURVEY_S_SurveyResult"" set ""PreferredLanguage""='{lang}' where ""Id""='{surveyResultId}';
            Update cms.""N_SURVEY_S_SurveyResult"" set ""SurveyStartDate""='{DateTime.Now.ToDatabaseDateFormat()}' where ""Id""='{surveyResultId}' and ""SurveyStartDate"" is null";
            await _queryRepo.ExecuteCommand(query, null);
        }

        //Survey

        public async Task<AssessmentDetailViewModel> GetSurveyAssessmentQuestion(string serviceId)
        {
            var que = $@"Select ar.""CurrentTopicId""
                           From public.""NtsService"" as s
                           Join public.""NtsNote"" as n on s.""UdfNoteId""=n.""Id"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                           Join cms.""N_SURVEY_S_SurveyResult"" as ar on n.""Id""=ar.""NtsNoteId"" and ar.""IsDeleted""=false  and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
                           where s.""Id""='{serviceId}'  and s.""IsDeleted""=false  and s.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var res = await _queryRepo.ExecuteQuerySingle<AssessmentDetailViewModel>(que, null);
            return res;

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

        public async Task DeleteAssessmentSet(string NoteId)
        {
            var query = $@"Update cms.""N_TAS_AssessmentSet"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);

        }

        public async Task DeleteAssessmentSetAsessment(string NoteId)
        {
            var query = $@"Update cms.""N_TAS_AssessmentSetAssessment"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);

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

            var Result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(Query, null);
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

            var Result = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(Query, null);
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

            var Result = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(Query, null);
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

            var Result = await _queryRepo.ExecuteQueryList<AssessmentInterviewViewModel>(Query, null);
            return Result;

        }

        public async Task<IdNameViewModel> GetSponsorDetailsByUserId(string UserId)
        {
            var Query = $@"Select ""SponsorId"" as Id,""Name"" from  public.""User"" where ""Id""='{UserId}'";
            var result = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(Query, null);
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

            var result = await _queryRepo.ExecuteQueryList<AssessmentDetailViewModel>(query, null);

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

            var result = await _queryRepo.ExecuteQueryList<AssessmentInterviewViewModel>(Query, null);
            return result;
        }

        public async Task<IdNameViewModel> GetIndicatorbyName(string Name)
        {
            var query = @$"select I.""Id"",I.""IndicatorName"" as Name 
from cms.""N_TAS_Indicator"" I inner join public.""NtsNote"" N on I.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false
where I.""IndicatorName""='{Name}' and I.""IsDeleted""=false";


            var result = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return result;
        }

        public async Task<IdNameViewModel> GetCompetencybyName(string Name)
        {
            var query = @$"select I.""Id"",I.""CompetencyLevel"" as Name 
from cms.""N_TAS_CompetencyLevel"" I inner join public.""NtsNote"" N on I.""NtsNoteId""=N.""Id"" and N.""IsDeleted""=false
where I.""CompetencyLevel""='{Name}' and I.""IsDeleted""=false";


            var result = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return result;
        }


        public async Task<IdNameViewModel> GetAssessmentbyName(string Name)
        {
            var query = @$"select ""Id"" ,""Name"" from public.""LOV""  where ""LOVType""='Assessment_Type' and ""Name""='{Name}'";


            var result = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
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
            var result = await _queryRepo.ExecuteQuerySingle<SurveyScheduleViewModel>(query, null);
            return result;
        }

        public async Task DeleteAssessmentSchedule(string id)
        {
            var query = $@"Update cms.""N_TAS_UserAssessmentSchedule"" set ""IsDeleted""=true where ""Id""='{id}'";
            await _queryRepo.ExecuteCommand(query, null);
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


            var result = await _queryRepo.ExecuteQueryList<AssessmentDetailViewModel>(query, null);
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


            var result = await _queryRepo.ExecuteQuerySingle<AssessmentDetailViewModel>(query, null);
            return result;

        }


        public async Task<bool> UpdateUserResult(string Id, string Scores)
        {
            var query = $@"Update cms.""N_TAS_UserAssessmentResult"" set ""Score""='{Scores}' where ""Id""='{Id}' ";

            await _queryRepo.ExecuteCommand(query, null);


            return true;

        }

        public async Task<bool> UpdateInterViewScore(string Id, string Scores, string ActualStartDate, string ActualEndDate)
        {
            var query = $@"Update cms.""N_TAS_AssessmentInterview"" set ""Score""='{Scores}',""ActualStartDate""='{ActualStartDate}',""ActualEndDate""='{ActualEndDate}' where ""Id""='{Id}' ";

            await _queryRepo.ExecuteCommand(query, null);


            return true;

        }

        public async Task<IdNameViewModel> GetCanidateId(string UserId)
        {
            var Query = $@"select n.""Id"" from cms.""N_TAS_CandidateId"" as c inner join public.""NtsNote"" As n  on n.""Id""=c.""NtsNoteId"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
             where c.""UserId"" = '{UserId}' and c.""IsDeleted""=false  and c.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(Query, null);
            return result;
        }

        public async Task<bool> UpdateAssessmentArchived(string serviceIds, bool archive = false)
        {
            var cypher = $@"Update Public.""NtsService"" as s set ""IsArchived""={archive} where s.""Id"" in ({serviceIds})";

            await _queryRepo.ExecuteCommand(cypher, null);
            return true;
        }


        public async Task<IdNameViewModel> Getinterviewid(string ServiceId)
        {

            var query = $@"select I.* from public.""NtsService"" as sr
inner join public.""NtsNote"" as n on n.""Id""=sr.""UdfNoteId"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join cms.""N_TAS_AssessmentInterview"" as I on I.""NtsNoteId""=n.""Id"" and I.""IsDeleted""=false  and I.""CompanyId""='{_repo.UserContext.CompanyId}'
where sr.""Id""='{ServiceId}' and sr.""IsDeleted""=false  and sr.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var res = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
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
            await _queryRepo.ExecuteCommand(query, null);
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

            var result = await _queryRepo.ExecuteQuerySingle<AssessmentInterviewViewModel>(Query, null);
            return result;
        }

        public async Task<List<InterViewQuestionViewModel>> GetInterviewAssessmentQuestions(string userId, string Tableid)
        {


            var query = $@"select i.""SequenceOrder"",""Score"" from  cms.""N_TAS_InterviewQuestion"" as i
                         join public.""NtsNote"" as n on n.""Id""=i.""NtsNoteId"" and n.""ParentServiceId""='{Tableid}' and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'   
                        where i.""UserId""='{userId}' and  i.""IsDeleted""=false  and i.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var result = await _queryRepo.ExecuteQueryList<InterViewQuestionViewModel>(query, null);
            return result;
        }

        public async Task<List<AssessmentQuestionsOptionViewModel>> GetOptionList(string QuestionId)
        {

            var query = $@"select o.""SequenceOrder"",o.""Id"" as OptionId,o.""Option"",o.""OptionArabic"",o.""IsRightAnswer"",o.""Score"" 
from cms.""N_TAS_QuestionOption"" o
inner join public.""NtsNote"" N on N.""Id""=o.""NtsNoteId""  and N.""IsDeleted""=false  and N.""CompanyId""='{_repo.UserContext.CompanyId}'
where ""ParentNoteId""='{QuestionId}' and o.""IsDeleted""=false  and o.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var res = await _queryRepo.ExecuteQueryList<AssessmentQuestionsOptionViewModel>(query, null);
            return res;


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

            var res = await _queryRepo.ExecuteQueryList<AssessmentAnswerViewModel>(query, null);
            return res;
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


            var result = await _queryRepo.ExecuteQuerySingle<AssessmentDetailViewModel>(query, null);


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


            var result = await _queryRepo.ExecuteQueryList<AssessmentDetailViewModel>(query, null);

            return result;



        }

        public async Task<List<InterViewQuestionViewModel>> GetRandamQuestions()
        {

            var Query = $@"select n.""NoteSubject"",q.""Question"",n.""NoteDescription"" from cms.""N_TAS_Question"" q inner join public.""NtsNote"" n on q.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
             where  q.""IsDeleted""=false  and q.""CompanyId""='{_repo.UserContext.CompanyId}' order by random()  limit 3";

            var result = await _queryRepo.ExecuteQueryList<InterViewQuestionViewModel>(Query, null);
            return result;


        }


        public async Task<List<InterViewQuestionViewModel>> GetQuestionDetails(string UserId, string TableId)
        {

            var Query = $@"select n.""NoteSubject"",q.""Question"",n.""NoteDescription"" 
    from cms.""N_TAS_Question"" q inner join public.""NtsNote"" n on q.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
where  q.""IsDeleted""=false  and q.""CompanyId""='{_repo.UserContext.CompanyId}'
   order by random()  limit 3";

            var result = await _queryRepo.ExecuteQueryList<InterViewQuestionViewModel>(Query, null);
            return result;


        }


        public async Task<List<InterViewQuestionViewModel>> GetQuestionListofUser(string UserId, string TableId)
        {

            var Query = $@"select n.""Id"" as NoteId,  n.""NoteSubject"",n.""NoteDescription"", q.* 
from cms.""N_TAS_InterviewQuestion"" q inner join public.""NtsNote"" n on q.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
where q.""UserId"" = '{UserId}' and n.""ParentServiceId"" = '{TableId}' and q.""IsDeleted""=false  and q.""CompanyId""='{_repo.UserContext.CompanyId}'
order by q.""SequenceOrder"" ";

            var result = await _queryRepo.ExecuteQueryList<InterViewQuestionViewModel>(Query, null);
            return result;


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

            var result = await _queryRepo.ExecuteQuerySingle<AssessmentInterviewViewModel>(Query, null);
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

            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
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
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task<List<AssessmentInterviewViewModel>> GetAssessmentInterview(string UserID, bool isarchived = false, string source = null)
        {
            var Query = "";
            if (_uc.IsSystemAdmin || source == "VIEWALL")
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
            else if (_uc.UserRoleCodes.Contains("INTERVIEWER"))
            {
                Query = $@"select distinct u.""Id"" as Id,sr.""Id"" as ServiceId, sr.""ServiceSubject"" as Subject,sr.""IsArchived"" as IsRowArchived, u.""Name""
 as OwnerUserName, L.""Name"" as AssessmentStatus,u.""Id"" as OwnerUserId,sc.""AttachmentId"" as ""CandidateCVId"",
'AssessmentInterview' as AssessmentType,false as IsAssessmentStopped,i.""Id"" as ""TableId"",u.""JobTitle"" as JobTitle,
sc.""AssessmentStartDate"" as ScheduledStartDate,sc.""Id"" as ""InterviewScheduleId"" ,u.""Email"" as Email,t.""Name"" as PanelTeamName,sc.""Url"" as AssessmentInterviewUrl,c.""InterviewSheetId""
from public.""User"" as u
inner join cms.""N_TAS_UserAssessmentSchedule"" as sc on sc.""UserId""=u.""Id"" and sc.""IsDeleted""=false and sc.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join public.""Team""  as t on  t.""Id""=sc.""InterviewerId""  and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join public.""TeamUser""  as tm on t.""Id""=tm.""TeamId"" and tm.""UserId""='{_uc.UserId}' and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
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
where u.""Id""='{_uc.UserId}' and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and sc.""SlotType""='1'"; }

            var result = await _queryRepo.ExecuteQueryList<AssessmentInterviewViewModel>(Query, null);
            return result;


        }

        public async Task ManageAssessmentStatus(bool isAssessmentStopped, ServiceViewModel sermodel)
        {
            if (!isAssessmentStopped)
            {
                var query = $@"Update cms.""N_TAS_UserAssessmentResult"" set ""IsAssessmentStopped""='{isAssessmentStopped.ToString()}'
                                where ""Id""='{sermodel.UdfNoteTableId}' ";

                await _queryRepo.ExecuteCommand(query, null);
            }
            else
            {
                var query = $@"Update cms.""N_TAS_UserAssessmentResult"" set ""IsAssessmentStopped""='{isAssessmentStopped.ToString()}'
                                where ""Id""='{sermodel.UdfNoteTableId}' ";

                await _queryRepo.ExecuteCommand(query, null);
            }
        }

        public async Task UpdateSurvey(ServiceTemplateViewModel service, string nextQTopicId = null)
        {
            var query1 = $@"Update cms.""N_SURVEY_S_SurveyResult"" set ""CurrentTopicId""='{nextQTopicId}' 
                           where ""Id""='{service.UdfNoteTableId}'";

            //var setdate = "";
            //if (startDate.IsNotNull())
            //{
            //    setdate = $@", ""StartDate""='{startDate}' ";
            //}
            //query1 = query1.Replace("#SETDATE#", setdate);
            await _queryRepo.ExecuteCommand(query1, null);

            //var query = $@"Select ""IsAssessmentStopped"" from public.""NtsService"" as s
            //            Join public.""NtsNote"" as n on s.""UdfNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
            //            join cms.""N_TAS_UserAssessmentResult"" as ar on n.""Id""=ar.""NtsNoteId"" and ar.""IsDeleted""=false and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
            //            where s.""Id""='{serviceId}' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'";
            //var result = await _queryRepo.ExecuteScalar<bool>(query, null);


        }

        public async Task UpdateAssessment(ServiceTemplateViewModel service, int? timeElapsed, int? timeElapsedSec, string nextQuestionId = null, DateTime? startDate = null)
        {
            var query1 = $@"Update cms.""N_TAS_UserAssessmentResult"" set ""TimeElapsed""='{timeElapsed}', ""TimeElapsedSec""='{timeElapsedSec}', ""CurrentQuestionId""='{nextQuestionId}' #SETDATE#
                           where ""Id""='{service.UdfNoteTableId}'";

            var setdate = "";
            if (startDate.IsNotNull())
            {
                setdate = $@", ""StartDate""='{startDate}' ";
            }
            query1 = query1.Replace("#SETDATE#", setdate);
            await _queryRepo.ExecuteCommand(query1, null);
        }

        public async Task<bool> GetAssessmentStatus(string serviceId)
        {
            var query = $@"Select ""IsAssessmentStopped"" from public.""NtsService"" as s
                        Join public.""NtsNote"" as n on s.""UdfNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_TAS_UserAssessmentResult"" as ar on n.""Id""=ar.""NtsNoteId"" and ar.""IsDeleted""=false and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
                        where s.""Id""='{serviceId}' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var result = await _queryRepo.ExecuteScalar<bool>(query, null);
            return result;
        }

        public async Task<AssessmentAnswerViewModel> GetAssessmentAnswer(string serviceId, string currentQuestionId)
        {
            var arquery = $@"select ans.* from public.""NtsService"" as s
join public.""NtsNote"" as n on s.""UdfNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_TAS_UserAssessmentAnswer"" as ans on n.""Id""=ans.""NtsNoteId"" and ans.""QuestionId""='{currentQuestionId}' and ans.""IsDeleted""=false and ans.""CompanyId""='{_repo.UserContext.CompanyId}'
where s.""ParentServiceId""='{serviceId}' and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _queryRepo.ExecuteQuerySingle<AssessmentAnswerViewModel>(arquery, null);
            return result;
        }

        public async Task<AssessmentResultViewModel> GetAssessmentResult(ServiceTemplateViewModel service1)
        {
            var query = $@"Select * from cms.""N_TAS_UserAssessmentResult"" where ""NtsNoteId""='{service1.UdfNoteId}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var assResult = await _queryRepo.ExecuteQuerySingle<AssessmentResultViewModel>(query, null);
            return assResult;
        }

        public async Task UpdateAssessmentResult(ServiceTemplateViewModel service1)
        {
            var query1 = $@"Update cms.""N_TAS_UserAssessmentResult"" set ""EndDate""='{DateTime.Now}'
                                where ""Id""='{service1.UdfNoteTableId}'";
            await _queryRepo.ExecuteCommand(query1, null);
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
            var result = await _queryRepo.ExecuteQueryList<AssessmentQuestionsOptionViewModel>(query, null);
            return result;
        }

        public async Task UpdateScoreComment(List<AssessmentAnswerViewModel> modelList)
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

            await _queryRepo.ExecuteCommand(query, null);

        }

        public async Task<AssessmentAnswerViewModel> GetQuestionByAnswerId(string noteId)
        {
            var query = $@"select * from cms.""N_TAS_UserAssessmentAnswer"" where ""NtsNoteId""='{noteId}' ";
            var result = await _queryRepo.ExecuteQuerySingle<AssessmentAnswerViewModel>(query, null);
            return result;
        }

        public async Task<List<AssessmentDetailViewModel>> GetAssessmentQuestionsDetail(string serviceId, string lang)
        {
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

            var list = await _queryRepo.ExecuteQueryList<AssessmentDetailViewModel>(query, null);
            return list;
        }

        public async Task<AssessmentDetailViewModel> GetAssessmentQuestionArabic(string serviceId)
        {
            var que = $@"Select ar.""CurrentQuestionId""
                           From public.""NtsService"" as s
                           Join public.""NtsNote"" as n on s.""UdfNoteId""=n.""Id"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                           Join cms.""N_TAS_UserAssessmentResult"" as ar on n.""Id""=ar.""NtsNoteId"" and ar.""IsDeleted""=false  and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
                           where s.""Id""='{serviceId}'  and s.""IsDeleted""=false  and s.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var res = await _queryRepo.ExecuteQuerySingle<AssessmentDetailViewModel>(que, null);
            return res;


        }

        public async Task<AssessmentDetailViewModel> GetAssessmentQuestionEnglish(string serviceId)
        {
            var que = $@"Select ar.""CurrentQuestionId""
                           From public.""NtsService"" as s
                           Join public.""NtsNote"" as n on s.""UdfNoteId""=n.""Id"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                           Join cms.""N_TAS_UserAssessmentResult"" as ar on n.""Id""=ar.""NtsNoteId"" and ar.""IsDeleted""=false and ar.""UserId""='{_repo.UserContext.UserId}' and ar.""CompanyId""='{_repo.UserContext.CompanyId}'
                           where s.""Id""='{serviceId}' and s.""IsDeleted""=false  and s.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var res = await _queryRepo.ExecuteQuerySingle<AssessmentDetailViewModel>(que, null);
            return res;

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
            var result = await _queryRepo.ExecuteQueryList<AssessmentQuestionsOptionViewModel>(query, null);
            return result;
        }

        public async Task<List<AssessmentDetailViewModel>> GetQuestions(string assessmentId, string lang)
        {
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

            var list = await _queryRepo.ExecuteQueryList<AssessmentDetailViewModel>(query, null);
            return list;
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





            var queryData = await _queryRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
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


            var queryData = await _queryRepo.ExecuteQueryList<TeamWorkloadViewModel>(query, null);
            return queryData;
        }

        public async Task<IList<IdNameViewModel>> GetCompetencyList()
        {
            var query = $@"Select c.""Id"", c.""CompetencyLevel"" as Name
                            From public.""NtsNote"" as n
                            join cms.""N_TAS_CompetencyLevel"" as c on n.""Id""=c.""NtsNoteId"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}' ";

            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result;
        }

        public async Task<List<AssignmentViewModel>> ReadCertificationData(string userId)
        {
            var query = $@"select hp.""Id"",u.""Id"", crf.""CertificationName"" as ""CertificationName"",crf.""CertificateReferenceNo"" as ""CertificateReferenceNo"",crf.""ExpiryLicenseDate"" as ""ExpiryLicenseDate""
				from cms.""N_CoreHR_HRPerson"" as hp
				left join public.""User"" as u on u.""Id""=hp.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join cms.""N_TAS_Certification"" as crf on crf.""UserId""=hp.""UserId"" and crf.""IsDeleted""=false and crf.""CompanyId""='{_repo.UserContext.CompanyId}'
				where hp.""IsDeleted""=false and u.""Id"" = '{userId}'  and hp.""CompanyId""='{_repo.UserContext.CompanyId}'";
            return await _queryRepo.ExecuteQueryList<AssignmentViewModel>(query, null);
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
            return await _queryRepo.ExecuteQueryList<AssignmentViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQueryList<AssessmentDetailViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQueryList<AssessmentDetailViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
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
            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return result.ToList();
        }

        public async Task<IList<IdNameViewModel>> GetCompetencyLevelList(string noteId = null)
        {
            var query = $@"Select ind.""Id"", ind.""CompetencyLevel"" as Name
                            From cms.""N_TAS_CompetencyLevel"" as ind
                            join public.""NtsNote"" as n on ind.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where ind.""IsDeleted""=false and ind.""CompanyId""='{_repo.UserContext.CompanyId}' ";

            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQueryList<AssessmentDetailViewModel>(query, null);

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

            var result = await _queryRepo.ExecuteQuerySingle<AssessmentResultViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQuerySingle<SurveyTopicViewModel>(query, null);
            return result;
        }

        public async Task<SurveyScheduleViewModel> GetSurveyClosingInstruction(string surveyId)
        {
            var query = $@"select ss.*,sslov.""Code"" as LanguageCode, sslov.""Name"" as PreferredLanguageName
                            from cms.""N_SURVEY_SurveySchedule"" as ss
                            left join public.""LOV"" as sslov on ss.""PreferredLanguage""=sslov.""Id"" and sslov.""IsDeleted""=false
                            WHERE ss.""IsDeleted""=false and ss.""SurveyId""='{surveyId}'  and ss.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _queryRepo.ExecuteQuerySingle<SurveyScheduleViewModel>(query, null);
            return result;
        }

        public async Task DeleteAssessmentQuestion(string NoteId)
        {
            var query = $@"Update cms.""N_TAS_AssessmentQuestion"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);

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

            var result = await _queryRepo.ExecuteQueryList<AssessmentQuestionsViewModel>(query, null);
            return result;

        }

        public async Task UpdateSequenceNo(string Id, long? sequenceNo)
        {
            var query = $@"Update cms.""N_TAS_AssessmentQuestion"" set ""SequenceOrder""='{sequenceNo}' where ""Id""='{Id}' ";
            await _queryRepo.ExecuteCommand(query, null);

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
            return await _queryRepo.ExecuteQueryList<AssessmentQuestionsViewModel>(query, null);
        }

        public async Task<List<IdNameViewModel>> GetJobTitles(string departmentId)
        {
            var query = $@"select distinct j.""JobTitle"" as Name,j.""Id"" as Id
FROM cms.""N_CoreHR_HRDepartment"" as d
 join cms.""N_CoreHR_HRPosition"" as p on d.""Id"" = p.""DepartmentId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
  join cms.""N_CoreHR_HRJob"" as j on j.""Id"" = p.""JobId"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
  where d.""Id""='{departmentId}' and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var dept = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return dept;
        }

        public async Task<List<CapacityRiskViewModel>> GetJobCapacityChartData(string jobids, string jobId)
        {
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
            return await _queryRepo.ExecuteQueryList<CapacityRiskViewModel>(query1, null);
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
            return await _queryRepo.ExecuteQueryList<CapacityRiskViewModel>(query1, null);
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


            var queryData = await _queryRepo.ExecuteQuerySingle<AssignmentViewModel>(query, null);
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
            return await _queryRepo.ExecuteQueryList<AssignmentViewModel>(query, null);
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
            return await _queryRepo.ExecuteQueryList<AssignmentViewModel>(query, null);
        }

        public async Task<List<CapacityRiskViewModel>> GetJobCapacityData(string deptids)
        {

            var query1 = $@"select j.""JobTitle"" as JobName,j.""Id"" as JobId,cr.""ExternalAvailability""::int,cr.""InternalAvailability""::int
from cms.""N_CoreHR_HRJob"" as j 
left join cms.""N_TAS_JobCapacityRisk"" as cr on cr.""JobId""=j.""Id"" and cr.""IsDeleted""=false and cr.""CompanyId""='{_repo.UserContext.CompanyId}'
where j.""Id"" in ({deptids}) and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'";
            return await _queryRepo.ExecuteQueryList<CapacityRiskViewModel>(query1, null);
        }

        public async Task<List<AssessmentViewModel>> GetQuestionsByAssessmentId(string assessmentId)
        {
            var query = $@"Select aq.""QuestionId"", q.""Question"" from cms.""N_TAS_AssessmentQuestion"" as aq 
                        join cms.""N_TAS_Question"" as q on aq.""QuestionId""=q.""Id"" and q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}'
where aq.""AssessmentId""='{assessmentId}' and aq.""IsDeleted""=false and aq.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var result = await _queryRepo.ExecuteQueryList<AssessmentViewModel>(query, null);
            return result;
        }

        public async Task<List<AssessmentQuestionsOptionViewModel>> GetOptionsForQuestion(string questionNoteId)
        {
            var query = $@"select q.*,q.""Id"" as OptionId,q.""NtsNoteId"" as NoteId 
from cms.""N_TAS_QuestionOption"" as q
join public.""NtsNote"" as n on n.""Id""=q.""NtsNoteId"" and q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}'
where n.""ParentNoteId""='{questionNoteId}'  and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}' ";
            return await _queryRepo.ExecuteQueryList<AssessmentQuestionsOptionViewModel>(query, null);
        }

        public async Task<List<AssessmentQuestionsOptionViewModel>> GetOptionsByQuestionId(string questionId)
        {
            var query = $@"select q.*,q.""Id"" as OptionId,q.""NtsNoteId"" as NoteId 
from cms.""N_TAS_QuestionOption"" as q
join public.""NtsNote"" as n on q.""NtsNoteId""=n.""Id"" and n.""IsDeleted"" = false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_TAS_Question"" as que on n.""ParentNoteId""=que.""NtsNoteId""
where que.""Id""='{questionId}' and que.""IsDeleted""=false and que.""CompanyId""='{_repo.UserContext.CompanyId}'  ";
            return await _queryRepo.ExecuteQueryList<AssessmentQuestionsOptionViewModel>(query, null);
        }


        public async Task<List<AssessmentQuestionsViewModel>> GetQuestion()
        {
            var query = $@"select q.*,q.""Id"" as QuestionId,q.""NtsNoteId"" as NoteId, i.""IndicatorName"" from cms.""N_TAS_Question"" as q
                            join cms.""N_TAS_Indicator"" as i on q.""IndicatorId""=i.""Id"" and i.""IsDeleted""=false and i.""CompanyId""='{_repo.UserContext.CompanyId}'
             where q.""IsDeleted""=false and q.""CompanyId""='{_repo.UserContext.CompanyId}'";
            return await _queryRepo.ExecuteQueryList<AssessmentQuestionsViewModel>(query, null);
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
            return await _queryRepo.ExecuteQueryList<AssessmentQuestionsViewModel>(query, null);
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

            var result = await _queryRepo.ExecuteQueryList<AssessmentQuestionsViewModel>(query, null);

            return result;
        }

        public async Task<AssessmentQuestionsOptionViewModel> GetOptionsById(string Id)
        {
            var query = $@"select q.*,q.""Id"" as OptionId,q.""NtsNoteId"" as NoteId from cms.""N_TAS_QuestionOption"" as q

where q.""Id""='{Id}' and q.""CompanyId""='{_repo.UserContext.CompanyId}'";
            return await _queryRepo.ExecuteQuerySingle<AssessmentQuestionsOptionViewModel>(query, null);
        }
        public async Task DeleteOption(string id)
        {
            var query = $@"update  cms.""N_TAS_QuestionOption"" set ""IsDeleted""=true 
where ""Id""='{id}'";
            await _queryRepo.ExecuteCommand(query, null);

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
            else if (searchtext.IsNotNullAndNotEmpty() && type != "Option" && type != "Question")
            {
                questionWhere = $@" and a.""AssessmentName"" like ('%{searchtext}%') COLLATE ""tr-TR-x-icu"" ";
            }
            query = query.Replace("#SearchWhere#", questionWhere);

            var result = await _queryRepo.ExecuteQueryList<AssessmentViewModel>(query, null);
            return result;

        }
        public async Task DeleteAssessment(string NoteId)
        {
            var query = $@"Update cms.""N_TAS_Assessment"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
        }

        public async Task<AssessmentViewModel> GetAssessmentDetailsById(string assmntId)
        {
            var query = $@"Select ass.*,lv.""Code"" as AssessmentType
                 from cms.""N_TAS_Assessment"" as ass
join public.""LOV"" as lv on lv.""Id""=ass.""AssessmentTypeId"" and lv.""IsDeleted""=false  
                where ass.""Id""='{assmntId}' and ass.""IsDeleted""=false and ass.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var result = await _queryRepo.ExecuteQuerySingle<AssessmentViewModel>(query, null);
            return result;
        }


        public async Task UpdateSurveyResultAnswer(SurveyTopicViewModel model)
        {
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
                        ,""LastUpdatedDate""='{DateTime.Now.ToDatabaseDateFormat()}',""LastUpdatedBy""='{_uc.UserId}'
                        where ""Id""='{item.AnswerItemId}';";
                    //if (result != null)
                    //{

                    //    await _queryAssRes.ExecuteCommand(query1, null);

                    //    //var serdetail = await _serviceBusiness.GetSingle(x => x.UdfNoteId == result.NtsNoteId);

                    //    //ServiceTemplateViewModel serviceModel = new ServiceTemplateViewModel();
                    //    //serviceModel.ActiveUserId = _uc.UserId;
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
                    serviceModel.ActiveUserId = _uc.UserId;
                    serviceModel.TemplateCode = "S_SURVEY_RESULT_ANSWER";
                    var service = await _serviceBusiness.GetServiceDetails(serviceModel);

                    //service.ServiceSubject = ;
                    service.OwnerUserId = _uc.UserId;
                    service.StartDate = DateTime.Now;
                    service.DueDate = DateTime.Now.AddDays(10);
                    service.ActiveUserId = _uc.UserId;
                    service.DataAction = DataActionEnum.Create;
                    service.ParentServiceId = model.ServiceId;
                    service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

                    service.Json = JsonConvert.SerializeObject(exo);

                    var res = await _serviceBusiness.ManageService(service);
                }
            }
            if (updateQuery.IsNotNullAndNotEmpty())
            {
                await _queryRepo.ExecuteCommand(updateQuery, null);
            }
        }
        public async Task<long> GetSurveyUserCount(string prefix)
        {
            var query = $@"select count(*) from public.""User"" where ""Name"" like '{prefix}%' COLLATE ""tr-TR-x-icu""";
            return await _queryRepo.ExecuteScalar<long>(query, null);
        }
    }
}
