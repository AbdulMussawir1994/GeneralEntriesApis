using GeneralEntries.DTOs;
using GeneralEntries.Helpers.Response;

namespace GeneralEntries.RepositoryLayer.InterfaceClass;

public interface IEmployeeLayer
{
    Task<ServiceResponse<GetEmployeeDto>> AddNewEmployeeAsync(CreateEmployeeDto model, CancellationToken cancellationToken);
    Task<ServiceResponse<bool>> DeleteByIdAsync(int Id, CancellationToken cancellationToken);
    Task<ServiceResponse<GetEmployeeDto>> GetIdByAsync(int Id, CancellationToken cancellationToken);
    Task<ServiceResponse<IEnumerable<GetEmployeeDto>>> GetListAsync(CancellationToken cancellationToken);
    Task<ServiceResponse<GetEmployeeDto>> PatchEmployeeAsync(int Id, string empName, CancellationToken cancellationToken);
    Task<ServiceResponse<GetEmployeeDto>> UpdateEmployeeAsync(CreateEmployeeDto model, CancellationToken cancellationToken);
}
