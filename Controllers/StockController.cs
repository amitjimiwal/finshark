using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stockapi.Data;
using stockapi.DTO.Stocks;
using stockapi.Mapper;
using stockapi.Models;

namespace stockapi.Controllers
{
    [ApiController] //attribute specifying controller
    [Route("api/[controller]")] //attribute specifying route
    public class StockController : ControllerBase
    {
        private readonly AppDbContext _context; //context variable

        public StockController(AppDbContext context){
            this._context=context;
        }
        //get route
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks =await _context.Stocks.ToListAsync();
            stocks.Select(s => s.ToStockDto()); //mapping to dto select === map in javascript
            if(stocks==null){
                return NotFound();
            }
            // ok method is used to get 200 response
            return Ok(stocks);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockById([FromRoute] int id){
            var stock=await _context.Stocks.FindAsync(id);
            if(stock==null){
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockDto stockDto){
            Stock stockModel=stockDto.CreateStockFromDto();
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStockById),new {ID=stockModel.ID},stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> updateStockByID([FromRoute] int id,[FromBody] UpdateStockRequestDto stockUpdateRequest){
            var stock=await _context.Stocks.FirstOrDefaultAsync(x => x.ID==id);
            if(stock==null){
                return NotFound();
            }
            stock.Symbol = stockUpdateRequest.Symbol;
                stock.CompanyName = stockUpdateRequest.CompanyName;
                stock.Purchase = stockUpdateRequest.Purchase;
                stock.LastDiv = stockUpdateRequest.LastDiv;
                stock.Industry = stockUpdateRequest.Industry;
                stock.MarketCap = stockUpdateRequest.MarketCap;
            await _context.SaveChangesAsync();
            return Ok(stock.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteStockById([FromRoute] int id){
            var stock=await _context.Stocks.FirstOrDefaultAsync(x => x.ID==id);
            if(stock==null){
                return NotFound();
            }
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();

            return Ok(new {
                message="Successfully deleted the record",
                success=true
            });
        }

    }
}