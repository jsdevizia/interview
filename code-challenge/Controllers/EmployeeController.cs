using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    [Route("api/employee")]
    public class EmployeeController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        #region Base Employee Endpoints
        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");

            _employeeService.Create(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        [HttpGet("{id}", Name = "getEmployeeById")]
        public IActionResult GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public IActionResult ReplaceEmployee(String id, [FromBody] Employee newEmployee)
        {
            _logger.LogDebug($"Recieved employee update request for '{id}'");

            var existingEmployee = _employeeService.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            _employeeService.Replace(existingEmployee, newEmployee);

            return Ok(newEmployee);
        }
        #endregion

        #region Reports Endpoints
        [HttpGet("{id}/reports")]
        public IActionResult GetEmployeeReportingStructure(String id)
        {
            _logger.LogDebug($"Recieved employee report structure get request for '{id}'");

            var employee = _employeeService.GetById(id);
            if (employee == null)
                return NotFound();

            return Ok(_employeeService.GetReportingStructure(employee));
        }
        #endregion

        #region Compensation Endpoints
        [HttpGet("{id}/compensation")]
        public IActionResult GetEmployeeCompensation(String id)
        {
            _logger.LogDebug($"Recieved employee compensation get request for '{id}'");

            //pull the employee to verify they exist
            var employee = _employeeService.GetById(id);
            if (employee == null)
                return NotFound();

            //pull the compensation
            var compensation = _employeeService.GetCompensation(employee);

            //verify that a current compensation record exists
            if (compensation == null)
            {
                return NotFound();
            }

            return Ok(compensation);
        }

        [HttpPut("{id}/compensation")]
        public IActionResult CreateEmployeeCompensation(String id, [FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Recieved employee compensation post request for '{id}'");

            //Input validation, verify that if the employeeid was provided, that it matches the route
            if (!string.IsNullOrEmpty(compensation.EmployeeId) && compensation.EmployeeId != id)
            {
                return BadRequest();
            }

            compensation.EmployeeId = id;

            //pull the employee to verify the record exists
            var employee = _employeeService.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            compensation.Employee = employee;

            //write the compensation to the db
            _employeeService.CreateOrReplaceCompensation(compensation);

            //since the record is a value object, there is no identifier or new information generated server side.
            return Ok();
        }
        #endregion
    }
}
