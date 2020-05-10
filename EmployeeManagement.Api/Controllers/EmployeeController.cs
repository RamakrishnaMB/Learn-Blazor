using EmployeeManagement.Api.Models;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        //from hardcoded service
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetEmployes()
        {
            try
            {
                return Ok(await employeeRepository.GetEmployees());
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Employee>> GetEmployeByID(int Id)
        {
            try
            {
                var result = await employeeRepository.GetEmployee(Id);
                if (result == null)
                {
                    return NotFound();
                }
                return result;
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }
                else
                {
                    var empEmail = employeeRepository.GetEmployeeByEmail(employee.Email);
                    if (empEmail != null)
                    {
                        ModelState.AddModelError("email", "Employee email already in user");
                        return BadRequest(ModelState);
                    }
                    var empCreatedObj = await employeeRepository.AddEmployee(employee);
                    return CreatedAtAction(nameof(GetEmployeByID), new { Id = empCreatedObj.EmployeeId }, empCreatedObj);
                }
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employee)
        {
            try
            {
                if (id != employee.EmployeeId)
                {
                    return BadRequest("Employe ID Mismatch");
                }
                var empToupdate = await employeeRepository.GetEmployee(id);
                if (empToupdate == null)
                {
                    return NotFound($"Employee with Id ={id} not found.");
                }
                return await employeeRepository.UpdateEmployee(employee);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating data from database");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            try
            {
                var emp = await employeeRepository.GetEmployee(id);
                if (emp == null)
                {
                    return NotFound($"Employee with Id ={id} not found");
                }
                return await employeeRepository.DeleteEmployee(id);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating data from database");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Employee>>> Search(string name, Gender? gender)
        {
            try
            {
                var employees = await employeeRepository.Search(name, gender);
                if (employees != null)
                {
                    return Ok(employees);
                }
                return NotFound();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating data from database");
            }
        }
    }
}