using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stockapi.Data;
using stockapi.DTO.Stocks;
using stockapi.Helpers;
using stockapi.Interface;
using stockapi.Models;

namespace stockapi.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly AppDbContext dbContext;
        public StockRepository(AppDbContext context)
        {
            this.dbContext=context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await dbContext.Stocks.AddAsync(stockModel);
            await dbContext.SaveChangesAsync();

            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel=await dbContext.Stocks.FirstOrDefaultAsync(x => x.ID==id);

            if(stockModel==null) return null;

            dbContext.Stocks.Remove(stockModel);
            await dbContext.SaveChangesAsync();

            return stockModel;
        }

        // Important - All the filters are applied to this function only
        public async Task<List<Stock>> GetAllAsync(QueryObject query){
            var stocks= dbContext.Stocks.Include("Comments").AsQueryable();
            //method to check the string has Null or Whitespace
            if(!string.IsNullOrWhiteSpace(query.CompanyName)){
                stocks=stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            };
            if(!string.IsNullOrWhiteSpace(query.Symbol)){
                stocks=stocks.Where(s => s.Symbol.Contains(query.Symbol));
            };

            //sorting
            if(!string.IsNullOrWhiteSpace(query.SortBY)){
                //sort by Symbol
                if(query.SortBY.Equals("Symbol",StringComparison.OrdinalIgnoreCase)){
                    stocks=query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                };

                //sort by CompanyName
                if(query.SortBY.Equals("CompanyName",StringComparison.OrdinalIgnoreCase)){
                    stocks=query.IsDescending ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
                };
            }


            //pagination
            int skipNumber=(query.PageNumber==0 ? 0 : query.PageNumber-1)*query.PageSize;
            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIDAsync(int id)
        {
            var stockModel=await dbContext.Stocks.Include(c=> c.Comments).FirstOrDefaultAsync(i => i.ID==id);
            if (stockModel==null)
            {
                return null;
            }
            return stockModel;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateStockRequestDto)
        {
             var stock=await dbContext.Stocks.FirstOrDefaultAsync(x => x.ID==id);
            if(stock==null){
                return null;
            }
            stock.Symbol = updateStockRequestDto.Symbol;
            stock.CompanyName = updateStockRequestDto.CompanyName;
            stock.Purchase = updateStockRequestDto.Purchase;
            stock.LastDiv = updateStockRequestDto.LastDiv;
            stock.Industry = updateStockRequestDto.Industry;
            stock.MarketCap = updateStockRequestDto.MarketCap;
            await dbContext.SaveChangesAsync();

            return stock;
        }

        public async Task<bool> StockExists(int id){
            return await dbContext.Stocks.AnyAsync(s => s.ID==id);
        }

        public async Task<Stock?> GetBySymbolAsync(string Symbol)
        {
             var stockModel=await dbContext.Stocks.Include(c=> c.Comments).FirstOrDefaultAsync(i => i.Symbol==Symbol);
            if (stockModel==null)
            {
                return null;
            }
            return stockModel;
        }
    }
}