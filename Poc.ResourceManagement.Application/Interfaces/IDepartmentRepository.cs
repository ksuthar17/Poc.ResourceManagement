using Poc.ResourceManagement.Domain.Entities;
using Poc.ResourceManagement.Domain.Shared;

namespace Poc.ResourceManagement.Application.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<Result> GetDepartments();
        Task<Result> InsertAsync(Department department);
        Task<Result> UpdateAsync(Department department);
        Task<Result> DeleteAsync(int Id);
        Task<Result> GetDepartmentById(int Id);
    }
}
