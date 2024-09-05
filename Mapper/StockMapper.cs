using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using stockapi.DTO.Stocks;
using stockapi.Models;

namespace stockapi.Mapper
{
    public static class StockMapper
    {
        public static StockDto ToStockDto(this Stock stock)
        {
            return new StockDto
            {
                ID = stock.ID,
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                Purchase = stock.Purchase,
                LastDiv = stock.LastDiv,
                Industry = stock.Industry,
                MarketCap = stock.MarketCap
            };
        }
        public static Stock CreateStockFromDto(this CreateStockDto stockDto){
            return new Stock(){
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Purchase = stockDto.Purchase,
                LastDiv = stockDto.LastDiv,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap
            };
        }
    }
}