using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerModels
{
    public class Region
    {
        [Key]
        public int RegionId { get; set; }
        public string RegionCode { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        //public Country Country { get; set; }
    }
}
