using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyCrm.Models
{
    public class AddressDisplayViewModel
    {
        [Display(Name = "Address ID")]
        public int AddressId { get; set; }

        [Display(Name = "Street Name")]
        public string StreetLine1 { get; set; }

        [Display(Name = "Country")]
        public string CountryName { get; set; }

        [Display(Name = "State / Province / Region")]
        public string RegionName { get; set; }
    }
}
