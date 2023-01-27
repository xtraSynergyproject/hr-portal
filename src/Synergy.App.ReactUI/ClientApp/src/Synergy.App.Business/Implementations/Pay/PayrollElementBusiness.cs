using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class PayrollElementBusiness : BusinessBase<NoteViewModel, NtsNote>, IPayrollElementBusiness
    {
        private readonly IRepositoryQueryBase<SalaryInfoViewModel> _salaryInfo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<PayrollBatchViewModel> _queryPayBatch;
        private readonly IRepositoryQueryBase<SalaryElementInfoViewModel> _salEleInfo;
        private readonly IServiceProvider _sp;
        private readonly IPayRollQueryBusiness _payRollQueryBusiness;

        public PayrollElementBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper,
            IRepositoryQueryBase<SalaryInfoViewModel> salaryInfo, IServiceProvider sp
            , IPayRollQueryBusiness payRollQueryBusiness
            , IRepositoryQueryBase<IdNameViewModel> queryRepo1, IRepositoryQueryBase<SalaryElementInfoViewModel> salEleInfo,
            IRepositoryQueryBase<NoteViewModel> queryRepo, IRepositoryQueryBase<PayrollBatchViewModel> queryPayBatch) : base(repo, autoMapper)
        {
            _salaryInfo = salaryInfo;
            _queryRepo1 = queryRepo1;
            _queryRepo = queryRepo;
            _queryPayBatch = queryPayBatch;
            _salEleInfo = salEleInfo;
            _sp = sp;
            _payRollQueryBusiness = payRollQueryBusiness;
        }

        public async Task CalculateSalaryElement(string personId, string salaryInfoId, double total)
        {
            //var cypher = string.Concat(@"
            //match (pr:HRS_PersonRoot {Id:{PersonId},IsDeleted:0})-[:R_PersonRoot_LegalEntity_OrganizationRoot]->(or:HRS_OrganizationRoot)         
            //match (or)<-[:R_LegalEntity_OrganizationRoot]-(le:ADM_LegalEntity)
            //return le");
            
            //var prms = new Dictionary<string, object>
            //{
            //    { "PersonId", personId }
            //};
            var salDetail = await _payRollQueryBusiness.GetPersonDetails(personId);

            var salarystartDate = DateTime.Today;
            var ab = _sp.GetService<IHRCoreBusiness>();
            var assignment = await ab.GetAssignmentByPerson(personId);
            if (assignment != null && assignment.DateOfJoin.IsNotNullAndNotEmpty())
            {
                salarystartDate = assignment.DateOfJoin.ToSafeDateTime();
            }

            if (salDetail != null)
            {
                var basicCode = "BASIC";
                var houseCode = "HOUSING";
                var transCode = "TRANSPORT";
                double totalSal = 0.0;

                if (salDetail.BasicSalaryPercentage != 0 && salDetail.HousingAllowancePercentage != 0 && salDetail.TransportAllowancePercentage != 0)
                {
                    var _tableMetadataBusiness = _sp.GetService<ITableMetadataBusiness>();
                    var _noteBusiness = _sp.GetService<INoteBusiness>();
                    var pay = await _tableMetadataBusiness.GetTableDataByColumn("PayrollElement", "", "ElementCode", houseCode);
                    var sal = await _tableMetadataBusiness.GetTableDataByColumn("SalaryInfo", "", "Id", salaryInfoId);
                    var houseSal = total * (salDetail.HousingAllowancePercentage / 100.00);
                    houseSal = Math.Round(houseSal);
                    if (pay.IsNotNull())
                    {                       
                        var housemodel = new SalaryElementInfoViewModel
                        {
                            ElementId = pay["Id"].ToString(),
                            SalaryInfoId = sal["NtsNoteId"].ToString(),
                            Amount = houseSal,
                            EffectiveStartDate = salarystartDate,
                            EffectiveEndDate = DateTime.MaxValue,
                        };
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Create;
                        noteTempModel.ActiveUserId = _repo.UserContext.UserId;
                        noteTempModel.TemplateCode = "SalaryElementInfo";
                        noteTempModel.ParentNoteId = sal["NtsNoteId"].ToString();
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);                       
                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(housemodel);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";                       
                        var result = await _noteBusiness.ManageNote(notemodel);

                    }
                    //var house = _elementBusiness.GetActiveSingle(x => x.Code == houseCode);                   
                    //Create(housemodel);

                    var trans = await _tableMetadataBusiness.GetTableDataByColumn("PayrollElement", "", "ElementCode", transCode);
                    // var trans = _elementBusiness.GetActiveSingle(x => x.Code == transCode);
                    var transSal = total * (salDetail.TransportAllowancePercentage / 100.00);
                    transSal = Math.Round(transSal);
                    if (trans.IsNotNull())
                    {                       
                        var transmodel = new SalaryElementInfoViewModel
                        {
                            ElementId = trans["Id"].ToString(),
                            SalaryInfoId = sal["NtsNoteId"].ToString(),
                            Amount = transSal,
                            EffectiveStartDate = salarystartDate,
                            EffectiveEndDate = DateTime.MaxValue,
                        };
                        // Create(transmodel);
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Create;
                        noteTempModel.ActiveUserId = _repo.UserContext.UserId;
                        noteTempModel.TemplateCode = "SalaryElementInfo";
                        noteTempModel.ParentNoteId = sal["NtsNoteId"].ToString();
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(transmodel);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        var result = await _noteBusiness.ManageNote(notemodel);
                    }
                    // var basic = _elementBusiness.GetActiveSingle(x => x.Code == basicCode);
                    var basic = await _tableMetadataBusiness.GetTableDataByColumn("PayrollElement", "", "ElementCode", basicCode);
                    // var trans = _elementBusiness.GetActiveSingle(x => x.Code == transCode);
                    if (basic.IsNotNull())
                    {
                        var basicSal = total * (salDetail.BasicSalaryPercentage / 100.00);
                        basicSal = Math.Round(basicSal);
                        totalSal = basicSal + houseSal + transSal;

                        var different = total - totalSal;

                        var basicmodel = new SalaryElementInfoViewModel
                        {
                            ElementId = basic["Id"].ToString(),
                            SalaryInfoId = sal["NtsNoteId"].ToString(),
                            Amount = basicSal + different,
                            EffectiveStartDate = salarystartDate,
                            EffectiveEndDate = DateTime.MaxValue,
                        };
                        // Create(basicmodel);
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Create;
                        noteTempModel.ActiveUserId = _repo.UserContext.UserId;
                        noteTempModel.TemplateCode = "SalaryElementInfo";
                        noteTempModel.ParentNoteId = sal["NtsNoteId"].ToString();
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(basicmodel);
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        var result = await _noteBusiness.ManageNote(notemodel);
                    }
                        
                }
            }
        }

        public async Task DeleteSalaryInfo(string id)
        {
            var _tableMetadataBusiness = _sp.GetService<ITableMetadataBusiness>();
            var _noteBusiness = _sp.GetService<INoteBusiness>();           
            var sal = await _tableMetadataBusiness.GetTableDataByColumn("SalaryInfo", "", "Id", id);
            if (sal!=null) 
            {
                await _payRollQueryBusiness.DeleteSalaryInfo(id);

                await _noteBusiness.Delete(sal["NtsNoteId"].ToString());
            }
        }

        public async Task<List<SalaryElementInfoViewModel>> GetAllSalaryElementInfo()
        {
            var list = await _payRollQueryBusiness.GetAllSalaryElementInfo();
            return list.ToList();
        }

        public async Task<List<IdNameViewModel>> GetAllUserSalary()
        {
            var totalSalary = await _payRollQueryBusiness.GetAllUserSalary();
            return totalSalary.ToList();
        }

        public async Task<double> GetBasicSalary(string userId, DateTime? asofDate = null)
        {
            asofDate = asofDate ?? DateTime.Today;
            //var cypher = string.Concat(@"match(u:ADM_User{Id:{UserId},IsDeleted: 0,Status:'Active'})-[:R_User_PersonRoot]->(pr:HRS_PersonRoot)
            //<-[:R_SalaryInfoRoot_PersonRoot]-(psr:PAY_SalaryInfoRoot)<-[:R_SalaryInfoRoot]-(si:PAY_SalaryInfo)
            //where si.EffectiveStartDate <= {ESD} <= si.EffectiveEndDate
            //match (psr)<-[:R_SalaryElementInfo_SalaryInfoRoot]-(ps:PAY_SalaryElementInfo{IsDeleted: 0,Status:'Active'})
            //where ps.EffectiveStartDate <= {ESD} <= ps.EffectiveEndDate
            //match (ps)-[:R_SalaryElementInfo_ElementRoot]->(pe:PAY_ElementRoot{IsDeleted: 0,Status:'Active'})
            //<-[:R_ElementRoot]-(e:PAY_Element{IsDeleted: 0,Status:'Active'})
            //where e.EffectiveStartDate <= {ESD} <= e.EffectiveEndDate and e.Code in['BASIC']
            //return toFloat(SUM(ps.Amount))");

            //var prms = new Dictionary<string, object>
            //{
            //    { "UserId", userId },
            //    { "ESD", asofDate },
            //};

            var basicSalary = await _payRollQueryBusiness.GetBasicSalary(userId, asofDate);
            return basicSalary;
        }

        public async Task<List<ElementViewModel>> GetElementListForPayrollRun(DateTime asofDate)
        {
            return await _payRollQueryBusiness.GetElementListForPayrollRun(asofDate);
        }
        public async Task<ElementViewModel> GetPayrollElementById(string Id)
        {
            return await _payRollQueryBusiness.GetPayrollElementById(Id);
        }
        public async Task<List<IdNameViewModel>> GetPayrollDeductionElement()
        {
            return await _payRollQueryBusiness.GetPayrollDeductionElement();
        }
        public async Task<SalaryInfoViewModel> GetEligiblityForEOS(string userId)
        {
            return await _payRollQueryBusiness.GetEligiblityForEOS(userId);
        }

        public async  Task<SalaryInfoViewModel> GetEligiblityForTickets(string userId)
        {
            return await _payRollQueryBusiness.GetEligiblityForTickets(userId);
        }

        public async Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoForPayrollRun(PayrollRunViewModel viewModel)
        {
            var list = await _payRollQueryBusiness.GetSalaryElementInfoForPayrollRun(viewModel);
            return list.ToList();
        }

        public async Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoListByNodeId(string nodeId)
        {
            return await _payRollQueryBusiness.GetSalaryElementInfoListByNodeId(nodeId);
        }

        public async Task<List<SalaryElementInfoViewModel>> GetSalaryElementInfoListByUser(string userId)
        {
            var asofDate = DateTime.Today;
            var result = await _payRollQueryBusiness.GetSalaryElementInfoListByUser(userId, asofDate);
            return result;
        }
        public async Task<SalaryElementInfoViewModel> GetSalaryElementInfoListByUserAndELement(string personId,string elementId)
        {
            var result = await _payRollQueryBusiness.GetSalaryElementInfoListByUserAndELement(personId, elementId);
            return result;
        }
        public async Task<double> GetUserOneDaySalary(string userId, DateTime? asofDate = null)
        {
            // throw new NotImplementedException();
            var res = await GetUserSalary(userId, asofDate ?? DateTime.Today);
            var list = res.PayrollDailyAmountAsPerWorkingDays();
            return list;
        }

   

        public async Task<double> GetUserSalary(string userId, DateTime? asofDate = null)
        {
            asofDate = asofDate ?? DateTime.Today;

            var totalSalary = await _payRollQueryBusiness.GetUserSalary(userId, asofDate);
            if (totalSalary!=null) 
            {
                return totalSalary.Amount;
            }
            return 0.0;
        }
    }
}
