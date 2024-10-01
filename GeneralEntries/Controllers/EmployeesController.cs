using GeneralEntries.DTOs;
using GeneralEntries.Helpers.Response;
using GeneralEntries.RepositoryLayer.InterfaceClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace GeneralEntries.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeLayer _employeeLayer;
        private readonly ILogger<EmployeesController> _logger;
        private readonly IDistributedCache _distributedCache;
        string RedisCacheKey = "EmployeeCon";

        public EmployeesController(IEmployeeLayer employeeLayer, ILogger<EmployeesController> logger, IDistributedCache distributedCache)
        {
            _employeeLayer = employeeLayer;
            _logger = logger;
            _distributedCache = distributedCache;
        }

        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetEmployeeDto>>>> GetEmployees(CancellationToken cancellationToken)
        {

            var serviceResponse = new ServiceResponse<IEnumerable<GetEmployeeDto>>();

            try
            {
                // Attempt to retrieve from cache
                var cachedData = await _distributedCache.GetStringAsync(RedisCacheKey, cancellationToken);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    serviceResponse.Value = JsonConvert.DeserializeObject<IEnumerable<GetEmployeeDto>>(cachedData);
                    serviceResponse.Status = true;
                    serviceResponse.Message = $"Fetched employee records from Redis.";

                    return Ok(serviceResponse);
                }

                // Fetch from database if cache is empty
                var result = await _employeeLayer.GetListAsync(cancellationToken);
                if (result.Status)
                {
                    var serializedResult = JsonConvert.SerializeObject(result.Value);
                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(1),
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6)
                    };
                    await _distributedCache.SetStringAsync(RedisCacheKey, serializedResult, cacheOptions, cancellationToken);

                    return Ok(result);
                }
                return BadRequest(result);

            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Employees list request was canceled.");
                return StatusCode(StatusCodes.Status499ClientClosedRequest, "Request was canceled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the employees list.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
            }
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> AddEmployee([FromBody] CreateEmployeeDto model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _employeeLayer.AddNewEmployeeAsync(model, cancellationToken);

                if (!response.Status)
                {
                    _logger.LogWarning("AddEmployee failed: {@Response}", response);
                    return BadRequest(response);
                }

                await _distributedCache.RemoveAsync(RedisCacheKey);

                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("AddEmployee request was canceled.");
                return StatusCode(StatusCodes.Status499ClientClosedRequest, "Request was canceled.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding employee: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> UpdateEmployee([FromBody] CreateEmployeeDto model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _employeeLayer.UpdateEmployeeAsync(model, cancellationToken);

                if (!response.Status)
                {
                    _logger.LogWarning("UpdateEmployee failed: {@Response}", response);
                    return BadRequest(response);
                }

                await _distributedCache.RemoveAsync(RedisCacheKey);

                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("UpdateEmployee request was canceled.");
                return StatusCode(StatusCodes.Status499ClientClosedRequest, "Request was canceled.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating employee: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetEmployeeById/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> GetEmployeeByIdAsync(int Id, CancellationToken cancellationToken)
        {
            if (Id <= 0)
            {
                return BadRequest(ApiMessages.InvalidInput);
            }

            try
            {
                var response = await _employeeLayer.GetIdByAsync(Id, cancellationToken);

                if (!response.Status)
                {
                    _logger.LogWarning("GetEmployeeByIdAsync failed for Id = {Id}. Response: {@Response}", Id, response);
                    return NotFound(response);
                }

                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("GetEmployeeByIdAsync request for Id = {Id} was canceled.", Id);
                return StatusCode(StatusCodes.Status499ClientClosedRequest, "Request was canceled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching employee with Id = {Id}.", Id);
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("DeleteId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteEmployeeAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                return BadRequest(ApiMessages.InvalidInput);
            }

            try
            {
                var response = await _employeeLayer.DeleteByIdAsync(id, cancellationToken);

                if (!response.Status)
                {
                    _logger.LogWarning("DeleteEmployee failed: {@Response}", response);
                    return BadRequest(response);
                }

                await _distributedCache.RemoveAsync(RedisCacheKey);

                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("DeleteEmployee request was canceled.");
                return StatusCode(StatusCodes.Status499ClientClosedRequest, "Request was canceled.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting employee: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPatch("PatchEmployee/{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> PatchEmployeeAsync(int Id, string empName, CancellationToken cancellationToken)
        {
            if (Id <= 0 || string.IsNullOrWhiteSpace(empName))
            {
                return BadRequest(ApiMessages.InvalidInput);
            }

            try
            {
                var response = await _employeeLayer.PatchEmployeeAsync(Id, empName, cancellationToken);

                if (!response.Status)
                {
                    _logger.LogWarning("PatchEmployeeAsync failed for Id = {Id}. Response: {@Response}", Id, response);
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("PatchEmployeeAsync request for Id = {Id} was canceled.", Id);
                return StatusCode(StatusCodes.Status499ClientClosedRequest, "Request was canceled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while patching employee with Id = {Id}.", Id);
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("RedisRefreshed")]
        public async Task<IActionResult> RefreshCache()
        {
            var serviceResponse = new ServiceResponse<bool>();

            try
            {
                await _distributedCache.RefreshAsync(RedisCacheKey);
                serviceResponse.Status = true;
                serviceResponse.Message = "Redis has been refreshed";
            }
            catch (Exception ex)
            {
                serviceResponse.Status = false;
                serviceResponse.Message = ex.Message;
            }

            return Ok(serviceResponse);
        }

    }
}
