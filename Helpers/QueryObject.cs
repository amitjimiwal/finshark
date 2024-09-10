using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stockapi.Helpers
{
    public class QueryObject
    {
        public string? Symbol {get; set;}=null;
        public string? CompanyName {get; set;}=null;
        
        //sort by specifies the SORT BY PARAMETER , like "Symbol","Company Name"
        public string? SortBY { get; set; }=null;

        //specifies the order of the above SORTBY parameter
        public bool IsDescending { get; set; }=false;

        public int PageNumber {get; set;}=1;

        public int PageSize {get; set;}=5;
    }
}