using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using stockapi.DTO.Comments;
using stockapi.Models;

namespace stockapi.DTO.Stocks
{
    public class StockDto
    {
           public int ID { get; set; }
        public string Symbol { get; set; }=string.Empty;
        public string CompanyName { get; set; }=string.Empty;
        public decimal Purchase { get; set; }

        public decimal LastDiv { get; set; }

        public string Industry { get; set; }=string.Empty;

        public long MarketCap { get; set; }

        public List<CommentDTO> Comments {get; set;}
    }
}