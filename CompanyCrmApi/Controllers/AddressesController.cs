using AutoMapper;
using CompanyCrmApi.Data;
using CompanyCrmApi.Data.Entities;
using CustomerModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using CompanyCrmApi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyCrmApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressRepository _repository;
        private readonly IMapper _mapper;

        public AddressesController(IAddressRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{addressId:int}")]
        public async Task<ActionResult<AddressModel>> Get(int addressId)
        {
            try
            {
                Address address = await _repository.GetAddressAsync(addressId);

                if (address == null) return NotFound($"Could not find address with ID: {addressId}");

                return _mapper.Map<AddressModel>(address);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("[action]/{customerId:int}")]
        public async Task<ActionResult<AddressModel[]>> GetByCustomer(int customerId)
        {
            try
            {
                Address[] addresses = await _repository.GetAllAddressesForCustomerAsync(customerId);

                if (addresses == null) return NotFound($"Could not find any addresses for Customer ID: {customerId}");
                
                return _mapper.Map<AddressModel[]>(addresses);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Country[]>> GetAllCountries()
        {
            try
            {
                Country[] countries = await _repository.GetAllCountriesAsync();

                return countries;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("[action]/{countryCode}")]
        public async Task<ActionResult<Region[]>> GetRegions(string countryCode)
        {
            try
            {
                var regions = await _repository.GetAllRegionsForCountryAsync(countryCode);

                return regions;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<AddressModel>> Post([FromBody] AddressModel model)
        {
            try
            {
                Address address = _mapper.Map<Address>(model);

                _repository.Add(address);

                if (await _repository.SaveChangesAsync())
                {
                    return CreatedAtAction(nameof(Get), new { addressId = address.AddressId }, address);
                }
                else
                {
                    return BadRequest("Failed to save new address");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
        }

        [HttpDelete("{addressId:int}")]
        public async Task<IActionResult> Delete(int addressId)
        {
            try
            {
                Address address = await _repository.GetAddressAsync(addressId);
                if (address == null) return NotFound("No address found");
                _repository.Delete(address);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Unable to delete address");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
        }
    }
}
