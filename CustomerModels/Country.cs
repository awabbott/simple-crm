using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerModels
{
    public class Country
    {
        [Key]
        public string CountryCode { get; set; }
        public string Name { get; set; }
        //public List<Region> Regions { get; set; }
    }
}
