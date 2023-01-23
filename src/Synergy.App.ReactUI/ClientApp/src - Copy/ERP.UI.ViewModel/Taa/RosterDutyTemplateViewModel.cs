using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class RosterDutyTemplateViewModel : ViewModelBase {

        public string Name { get; set; }
        public bool Duty1Enabled { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? Duty1StartTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? Duty1EndTime { get; set; }      
        public bool Duty1FallsNextDay { get; set; }

        public bool Duty2Enabled { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? Duty2StartTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? Duty2EndTime { get; set; }
        public bool Duty2FallsNextDay { get; set; }

        public bool Duty3Enabled { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? Duty3StartTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultTimeFormat)]
        public TimeSpan? Duty3EndTime { get; set; }
        public bool Duty3FallsNextDay { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }

        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }

        [Display(Name = "Duty1 Start Time")]
        public string Duty1StartTimeText
        {
            get
            {
                if (Duty1StartTime != null)
                {
                    TimeSpan t = TimeSpan.Parse(Duty1StartTime.ToString());
                    return string.Format("{0}:{1}", t.Hours, t.Minutes);
                }
                return "";
            }
        }
        [Display(Name = "Duty2 Start Time")]
        public string Duty2StartTimeText
        {
            get
            {
                if (Duty2StartTime != null)
                {
                    TimeSpan t = TimeSpan.Parse(Duty2StartTime.ToString());
                    return string.Format("{0}:{1}", t.Hours, t.Minutes);
                }
                return "";
            }
        }
        [Display(Name = "Duty3 Start Time")]
        public string Duty3StartTimeText
        {
            get
            {
                if (Duty3StartTime != null)
                {
                    TimeSpan t = TimeSpan.Parse(Duty3StartTime.ToString());
                    return string.Format("{0}:{1}", t.Hours, t.Minutes);
                }
                return "";
            }
        }
        [Display(Name = "Duty1 End Time")]
        public string Duty1EndTimeText
        {
            get
            {
                if (Duty1EndTime != null)
                {
                    TimeSpan t = TimeSpan.Parse(Duty1EndTime.ToString());
                    return string.Format("{0}:{1}", t.Hours, t.Minutes);
                }
                return "";
            }
        }
        [Display(Name = "Duty2 End Time")]
        public string Duty2EndTimeText
        {
            get
            {
                if (Duty2EndTime != null)
                {
                    TimeSpan t = TimeSpan.Parse(Duty2EndTime.ToString());
                    return string.Format("{0}:{1}", t.Hours, t.Minutes);
                }
                return "";
            }
        }
        [Display(Name = "Duty3 End Time")]
        public string Duty3EndTimeText
        {
            get
            {
                if (Duty3EndTime != null)
                {
                    TimeSpan t = TimeSpan.Parse(Duty3EndTime.ToString());
                    return string.Format("{0}:{1}", t.Hours, t.Minutes);
                }
                return "";
            }
        }

        public string Duty1StartTimeVal { get; set; }
        public string Duty1EndTimeVal { get; set; }
        public string Duty2StartTimeVal { get; set; }
        public string Duty2EndTimeVal { get; set; }
        public string Duty3StartTimeVal { get; set; }
        public string Duty3EndTimeVal { get; set; }
    }
}
