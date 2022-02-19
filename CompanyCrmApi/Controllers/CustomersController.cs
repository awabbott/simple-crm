using AutoMapper;
using CompanyCrmApi.Data.Repositories;
using CompanyCrmApi.Data.Entities;
using CustomerModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace CompanyCrmApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private const bool IncludeAddresses = true;
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerModel[]>> Get()
        {
            try
            {
                Customer[] customerList = await _repository.GetAllCustomersAsync();

                CustomerModel[] customerModelList = _mapper.Map<CustomerModel[]>(customerList);

                return customerModelList;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
        }

        [HttpGet("{customerId:int}")]
        public async Task<ActionResult<CustomerModel>> Get(int customerId)
        {
            try
            {
                Customer customer = await _repository.GetCustomerAsync(customerId, IncludeAddresses);

                if (customer == null)
                {
                    return NotFound($"Could not find customer with ID: {customerId}");
                }

                CustomerModel customerModel = _mapper.Map<CustomerModel>(customer);
                
                return customerModel;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<CustomerModel[]>> Get(string userId)
        {
            try
            {
                Customer[] customers = await _repository.GetAllCustomersForUserAsync(userId);

                if (customers == null) return NotFound($"Could not find any customers for User ID: {userId}");

                return _mapper.Map<CustomerModel[]>(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomerModel>> Post([FromBody] CustomerModel model)
        {
            try
            {
                Customer customer = _mapper.Map<Customer>(model);

                _repository.Add(customer);

                if (await _repository.SaveChangesAsync())
                {
                    return CreatedAtAction(nameof(Get), new { customerId = customer.CustomerId }, customer);
                }
                else
                {
                    return BadRequest("Failed to save new customer");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
        }

        [HttpPut("{customerId:int}")]
        public async Task<ActionResult<CustomerModel>> Put(int customerId, [FromBody] CustomerModel model)
        {
            try
            {
                Customer customer = await _repository.GetCustomerAsync(customerId);

                if (customer == null) return NotFound($"Customer with ID {customerId} not found");

                _mapper.Map(model, customer);

                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<CustomerModel>(customer);
                }

                else
                {
                    return BadRequest("Unable to update Customer");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
        }

        [HttpDelete("{customerId:int}")]
        public async Task<IActionResult> Delete(int customerId)
        {
            try
            {
                Customer customer = await _repository.GetCustomerAsync(customerId);

                if (customer == null)
                {
                    return NotFound($"Could not find customer with ID {customerId}");
                }
                
                _repository.Delete(customer);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Failed to delete customer");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
        }
    }
}
