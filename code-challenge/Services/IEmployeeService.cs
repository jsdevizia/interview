using challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Services
{
    public interface IEmployeeService
    {
        Employee GetById(String id);
        Employee Create(Employee employee);
        Employee Replace(Employee originalEmployee, Employee newEmployee);

        ReportingStructure GetReportingStructure(Employee employee);

        Compensation GetCompensation(Employee employee);

        /// <summary>
        /// Creates a new Compensation record for the employee.
        /// </summary>
        /// <param name="compensation">Compensation record to create</param>
        /// <exception cref="AggregateException">Thrown when the EmployeeId-EffectiveDate pair is a duplicate</exception>
        void CreateOrReplaceCompensation(Compensation compensation);
    }
}
