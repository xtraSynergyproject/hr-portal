using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCLayerViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string GeoJson { get; set; }
    }


    public partial class GeoJson
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("features")]
        public List<Feature> Features { get; set; }
    }

    public partial class Feature
    {
        [JsonProperty("type")]
        public FeatureType Type { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("geometry")]
        public dynamic Geometry { get; set; }
    }

    public partial class Geometry
    {
        [JsonProperty("type")]
        public GeometryType Type { get; set; }

        [JsonProperty("coordinates")]
        public Coordinate[][][] Coordinates { get; set; }
    }

    public partial class Properties
    {
        [JsonProperty("scalerank")]
        public long Scalerank { get; set; }

        [JsonProperty("featurecla")]
        public Featurecla Featurecla { get; set; }

        [JsonProperty("labelrank")]
        public long Labelrank { get; set; }

        [JsonProperty("sovereignt")]
        public string Sovereignt { get; set; }

        [JsonProperty("sov_a3")]
        public string SovA3 { get; set; }

        [JsonProperty("adm0_dif")]
        public long Adm0Dif { get; set; }

        [JsonProperty("level")]
        public long Level { get; set; }

        [JsonProperty("type")]
        public PropertiesType Type { get; set; }

        [JsonProperty("admin")]
        public string Admin { get; set; }

        [JsonProperty("adm0_a3")]
        public string Adm0A3 { get; set; }

        [JsonProperty("geou_dif")]
        public long GeouDif { get; set; }

        [JsonProperty("geounit")]
        public string Geounit { get; set; }

        [JsonProperty("gu_a3")]
        public string GuA3 { get; set; }

        [JsonProperty("su_dif")]
        public long SuDif { get; set; }

        [JsonProperty("subunit")]
        public string Subunit { get; set; }

        [JsonProperty("su_a3")]
        public string SuA3 { get; set; }

        [JsonProperty("brk_diff")]
        public long BrkDiff { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("name_long")]
        public string NameLong { get; set; }

        [JsonProperty("brk_a3")]
        public string BrkA3 { get; set; }

        [JsonProperty("brk_name")]
        public string BrkName { get; set; }

        [JsonProperty("brk_group")]
        public object BrkGroup { get; set; }

        [JsonProperty("abbrev")]
        public string Abbrev { get; set; }

        [JsonProperty("postal")]
        public string Postal { get; set; }

        [JsonProperty("formal_en")]
        public string FormalEn { get; set; }

        [JsonProperty("formal_fr")]
        public string FormalFr { get; set; }

        [JsonProperty("note_adm0")]
        public string NoteAdm0 { get; set; }

        [JsonProperty("note_brk")]
        public string NoteBrk { get; set; }

        [JsonProperty("name_sort")]
        public string NameSort { get; set; }

        [JsonProperty("name_alt")]
        public string NameAlt { get; set; }

        [JsonProperty("mapcolor7")]
        public long Mapcolor7 { get; set; }

        [JsonProperty("mapcolor8")]
        public long Mapcolor8 { get; set; }

        [JsonProperty("mapcolor9")]
        public long Mapcolor9 { get; set; }

        [JsonProperty("mapcolor13")]
        public long Mapcolor13 { get; set; }

        [JsonProperty("pop_est")]
        public long PopEst { get; set; }

        [JsonProperty("gdp_md_est")]
        public double GdpMdEst { get; set; }

        [JsonProperty("pop_year")]
        public long PopYear { get; set; }

        [JsonProperty("lastcensus")]
        public long Lastcensus { get; set; }

        [JsonProperty("gdp_year")]
        public long GdpYear { get; set; }

        [JsonProperty("economy")]
        public Economy Economy { get; set; }

        [JsonProperty("income_grp")]
        public IncomeGrp IncomeGrp { get; set; }

        [JsonProperty("wikipedia")]
        public long Wikipedia { get; set; }

        [JsonProperty("fips_10")]
        public object Fips10 { get; set; }

        [JsonProperty("iso_a2")]
        public string IsoA2 { get; set; }

        [JsonProperty("iso_a3")]
        public string IsoA3 { get; set; }

        [JsonProperty("iso_n3")]
        public string IsoN3 { get; set; }

        [JsonProperty("un_a3")]
        public string UnA3 { get; set; }

        [JsonProperty("wb_a2")]
        public string WbA2 { get; set; }

        [JsonProperty("wb_a3")]
        public string WbA3 { get; set; }

        [JsonProperty("woe_id")]
        public long WoeId { get; set; }

        [JsonProperty("adm0_a3_is")]
        public string Adm0A3Is { get; set; }

        [JsonProperty("adm0_a3_us")]
        public string Adm0A3Us { get; set; }

        [JsonProperty("adm0_a3_un")]
        public long Adm0A3Un { get; set; }

        [JsonProperty("adm0_a3_wb")]
        public long Adm0A3Wb { get; set; }

        [JsonProperty("continent")]
        public Continent Continent { get; set; }

        [JsonProperty("region_un")]
        public Continent RegionUn { get; set; }

        [JsonProperty("subregion")]
        public string Subregion { get; set; }

        [JsonProperty("region_wb")]
        public RegionWb RegionWb { get; set; }

        [JsonProperty("name_len")]
        public long NameLen { get; set; }

        [JsonProperty("long_len")]
        public long LongLen { get; set; }

        [JsonProperty("abbrev_len")]
        public long AbbrevLen { get; set; }

        [JsonProperty("tiny")]
        public long Tiny { get; set; }

        [JsonProperty("homepart")]
        public long Homepart { get; set; }
    }

    public enum GeometryType { MultiPolygon, Polygon };

    public enum Continent { Africa, Americas, Antarctica, Asia, Europe, NorthAmerica, Oceania, SevenSeasOpenOcean, SouthAmerica };

    public enum Economy { The1DevelopedRegionG7, The2DevelopedRegionNonG7, The3EmergingRegionBric, The4EmergingRegionMikt, The5EmergingRegionG20, The6DevelopingRegion, The7LeastDevelopedRegion };

    public enum Featurecla { Admin0Country };

    public enum IncomeGrp { The1HighIncomeOecd, The2HighIncomeNonOecd, The3UpperMiddleIncome, The4LowerMiddleIncome, The5LowIncome };

    public enum RegionWb { Antarctica, EastAsiaPacific, EuropeCentralAsia, LatinAmericaCaribbean, MiddleEastNorthAfrica, NorthAmerica, SouthAsia, SubSaharanAfrica };

    public enum PropertiesType { Country, Dependency, Disputed, Indeterminate, SovereignCountry };

    public enum FeatureType { Feature };

    public partial struct Coordinate
    {
        public double? Double;
        public double[] DoubleArray;

        public static implicit operator Coordinate(double Double) => new Coordinate { Double = Double };
        public static implicit operator Coordinate(double[] DoubleArray) => new Coordinate { DoubleArray = DoubleArray };
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                CoordinateConverter.Singleton,
                GeometryTypeConverter.Singleton,
                ContinentConverter.Singleton,
                EconomyConverter.Singleton,
                FeatureclaConverter.Singleton,
                IncomeGrpConverter.Singleton,
                RegionWbConverter.Singleton,
                PropertiesTypeConverter.Singleton,
                FeatureTypeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class CoordinateConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Coordinate) || t == typeof(Coordinate?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                case JsonToken.Float:
                    var doubleValue = serializer.Deserialize<double>(reader);
                    return new Coordinate { Double = doubleValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<double[]>(reader);
                    return new Coordinate { DoubleArray = arrayValue };
            }
            throw new Exception("Cannot unmarshal type Coordinate");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Coordinate)untypedValue;
            if (value.Double != null)
            {
                serializer.Serialize(writer, value.Double.Value);
                return;
            }
            if (value.DoubleArray != null)
            {
                serializer.Serialize(writer, value.DoubleArray);
                return;
            }
            throw new Exception("Cannot marshal type Coordinate");
        }

        public static readonly CoordinateConverter Singleton = new CoordinateConverter();
    }

    internal class GeometryTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(GeometryType) || t == typeof(GeometryType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "MultiPolygon":
                    return GeometryType.MultiPolygon;
                case "Polygon":
                    return GeometryType.Polygon;
            }
            throw new Exception("Cannot unmarshal type GeometryType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (GeometryType)untypedValue;
            switch (value)
            {
                case GeometryType.MultiPolygon:
                    serializer.Serialize(writer, "MultiPolygon");
                    return;
                case GeometryType.Polygon:
                    serializer.Serialize(writer, "Polygon");
                    return;
            }
            throw new Exception("Cannot marshal type GeometryType");
        }

        public static readonly GeometryTypeConverter Singleton = new GeometryTypeConverter();
    }

    internal class ContinentConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Continent) || t == typeof(Continent?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Africa":
                    return Continent.Africa;
                case "Americas":
                    return Continent.Americas;
                case "Antarctica":
                    return Continent.Antarctica;
                case "Asia":
                    return Continent.Asia;
                case "Europe":
                    return Continent.Europe;
                case "North America":
                    return Continent.NorthAmerica;
                case "Oceania":
                    return Continent.Oceania;
                case "Seven seas (open ocean)":
                    return Continent.SevenSeasOpenOcean;
                case "South America":
                    return Continent.SouthAmerica;
            }
            throw new Exception("Cannot unmarshal type Continent");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Continent)untypedValue;
            switch (value)
            {
                case Continent.Africa:
                    serializer.Serialize(writer, "Africa");
                    return;
                case Continent.Americas:
                    serializer.Serialize(writer, "Americas");
                    return;
                case Continent.Antarctica:
                    serializer.Serialize(writer, "Antarctica");
                    return;
                case Continent.Asia:
                    serializer.Serialize(writer, "Asia");
                    return;
                case Continent.Europe:
                    serializer.Serialize(writer, "Europe");
                    return;
                case Continent.NorthAmerica:
                    serializer.Serialize(writer, "North America");
                    return;
                case Continent.Oceania:
                    serializer.Serialize(writer, "Oceania");
                    return;
                case Continent.SevenSeasOpenOcean:
                    serializer.Serialize(writer, "Seven seas (open ocean)");
                    return;
                case Continent.SouthAmerica:
                    serializer.Serialize(writer, "South America");
                    return;
            }
            throw new Exception("Cannot marshal type Continent");
        }

        public static readonly ContinentConverter Singleton = new ContinentConverter();
    }

    internal class EconomyConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Economy) || t == typeof(Economy?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "1. Developed region: G7":
                    return Economy.The1DevelopedRegionG7;
                case "2. Developed region: nonG7":
                    return Economy.The2DevelopedRegionNonG7;
                case "3. Emerging region: BRIC":
                    return Economy.The3EmergingRegionBric;
                case "4. Emerging region: MIKT":
                    return Economy.The4EmergingRegionMikt;
                case "5. Emerging region: G20":
                    return Economy.The5EmergingRegionG20;
                case "6. Developing region":
                    return Economy.The6DevelopingRegion;
                case "7. Least developed region":
                    return Economy.The7LeastDevelopedRegion;
            }
            throw new Exception("Cannot unmarshal type Economy");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Economy)untypedValue;
            switch (value)
            {
                case Economy.The1DevelopedRegionG7:
                    serializer.Serialize(writer, "1. Developed region: G7");
                    return;
                case Economy.The2DevelopedRegionNonG7:
                    serializer.Serialize(writer, "2. Developed region: nonG7");
                    return;
                case Economy.The3EmergingRegionBric:
                    serializer.Serialize(writer, "3. Emerging region: BRIC");
                    return;
                case Economy.The4EmergingRegionMikt:
                    serializer.Serialize(writer, "4. Emerging region: MIKT");
                    return;
                case Economy.The5EmergingRegionG20:
                    serializer.Serialize(writer, "5. Emerging region: G20");
                    return;
                case Economy.The6DevelopingRegion:
                    serializer.Serialize(writer, "6. Developing region");
                    return;
                case Economy.The7LeastDevelopedRegion:
                    serializer.Serialize(writer, "7. Least developed region");
                    return;
            }
            throw new Exception("Cannot marshal type Economy");
        }

        public static readonly EconomyConverter Singleton = new EconomyConverter();
    }

    internal class FeatureclaConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Featurecla) || t == typeof(Featurecla?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Admin-0 country")
            {
                return Featurecla.Admin0Country;
            }
            throw new Exception("Cannot unmarshal type Featurecla");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Featurecla)untypedValue;
            if (value == Featurecla.Admin0Country)
            {
                serializer.Serialize(writer, "Admin-0 country");
                return;
            }
            throw new Exception("Cannot marshal type Featurecla");
        }

        public static readonly FeatureclaConverter Singleton = new FeatureclaConverter();
    }

    internal class IncomeGrpConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(IncomeGrp) || t == typeof(IncomeGrp?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "1. High income: OECD":
                    return IncomeGrp.The1HighIncomeOecd;
                case "2. High income: nonOECD":
                    return IncomeGrp.The2HighIncomeNonOecd;
                case "3. Upper middle income":
                    return IncomeGrp.The3UpperMiddleIncome;
                case "4. Lower middle income":
                    return IncomeGrp.The4LowerMiddleIncome;
                case "5. Low income":
                    return IncomeGrp.The5LowIncome;
            }
            throw new Exception("Cannot unmarshal type IncomeGrp");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (IncomeGrp)untypedValue;
            switch (value)
            {
                case IncomeGrp.The1HighIncomeOecd:
                    serializer.Serialize(writer, "1. High income: OECD");
                    return;
                case IncomeGrp.The2HighIncomeNonOecd:
                    serializer.Serialize(writer, "2. High income: nonOECD");
                    return;
                case IncomeGrp.The3UpperMiddleIncome:
                    serializer.Serialize(writer, "3. Upper middle income");
                    return;
                case IncomeGrp.The4LowerMiddleIncome:
                    serializer.Serialize(writer, "4. Lower middle income");
                    return;
                case IncomeGrp.The5LowIncome:
                    serializer.Serialize(writer, "5. Low income");
                    return;
            }
            throw new Exception("Cannot marshal type IncomeGrp");
        }

        public static readonly IncomeGrpConverter Singleton = new IncomeGrpConverter();
    }

    internal class RegionWbConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(RegionWb) || t == typeof(RegionWb?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Antarctica":
                    return RegionWb.Antarctica;
                case "East Asia & Pacific":
                    return RegionWb.EastAsiaPacific;
                case "Europe & Central Asia":
                    return RegionWb.EuropeCentralAsia;
                case "Latin America & Caribbean":
                    return RegionWb.LatinAmericaCaribbean;
                case "Middle East & North Africa":
                    return RegionWb.MiddleEastNorthAfrica;
                case "North America":
                    return RegionWb.NorthAmerica;
                case "South Asia":
                    return RegionWb.SouthAsia;
                case "Sub-Saharan Africa":
                    return RegionWb.SubSaharanAfrica;
            }
            throw new Exception("Cannot unmarshal type RegionWb");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (RegionWb)untypedValue;
            switch (value)
            {
                case RegionWb.Antarctica:
                    serializer.Serialize(writer, "Antarctica");
                    return;
                case RegionWb.EastAsiaPacific:
                    serializer.Serialize(writer, "East Asia & Pacific");
                    return;
                case RegionWb.EuropeCentralAsia:
                    serializer.Serialize(writer, "Europe & Central Asia");
                    return;
                case RegionWb.LatinAmericaCaribbean:
                    serializer.Serialize(writer, "Latin America & Caribbean");
                    return;
                case RegionWb.MiddleEastNorthAfrica:
                    serializer.Serialize(writer, "Middle East & North Africa");
                    return;
                case RegionWb.NorthAmerica:
                    serializer.Serialize(writer, "North America");
                    return;
                case RegionWb.SouthAsia:
                    serializer.Serialize(writer, "South Asia");
                    return;
                case RegionWb.SubSaharanAfrica:
                    serializer.Serialize(writer, "Sub-Saharan Africa");
                    return;
            }
            throw new Exception("Cannot marshal type RegionWb");
        }

        public static readonly RegionWbConverter Singleton = new RegionWbConverter();
    }

    internal class PropertiesTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(PropertiesType) || t == typeof(PropertiesType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Country":
                    return PropertiesType.Country;
                case "Dependency":
                    return PropertiesType.Dependency;
                case "Disputed":
                    return PropertiesType.Disputed;
                case "Indeterminate":
                    return PropertiesType.Indeterminate;
                case "Sovereign country":
                    return PropertiesType.SovereignCountry;
            }
            throw new Exception("Cannot unmarshal type PropertiesType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (PropertiesType)untypedValue;
            switch (value)
            {
                case PropertiesType.Country:
                    serializer.Serialize(writer, "Country");
                    return;
                case PropertiesType.Dependency:
                    serializer.Serialize(writer, "Dependency");
                    return;
                case PropertiesType.Disputed:
                    serializer.Serialize(writer, "Disputed");
                    return;
                case PropertiesType.Indeterminate:
                    serializer.Serialize(writer, "Indeterminate");
                    return;
                case PropertiesType.SovereignCountry:
                    serializer.Serialize(writer, "Sovereign country");
                    return;
            }
            throw new Exception("Cannot marshal type PropertiesType");
        }

        public static readonly PropertiesTypeConverter Singleton = new PropertiesTypeConverter();
    }

    internal class FeatureTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(FeatureType) || t == typeof(FeatureType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Feature")
            {
                return FeatureType.Feature;
            }
            throw new Exception("Cannot unmarshal type FeatureType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (FeatureType)untypedValue;
            if (value == FeatureType.Feature)
            {
                serializer.Serialize(writer, "Feature");
                return;
            }
            throw new Exception("Cannot marshal type FeatureType");
        }

        public static readonly FeatureTypeConverter Singleton = new FeatureTypeConverter();
    }
}
