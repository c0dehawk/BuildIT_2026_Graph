using BuildIT2026_Graph.Core.Models;
using BuildIT2026_Graph.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildIT2026_Graph.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        private readonly INeo4jService _neo4jService;

        public GraphController(INeo4jService neo4jService)
        {
            _neo4jService = neo4jService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNodes()
        {
            try
            {
                var result = await _neo4jService.QueryAsync("MATCH (n) RETURN n LIMIT 50", new Dictionary<string, object?>());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("nodes")]
        public async Task<IActionResult> CreateNode([FromBody] CreateNodeRequest request)
        {
            try
            {
                var result = await _neo4jService.CreateNodeAsync(request.Label, request.Properties);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("relationships")]
        public async Task<IActionResult> CreateRelationship([FromBody] CreateRelationshipRequest request)
        {
            try
            {
                var result = await _neo4jService.CreateRelationshipAsync(
                    request.FromLabel,
                    request.FromKey,
                    request.FromValue,
                    request.ToLabel,
                    request.ToKey,
                    request.ToValue,
                    request.RelationshipType,
                    request.Properties);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("query")]
        public async Task<IActionResult> Query([FromBody] QueryRequest request)
        {
            try
            {
                var result = await _neo4jService.QueryAsync(request.Cypher, request.Parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("nodes/find")]
        public async Task<IActionResult> FindNodes([FromBody] FindNodesRequest request)
        {
            try
            {
                var result = await _neo4jService.FindNodesAsync(
                    request.Label,
                    request.PropertyName,
                    request.PropertyValue);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
