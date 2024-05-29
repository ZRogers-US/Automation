using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpSpecFlow.Models
{
        public class LocationsResultsModel
        {
            public RequestInfo RequestInfo { get; set; }
            public int Locations_Total { get; set; }
            public int Locations_Total_Current_Page { get; set; }
            public int Page { get; set; }
            public int Limit { get; set; }
            public Location[] Locations { get; set; }
        }

        public class Location
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Full_Name { get; set; }
            public int Parent_Id { get; set; }
            public string Country_Code { get; set; }
            public int Reach { get; set; }
            public Cords Gps_Coordinates { get; set; }
        }

        public class Cords
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        public class RequestInfo
        {
            public bool success { get; set; }
        }
}
