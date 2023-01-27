using Synergy.App.Common;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Hangfire;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class ProjectWorkboardBusiness : IProjectWorkboardBusiness
    {
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly INoteBusiness _noteBusiness;
        private readonly IUserContext _userContext;
        private readonly IProjectManagementBusiness _projectManagementBusiness;
        private readonly IProjectManagementQueryBusiness _projectManagementQueryBusiness;
        private readonly IServiceProvider _serviceProvider;

        //private readonly IHangfireScheduler _hangfireScheduler;
        public ProjectWorkboardBusiness(IRepositoryQueryBase<NoteViewModel> queryRepo, INoteBusiness noteBusiness, IUserContext userContext,
            IProjectManagementBusiness projectManagementBusiness, IProjectManagementQueryBusiness projectManagementQueryBusiness
            , IServiceProvider serviceProvider
            //, IHangfireScheduler hangfireScheduler
            )
        {
            _queryRepo = queryRepo;
            _noteBusiness = noteBusiness;
            _userContext = userContext;
            _projectManagementBusiness = projectManagementBusiness;
            _projectManagementQueryBusiness = projectManagementQueryBusiness;
            _serviceProvider = serviceProvider;
            //_hangfireScheduler = hangfireScheduler;
        }
        public async Task<List<WorkBoardViewModel>> GetWorkboardList(WorkBoardstatusEnum status)
        {
            var list = new List<WorkBoardViewModel>();
            //list.Add(new WorkBoardViewModel { Id = "new" });
            //var query = @$"SELECT wb.""Id"" as WorkboardId, wb.""WorkBoardName"" as WorkBoardName, wb.""WorkBoardDescription"" as WorkBoardDescription,
            //                wb.""TemplateTypeId"" as TemplateTypeId,tt.""TemplateName"" as TemplateTypeName,
            //                wb.""NtsNoteId"" as NoteId , tt.""ContentImage"" as IconFileId, wb.""WorkBoardStatus"" as WorkBoardStatus
            //                FROM cms.""N_WORKBOARD_WorkBoard"" as wb
            //                join cms.""N_WORKBOARD_TemplateType"" as tt on tt.""Id"" = wb.""TemplateTypeId""
            //                join public.""NtsNote"" as n on n.""Id""=wb.""NtsNoteId"" and n.""IsDeleted""=false and n.""OwnerUserId""='{_userContext.UserId}'
            //                where wb.""IsDeleted"" = false and wb.""WorkBoardStatus"" = '{(int)status}'";
            //var querydata = await _queryRepo.ExecuteQueryList<WorkBoardViewModel>(query, null);
            //list.AddRange(querydata);
            var data = await _projectManagementBusiness.GetProjectData();
            list = data.Select(x => new WorkBoardViewModel
            {
                WorkBoardName = x.ProjectName,
                WorkBoardDate = x.StartDate,
                //WorkBoardStatus = x.ProjectStatus,

            }).ToList();
            return list;
        }

        public async Task<List<WorkBoardViewModel>> GetWorkboardTaskList()
        {
            var list = new List<WorkBoardViewModel>();
            list.Add(new WorkBoardViewModel { Id = "new" });
            var querydata = await _projectManagementQueryBusiness.GetWorkboardTaskListData();
            list.AddRange(querydata);
            return list;
        }

        public async Task<bool> UpdateWorkBoardStatus(string id, WorkBoardstatusEnum status)
        {
            await _projectManagementQueryBusiness.UpdateWorkBoardStatus(id, status);
            return true;
        }
        public async Task<WorkBoardSectionViewModel> GetWorkBoardSectionDetails(string sectionId)
        {
            var querydata = await _projectManagementQueryBusiness.GetWorkBoardSectionData(sectionId);
            return querydata;
        }

        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionListByWorkbBoardId(string workboardId)
        {
            var querydata = await _projectManagementQueryBusiness.GetWorkBoardSectionListByWorkbBoardId(workboardId);
            return querydata;
        }


        public async Task<WorkBoardItemViewModel> GetWorkBoardItemDetails(string itemId)
        {
            var querydata = await _projectManagementQueryBusiness.GetWorkBoardItemDetails(itemId);
            return querydata;
        }
        public async Task<IList<WorkBoardItemViewModel>> GetWorkBoardItemBySectionId(string sectionId)
        {
            var querydata = await _projectManagementQueryBusiness.GetWorkBoardItemBySectionId(sectionId);
            return querydata;
        }

        public async Task<List<WorkBoardItemViewModel>> GetItemBySectionId(string sectionId)
        {
            var querydata = await _projectManagementQueryBusiness.GetItemBySectionId(sectionId);
            return querydata;
        }

        public async Task DeleteItem(string itemId)
        {
            await _projectManagementQueryBusiness.DeleteItem(itemId);
        }
        public async Task DeleteSection(string itemId)
        {
            await _projectManagementQueryBusiness.DeleteSection(itemId);
        }
        public async Task UpdateWorkBoardJson(WorkBoardViewModel data)
        {
            await _projectManagementQueryBusiness.UpdateWorkBoardJson(data);
        }
        public async Task UpdateWorkBoardSectionSequenceOrder(WorkBoardSectionViewModel data)
        {
            await _projectManagementQueryBusiness.UpdateWorkBoardSectionSequenceOrder(data);
        }
        public async Task UpdateWorkBoardItemDetails(WorkBoardItemViewModel data)
        {
            await _projectManagementQueryBusiness.UpdateWorkBoardItemDetails(data);
        }
        public async Task UpdateWorkBoardItemSequenceOrder(WorkBoardItemViewModel data)
        {
            await _projectManagementQueryBusiness.UpdateWorkBoardItemSequenceOrder(data);
        }
        public async Task UpdateWorkBoardItemSectionId(WorkBoardItemViewModel data)
        {
            await _projectManagementQueryBusiness.UpdateWorkBoardItemSectionId(data);
        }
        public async Task<WorkBoardViewModel> GetWorkBoardDetails(string workBoradId)
        {
            var querydata = await _projectManagementQueryBusiness.GetWorkBoardDetails(workBoradId);
            return querydata;
        }
        public async Task<WorkBoardViewModel> GetWorkBoardDetailsByIdKey(string workBoardUniqueId, string shareKey)
        {
            var querydata = await _projectManagementQueryBusiness.GetWorkBoardDetailsByIdKey(workBoardUniqueId, shareKey);
            return querydata;
        }

        public async Task<List<WorkBoardTemplateViewModel>> GetTemplateList()
        {
            var result = await _projectManagementQueryBusiness.GetTemplateList();
            return result;
        }

        public async Task<List<LOVViewModel>> GetTemplateCategoryList()
        {
            var result = await _projectManagementQueryBusiness.GetTemplateCategoryList();
            return result;
        }

        public async Task<List<WorkBoardTemplateViewModel>> GetSearchResults(string[] values)
        {
            string s1 = string.Join("|", values);

            var result = await _projectManagementQueryBusiness.GetSearchResults(s1);
            return result;
        }

        public async Task<string> GetJsonContent(string templateTypeId, string workBoardId, DateTime? date = null, string templateTypeCode = null, string workboardItemId = null)
        {
            List<WorkBoardSectionViewModel> list = new List<WorkBoardSectionViewModel>();
            List<WorkBoardItemViewModel> itemlist = new List<WorkBoardItemViewModel>();
            if (workBoardId.IsNotNullAndNotEmpty())
            {
                // Check if sections are available for work board id

                list = await _projectManagementQueryBusiness.GetSectionListOrderBySequenceNo(workBoardId);
                if (list.IsNotNull() && list.Count() > 0)
                {
                    // if exist get list item inside section
                    foreach (var section in list)
                    {

                        section.item = await _projectManagementQueryBusiness.GetItemsInSection(section.Id);
                        itemlist.AddRange(section.item);
                    }
                }
                else
                {
                    if (templateTypeCode == null)
                    {
                        var workBoardTemplate = await GetWorkBoardTemplateById(templateTypeId);
                        if (workBoardTemplate.IsNotNull())
                        {
                            templateTypeCode = workBoardTemplate.TemplateTypeCode;
                        }
                    }

                    DateTime date2 = date ?? DateTime.Now;
                    if (templateTypeCode.IsNotNullAndNotEmpty())
                    {

                        switch (templateTypeCode)
                        {

                            case "BASIC":
                                // convert the list to json and return
                                list = await GetWorkBoardSectionForBasic(workBoardId);
                                break;
                            case "KANBAN":

                                break;
                            case "CALENDAR_MONTHLY":
                                list = await GetWorkBoardSectionForMonthly(workBoardId, date2);
                                break;
                            case "CALENDER_WEEKLEY":
                                list = await GetWorkBoardSectionForWeekly(workBoardId, DateTime.Now);
                                break;
                            case "CALENDER_YEARLY":
                                list = await GetWorkBoardSectionForYearly(workBoardId, DateTime.Now);
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            var json = list.Select(x => new
            {
                id = x.Id,
                title = x.SectionDigit != 0 ? string.Concat(x.SectionDigit, ".", x.SectionName) : x.SectionName,
                @class = "section-" + (x.HeaderColor.IsNullOrEmpty() ? "blue" : x.HeaderColorName),
                item = itemlist.Where(y => y.WorkBoardSectionId == x.Id && y.ParentId.IsNullOrEmpty()).Select(z => new
                {
                    id = z.Id,
                    title = GetItemContent(z, itemlist),
                    @class = "wb-item-color-" + (z.ColorName.IsNullOrEmpty() ? "yellow" : z.ColorName) + " wb-item-shape-" + Enum.GetName(typeof(WorkBoardItemSizeEnum), z.WorkBoardItemShape) + " wb-item-size-" + Enum.GetName(typeof(WorkBoardItemSizeEnum), z.WorkBoardItemSize),
                }),
                sectionDigit = x.SectionDigit
            }).ToList();
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(json);
            return data;
        }
        public string GetItemContent(WorkBoardItemViewModel model, List<WorkBoardItemViewModel> list)
        {
            string Title = string.Empty;
            model.WorkBoardItemShape = model.WorkBoardItemShape != 0 ? model.WorkBoardItemShape : WorkBoardItemShapeEnum.Square;
            model.WorkBoardItemSize = model.WorkBoardItemSize != 0 ? model.WorkBoardItemSize : WorkBoardItemSizeEnum.Standard;
            if (model.ItemType == WorkBoardItemTypeEnum.Text)
            {
                Title = "<div data-action='Edit' id='" + model.NtsNoteId + "' data-workBoardItemId='" + model.Id + "' class='card wb-item wb-item-text wb-item-color-" + model.ColorName + " wb-item-shape-" + Enum.GetName(typeof(WorkBoardItemShapeEnum), model.WorkBoardItemShape).ToLowerInvariant() + " wb-item-size-" + Enum.GetName(typeof(WorkBoardItemSizeEnum), model.WorkBoardItemSize).ToLowerInvariant() + "'><div class='card-header'><span class='wb-item-icons'><i data-id='" + model.NtsNoteId + "' onclick='getMessages(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-message-middle'></i><i data-id='" + model.NtsNoteId + "' onclick='getTaskList(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-list-check'></i><i data-id='" + model.NtsNoteId + "' class='wb-item-icon wb-item-menu fa fa-ellipsis-v'></i></span></div><div class='card-body'><p class='card-text'>" + model.ItemContent + "</p></div></div>";
            }
            else if (model.ItemType == WorkBoardItemTypeEnum.Index)
            {
                Title = "<div data-action='Edit' id='" + model.NtsNoteId + "'  data-workBoardItemId='" + model.Id + "' class='card wb-item wb-item-index wb-item-color-" + model.ColorName + " wb-item-shape-" + Enum.GetName(typeof(WorkBoardItemShapeEnum), model.WorkBoardItemShape).ToLowerInvariant() + " wb-item-size-" + Enum.GetName(typeof(WorkBoardItemSizeEnum), model.WorkBoardItemSize).ToLowerInvariant() + "'><div class='card-header'><span class='card-text'>" + model.ItemName + "</span><span class='wb-item-icons'><i data-id='" + model.Id + "' onclick='getCard(\"" + model.Id + "\");' class='wb-item-icon fa fa-external-link' title='Open this Index in Work Book'></i><i data-id='" + model.NtsNoteId + "' onclick='getMessages(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-message-middle' title='Comments'></i><i data-id='" + model.NtsNoteId + "' onclick='getTaskList(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-list-check' title='Tasks'></i><i data-id='" + model.NtsNoteId + "' class='wb-item-icon wb-item-menu fa fa-ellipsis-v' title='Actions'></i></span></div><div class='card-body'><ul>";
                foreach (var data in list.Where(x => x.ParentId == model.Id))
                {
                    Title = string.Concat(Title, "<li id='" + data.NtsNoteId + "' data-type='card wb-item wb-item-text wb-item-color-" + data.ColorName + "' class='subItem' data-workboarditemid='" + data.Id + "' data-parentindexitemid='" + data.ParentId + "'>" + data.ItemContent + "<span class='fa fa-minus-circle index-item' title='Move Out' data-indexitemid='" + data.NtsNoteId + "' onClick='onMoveOut(this)'></span></li>");
                }
                Title = string.Concat(Title, "</ul></div></div>");
            }
            else if (model.ItemType == WorkBoardItemTypeEnum.WhiteBoard)
            {
                Title = "<div data-action='Edit' id='" + model.NtsNoteId + "' data-workBoardItemId='" + model.Id + "' class='card wb-item wb-item-whiteboard wb-item-color-" + model.ColorName + " wb-item-shape-" + Enum.GetName(typeof(WorkBoardItemShapeEnum), model.WorkBoardItemShape).ToLowerInvariant() + " wb-item-size-" + Enum.GetName(typeof(WorkBoardItemSizeEnum), model.WorkBoardItemSize).ToLowerInvariant() + "'><div class='card-header'><span class='wb-item-icons'><i data-id='" + model.NtsNoteId + "' onclick='getMessages(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-message-middle'></i><i data-id='" + model.NtsNoteId + "' onclick='getTaskList(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-list-check'></i><i data-id='" + model.NtsNoteId + "' class='wb-item-icon wb-item-menu fa fa-ellipsis-v'></i></span></div><div class='card-body'><div class='card-content' style = 'background-color:#fff;' ><img id='" + model.ItemFileId + "' src='/cms/Document/GetImageMongo?id=" + model.ItemFileId + "' style='max-width:100%;max-height:100 %;' /></div></div></div>";
            }
            else if (model.ItemType == WorkBoardItemTypeEnum.Image)
            {
                Title = "<div data-action='Edit' id='" + model.NtsNoteId + "' data-workBoardItemId='" + model.Id + "' class='card wb-item wb-item-image wb-item-color-" + model.ColorName + " wb-item-shape-" + Enum.GetName(typeof(WorkBoardItemShapeEnum), model.WorkBoardItemShape).ToLowerInvariant() + " wb-item-size-" + Enum.GetName(typeof(WorkBoardItemSizeEnum), model.WorkBoardItemSize).ToLowerInvariant() + "'><div class='card-header'><span class='wb-item-icons'><i data-id='" + model.NtsNoteId + "' onclick='getMessages(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-message-middle'></i><i data-id='" + model.NtsNoteId + "' onclick='getTaskList(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-list-check'></i><i data-id='" + model.NtsNoteId + "' class='wb-item-icon wb-item-menu fa fa-ellipsis-v'></i></span></div><div class='card-body'><div class='card-content'><img id ='" + model.ItemFileId + "' src='/cms/Document/GetImageMongo?id=" + model.ItemFileId + "' style='max-width:100%;max-height:100%;' /></div></div></div>";
            }
            else if (model.ItemType == WorkBoardItemTypeEnum.Video)
            {

            }
            else if (model.ItemType == WorkBoardItemTypeEnum.File)
            {
                Title = "<div data-action='Edit' id='" + model.NtsNoteId + "' data-workBoardItemId='" + model.Id + "' class='card wb-item wb-item-file wb-item-color" + model.ColorName + " wb-item-shape-" + Enum.GetName(typeof(WorkBoardItemShapeEnum), model.WorkBoardItemShape).ToLowerInvariant() + " wb-item-size-" + Enum.GetName(typeof(WorkBoardItemSizeEnum), model.WorkBoardItemSize).ToLowerInvariant() + "'><div class='card-header'><span class='wb-item-icons'><i data-id='" + model.NtsNoteId + "' onclick='getMessages(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-message-middle'></i><i data-id='" + model.NtsNoteId + "' onclick='getTaskList(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-list-check'></i><i data-id='" + model.NtsNoteId + "' class='wb-item-icon wb-item-menu fa fa-ellipsis-v'></i></span></div><div class='card-body'><div class='card-content'><img id = '" + model.ItemFileId + "' src='/cms/Document/GetSnapMongo?id=" + model.ItemFileId + "' onerror='OnDocError(this)' style='max-width:100%;max-height:100%;' /></div></div></div>";
            }
            else if (model.ItemType == WorkBoardItemTypeEnum.Note)
            {
                Title = "<div data-action='Edit' id='" + model.NtsNoteId + "' data-workBoardItemId='" + model.Id + "' class='wb-item wb-item-note'>" + model.ItemContent + "<span id='tree-menuItem' class='fal fa-ellipsis-h item-custom-button'></span></div>";
            }
            else if (model.ItemType == WorkBoardItemTypeEnum.Task)
            {
                Title = "<div data-action='Edit' id='" + model.NtsNoteId + "' data-workBoardItemId='" + model.WorkBoardItemId + "'  class='wb-item wb-item-task'>" + model.ItemContent + "<span id='tree-menuItem' class='fal fa-ellipsis-h item-custom-button'></span></div>";
            }
            else if (model.ItemType == WorkBoardItemTypeEnum.Servivce)
            {
                Title = "<div data-action='Edit' id='" + model.NtsNoteId + "' data-workBoardItemId='" + model.WorkBoardItemId + "' class='wb-item wb-item-service'>" + model.ItemContent + "<span id='tree-menuItem' class='fal fa-ellipsis-h item-custom-button'></span></div>";
            }
            return Title;
        }
        public async Task<string> GetWorkBoardSectionForIndex(WorkBoardItemViewModel item)
        {
            var sectionsList = new List<WorkBoardSectionViewModel>();
            var sectionModel = new WorkBoardSectionViewModel()
            {
                Id = item.WorkBoardItemId,
                SectionName = item.ItemName,
                HeaderColor = item.ColorName,
            };


            //var query = $@"select * from cms.""N_WORKBOARD_WorkBoardItem"" where ""WorkBoardSectionId"" = '{item.WorkBoardSectionId}' and ""IsDeleted"" = false ";
            var result = await _projectManagementQueryBusiness.GetItemsInSection(item.WorkBoardSectionId);
            dynamic items = null;
            if (result.IsNotNull() && result.Any())
            {
                items = result.Select(x => new { id = x.Id, title = x.ItemContent, @class = x.ColorName });
            }

            sectionsList.Add(sectionModel);
            var json = sectionsList.Select(x => new
            {
                id = x.Id,
                title = x.SectionName,
                @class = "section-" + (x.HeaderColor.IsNullOrEmpty() ? "blue" : x.HeaderColor),
                item = result.Where(y => y.ParentId == x.Id).Select(z => new
                {
                    id = z.Id,
                    title = GetItemContent(z, result),
                    @class = "wb-item-color-" + (z.ColorName.IsNullOrEmpty() ? "yellow" : z.ColorName) + " wb-item-shape-" + Enum.GetName(typeof(WorkBoardItemSizeEnum), z.WorkBoardItemShape) + " wb-item-size-" + Enum.GetName(typeof(WorkBoardItemSizeEnum), z.WorkBoardItemSize),
                }),
            }).ToList();

            var data = Newtonsoft.Json.JsonConvert.SerializeObject(json);

            return data;
        }

        public async Task<WorkBoardTemplateViewModel> GetWorkBoardTemplateById(string templateTypeId)
        {
            return await _projectManagementQueryBusiness.GetWorkBoardTemplateById(templateTypeId);
        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForBasic(string workBoardId)
        {
            //var delete = $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""=null where ""WorkBoardId""='{workBoardId}'";
            //await _queryRepo.ExecuteCommand(delete, null);
            //var query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST 3 ROWS ONLY ";
            var sections = await _projectManagementQueryBusiness.GetParticularNumberOfSections(3);
            if (sections == null || sections.Count() != 3)
            {
                await GenerateDummyWorkBoardSections(3);
                //query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST 3 ROWS ONLY ";
                sections = await _projectManagementQueryBusiness.GetParticularNumberOfSections(3);
            }
            if (sections.IsNotNull() && sections.Count() > 0)
            {
                var Ids = string.Join(",", sections.Select(x => x.Id));
                Ids = Ids.Replace(",", "','");
                // update All sections where workBoardId is null(Top 3) 
                //var query1 = $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}' where ""Id"" in ('{Ids}') and ""IsDeleted""=false";
                //await _queryRepo.ExecuteCommand(query1, null);
                // and then Get using this workboard Id order by Sequence Order


                await _projectManagementQueryBusiness.UpdateSectionsforBasicTemplate(sections, workBoardId);
                var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                await hangfireScheduler.Enqueue<Business.HangfireScheduler>(x => x.GenerateDummyWorkBoardSections(100, _userContext.ToIdentityUser()));
                //var query2 = $@"select * from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId""='{workBoardId}' and ""IsDeleted""=false order by ""SequenceOrder""";
                return await _projectManagementQueryBusiness.GetSectionListOrderBySequenceNo(workBoardId);
            }


            return new List<WorkBoardSectionViewModel>();
        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForMonthly(string workBoardId, DateTime date)
        {
            var days = DateTime.DaysInMonth(date.Year, date.Month);
            //date.IsLastDayOfMonth();
            // update All sections where workBoard Id is null(Days in month) and then Get using this workboard Id
            //var query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST {days} ROWS ONLY ";
            var sections = await _projectManagementQueryBusiness.GetParticularNumberOfSections(days);
            if (sections == null || sections.Count() != days)
            {
                await GenerateDummyWorkBoardSections(days);
                //query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST {days} ROWS ONLY ";
                sections = await _projectManagementQueryBusiness.GetParticularNumberOfSections(days);
            }
            if (sections.IsNotNull() && sections.Count() > 0)
            {
                var Ids = string.Join(",", sections.Select(x => x.Id));
                Ids = Ids.Replace(",", "','");
                // update All sections where workBoardId is null(Days in month) 
                // var query1 = $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}' where ""Id"" in ('{Ids}') and ""IsDeleted""=false";
                // await _queryRepo.ExecuteCommand(query1, null);

                await _projectManagementQueryBusiness.UpdateSectionsforMonthlyTemplate(sections, workBoardId, date);
                var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                await hangfireScheduler.Enqueue<Business.HangfireScheduler>(x => x.GenerateDummyWorkBoardSections(100, _userContext.ToIdentityUser()));
                // and then Get using this workboard Id order by Sequence Order
                //var query2 = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId""='{workBoardId}' and ""IsDeleted""=false order by ""SequenceOrder""";
                return await _projectManagementQueryBusiness.GetSectionsIdOrderBySequenceNo(workBoardId);
            }
            // job to generateDummyWorkBoardSections(No Of Items)
            return new List<WorkBoardSectionViewModel>();

        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForWeekly(string workBoardId, DateTime date)
        {

            // update All sections where workBoard Id is null(Days in month) and then Get using this workboard Id
            //var query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST 7 ROWS ONLY ";
            var sections = await _projectManagementQueryBusiness.GetParticularNumberOfSections(7);
            if (sections == null || sections.Count() != 7)
            {
                await GenerateDummyWorkBoardSections(7);
                //query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST 7 ROWS ONLY ";
                sections = await _projectManagementQueryBusiness.GetParticularNumberOfSections(7);
            }
            if (sections.IsNotNull() && sections.Count() > 0)
            {
                var Ids = string.Join(",", sections.Select(x => x.Id));
                Ids = Ids.Replace(",", "','");
                // update All sections where workBoardId is null(Days in month) 
                //var query1 = $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}' where ""Id"" in ('{Ids}') and ""IsDeleted""=false";
                // await _queryRepo.ExecuteCommand(query1, null);
                // and then Get using this workboard Id order by Sequence Order

                await _projectManagementQueryBusiness.UpdateSectionsforWeeklyTemplate(sections, workBoardId);
                // job to generateDummyWorkBoardSections(No Of Items)
                var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                await hangfireScheduler.Enqueue<Business.HangfireScheduler>(x => x.GenerateDummyWorkBoardSections(100, _userContext.ToIdentityUser()));
                //var query2 = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId""='{workBoardId}' and ""IsDeleted""=false order by ""SequenceOrder""";
                return await _projectManagementQueryBusiness.GetSectionsIdOrderBySequenceNo(workBoardId);
            }


            return new List<WorkBoardSectionViewModel>();
        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForYearly(string workBoardId, DateTime date)
        {

            // update All sections where workBoard Id is null(Days in month) and then Get using this workboard Id
            //var query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST 12 ROWS ONLY ";
            var sections = await _projectManagementQueryBusiness.GetParticularNumberOfSections(12);
            if (sections == null || sections.Count() != 7)
            {
                await GenerateDummyWorkBoardSections(12);
                //query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST 12 ROWS ONLY ";
                sections = await _projectManagementQueryBusiness.GetParticularNumberOfSections(12);
            }
            if (sections.IsNotNull() && sections.Count() > 0)
            {
                var Ids = string.Join(",", sections.Select(x => x.Id));
                Ids = Ids.Replace(",", "','");
                // update All sections where workBoardId is null(Days in month) 
                //var query1 = $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}' where ""Id"" in ('{Ids}') and ""IsDeleted""=false";
                //await _queryRepo.ExecuteCommand(query1, null);
                // and then Get using this workboard Id order by Sequence Order

                await _projectManagementQueryBusiness.UpdateSectionsforYearlyTemplate(sections, workBoardId);
                var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                await hangfireScheduler.Enqueue<Business.HangfireScheduler>(x => x.GenerateDummyWorkBoardSections(100, _userContext.ToIdentityUser()));
                //var query2 = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId""='{workBoardId}' and ""IsDeleted""=false order by ""SequenceOrder""";
                return await _projectManagementQueryBusiness.GetSectionsIdOrderBySequenceNo(workBoardId);

            }
            // job to generateDummyWorkBoardSections(No Of Items)
            return new List<WorkBoardSectionViewModel>();

        }
        public async Task<List<WorkBoardSectionViewModel>> GenerateDummyWorkBoardSections(int NoOfItems)
        {
            List<WorkBoardSectionViewModel> model = new List<WorkBoardSectionViewModel>();
            if (NoOfItems > 0)
            {
                for (var i = 0; i < NoOfItems; i++)
                {
                    // Create Note with this any items
                    var templateModel = new NoteTemplateViewModel();
                    templateModel.ActiveUserId = _userContext.UserId;
                    templateModel.DataAction = DataActionEnum.Create;
                    templateModel.TemplateCode = "WB_SECTION";
                    var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                    dynamic exo = new System.Dynamic.ExpandoObject();
                    //((IDictionary<String, Object>)exo).Add("WorkBoardId", workBoardId);
                    //((IDictionary<String, Object>)exo).Add("SectionName", "Section "+i);
                    //((IDictionary<String, Object>)exo).Add("SectionDescription", "Section " + i);
                    //((IDictionary<String, Object>)exo).Add("SectionDigit", i);
                    //  newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    var result = await _noteBusiness.ManageNote(newmodel);
                }
            }
            return model;
        }



        public async Task<List<WorkBoardViewModel>> GetOtherWorkboardList(WorkBoardstatusEnum status, string id)
        {
            var querydata = await _projectManagementQueryBusiness.GetOtherWorkboardList(status, id);
            return querydata;
        }

        public async Task<List<WorkBoardSectionViewModel>> GetWorkboardSectionList(string id)
        {
            var result = await _projectManagementQueryBusiness.GetWorkboardSectionList(id);
            return result;
        }

        public async Task<WorkBoardItemViewModel> GetWorkBoardItemByNtsNoteId(string ntsNoteId)
        {
            var result = await _projectManagementQueryBusiness.GetWorkBoardItemByNtsNoteId(ntsNoteId);
            return result;
        }
        public async Task<WorkBoardItemViewModel> GetWorkboardItemById(string id)
        {
            var result = await _projectManagementQueryBusiness.GetWorkboardItemById(id);
            return result;
        }

        public async Task<WorkBoardItemViewModel> GetWorkboardItemByNoteId(string id)
        {
            var result = await _projectManagementQueryBusiness.GetWorkboardItemByNoteId(id);
            return result;
        }

        public async Task<bool> UpdateWorkboardItem(string workboardId, string sectionId, string workboardItemId)
        {
            await _projectManagementQueryBusiness.UpdateWorkboardItem(workboardId, sectionId, workboardItemId);
            return true;
        }

        public async Task<IList<UserViewModel>> GetUserList(string noteId)
        {
            var list = await _projectManagementQueryBusiness.GetUserList(noteId);
            return list;
        }

        public async Task<List<WorkBoardViewModel>> GetSharedWorkboardList(WorkBoardstatusEnum status, string sharedWithUserId)
        {
            var querydata = await _projectManagementQueryBusiness.GetSharedWorkboardList(status, sharedWithUserId);
            return querydata;
        }
    }
}
