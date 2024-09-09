using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace stockapi.DTO.Stocks
{
    public class CreateStockDto
    {
        [Required]
        [MinLength(1,ErrorMessage ="Symbol must be atleast 1 characters")]
        [MaxLength(10,ErrorMessage ="Symbol can't be over 10 characters")]
        public string Symbol { get; set; }=string.Empty;

        [Required]
        [MaxLength(10,ErrorMessage ="Company Name can't be over 10 characters")]
        public string CompanyName { get; set; }=string.Empty;

        [Required]
        [Range(1,10000000)]
        public decimal Purchase { get; set; }

          [Required]
        [Range(0.001,100)]
        public decimal LastDiv { get; set; }

        [Required]
        [MaxLength(30,ErrorMessage ="Industry Name can't be over 10 characters")]
        public string Industry { get; set; }=string.Empty;

        [Required]
        [Range(1,500000000)]
        public long MarketCap { get; set; }
    }
}