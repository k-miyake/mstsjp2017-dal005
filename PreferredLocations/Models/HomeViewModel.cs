using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreferredLocations.Models
{
    public class HomeViewModel
    {
        public string WriteRegion { get; set; }
        public  string ReadRegion { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}
