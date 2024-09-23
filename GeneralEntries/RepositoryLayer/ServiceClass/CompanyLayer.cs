using GeneralEntries.ContextClass;
using GeneralEntries.DTOs;
using GeneralEntries.DTOs.CompaniesDto;
using GeneralEntries.Helpers.Response;
using GeneralEntries.Models;
using GeneralEntries.RepositoryLayer.InterfaceClass;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace GeneralEntries.RepositoryLayer.ServiceClass
{
    public class CompanyLayer : ICompanyLayer
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<EmployeeLayer> _logger;
        private readonly DbContextClass _dbContextClass;

        public CompanyLayer(IConfiguration configuration, ILogger<EmployeeLayer> logger, DbContextClass dbContextClass)
        {
            _configuration = configuration;
            _logger = logger;
            _dbContextClass = dbContextClass;
        }

        public async Task<ServiceResponse<IEnumerable<GetCompanyDto>>> GetCompaniesList(CancellationToken cancellationToken)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<GetCompanyDto>>();

            try
            {
                _logger.LogInformation("Fetching company data from the database...");

                var result = await _dbContextClass.Companies
                                                                 .Include(x => x.Employee)
                                                                 .AsNoTracking()
                                                                 .IgnoreQueryFilters()
                                                                 .AsSplitQuery()
                                                                 .ToListAsync(cancellationToken);

                serviceResponse.Value = result.Adapt<IEnumerable<GetCompanyDto>>();
                serviceResponse.Status = true;
                serviceResponse.Message = result.Any()
                    ? $"Fetched {result.Count} company records successfully."
                    : "No company records found.";
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Operation was canceled while fetching companies.");
                serviceResponse.Status = false;
                serviceResponse.Message = "Request was canceled.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching company data.");
                serviceResponse.Status = false;
                serviceResponse.Message = $"An error occurred: {ex.Message}";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCompanyDto>> AddNewCompanyAsync(CreateComDto model, CancellationToken cancellationToken)
        {
            var serviceResponse = new ServiceResponse<GetCompanyDto>();

            try
            {
                var newCompany = model.Adapt<Company>();

                _dbContextClass.Companies.Add(newCompany);
                var result = await _dbContextClass.SaveChangesAsync(cancellationToken);

                if (result > 0)
                {
                    serviceResponse.Status = true;
                    serviceResponse.Message = "Company added successfully.";
                    serviceResponse.Value = newCompany.Adapt<GetCompanyDto>();
                }
                else
                {
                    serviceResponse.Status = false;
                    serviceResponse.Message = "Failed to add new company.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new company.");
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
                var company = await _dbContextClass.Companies
                                                   .SingleOrDefaultAsync(c => c.CompanyId == model.CompanyId, cancellationToken);

                if (company is null)
                {
                    serviceResponse.Status = false;
                    serviceResponse.Message = "Company not found.";
                    return serviceResponse;
                }

                company.CompanyName = model.CompanyName;
                company.Country = model.Country;
                company.City = model.City;
                company.Branch = model.Branch;
                company.EmployeeId = model.EmployeeId;

                await _dbContextClass.SaveChangesAsync(cancellationToken);

                serviceResponse.Status = true;
                serviceResponse.Message = "Company updated successfully.";
                serviceResponse.Value = company.Adapt<GetCompanyDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the company.");
                serviceResponse.Status = false;
                serviceResponse.Message = $"An error occurred: {ex.Message}";
            }

            return serviceResponse;
        }
    }
}
