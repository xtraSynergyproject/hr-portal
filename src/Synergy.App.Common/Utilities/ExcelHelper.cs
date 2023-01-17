using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;


namespace Synergy.App.Common.Utilities
{
    public static class ExcelHelper
    {



        public static SLStyle GetHeaderRowStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(220, 230, 241), System.Drawing.Color.FromArgb(220, 230, 241));
            return style;
        }

        public static SLStyle GetHeaderRowCayanStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 10;
            style.Font.FontName = "Verdana";
            //style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            style.Border.DiagonalBorder.BorderStyle = BorderStyleValues.None;
            return style;
        }
        public static SLStyle GetFlightSARTopStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.SetFontColor(System.Drawing.Color.DarkBlue);
            style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            return style;
        }
        public static SLStyle GetFlightGrandTotalStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.SetFontBold(true);
            //style.SetFontColor(System.Drawing.Color.Blue);
            // style.SetFontItalic(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            return style;
        }
        public static SLStyle GetFlightCountStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.SetFontBold(true);
            //style.SetFontColor(System.Drawing.Color.Blue);
            // style.SetFontItalic(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Right);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            return style;
        }
        public static SLStyle GetFlightSARBottomStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.SetFontColor(System.Drawing.Color.Blue);
            style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            return style;
        }
        public static SLStyle GetFlightSARTotalStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            //style.SetFontColor(System.Drawing.Color.Black);
            style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Right);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.TopBorder.Color = System.Drawing.Color.Red;
            return style;
        }
        public static SLStyle GetFlightOptionStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 8;
            style.Font.FontName = "Verdana";
            //style.SetFontColor(System.Drawing.Color.Black);
            //style.SetFontItalic(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            return style;
        }
        public static SLStyle GetFlightOptionAmountStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 8;
            style.Font.FontName = "Verdana";
            //style.SetFontColor(System.Drawing.Color.Black);
            //style.SetFontItalic(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Right);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            return style;
        }
        public static SLStyle GetFlightNameStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 9;
            style.Font.FontName = "Verdana";
            style.SetFontColor(System.Drawing.Color.Red);
            style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            return style;
        }
        public static SLStyle GetHeaderRowDateStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 11;
            style.Font.FontName = "Verdana";
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Right);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            return style;
        }
        public static SLStyle SetWrapText(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.SetWrapText(true);
            return style;
        }

        public static SLStyle GetHeaderRowHeadingStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 10;
            style.Font.FontName = "Verdana";
            style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.BottomBorder.Color = System.Drawing.Color.Black;
            return style;
        }

        public static SLStyle GetFlightReportSubHeadingStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 8;
            style.Font.FontName = "Verdana";
            //style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.TopBorder.Color = System.Drawing.Color.Black;
            //style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            //style.Border.LeftBorder.Color = System.Drawing.Color.Black;
            //style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
            //style.Border.BottomBorder.Color = System.Drawing.Color.Black;
            //style.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
            //style.Border.RightBorder.Color = System.Drawing.Color.Black;
            return style;
        }

        public static SLStyle GetFlightReportHeadingStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 10;
            style.Font.FontName = "Verdana";
            style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            //style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            //style.Border.TopBorder.Color = System.Drawing.Color.Black;
            //style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            //style.Border.LeftBorder.Color = System.Drawing.Color.Black;
            //style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
            //style.Border.BottomBorder.Color = System.Drawing.Color.Black;
            //style.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
            //style.Border.RightBorder.Color = System.Drawing.Color.Black;
            return style;
        }

        public static SLStyle GetReportHeadingStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 12;
            style.Font.FontName = "Verdana";
            style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            //style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            //style.Border.TopBorder.Color = System.Drawing.Color.Black;
            //style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            //style.Border.LeftBorder.Color = System.Drawing.Color.Black;
            //style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
            //style.Border.BottomBorder.Color = System.Drawing.Color.Black;
            //style.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
            //style.Border.RightBorder.Color = System.Drawing.Color.Black;
            return style;
        }

        public static SLStyle GetHeaderRowSubHeadingStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";

            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            return style;
        }

        public static SLStyle GetItalicFontStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.Bold = true;
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            return style;
        }

        public static SLStyle GetBoldFontStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.Bold = true;

            return style;
        }

        public static SLStyle GetHeaderRowEmployeeDataStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(0, 0, 128), System.Drawing.Color.FromArgb(0, 0, 128));
            style.SetFontColor(System.Drawing.Color.White);
            style.Font.FontSize = 11;
            style.Font.FontName = "Verdana";
            style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);

            return style;
        }
        public static SLStyle GetApprovedStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.SetFontColor(System.Drawing.Color.Green);
            //style.Font.FontSize = 11;
            //style.Font.FontName = "Verdana";
            //style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);

            return style;
        }
        public static SLStyle GetRejectedStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.SetFontColor(System.Drawing.Color.Red);
            //style.Font.FontSize = 11;
            //style.Font.FontName = "Verdana";
            //style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);

            return style;
        }
        public static SLStyle GetHeaderRowCayanLabelStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 10;
            style.Font.FontName = "Verdana";
            style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);

            return style;
        }
        public static SLStyle GetHeaderRowEmployeeDataLabelStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 9;
            style.Font.FontName = "Verdana";
            style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);

            return style;
        }
        public static SLStyle GetHeaderRowEOSDataLabelStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.Font.FontColor = System.Drawing.Color.DarkBlue;
            style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);

            return style;
        }

        public static SLStyle GetHeaderRowEmployeeDataTextStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.Border.LeftBorder.BorderStyle = BorderStyleValues.Dotted;
            style.Border.LeftBorder.Color = System.Drawing.Color.Gray;
            style.Border.RightBorder.BorderStyle = BorderStyleValues.Dotted;
            style.Border.RightBorder.Color = System.Drawing.Color.Gray;
            style.Border.TopBorder.BorderStyle = BorderStyleValues.Dotted;
            style.Border.TopBorder.Color = System.Drawing.Color.Gray;
            style.Border.BottomBorder.BorderStyle = BorderStyleValues.Dotted;
            style.Border.BottomBorder.Color = System.Drawing.Color.Gray;
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);

            return style;
        }

        public static SLStyle GetTopBorderStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 10;
            style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.TopBorder.Color = System.Drawing.Color.Black;

            return style;
        }
        public static SLStyle GetRedTopBorderStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Border.TopBorder.BorderStyle = BorderStyleValues.Thick;
            style.Border.TopBorder.Color = System.Drawing.Color.Red;

            return style;
        }


        public static SLStyle GetLeftBorderStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.LeftBorder.Color = System.Drawing.Color.Black;

            return style;
        }

        public static SLStyle GetRightBorderStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.RightBorder.Color = System.Drawing.Color.Black;

            return style;
        }

        public static SLStyle GeBottomBorderStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.BottomBorder.Color = System.Drawing.Color.Black;

            return style;
        }

        public static SLStyle FillColorStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(220, 230, 241), System.Drawing.Color.FromArgb(220, 230, 241));

            return style;
        }

        public static SLStyle GetFlightColumnHeaderStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(220, 230, 241), System.Drawing.Color.FromArgb(220, 230, 241));
            style.Font.FontSize = 9;
            style.Font.FontName = "Verdana";
            style.SetFontBold(true);
            style.SetWrapText(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            //style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            //style.Border.TopBorder.Color = System.Drawing.Color.Black;
            //style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            //style.Border.LeftBorder.Color = System.Drawing.Color.Black;
            //style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thick;
            //style.Border.BottomBorder.Color = System.Drawing.Color.Red;
            //style.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
            //style.Border.RightBorder.Color = System.Drawing.Color.Black;
            return style;
        }
        public static SLStyle GetShiftEntryDateStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(220, 230, 241), System.Drawing.Color.FromArgb(220, 230, 241));
            style.Font.FontSize = 10;
            style.Font.FontName = "Verdana";
            style.SetFontBold(true);
            style.SetWrapText(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.TopBorder.Color = System.Drawing.Color.Black;
            style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.LeftBorder.Color = System.Drawing.Color.Black;
            style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.BottomBorder.Color = System.Drawing.Color.Black;
            style.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.RightBorder.Color = System.Drawing.Color.Black;
            return style;
        }

        public static SLStyle GetTextWithAllBorderStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            //style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(220, 230, 241), System.Drawing.Color.FromArgb(220, 230, 241));
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            //style.SetFontBold(true);
            style.SetWrapText(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.TopBorder.Color = System.Drawing.Color.Black;
            style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.LeftBorder.Color = System.Drawing.Color.Black;
            style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.BottomBorder.Color = System.Drawing.Color.Black;
            style.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.RightBorder.Color = System.Drawing.Color.Black;
            return style;
        }

        public static SLStyle GetTextBoldwithoutBorderStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 10;
            style.Font.FontName = "Verdana";
            style.SetFontBold(true);
            style.SetWrapText(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            return style;
        }

        public static SLStyle GetTextBoldwithoutBorderAlignTopStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.SetFontBold(true);
            style.SetWrapText(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);
            style.SetVerticalAlignment(VerticalAlignmentValues.Top);
            return style;
        }

        public static SLStyle GetTextCaseStudyStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.SetWrapText(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);
            style.SetVerticalAlignment(VerticalAlignmentValues.Top);
            return style;
        }

        public static SLStyle GetTextAssessmentQuestionnaireStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 10;
            style.Font.FontName = "Verdana";
            style.SetWrapText(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            return style;
        }

        public static SLStyle GetHeaderStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(220, 230, 241), System.Drawing.Color.FromArgb(220, 230, 241));
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.TopBorder.Color = System.Drawing.Color.Black;
            style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.LeftBorder.Color = System.Drawing.Color.Black;
            style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.BottomBorder.Color = System.Drawing.Color.Black;
            style.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.RightBorder.Color = System.Drawing.Color.Black;
            return style;
        }

        public static SLStyle GetShiftWorkHrsStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(220, 230, 241), System.Drawing.Color.FromArgb(220, 230, 241));
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.SetWrapText(true);
            style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.TopBorder.Color = System.Drawing.Color.Black;
            style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.LeftBorder.Color = System.Drawing.Color.Black;


            return style;
        }

        public static SLStyle GetWrapTextStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();

            style.SetWrapText(true);

            return style;
        }

        public static SLStyle GetShiftWorkHrsEmptyStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(220, 230, 241), System.Drawing.Color.FromArgb(220, 230, 241));

            style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.LeftBorder.Color = System.Drawing.Color.Black;


            return style;
        }

        public static SLStyle GetShiftEntryDateEmptyStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(220, 230, 241), System.Drawing.Color.FromArgb(220, 230, 241));

            return style;
        }

        public static SLStyle GetShiftNoStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(220, 230, 241), System.Drawing.Color.FromArgb(220, 230, 241));
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Right);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.TopBorder.Color = System.Drawing.Color.Black;
            style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.LeftBorder.Color = System.Drawing.Color.Black;

            return style;
        }

        public static SLStyle GetShiftNoEmptyStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(220, 230, 241), System.Drawing.Color.FromArgb(220, 230, 241));

            style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.LeftBorder.Color = System.Drawing.Color.Black;

            return style;
        }

        public static SLStyle GetTimeStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(220, 230, 241), System.Drawing.Color.FromArgb(220, 230, 241));

            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.TopBorder.Color = System.Drawing.Color.Black;
            style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.LeftBorder.Color = System.Drawing.Color.Black;

            return style;
        }

        public static SLStyle GetPartInStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.FromArgb(220, 230, 241), System.Drawing.Color.FromArgb(220, 230, 241));
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.SetFontBold(true);
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Right);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.TopBorder.Color = System.Drawing.Color.Black;
            style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.LeftBorder.Color = System.Drawing.Color.Black;

            return style;
        }

        public static SLStyle GetEntryDayStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Right);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.TopBorder.Color = System.Drawing.Color.Black;
            style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.LeftBorder.Color = System.Drawing.Color.Black;
            style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.BottomBorder.Color = System.Drawing.Color.Black;
            style.Border.RightBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.RightBorder.Color = System.Drawing.Color.Black;

            return style;
        }

        public static SLStyle GetGrandTotalStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.Bold = true;
            style.Font.FontName = "Verdana";
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.TopBorder.Color = System.Drawing.Color.Black;
            style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.LeftBorder.Color = System.Drawing.Color.Black;

            return style;
        }

        public static SLStyle GetEntryDayDateStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.TopBorder.Color = System.Drawing.Color.Black;
            style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.LeftBorder.Color = System.Drawing.Color.Black;

            return style;
        }
        public static SLStyle GetEnteryTextWrapStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            style.Border.TopBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.TopBorder.Color = System.Drawing.Color.Black;
            style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.LeftBorder.Color = System.Drawing.Color.Black;
            //style.SetWrapText(true);
            style.Alignment.WrapText = true;

            return style;
        }
        public static SLStyle GetBottomBorderStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Right);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);
            style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.BottomBorder.Color = System.Drawing.Color.Red;
            //style.Border.LeftBorder.BorderStyle = BorderStyleValues.Thin;
            // style.Border.LeftBorder.Color = System.Drawing.Color.Black;

            return style;
        }

        public static SLStyle GetBottomBorderOnlyStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 10;
            style.Font.FontName = "Verdana";
            style.Border.BottomBorder.BorderStyle = BorderStyleValues.Thin;
            style.Border.BottomBorder.Color = System.Drawing.Color.Black;

            return style;
        }

        public static SLStyle GetNoBorderStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Left);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);


            return style;
        }
        public static SLStyle GetNoBorderCentreAlignStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Center);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);


            return style;
        }
        public static SLStyle GetNoBorderRightAlignStyle(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Right);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);


            return style;
        }
        public static SLStyle GetNoBorderRightAlignStyleWithBold(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            style.Font.Bold = true;
            style.SetHorizontalAlignment(HorizontalAlignmentValues.Right);
            style.SetVerticalAlignment(VerticalAlignmentValues.Center);


            return style;
        }
        public static SLStyle GetCurrencyStyleWith2Decimal(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.FormatCode = "#,##0.00";
            return style;
        }
        public static SLStyle SetFont(SLDocument sl)
        {
            var style = sl.CreateStyle();
            style.Font.FontSize = 7;
            style.Font.FontName = "Verdana";
            return style;
        }
        public static SLStyle SetFontColor(SLDocument sl, System.Drawing.Color color, bool isBold)
        {
            var style = sl.CreateStyle();
            style.Font.FontColor = color;
            style.Font.Bold = isBold;
            return style;
        }
        public static SLStyle SetFillColor(SLDocument sl, System.Drawing.Color color)
        {
            var style = sl.CreateStyle();
            style.Fill.SetPattern(PatternValues.Solid, color, color);
            return style;
        }

    }
}
