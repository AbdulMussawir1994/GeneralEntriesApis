using GeneralEntries.DTOs;
using GeneralEntries.Helpers.Response;

namespace GeneralEntries.RepositoryLayer.InterfaceClass;

public interface IEmployeeLayer
{
    Task<ServiceResponse<GetEmployeeDto>> AddNewEmployeeAsync(CreateEmployeeDto model);
    Task<ServiceResponse<bool>> DeleteByIdAsync(int Id);
    Task<ServiceResponse<GetEmployeeDto>> GetIdByAsync(int Id);
    Task<ServiceResponse<IEnumerable<GetEmployeeDto>>> GetListAsync();
    Task<ServiceResponse<GetEmployeeDto>> PatchEmployeeAsync(int Id, string empName);
    Task<ServiceResponse<GetEmployeeDto>> UpdateEmployeeAsync(UpdateEmployeeDto model);
}
