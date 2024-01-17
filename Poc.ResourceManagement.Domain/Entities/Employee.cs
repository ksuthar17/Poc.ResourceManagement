using System.ComponentModel.DataAnnotations;

namespace Poc.ResourceManagement.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }

    }
}
