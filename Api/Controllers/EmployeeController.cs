using Api.Context;
using Api.Interfaces.Services;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[Route("/api/employee")]
public class EmployeeController(IEmployeeServices services, ILogger<EmployeeController> logger)
    : Controller
{
    private readonly IEmployeeServices _services = services;
    private readonly ILogger<EmployeeController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var employees = await _services.GetAllEmployees();
            _logger.LogInformation("EmployeeController GetAll Employees Called");
            return Json(employees);
        }
        catch (Exception ex)
        {
            _logger.LogError("EmployeeController Could not GetAll Employees {error}", ex.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOne([FromRoute] int id)
    {
        try
        {
            var employee = await _services.GetOneEmployee(id);

            _logger.LogInformation($"EmployeeController GetOne:{id} Employees Called");

            if (employee == null)
            {
                _logger.LogInformation($"EmployeeController Employee with id: {id} not found");
                return NotFound();
            }

            return Json(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError("EmployeeController Could not GetOne Employee {error}", ex.Message);
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmployeeRequestModel employeeRequest)
    {
        try
        {
            var employee = await _services.AddEmployee(employeeRequest);

            _logger.LogInformation($"EmployeeController Create {employee}");
            return Json(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError($"EmployeeController Could not Create Employee {ex.Message}");
            return StatusCode(500);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] EmployeeModel employee)
    {
        try
        {
            var responseEmployee = await _services.UpdateEmployee(employee);
            if (responseEmployee == null)
            {
                _logger.LogInformation($"EmployeeController Employee with id: {employee.Id} not found");
                return NotFound();
            }

            _logger.LogInformation($"EmployeeController Update {responseEmployee}");
            return Json(responseEmployee);
        }
        catch (Exception ex)
        {
            _logger.LogError($"EmployeeController Could not Update Employee {ex.Message}");
            return StatusCode(500);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        try
        {
            var employee = await _services.DeleteEmployee(id);

            _logger.LogInformation($"EmployeeController Delete:{id} Employee Called");

            if (employee == null)
            {
                _logger.LogInformation($"EmployeeController Employee with id: {id} not found");
                return NotFound();
            }

            return Json(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError("EmployeeController Could not Delete Employee {error}", ex.Message);
            return StatusCode(500);
        }
    }
}
