using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Poc.ResourceManagement.Pages.Model
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Department is Required")]
        public int? DepartmentId { get; set; } = null;

        [Required(ErrorMessage = "Birth date is Required")]
        [BindProperty]
        public DateTime? DateOfBirth { get; set; }
        public List<SelectListItem> Departments { get; set; } = new List<SelectListItem>();
        [Required(ErrorMessage = "Gender is Required.")]
        public string? Gender { get; set; }
    }
}
