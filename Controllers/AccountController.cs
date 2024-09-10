using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using stockapi.DTO.Account;
using stockapi.Models;

namespace stockapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]


    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        public AccountController(UserManager<AppUser> manager)  
        {   
            this.userManager=manager;
        }    

        [HttpPost]
        [Route("register")]
         public async Task<IActionResult> RegisterUSer([FromBody] RegisterDTO payload)
         {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                //creating a new appUser
                var appUser=new AppUser
                {
                    UserName=payload.Username,
                    Email=payload.EmailAddress
                };

                //create a user to the application
                var CreatedUser=await userManager.CreateAsync(appUser,payload.Password);

                if(CreatedUser.Succeeded){
                    //add role to the user
                    var roleResult=await userManager.AddToRoleAsync(appUser,"User");
                    if(roleResult.Succeeded) return Ok("User Created");
                    else return StatusCode(500,roleResult.Errors);
                }else{
                    return StatusCode(500,CreatedUser.Errors);
                }

            }
            catch (Exception ex)
            {
                 // TODO
                 return StatusCode(500,ex);
            }
         }
    }
}