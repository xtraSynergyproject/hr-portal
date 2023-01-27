using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
  
    public partial class NTS_TaskVersion : NTS_Task
    {
        public long VersionedByUserId { get; set; }
        public DateTime VersionedDate { get; set; }
    }
    public class R_TaskVersion_Task : RelationshipBase
    {

    }
    public class R_TaskVersion_ServiceVersion : RelationshipBase
    {

    }

    public class R_TaskVersion_Template : RelationshipBase
    {

    }
    public class R_TaskVersion_Owner_User : RelationshipBase
    {

    }
    public class R_TaskVersion_AssignedTo_User : RelationshipBase
    {

    }
    public class R_TaskVersion_AssignedTo_Team : RelationshipBase
    {

    }
    public class R_TaskVersion_Status_ListOfValue : RelationshipBase
    {

    }
    public class R_TaskVersion_Parent_Task : RelationshipBase
    {

    }
    //public class R_Task_Team : RelationshipBase
    //{

    //}
    public class R_TaskVersion_Holder_User : RelationshipBase
    {

    }
    public class R_TaskVersion_RequestedBy_User : RelationshipBase
    {

    }
    public class R_TaskVersion_Step_Service : RelationshipBase
    {
        public TaskCreationMode? CreationMode { get; set; }
    }
    public class R_TaskVersion_Adhoc_Service : RelationshipBase
    {

    }
    public class R_TaskVersion_ServiceTaskTemplate : RelationshipBase
    {

    }

    public class R_TaskVersion_SharedTo_User : RelationshipBase
    {

    }
    public class R_TaskVersion_SharedTo_Team : RelationshipBase
    {

    }

    public class R_TaskVersion_Reference : RelationshipBase
    {
        public ReferenceTypeEnum? ReferenceTypeCode { get; set; }
    }
    public class R_TaskVersion_ParentReference : RelationshipBase
    {

    }
}
