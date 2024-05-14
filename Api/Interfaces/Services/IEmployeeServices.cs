using Api.Models;

namespace Api.Interfaces.Services;

public interface IEmployeeServices
{
    Task<List<EmployeeModel>> GetAllEmployees();
    Task<EmployeeModel?> GetOneEmployee(int id);
    Task<EmployeeModel> AddEmployee(EmployeeRequestModel employee);
    Task<EmployeeModel?> UpdateEmployee(EmployeeModel employee);
    Task<EmployeeModel?> DeleteEmployee(int id);
}