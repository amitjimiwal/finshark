using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using stockapi.Extensions;
using stockapi.Interface;
using stockapi.Models;

namespace stockapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IStockRepository stockRepository;
        private readonly IPortfolioRepository portfolioRepository;
        public PortfolioController(UserManager<AppUser> user,IStockRepository stockRepo,IPortfolioRepository portfolio)    
        {   
            this.userManager=user;
            this.stockRepository=stockRepo;
            this.portfolioRepository=portfolio;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio(){
            string username=User.GetUserName();
            // string email=User.GetEmail();

            //find the user

            var appUser=await userManager.FindByNameAsync(username);
            var userPortfolio=await portfolioRepository.GetPortfolio(appUser);

            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePortfolio(string Symbol){
            string username=User.GetUserName();
            //find the user
            var appUser=await userManager.FindByNameAsync(username);

            var stock=await stockRepository.GetBySymbolAsync(Symbol);
            if(stock==null) return BadRequest("Stock Not Found");
            
            var userPortfolio=await portfolioRepository.GetPortfolio(appUser);
            if(userPortfolio.Any(p => p.Symbol.ToLower()==Symbol.ToLower())) return BadRequest("CANNOT ADD STOCK WITH THE SAME NAME");

            var newPortfolio=new Portfolio{
                AppUserId=appUser.Id,
                StockID=stock.ID
            };

            if(newPortfolio==null) return StatusCode(500,"Couldn't create portfolio");

            await portfolioRepository.CreatePortfolio(newPortfolio);

            return Created();
        }
    }
}