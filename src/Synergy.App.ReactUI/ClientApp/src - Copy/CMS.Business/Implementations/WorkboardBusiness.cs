using CMS.Common;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using Hangfire;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public class WorkboardBusiness : IWorkboardBusiness
    {
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly INoteBusiness _noteBusiness;
        private readonly IUserContext _userContext;

        public WorkboardBusiness(IRepositoryQueryBase<NoteViewModel> queryRepo, INoteBusiness noteBusiness, IUserContext userContext)
        {
            _queryRepo = queryRepo;
            _noteBusiness = noteBusiness;
            _userContext = userContext;
        }
        public async Task<List<WorkBoardViewModel>> GetWorkboardList(WorkBoardstatusEnum status)
        {
            var list = new List<WorkBoardViewModel>();
            list.Add(new WorkBoardViewModel { Id = "new" });
            var query = @$"SELECT wb.""Id"" as WorkboardId, wb.""WorkBoardName"" as WorkBoardName, wb.""WorkBoardDescription"" as WorkBoardDescription,
                            wb.""TemplateTypeId"" as TemplateTypeId,tt.""TemplateName"" as TemplateTypeName,
                            wb.""NtsNoteId"" as NoteId , tt.""ContentImage"" as IconFileId, wb.""WorkBoardStatus"" as WorkBoardStatus
                            FROM cms.""N_WORKBOARD_WorkBoard"" as wb
                            join cms.""N_WORKBOARD_TemplateType"" as tt on tt.""Id"" = wb.""TemplateTypeId""
                            join public.""NtsNote"" as n on n.""Id""=wb.""NtsNoteId"" and n.""IsDeleted""=false and n.""OwnerUserId""='{_userContext.UserId}'
                            where wb.""IsDeleted"" = false and wb.""WorkBoardStatus"" = '{(int)status}'";
            var querydata = await _queryRepo.ExecuteQueryList<WorkBoardViewModel>(query, null);
            list.AddRange(querydata);
            return list;
        }

        public async Task<List<WorkBoardViewModel>> GetWorkboardTaskList()
        {
            var list = new List<WorkBoardViewModel>();
            list.Add(new WorkBoardViewModel { Id = "new" });
            var query = @$"SELECT wb.""Id"" as WorkboardId, wb.""WorkBoardName"" as WorkBoardName, wb.""WorkBoardDescription"" as WorkBoardDescription,
                            wb.""TemplateTypeId"" as TemplateTypeId,tt.""TemplateName"" as TemplateTypeName,
                            wb.""NtsNoteId"" as NoteId , tt.""ContentImage"" as IconFileId, wb.""WorkBoardStatus"" as WorkBoardStatus
                            FROM cms.""N_WORKBOARD_WorkBoard"" as wb
                            join cms.""N_WORKBOARD_TemplateType"" as tt on tt.""Id"" = wb.""TemplateTypeId""
                            where wb.""IsDeleted"" = false ";
            var querydata = await _queryRepo.ExecuteQueryList<WorkBoardViewModel>(query, null);
            list.AddRange(querydata);
            return list;
        }

        public async Task<bool> UpdateWorkBoardStatus(string id, WorkBoardstatusEnum status)
        {
            var query = @$"UPDATE cms.""N_WORKBOARD_WorkBoard""
                                SET ""WorkBoardStatus"" = '{(int)status}'
                                WHERE ""Id"" = '{id}';";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }
        public async Task<WorkBoardSectionViewModel> GetWorkBoardSectionDetails(string sectionId)
        {
            var query = @$"select wbs.*,wbs.""Id"" as ""WorkBoardSectionId"",wbs.""NtsNoteId"" as NoteId
                            from cms.""N_WORKBOARD_WorkBoardSection"" as wbs
                            where wbs.""Id""='{sectionId}' and wbs.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<WorkBoardSectionViewModel>(query, null);
            return querydata;
        }

        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionListByWorkbBoardId(string workboardId)
        {
            var query = @$"select wbs.*,wbs.""NtsNoteId"" as ""NoteId""
                            from cms.""N_WORKBOARD_WorkBoardSection"" as wbs
                            where wbs.""WorkBoardId""='{workboardId}' and wbs.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            return querydata;
        }


        public async Task<WorkBoardItemViewModel> GetWorkBoardItemDetails(string itemId)
        {
            var query = @$"select *,""NtsNoteId"" as NoteId
                            from cms.""N_WORKBOARD_WorkBoardItem"" 
                            where ""Id""='{itemId}' and ""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<WorkBoardItemViewModel>(query, null);
            return querydata;
        }
        public async Task<IList<WorkBoardItemViewModel>> GetWorkBoardItemBySectionId(string sectionId)
        {
            var query = @$"select *
                            from cms.""N_WORKBOARD_WorkBoardItem"" 
                            where ""WorkBoardSectionId""='{sectionId}' and (""ParentId"" is null or ""ParentId""='')  and ""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<WorkBoardItemViewModel>(query, null);
            return querydata;
        }

        public async Task<List<WorkBoardItemViewModel>> GetItemBySectionId(string sectionId)
        {
            var query = @$"select *
                            from cms.""N_WORKBOARD_WorkBoardItem"" 
                            where ""WorkBoardSectionId""='{sectionId}' and ""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<WorkBoardItemViewModel>(query, null);
            return querydata;
        }

        public async Task DeleteItem(string itemId)
        {
            var query = @$"Update cms.""N_WORKBOARD_WorkBoardItem"" set ""IsDeleted""=true
                            where ""Id""='{itemId}' ";
            await _queryRepo.ExecuteCommand(query, null);
           
        }
        public async Task DeleteSection(string itemId)
        {
            var query = @$"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""IsDeleted""=true
                            where ""Id""='{itemId}' ";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task UpdateWorkBoardJson(WorkBoardViewModel data)
        {
            var query = @$"Update cms.""N_WORKBOARD_WorkBoard"" set ""JsonContent""='{data.JsonContent /*JsonConvert.SerializeObject(data.JsonContent)*/}', ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_userContext.UserId}'
                            where ""Id""='{data.WorkboardId}' and ""IsDeleted""=false ";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task UpdateWorkBoardSectionSequenceOrder(WorkBoardSectionViewModel data)
        {
            var query = @$"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""SequenceOrder""='{data.SequenceOrder}', ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_userContext.UserId}'
                            where ""Id""='{data.id}' and ""IsDeleted""=false ";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task UpdateWorkBoardItemDetails(WorkBoardItemViewModel data)
        {
            var query = @$"Update cms.""N_WORKBOARD_WorkBoardItem"" set ""SequenceOrder""='{data.SequenceOrder}',""ParentId""='{data.ParentId}',""ColorCode""='{data.ColorCode}',""WorkBoardSectionId""='{data.WorkBoardSectionId}', ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_userContext.UserId}'
                            where ""Id""='{data.id}' and ""IsDeleted""=false ";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task UpdateWorkBoardItemSequenceOrder(WorkBoardItemViewModel data)
        {
            var query = @$"Update cms.""N_WORKBOARD_WorkBoardItem"" set ""SequenceOrder""='{data.SequenceOrder}',""ParentId""='{data.ParentId}',""ColorCode""='{data.ColorCode}',""WorkBoardItemShape""='{data.WorkBoardItemShape}',""WorkBoardItemSize""='{data.WorkBoardItemSize}', ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_userContext.UserId}'
                            where ""Id""='{data.id}' and ""IsDeleted""=false ";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task UpdateWorkBoardItemSectionId(WorkBoardItemViewModel data)
        {
            var query = @$"Update cms.""N_WORKBOARD_WorkBoardItem"" set ""WorkBoardSectionId""='{data.WorkBoardSectionId}', ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_userContext.UserId}'
                            where ""NtsNoteId""='{data.WorkBoardItemId}' and ""IsDeleted""=false ";
            await _queryRepo.ExecuteCommand(query, null);

        }
        public async Task<WorkBoardViewModel> GetWorkBoardDetails(string workBoradId)
        {
            var query = @$"select wb.*,wb.""NtsNoteId"" as NoteId,wbtt.""TemplateTypeName"" as TemplateTypeName, wbtt.""TemplateTypeCode"" as TemplateTypeCode 
                            from cms.""N_WORKBOARD_WorkBoard"" as wb
                            left join cms.""N_WORKBOARD_TemplateType"" as wbtt on wbtt.""Id""=wb.""TemplateTypeId"" and wbtt.""IsDeleted""=false
                            where wb.""Id""='{workBoradId}' and wb.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<WorkBoardViewModel>(query, null);
            return querydata;
        }
        public async Task<WorkBoardViewModel> GetWorkBoardDetailsByIdKey(string workBoardUniqueId, string shareKey)
        {
            var query = @$"select wb.""WorkBoardId"" as WorkboardId 
                            from cms.""N_WORKBOARD_WorkBoardShare"" as wb
                            where wb.""WorkBoardUniqueId""='{workBoardUniqueId}' and  wb.""ShareKey""='{shareKey}' and wb.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQuerySingle<WorkBoardViewModel>(query, null);
            return querydata;
        }

        public async Task<List<WorkBoardTemplateViewModel>> GetTemplateList()
        {
            var query = $@"SELECT wt.""Id"" as TemplateId, wt.""NtsNoteId"" as NoteId, wt.""TemplateDescription"" as TemplateDescription,
                            wt.""SampleContent"" as SamplecContent, wt.""ContentImage"" as ContentImage, wt.""TemplateTypeCode"" as TemplateTypeCode,
                            wt.""TemplateTypeName"" as TemplateTypeName, lov.""Name"" as TemplateDisplayName
                            FROM cms.""N_WORKBOARD_TemplateType"" as wt
                            join public.""LOV"" as lov on lov.""Id"" = wt.""TemplateCategoryId"" where lov.""IsDeleted""=false and lov.""Name"" NOT IN ('Frameworks','Categories','Use Cases', 'Teams')
                            and wt.""IsDeleted""=false ";
            var result = await _queryRepo.ExecuteQueryList<WorkBoardTemplateViewModel>(query, null);
            return result;
        }

        public async Task<List<LOVViewModel>> GetTemplateCategoryList()
        {
            var query = $@"select lov.""Id"" as TemplateCategoryId, lov.""Name"" as Name from public.""LOV"" as lov where ""LOVType"" = 'WB_TMPLT_CATEGORY' and lov.""Name"" NOT IN ('Frameworks','Categories','Use Cases', 'Teams') and ""IsDeleted""=false;";
            var result = await _queryRepo.ExecuteQueryList<LOVViewModel>(query, null);
            return result;
        }

        public async Task<List<WorkBoardTemplateViewModel>> GetSearchResults(string[] values)
        {
            string s1 = string.Join("|", values);

            var query = $@"SELECT wt.""Id"" as TemplateId, wt.""NtsNoteId"" as NoteId, wt.""TemplateDescription"" as TemplateDescription,
                            wt.""SampleContent"" as SamplecContent, wt.""ContentImage"" as ContentImage, wt.""TemplateTypeCode"" as TemplateTypeCode,
                            wt.""TemplateTypeName"" as TemplateTypeName, lov.""Name"" as TemplateDisplayName
                            FROM cms.""N_WORKBOARD_TemplateType"" as wt
                            join public.""LOV"" as lov on lov.""Id"" = wt.""TemplateCategoryId"" 
                            where lov.""IsDeleted""=false 
                                  and lov.""Name"" NOT IN ('Frameworks','Categories','Use Cases', 'Teams')
                                  and wt.""IsDeleted""=false
                                  and ""TemplateDescription"" ~* '({s1})' COLLATE ""tr-TR-x-icu"" ";

            var result = await _queryRepo.ExecuteQueryList<WorkBoardTemplateViewModel>(query, null);
            return result;
        }

        public async Task<string> GetJsonContent(string templateTypeId, string workBoardId, DateTime? date = null, string templateTypeCode = null, string workboardItemId = null)
        {
            List<WorkBoardSectionViewModel> list = new List<WorkBoardSectionViewModel>();
            List<WorkBoardItemViewModel> itemlist = new List<WorkBoardItemViewModel>();
            if (workBoardId.IsNotNullAndNotEmpty())
            {
                // Check if sections are available for work board id
                var query = $@"select * from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId""='{workBoardId}' and ""IsDeleted""=false order by ""SequenceOrder""";
                list = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
                if (list.IsNotNull() && list.Count() > 0)
                {
                    // if exist get list item inside section
                    foreach (var section in list)
                    {
                        var query1 = $@"select * from cms.""N_WORKBOARD_WorkBoardItem"" where ""WorkBoardSectionId""='{section.Id}' and ""IsDeleted""=false order by ""SequenceOrder""";
                        section.item = await _queryRepo.ExecuteQueryList<WorkBoardItemViewModel>(query1, null);
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
                    Title = string.Concat(Title, "<li id='" + data.NtsNoteId + "' data-type='card wb-item wb-item-text wb-item-color-"+data.ColorName+"' class='subItem' data-workboarditemid='" + data.Id + "' data-parentindexitemid='" + data.ParentId + "'>" + data.ItemContent + "<span class='fa fa-minus-circle index-item' title='Move Out' data-indexitemid='" + data.NtsNoteId + "' onClick='onMoveOut(this)'></span></li>");
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
                Id = item.WorkBoardSectionId,
                SectionName = item.ItemName,
                HeaderColor = item.ColorName,
            };


            var query = $@"select * from cms.""N_WORKBOARD_WorkBoardItem"" where ""WorkBoardSectionId"" = '{sectionModel.Id}' and ""IsDeleted"" = false ";
            var result = await _queryRepo.ExecuteQueryList<WorkBoardItemViewModel>(query, null);
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
            var query = $@"Select * from cms.""N_WORKBOARD_TemplateType"" where ""Id""='{templateTypeId}' and ""IsDeleted""=false";
            return await _queryRepo.ExecuteQuerySingle<WorkBoardTemplateViewModel>(query, null);
        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForBasic(string workBoardId)
        {
            //var delete = $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""=null where ""WorkBoardId""='{workBoardId}'";
            //await _queryRepo.ExecuteCommand(delete, null);
            var query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST 3 ROWS ONLY ";
            var sections = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            if (sections == null || sections.Count() != 3)
            {
                await GenerateDummyWorkBoardSections(3);
                query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST 3 ROWS ONLY ";
                sections = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            }
            if (sections.IsNotNull() && sections.Count() > 0)
            {
                var Ids = string.Join(",", sections.Select(x => x.Id));
                Ids = Ids.Replace(",", "','");
                // update All sections where workBoardId is null(Top 3) 
                //var query1 = $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}' where ""Id"" in ('{Ids}') and ""IsDeleted""=false";
                //await _queryRepo.ExecuteCommand(query1, null);
                // and then Get using this workboard Id order by Sequence Order

                var i = 1;
                string uquery = string.Empty;
                foreach (var id in sections.Select(x => x.Id))
                {
                    // Change Section Name as Section 1 and Digit as 1(lly for others)
                    uquery = string.Concat(uquery, $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}',""SectionName""='Section {i}' , ""SectionDigit""='{i}',""SequenceOrder""='{i}' where ""Id""='{id}' and ""IsDeleted""=false;");

                    i++;
                }
                await _queryRepo.ExecuteCommand(uquery, null);
                BackgroundJob.Enqueue<Business.HangfireScheduler>(x => x.GenerateDummyWorkBoardSections(100, _userContext.ToIdentityUser()));
                var query2 = $@"select * from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId""='{workBoardId}' and ""IsDeleted""=false order by ""SequenceOrder""";
                return await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query2, null);
            }


            return new List<WorkBoardSectionViewModel>();
        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForMonthly(string workBoardId, DateTime date)
        {
            var days = DateTime.DaysInMonth(date.Year, date.Month);
            //date.IsLastDayOfMonth();
            // update All sections where workBoard Id is null(Days in month) and then Get using this workboard Id
            var query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST {days} ROWS ONLY ";
            var sections = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            if (sections == null || sections.Count() != days)
            {
                await GenerateDummyWorkBoardSections(days);
                query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST {days} ROWS ONLY ";
                sections = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            }
            if (sections.IsNotNull() && sections.Count() > 0)
            {
                var Ids = string.Join(",", sections.Select(x => x.Id));
                Ids = Ids.Replace(",", "','");
                // update All sections where workBoardId is null(Days in month) 
                // var query1 = $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}' where ""Id"" in ('{Ids}') and ""IsDeleted""=false";
                // await _queryRepo.ExecuteCommand(query1, null);

                var i = 1;
                string uquery = string.Empty;
                foreach (var id in sections.Select(x => x.Id))
                {
                    var SectionName = new DateTime(date.Year, date.Month, i);
                    // Change Section Name as 1 March 2022 and Digit as 1(lly for others)
                    uquery = string.Concat(uquery, $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}',""SectionName""='{SectionName.ToString("dd MMMM yyyy")}' , ""SectionDigit""='{i}',""SequenceOrder""='{i}' where ""Id""='{id}' and ""IsDeleted""=false;");

                    i++;
                }
                await _queryRepo.ExecuteCommand(uquery, null);
                BackgroundJob.Enqueue<Business.HangfireScheduler>(x => x.GenerateDummyWorkBoardSections(100, _userContext.ToIdentityUser()));
                // and then Get using this workboard Id order by Sequence Order
                var query2 = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId""='{workBoardId}' and ""IsDeleted""=false order by ""SequenceOrder""";
                return await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query2, null);
            }
            // job to generateDummyWorkBoardSections(No Of Items)
            return new List<WorkBoardSectionViewModel>();

        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForWeekly(string workBoardId, DateTime date)
        {

            // update All sections where workBoard Id is null(Days in month) and then Get using this workboard Id
            var query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST 7 ROWS ONLY ";
            var sections = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            if (sections == null || sections.Count() != 7)
            {
                await GenerateDummyWorkBoardSections(7);
                query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST 7 ROWS ONLY ";
                sections = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            }
            if (sections.IsNotNull() && sections.Count() > 0)
            {
                var Ids = string.Join(",", sections.Select(x => x.Id));
                Ids = Ids.Replace(",", "','");
                // update All sections where workBoardId is null(Days in month) 
                //var query1 = $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}' where ""Id"" in ('{Ids}') and ""IsDeleted""=false";
                // await _queryRepo.ExecuteCommand(query1, null);
                // and then Get using this workboard Id order by Sequence Order

                var i = 0;
                string uquery = string.Empty;
                foreach (var id in sections.Select(x => x.Id))
                {
                    // Change Section Name as Section 1 and Digit as 1(lly for others)
                    uquery = string.Concat(uquery, $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}',""SectionName""='{Enum.GetName(typeof(DayOfWeek), i)}' , ""SectionDigit""='{i}',""SequenceOrder""='{i}' where ""Id""='{id}' and ""IsDeleted""=false;");

                    i++;
                }
                await _queryRepo.ExecuteCommand(uquery, null);
                // job to generateDummyWorkBoardSections(No Of Items)
                BackgroundJob.Enqueue<Business.HangfireScheduler>(x => x.GenerateDummyWorkBoardSections(100, _userContext.ToIdentityUser()));
                var query2 = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId""='{workBoardId}' and ""IsDeleted""=false order by ""SequenceOrder""";
                return await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query2, null);
            }


            return new List<WorkBoardSectionViewModel>();
        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForYearly(string workBoardId, DateTime date)
        {

            // update All sections where workBoard Id is null(Days in month) and then Get using this workboard Id
            var query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST 12 ROWS ONLY ";
            var sections = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            if (sections == null || sections.Count() != 7)
            {
                await GenerateDummyWorkBoardSections(12);
                query = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId"" is null and ""IsDeleted""=false FETCH FIRST 12 ROWS ONLY ";
                sections = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            }
            if (sections.IsNotNull() && sections.Count() > 0)
            {
                var Ids = string.Join(",", sections.Select(x => x.Id));
                Ids = Ids.Replace(",", "','");
                // update All sections where workBoardId is null(Days in month) 
                //var query1 = $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}' where ""Id"" in ('{Ids}') and ""IsDeleted""=false";
                //await _queryRepo.ExecuteCommand(query1, null);
                // and then Get using this workboard Id order by Sequence Order

                var i = 1;
                string uquery = string.Empty;
                foreach (var id in sections.Select(x => x.Id))
                {
                    // Change Section Name as Section 1 and Digit as 1(lly for others)
                    uquery = string.Concat(uquery, $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}',""SectionName""='{Enum.GetName(typeof(MonthEnum), i)}' , ""SectionDigit""='{i}',""SequenceOrder""='{i}' where ""Id""='{id}' and ""IsDeleted""=false;");

                    i++;
                }
                await _queryRepo.ExecuteCommand(uquery, null);
                BackgroundJob.Enqueue<Business.HangfireScheduler>(x => x.GenerateDummyWorkBoardSections(100, _userContext.ToIdentityUser()));
                var query2 = $@"select ""Id"" from cms.""N_WORKBOARD_WorkBoardSection"" where ""WorkBoardId""='{workBoardId}' and ""IsDeleted""=false order by ""SequenceOrder""";
                return await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query2, null);

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
            var query = @$"SELECT wb.""Id"" as WorkboardId, wb.""WorkBoardName"" as WorkBoardName, wb.""WorkBoardDescription"" as WorkBoardDescription,
                            wb.""TemplateTypeId"" as TemplateTypeId,tt.""TemplateName"" as TemplateTypeName,
                            wb.""NtsNoteId"" as NoteId , wb.""IconFileId"" as IconFileId, wb.""WorkBoardStatus"" as WorkBoardStatus
                            FROM cms.""N_WORKBOARD_WorkBoard"" as wb
                            join cms.""N_WORKBOARD_TemplateType"" as tt on tt.""Id"" = wb.""TemplateTypeId""
                            where wb.""IsDeleted"" = false and wb.""WorkBoardStatus"" = '{(int)status}'
                            and wb.""Id"" != '{id}' ";
            var querydata = await _queryRepo.ExecuteQueryList<WorkBoardViewModel>(query, null);
            return querydata;
        }

        public async Task<List<WorkBoardSectionViewModel>> GetWorkboardSectionList(string id)
        {
            var query = $@"Select sec.""Id"" as ""Id"", sec.""SectionName"" as ""SectionName"" 
                            from cms.""N_WORKBOARD_WorkBoardSection"" as sec
                            where sec.""IsDeleted"" = false and sec.""WorkBoardId"" = '{id}' ";
            var result = await _queryRepo.ExecuteQueryList<WorkBoardSectionViewModel>(query, null);
            return result;
        }

        public async Task<WorkBoardItemViewModel> GetWorkBoardItemByNtsNoteId(string ntsNoteId)
        {
            var query = $@"Select wbi.*
                        ,case when wbi.""ItemType""='2' then wbi.""ItemContent"" end as ItemContentIndex
                        ,case when wbi.""ItemType""='6' then wbi.""ItemFileId"" end as ItemFileFileId
from cms.""N_WORKBOARD_WorkBoardItem"" as wbi
                            where wbi.""IsDeleted"" = false and wbi.""NtsNoteId"" = '{ntsNoteId}' ";
            var result = await _queryRepo.ExecuteQuerySingle<WorkBoardItemViewModel>(query, null);
            return result;
        }
        public async Task<WorkBoardItemViewModel> GetWorkboardItemById(string id)
        {
            var query = $@"Select *,""Id"" ""WorkBoardItemId"" from cms.""N_WORKBOARD_WorkBoardItem""
                            where ""IsDeleted"" = false and ""Id"" = '{id}' ";
            var result = await _queryRepo.ExecuteQuerySingle<WorkBoardItemViewModel>(query, null);
            return result;
        }

        public async Task<WorkBoardItemViewModel> GetWorkboardItemByNoteId(string id)
        {
            var query = $@"Select *,""Id"" ""WorkBoardItemId"" from cms.""N_WORKBOARD_WorkBoardItem""
                            where ""IsDeleted"" = false and ""NtsNoteId"" = '{id}' ";
            var result = await _queryRepo.ExecuteQuerySingle<WorkBoardItemViewModel>(query, null);
            return result;
        }

        public async Task<bool> UpdateWorkboardItem(string workboardId, string sectionId, string workboardItemId)
        {
            var query = @$"UPDATE cms.""N_WORKBOARD_WorkBoardItem""
                                SET ""WorkBoardId"" = '{workboardId}', ""WorkBoardSectionId"" = '{sectionId}'
                                WHERE ""Id"" = '{workboardItemId}';";
            await _queryRepo.ExecuteCommand(query, null);
            return true;
        }

        public async Task<IList<UserViewModel>> GetUserList(string noteId)
        {
            string query = @$"SELECT * ,CONCAT(""Name"",'<',""Email"",'>') as Name
                            FROM public.""User"" as t
                            join public.""UserPortal"" as up on up.""UserId""=t.""Id"" and up.""IsDeleted""=false and up.""PortalId""='{_userContext.PortalId}'
                            where t.""Id"" not in (select ""SharedWithUserId"" from public.""NtsNoteShared"" where ""NtsNoteId""= '{noteId}' and ""IsDeleted""=false)
                                and t.""IsDeleted"" = false ";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }

        public async Task<List<WorkBoardViewModel>> GetSharedWorkboardList(WorkBoardstatusEnum status, string sharedWithUserId)
        {
            var query = @$"SELECT wb.""Id"" as WorkboardId, wb.""WorkBoardName"" as WorkBoardName, wb.""WorkBoardDescription"" as WorkBoardDescription,
                            wb.""TemplateTypeId"" as TemplateTypeId,tt.""TemplateName"" as TemplateTypeName,
                            wb.""NtsNoteId"" as NoteId , tt.""ContentImage"" as IconFileId, wb.""WorkBoardStatus"" as WorkBoardStatus
                            FROM public.""NtsNoteShared"" as t
                            join cms.""N_WORKBOARD_WorkBoard"" as wb on t.""NtsNoteId"" = wb.""NtsNoteId""
                            join cms.""N_WORKBOARD_TemplateType"" as tt on tt.""Id"" = wb.""TemplateTypeId"" and tt.""IsDeleted"" = false                            
                            where wb.""IsDeleted"" = false and wb.""WorkBoardStatus"" = '{(int)status}' 
                            and t.""SharedWithUserId"" = '{sharedWithUserId}' and t.""IsDeleted"" = false
                        Union
                            SELECT wb.""Id"" as WorkboardId, wb.""WorkBoardName"" as WorkBoardName, wb.""WorkBoardDescription"" as WorkBoardDescription,
                            wb.""TemplateTypeId"" as TemplateTypeId,tt.""TemplateName"" as TemplateTypeName,
                            wb.""NtsNoteId"" as NoteId , tt.""ContentImage"" as IconFileId, wb.""WorkBoardStatus"" as WorkBoardStatus
                            FROM cms.""N_WORKBOARD_WorkBoard"" as wb
                            join cms.""N_WORKBOARD_TemplateType"" as tt on tt.""Id"" = wb.""TemplateTypeId"" and tt.""IsDeleted"" = false
                            join cms.""N_WORKBOARD_WorkBoardShare"" as wbs on wbs.""WorkBoardId""=wb.""Id"" and wbs.""IsDeleted""=false
                            join public.""User"" as u on u.""Email""=wbs.""EmailAddress"" and u.""IsDeleted""=false and u.""Id""='{sharedWithUserId}'
                            where wb.""IsDeleted"" = false and wb.""WorkBoardStatus"" = '{(int)status}'";
            var querydata = await _queryRepo.ExecuteQueryList<WorkBoardViewModel>(query, null);
            return querydata;
        }
    }
}
