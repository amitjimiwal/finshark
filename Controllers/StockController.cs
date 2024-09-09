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
            //abstraction for validating the incoming data structure , just like zod in javascript/typescript
            //returns BAd request if data validation fails
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var stocks =await stockRepository.GetAllAsync();
            if(stocks==null){
                return NotFound();
            }
            stocks.Select(s => s.ToStockDto()); //mapping to dto select === map in javascript
            // ok method is used to get 200 response
            return Ok(stocks);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStockById([FromRoute] int id){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var stock=await stockRepository.GetByIDAsync(id);
            if(stock==null){
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockDto stockDto){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            Stock stockModel=stockDto.CreateStockFromDto();
            await stockRepository.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetStockById),new {ID=stockModel.ID},stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> updateStockByID([FromRoute] int id,[FromBody] UpdateStockRequestDto stockUpdateRequest){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock=await stockRepository.UpdateAsync(id,stockUpdateRequest);
            if(stock==null){
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteStockById([FromRoute] int id){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

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