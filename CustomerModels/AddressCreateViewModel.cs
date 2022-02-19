using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CustomerModels
{
    public class AddressCreateViewModel
    {
        public int AddressId { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Address Type")]
        public AddressType AddressType { get; set; }

        [Required]
        [Display(Name = "Street Line 1")]
        [StringLength(50)]
        public string StreetLine1 { get; set; }

        [Display(Name = "Street Line 2")]
        [StringLength(50)]
        public string StreetLine2 { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string SelectedCountry { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }

        [Required]
        [Display(Name = "State / Region")]
        public string SelectedRegion { get; set; }
        public IEnumerable<SelectListItem> Regions { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }


        public IEnumerable<SelectListItem> ConvertCountriesToSelectList(IEnumerable<Country> countries)
        {
            List<SelectListItem> countriesList = countries
                .OrderBy(c => c.Name)
                .Select(c =>
                    new SelectListItem
                    {
                        Value = c.CountryCode.ToString(),
                        Text = c.Name
                    })
                .ToList();
            var countrytip = new SelectListItem()
            {
                Value = null,
                Text = "--- select country ---"
            };
            countriesList.Insert(0, countrytip);

            return new SelectList(countries, "Value", "Text");
        }
    }
}
