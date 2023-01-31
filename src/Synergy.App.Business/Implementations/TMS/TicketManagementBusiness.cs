using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class TicketManagementBusiness : BusinessBase<NoteViewModel, NtsNote>, ITicketAssessmentBusiness
    {
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<AssessmentQuestionsViewModel> _queryAssQues;
        private IUserContext _userContext;
        private IServiceBusiness _serviceBusiness;
        private INoteBusiness _noteBusiness;
        private readonly IServiceProvider _sp;
        private readonly IRepositoryQueryBase<AssessmentQuestionsOptionViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<AssignmentViewModel> _queryAssignment;
        private readonly ITicketManagementQueryBusiness _ticketManagementQueryBusiness;
        public TicketManagementBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper
            , IRepositoryQueryBase<AssessmentQuestionsOptionViewModel> queryRepo1, IServiceProvider sp,
            IUserContext userContext, IServiceBusiness serviceBusiness, INoteBusiness noteBusiness,
            IRepositoryQueryBase<IdNameViewModel> queryRepo, IRepositoryQueryBase<AssignmentViewModel> queryAssignment
            , ITicketManagementQueryBusiness ticketManagementQueryBusiness
           , IRepositoryQueryBase<AssessmentQuestionsViewModel> queryAssQues) : base(repo, autoMapper)
        {
            _queryRepo1 = queryRepo1;
            _sp = sp;
            _userContext = userContext;
            _serviceBusiness = serviceBusiness;
            _noteBusiness = noteBusiness;
            _queryRepo = queryRepo;
            _queryAssignment = queryAssignment;
            _queryAssQues = queryAssQues;
            _ticketManagementQueryBusiness = ticketManagementQueryBusiness;
        }

        public async Task<List<HelpDeskViewModel>> GetRequestCounts(string type)
        {
            List<HelpDeskViewModel> result = new();
            if (type == "Category")
            {
                result = await _ticketManagementQueryBusiness.GetRequestCountsByCategory();
            }
            else if (type == "Service")
            {
                result = await _ticketManagementQueryBusiness.GetRequestCountsByService();
            }
            else if (type == "Priority")
            {
                result = await _ticketManagementQueryBusiness.GetRequestCountsByPriority();
            } 
            else if (type == "Owner")
            {
                result = await _ticketManagementQueryBusiness.GetRequestCountsByOwner();
            }

            return result;
        }

        public async Task<List<HelpDeskViewModel>> GetRequestCountsSLAViolation(string type)
        {
            List<HelpDeskViewModel> result = new();
            if (type == "Category")
            {
                result = await _ticketManagementQueryBusiness.GetRequestCountsSLAViolationByCategory();
            }
            else if (type == "Service")
            {
                result = await _ticketManagementQueryBusiness.GetRequestCountsSLAViolationByService();
            }
            else if (type == "Priority")
            {
                result = await _ticketManagementQueryBusiness.GetRequestCountsSLAViolationByPriority();
            }
            else if (type == "Owner")
            {
                result = await _ticketManagementQueryBusiness.GetRequestCountsSLAViolationByOwner();
            }
            return result;
        }

        public async Task<List<HelpDeskViewModel>> GetChartData(string type)
        {
            List<HelpDeskViewModel> result = new();

            if (type == "thisweek")
            {
                result = await _ticketManagementQueryBusiness.GetChartDataThisMonth();
            }
            else if (type == "lastweek")
            {
                result = await _ticketManagementQueryBusiness.GetChartDataLastWeek();
            }
            else if (type == "thismonth")
            {
                result = await _ticketManagementQueryBusiness.GetChartDataThisMonth();
            }
            else if (type == "lastMonth")
            {
                result = await _ticketManagementQueryBusiness.GetChartDataLastMonth();
            }
            return result;
        }

        public async Task<List<HelpDeskViewModel>> GetPendingTaskCountWithAssignee()
        {
            var result = await _ticketManagementQueryBusiness.GetPendingTaskCountWithAssignee();
            return result;
        }
        
        public async Task<List<HelpDeskViewModel>> GetServiceSLAVoilationInLast20Days()
        {
            var result = await _ticketManagementQueryBusiness.GetServiceSLAVoilationInLast20Days();
            return result;
        } 
        
        
        public async Task<List<HelpDeskViewModel>> GetServiceSLAVoilationCompletedIn20Days()
        {
            var result = await _ticketManagementQueryBusiness.GetServiceSLAVoilationCompletedIn20Days();
            return result;
        }

        public async Task<List<HelpDeskViewModel>> GetRequestForServiceChart(string type)
        {
            var result = await _ticketManagementQueryBusiness.GetRequestForServiceChart(type);
            return result;
        }
        public async Task<List<HelpDeskViewModel>> GetOverDueRequest()
        {
            var result = await _ticketManagementQueryBusiness.GetOverDueRequest();
            return result;
        }
    }
}
