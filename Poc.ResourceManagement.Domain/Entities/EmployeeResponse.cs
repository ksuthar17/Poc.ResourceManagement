namespace Poc.ResourceManagement.Domain.Entities
{
    public class EmployeeResponse
    {
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public int TotalRecors { get; set; }
    }
}
