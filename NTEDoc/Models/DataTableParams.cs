using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NTEDoc.DataRepository.Models;

namespace NTEDoc.Models
{
    public class DataTableParams
    {
        public string SearchYear { get; set; }
        public string SearchStatus { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int[] Partners { get; set; }
        public int[] Executors { get; set; }
        public int[] Sectors { get; set; }
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public int? ContractType { get; set; }
        public bool PrintAll { get; set; }
        public AdvancedFilters AdvancedFilters { get; set; }
        public ColumnRequestItem[] Columns { get; set; }
        public OrderRequestItem[] Order { get; set; }
        public SearchRequestItem Search { get; set; }
        public RoleFilter ShowAll { get; set; }
        public RoleFilter AwaitingUser { get; set; }
        public RoleFilter LongerThan48 { get; set; }
    }

    public class ColumnRequestItem
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public SearchRequestItem Search { get; set; }
    }

    public class OrderRequestItem
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public class SearchRequestItem
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }
}
