using Dapper;
using Poc.Application.Interfaces;
using Poc.ResourceManagement.Domain.Entities;
using Poc.ResourceManagement.Domain.Shared;
using Poc.ResourceManagement.Infrastructure.Data;
using System.Data;

namespace Poc.ResourceManagement.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DapperContext dapperContext;
        public EmployeeRepository(DapperContext dapperContext)
        {
            this.dapperContext = dapperContext;
        }

        public async Task<Result> DeleteAsync(int Id)
        {
            using (var connection = dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Id", Id);
                var result = await connection.ExecuteAsync("sp_DeleteEmployee", parameters,
                                 commandType: CommandType.StoredProcedure);

                if (result > 0) return Result.Success(null , "Employee Deleted");
            }

            return Result.Failure(new Error(
                         "Employee.DeleteFailled", "Delete Failled"));
        }

        public async Task<Result> GetEmployees()
        {
            using (var connection = dapperContext.CreateConnection())
            {
                var employees = await connection.QueryAsync<Employee>("sp_GetEmployees" , 
                                 commandType: CommandType.StoredProcedure);

                if(employees != null) return Result.Success(employees);
            }

            return Result.Failure(new Error(
                         "Employee.NotFound", "No records found"));
        }

        public async Task<Result> InsertAsync(Employee employee)
        {
            using (var connection = dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Name", employee.Name);
                parameters.Add("DateOfBirth", employee.DateOfBirth);
                parameters.Add("DepartmentId", employee.DepartmentId);

                var result = await connection.ExecuteAsync("sp_InsertEmployee", parameters,
                                 commandType: CommandType.StoredProcedure);

                if (result > 0) return Result.Success(null, "Employee Added Successfully");
            }

            return Result.Failure(new Error(
                         "Employee.AddFailled", "Insert Failled"));
        }

        public async Task<Result> UpdateAsync(Employee employee)
        {
            using (var connection = dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Id", employee.Id);
                parameters.Add("Name", employee.Name);
                parameters.Add("DateOfBirth", employee.DateOfBirth);
                parameters.Add("DepartmentId", employee.DepartmentId);

                var result = await connection.ExecuteAsync("sp_UpdateEmployee", parameters,
                                 commandType: CommandType.StoredProcedure);

                if (result > 0) return Result.Success(null, "Employee Updated Successfully");
            }

            return Result.Failure(new Error(
                         "Employee.UpdateFailled", "Update Failled"));
        }

        public async Task<Result> GetEmployeeById(int Id)
        {
            using (var connection = dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Id", Id);
                var employee = await connection.QueryFirstOrDefaultAsync<Employee>("sp_GetEmployeeById",parameters,
                                 commandType: CommandType.StoredProcedure);

                if (employee != null) return Result.Success(employee);
            }

            return Result.Failure(new Error(
                         "Employee.NotDound", "Employee Not Found"));
        }
    }
}
