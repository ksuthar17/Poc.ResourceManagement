using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Poc.Application.Interfaces;
using Poc.ResourceManagement.Domain.Entities;

namespace Poc.ResourceManagement.Pages
{
    public class IndexModel(ILogger<IndexModel> _logger ,IEmployeeRepository _employeeRepository) : PageModel
    {
        //private readonly ILogger<IndexModel> _logger;

        //public IndexModel(ILogger<IndexModel> logger)
        //{
        //    _logger = logger;
        //}

        public void OnGet()
        {

        }

        public async void OnGetEmployee()
        {
            JqueryDataTableParams jParams = new();
            int totalRecord = 0;
            int filterRecord = 0;
            //var draw = Request.Form["draw"].FirstOrDefault();

            jParams.SortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            jParams.SortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            jParams.SearchText = Request.Form["search[value]"].FirstOrDefault();
            jParams.PageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            jParams.Skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

            var data = await _employeeRepository.GetEmployees(jParams);

            var returnObj = new
            {
               // draw = draw,
                recordsTotal = data.TotalRecors,
                recordsFiltered = filterRecord,
                data = data.Employees
            };
        }
    }
}
