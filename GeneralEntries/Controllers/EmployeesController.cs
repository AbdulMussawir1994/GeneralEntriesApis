using Azure;
using GeneralEntries.DTOs;
using GeneralEntries.Helpers.Response;
using GeneralEntries.Models;
using GeneralEntries.RepositoryLayer.InterfaceClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.CodeDom;

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
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetEmployeeDto>>>> GetEmployees()
        {
            var result = await _employeeLayer.GetListAsync();

            if (!result.Status)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return Ok(result);
        }

        [HttpPost(template: "Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> AddEmployee([FromBody] CreateEmployeeDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _employeeLayer.AddNewEmployeeAsync(model);

            if (!response.Status)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut(template: "Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> UpdateEmployee(UpdateEmployeeDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _employeeLayer.UpdateEmployeeAsync(model);

            if (!response.Status)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet(template: "GetId/{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> GetEmployeeByIdAsync(int Id)
        {
            if (Id <= 0)
            {
                return BadRequest(ApiMessages.InvalidInput);
            }

            var response = await _employeeLayer.GetIdByAsync(Id);

            if (!response.Status)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("DeleteId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteStudentAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest(ApiMessages.InvalidInput);
            }

            var response = await _employeeLayer.DeleteByIdAsync(id);

            if (!response.Status)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Update only Employee Name by Id
        /// </summary>
        /// <param name="empName"></param>
        /// <param name="Id"></param>
        [HttpPatch("PatchEmployee/{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> PatchEmployeeAsync(int Id, string empName)
        {
            if (Id <= 0 || empName is null)
            {
                return BadRequest(ApiMessages.InvalidInput);
            }

            var response = await _employeeLayer.PatchEmployeeAsync(Id, empName);
            if (!response.Status)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

    }
}
