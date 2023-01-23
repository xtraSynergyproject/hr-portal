namespace Synergy.App.Api.Areas.EGov.Models
{
    public class PropertyViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Polygon { get; set; }
        public bool IsGarbageCollected { get; set; }
    }

    public class Ward
    {
        public string Id { get; set; }
        public string WardName { get; set; }

    }

    public class Locality
    {
        public string Id { get; set; }
        public string LocalityName { get; set; }
        public string WardId { get; set; }


    }

    public class SubLocality
    {
        public string Id { get; set; }
        public string SubLocalityName { get; set; }
        public string localityId { get; set; }

    }

}

