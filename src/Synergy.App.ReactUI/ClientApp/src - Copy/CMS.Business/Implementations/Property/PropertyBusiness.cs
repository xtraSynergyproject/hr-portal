using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;



namespace CMS.Business
{
    public class PropertyBusiness : BusinessBase<NoteViewModel, NtsNote>, IPropertyBusiness
    {
        private readonly IRepositoryQueryBase<PayPropertyTaxViewModel> _queryRepo1;
        private readonly IServiceBusiness _serviceBusiness;
        public PropertyBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, 
            IRepositoryQueryBase<PayPropertyTaxViewModel> queryRepo1, IMapper autoMapper,
            IServiceBusiness serviceBusiness) : base(repo, autoMapper)
        {
            _queryRepo1 = queryRepo1;
            _serviceBusiness = serviceBusiness;


        }
        public async Task<IList<PayPropertyTaxViewModel>> GetPropertySearch(string city, string propertyId, string ownerName, string oldPropertyId, string houseNo, string street, string mobile, string wardNo, string postalCode, string email)
        {


            string query = @$"select  n.""NoteNo"" as ""PropertyId"",p.""Id"",p.""OldPropertyId"" as ""OldPropertyId"",p.""Email"" as ""Email""
,lov.""Name"" as ""CityName"",lov1.""Name"" as ""WardNo"",p.""Mobile"" as ""Mobile""
, p.""PostalCode"" as ""PostalCode"",p.""Locality"" as ""Locality"",p.""HouseNo"" as ""HouseNo"",
p.""UIbCityCodeId"" as ""CityId"",p.""WardNoId"" as ""WardNoId""
,O.""FullName"" as ""OwnerName""
from  public.""NtsNote"" as N
 inner join cms.""N_PROPERTY_PropertyMaster"" as P on P.""NtsNoteId""=N.""Id"" 
left join cms.""N_PROPERTY_PropertyOwner"" as O on O.""Id""=p.""OwnerNameId""
join public.""LOV"" as lov on lov.""Id""=p.""UIbCityCodeId""
 join public.""LOV"" as lov1 on lov1.""Id""=p.""WardNoId""
#CityWhere# #PropertyIdWhere# #OldPropertyIdWhere# #EmailWhere# #MobileWhere# #WardNoWhere# #LocalityWhere# #OwnerNameWhere# #PostalCodeWhere# #HouseNoWhere#";

            var CityWhere = "";
            if (city.IsNotNull())
            {

                CityWhere = @$" Where p.""UIbCityCodeId""='{city}' ";
            }

            query = query.Replace("#CityWhere#", CityWhere);
            var PropertyIdWhere = "";
            if (propertyId.IsNotNull())
            {

                PropertyIdWhere = @$" and n.""NoteNo""='{propertyId}' ";
            }

            query = query.Replace("#PropertyIdWhere#", PropertyIdWhere);
            var OldPropertyIdWhere = "";
            if (oldPropertyId.IsNotNull())
            {

                OldPropertyIdWhere = @$" and p.""OldPropertyId""='{oldPropertyId}' ";
            }

            query = query.Replace("#OldPropertyIdWhere#", OldPropertyIdWhere);
            var EmailWhere = "";
            if (email.IsNotNull())
            {

                EmailWhere = @$" and p.""Email""='{email}' ";
            }

            query = query.Replace("#EmailWhere#", EmailWhere);
            var MobileWhere = "";
            if (mobile.IsNotNull())
            {

                MobileWhere = @$" and p.""Mobile""='{mobile}' ";
            }

            query = query.Replace("#MobileWhere#", MobileWhere);
            var WardNoWhere = "";
            if (wardNo.IsNotNull())
            {

                WardNoWhere = @$" and p.""WardNoId""='{wardNo}' ";
            }

            query = query.Replace("#WardNoWhere#", WardNoWhere);
            var LocalityWhere = "";
            if (street.IsNotNull())
            {

                LocalityWhere = @$" and p.""Locality""='{street}' ";
            }

            query = query.Replace("#LocalityWhere#", LocalityWhere);
            var OwnerNameWhere = "";
            if (ownerName.IsNotNull())
            {

                OwnerNameWhere = @$" and O.""FullName""='{ownerName}' ";
            }

            query = query.Replace("#OwnerNameWhere#", OwnerNameWhere);
            var PostalCodeWhere = "";
            if (postalCode.IsNotNull())
            {

                PostalCodeWhere = @$" and p.""PostalCode""='{postalCode}' ";
            }

            query = query.Replace("#PostalCodeWhere#", PostalCodeWhere);
            var HouseNoWhere = "";
            if (houseNo.IsNotNull())
            {

                HouseNoWhere = @$" and p.""HouseNo""='{houseNo}' ";
            }

            query = query.Replace("#HouseNoWhere#", HouseNoWhere);


            var result = await _queryRepo1.ExecuteQueryList<PayPropertyTaxViewModel>(query, null);


            //result = result.Where(x => x.CityId == city && x.WardNoId == wardNo && x.PropertyId== propertyId && x.OldPropertyId== oldPropertyId && x.OwnerName==ownerName && x.HouseNo==houseNo && x.PostalCode==postalCode && x.Email==email && x.Locality==street && x.Mobile==mobile).ToList();


            return result;
        }
        public async Task<PayPropertyTaxViewModel> GetPropertyTaxbyId(string PropertyId)
        {
            var query = @$"select  CONCAT(p.""FirstName"", ' ', p.""MiddleName"", ' ', p.""LastName"")  as ""OwnerName"",
P.""HouseNo"", wd.""Name"" as WardNo,p.""Locality"" as Locality,P.""Zone"" as Zone,
    cl.""Name"" as Colony,s.""ServiceNo"" as PropertyId,P.""PostalCode"" as PostalCode,P.""CIty"" as CityName,
	RZ.""Name"" as RateZone
    from  public.""NtsService"" as S inner join public.""NtsNote"" as N on N.""Id""=S.""UdfNoteId"" 
	inner join cms.""N_PROP_TAX_NEW_PROPERTY"" as P on P.""NtsNoteId""=N.""Id""
	left join public.""LOV"" as wd on p.""WardNoId""=wd.""Id""
	left join public.""LOV"" as cl on p.""Colony""=cl.""Id""
	left join public.""LOV"" as RZ on P.""RateZoneId""=RZ.""Id""
	where s.""ServiceNo""='{PropertyId}'";

            var result = await _queryRepo1.ExecuteQuerySingle<PayPropertyTaxViewModel>(query, null);
            return result;
        }

        public async Task<List<PropertyAreaDetailsViewModel>> GetCurrentYearSummary(string PropertyId)
        {

            var Query = $@"select  UT.""Name"" as UsageType,UF.""Name"" as UsageFactor,FN.""Name"" as FloorNo,TC.""Name"" as TypeOfConstruction,P.""areaSqFeet"" as Area
    from  public.""NtsService"" as S inner join public.""NtsNote"" as N on N.""Id""=S.""UdfNoteId"" 
	inner join cms.""N_PROP_TAX_NEW_PROPERTY"" as P on P.""NtsNoteId""=N.""Id""
	left join public.""LOV"" as UT on p.""UsageTypeId""=UT.""Id""
	left join public.""LOV"" as UF on p.""UsageFactorId""=UF.""Id""
	left join public.""LOV"" as FN on P.""floorNo""=FN.""Id""
	left join public.""LOV"" as TC on P.""TypeOfConstructionId""=TC.""Id""
	where s.""ServiceNo""='{PropertyId}'";

            var result = await _queryRepo1.ExecuteQueryList<PropertyAreaDetailsViewModel>(Query, null);
            return result;
        }

        public async Task<PayPropertyTaxViewModel> GetNDCDetails(string serviceId)
        {
            var query = $@"select s.""Id"" as PropertyServiceId ,ndc.""Ubl"" as UlbIdDuc, ndc.""PropertyId"", CONCAT(p.""FirstName"",' ',p.""MiddleName"",' ',p.""LastName"") as OwnerName,col.""Name"" as Colony,
p.""CIty"" as CityName,p.""Mobile"",p.""EmailId"" as Email
from public.""NtsService"" as s
join public.""NtsNote"" as n on s.""UdfNoteId""=n.""Id"" and n.""IsDeleted""=false
join cms.""N_PROP_TAX_NoDuesCertificateRequest"" as ndc on n.""Id""=ndc.""NtsNoteId"" and ndc.""IsDeleted"" = false
left join public.""NtsService"" as ps on ndc.""PropertyId""=ps.""ServiceNo"" and ps.""IsDeleted""=false
left join cms.""N_PROP_TAX_NEW_PROPERTY"" as p on ps.""UdfNoteTableId""=p.""Id"" and p.""IsDeleted""=false
left join public.""LOV"" as col on p.""Colony""=col.""Id"" and col.""IsDeleted""=false
where s.""Id""='{serviceId}' and s.""IsDeleted""=false ";

            var result = await _queryRepo1.ExecuteQuerySingle(query,null);
            return result;
        }

        public async Task UpdateOTP(string serviceId, string OTP, DateTime ExpiryDate)
        {
            var serviceModel = await _serviceBusiness.GetSingleById(serviceId);

            var query = $@"Update cms.""N_PROP_TAX_NoDuesCertificateRequest"" set ""Otp""={OTP}, ""OtpExpiryDate""='{ExpiryDate}'
                            where ""Id""='{serviceModel.UdfNoteTableId}' ";

            await _queryRepo1.ExecuteCommand(query, null);

        }

        public async Task<PayPropertyTaxViewModel> ValidateOTP(string serviceId, string curOTP)
        {
            var serviceModel = await _serviceBusiness.GetSingleById(serviceId);

            var query = $@"Select * from cms.""N_PROP_TAX_NoDuesCertificateRequest"" where ""Id""='{serviceModel.UdfNoteTableId}' ";

            var result = await _queryRepo1.ExecuteQuerySingle(query, null);

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
