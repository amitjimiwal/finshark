using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using stockapi.DTO.Stocks;
using stockapi.Helpers;
using stockapi.Models;

namespace stockapi.Interface
{
    public interface IStockRepository
    {
         Task<List<Stock>> GetAllAsync(QueryObject query);
         Task<Stock?> GetByIDAsync(int id);

         Task<Stock> CreateAsync(Stock stockModel );
         Task<Stock?> UpdateAsync(int id,UpdateStockRequestDto updateStockRequestDto);

         Task<Stock?> DeleteAsync(int id);

         Task<bool> StockExists(int id);
    }
}