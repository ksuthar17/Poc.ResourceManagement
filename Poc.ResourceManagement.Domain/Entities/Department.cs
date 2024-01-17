using System.ComponentModel.DataAnnotations;

namespace Poc.ResourceManagement.Domain.Entities
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();
    }
}
