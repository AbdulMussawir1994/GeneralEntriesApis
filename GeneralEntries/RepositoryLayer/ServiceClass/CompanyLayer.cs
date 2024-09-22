using GeneralEntries.ContextClass;
using GeneralEntries.DTOs;
using GeneralEntries.DTOs.CompaniesDto;
using GeneralEntries.Helpers.Response;
using GeneralEntries.Models;
using GeneralEntries.RepositoryLayer.InterfaceClass;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace GeneralEntries.RepositoryLayer.ServiceClass
{
    public class CompanyLayer : ICompanyLayer
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<EmployeeLayer> _logger;
        private readonly DbContextClass _dbContextClass;
        private readonly IDistributedCache _distributedCache;

        public CompanyLayer(IConfiguration configuration, ILogger<EmployeeLayer> logger, DbContextClass dbContextClass, IDistributedCache distributedCache)
        {
            _configuration = configuration;
            _logger = logger;
            _dbContextClass = dbContextClass;
            _distributedCache = distributedCache;
        }

        public async Task<ServiceResponse<IEnumerable<GetCompanyDto>>> GetCompaniesList(CancellationToken cancellationToken)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<GetCompanyDto>>();

            try
            {
                _logger.LogInformation("Fetching companies...");

                var result = await _dbContextClass.Companies
                                                     .Include(x => x.Employee)
                                                     .IgnoreQueryFilters()
                                                     .AsNoTracking()
                                                     .ToListAsync(cancellationToken);


                ////Redis Working

                //var tomorrow = DateTime.Now.Date.AddDays(1);
                //var totalSeconds = tomorrow.Subtract(DateTime.Now).TotalSeconds;

                //var distributedCache = new DistributedCacheEntryOptions();
                //distributedCache.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(totalSeconds);
                //distributedCache.SlidingExpiration = null;

                //var jsonData = JsonConvert.SerializeObject(empData);
                //await _distributedCache.SetStringAsync("DashboardData", jsonData, distributedCache);

                ////End

                serviceResponse.Value = result.Adapt<IEnumerable<GetCompanyDto>>();
                serviceResponse.Status = true;
                serviceResponse.Message = result.Any()
                         ? $"Fetched {result.Count} company records successfully."
                        : "No company records found.";
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Operation canceled by the client.");
                serviceResponse.Status = false;
                serviceResponse.Message = "Request was canceled.";
            }
            catch (Exception ex)
            {
                serviceResponse.Status = false;
                serviceResponse.Message = $"An error occurred: {ex.Message}";
                _logger.LogError("GetCompanies Error: " + ex.Message);
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCompanyDto>> AddNewCompanyAsync(CreateComDto model, CancellationToken cancellationToken)
        {
            var serviceResponse = new ServiceResponse<GetCompanyDto>();

            try
            {
                var newEmployee = model.Adapt<Company>(); // Assuming Mapster or other mapper

                _dbContextClass.Companies.Add(newEmployee);

                var result = await _dbContextClass.SaveChangesAsync(cancellationToken);

                if (result > 0)
                {
                    serviceResponse.Status = true;
                    serviceResponse.Message = "Company added successfully.";
                    serviceResponse.Value = newEmployee.Adapt<GetCompanyDto>();
                }
                else
                {
                    serviceResponse.Status = false;
                    serviceResponse.Message = "Failed to add new company.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("AddCompany Error: " + ex.Message);
                serviceResponse.Status = false;
                serviceResponse.Message = $"An error occurred: {ex.InnerException.Message}";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCompanyDto>> UpdateCompanyAsync(CreateComDto model, CancellationToken cancellationToken)
        {
            var serviceResponse = new ServiceResponse<GetCompanyDto>();

            try
            {
                var emp = await _dbContextClass.Companies
                                               .Include(x => x.Employee)
                                               .SingleOrDefaultAsync(c => c.CompanyId== model.CompanyId, cancellationToken);

                if (emp == null)
                {
                    serviceResponse.Status = false;
                    serviceResponse.Message = "Company not found.";
                    return serviceResponse;
                }

                emp.CompanyName = model.CompanyName;
                emp.Country = model.Country;
                emp.City = model.City;
                emp.Branch = model.Branch;
                emp.EmployeeId = model.EmployeeId;

                await _dbContextClass.SaveChangesAsync(cancellationToken);

                serviceResponse.Status = true;
                serviceResponse.Message = "Company updated successfully.";
                serviceResponse.Value = emp.Adapt<GetCompanyDto>();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = false;
                serviceResponse.Message = $"An error occurred: {ex.Message}";
            }

            return serviceResponse;
        }
    }
}
