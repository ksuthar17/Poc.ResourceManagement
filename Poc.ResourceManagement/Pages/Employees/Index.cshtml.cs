using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Poc.Application.Interfaces;
using Poc.ResourceManagement.Application.Interfaces;
using Poc.ResourceManagement.Domain.Entities;
using Poc.ResourceManagement.Pages.Model;

namespace Poc.ResourceManagement.Pages.Employees
{
    public class IndexModel(ILogger<IndexModel> _logger,
    IEmployeeRepository _employeeRepository,
    IDepartmentRepository _departmentRepository) : PageModel
    {
        public void OnGet()
        {

        }

        public async Task<JsonResult> OnGetEmployee()
        {
            JqueryDataTableParams jParams = new();

            var draw = Request.Query["draw"].FirstOrDefault();

            jParams.SortColumn = Request.Query["columns[" + Request.Query["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            jParams.SortColumnDirection = Request.Query["order[0][dir]"].FirstOrDefault();
            jParams.SearchText = Request.Query["search[value]"].FirstOrDefault();
            jParams.PageSize = Convert.ToInt32(Request.Query["length"].FirstOrDefault() ?? "0");
            jParams.Skip = Convert.ToInt32(Request.Query["start"].FirstOrDefault() ?? "0");

            var data = await _employeeRepository.GetEmployees(jParams);

            return new JsonResult(new
            {
                draw = draw,
                recordsTotal = data.TotalRecors,
                recordsFiltered = data.TotalRecors,
                data = data.Employees
            });
        }

        public async Task OnGetDelete(int id)
        {
            await _employeeRepository.DeleteAsync(id);
        }

        public async Task<PartialViewResult> OnGetAddEditEmployeePartial(int? id)
        {
            EmployeeDto employeeDto = new();
            if (id.HasValue)
            {
                var data = await _employeeRepository.GetEmployeeById((int)id);
                if (data.IsSuccess)
                {
                    var employee = (Employee)data.Data;
                    employeeDto.Id = employee.Id;
                    employeeDto.Name = employee.Name;
                    employeeDto.DepartmentId = employee.DepartmentId;
                    employeeDto.Gender = employee.Gender;

                }
            }

            employeeDto.Departments = GetDepartments();

            return new PartialViewResult
            {
                ViewName = "_addEditEmployeePartialView",
                ViewData = new ViewDataDictionary<EmployeeDto>(ViewData, employeeDto)
            };
        }


        public PartialViewResult OnPostAddEditEmployeePartial(EmployeeDto model)
        {
            model.Departments = GetDepartments();
            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    //we can use AutoMapper or Mapster in future.
                    Employee employee = new Employee { Name = model.Name, DateOfBirth = (DateTime)model.DateOfBirth, DepartmentId = (int)model.DepartmentId, Id = model.Id, Gender = model.Gender };

                    if (model.Id > 0)
                    {
                        _employeeRepository.UpdateAsync(employee);
                    }

                    else
                    {
                        _employeeRepository.InsertAsync(employee);
                    }
                }
            }


            return new PartialViewResult
            {
                ViewName = "_addEditEmployeePartialView",
                ViewData = new ViewDataDictionary<EmployeeDto>(ViewData, model)
            };
        }

        private List<SelectListItem> GetDepartments()
        {
            List<SelectListItem> departments = new List<SelectListItem>();
            var department = _departmentRepository.GetDepartments();
            departments.Add(new SelectListItem { Value = "", Text = "--select department---" });
            foreach (var item in (List<Department>)department.Result.Data)
            {
                departments.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }
            return departments;
        }
    }
}
