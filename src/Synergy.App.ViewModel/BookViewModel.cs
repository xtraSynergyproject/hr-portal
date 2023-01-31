using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class BookViewModel
    {
        public bool Select { get; set; }
        public string Id { get; set; }
        public string BookName { get; set; }
        public string BookImage { get; set; }
        public string BookDescription { get; set; }

        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string HtmlContent { get; set; }
        public string HtmlCssField { get; set; }
        public string CurrentId { get; set; }
        public string PreviousId { get; set; }
        public string NextId { get; set; }
        public long? SequenceOrder { get; set; }
        public string PageName { get; set; }
        public string PageDescription { get; set; }
        public string ServiceId { get; set; }
        public string ParentServiceId { get; set; }
        public string ServiceNo { get; set; }
        public int TotalPages { get; set; }
        public int RatingCount { get; set; }
        public string File { get; set; }
        public string ContentType { get; set; }
        public string LastUpdatedDateDisplay
        {
            get
            {
                return LastUpdatedDate.HasValue ? LastUpdatedDate.ToDefaultDateFormat() : DateTime.Today.ToDefaultDateFormat();
            }
        }
        public bool IsNew
        {
            get
            {
                if (CreatedDate.HasValue && CreatedDate.Value.AddDays(7) > DateTime.Today)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool IsUpdated
        {
            get
            {
                if (IsNew == false && LastUpdatedDate.HasValue && LastUpdatedDate.Value.AddDays(7) > DateTime.Today)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public double Rating { get; set; }
        public int RatingRounded
        {
            get
            {
                return Convert.ToInt32(Math.Round(Rating));
            }
        }
        public int MyRating { get; set; }
        public string MyRatingId { get; set; }
        public string BookDescriptionText
        {
            get
            {
                if (BookDescription != null && BookDescription.Length > 150)
                {
                    return $"{BookDescription.Substring(0, 147)}...";
                }
                else
                {
                    return BookDescription;
                }
            }
        }
        public string DescriptionHoverClass
        {
            get
            {
                if (BookDescription != null && BookDescription.Length > 150)
                {
                    return "desc-popup-card";
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
