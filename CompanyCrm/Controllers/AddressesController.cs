using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CustomerModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CompanyCrm.Controllers
{
    public class AddressesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AddressesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<AddressModel[]>> List(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("CompanyCrmClient");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"addresses/GetByCustomer/{id}");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            //IEnumerable<AddressModel> addresses = JsonConvert.DeserializeObject<IEnumerable<AddressModel>>(content);  *** Converted from IEnum<AddrModel> to AddrModel[] for testing
            AddressModel[] addresses = JsonConvert.DeserializeObject<AddressModel[]>(content);

            return View(addresses);
        }

        [HttpGet]
        public async Task<ActionResult<AddressModel>> Details(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("CompanyCrmClient");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"addresses/{id}");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            AddressModel address = JsonConvert.DeserializeObject<AddressModel>(content);

            return View("Details", address);
        }

        [HttpGet]
        public async Task<ActionResult<AddressCreateViewModel>> Create(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("CompanyCrmClient");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "addresses/GetAllCountries");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            Country[] countries = JsonConvert.DeserializeObject<Country[]>(content);

            List<SelectListItem> countriesList = countries
           .OrderBy(c => c.Name)
           .Select(c => new SelectListItem
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

            IEnumerable<SelectListItem> regions = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = null,
                    Text = " "
                }
            };

        AddressCreateViewModel newAddress = new AddressCreateViewModel()
            {
                CustomerId = id,
                Countries = countriesList,
                Regions = regions
            };

            return View(newAddress);
        }

        [HttpGet]
        public async Task<ActionResult> GetRegions(string countryCode)
        {
            if (!string.IsNullOrWhiteSpace(countryCode) && countryCode.Length == 2)
            {
                var httpClient = _httpClientFactory.CreateClient("CompanyCrmClient");

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"addresses/GetRegions/{countryCode}");
                HttpResponseMessage response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                
                // need to find regions that match country code parameter
                Region[] associatedRegions = JsonConvert.DeserializeObject<Region[]>(content);

                // Change model.Countries into SelectList
                IEnumerable<SelectListItem> results = associatedRegions
                    .OrderBy(r => r.Name)
                    .Select(r => new SelectListItem
                    {
                        Value = r.Name,
                        Text = r.Name
                    })
                    .ToList();

                var regions = new SelectList(results, "Value", "Text");
                return Json(regions);
            }

            return null;
        }

        [HttpPost]
        public async Task<ActionResult<AddressModel>> Create(AddressModel address)
        {
            var httpClient = _httpClientFactory.CreateClient("CompanyCrmClient");
            
            string newAddress = JsonConvert.SerializeObject(address);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"addresses");
            request.Content = new StringContent(newAddress);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Details", "Customers", new { id = address.CustomerId} );
        }

        [HttpGet]
        public async Task<ActionResult<AddressModel>> Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("CompanyCrmClient");
            
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"addresses/{id}");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            AddressModel address = JsonConvert.DeserializeObject<AddressModel>(content);

            return View(address);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AddressModel address)
        {
            var httpClient = _httpClientFactory.CreateClient("CompanyCrmClient");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"addresses/{address.AddressId}");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Details", "Customers", new { id = address.CustomerId });
        }
    }
}