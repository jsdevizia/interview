using challenge.Models;
using System;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(String id);
        Employee Add(Employee employee);
        Employee Remove(Employee employee);
        Task SaveAsync();
        void LoadReports(Employee employee);
        void CreateCompensation(Compensation compensation);
        void RemoveCompensation(Compensation compensation);
        Compensation GetCompensation(Employee employee);
    }
}