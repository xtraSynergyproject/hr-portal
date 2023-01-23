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
    public class WorkboardBusiness : IWorkboardBusiness
    {
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly INoteBusiness _noteBusiness;
        private readonly IUserContext _userContext;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;

        public WorkboardBusiness(IRepositoryQueryBase<NoteViewModel> queryRepo, INoteBusiness noteBusiness, IUserContext userContext, ICmsQueryBusiness cmsQueryBusiness)
        {
            _queryRepo = queryRepo;
            _noteBusiness = noteBusiness;
            _userContext = userContext;
            _cmsQueryBusiness = cmsQueryBusiness;
        }
        public async Task<List<WorkBoardViewModel>> GetWorkboardList(WorkBoardstatusEnum status,string id=null)
        {
            if (id != null)
            {

                var querydata1 = await _cmsQueryBusiness.GetWorkboardList(status, id);
                return querydata1;
            }

            var list = new List<WorkBoardViewModel>();
            list.Add(new WorkBoardViewModel { Id = "new" });

            var querydata = await _cmsQueryBusiness.GetWorkboardListData(status, id);
            list.AddRange(querydata);
            return list;
        }

        public async Task<List<WorkBoardViewModel>> GetWorkboardTaskList()
        {
            var list = new List<WorkBoardViewModel>();
            list.Add(new WorkBoardViewModel { Id = "new" });

            var querydata = await _cmsQueryBusiness.GetWorkboardTaskList();
            list.AddRange(querydata);
            return list;
        }

        public async Task<bool> UpdateWorkBoardStatus(string id, WorkBoardstatusEnum status)
        {

            await _cmsQueryBusiness.UpdateWorkBoardStatus(id, status);
            return true;
        }
        public async Task<WorkBoardSectionViewModel> GetWorkBoardSectionDetails(string sectionId)
        {

            var querydata = await _cmsQueryBusiness.GetWorkBoardSectionDetails(sectionId);
            return querydata;
        }

        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionListByWorkbBoardId(string workboardId)
        {

            var querydata = await _cmsQueryBusiness.GetWorkBoardSectionListByWorkbBoardId(workboardId);
            return querydata;
        }


        public async Task<WorkBoardItemViewModel> GetWorkBoardItemDetails(string itemId)
        {

            var querydata = await _cmsQueryBusiness.GetWorkBoardItemDetails(itemId);
            return querydata;
        }
        public async Task<IList<WorkBoardItemViewModel>> GetWorkBoardItemBySectionId(string sectionId)
        {

            var querydata = await _cmsQueryBusiness.GetWorkBoardItemBySectionId(sectionId);
            return querydata;
        }
        public async Task<IList<WorkBoardItemViewModel>> GetWorkBoardItemByParentId(string parentId)
        {

            var querydata = await _cmsQueryBusiness.GetWorkBoardItemByParentId(parentId);
            return querydata;
        }

        public async Task<List<WorkBoardItemViewModel>> GetItemBySectionId(string sectionId)
        {

            var querydata = await _cmsQueryBusiness.GetItemBySectionId(sectionId);
            return querydata;
        }

        public async Task DeleteItem(string itemId)
        {

            await _cmsQueryBusiness.DeleteItem(itemId);


        }
        public async Task DeleteSection(string itemId)
        {

            await _cmsQueryBusiness.DeleteSection(itemId);

        }
        public async Task UpdateWorkBoardJson(WorkBoardViewModel data)
        {

            await _cmsQueryBusiness.UpdateWorkBoardJson(data);

        }
        public async Task UpdateWorkBoardSectionSequenceOrder(WorkBoardSectionViewModel data)
        {
            await _cmsQueryBusiness.UpdateWorkBoardSectionSequenceOrder(data);

        }
        public async Task UpdateWorkBoardItemDetails(WorkBoardItemViewModel data)
        {

            await _cmsQueryBusiness.UpdateWorkBoardItemDetails(data);

        }
        public async Task UpdateWorkBoardItemSequenceOrder(WorkBoardItemViewModel data)
        {

            await _cmsQueryBusiness.UpdateWorkBoardItemSequenceOrder(data);

        }
        public async Task UpdateWorkBoardItemSectionId(WorkBoardItemViewModel data)
        {
            //var query = @$"Update cms.""N_WORKBOARD_WorkBoardItem"" set ""WorkBoardSectionId""='{data.WorkBoardSectionId}', ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_userContext.UserId}'
            //                where ""NtsNoteId""='{data.WorkBoardItemId}' and ""IsDeleted""=false ";

            await _cmsQueryBusiness.UpdateWorkBoardItemSectionId(data);

        }
        public async Task<List<WorkBoardInviteDetailsViewModel>> GetWorkBoardInvites(string workBoradId)
        {
;
            var querydata = await _cmsQueryBusiness.GetWorkBoardInvites(workBoradId); 
            return querydata;
        }
        public async Task<WorkBoardViewModel> GetWorkBoardDetails(string workBoradId)
        {

            var querydata = await _cmsQueryBusiness.GetWorkBoardDetails(workBoradId);
            return querydata;
        }
        public async Task<WorkBoardViewModel> GetWorkBoardDetail(string workBoradId)
        {

            var querydata = await _cmsQueryBusiness.GetWorkBoardDetail(workBoradId);
            return querydata;
        }
        public async Task<WorkBoardViewModel> GetWorkBoardDetailsByIdKey(string workBoardUniqueId, string shareKey)
        {
     
            var querydata = await _cmsQueryBusiness.GetWorkBoardDetailsByIdKey(workBoardUniqueId, shareKey);
            return querydata;
        }

        public async Task<List<WorkBoardTemplateViewModel>> GetTemplateList()
        {

            var result = await _cmsQueryBusiness.GetTemplateList();
            return result;
        }

        public async Task<List<LOVViewModel>> GetTemplateCategoryList()
        {
            var result = await _cmsQueryBusiness.GetTemplateCategoryList();
            return result;
        }

        public async Task<List<WorkBoardTemplateViewModel>> GetSearchResults(string[] values)
        {
 

            var result = await _cmsQueryBusiness.GetSearchResults( values);
            return result;
        }

        public async Task<string> GetJsonContent(string templateTypeId, string workBoardId, DateTime? date = null, string templateTypeCode = null, string workboardItemId = null, WorkBoardOwnerTypeEnum? ownerType=null)
        {
            List<WorkBoardSectionViewModel> list = new List<WorkBoardSectionViewModel>();
            List<WorkBoardItemViewModel> itemlist = new List<WorkBoardItemViewModel>();
            if (workBoardId.IsNotNullAndNotEmpty())
            {
                // Check if sections are available for work board id
                list = await _cmsQueryBusiness.GetJsonContent(workBoardId,list);
                if (list.IsNotNull() && list.Count() > 0)
                {
                    // if exist get list item inside section
                    var itemList = await _cmsQueryBusiness.GetJsonContentData(list);
                    itemlist.AddRange(itemList);
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
                    title = GetItemContent(z, itemlist, ownerType),
                    @class = "wb-item-color-" + (z.ColorName.IsNullOrEmpty() ? "yellow" : z.ColorName) + " wb-item-shape-" + Enum.GetName(typeof(WorkBoardItemSizeEnum), z.WorkBoardItemShape) + " wb-item-size-" + Enum.GetName(typeof(WorkBoardItemSizeEnum), z.WorkBoardItemSize),
                }),
                sectionDigit = x.SectionDigit,
                ownerUserId = x.OwnerUserId
            }).ToList();
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(json);
            return data;
        }


        public async Task<List<WorkBoardSectionViewModel>> GetSectionContent(string templateTypeId, string workBoardId, DateTime? date = null, string templateTypeCode = null, string workboardItemId = null, WorkBoardOwnerTypeEnum? ownerType = null)
        {
            List<WorkBoardSectionViewModel> list = new List<WorkBoardSectionViewModel>();
            List<WorkBoardItemViewModel> itemlist = new List<WorkBoardItemViewModel>();
            if (workBoardId.IsNotNullAndNotEmpty())
            {
                // Check if sections are available for work board id
                list = await _cmsQueryBusiness.GetJsonContent(workBoardId, list);
                if (list.IsNotNull() && list.Count() > 0)
                {
                    // if exist get list item inside section
                    var itemList = await _cmsQueryBusiness.GetJsonContentData(list);
                    itemlist.AddRange(itemList);
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
            foreach (var item in list) // _list is an instance of List<string>
            {
                
                List<WorkBoardItemViewModel> items = (from pair in itemlist
                                                     where pair.WorkBoardSectionId == item.WorkBoardSectionId
                                                     select pair).ToList();
                item.item.AddRange(items);
            }
            

            
            //var json = list.Select(x => new
            //{
            //    id = x.Id,
            //    title = x.SectionDigit != 0 ? string.Concat(x.SectionDigit, ".", x.SectionName) : x.SectionName,
            //    @class = "section-" + (x.HeaderColor.IsNullOrEmpty() ? "blue" : x.HeaderColorName),

            //    item = itemlist.Where(y => y.WorkBoardSectionId == x.Id && y.ParentId.IsNullOrEmpty()).Select(z => new
            //    {
            //        id = z.Id,
            //        title = GetItemContent(z, itemlist, ownerType),
            //        @class = "wb-item-color-" + (z.ColorName.IsNullOrEmpty() ? "yellow" : z.ColorName) + " wb-item-shape-" + Enum.GetName(typeof(WorkBoardItemSizeEnum), z.WorkBoardItemShape) + " wb-item-size-" + Enum.GetName(typeof(WorkBoardItemSizeEnum), z.WorkBoardItemSize),
            //    }),
            //    sectionDigit = x.SectionDigit,
            //    ownerUserId = x.OwnerUserId
        //}).ToList();
            //var data = Newtonsoft.Json.JsonConvert.SerializeObject(json);
            return list;
        }


        public string GetItemContent(WorkBoardItemViewModel model, List<WorkBoardItemViewModel> list, WorkBoardOwnerTypeEnum? ownerType=null)
        {
            string Title = string.Empty;
            string display = "block";
            if(ownerType == WorkBoardOwnerTypeEnum.Viewer)
            {
                display = "none";
            }
            else if(ownerType == WorkBoardOwnerTypeEnum.Contributor)
            {
                display = model.OwnerUserId == _userContext.UserId ? "block":"none";
            }            

            model.WorkBoardItemShape = model.WorkBoardItemShape != 0 ? model.WorkBoardItemShape : WorkBoardItemShapeEnum.Square;
            model.WorkBoardItemSize = model.WorkBoardItemSize != 0 ? model.WorkBoardItemSize : WorkBoardItemSizeEnum.Standard;
            if (model.ItemType == WorkBoardItemTypeEnum.Text)
            {
                Title = "<div data-action='Edit' id='" + model.NtsNoteId + "' data-workBoardItemId='" + model.Id + "' class='card wb-item wb-item-text wb-item-color-" + model.ColorName + " wb-item-shape-" + Enum.GetName(typeof(WorkBoardItemShapeEnum), model.WorkBoardItemShape).ToLowerInvariant() + " wb-item-size-" + Enum.GetName(typeof(WorkBoardItemSizeEnum), model.WorkBoardItemSize).ToLowerInvariant() + "'><div class='card-header'><span class='wb-item-icons'><i data-id='" + model.NtsNoteId + "' onclick='getMessages(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-message-middle'></i><i data-id='" + model.NtsNoteId + "' onclick='getTaskList(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-list-check'></i><i data-id='" + model.NtsNoteId + "' class='wb-item-icon wb-item-menu fa fa-ellipsis-v' style='display:"+display+"'></i></span></div><div class='card-body'><p class='card-text'>" + model.ItemContent + "</p></div></div>";
            }
            else if (model.ItemType == WorkBoardItemTypeEnum.Index)
            {
                Title = "<div data-action='Edit' id='" + model.NtsNoteId + "'  data-workBoardItemId='" + model.Id + "' class='card wb-item wb-item-index wb-item-color-" + model.ColorName + " wb-item-shape-" + Enum.GetName(typeof(WorkBoardItemShapeEnum), model.WorkBoardItemShape).ToLowerInvariant() + " wb-item-size-" + Enum.GetName(typeof(WorkBoardItemSizeEnum), model.WorkBoardItemSize).ToLowerInvariant() + "'><div class='card-header'><span class='card-text'>" + model.ItemName + "</span><span class='wb-item-icons'><i data-id='" + model.Id + "' onclick='getCard(\"" + model.Id + "\");' class='wb-item-icon fa fa-external-link' title='Open this Index in Work Book'></i><i data-id='" + model.NtsNoteId + "' onclick='getMessages(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-message-middle' title='Comments'></i><i data-id='" + model.NtsNoteId + "' onclick='getTaskList(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-list-check' title='Tasks'></i><i data-id='" + model.NtsNoteId + "' class='wb-item-icon wb-item-menu fa fa-ellipsis-v' title='Actions' style='display:" + display + "'></i></span></div><div class='card-body'><ul>";
                foreach (var data in list.Where(x => x.ParentId == model.Id))
                {
                    Title = string.Concat(Title, "<li id='" + data.NtsNoteId + "' data-type='card wb-item wb-item-text wb-item-color-"+data.ColorName+"' class='subItem' data-workboarditemid='" + data.Id + "' data-parentindexitemid='" + data.ParentId + "'>" + data.ItemContent + "<span class='fa fa-minus-circle index-item' title='Move Out' data-indexitemid='" + data.NtsNoteId + "' onClick='onMoveOut(this)'></span></li>");
                }
                Title = string.Concat(Title, "</ul></div></div>");
            }
            else if (model.ItemType == WorkBoardItemTypeEnum.WhiteBoard)
            {
                Title = "<div data-action='Edit' id='" + model.NtsNoteId + "' data-workBoardItemId='" + model.Id + "' class='card wb-item wb-item-whiteboard wb-item-color-" + model.ColorName + " wb-item-shape-" + Enum.GetName(typeof(WorkBoardItemShapeEnum), model.WorkBoardItemShape).ToLowerInvariant() + " wb-item-size-" + Enum.GetName(typeof(WorkBoardItemSizeEnum), model.WorkBoardItemSize).ToLowerInvariant() + "'><div class='card-header'><span class='wb-item-icons'><i data-id='" + model.NtsNoteId + "' onclick='getMessages(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-message-middle'></i><i data-id='" + model.NtsNoteId + "' onclick='getTaskList(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-list-check'></i><i data-id='" + model.NtsNoteId + "' class='wb-item-icon wb-item-menu fa fa-ellipsis-v' style='display:" + display + "'></i></span></div><div class='card-body'><div class='card-content' style = 'background-color:#fff;' ><img id='" + model.ItemFileId + "' src='/cms/Document/GetImageMongo?id=" + model.ItemFileId + "' style='max-width:100%;max-height:100 %;' /></div></div></div>";
            }
            else if (model.ItemType == WorkBoardItemTypeEnum.Image)
            {
                Title = "<div data-action='Edit' id='" + model.NtsNoteId + "' data-workBoardItemId='" + model.Id + "' class='card wb-item wb-item-image wb-item-color-" + model.ColorName + " wb-item-shape-" + Enum.GetName(typeof(WorkBoardItemShapeEnum), model.WorkBoardItemShape).ToLowerInvariant() + " wb-item-size-" + Enum.GetName(typeof(WorkBoardItemSizeEnum), model.WorkBoardItemSize).ToLowerInvariant() + "'><div class='card-header'><span class='wb-item-icons'><i data-id='" + model.NtsNoteId + "' onclick='getMessages(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-message-middle'></i><i data-id='" + model.NtsNoteId + "' onclick='getTaskList(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-list-check'></i><i data-id='" + model.NtsNoteId + "' class='wb-item-icon wb-item-menu fa fa-ellipsis-v' style='display:" + display + "'></i></span></div><div class='card-body'><div class='card-content'><img id ='" + model.ItemFileId + "' src='/cms/Document/GetImageMongo?id=" + model.ItemFileId + "' style='max-width:100%;max-height:100%;' /></div></div></div>";
            }
            else if (model.ItemType == WorkBoardItemTypeEnum.Video)
            {

            }
            else if (model.ItemType == WorkBoardItemTypeEnum.File)
            {
                Title = "<div data-action='Edit' id='" + model.NtsNoteId + "' data-workBoardItemId='" + model.Id + "' class='card wb-item wb-item-file wb-item-color" + model.ColorName + " wb-item-shape-" + Enum.GetName(typeof(WorkBoardItemShapeEnum), model.WorkBoardItemShape).ToLowerInvariant() + " wb-item-size-" + Enum.GetName(typeof(WorkBoardItemSizeEnum), model.WorkBoardItemSize).ToLowerInvariant() + "'><div class='card-header'><span class='wb-item-icons'><i data-id='" + model.NtsNoteId + "' onclick='getMessages(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-message-middle'></i><i data-id='" + model.NtsNoteId + "' onclick='getTaskList(\"" + model.NtsNoteId + "\");' class='wb-item-icon fa fa-list-check'></i><i data-id='" + model.NtsNoteId + "' class='wb-item-icon wb-item-menu fa fa-ellipsis-v' style='display:" + display + "'></i></span></div><div class='card-body'><div class='card-content'><img id = '" + model.ItemFileId + "' src='/cms/Document/GetSnapMongo?id=" + model.ItemFileId + "' onerror='OnDocError(this)' style='max-width:100%;max-height:100%;' /></div></div></div>";
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

            var result = await _cmsQueryBusiness.GetWorkBoardSectionForIndex(item);
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
            return await _cmsQueryBusiness.GetWorkBoardTemplateById(templateTypeId);
        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForBasic(string workBoardId)
        {
            //var delete = $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""=null where ""WorkBoardId""='{workBoardId}'";
            //await _queryRepo.ExecuteCommand(delete, null);
            var sections = await _cmsQueryBusiness.GetWorkBoardSectionForBasic();
            if (sections == null || sections.Count() != 3)
            {
                await GenerateDummyWorkBoardSections(3);
                sections = await _cmsQueryBusiness.GetWorkBoardSectionForBasicData();
            }
            if (sections.IsNotNull() && sections.Count() > 0)
            {
                var Ids = string.Join(",", sections.Select(x => x.Id));
                Ids = Ids.Replace(",", "','");
                // update All sections where workBoardId is null(Top 3) 
                //var query1 = $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}' where ""Id"" in ('{Ids}') and ""IsDeleted""=false";
                //await _queryRepo.ExecuteCommand(query1, null);
                // and then Get using this workboard Id order by Sequence Order

                return await _cmsQueryBusiness.GetWorkBoardSectionForBasicDataUpdate(workBoardId, sections);
            }


            return new List<WorkBoardSectionViewModel>();
        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForMonthly(string workBoardId, DateTime date)
        {
            var days = DateTime.DaysInMonth(date.Year, date.Month);
            //date.IsLastDayOfMonth();
            // update All sections where workBoard Id is null(Days in month) and then Get using this workboard Id
            var sections = await _cmsQueryBusiness.GetWorkBoardSectionForMonthly(days);
            if (sections == null || sections.Count() != days)
            {
                await GenerateDummyWorkBoardSections(days);
                sections = await _cmsQueryBusiness.GetWorkBoardSectionForMonthlyData(days);
            }
            if (sections.IsNotNull() && sections.Count() > 0)
            {
                var Ids = string.Join(",", sections.Select(x => x.Id));
                Ids = Ids.Replace(",", "','");
                // update All sections where workBoardId is null(Days in month) 
                // var query1 = $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}' where ""Id"" in ('{Ids}') and ""IsDeleted""=false";
                // await _queryRepo.ExecuteCommand(query1, null);


                return await _cmsQueryBusiness.GetWorkBoardSectionForMonthlyDataUpdate(workBoardId, sections, date);
            }
            // job to generateDummyWorkBoardSections(No Of Items)
            return new List<WorkBoardSectionViewModel>();

        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForWeekly(string workBoardId, DateTime date)
        {

            // update All sections where workBoard Id is null(Days in month) and then Get using this workboard Id
            var sections = await _cmsQueryBusiness.GetWorkBoardSectionForWeekly();
            if (sections == null || sections.Count() != 7)
            {
                await GenerateDummyWorkBoardSections(7);
                sections = await _cmsQueryBusiness.GetWorkBoardSectionForWeeklyData();
            }
            if (sections.IsNotNull() && sections.Count() > 0)
            {
                var Ids = string.Join(",", sections.Select(x => x.Id));
                Ids = Ids.Replace(",", "','");
                // update All sections where workBoardId is null(Days in month) 
                //var query1 = $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}' where ""Id"" in ('{Ids}') and ""IsDeleted""=false";
                // await _queryRepo.ExecuteCommand(query1, null);
                // and then Get using this workboard Id order by Sequence Order

                return await _cmsQueryBusiness.GetWorkBoardSectionForWeeklyDataUpdate(workBoardId,sections);
            }


            return new List<WorkBoardSectionViewModel>();
        }
        public async Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForYearly(string workBoardId, DateTime date)
        {

            // update All sections where workBoard Id is null(Days in month) and then Get using this workboard Id
            var sections = await _cmsQueryBusiness.GetWorkBoardSectionForYearly();
            if (sections == null || sections.Count() != 7)
            {
                await GenerateDummyWorkBoardSections(12);
                sections = await _cmsQueryBusiness.GetWorkBoardSectionForYearlyData();
            }
            if (sections.IsNotNull() && sections.Count() > 0)
            {
                var Ids = string.Join(",", sections.Select(x => x.Id));
                Ids = Ids.Replace(",", "','");
                // update All sections where workBoardId is null(Days in month) 
                //var query1 = $@"Update cms.""N_WORKBOARD_WorkBoardSection"" set ""WorkBoardId""='{workBoardId}' where ""Id"" in ('{Ids}') and ""IsDeleted""=false";
                //await _queryRepo.ExecuteCommand(query1, null);
                // and then Get using this workboard Id order by Sequence Order

                return await _cmsQueryBusiness.GetWorkBoardSectionForYearlyDataUpdate(workBoardId,sections);

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



        //public async Task<List<WorkBoardViewModel>> GetOtherWorkboardList(WorkBoardstatusEnum status, string id)
        //{
        //    var query = @$"SELECT wb.""Id"" as WorkboardId, wb.""WorkBoardName"" as WorkBoardName, wb.""WorkBoardDescription"" as WorkBoardDescription,
        //                    wb.""TemplateTypeId"" as TemplateTypeId,tt.""TemplateName"" as TemplateTypeName,
        //                    wb.""NtsNoteId"" as NoteId , wb.""IconFileId"" as IconFileId, wb.""WorkBoardStatus"" as WorkBoardStatus
        //                    FROM cms.""N_WORKBOARD_WorkBoard"" as wb
        //                    join cms.""N_WORKBOARD_TemplateType"" as tt on tt.""Id"" = wb.""TemplateTypeId""
        //                    where wb.""IsDeleted"" = false and wb.""WorkBoardStatus"" = '{(int)status}'
        //                    and wb.""Id"" != '{id}' ";
        //    var querydata = await _queryRepo.ExecuteQueryList<WorkBoardViewModel>(query, null);
        //    return querydata;
        //}

        public async Task<List<WorkBoardSectionViewModel>> GetWorkboardSectionList(string id)
        {
            var result = await _cmsQueryBusiness.GetWorkboardSectionList(id);
            return result;
        }

        public async Task<WorkBoardItemViewModel> GetWorkBoardItemByNtsNoteId(string ntsNoteId)
        {
            var result = await _cmsQueryBusiness.GetWorkBoardItemByNtsNoteId(ntsNoteId);
            return result;
        }
        public async Task<WorkBoardItemViewModel> GetWorkboardItemById(string id)
        {
            var result = await _cmsQueryBusiness.GetWorkboardItemById(id);
            return result;
        }

        public async Task<WorkBoardItemViewModel> GetWorkboardItemByNoteId(string id)
        {
            var result = await _cmsQueryBusiness.GetWorkboardItemByNoteId(id);
            return result;
        }

        public async Task<bool> UpdateWorkboardItem(string workboardId, string sectionId, string workboardItemId)
        {

            await _cmsQueryBusiness.UpdateWorkboardItem(workboardId, sectionId, workboardItemId);
            return true;
        }

        public async Task<IList<UserViewModel>> GetUserList(string noteId)
        {
            var list = await _cmsQueryBusiness.GetUserList(noteId);
            return list;
        }

        public async Task<List<WorkBoardViewModel>> GetSharedWorkboardList(WorkBoardstatusEnum status, string sharedWithUserId)
        {
            var querydata = await _cmsQueryBusiness.GetSharedWorkboardList(status, sharedWithUserId);
            return querydata;
        }
    }
}
