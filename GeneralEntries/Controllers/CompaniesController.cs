using GeneralEntries.DTOs;
using GeneralEntries.DTOs.CompaniesDto;
using GeneralEntries.Helpers.Response;
using GeneralEntries.RepositoryLayer.InterfaceClass;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace GeneralEntries.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ILogger<CompaniesController> _logger;
        private readonly ICompanyLayer _companyLayer;
        private readonly IDistributedCache _distributedCache;
        string RedisCacheKey = "CompanyCon";


        public CompaniesController(ILogger<CompaniesController> logger, ICompanyLayer companyLayer, IDistributedCache distributedCache)
        {
            _logger = logger;
            _companyLayer = companyLayer;
            _distributedCache = distributedCache;
        }

        //[HttpGet]
        //public async Task<IQueryable<ChartsDtos>> GetAllChartsofDtos1()
        //{
        //    var Charts = await _context.ChartsofAccounts.Include(p => p.CompanyIdNavigation).Select(p => ModelConverter.ModeltoDto(p)).ToListAsync();
        //    return (IQueryable<ChartsDtos>)Charts;
        //}


        [HttpGet("CompanyList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetCompanyDto>>>> GetCompaniesList(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting request to fetch company list.");
            var serviceResponse = new ServiceResponse<IEnumerable<GetCompanyDto>>();

            try
            {
                // Attempt to retrieve from cache
                var cachedData = await _distributedCache.GetStringAsync(RedisCacheKey, cancellationToken);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    serviceResponse.Value = JsonConvert.DeserializeObject<IEnumerable<GetCompanyDto>>(cachedData);
                    serviceResponse.Status = true;
                    serviceResponse.Message = $"Fetched company records from Redis.";

                    return Ok(serviceResponse);
                }

                // Fetch from database if cache is empty
                var result = await _companyLayer.GetCompaniesList(cancellationToken);
                if (result.Status)
                {
                    var serializedResult = JsonConvert.SerializeObject(result.Value);
                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(20),
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6)
                    };
                    await _distributedCache.SetStringAsync(RedisCacheKey, serializedResult, cacheOptions, cancellationToken);

                    return Ok(result);
                }

                _logger.LogWarning("Company list retrieval failed: {Message}", result.Message);
                return BadRequest(result);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Company list request was canceled.");
                return StatusCode(StatusCodes.Status499ClientClosedRequest, "Request was canceled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the company list.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
            }
        }

        [HttpGet("CompanyListAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetCompanyDto>>>> GetCompaniesListAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting request to fetch company list.");
            var serviceResponse = new ServiceResponse<IEnumerable<GetCompanyDto>>();

            try
            {
                // First, attempt to retrieve the data from Redis cache
                var cachedData = await _distributedCache.GetAsync(RedisCacheKey, cancellationToken);
                if (cachedData is not null && cachedData.Length > 0)
                {
                    try
                    {
                        var serializedData = Encoding.UTF8.GetString(cachedData);
                        var cachedCompanies = JsonConvert.DeserializeObject<IEnumerable<GetCompanyDto>>(serializedData);
                        if (cachedCompanies is not null)
                        {
                            serviceResponse.Value = cachedCompanies;
                            serviceResponse.Status = true;
                            serviceResponse.Message = "Fetched company records from Redis.";
                            return Ok(serviceResponse);
                        }
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError(ex, "Error deserializing cached company list.");
                    }
                }

                // If cache is empty or deserialization failed, fetch from database
                var result = await _companyLayer.GetCompaniesList(cancellationToken);
                if (result.Status && result.Value is not null)
                {
                    // Serialize the result and cache it for future requests
                    var cacheEntryOptions = new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(20),
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6)
                    };
                    var serializedResult = JsonConvert.SerializeObject(result.Value);
                    var cacheData = Encoding.UTF8.GetBytes(serializedResult);

                    await _distributedCache.SetAsync(RedisCacheKey, cacheData, cacheEntryOptions, cancellationToken);

                    serviceResponse.Value = result.Value;
                    serviceResponse.Status = true;
                    serviceResponse.Message = "Fetched company records from database.";
                    return Ok(serviceResponse);
                }

                _logger.LogWarning("Failed to retrieve company list: {Message}", result.Message);
                return BadRequest(result);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Company list request was canceled.");
                return StatusCode(StatusCodes.Status499ClientClosedRequest, "Request was canceled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the company list.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An internal error occurred.");
            }
        }

        [HttpPost("AddNewCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ServiceResponse<GetCompanyDto>>> AddCompany([FromBody] CreateComDto model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for AddCompany: {Errors}", ModelState.Values.SelectMany(x => x.Errors));
                return BadRequest(ModelState);
            }

            var response = await _companyLayer.AddNewCompanyAsync(model, cancellationToken);

            if (!response.Status)
            {
                _logger.LogWarning("AddCompany failed: {Message}", response.Message);
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ServiceResponse<GetCompanyDto>>> UpdateCompany([FromBody] CreateComDto model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for UpdateCompany: {Errors}", ModelState.Values.SelectMany(x => x.Errors));
                return BadRequest(ModelState);
            }

            var response = await _companyLayer.UpdateCompanyAsync(model, cancellationToken);

            if (!response.Status)
            {
                _logger.LogWarning("UpdateCompany failed: {Message}", response.Message);
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
