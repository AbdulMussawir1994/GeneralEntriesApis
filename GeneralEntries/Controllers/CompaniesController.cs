﻿using GeneralEntries.DTOs;
using GeneralEntries.DTOs.CompaniesDto;
using GeneralEntries.RepositoryLayer.InterfaceClass;
using GeneralEntries.RepositoryLayer.ServiceClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace GeneralEntries.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ILogger<CompaniesController> _logger;
        private readonly ICompanyLayer _companyLayer;


        public CompaniesController(ILogger<CompaniesController> logger, ICompanyLayer companyLayer)
        {
            _logger = logger;
            _companyLayer = companyLayer;
        }

        [HttpGet("CompanyList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCompaniesList(CancellationToken cancellationToken)
        {
            try
            {
                //await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Console.WriteLine("Cancelled");
                }

                _logger.LogInformation("Fetching Company Controller");

                var result = await _companyLayer.GetCompaniesList(cancellationToken);

                if (!result.Status)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Request was canceled.");
                return StatusCode(StatusCodes.Status499ClientClosedRequest, "Request was canceled.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetCompanies Controller Error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("AddNewCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetCompanyDto>> AddCompany([FromBody] CreateComDto model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _companyLayer.AddNewCompanyAsync(model, cancellationToken);

            if (!response.Status)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetCompanyDto>> UpdateCompany([FromBody] CreateComDto model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _companyLayer.UpdateCompanyAsync(model, cancellationToken);

            if (!response.Status)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
