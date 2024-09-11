using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace stockapi.Models
{
    [Table("Comments")]
    public class Comment
    {
        public int ID { get; set; }
        public string Title { get; set; }  =string.Empty;
        public string Description { get; set; }=string.Empty;

        public DateTime CreatedOn { get; set; }=new DateTime();
        public int? StockID { get; set; }
        public Stock? Stocks { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}