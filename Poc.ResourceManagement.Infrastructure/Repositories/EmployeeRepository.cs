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

        public async Task<EmployeeResponse> GetEmployees(JqueryDataTableParams jparams)
        {
            try
            {
                EmployeeResponse response = new EmployeeResponse();
                using (var connection = dapperContext.CreateConnection())
                {
                    if (string.IsNullOrEmpty(jparams.SortColumn))
                    {
                        jparams.SortColumn = "Name";
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("SORT_COLUMN", jparams.SortColumn);
                    parameters.Add("SORT_COLUMN_DIRECTION", jparams.SortColumnDirection);
                    parameters.Add("SEARCH_TEXT", jparams.SearchText);
                    parameters.Add("PAGESIZE", jparams.PageSize);
                    parameters.Add("SKIP", jparams.Skip);

                    var result = await connection.QueryMultipleAsync("sp_GetEmployees", parameters,
                                     commandType: CommandType.StoredProcedure);

                    response.Employees = result.Read<Employee>().ToList();
                    response.TotalRecors = result.Read<int>().FirstOrDefault();
                }

                return response;
            }catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Result> InsertAsync(Employee employee)
        {
            using (var connection = dapperContext.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Name", employee.Name);
                parameters.Add("DateOfBirth", employee.DateOfBirth);
                parameters.Add("DepartmentId", employee.DepartmentId);
                parameters.Add("Gender", employee.Gender);

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
                parameters.Add("Gender", employee.Gender);

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
