namespace Poc.ResourceManagement.Domain.Entities
{
    public class JqueryDataTableParams
    {
        public string SortColumn { get; set; } = "Name";
        public string SortColumnDirection { get; set; } = "ASC";
        public string SearchText { get; set; }
        public int PageSize { get; set; } = 10;
        public int Skip { get; set; } = 0;
    }
}
