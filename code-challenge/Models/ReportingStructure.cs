using System.Collections.Generic;
using System.Linq;

namespace challenge.Models
{
    public class ReportingStructure
    {
        public Employee Employee { get; private set; }
        public int NumberOfReports { get; private set; }

        public ReportingStructure(Employee employee)
        {
            Employee = employee;

            NumberOfReports = GetReportCount(Employee, new List<string>());
        }

        private int GetReportCount(Employee employee, List<string> ignore)
        {
            if (employee.DirectReports == null || ignore.Any(e => e == employee.EmployeeId))
            {
                return 0;
            }

            //ignore any repeated employees (in cases of multiple supervisors or cyclical report structures
            ignore.Add(employee.EmployeeId);

            //recursive linq call
            return employee.DirectReports.Aggregate(0, (count, e) => GetReportCount(e, ignore) + 1 + count);
        }
    }
}
