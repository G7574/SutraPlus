namespace SutraPlus.Models
{
    using System.Collections.Generic;
    public class pagination<T> where T : class
    {

        public long? UserRoleId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<T> Records { get; set; }
        public string SortBy { get; set; }
        public bool SortOrder { get; set; }
        public dynamic Filter { get; set; }
        public string UserRole { get; set; }
    }
}
