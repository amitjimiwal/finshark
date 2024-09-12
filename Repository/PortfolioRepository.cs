using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stockapi.Data;
using stockapi.Interface;
using stockapi.Models;

namespace stockapi.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly IStockRepository stockRepository;
        public PortfolioRepository(AppDbContext context, IStockRepository stock)
        {
            this.appDbContext = context;
            this.stockRepository = stock;
        }

        public async Task<Portfolio> CreatePortfolio(Portfolio portfolio)
        {
            await appDbContext.Portfolios.AddAsync(portfolio);
            await appDbContext.SaveChangesAsync();

            return portfolio;
        }

        public async Task<Portfolio> DeletePortfolio(AppUser user, string Symbol)
        {
            var stockToBeDeleted = await appDbContext.Portfolios.FirstOrDefaultAsync(x => x.AppUserId == user.Id && x.Stock.Symbol.ToLower() == Symbol.ToLower());
            appDbContext.Portfolios.Remove(stockToBeDeleted);
            await appDbContext.SaveChangesAsync();

            return stockToBeDeleted;
        }

        public async Task<List<Stock>> GetPortfolio(AppUser user)
        {
            return await appDbContext.Portfolios.Where(u => u.AppUserId == user.Id).Select(stock => new Stock
            {
                ID = stock.StockID,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                LastDiv = stock.Stock.LastDiv,
                Industry = stock.Stock.Industry,
                MarketCap = stock.Stock.MarketCap
            }).ToListAsync();
        }
    }
}