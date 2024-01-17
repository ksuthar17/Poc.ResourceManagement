using Poc.ResourceManagement.Domain.Entities;
using Poc.ResourceManagement.Domain.Shared;

namespace Poc.Application.Interfaces
{
    public interface IEmployeeRepository
    {
       Task<EmployeeResponse> GetEmployees(JqueryDataTableParams jparams);
       Task<Result> InsertAsync(Employee employee);
       Task<Result> UpdateAsync(Employee employee);
       Task<Result> DeleteAsync(int Id);
        Task<Result> GetEmployeeById(int Id);
    }
}
