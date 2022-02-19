using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerModels
{
    public class AddressModel
    {
        public int AddressId { get; set; }

        [Required]
        public AddressType AddressType { get; set; }

        [Required]
        [MaxLength(50)]
        public string StreetLine1 { get; set; }

        [MaxLength(50)]
        public string StreetLine2 { get; set; }

        [Required]
        [MaxLength(25)]
        public string City { get; set; }

        [Required]
        [MaxLength(25)]
        [FromForm(Name = "SelectedRegion")]
        public string State { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [Required]
        [FromForm(Name = "SelectedCountry")]
        public string Country { get; set; }

        [Required]
        public int CustomerId { get; set; }
    }
}
