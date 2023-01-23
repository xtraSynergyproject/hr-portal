using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class CctnsViewModel
    {
        public string ZONE_NAME { get; set; }
        public string RANGE_NAME { get; set; }
        public string DISTRICT { get; set; }
        public string POLICE_STATION { get; set; }
        public DateTime REG_DT { get; set; }
        public string FIR_NUM { get; set; }
        public string ACT_SECTION { get; set; }
        public string FIR_OCCURANCE_PLACE { get; set; }
        public string FIR_COMPLINANT_DETAIL { get; set; }
        public string FIR_STATUS_TYPE { get; set; }
        
    }
    public class CctnsArrayViewModel
    {
        public string[] ZONE_NAME { get; set; }
        public string[] RANGE_NAME { get; set; }
        public string[] DISTRICT { get; set; }
        public string[] POLICE_STATION { get; set; }
        public string[] REG_DT { get; set; }
        public string[] FIR_NUM { get; set; }
        public string[] ACT_SECTION { get; set; }
        public string[] FIR_OCCURANCE_PLACE { get; set; }
        public string[] FIR_COMPLINANT_DETAIL { get; set; }
        public string[] FIR_STATUS_TYPE { get; set; }

    }
    public class CctnsKeywordViewModel
    {
        public string ZONE_NAME { get; set; }
        public string Zone_Name { get; set; }
        public string zone_name { get; set; }
        public string RANGE_NAME { get; set; }
        public string Range_Name { get; set; }
        public string range_name { get; set; }
        public string DISTRICT { get; set; }
        public string District { get; set; }
        public string district { get; set; }
        public string POLICE_STATION { get; set; }
        public string Police_Station { get; set; }
        public string police_station { get; set; }
        public string PS { get; set; }
        public string GANG_POLICE_STATION { get; set; }
        public string MEMBER_POLICE_STATION { get; set; }
        public string SUBMIT_POLICE_STATION_OFFICE { get; set; }
        public string _ZoneName {
            get
            {
                if (ZONE_NAME != null)
                {
                    return ZONE_NAME;
                }
                if (Zone_Name != null)
                {
                    return Zone_Name;
                }
                if (zone_name != null)
                {
                    return zone_name;
                }  
                return null;
            }
        }
        public string _RangeName {
            get
            {
                if (RANGE_NAME != null)
                {
                    return RANGE_NAME;
                }
                if (Range_Name != null)
                {
                    return Range_Name;
                }
                if (range_name != null)
                {
                    return range_name;
                }
                return null;
            }
        }
        public string _District {
            get
            {
                if (DISTRICT != null)
                {
                    return DISTRICT;
                }
                if (District != null)
                {
                    return District;
                }
                if (district != null)
                {
                    return district;
                }
                return null;
            }
        }
        public string _PoliceStation {
            get
            {
                if (POLICE_STATION != null)
                {
                    return POLICE_STATION;
                }
                if (Police_Station != null)
                {
                    return Police_Station;
                }
                if (police_station != null)
                {
                    return police_station;
                }
                if (PS != null)
                {
                    return PS;
                }
                if (GANG_POLICE_STATION != null)
                {
                    return GANG_POLICE_STATION;
                }
                if (MEMBER_POLICE_STATION != null)
                {
                    return MEMBER_POLICE_STATION;
                } 
                if (SUBMIT_POLICE_STATION_OFFICE != null)
                {
                    return SUBMIT_POLICE_STATION_OFFICE;
                }                 
                return null;
            }
        }
        
    }
    public class CctnsCommonViewModel
    {
        public string IndexName { get; set; }
        public string ZoneName { get; set; }
        public string RangeName { get; set; }
        public string District { get; set; }
        public string PoliceStation { get; set; }
        public DateTime ReportDate { get; set; }
        public string JsonString { get; set; }
        

    }
    public class CctnsCommonArrayViewModel
    {        
        public string[] JsonString { get; set; }


    }

}
