using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stockapi.Data;
using stockapi.DTO.Stocks;
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

        public async Task<List<Stock>> GetAllAsync(){
            return await dbContext.Stocks.ToListAsync();
        }

        public async Task<Stock?> GetByIDAsync(int id)
        {
            var stockModel=await dbContext.Stocks.FindAsync(id);
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
    }
}