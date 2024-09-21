
using GeneralEntries.DTOs.CompaniesDto;
using GeneralEntries.Helpers.Response;

namespace GeneralEntries.RepositoryLayer.InterfaceClass
{
    public interface ICompanyLayer
    {
        Task<ServiceResponse<GetCompanyDto>> AddNewCompanyAsync(CreateComDto model, CancellationToken cancellationToken);
        Task<ServiceResponse<IEnumerable<GetCompanyDto>>> GetCompaniesList(CancellationToken cancellationToken);
        Task<ServiceResponse<GetCompanyDto>> UpdateCompanyAsync(CreateComDto model, CancellationToken cancellationToken);
    }
}
