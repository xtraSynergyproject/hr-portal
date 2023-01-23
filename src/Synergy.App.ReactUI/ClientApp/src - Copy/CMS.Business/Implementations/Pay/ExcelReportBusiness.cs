using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using CMS.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetLight;
using System.IO;
using CMS.Common.Utilities;
using System.Drawing;

namespace CMS.Business
{
    public class ExcelReportBusiness : BusinessBase<NoteViewModel, NtsNote>,IExcelReportBusiness
    {
        ILegalEntityBusiness _legalEntityBusiness;
        private readonly IUserContext _userContext;
        IPayrollElementBusiness _PayrollelementBusiness;
        IHRCoreBusiness _HrCoreBusiness;
     
        public ExcelReportBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper, ILegalEntityBusiness legalEntityBusiness, IUserContext userContext, IPayrollElementBusiness PayrollelementBusiness, IHRCoreBusiness HrCoreBusiness) : base(repo, autoMapper)
        {
            _legalEntityBusiness = legalEntityBusiness;
            _userContext = userContext;
            _PayrollelementBusiness = PayrollelementBusiness;
            _HrCoreBusiness = HrCoreBusiness;
        }

        public async Task<MemoryStream> GetFlightAccrualDetails(List<PayrollReportViewModel> list)
        {

            var personsList = list.Select(x => x.SponsorshipNo).Distinct().ToList();
            var ms = new MemoryStream();
            var code = _userContext.LegalEntityCode;
            var le =await _legalEntityBusiness.GetSingle(x => x.Code == code);
           


           // var code = "";
            //var le = _legalEntityBusiness.GetSingle(x => x.Code =="");
            var leName = "CAYAN";
            if (le != null)
            {
                //leName = le.Name;
            }
            using (var sl = new SLDocument())
            {

                sl.AddWorksheet("EmpBenefitPlansBal");

                SLPageSettings pageSettings = new SLPageSettings();
                pageSettings.ShowGridLines = false;
                sl.SetPageSettings(pageSettings);


                sl.SetColumnWidth("A", 20);
                sl.SetColumnWidth("B", "G", 15);
                //sl.SetColumnWidth("C", 20);
                //sl.SetColumnWidth("D", 20);
                //sl.SetColumnWidth("E", 15);
                //sl.SetColumnWidth("F", 15);
                //sl.SetColumnWidth("G", 15);
                sl.SetRowHeight(4, 35);

                sl.SetCellStyle(string.Concat("A5"), string.Concat("G5"), ExcelHelper.GetRedTopBorderStyle(sl));
                sl.MergeWorksheetCells("A1", "E1");
                // sl.SetCellValue("A1", le.Name);
                sl.SetCellStyle("A1", "E1", ExcelHelper.GetHeaderRowCayanLabelStyle(sl));

                sl.SetCellValue("G1", DateTime.Today.ToDefaultDateFormat());
                sl.SetCellStyle("G1", ExcelHelper.GetHeaderRowDateStyle(sl));

                sl.MergeWorksheetCells("A2", "G2");
                sl.SetCellValue("A2", "Employee Benefits - Flight Ticket Accrual");
                sl.SetCellStyle("A2", "G2", ExcelHelper.GetFlightReportHeadingStyle(sl));

                sl.MergeWorksheetCells("A3", "G3");
                sl.SetCellValue("A3", "");
                sl.SetCellStyle("A3", "G3", ExcelHelper.GetFlightReportSubHeadingStyle(sl));

                //sl.MergeWorksheetCells("A4", "C17");
                sl.SetCellValue("A4", "Plan");
                sl.SetCellStyle("A4", ExcelHelper.GetFlightColumnHeaderStyle(sl));
                sl.SetCellValue("B4", "Option");
                sl.SetCellStyle("B4", ExcelHelper.GetFlightColumnHeaderStyle(sl));
                sl.SetCellValue("C4", "Enroll Date");
                sl.SetCellStyle("C4", ExcelHelper.GetFlightColumnHeaderStyle(sl));
                sl.SetCellValue("D4", "Opening Balance");
                sl.SetCellStyle("D4", ExcelHelper.GetFlightColumnHeaderStyle(sl));
                sl.SetCellValue("E4", "Debit");
                sl.SetCellStyle("E4", ExcelHelper.GetFlightColumnHeaderStyle(sl));
                sl.SetCellValue("F4", "Credit");
                sl.SetCellStyle("F4", ExcelHelper.GetFlightColumnHeaderStyle(sl));
                sl.SetCellValue("G4", "Closing Balance");
                sl.SetCellStyle("G4", ExcelHelper.GetFlightColumnHeaderStyle(sl));


                int i = 5;

                foreach (var _person in personsList)
                {
                    var selectedPersons = list.Where(x => x.SponsorshipNo == _person).ToList();
                    var firstRow = selectedPersons.FirstOrDefault();

                    var personNo = firstRow.SponsorshipNo ?? firstRow.PersonNo;

                    sl.MergeWorksheetCells(string.Concat("A", i), string.Concat("G", i));
                    sl.SetCellValue(string.Concat("A", i), firstRow.PersonNo + "-" + firstRow.PersonName);
                    sl.SetCellStyle(string.Concat("A", i), string.Concat("G", i), ExcelHelper.GetFlightNameStyle(sl));
                    //sl.SetCellStyle(string.Concat("A", i), string.Concat("G", i), ExcelHelper.SetFontColor(sl, Color.Red, true));

                    i++;

                    sl.MergeWorksheetCells(string.Concat("A", i), string.Concat("G", i));
                    sl.SetCellValue(string.Concat("A", i), le.CurrencyName + " (" + le.CurrencySymbol + ")");
                    sl.SetCellStyle(string.Concat("A", i), string.Concat("G", i), ExcelHelper.GetFlightSARTopStyle(sl));
                    //sl.SetCellStyle(string.Concat("A", i), string.Concat("G", i), ExcelHelper.SetFontColor(sl, Color.DarkBlue, true));

                    i++;

                    sl.SetCellValue(string.Concat("A", i), "Flight Ticket");
                    sl.SetCellStyle(string.Concat("A", i), ExcelHelper.GetFlightOptionStyle(sl));

                    //var persondep = list.DistinctBy(e => e.PersonNo).ToList();
                    //var persondep = list.GroupBy(e => e.PersonNo).ToList();

                    foreach (PayrollReportViewModel model in selectedPersons)
                    {
                        if (model.ElementCode == "MONTHLY_SELF_TICKET_ACCRUAL")
                        {
                            sl.SetCellValue(string.Concat("B", i), "Employee");
                        }
                        else if (model.ElementCode == "MONTHLY_DEPENDENT_INFANT_TICKET_ACCRUAL")
                        {
                            sl.SetCellValue(string.Concat("B", i), "Infant");

                        }
                        else if (model.ElementCode == "MONTHLY_DEPENDENT_CHILD_TICKET_ACCRUAL")
                        {
                            sl.SetCellValue(string.Concat("B", i), "Child");

                        }
                        else if (model.ElementCode == "MONTHLY_DEPENDENT_ADULT_TICKET_ACCRUAL")
                        {
                            sl.SetCellValue(string.Concat("B", i), "Adult");

                        }
                        else if (model.ElementCode == "MONTHLY_WIFE_TICKET_ACCRUAL")
                        {
                            sl.SetCellValue(string.Concat("B", i), "Wife");

                        }
                        else if (model.ElementCode == "MONTHLY_HUSBAND_TICKET_ACCRUAL")
                        {
                            sl.SetCellValue(string.Concat("B", i), "Husband");

                        }

                        sl.SetCellStyle(string.Concat("B", i), ExcelHelper.GetFlightOptionStyle(sl));
                        sl.SetCellValue(string.Concat("C", i), model.EnrollDate.ToDefaultDateFormat());
                        sl.SetCellStyle(string.Concat("C", i), ExcelHelper.GetFlightOptionStyle(sl));

                        sl.SetCellValue(string.Concat("D", i), model.OpeningBalance.Value);
                        sl.SetCellStyle(string.Concat("D", i), ExcelHelper.GetFlightOptionAmountStyle(sl));
                        sl.SetCellStyle(string.Concat("D", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("E", i), model.EarningAmount);
                        sl.SetCellStyle(string.Concat("E", i), ExcelHelper.GetFlightOptionAmountStyle(sl));
                        sl.SetCellStyle(string.Concat("E", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("F", i), model.DeductionAmount);
                        sl.SetCellStyle(string.Concat("F", i), ExcelHelper.GetFlightOptionAmountStyle(sl));
                        sl.SetCellStyle(string.Concat("F", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("G", i), model.ClosingBalance.Value);
                        sl.SetCellStyle(string.Concat("G", i), ExcelHelper.GetFlightOptionAmountStyle(sl));
                        sl.SetCellStyle(string.Concat("G", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        i++;
                    }


                    sl.MergeWorksheetCells(string.Concat("A", i), string.Concat("C", i));
                    sl.SetCellValue(string.Concat("A", i), le.CurrencyName + " (" + le.CurrencySymbol + ")");
                    sl.SetCellStyle(string.Concat("A", i), ExcelHelper.GetFlightSARBottomStyle(sl));
                    //sl.SetCellStyle(string.Concat("A", i), ExcelHelper.SetFontColor(sl, Color.DarkBlue, true));



                    #region for sum of value
                    var _openingBalance = list.Where(x => x.PersonNo == firstRow.PersonNo).ToList();
                    var _openingBalanceSum = _openingBalance.Sum(x => x.OpeningBalance);
                    var _earningAmountSum = _openingBalance.Sum(x => x.EarningAmount);
                    var _deductionAmountSum = _openingBalance.Sum(x => x.DeductionAmount);
                    var _closingBalanceSum = _openingBalance.Sum(x => x.ClosingBalance);


                    sl.SetCellStyle(string.Concat("D", i), ExcelHelper.GetFlightSARTotalStyle(sl));
                    sl.SetCellValue(string.Concat("D", i), _openingBalanceSum.Value);
                    sl.SetCellStyle(string.Concat("D", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));
                    //sl.SetCellStyle(string.Concat("D", i), ExcelHelper.SetFontColor(sl, Color.DarkBlue, true));


                    sl.SetCellStyle(string.Concat("E", i), ExcelHelper.GetFlightSARTotalStyle(sl));
                    sl.SetCellValue(string.Concat("E", i), _earningAmountSum);
                    sl.SetCellStyle(string.Concat("E", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));
                    //sl.SetCellStyle(string.Concat("E", i), ExcelHelper.SetFontColor(sl, Color.DarkBlue, true));


                    sl.SetCellStyle(string.Concat("F", i), ExcelHelper.GetFlightSARTotalStyle(sl));
                    sl.SetCellValue(string.Concat("F", i), _deductionAmountSum);
                    sl.SetCellStyle(string.Concat("F", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));
                    //sl.SetCellStyle(string.Concat("F", i), ExcelHelper.SetFontColor(sl, Color.DarkBlue, true));


                    sl.SetCellStyle(string.Concat("G", i), ExcelHelper.GetFlightSARTotalStyle(sl));
                    sl.SetCellValue(string.Concat("G", i), _closingBalanceSum.Value);
                    sl.SetCellStyle(string.Concat("G", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));
                    //sl.SetCellStyle(string.Concat("G", i), ExcelHelper.SetFontColor(sl, Color.DarkBlue, true));

                    #endregion
                    i++;

                }


                sl.SetCellValue(string.Concat("A", i), "Grand Total:");
                sl.SetCellStyle(string.Concat("A", i), string.Concat("G", i), ExcelHelper.GetFlightGrandTotalStyle(sl));
                //sl.SetCellStyle(string.Concat("A", i), string.Concat("G", i), ExcelHelper.GeBottomBorderStyle(sl));
                //sl.SetCellStyle(string.Concat("A",i), string.Concat("G", i), ExcelHelper.FillColorStyle(sl));
                //sl.SetCellStyle(string.Concat("A", i), string.Concat("G", i), ExcelHelper.GetItalicFontStyle(sl));
                sl.SetCellValue(string.Concat("C", i), (personsList.Count()));
                sl.SetCellStyle(string.Concat("C", i), ExcelHelper.GetFlightCountStyle(sl));

                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);


            }
            ms.Position = 0;
            return ms;
        }

        public async Task<MemoryStream> GetEndOfServiceAccrualDetails(List<PayrollReportViewModel> list)
        {
            //var personsList = list.Select(x => x.SponsorshipNo).Distinct().ToList();
            var personsList = list.DistinctBy(x => x.SponsorshipNo).ToList();
            var ms = new MemoryStream();
            var code = _userContext.LegalEntityCode;
            var le =await _legalEntityBusiness.GetSingle(x => x.Code == code);
            var leName = "CAYAN";
            if (le != null)
            {
                leName = le.Name;
            }
            using (var sl = new SLDocument())
            {
                sl.AddWorksheet("EmpBenefitsPlan");
                foreach (var _person in personsList)
                {
                    var selectedPersons = list.Where(x => x.SponsorshipNo == _person.SponsorshipNo).ToList();
                    var firstRow = selectedPersons.FirstOrDefault();

                    var personNo = firstRow.SponsorshipNo ?? firstRow.PersonNo;


                    SLPageSettings pageSettings = new SLPageSettings();
                    pageSettings.ShowGridLines = false;
                    sl.SetPageSettings(pageSettings);

                    //sl.FreezePanes(2, 0);
                    // sl.SetColumnWidth(2, 15);
                    sl.SetColumnWidth("A", 16);
                    sl.SetColumnWidth("B", 22);
                    sl.SetColumnWidth("C", "H", 15);
                    //sl.SetColumnWidth("A", "B", 4.5);
                    //sl.SetColumnWidth("B", "C", 2);
                    //sl.SetColumnWidth("C", "D", 2);
                    //sl.SetColumnWidth("D", "E", 2);
                    //sl.SetColumnWidth("E", "F", 2);
                    //sl.SetColumnWidth("F", "G", 2);

                    // sl.SetCellStyle("A1", "L1", ExcelHelper.GetTopBorderStyle(sl));
                    //sl.SetCellStyle("A1", "A36", ExcelHelper.GetLeftBorderStyle(sl));
                    // sl.SetCellStyle("A37", "AI37", ExcelHelper.GetTopBorderStyle(sl));
                    // sl.SetCellStyle("AJ1", "AJ36", ExcelHelper.GetLeftBorderStyle(sl));

                    //sl.MergeWorksheetCells("A1", "AI14");
                    //sl.SetCellValue("A1", "Cayan");
                    //sl.SetCellStyle("A1", "AI14", ExcelHelper.GetHeaderRowEmployeeDataStyle(sl));

                    sl.SetCellStyle(string.Concat("A5"), string.Concat("H5"), ExcelHelper.GetRedTopBorderStyle(sl));
                    sl.MergeWorksheetCells("A1", "E1");
                    //sl.SetCellValue("A1", le.Name);
                    sl.SetCellStyle("A1", "E1", ExcelHelper.GetHeaderRowEmployeeDataLabelStyle(sl));



                    //  sl.MergeWorksheetCells("F1", "G1");
                    sl.SetCellValue("G1", DateTime.Today.ToDefaultDateFormat());
                    sl.SetCellStyle("G1", ExcelHelper.GetHeaderRowDateStyle(sl));

                    sl.MergeWorksheetCells("A2", "G3");
                    sl.SetCellValue("A2", "Employee Benefits - End Of Service");
                    sl.SetCellStyle("A2", "G3", ExcelHelper.GetReportHeadingStyle(sl));


                    //sl.MergeWorksheetCells("A4", "C17");
                    sl.SetCellValue("A4", "Plan");
                    sl.SetCellStyle("A4", ExcelHelper.GetShiftEntryDateStyle(sl));
                    sl.SetCellValue("B4", "Option");
                    sl.SetCellStyle("B4", ExcelHelper.GetShiftEntryDateStyle(sl));
                    sl.SetCellValue("C4", "Hiring Date");
                    sl.SetCellStyle("C4", ExcelHelper.GetShiftEntryDateStyle(sl));
                    sl.SetCellValue("D4", "Opening Balance");
                    sl.SetCellStyle("D4", ExcelHelper.GetShiftEntryDateStyle(sl));
                    sl.SetCellValue("E4", "Debit");
                    sl.SetCellStyle("E4", ExcelHelper.GetShiftEntryDateStyle(sl));
                    sl.SetCellValue("F4", "Credit");
                    sl.SetCellStyle("F4", ExcelHelper.GetShiftEntryDateStyle(sl));
                    sl.SetCellValue("G4", "Closing Balance");
                    sl.SetCellStyle("G4", ExcelHelper.GetShiftEntryDateStyle(sl));
                    sl.SetCellValue("H4", "Total Unpaid Leave Days");
                    sl.SetCellStyle("H4", ExcelHelper.GetShiftEntryDateStyle(sl));
                    int i = 5;
                    int j = 0;
                    foreach (PayrollReportViewModel model in personsList)
                    {


                        sl.MergeWorksheetCells(string.Concat("A", i), string.Concat("G", i));
                        sl.SetCellValue(string.Concat("A", i), model.PersonNo + "-" + model.PersonName);
                        sl.SetCellStyle(string.Concat("A", i), string.Concat("G", i), ExcelHelper.GetHeaderRowEmployeeDataLabelStyle(sl));
                        sl.SetCellStyle(string.Concat("A", i), string.Concat("G", i), ExcelHelper.SetFontColor(sl, Color.Red, true));

                        sl.MergeWorksheetCells(string.Concat("A", i + 1), string.Concat("G", i + 1));
                        sl.SetCellValue(string.Concat("A", i + 1), le.CurrencyName + "- (" + le.CurrencySymbol + ")");
                        sl.SetCellStyle(string.Concat("A", i + 1), string.Concat("G", i + 1), ExcelHelper.GetHeaderRowEmployeeDataLabelStyle(sl));
                        sl.SetCellStyle(string.Concat("A", i + 1), string.Concat("G", i + 1), ExcelHelper.SetFontColor(sl, Color.DarkBlue, true));


                        sl.SetCellValue(string.Concat("A", i + 2), "End of Service");
                        sl.SetCellStyle(string.Concat("A", i + 2), ExcelHelper.GetNoBorderStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("A", i + 3), string.Concat("C", i + 3));
                        sl.SetCellValue(string.Concat("A", i + 3), le.CurrencyName + "- (" + le.CurrencySymbol + ")");
                        sl.SetCellStyle(string.Concat("A", i + 3), ExcelHelper.GetHeaderRowEOSDataLabelStyle(sl));
                        sl.SetCellStyle(string.Concat("A", i + 3), ExcelHelper.SetFontColor(sl, Color.DarkBlue, true));

                        sl.SetCellValue(string.Concat("B", i + 2), le.Name);
                        sl.SetCellStyle(string.Concat("B", i + 2), ExcelHelper.GetNoBorderCentreAlignStyle(sl));

                        //sl.SetCellValue(string.Concat("C", i + 2), model.EnrollDate.ToDayAndDateFormat());
                        sl.SetCellValue(string.Concat("C", i + 2), model.EnrollDate.ToDefaultDateFormat());
                        sl.SetCellStyle(string.Concat("C", i + 2), ExcelHelper.GetNoBorderCentreAlignStyle(sl));

                        sl.SetCellValue(string.Concat("D", i + 2), model.OpeningBalance.Value);
                        sl.SetCellStyle(string.Concat("D", i + 2), ExcelHelper.GetBottomBorderStyle(sl));
                        sl.SetCellStyle(string.Concat("D", i + 2), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));


                        sl.SetCellValue(string.Concat("E", i + 2), model.EarningAmount);
                        sl.SetCellStyle(string.Concat("E", i + 2), ExcelHelper.GetBottomBorderStyle(sl));
                        sl.SetCellStyle(string.Concat("E", i + 2), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("F", i + 2), model.DeductionAmount);
                        sl.SetCellStyle(string.Concat("F", i + 2), ExcelHelper.GetBottomBorderStyle(sl));
                        sl.SetCellStyle(string.Concat("F", i + 2), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("G", i + 2), model.ClosingBalance.Value);
                        sl.SetCellStyle(string.Concat("G", i + 2), ExcelHelper.GetBottomBorderStyle(sl));
                        sl.SetCellStyle(string.Concat("G", i + 2), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("H", i + 2), model.TotalUnpaidLeaveDays.IsNotNull() ? model.TotalUnpaidLeaveDays.Value : 0);
                        sl.SetCellStyle(string.Concat("H", i + 2), ExcelHelper.GetBottomBorderStyle(sl));
                        sl.SetCellStyle(string.Concat("H", i + 2), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("D", i + 3), model.OpeningBalance.Value);
                        sl.SetCellStyle(string.Concat("D", i + 3), ExcelHelper.GetNoBorderRightAlignStyleWithBold(sl));
                        sl.SetCellStyle(string.Concat("D", i + 3), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("E", i + 3), model.EarningAmount);
                        sl.SetCellStyle(string.Concat("E", i + 3), ExcelHelper.GetNoBorderRightAlignStyleWithBold(sl));
                        sl.SetCellStyle(string.Concat("E", i + 3), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("F", i + 3), model.DeductionAmount);
                        sl.SetCellStyle(string.Concat("F", i + 3), ExcelHelper.GetNoBorderRightAlignStyleWithBold(sl));
                        sl.SetCellStyle(string.Concat("F", i + 3), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("G", i + 3), model.ClosingBalance.Value);
                        sl.SetCellStyle(string.Concat("G", i + 3), ExcelHelper.GetNoBorderRightAlignStyleWithBold(sl));
                        sl.SetCellStyle(string.Concat("G", i + 3), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("H", i + 3), model.TotalUnpaidLeaveDays.IsNotNull() ? model.TotalUnpaidLeaveDays.Value : 0);
                        sl.SetCellStyle(string.Concat("H", i + 3), ExcelHelper.GetNoBorderRightAlignStyleWithBold(sl));
                        sl.SetCellStyle(string.Concat("H", i + 3), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));
                        i = i + 5;
                        j = i;
                    }
                    //sl.MergeWorksheetCells(string.Concat("A", j), string.Concat("C", j));
                    sl.SetCellValue(string.Concat("A", j - 1), "Grand Total:");
                    sl.SetCellStyle(string.Concat("A", j - 1), string.Concat("H", j - 1), ExcelHelper.GetGrandTotalStyle(sl));
                    sl.SetCellStyle(string.Concat("A", j - 1), string.Concat("H", j - 1), ExcelHelper.GeBottomBorderStyle(sl));
                    sl.SetCellStyle(string.Concat("A", j - 1), string.Concat("H", j - 1), ExcelHelper.FillColorStyle(sl));
                    sl.SetCellStyle(string.Concat("A", j - 1), string.Concat("H", j - 1), ExcelHelper.GetItalicFontStyle(sl));
                    sl.SetCellValue(string.Concat("C", j - 1), ((i - 5) / 5));
                }

                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);

            }

            ms.Position = 0;
            return ms;
        }
        public async Task<MemoryStream> GetLoanAccrualDetails(List<PayrollReportViewModel> list)
        {
            //var personsList = list.Select(x => x.SponsorshipNo).Distinct().ToList();
            var personsList = list.DistinctBy(x => x.SponsorshipNo).ToList();
            var ms = new MemoryStream();
            var code = _userContext.LegalEntityCode;
            var le = await _legalEntityBusiness.GetSingle(x => x.Code == code);
            var leName = "CAYAN";
            if (le != null)
            {
                leName = le.Name;
            }
            using (var sl = new SLDocument())
            {
                sl.AddWorksheet("EmpBenefitsPlan");
                foreach (var _person in personsList)
                {
                    var selectedPersons = list.Where(x => x.SponsorshipNo == _person.SponsorshipNo).ToList();
                    var firstRow = selectedPersons.FirstOrDefault();

                    var personNo = firstRow.SponsorshipNo ?? firstRow.PersonNo;


                    SLPageSettings pageSettings = new SLPageSettings();
                    pageSettings.ShowGridLines = false;
                    sl.SetPageSettings(pageSettings);

                    //sl.FreezePanes(2, 0);
                    // sl.SetColumnWidth(2, 15);
                    sl.SetColumnWidth("A", 16);
                    sl.SetColumnWidth("B", 22);
                    sl.SetColumnWidth("C", "G", 15);
                    //sl.SetColumnWidth("A", "B", 4.5);
                    //sl.SetColumnWidth("B", "C", 2);
                    //sl.SetColumnWidth("C", "D", 2);
                    //sl.SetColumnWidth("D", "E", 2);
                    //sl.SetColumnWidth("E", "F", 2);
                    //sl.SetColumnWidth("F", "G", 2);

                    // sl.SetCellStyle("A1", "L1", ExcelHelper.GetTopBorderStyle(sl));
                    //sl.SetCellStyle("A1", "A36", ExcelHelper.GetLeftBorderStyle(sl));
                    // sl.SetCellStyle("A37", "AI37", ExcelHelper.GetTopBorderStyle(sl));
                    // sl.SetCellStyle("AJ1", "AJ36", ExcelHelper.GetLeftBorderStyle(sl));

                    //sl.MergeWorksheetCells("A1", "AI14");
                    //sl.SetCellValue("A1", "Cayan");
                    //sl.SetCellStyle("A1", "AI14", ExcelHelper.GetHeaderRowEmployeeDataStyle(sl));

                    sl.SetCellStyle(string.Concat("A5"), string.Concat("G5"), ExcelHelper.GetRedTopBorderStyle(sl));
                    sl.MergeWorksheetCells("A1", "E1");
                    //sl.SetCellValue("A1", le.Name);
                    sl.SetCellStyle("A1", "E1", ExcelHelper.GetHeaderRowEmployeeDataLabelStyle(sl));



                    //  sl.MergeWorksheetCells("F1", "G1");
                    sl.SetCellValue("G1", DateTime.Today.ToDefaultDateFormat());
                    sl.SetCellStyle("G1", ExcelHelper.GetHeaderRowDateStyle(sl));

                    sl.MergeWorksheetCells("A2", "G3");
                    sl.SetCellValue("A2", "Employee Benefits - Loan");
                    sl.SetCellStyle("A2", "G3", ExcelHelper.GetReportHeadingStyle(sl));


                    //sl.MergeWorksheetCells("A4", "C17");
                    sl.SetCellValue("A4", "Plan");
                    sl.SetCellStyle("A4", ExcelHelper.GetShiftEntryDateStyle(sl));
                    sl.SetCellValue("B4", "Option");
                    sl.SetCellStyle("B4", ExcelHelper.GetShiftEntryDateStyle(sl));
                    sl.SetCellValue("C4", "Enroll Date");
                    sl.SetCellStyle("C4", ExcelHelper.GetShiftEntryDateStyle(sl));
                    sl.SetCellValue("D4", "Opening Balance");
                    sl.SetCellStyle("D4", ExcelHelper.GetShiftEntryDateStyle(sl));
                    sl.SetCellValue("E4", "Debit");
                    sl.SetCellStyle("E4", ExcelHelper.GetShiftEntryDateStyle(sl));
                    sl.SetCellValue("F4", "Credit");
                    sl.SetCellStyle("F4", ExcelHelper.GetShiftEntryDateStyle(sl));
                    sl.SetCellValue("G4", "Closing Balance");
                    sl.SetCellStyle("G4", ExcelHelper.GetShiftEntryDateStyle(sl));

                    int i = 5;
                    int j = 0;
                    foreach (PayrollReportViewModel model in personsList)
                    {


                        sl.MergeWorksheetCells(string.Concat("A", i), string.Concat("G", i));
                        sl.SetCellValue(string.Concat("A", i), model.PersonNo + "-" + model.PersonName);
                        sl.SetCellStyle(string.Concat("A", i), string.Concat("G", i), ExcelHelper.GetHeaderRowEmployeeDataLabelStyle(sl));
                        sl.SetCellStyle(string.Concat("A", i), string.Concat("G", i), ExcelHelper.SetFontColor(sl, Color.Red, true));

                        sl.MergeWorksheetCells(string.Concat("A", i + 1), string.Concat("G", i + 1));
                        sl.SetCellValue(string.Concat("A", i + 1), le.CurrencyName + "- (" + le.CurrencySymbol + ")");
                        sl.SetCellStyle(string.Concat("A", i + 1), string.Concat("G", i + 1), ExcelHelper.GetHeaderRowEmployeeDataLabelStyle(sl));
                        sl.SetCellStyle(string.Concat("A", i + 1), string.Concat("G", i + 1), ExcelHelper.SetFontColor(sl, Color.DarkBlue, true));


                        sl.SetCellValue(string.Concat("A", i + 2), "Loan");
                        sl.SetCellStyle(string.Concat("A", i + 2), ExcelHelper.GetNoBorderStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("A", i + 3), string.Concat("C", i + 3));
                        sl.SetCellValue(string.Concat("A", i + 3), le.CurrencyName + "- (" + le.CurrencySymbol + ")");
                        sl.SetCellStyle(string.Concat("A", i + 3), ExcelHelper.GetHeaderRowEOSDataLabelStyle(sl));
                        sl.SetCellStyle(string.Concat("A", i + 3), ExcelHelper.SetFontColor(sl, Color.DarkBlue, true));

                        sl.SetCellValue(string.Concat("B", i + 2), le.Name);
                        sl.SetCellStyle(string.Concat("B", i + 2), ExcelHelper.GetNoBorderCentreAlignStyle(sl));

                        //sl.SetCellValue(string.Concat("C", i + 2), model.EnrollDate.ToDayAndDateFormat());
                        sl.SetCellValue(string.Concat("C", i + 2), model.EnrollDate.ToDefaultDateFormat());
                        sl.SetCellStyle(string.Concat("C", i + 2), ExcelHelper.GetNoBorderCentreAlignStyle(sl));

                        sl.SetCellValue(string.Concat("D", i + 2), model.OpeningBalance.Value);
                        sl.SetCellStyle(string.Concat("D", i + 2), ExcelHelper.GetBottomBorderStyle(sl));
                        sl.SetCellStyle(string.Concat("D", i + 2), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("E", i + 2), model.EarningAmount);
                        sl.SetCellStyle(string.Concat("E", i + 2), ExcelHelper.GetBottomBorderStyle(sl));
                        sl.SetCellStyle(string.Concat("E", i + 2), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("F", i + 2), model.DeductionAmount);
                        sl.SetCellStyle(string.Concat("F", i + 2), ExcelHelper.GetBottomBorderStyle(sl));
                        sl.SetCellStyle(string.Concat("F", i + 2), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("G", i + 2), model.ClosingBalance.Value);
                        sl.SetCellStyle(string.Concat("G", i + 2), ExcelHelper.GetBottomBorderStyle(sl));
                        sl.SetCellStyle(string.Concat("G", i + 2), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("D", i + 3), model.OpeningBalance.Value);
                        sl.SetCellStyle(string.Concat("D", i + 3), ExcelHelper.GetNoBorderRightAlignStyleWithBold(sl));
                        sl.SetCellStyle(string.Concat("D", i + 3), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("E", i + 3), model.EarningAmount);
                        sl.SetCellStyle(string.Concat("E", i + 3), ExcelHelper.GetNoBorderRightAlignStyleWithBold(sl));
                        sl.SetCellStyle(string.Concat("E", i + 3), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("F", i + 3), model.DeductionAmount);
                        sl.SetCellStyle(string.Concat("F", i + 3), ExcelHelper.GetNoBorderRightAlignStyleWithBold(sl));
                        sl.SetCellStyle(string.Concat("F", i + 3), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("G", i + 3), model.ClosingBalance.Value);
                        sl.SetCellStyle(string.Concat("G", i + 3), ExcelHelper.GetNoBorderRightAlignStyleWithBold(sl));
                        sl.SetCellStyle(string.Concat("G", i + 3), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));
                        i = i + 5;
                        j = i;
                    }
                    //sl.MergeWorksheetCells(string.Concat("A", j), string.Concat("C", j));
                    sl.SetCellValue(string.Concat("A", j - 1), "Grand Total:");
                    sl.SetCellStyle(string.Concat("A", j - 1), string.Concat("G", j - 1), ExcelHelper.GetGrandTotalStyle(sl));
                    sl.SetCellStyle(string.Concat("A", j - 1), string.Concat("G", j - 1), ExcelHelper.GeBottomBorderStyle(sl));
                    sl.SetCellStyle(string.Concat("A", j - 1), string.Concat("G", j - 1), ExcelHelper.FillColorStyle(sl));
                    sl.SetCellStyle(string.Concat("A", j - 1), string.Concat("G", j - 1), ExcelHelper.GetItalicFontStyle(sl));
                    sl.SetCellValue(string.Concat("C", j - 1), ((i - 5) / 5));
                }

                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);

            }

            ms.Position = 0;
            return ms;
        }
        public async Task<MemoryStream> GetVacationAccuralDetails(List<PayrollReportViewModel> list)
        {

         //   var _salaryElementInfoBussiness = BusinessHelper.GetInstance<ISalaryElementInfoBusiness>();
            var sponsorshipList = list.Select(x => x.SponsorId).Distinct().ToList();
            var ms = new MemoryStream();
            var code = _userContext.LegalEntityCode;
            var le = await _legalEntityBusiness.GetSingle(x => x.Code == code);
            var sponsorList = await _HrCoreBusiness.GetSponsorList();
            var leName = "CAYAN";
            if (le != null)
            {
                leName = le.Name;
            }
            using (var sl = new SLDocument())
            {
                //foreach (var _sponsor in sponsorshipList)
                //{
                //    var selectedaccural = list.Where(x => x.SponsorId == _sponsor).ToList();
                //    var firstRow = selectedaccural.FirstOrDefault();

                //    var personNo = firstRow.SponsorshipNo ?? firstRow.PersonNo;
                //    var sponsor = sponsorList.Where(e => e.Id == _sponsor).FirstOrDefault();
                sl.AddWorksheet(le.Name);
                SLPageSettings pageSettings = new SLPageSettings();
                pageSettings.ShowGridLines = false;
                sl.SetPageSettings(pageSettings);

                //sl.FreezePanes(2, 0);
                //sl.SetColumnWidth(2, 15);
                sl.SetColumnWidth("A", "B", 5);
                sl.SetColumnWidth("C", 5);
                sl.SetColumnWidth("D", "E", 14);
                sl.SetColumnWidth("F", "K", 10); //11.5

                //sl.SetColumnWidth("L", "Q", 4);
                //sl.SetColumnWidth("R", "AI", 2.5);


                sl.SetCellStyle("A1", "W1", ExcelHelper.GetTopBorderStyle(sl));


                sl.MergeWorksheetCells("A1", "E1");
                //  sl.SetCellValue("A1", le.Name);
                sl.SetCellStyle("A1", "E1", ExcelHelper.GetHeaderRowEmployeeDataLabelStyle(sl));

                sl.MergeWorksheetCells("J1", "K1");
                sl.SetCellValue("J1", DateTime.Today.ToDefaultDateFormat());
                sl.SetCellStyle("J1", "K1", ExcelHelper.GetHeaderRowDateStyle(sl));

                sl.MergeWorksheetCells("A2", "K3");
                sl.SetCellValue("A2", "Employee Benefits - Vacation Accural");
                sl.SetCellStyle("A2", "K3", ExcelHelper.GetReportHeadingStyle(sl));

                sl.MergeWorksheetCells("A3", "K3");
                sl.SetCellStyle("A3", "K3", ExcelHelper.GetHeaderRowSubHeadingStyle(sl));

                //sl.MergeWorksheetCells("A5", "K5");
                //sl.SetCellValue("A5", sponsor.Name + " - " + le.CurrencyName + "(" + le.CurrencyCode + ")");
                //sl.SetCellStyle("A5", "K5", ExcelHelper.SetFontColor(sl, Color.Red, true));

                //sl.MergeWorksheetCells("A7", "B9");
                //sl.SetCellValue("A7", "Emp ID");
                //sl.SetCellStyle("A7", "B9", ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells("A7", "C9");
                sl.SetCellValue("A7", "Emp No.");
                sl.SetCellStyle("A7", "C9", ExcelHelper.GetShiftEntryDateStyle(sl));


                sl.MergeWorksheetCells("D7", "E9");
                sl.SetCellValue("D7", "Name");
                sl.SetCellStyle("D7", "E9", ExcelHelper.GetShiftEntryDateStyle(sl));


                sl.MergeWorksheetCells("F7", "K7");
                sl.SetCellValue("F7", "Total Salary");
                sl.SetCellStyle("F7", "K7", ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells("F8", "F9");
                sl.SetCellValue("F8", "Starting Balance");
                sl.SetCellStyle("F8", "F9", ExcelHelper.GetShiftEntryDateStyle(sl));


                sl.MergeWorksheetCells("G8", "H8");
                sl.SetCellValue("G8", "Month Due");
                sl.SetCellStyle("G8", "H8", ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells("I8", "J8");
                sl.SetCellValue("I8", "Taken");
                sl.SetCellStyle("I8", "J8", ExcelHelper.GetShiftEntryDateStyle(sl));


                sl.MergeWorksheetCells("K8", "K9");
                sl.SetCellValue("K8", "Closing Balance");
                sl.SetCellStyle("K8", "K9", ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.SetCellValue("G9", "Days");
                sl.SetCellStyle("G9", ExcelHelper.GetTimeStyle(sl));
                sl.SetCellValue("H9", "Amount");
                sl.SetCellStyle("H9", ExcelHelper.GetTimeStyle(sl));
                sl.SetCellValue("I9", "Annual Leave Days");
                sl.SetCellStyle("I9", ExcelHelper.GetShiftEntryDateStyle(sl));
                sl.SetCellValue("J9", "Amount");
                sl.SetCellStyle("J9", ExcelHelper.GetTimeStyle(sl));

                sl.MergeWorksheetCells("L7", "Q7");
                sl.SetCellValue("L7", " ");
                sl.SetCellStyle("L7", "Q7", ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells("L8", "M8");
                sl.SetCellValue("L8", "Taken");
                sl.SetCellStyle("L8", "M8", ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.SetCellValue("L9", "Sick Leave Days");
                sl.SetCellStyle("L9", ExcelHelper.GetShiftEntryDateStyle(sl));
                sl.SetCellValue("M9", "Amount");
                sl.SetCellStyle("M9", ExcelHelper.GetTimeStyle(sl));

                sl.MergeWorksheetCells("N8", "O8");
                sl.SetCellValue("N8", "Taken");
                sl.SetCellStyle("N8", "O8", ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.SetCellValue("N9", "Unpaid Leave Days");
                sl.SetCellStyle("N9", ExcelHelper.GetShiftEntryDateStyle(sl));
                sl.SetCellValue("O9", "Amount");
                sl.SetCellStyle("O9", ExcelHelper.GetTimeStyle(sl));

                sl.MergeWorksheetCells("P8", "Q8");
                sl.SetCellValue("P8", "Taken");
                sl.SetCellStyle("P8", "Q8", ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.SetCellValue("P9", "Non Working Days");
                sl.SetCellStyle("P9", ExcelHelper.GetShiftEntryDateStyle(sl));
                sl.SetCellValue("Q9", "Amount");
                sl.SetCellStyle("Q9", ExcelHelper.GetShiftEntryDateStyle(sl));

                //sl.MergeWorksheetCells("L7", "Q7");
                //sl.SetCellValue("L7", "Holidays");
                //sl.SetCellStyle("L7", "Q7", ExcelHelper.GetShiftEntryDateStyle(sl));

                //sl.MergeWorksheetCells("L8", "L9");
                //sl.SetCellValue("L8", "Starting Balance");
                //sl.SetCellStyle("L8", "L9", ExcelHelper.GetShiftEntryDateStyle(sl));

                //sl.MergeWorksheetCells("M8", "N8");
                //sl.SetCellValue("M8", "Month Due");
                //sl.SetCellStyle("M8", "N8", ExcelHelper.GetShiftEntryDateStyle(sl));

                //sl.MergeWorksheetCells("O8", "P8");
                //sl.SetCellValue("O8", "Taken");
                //sl.SetCellStyle("O8", "P8", ExcelHelper.GetShiftEntryDateStyle(sl));

                //sl.MergeWorksheetCells("Q8", "Q9");
                //sl.SetCellValue("Q8", "Closing Balance");
                //sl.SetCellStyle("Q8", "Q9", ExcelHelper.GetShiftEntryDateStyle(sl));

                //sl.SetCellValue("M9", "Days");
                //sl.SetCellStyle("M9", ExcelHelper.GetTimeStyle(sl));
                //sl.SetCellValue("N9", "Amount");
                //sl.SetCellStyle("N9", ExcelHelper.GetTimeStyle(sl));
                //sl.SetCellValue("O9", "Days");
                //sl.SetCellStyle("O9", ExcelHelper.GetTimeStyle(sl));
                //sl.SetCellValue("P9", "Amount");
                //sl.SetCellStyle("P9", ExcelHelper.GetTimeStyle(sl));

                //sl.MergeWorksheetCells("R7", "W7");
                //sl.SetCellValue("R7", "Days Off");
                //sl.SetCellStyle("R7", "W7", ExcelHelper.GetShiftEntryDateStyle(sl));

                //sl.MergeWorksheetCells("R8", "R9");
                //sl.SetCellValue("R8", "Starting Balance");
                //sl.SetCellStyle("R8", "R9", ExcelHelper.GetShiftEntryDateStyle(sl));

                //sl.MergeWorksheetCells("S8", "T8");
                //sl.SetCellValue("S8", "Month Due");
                //sl.SetCellStyle("S8", "T8", ExcelHelper.GetShiftEntryDateStyle(sl));

                //sl.MergeWorksheetCells("U8", "V8");
                //sl.SetCellValue("U8", "Taken");
                //sl.SetCellStyle("U8", "V8", ExcelHelper.GetShiftEntryDateStyle(sl));

                //sl.MergeWorksheetCells("W8", "W9");
                //sl.SetCellValue("W8", "Closing Balance");
                //sl.SetCellStyle("W8", "W9", ExcelHelper.GetShiftEntryDateStyle(sl));

                //sl.SetCellValue("S9", "Days");
                //sl.SetCellStyle("S9", ExcelHelper.GetTimeStyle(sl));
                //sl.SetCellValue("T9", "Amount");
                //sl.SetCellStyle("T9", ExcelHelper.GetTimeStyle(sl));
                //sl.SetCellValue("U9", "Days");
                //sl.SetCellStyle("U9", ExcelHelper.GetTimeStyle(sl));
                //sl.SetCellValue("V9", "Amount");
                //sl.SetCellStyle("V9", ExcelHelper.GetTimeStyle(sl));

                int row = 10;
                foreach (var _sponsor in sponsorshipList)
                {
                    var selectedaccural = list.Where(x => x.SponsorId == _sponsor).ToList();
                    var firstRow = selectedaccural.FirstOrDefault();

                    var personNo = firstRow.SponsorshipNo ?? firstRow.PersonNo;
                    var sponsor = sponsorList.Where(e => e.Id == _sponsor).FirstOrDefault();

                    sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("Q", row));
                    sl.SetCellValue(string.Concat("A", row), sponsor.Name);
                    sl.SetCellStyle(string.Concat("A", row), string.Concat("Q", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                    //sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("K", row));
                    //sl.SetCellValue(string.Concat("F", row), " ");
                    //sl.SetCellStyle(string.Concat("F", row), string.Concat("K", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                    //sl.MergeWorksheetCells(string.Concat("L", row), string.Concat("Q", row));
                    //sl.SetCellValue(string.Concat("L", row), " ");
                    //sl.SetCellStyle(string.Concat("L", row), string.Concat("Q", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                    row++;

                    int i = row;
                    int j = 0;
                    double MonthlyStartingBalance = 0.0;
                    double MonthlyLeaveBalance = 0.0;
                    double MonthlyclosingBalance = 0.0;
                    double TotalEarningAmount = 0.0;
                    double TakenDays = 0.0;
                    double HolidayMonthlyDays = 0.0;
                    double HolidayMonthlyAmount = 0.0;
                    double HolidayTakenDays = 0.0;
                    double HolidayTakenAmount = 0.0;
                    double DaysOffMonthlyDays = 0.0;
                    double DaysOffMonthlyAmount = 0.0;
                    double DaysOffTakenDays = 0.0;
                    double DaysOffTakenAmount = 0.0;
                    double DaysOffClosingBalance = 0.0;
                    double DaysOffStartingBalance = 0.0;
                    double HolidayClosingBalance = 0.0;
                    double HolidayStartingBalance = 0.0;
                    var TotalDeductionAmount = 0.0;
                    var TotalSickLeave = 0.0;
                    var TotalSickLeaveAmount = 0.0;
                    var TotalUnpaidLeave = 0.0;
                    var TotalUnpaidLeaveAmount = 0.0;
                    var TotalNonWorking = 0.0;
                    var TotalNonWorkingAmount = 0.0;
                    int empcount = 0;
                    foreach (var model in selectedaccural)
                    {
                        var onedayamount =  await _PayrollelementBusiness.GetUserOneDaySalary(model.UserId);
                        //if (!model.RosterText.Contains('-'))
                        //{
                        //    // sl.SetCellStyle(string.Concat("A", i), string.Concat("BC", i), ExcelHelper.GetBoldFontStyle(sl));
                        //}


                        //sl.MergeWorksheetCells(string.Concat("A", i), string.Concat("B", i));
                        //sl.SetCellValue(string.Concat("A", i), model.SponsorshipNo);
                        //sl.SetCellStyle(string.Concat("A", i), string.Concat("B", i), ExcelHelper.GetEnteryTextWrapStyle(sl));
                        //sl.SetCellStyle(string.Concat("A", i), string.Concat("B", i), ExcelHelper.GetEntryDayDateStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("A", i), string.Concat("C", i));
                        sl.SetCellValue(string.Concat("A", i), model.PersonNo);
                        sl.SetCellStyle(string.Concat("A", i), string.Concat("C", i), ExcelHelper.GetEnteryTextWrapStyle(sl));
                        //sl.SetCellStyle(string.Concat("C", i), ExcelHelper.GetEntryDayDateStyle(sl));


                        sl.MergeWorksheetCells(string.Concat("D", i), string.Concat("E", i));
                        sl.SetCellValue(string.Concat("D", i), model.PersonName);
                        sl.SetCellStyle(string.Concat("D", i), string.Concat("E", i), ExcelHelper.GetEnteryTextWrapStyle(sl));
                        //sl.SetCellStyle(string.Concat("D", i), string.Concat("E", i), ExcelHelper.GetEntryDayDateStyle(sl));

                        sl.SetCellValue(string.Concat("F", i), model.OpeningQuantity.Value);
                        sl.SetCellStyle(string.Concat("F", i), ExcelHelper.GetEntryDayStyle(sl));
                        sl.SetCellStyle(string.Concat("F", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("G", i), model.EarningQuantity.Value);
                        sl.SetCellStyle(string.Concat("G", i), ExcelHelper.GetEntryDayStyle(sl));
                        sl.SetCellStyle(string.Concat("G", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        model.EarningAmount = Convert.ToDouble(model.EarningQuantity.Value) * onedayamount;
                        sl.SetCellValue(string.Concat("H", i), model.EarningAmount);
                        sl.SetCellStyle(string.Concat("H", i), ExcelHelper.GetEntryDayStyle(sl));
                        sl.SetCellStyle(string.Concat("H", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("I", i), model.DeductionQuantity.Value);
                        sl.SetCellStyle(string.Concat("I", i), ExcelHelper.GetEntryDayStyle(sl));
                        sl.SetCellStyle(string.Concat("I", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        model.DeductionAmount = Convert.ToDouble(model.DeductionQuantity.Value) * onedayamount;
                        sl.SetCellValue(string.Concat("J", i), model.DeductionAmount);
                        sl.SetCellStyle(string.Concat("J", i), ExcelHelper.GetEntryDayStyle(sl));
                        sl.SetCellStyle(string.Concat("J", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        sl.SetCellValue(string.Concat("K", i), model.ClosingQuantity.Value);
                        sl.SetCellStyle(string.Concat("K", i), ExcelHelper.GetEntryDayStyle(sl));
                        sl.SetCellStyle(string.Concat("K", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        //Sick Leave
                        sl.SetCellValue(string.Concat("L", i), model.SickLeaveDays.Value);
                        sl.SetCellStyle(string.Concat("L", i), ExcelHelper.GetEntryDayStyle(sl));
                        sl.SetCellStyle(string.Concat("L", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        //var sickLeaveAmount = Convert.ToDouble(model.SickLeaveDays.Value) * onedayamount;
                        sl.SetCellValue(string.Concat("M", i), model.SickLeaveAmount.Value);
                        sl.SetCellStyle(string.Concat("M", i), ExcelHelper.GetEntryDayStyle(sl));
                        sl.SetCellStyle(string.Concat("M", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        //Unpaid Leave
                        sl.SetCellValue(string.Concat("N", i), model.UnpaidLeaveDays.Value);
                        sl.SetCellStyle(string.Concat("N", i), ExcelHelper.GetEntryDayStyle(sl));
                        sl.SetCellStyle(string.Concat("N", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        //var unpaidLeaveAmount = Convert.ToDouble(model.UnpaidLeaveDays.Value) * onedayamount;
                        sl.SetCellValue(string.Concat("O", i), model.UnpaidLeaveAmount.Value);
                        sl.SetCellStyle(string.Concat("O", i), ExcelHelper.GetEntryDayStyle(sl));
                        sl.SetCellStyle(string.Concat("O", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        //Non Working Leave
                        sl.SetCellValue(string.Concat("P", i), model.NonWorkingDays.Value);
                        sl.SetCellStyle(string.Concat("P", i), ExcelHelper.GetEntryDayStyle(sl));
                        sl.SetCellStyle(string.Concat("P", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        //model.DeductionAmount = Convert.ToDouble(model.DeductionQuantity.Value) * onedayamount;
                        sl.SetCellValue(string.Concat("Q", i), model.NonWorkingAmount.Value);
                        sl.SetCellStyle(string.Concat("Q", i), ExcelHelper.GetEntryDayStyle(sl));
                        sl.SetCellStyle(string.Concat("Q", i), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                        //sl.SetCellValue(string.Concat("L", i), model.HolidayOpeningBalance.HasValue ? model.HolidayOpeningBalance.Value : 0.0);
                        //sl.SetCellStyle(string.Concat("L", i), ExcelHelper.GetEntryDayStyle(sl));
                        //sl.SetCellValue(string.Concat("M", i), model.HolidayMonthlyDueDays.HasValue ? model.HolidayMonthlyDueDays.Value : 0.0);
                        //sl.SetCellStyle(string.Concat("M", i), ExcelHelper.GetEntryDayStyle(sl));
                        //sl.SetCellValue(string.Concat("N", i), model.HolidayMonthlyDueAmount.HasValue ? model.HolidayMonthlyDueAmount.Value : 0.0);
                        //sl.SetCellStyle(string.Concat("N", i), ExcelHelper.GetEntryDayStyle(sl));
                        //sl.SetCellValue(string.Concat("O", i), model.HolidayLeaveTakenDays.HasValue ? model.HolidayLeaveTakenDays.Value : 0.0);
                        //sl.SetCellStyle(string.Concat("O", i), ExcelHelper.GetEntryDayStyle(sl));
                        //sl.SetCellValue(string.Concat("P", i), model.HolidayLeaveTakenAmount.HasValue ? model.HolidayLeaveTakenAmount.Value : 0.0);
                        //sl.SetCellStyle(string.Concat("P", i), ExcelHelper.GetEntryDayStyle(sl));
                        //sl.SetCellValue(string.Concat("Q", i), model.HolidayClosingBalance.HasValue ? model.HolidayClosingBalance.Value : 0.0);
                        //sl.SetCellStyle(string.Concat("Q", i), ExcelHelper.GetEntryDayStyle(sl));

                        //sl.SetCellValue(string.Concat("R", i), model.DaysOffOpeningBalance.HasValue ? model.DaysOffOpeningBalance.Value : 0.0);
                        //sl.SetCellStyle(string.Concat("R", i), ExcelHelper.GetEntryDayStyle(sl));
                        //sl.SetCellValue(string.Concat("S", i), model.DayOffMonthlyDueDays.HasValue ? model.DayOffMonthlyDueDays.Value : 0.0);
                        //sl.SetCellStyle(string.Concat("S", i), ExcelHelper.GetEntryDayStyle(sl));
                        //sl.SetCellValue(string.Concat("T", i), model.DaysOffMonthlyDueAmount.HasValue ? model.DaysOffMonthlyDueAmount.Value : 0.0);
                        //sl.SetCellStyle(string.Concat("T", i), ExcelHelper.GetEntryDayStyle(sl));
                        //sl.SetCellValue(string.Concat("U", i), model.DayOffLeaveTakenDays.HasValue ? model.DayOffLeaveTakenDays.Value : 0.0);
                        //sl.SetCellStyle(string.Concat("U", i), ExcelHelper.GetEntryDayStyle(sl));
                        //sl.SetCellValue(string.Concat("V", i), model.DaysOffLeaveTakenAmount.HasValue ? model.DaysOffLeaveTakenAmount.Value : 0.0);
                        //sl.SetCellStyle(string.Concat("V", i), ExcelHelper.GetEntryDayStyle(sl));
                        //sl.SetCellValue(string.Concat("W", i), model.DaysOffClosingBalance.HasValue ? model.DaysOffClosingBalance.Value : 0.0);
                        //sl.SetCellStyle(string.Concat("W", i), ExcelHelper.GetEntryDayStyle(sl));
                        i++;
                        j = i;
                        MonthlyStartingBalance = MonthlyStartingBalance + model.OpeningQuantity.Value;
                        MonthlyclosingBalance = MonthlyclosingBalance + model.ClosingQuantity.Value;
                        MonthlyLeaveBalance = MonthlyLeaveBalance + model.EarningQuantity.Value;
                        TotalEarningAmount = TotalEarningAmount + model.EarningAmount;
                        TakenDays = TakenDays + model.DeductionQuantity.Value;
                        TotalDeductionAmount = TotalDeductionAmount + model.DeductionAmount;
                        TotalSickLeave = TotalSickLeave + model.SickLeaveDays.Value;
                        TotalSickLeaveAmount = TotalSickLeaveAmount + model.SickLeaveAmount.Value;
                        TotalUnpaidLeave = TotalUnpaidLeave + model.UnpaidLeaveDays.Value;
                        TotalUnpaidLeaveAmount = TotalUnpaidLeaveAmount + model.UnpaidLeaveAmount.Value;
                        TotalNonWorking = TotalNonWorking + model.NonWorkingDays.Value;
                        TotalNonWorkingAmount = TotalNonWorkingAmount + model.NonWorkingAmount.Value;

                        //HolidayMonthlyDays = HolidayMonthlyDays + model.HolidayMonthlyDueDays ?? 0.0;
                        //HolidayMonthlyAmount = HolidayMonthlyAmount + model.HolidayMonthlyDueAmount ?? 0.0;
                        //HolidayTakenDays = HolidayTakenDays + model.HolidayLeaveTakenDays ?? 0.0;
                        //HolidayTakenAmount = HolidayTakenAmount + model.HolidayLeaveTakenAmount ?? 0.0;
                        //DaysOffMonthlyDays = DaysOffMonthlyDays + model.DayOffMonthlyDueDays ?? 0.0;
                        //DaysOffMonthlyAmount = DaysOffMonthlyAmount + model.DaysOffMonthlyDueAmount ?? 0.0;
                        //DaysOffTakenDays = DaysOffTakenDays + model.DayOffLeaveTakenDays ?? 0.0;
                        //DaysOffTakenAmount = DaysOffTakenAmount + model.DaysOffLeaveTakenAmount ?? 0.0;
                        //DaysOffClosingBalance = DaysOffClosingBalance + model.DaysOffClosingBalance ?? 0.0;
                        //DaysOffStartingBalance = DaysOffStartingBalance + model.DaysOffOpeningBalance ?? 0.0;
                        //HolidayClosingBalance = HolidayClosingBalance + model.HolidayClosingBalance ?? 0.0;
                        //HolidayStartingBalance = HolidayStartingBalance + model.HolidayOpeningBalance ?? 0.0;
                        empcount++;
                    }

                    // sl.SetCellStyle(string.Concat("A", j), string.Concat("W", j), ExcelHelper.GetTopBorderStyle(sl));

                    sl.MergeWorksheetCells(string.Concat("A", j), string.Concat("E", j));
                    sl.SetCellValue(string.Concat("A", j), sponsor.Name + "- " + le.CurrencyName + "(" + le.CurrencySymbol + ") - " + (empcount).ToString());
                    sl.SetCellStyle(string.Concat("A", j), string.Concat("E", j), ExcelHelper.GetGrandTotalStyle(sl));
                    sl.SetCellStyle(string.Concat("A", j), string.Concat("E", j), ExcelHelper.GeBottomBorderStyle(sl));
                    sl.SetCellStyle(string.Concat("A", j), string.Concat("E", j), ExcelHelper.FillColorStyle(sl));
                    sl.SetCellStyle(string.Concat("A", j), string.Concat("E", j), ExcelHelper.GetItalicFontStyle(sl));
                    sl.SetCellStyle(string.Concat("A", j), string.Concat("E", j), ExcelHelper.SetFontColor(sl, Color.Red, true));

                    //sl.MergeWorksheetCells(string.Concat("F", j), string.Concat("F", j));
                    sl.SetCellValue(string.Concat("F", j), MonthlyStartingBalance);

                    sl.SetCellStyle(string.Concat("F", j), ExcelHelper.GetEntryDayStyle(sl));
                    sl.SetCellStyle(string.Concat("F", j), ExcelHelper.GeBottomBorderStyle(sl));
                    sl.SetCellStyle(string.Concat("F", j), ExcelHelper.FillColorStyle(sl));
                    sl.SetCellStyle(string.Concat("F", j), ExcelHelper.GetItalicFontStyle(sl));
                    sl.SetCellStyle(string.Concat("F", j), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                    //sl.MergeWorksheetCells(string.Concat("G", j), string.Concat("G", j));
                    sl.SetCellValue(string.Concat("G", j), MonthlyLeaveBalance);
                    sl.SetCellStyle(string.Concat("G", j), ExcelHelper.GetEntryDayStyle(sl));
                    sl.SetCellStyle(string.Concat("G", j), ExcelHelper.GeBottomBorderStyle(sl));
                    sl.SetCellStyle(string.Concat("G", j), ExcelHelper.FillColorStyle(sl));
                    sl.SetCellStyle(string.Concat("G", j), ExcelHelper.GetItalicFontStyle(sl));
                    sl.SetCellStyle(string.Concat("G", j), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                    //sl.MergeWorksheetCells(string.Concat("H", j), string.Concat("H", j ));
                    sl.SetCellValue(string.Concat("H", j), TotalEarningAmount);

                    sl.SetCellStyle(string.Concat("H", j), ExcelHelper.GetEntryDayStyle(sl));
                    sl.SetCellStyle(string.Concat("H", j), ExcelHelper.GeBottomBorderStyle(sl));
                    sl.SetCellStyle(string.Concat("H", j), ExcelHelper.FillColorStyle(sl));
                    sl.SetCellStyle(string.Concat("H", j), ExcelHelper.GetItalicFontStyle(sl));
                    sl.SetCellStyle(string.Concat("H", j), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                    //sl.MergeWorksheetCells(string.Concat("I", j), string.Concat("I", j));
                    sl.SetCellValue(string.Concat("I", j), TakenDays);
                    sl.SetCellStyle(string.Concat("I", j), ExcelHelper.GetEntryDayStyle(sl));
                    sl.SetCellStyle(string.Concat("I", j), ExcelHelper.GeBottomBorderStyle(sl));
                    sl.SetCellStyle(string.Concat("I", j), ExcelHelper.FillColorStyle(sl));
                    sl.SetCellStyle(string.Concat("I", j), ExcelHelper.GetItalicFontStyle(sl));
                    sl.SetCellStyle(string.Concat("I", j), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                    //sl.MergeWorksheetCells(string.Concat("J", j), string.Concat("J", j));
                    sl.SetCellValue(string.Concat("J", j), TotalDeductionAmount);
                    sl.SetCellStyle(string.Concat("J", j), ExcelHelper.GetEntryDayStyle(sl));
                    sl.SetCellStyle(string.Concat("J", j), ExcelHelper.GeBottomBorderStyle(sl));
                    sl.SetCellStyle(string.Concat("J", j), ExcelHelper.FillColorStyle(sl));
                    sl.SetCellStyle(string.Concat("J", j), ExcelHelper.GetItalicFontStyle(sl));
                    sl.SetCellStyle(string.Concat("J", j), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));


                    //sl.MergeWorksheetCells(string.Concat("K", j), string.Concat("K", j));
                    sl.SetCellValue(string.Concat("K", j), MonthlyclosingBalance);
                    sl.SetCellStyle(string.Concat("K", j), ExcelHelper.GetEntryDayStyle(sl));
                    sl.SetCellStyle(string.Concat("K", j), ExcelHelper.GeBottomBorderStyle(sl));
                    sl.SetCellStyle(string.Concat("K", j), ExcelHelper.FillColorStyle(sl));
                    sl.SetCellStyle(string.Concat("K", j), ExcelHelper.GetItalicFontStyle(sl));
                    sl.SetCellStyle(string.Concat("K", j), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                    sl.SetCellValue(string.Concat("L", j), TotalSickLeave);
                    sl.SetCellStyle(string.Concat("L", j), ExcelHelper.GetEntryDayStyle(sl));
                    sl.SetCellStyle(string.Concat("L", j), ExcelHelper.GeBottomBorderStyle(sl));
                    sl.SetCellStyle(string.Concat("L", j), ExcelHelper.FillColorStyle(sl));
                    sl.SetCellStyle(string.Concat("L", j), ExcelHelper.GetItalicFontStyle(sl));
                    sl.SetCellStyle(string.Concat("L", j), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                    sl.SetCellValue(string.Concat("M", j), TotalSickLeaveAmount);
                    sl.SetCellStyle(string.Concat("M", j), ExcelHelper.GetEntryDayStyle(sl));
                    sl.SetCellStyle(string.Concat("M", j), ExcelHelper.GeBottomBorderStyle(sl));
                    sl.SetCellStyle(string.Concat("M", j), ExcelHelper.FillColorStyle(sl));
                    sl.SetCellStyle(string.Concat("M", j), ExcelHelper.GetItalicFontStyle(sl));
                    sl.SetCellStyle(string.Concat("M", j), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                    sl.SetCellValue(string.Concat("N", j), TotalUnpaidLeave);
                    sl.SetCellStyle(string.Concat("N", j), ExcelHelper.GetEntryDayStyle(sl));
                    sl.SetCellStyle(string.Concat("N", j), ExcelHelper.GeBottomBorderStyle(sl));
                    sl.SetCellStyle(string.Concat("N", j), ExcelHelper.FillColorStyle(sl));
                    sl.SetCellStyle(string.Concat("N", j), ExcelHelper.GetItalicFontStyle(sl));
                    sl.SetCellStyle(string.Concat("N", j), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                    sl.SetCellValue(string.Concat("O", j), TotalUnpaidLeaveAmount);
                    sl.SetCellStyle(string.Concat("O", j), ExcelHelper.GetEntryDayStyle(sl));
                    sl.SetCellStyle(string.Concat("O", j), ExcelHelper.GeBottomBorderStyle(sl));
                    sl.SetCellStyle(string.Concat("O", j), ExcelHelper.FillColorStyle(sl));
                    sl.SetCellStyle(string.Concat("O", j), ExcelHelper.GetItalicFontStyle(sl));
                    sl.SetCellStyle(string.Concat("O", j), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                    sl.SetCellValue(string.Concat("P", j), TotalNonWorking);
                    sl.SetCellStyle(string.Concat("P", j), ExcelHelper.GetEntryDayStyle(sl));
                    sl.SetCellStyle(string.Concat("P", j), ExcelHelper.GeBottomBorderStyle(sl));
                    sl.SetCellStyle(string.Concat("P", j), ExcelHelper.FillColorStyle(sl));
                    sl.SetCellStyle(string.Concat("P", j), ExcelHelper.GetItalicFontStyle(sl));
                    sl.SetCellStyle(string.Concat("P", j), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                    sl.SetCellValue(string.Concat("Q", j), TotalNonWorkingAmount);
                    sl.SetCellStyle(string.Concat("Q", j), ExcelHelper.GetEntryDayStyle(sl));
                    sl.SetCellStyle(string.Concat("Q", j), ExcelHelper.GeBottomBorderStyle(sl));
                    sl.SetCellStyle(string.Concat("Q", j), ExcelHelper.FillColorStyle(sl));
                    sl.SetCellStyle(string.Concat("Q", j), ExcelHelper.GetItalicFontStyle(sl));
                    sl.SetCellStyle(string.Concat("Q", j), ExcelHelper.GetCurrencyStyleWith2Decimal(sl));

                    //sl.SetCellValue(string.Concat("L", j), HolidayStartingBalance);
                    //sl.SetCellStyle(string.Concat("L", j), ExcelHelper.GetEntryDayStyle(sl));
                    //sl.SetCellStyle(string.Concat("L", j), ExcelHelper.GeBottomBorderStyle(sl));
                    //sl.SetCellStyle(string.Concat("L", j), ExcelHelper.FillColorStyle(sl));
                    //sl.SetCellStyle(string.Concat("L", j), ExcelHelper.GetItalicFontStyle(sl));

                    //sl.SetCellValue(string.Concat("M", j), HolidayMonthlyDays);
                    //sl.SetCellStyle(string.Concat("M", j), ExcelHelper.GetEntryDayStyle(sl));
                    //sl.SetCellStyle(string.Concat("M", j), ExcelHelper.GeBottomBorderStyle(sl));
                    //sl.SetCellStyle(string.Concat("M", j), ExcelHelper.FillColorStyle(sl));
                    //sl.SetCellStyle(string.Concat("M", j), ExcelHelper.GetItalicFontStyle(sl));

                    //sl.SetCellValue(string.Concat("N", j), HolidayMonthlyAmount);
                    //sl.SetCellStyle(string.Concat("N", j), ExcelHelper.GetEntryDayStyle(sl));
                    //sl.SetCellStyle(string.Concat("N", j), ExcelHelper.GeBottomBorderStyle(sl));
                    //sl.SetCellStyle(string.Concat("N", j), ExcelHelper.FillColorStyle(sl));
                    //sl.SetCellStyle(string.Concat("N", j), ExcelHelper.GetItalicFontStyle(sl));

                    //sl.SetCellValue(string.Concat("O", j), HolidayTakenDays);
                    //sl.SetCellStyle(string.Concat("O", j), ExcelHelper.GetEntryDayStyle(sl));
                    //sl.SetCellStyle(string.Concat("O", j), ExcelHelper.GeBottomBorderStyle(sl));
                    //sl.SetCellStyle(string.Concat("O", j), ExcelHelper.FillColorStyle(sl));
                    //sl.SetCellStyle(string.Concat("O", j), ExcelHelper.GetItalicFontStyle(sl));


                    //sl.SetCellValue(string.Concat("P", j), HolidayTakenAmount);
                    //sl.SetCellStyle(string.Concat("P", j), ExcelHelper.GetEntryDayStyle(sl));
                    //sl.SetCellStyle(string.Concat("P", j), ExcelHelper.GeBottomBorderStyle(sl));
                    //sl.SetCellStyle(string.Concat("P", j), ExcelHelper.FillColorStyle(sl));
                    //sl.SetCellStyle(string.Concat("P", j), ExcelHelper.GetItalicFontStyle(sl));


                    //sl.SetCellValue(string.Concat("Q", j), HolidayClosingBalance);

                    //sl.SetCellStyle(string.Concat("Q", j), ExcelHelper.GetEntryDayStyle(sl));
                    //sl.SetCellStyle(string.Concat("Q", j), ExcelHelper.GeBottomBorderStyle(sl));
                    //sl.SetCellStyle(string.Concat("Q", j), ExcelHelper.FillColorStyle(sl));
                    //sl.SetCellStyle(string.Concat("Q", j), ExcelHelper.GetItalicFontStyle(sl));

                    //sl.SetCellValue(string.Concat("R", j), DaysOffStartingBalance);

                    //sl.SetCellStyle(string.Concat("R", j), ExcelHelper.GetEntryDayStyle(sl));
                    //sl.SetCellStyle(string.Concat("R", j), ExcelHelper.GeBottomBorderStyle(sl));
                    //sl.SetCellStyle(string.Concat("R", j), ExcelHelper.FillColorStyle(sl));
                    //sl.SetCellStyle(string.Concat("R", j), ExcelHelper.GetItalicFontStyle(sl));

                    //sl.SetCellValue(string.Concat("S", j), DaysOffMonthlyDays);

                    //sl.SetCellStyle(string.Concat("S", j), ExcelHelper.GetEntryDayStyle(sl));
                    //sl.SetCellStyle(string.Concat("S", j), ExcelHelper.GeBottomBorderStyle(sl));
                    //sl.SetCellStyle(string.Concat("S", j), ExcelHelper.FillColorStyle(sl));
                    //sl.SetCellStyle(string.Concat("S", j), ExcelHelper.GetItalicFontStyle(sl));

                    //sl.SetCellValue(string.Concat("T", j), DaysOffMonthlyAmount);

                    //sl.SetCellStyle(string.Concat("T", j), ExcelHelper.GetEntryDayStyle(sl));
                    //sl.SetCellStyle(string.Concat("T", j), ExcelHelper.GeBottomBorderStyle(sl));
                    //sl.SetCellStyle(string.Concat("T", j), ExcelHelper.FillColorStyle(sl));
                    //sl.SetCellStyle(string.Concat("T", j), ExcelHelper.GetItalicFontStyle(sl));

                    //sl.SetCellValue(string.Concat("U", j), DaysOffTakenDays);

                    //sl.SetCellStyle(string.Concat("U", j), ExcelHelper.GetEntryDayStyle(sl));
                    //sl.SetCellStyle(string.Concat("U", j), ExcelHelper.GeBottomBorderStyle(sl));
                    //sl.SetCellStyle(string.Concat("U", j), ExcelHelper.FillColorStyle(sl));
                    //sl.SetCellStyle(string.Concat("U", j), ExcelHelper.GetItalicFontStyle(sl));

                    //sl.SetCellValue(string.Concat("V", j), DaysOffTakenAmount);

                    //sl.SetCellStyle(string.Concat("V", j), ExcelHelper.GetEntryDayStyle(sl));
                    //sl.SetCellStyle(string.Concat("V", j), ExcelHelper.GeBottomBorderStyle(sl));
                    //sl.SetCellStyle(string.Concat("V", j), ExcelHelper.FillColorStyle(sl));
                    //sl.SetCellStyle(string.Concat("V", j), ExcelHelper.GetItalicFontStyle(sl));

                    //sl.SetCellValue(string.Concat("W", j), DaysOffClosingBalance);

                    //sl.SetCellStyle(string.Concat("W", j), ExcelHelper.GetEntryDayStyle(sl));
                    //sl.SetCellStyle(string.Concat("W", j), ExcelHelper.GeBottomBorderStyle(sl));
                    //sl.SetCellStyle(string.Concat("W", j), ExcelHelper.FillColorStyle(sl));
                    //sl.SetCellStyle(string.Concat("W", j), ExcelHelper.GetItalicFontStyle(sl));


                    sl.SetCellStyle("A1", string.Concat("A", j), ExcelHelper.GetLeftBorderStyle(sl));
                    sl.SetCellStyle(string.Concat("A", j), string.Concat("K", j), ExcelHelper.GetTopBorderStyle(sl));
                    // sl.SetCellStyle("X1", string.Concat("X", j), ExcelHelper.GetLeftBorderStyle(sl));
                    j++;
                    row = j;
                }

                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);

            }

            ms.Position = 0;
            return ms;
        }











    }
}
