using Microsoft.AspNetCore.Mvc;
using Para.Data.DapperRepository;

namespace Para.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerReportController : ControllerBase
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerReportController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerWithDetails(long customerId)
        {
            var customer = await _customerRepository.GetCustomerWithDetailsAsync(customerId);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }
    }
}
