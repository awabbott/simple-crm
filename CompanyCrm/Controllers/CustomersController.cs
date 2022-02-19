using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CustomerModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CompanyCrm.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomersController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerModel[]>> List()
        {
            var httpClient = _httpClientFactory.CreateClient("CompanyCrmClient");

            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"customers/{userId}");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            IEnumerable<CustomerModel> customers = JsonConvert.DeserializeObject<IEnumerable<CustomerModel>>(content);

            return View(customers);
        }

        [HttpGet]
        public async Task<ActionResult<CustomerModel>> Details(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("CompanyCrmClient");
         
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"customers/{id}");
            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            CustomerModel customer = JsonConvert.DeserializeObject<CustomerModel>(content);

            return View("Details", customer);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<CustomerModel>> Create(CustomerModel newCustomer)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("CompanyCrmClient");
                
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                newCustomer.UserId = userId;

                string customerString = JsonConvert.SerializeObject(newCustomer);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "customers");
                request.Content = new StringContent(customerString);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult<CustomerModel>> Update(int id)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("CompanyCrmClient");
                
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"customers/{id}");
                HttpResponseMessage response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                CustomerModel customer = JsonConvert.DeserializeObject<CustomerModel>(content);

                return View(customer);
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CustomerModel customer)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("CompanyCrmClient");

                string customerToUpdate = JsonConvert.SerializeObject(customer);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"customers/{customer.CustomerId}");
                request.Content = new StringContent(customerToUpdate);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                CustomerModel updatedCustomer = JsonConvert.DeserializeObject<CustomerModel>(content);

                return View("Details", updatedCustomer);
            }
            catch
            {
                return View("Update");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("CompanyCrmClient");

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"customers/{id}");
                HttpResponseMessage response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                CustomerModel customer = JsonConvert.DeserializeObject<CustomerModel>(content);

                return View(customer);
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("CompanyCrmClient");

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"customers/{id}");
                HttpResponseMessage response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return RedirectToAction("List");
            }
            catch
            {
                return View("Delete");
            }
        }
    }
}
