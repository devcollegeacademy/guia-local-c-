using Microsoft.AspNetCore.Mvc;
using guialocal.Models;
using guialocal.Services;
using FluentValidation;
using guialocal.Middlewares;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace guialocal.Controllers
{
    [Route("customer")]
    [ApiController]    
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;
        private readonly IValidator<Customer> _validator;
        private readonly HttpClient _client = new HttpClient();
        private IConfiguration _configuration { get; }


        public CustomerController(CustomerService customerService, IValidator<Customer> validator, IConfiguration configuration)
        {
            _customerService = customerService;
            _validator = validator;
            _configuration = configuration;
        }

        [HttpGet("auth")]
        public IActionResult Auth()
        {
            try
            {
                string authUri = _configuration.GetSection("Google:AuthUri").Value ?? throw new Exception("Auth Uri ausente");
                return Redirect(authUri);
            }
            
             catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [VerifyGoogle]
        public IActionResult Create(Customer customer)
        {
            var validationResult = new CustomerCreateValidator().Validate(customer);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var createdCustomer = _customerService.Create(customer);
                return Ok(createdCustomer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("filter/{title?}")]
        public IActionResult ReadAllByFilter(string? title)
        {
            try
            {
                var validationResult = new CustomerReadByFilterValidator().ValidateOrNull(title);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                var customers = _customerService.ReadByFilter(title);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }           
        }

        [HttpGet("{email}")]
        [VerifyGoogle]
        public IActionResult ReadOne(string email)
        {
            try
            {
                var validationResult = new CustomerReadOneValidator().Validate(email);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                var customers = _customerService.ReadOne(email);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{email}")]
        [VerifyGoogle]
        public IActionResult Update(string email, Customer customerData)
        {
            var validationResult = new CustomerUpdateValidator().Validate((email, customerData));
                     
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {             
                var updatedCustomer = _customerService.Update(email, customerData);
                return Ok(updatedCustomer);
            }
            catch (Exception ex)
            {             
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{email}")]
        [VerifyGoogle]
        public IActionResult Delete(string email)
        {
            var validationResult = new CustomerDeleteValidator().Validate(email);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                _customerService.Delete(email);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("address/{zipCode}")]
        [VerifyGoogle]
        public async Task<IActionResult> GetAddress(string zipCode)
        {
            try
            {
                var validationResult = new CustomerGetAddressValidator().Validate(zipCode);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
                                
                string apiKey = _configuration.GetSection("Google:ApiKey").Value ?? throw new Exception("API Key ausente");
                var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={zipCode}&key={apiKey}";
                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var resultJson = await response.Content.ReadAsStringAsync();                    
                    var resultObject = JsonConvert.DeserializeObject<dynamic>(resultJson);
                    if (resultObject != null)
                    {
                        var formattedAddress = resultObject.results[0].formatted_address.ToString();
                        return Ok(formattedAddress);
                    } else
                    {
                        return BadRequest("Endereço não encontrado.");
                    }
                } else
                {
                    return BadRequest("Erro ao acessar a API do Google Maps.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }


}
