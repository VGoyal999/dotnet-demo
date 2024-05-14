using Api.Context;
using Api.Interfaces.Services;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class EmployeeServices(ApplicationDbContext context) : IEmployeeServices
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<EmployeeModel>> GetAllEmployees() => await _context.Employees.ToListAsync();

    public async Task<EmployeeModel?> GetOneEmployee(int id) => await _context.Employees.FindAsync(id);

    public async Task<EmployeeModel> AddEmployee(EmployeeRequestModel employeeRequest)
    {
        var highestIdEmployee = await _context
            .Employees.OrderByDescending(e => e.Id)
            .FirstOrDefaultAsync();
        var employee = new EmployeeModel()
        {
            Id = highestIdEmployee?.Id + 1 ?? 1,
            FirstName = employeeRequest.FirstName,
            LastName = employeeRequest.LastName,
            Department = employeeRequest.Department,
            Position = employeeRequest.Position,
            Salary = employeeRequest.Salary
        };

        await _context.AddAsync(employee);
        await _context.SaveChangesAsync();

        return employee;
    }

    public async Task<EmployeeModel?> UpdateEmployee(EmployeeModel employee)
    {
        var toUpdateEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employee.Id);
        Console.WriteLine(toUpdateEmployee?.Id);
        if (toUpdateEmployee == null) return null;
        toUpdateEmployee.FirstName = employee.FirstName;
        toUpdateEmployee.LastName = employee.LastName;
        toUpdateEmployee.Department = employee.Department;
        toUpdateEmployee.Position = employee.Position;
        toUpdateEmployee.Salary = employee.Salary;

        await _context.SaveChangesAsync();

        return toUpdateEmployee;
    }

    public async Task<EmployeeModel?> DeleteEmployee(int id)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null) return null;
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return employee;
    }
}
