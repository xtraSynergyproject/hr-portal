using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{

    public class OnlinePaymentViewModel
    {
        public string Id { get; set; }
        public string NtsId { get; set; }
        public NtsTypeEnum NtsType { get; set; }
        public string UdfTableId { get; set; }
        public double Amount { get; set; }
        public string PaymentStatusId { get; set; }
        public string PaymentStatusCode { get; set; }
        public string PaymentStatusName { get; set; }
        public string MerchantID { get; set; }
        // public string CustomerID { get; set; }
        public string CurrencyType { get; set; }
        public string SecurityID { get; set; }
        public string TypeField1 { get; set; }
        public string TypeField2 { get; set; }
        public string Filler1 { get; set; }
        public string AdditionalInfo1 { get; set; }
        public string AdditionalInfo2 { get; set; }
        public string AdditionalInfo3 { get; set; }
        public string AdditionalInfo4 { get; set; }
        public string AdditionalInfo5 { get; set; }
        public string AdditionalInfo6 { get; set; }
        public string AdditionalInfo7 { get; set; }
        public string ChecksumKey { get; set; }
        public string ChecksumValue { get; set; }
        public string PaymentGatewayUrl { get; set; }
        public string PaymentGatewayReturnUrl { get; set; }
        public string Message { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseChecksumValue { get; set; }
        public string RequestUrl { get; set; }
        public string ResponseUrl { get; set; }
        public string ReturnUrl { get; set; }
        public string UserId { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public string ResponseErrorCode { get; set; }
        public string PaymentReferenceNo { get; set; }
        public string ResponseError { get; set; }
        public string AuthStatus { get; set; }
        public string UserMessage { get; set; }

    }
}
