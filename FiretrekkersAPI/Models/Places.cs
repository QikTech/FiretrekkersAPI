using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiretrekkersAPI.Models
{
    public class Places
    {
        public int PlaceId { get; set; }
        public string PlaceDate { get; set; }
        public string PlaceName { get; set; }
        public string PlaceArea { get; set; }
        public string PlaceElevation { get; set; }
        public string PlaceDescription { get; set; }
        public string PlaceCoverImage { get; set; }
        public string PlaceImages { get; set; }
    }
}
