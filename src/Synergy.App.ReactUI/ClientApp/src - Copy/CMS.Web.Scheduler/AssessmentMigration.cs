using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.UI.ViewModel;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using ERP.Data.GraphModel;
using CMS.Data.Model;
using CMS.Business;
using AutoMapper;
using CMS.Common;
using System.Net.Http.Headers;

namespace CMS.Web.Scheduler
{
    public class AssessmentMigration
    {
        private List<ADM_User> SourceUserList;
        private List<User> TargetUserList;
        private string WebApiUrl = "http://178.238.236.213:3001/";

        private IServiceProvider _serviceProvider;
        //private IUserBusiness _userBusiness;
        private IMapper _autoMapper;
        private List<ERP.UI.ViewModel.AssessmentViewModel> noteList;
        private List<ERP.UI.ViewModel.AssessmentViewModel> assessmentArabicList;
        private List<ERP.UI.ViewModel.ServiceViewModel> topicList;
        private List<NoteTemplateViewModel> TargetFolderList;


        public AssessmentMigration(IMapper autoMapper, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            //_userBusiness = userBusiness;
            _autoMapper = autoMapper;

        }
        public async Task MigrateAssessment()
        {        
            await MigrateQuestion();
        }
        public async Task MigrateQuestion()
        {
            await ExtractQuestion();
            await TransformQuestion();
        }

        private async Task ExtractQuestion()
        {
            // noteList = await GetApiListAsync<NoteViewModel>();
            var cypher1 = @"match (t:NTS_Template{IsDeleted:0})-[:R_TemplateRoot]->
                    (tr:NTS_TemplateMaster{IsDeleted:0}) 
                match(t)<-[:R_Service_Template]-(s:NTS_Service{IsDeleted:0}) where s.Id in [38658,38665]
                match(s)<-[:R_ServiceFieldValue_Service]-(nfv1: NTS_ServiceFieldValue{Code:'English'})-[:R_ServiceFieldValue_TemplateField]->(tf1:NTS_TemplateField{FieldName:'assessmentLanguage'})
                return s order by s.Id";

            topicList = await GetApiListCyherAsync<ERP.UI.ViewModel.ServiceViewModel>(cypher1);

            var idsList = topicList.Select(x => x.Id);
            var id = String.Join(",", idsList);

            var cypher = @"match (t:NTS_Template{IsDeleted:0})-[:R_TemplateRoot]->
                    (tr:NTS_TemplateMaster{IsDeleted:0}) 
                match(t)<-[:R_Service_Template]-(s:NTS_Service{IsDeleted:0}) where s.Id in [" + id+ @"]
          
            match (s)<-[:R_Note_Reference]-(n:NTS_Note{IsDeleted:0})                       

           optional match(s)-[:R_Service_Parent_Service]-(es:NTS_Service)<-[:R_Note_Reference]-(en:NTS_Note{IsDeleted:0,SequenceNo:n.SequenceNo})

           optional match(n)< -[:R_NoteMultipleFieldValue_Note] - (tfv: NTS_NoteMultipleFieldValue{ IsDeleted: 0})
           with distinct tfv.RowId as RowId,n,en,s

            optional match(en)< -[:R_NoteMultipleFieldValue_Note] - (etfv: NTS_NoteMultipleFieldValue{ IsDeleted: 0})
           with distinct etfv.RowId as eRowId,en,n,RowId,s


           optional match(n) < -[:R_NoteMultipleFieldValue_Note] - (tfv: NTS_NoteMultipleFieldValue{ IsDeleted: 0,RowId: RowId})
           -[:R_NoteMultipleFieldValue_TemplateField]->(ttf: NTS_TemplateField{ FieldName: 'slNo',IsDeleted: 0})

            optional match(n)< -[:R_NoteMultipleFieldValue_Note] - (tfv1: NTS_NoteMultipleFieldValue{ IsDeleted: 0,RowId: RowId})
           -[:R_NoteMultipleFieldValue_TemplateField]->(ttf1: NTS_TemplateField{ FieldName: 'option',IsDeleted: 0})

            optional match(n)< -[:R_NoteMultipleFieldValue_Note] - (tfv2: NTS_NoteMultipleFieldValue{ IsDeleted: 0,RowId: RowId})
           -[:R_NoteMultipleFieldValue_TemplateField]->(ttf2: NTS_TemplateField{ FieldName: 'isAnswer',IsDeleted: 0})

           optional match(en) < -[:R_NoteMultipleFieldValue_Note] - (etfv: NTS_NoteMultipleFieldValue{ IsDeleted: 0,RowId: eRowId,Code:tfv.Code})
           -[:R_NoteMultipleFieldValue_TemplateField]->(ettf: NTS_TemplateField{ FieldName: 'slNo',IsDeleted: 0})


            optional match(en)< -[:R_NoteMultipleFieldValue_Note] - (etfv1: NTS_NoteMultipleFieldValue{ IsDeleted: 0,RowId: eRowId})
           -[:R_NoteMultipleFieldValue_TemplateField]->(ettf1: NTS_TemplateField{ FieldName: 'option',IsDeleted: 0})

            optional match(en)< -[:R_NoteMultipleFieldValue_Note] - (etfv2: NTS_NoteMultipleFieldValue{ IsDeleted: 0,RowId: eRowId})
           -[:R_NoteMultipleFieldValue_TemplateField]->(ettf2: NTS_TemplateField{ FieldName: 'isAnswer',IsDeleted: 0})
       
              with s,n,en,tfv,tfv1,tfv2,etfv,etfv1 
            return s.Subject as Title,s.Id as ServiceId,n.Id as NoteId,n.Subject as Subject,n.Description as Description,n.SequenceNo as SequenceNo,tfv.Code as SerialNo,tfv1.Code as Option,tfv2.Code as IsAnswer
            ,en.Subject as SubjectAr,en.Description as DescriptionAr,etfv.Code as SerialNoa,etfv1.Code as OptionAr order by n.SequenceNo";
            noteList = await GetApiListCyherAsync<ERP.UI.ViewModel.AssessmentViewModel>(cypher);

           
        }
        private async Task TransformQuestion()
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();

            if (topicList.Count > 0)
            {
                foreach (var topic in topicList)
                {
                    if (topic.Subject.Length > 10)
                    {
                        var topicSubject = topic.Subject;
                        topic.Subject = topic.Subject.TrimEnd('1');
                        topic.Subject = topic.Subject.TrimEnd(' ');
                        topic.Subject = topic.Subject.Substring(10);
                        var parentId = "";
                        var topixexist = await _noteBusiness.GetSingle(x => x.TemplateCode == "TAS_TOPIC" && x.NoteSubject == topic.Subject);
                        if (topixexist != null)
                        {
                            parentId = topixexist.Id;
                        }
                        else
                        {
                            var noteTemp1 = new NoteTemplateViewModel();
                            noteTemp1.TemplateCode = "TAS_TOPIC";
                            var note1 = await _noteBusiness.GetNoteDetails(noteTemp1);


                            note1.NoteSubject = topic.Subject;
                            note1.NoteDescription = topic.Description;
                            note1.StartDate = DateTime.Now;
                            note1.OwnerUserId = "45bba746-3309-49b7-9c03-b5793369d73c";
                            note1.RequestedByUserId = note1.OwnerUserId;
                            note1.Json = "{}";
                            note1.DataAction = DataActionEnum.Create;
                            note1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                            var result = await _noteBusiness.ManageNote(note1);
                            parentId = result.Item.NoteId;
                        }


                        if (noteList.Count > 0)
                        {
                            var questionlist = noteList.Where(x => x.ServiceId == topic.Id).DistinctBy(x => x.NoteId).ToList();
                            foreach (var folder in questionlist)
                            {
                                var noteTemp = new NoteTemplateViewModel();
                                noteTemp.TemplateCode = "TAS_QUESTION";
                                var note = await _noteBusiness.GetNoteDetails(noteTemp);


                                note.NoteSubject = folder.Subject;
                                note.NoteDescription = folder.Description;
                                note.StartDate = DateTime.Now;
                                note.OwnerUserId = "45bba746-3309-49b7-9c03-b5793369d73c";
                                note.RequestedByUserId = note.OwnerUserId;
                                note.Json = "{}";
                                note.DataAction = DataActionEnum.Create;
                                note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                note.SequenceOrder = folder.SequenceNo;
                                note.ParentNoteId = parentId.IsNotNullAndNotEmpty() ? parentId : null;

                                dynamic exo = new System.Dynamic.ExpandoObject();

                                folder.Subject = folder.Subject.Replace("'", "''");
                                if (folder.Description.IsNotNullAndNotEmpty())
                                {
                                    folder.Description = folder.Description.Replace("'", "''");
                                }

                                if (topicSubject.StartsWith("TA"))
                                {
                                    ((IDictionary<String, Object>)exo).Add("AssessmentTypeId", "13d70a1f-1114-408e-811b-d9735dbe28bd");
                                }
                                else
                                {
                                    ((IDictionary<String, Object>)exo).Add("AssessmentTypeId", "8b60d893-0fb3-4ed5-bae4-1d95e86d211b");
                                }
                                 ((IDictionary<String, Object>)exo).Add("CompentencyLevelId", "bdbe758b-395b-4f45-bdb0-ce423f0fd83f");
                                ((IDictionary<String, Object>)exo).Add("IndicatorId", "9bc71a07-c990-4e8a-8a4d-273a0d0840ab");
                                ((IDictionary<String, Object>)exo).Add("Question", folder.Subject);
                                ((IDictionary<String, Object>)exo).Add("QuestionDescription", folder.Description);
                                ((IDictionary<String, Object>)exo).Add("QuestionArabic", folder.SubjectAr);
                                ((IDictionary<String, Object>)exo).Add("QuestionDescriptionArabic", folder.DescriptionAr);

                                note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);


                                var result1 = await _noteBusiness.ManageNote(note);
                                var optionalist = noteList.Where(x => x.NoteId == folder.NoteId).DistinctBy(x => x.Option).ToList();
                                if (optionalist.Count > 0)
                                {

                                    foreach (var option in optionalist)
                                    {

                                        var opnote = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel { TemplateCode = "TAS_QUESTION_OPTION" });



                                        opnote.StartDate = DateTime.Now;
                                        opnote.OwnerUserId = "45bba746-3309-49b7-9c03-b5793369d73c";
                                        opnote.RequestedByUserId = note.OwnerUserId;
                                        opnote.Json = "{}";
                                        opnote.DataAction = DataActionEnum.Create;
                                        opnote.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                                        opnote.ParentNoteId = result1.Item.NoteId;


                                        dynamic exo1 = new System.Dynamic.ExpandoObject();
                                        if (option.Option.IsNotNullAndNotEmpty())
                                        {
                                            option.Option = option.Option.Replace("'", "''");
                                        }

                                        ((IDictionary<String, Object>)exo1).Add("Option", option.Option);
                                        ((IDictionary<String, Object>)exo1).Add("OptionArabic", option.OptionAr);
                                        ((IDictionary<String, Object>)exo1).Add("IsRightAnswer", option.IsAnswer);
                                        ((IDictionary<String, Object>)exo1).Add("Score", option.ScoreOption);
                                        opnote.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);


                                        await _noteBusiness.ManageNote(opnote);

                                    }
                                }

                            }
                        }
                    }
                }
            }
        }
        
        public async Task<T> GetApiAsync<T>()
        {
            using (var client = new HttpClient())
            {
                var address = new Uri($"{WebApiUrl}api/getlist?type={nameof(T)}");
                var response = await client.GetAsync(address);
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
        }
        public async Task<List<T>> GetApiListAsync<T>()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var address = new Uri($"{WebApiUrl}api/getlist?type={typeof(T).Name}");
                    var response = await client.GetAsync(address);
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<T>>(content);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<List<T>> GetApiListCyherAsync<T>(string cypher)
        {
            try
            {
                var myContent = JsonConvert.SerializeObject(new { Type = typeof(T).Name, Cyhper = cypher });
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                using (var client = new HttpClient())
                {
                    var address = new Uri($"{WebApiUrl}api/GetListCypher");
                    var response = await client.PostAsync(address, byteContent);
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<T>>(content);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
    