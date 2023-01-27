using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;



namespace Synergy.App.Business
{
    public class PropertyBusiness : BusinessBase<NoteViewModel, NtsNote>, IPropertyBusiness
    {
        private readonly IRepositoryQueryBase<PayPropertyTaxViewModel> _queryRepo1;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IPropertyQueryBusiness _propertyQueryBusiness;
        public PropertyBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, 
            IRepositoryQueryBase<PayPropertyTaxViewModel> queryRepo1, IMapper autoMapper,
            IPropertyQueryBusiness propertyQueryBusiness,
            IServiceBusiness serviceBusiness) : base(repo, autoMapper)
        {
            _queryRepo1 = queryRepo1;
            _serviceBusiness = serviceBusiness;
            _propertyQueryBusiness= propertyQueryBusiness;


        }
        public async Task<IList<PayPropertyTaxViewModel>> GetPropertySearch(string city, string propertyId, string ownerName, string oldPropertyId, string houseNo, string street, string mobile, string wardNo, string postalCode, string email)
        {


            var result = await _propertyQueryBusiness.GetPropertySearch(city, propertyId, ownerName, oldPropertyId, houseNo, street, mobile, wardNo, postalCode, email);


            //result = result.Where(x => x.CityId == city && x.WardNoId == wardNo && x.PropertyId== propertyId && x.OldPropertyId== oldPropertyId && x.OwnerName==ownerName && x.HouseNo==houseNo && x.PostalCode==postalCode && x.Email==email && x.Locality==street && x.Mobile==mobile).ToList();


            return result;
        }
        public async Task<PayPropertyTaxViewModel> GetPropertyTaxbyId(string PropertyId)
        {
            var result = await _propertyQueryBusiness.GetPropertyTaxbyId(PropertyId);
            return result;
        }

        public async Task<List<PropertyAreaDetailsViewModel>> GetCurrentYearSummary(string PropertyId)
        {

            var result = await _propertyQueryBusiness.GetCurrentYearSummary(PropertyId);
            return result;
        }

        public async Task<PayPropertyTaxViewModel> GetNDCDetails(string serviceId)
        {

            var result = await _propertyQueryBusiness.GetNDCDetails(serviceId);
            return result;
        }

        public async Task UpdateOTP(string serviceId, string OTP, DateTime ExpiryDate)
        {
            var serviceModel = await _serviceBusiness.GetSingleById(serviceId);

            await _propertyQueryBusiness.UpdateOTP(serviceId, OTP, ExpiryDate, serviceModel.UdfNoteTableId);
                

        }

        public async Task<PayPropertyTaxViewModel> ValidateOTP(string serviceId, string curOTP)
        {
            var serviceModel = await _serviceBusiness.GetSingleById(serviceId);

            //var query = $@"Select * from cms.""N_PROP_TAX_NoDuesCertificateRequest"" where ""Id""='{serviceModel.UdfNoteTableId}' ";

            var result = await _propertyQueryBusiness.ValidateOTP(serviceId, curOTP, serviceModel.UdfNoteTableId);

            return result;

        }

        public List<PayPropertyTaxViewModel> GetYearWiseBreakUp()
        {
            string[] Year = new string[3];
            Year[0] = "2019-2020";
            Year[1] = "2020-2021";
            Year[2] = "2021-2022";

            List<PayPropertyTaxViewModel> model = new List<PayPropertyTaxViewModel>();
            PayPropertyTaxViewModel item;

            for (int i = 0; i <Year.Length; i++)
            {
                item = new PayPropertyTaxViewModel();
                item.AnnualLettingvalue = 7200.00;
                item.NetALV = 6480.00;
                item.Year = Year[i];
                item.RateZone = "03";
                item.PropertyTax = 454.00;
                item.EducationCess = 216.00;
                item.UrbanDev = 72.00;
                item.SamekitKar = 150.00;
                item.SewaKar = 0.00;
                item.Total = 892;
                item.Rebate = 0.00;
                item.Penalty = 89.00;
                item.TotalABC = 961;
                model.Add(item);
            }

            
                
            return model;

        }
    }
}
