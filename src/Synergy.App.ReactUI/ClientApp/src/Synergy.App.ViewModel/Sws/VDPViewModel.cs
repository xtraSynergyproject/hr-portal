using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{ 
    public class VDPViewModel : ExtraDetails
    {
        public string EventName { get; set; }
        public object Category { get; set; }
        public double DetConf { get; set; }
        public long DetTime { get; set; }
        public string DeviceName { get; set; }
        public int DeviceId { get; set; }
        public string DeviceIP { get; set; }
        public string FullImgPath { get; set; }
        public string PlateId { get; set; }
        public string PlateImgPath { get; set; }
        public string PlateNumber { get; set; }
        public string ViolationImgPath { get; set; }
        public bool RedLightViolated { get; set; }
        public string DriverImgPath { get; set; }
        public string Remark { get; set; }
        public double RecConf { get; set; }
        public bool Update { get; set; }
        public int Id { get; set; }
        public long ReceivedTime { get; set; }
        public string VehicleColor { get; set; }
        public int CategoryDataId { get; set; }
        public double Speed { get; set; }
        public bool SpeedViolated { get; set; }
        public object CategoryData { get; set; }
        public string VehicleRegistrationType { get; set; }
        public object VehicleType { get; set; }
        public object VehicleMake { get; set; }
        public object PlateType { get; set; }
        public bool NoHelmet { get; set; }
        public int NumberOfRiders { get; set; }
        public bool TrippleRiding { get; set; }
        public bool NoSeatBelt { get; set; }
        public bool IsDrivingWhileOnTheMobile { get; set; } 
        public string DistrictName { get; set; }
        public string IpAddress { get; set; }
        public DateTime Event_Date { get; set; }
    }
    public class ExtraDetails
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public object Location { get; set; }
        public object SerialNumber { get; set; }
        public int DistrictId { get; set; }
        public int RTOCircleNumber { get; set; }
    }
}
