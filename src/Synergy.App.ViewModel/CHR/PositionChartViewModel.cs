using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace Synergy.App.ViewModel
{
    public class PositionChartViewModel
    {
        public string Id { get; set; }
        public string NoteId { get; set; }
        public string PosHierarchyNoteId { get; set; }
        public string HierarchyId { get; set; }

        public string OrgHierarchyId { get; set; }

        public string ParentId { get; set; }

        public string PositionName { get; set; }

        public string OrganizationName { get; set; }
        public string OrganizationId { get; set; }

        public string DisplayName { get; set; }
        public string JobName { get; set; }
        public string GradeName { get; set; }
        public string JobId { get; set; }
        public string JobNoteId { get; set; }
        public string AssignmentNoteId { get; set; }
        public string PositionNoteId { get; set; }
        public string PersonNoteId { get; set; }
        public string PersonId { get; set; }

        public string PhotoId { get; set; }



        public long Level { get; set; }


        public bool HasChild { get; set; }
        public bool HasUser { get; set; }
        public long DirectChildCount { get; set; }

        public string UserId { get; set; }
        public string ReportingLine { get; set; }

        public bool? IsExpanded { get; set; }

        public bool? Collapsed { get; set; }

        public string CssClass { get; set; }
        public string PerformanceStageName { get; set; }
        public int GoalCount { get; set; }
        public int CompetencyCount { get; set; }
        public int TasksPendingCount { get; set; }
        public int TasksCompletedCount { get; set; }
        //public string CssClass
        //{
        //    get
        //    {
        //        return "org-node-1";
        //    }
        //}

    }
}
