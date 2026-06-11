using BuildIT2026_Graph.Core.Models;
using BuildIT2026_Graph.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildIT2026_Graph.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly INeo4jService _neo4jService;

        public CustomerController(INeo4jService neo4jService)
        {
            _neo4jService = neo4jService;
        }

        [HttpGet]
        public IActionResult GetCustomers()
        {
            var customers = _neo4jService.QueryAsync("MATCH (c:Customer) RETURN c LIMIT 50", new Dictionary<string, object?>()).Result;
            // Placeholder for actual data retrieval logic
            return Ok(customers);
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomer(string customerId)
        {
            var cypher = @"
            MATCH (c:Customer {customerId: $customerId})
            RETURN c";

            var parameters = new Dictionary<string, object> { { "customerId", customerId } };

            var result = await _neo4jService.QueryAsync(cypher, parameters);

            if (result == null || !result.Any())
                return NotFound();

            return Ok(result.First());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            var createnoderequest = customer.CreateNodeRequest();
            var newcustomer = await _neo4jService.CreateNodeAsync(createnoderequest.Label, createnoderequest.Properties);

            return Ok(newcustomer);   
        }

        //// ✅ UPDATE
        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateCustomer(string customerId, [FromBody] Customer customer)
        {
            if (customer == null)
                return BadRequest("Customer is null");

            var request = customer.CreateNodeRequest();

            var cypher = @"
            MERGE (c:Customer {customerId: $customerId})
            SET c += $properties
            RETURN c
        ";  

            var parameters = new Dictionary<string, object>
            {
                { "customerId", customerId },
                { "properties", request.Properties }
            };

            var result = await _neo4jService.UpdateNodeAsync(cypher, parameters);

            if (result == null || !result.Any())
                return NotFound();

            return Ok(result.First());
        }

        //[HttpDelete("{customerId}")]
        //public async Task<IActionResult> DeleteCustomer(string customerId)
        //{
        //    var cypher = @"
        //    MATCH (c:Customer {customerId: $customerId})
        //    DETACH DELETE c
        //";

        //    var parameters = new { customerId };

        //    await _neo4jService.ExecuteAsync(cypher, parameters);

        //    return NoContent();
        //}
    }
}
