using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stockapi.DTO.Account;
using stockapi.Interface;
using stockapi.Models;

namespace stockapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ItokenService itokenService;
        public AccountController(UserManager<AppUser> manager, ItokenService itoken, SignInManager<AppUser> signManager)
        {
            this.itokenService = itoken;
            this.signInManager = signManager;
            this.userManager = manager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUSer([FromBody] RegisterDTO payload)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                //checking if there exist a user with the email
                var userExisted = await userManager.Users.FirstOrDefaultAsync(u => u.Email == payload.EmailAddress);

                if (userExisted != null) return BadRequest("User already exists with the email , please login");


                //creating a new appUser
                var appUser = new AppUser
                {
                    UserName = payload.Username,
                    Email = payload.EmailAddress
                };

                //create a user to the application
                var CreatedUser = await userManager.CreateAsync(appUser, payload.Password);

                if (CreatedUser.Succeeded)
                {
                    //add role to the user
                    var roleResult = await userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded) return Ok(new NewUserResponseDTO
                    {
                        UserName = appUser.UserName,
                        EmailAddress = appUser.Email,
                        Token = itokenService.CreateToken(appUser)
                    });
                    else return StatusCode(500, roleResult.Errors);
                }
                else
                {
                    return StatusCode(500, CreatedUser.Errors);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        [Route("login")]

        public async Task<IActionResult> LoginUser([FromBody] LoginUserDTO payload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userExisted = await userManager.Users.FirstOrDefaultAsync(x => x.Email == payload.EmailAddress);

            if (userExisted == null) return Unauthorized("User doesn't Exists, please register yourself first");

            var result = await signInManager.CheckPasswordSignInAsync(userExisted, payload.Password,false);

            if(!result.Succeeded) return Unauthorized("Password is Incorrect");

            return Ok(new NewUserResponseDTO{
                UserName=userExisted.UserName,
                EmailAddress=userExisted.Email,
                Token=itokenService.CreateToken(userExisted)
            });
        }
    }
}