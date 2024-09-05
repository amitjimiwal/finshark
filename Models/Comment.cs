using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stockapi.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public string Title { get; set; }  =string.Empty;
        public string Description { get; set; }=string.Empty;

        public DateTime CreatedOn { get; set; }=new DateTime();
        public int? StockID { get; set; }
        public Stock? Stocks { get; set; }
    }
}