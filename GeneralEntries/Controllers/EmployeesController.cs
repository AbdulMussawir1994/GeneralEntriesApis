using GeneralEntries.DTOs;
using GeneralEntries.Helpers.Response;
using GeneralEntries.RepositoryLayer.InterfaceClass;
using Microsoft.AspNetCore.Mvc;

namespace GeneralEntries.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeLayer _employeeLayer;

        public EmployeesController(IEmployeeLayer employeeLayer)
        {
            _employeeLayer = employeeLayer;
        }

        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<GetEmployeeDto>>> GetEmployees(CancellationToken cancellationToken)
        {
            try
            {
                //  await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Cancelled");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Request was canceled.");
                }

                var result = await _employeeLayer.GetListAsync(cancellationToken);

                if (!result.Status)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, result);
                }
                return Ok(result);

            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Cancelled");
                return StatusCode(StatusCodes.Status500InternalServerError, "Request was canceled.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetEmployeeDto>> AddEmployee([FromBody] CreateEmployeeDto model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Cancelled");
                return StatusCode(StatusCodes.Status500InternalServerError, "Request was canceled.");
            }

            var response = await _employeeLayer.AddNewEmployeeAsync(model, cancellationToken);

            if (!response.Status)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetEmployeeDto>> UpdateEmployee([FromBody] CreateEmployeeDto model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _employeeLayer.UpdateEmployeeAsync(model, cancellationToken);

            if (!response.Status)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        [HttpGet("GetId/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetEmployeeDto>> GetEmployeeByIdAsync(int Id, CancellationToken cancellationToken)
        {
            if (Id <= 0)
            {
                return BadRequest(ApiMessages.InvalidInput);
            }

            var response = await _employeeLayer.GetIdByAsync(Id, cancellationToken);

            if (!response.Status)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        [HttpDelete("DeleteId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteStudentAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                return BadRequest(ApiMessages.InvalidInput);
            }

            var response = await _employeeLayer.DeleteByIdAsync(id, cancellationToken);

            if (!response.Status)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        [HttpPatch("PatchEmployee/{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetEmployeeDto>> PatchEmployeeAsync(int Id, string empName, CancellationToken cancellationToken)
        {
            if (Id <= 0 || empName is null)
            {
                return BadRequest(ApiMessages.InvalidInput);
            }

            var response = await _employeeLayer.PatchEmployeeAsync(Id, empName, cancellationToken);
            if (!response.Status)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

    }
}
