using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stockapi.Data;
using stockapi.DTO.Stocks;
using stockapi.Interface;
using stockapi.Mapper;
using stockapi.Models;

namespace stockapi.Controllers
{
    [ApiController] //attribute specifying controller
    [Route("api/[controller]")] //attribute specifying route
    public class StockController : ControllerBase
    {
        private readonly IStockRepository stockRepository; //context variable

        public StockController(IStockRepository repository){
            this.stockRepository=repository;
        }
        //get route
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks =await stockRepository.GetAllAsync();
            if(stocks==null){
                return NotFound();
            }
            stocks.Select(s => s.ToStockDto()); //mapping to dto select === map in javascript
            // ok method is used to get 200 response
            return Ok(stocks);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockById([FromRoute] int id){
            var stock=await stockRepository.GetByIDAsync(id);
            if(stock==null){
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockDto stockDto){
            Stock stockModel=stockDto.CreateStockFromDto();
            await stockRepository.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetStockById),new {ID=stockModel.ID},stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> updateStockByID([FromRoute] int id,[FromBody] UpdateStockRequestDto stockUpdateRequest){
            var stock=await stockRepository.UpdateAsync(id,stockUpdateRequest);
            if(stock==null){
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteStockById([FromRoute] int id){
            var stock=await stockRepository.DeleteAsync(id);
            if(stock==null){
                return NotFound();
            }
            return Ok(new {
                message="Successfully deleted the record",
                success=true,
                deletedStock=stock
            });
        }

    }
}