using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            return _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

        public void LoadReports(Employee employee)
        {
            employee.DirectReports = _employeeContext.Employees
                .Include(e => e.DirectReports)
                .Single(e => e.EmployeeId == employee.EmployeeId)
                .DirectReports;

            employee.DirectReports.ForEach(LoadReports);
        }

        public void CreateCompensation(Compensation compensation)
        {
            _employeeContext.Compensations.Add(compensation);
        }

        public void RemoveCompensation(Compensation compensation)
        {
            _employeeContext.Compensations.Remove(compensation);
        }

        public Compensation GetCompensation(Employee employee)
        {
            return _employeeContext.Compensations
                .OrderByDescending(c => c.EffectiveDate)
                .FirstOrDefault(c => c.EffectiveDate < DateTime.Now && c.EmployeeId == employee.EmployeeId);
        }
    }
}
