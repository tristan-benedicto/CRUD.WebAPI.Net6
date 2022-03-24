using CRUD.WebAPI.Net6.Data;
using CRUD.WebAPI.Net6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.WebAPI.Net6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly EmployeesDbContext employeesDbContext;
        
        public EmployeesController(EmployeesDbContext employeesDbContext)
        {
            this.employeesDbContext = employeesDbContext;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await employeesDbContext.Employees.ToListAsync();
            
            return Ok(employees);
        }

        [HttpGet]
        [Route("{employeeNumber:int}")]
        [ActionName("GetEmployee")]
        public async Task<IActionResult> GetEmployee([FromRoute] int employeeNumber)
        {
            var employee = await employeesDbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeNumber == employeeNumber);
            
            if (employee != null)
            {
                return Ok(employee);
            }

            return NotFound("Employee not found");
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> SearchEmployee([FromBody] Employee employee)
        {
            var existingmployees = await employeesDbContext.Employees.Where(e => e.EmployeeNumber == employee.EmployeeNumber
                || e.FirstName.Contains(employee.FirstName)
                || e.LastName.Contains(employee.LastName)
                || (e.Temperature >= (employee.Temperature - 2) && e.Temperature <= (employee.Temperature + 2))
                || (e.RecordDate >= employee.RecordDate.AddDays(-2) && e.RecordDate <= employee.RecordDate.AddDays(2)))
                .ToListAsync();

            return Ok(existingmployees);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            await employeesDbContext.Employees.AddAsync(employee);
            await employeesDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { employeeNumber = employee.EmployeeNumber }, employee);
        }

        [HttpPut]
        [Route("{employeeNumber:int}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] int employeeNumber, [FromBody] Employee employee)
        {
            var existingEmployee = await employeesDbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeNumber == employeeNumber);

            if (existingEmployee != null)
            {
                existingEmployee.FirstName = employee.FirstName;
                existingEmployee.LastName = employee.LastName;
                existingEmployee.Temperature = employee.Temperature;
                existingEmployee.RecordDate = employee.RecordDate;
                
                await employeesDbContext.SaveChangesAsync();

                return Ok(existingEmployee);
            }

            return NotFound("Employee not found");
        }

        [HttpDelete]
        [Route("{employeeNumber:int}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int employeeNumber)
        {
            var existingEmployee = await employeesDbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeNumber == employeeNumber);

            if (existingEmployee != null)
            {
                employeesDbContext.Remove(existingEmployee);
                
                await employeesDbContext.SaveChangesAsync();

                return Ok(existingEmployee);
            }

            return NotFound("Employee not found");
        }

    }
}
