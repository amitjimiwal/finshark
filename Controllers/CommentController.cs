using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using stockapi.Data;
using stockapi.DTO.Comments;
using stockapi.Extensions;
using stockapi.Interface;
using stockapi.Mapper;
using stockapi.Models;
using stockapi.Service;

namespace stockapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository commentRepository;
        private readonly IStockRepository stockRepository;
        private readonly UserManager<AppUser> userManager;
        private readonly IFinancialService fmpService;
        public CommentController(ICommentRepository repository, IStockRepository stRepository, UserManager<AppUser> manager, IFinancialService fmp)
        {
            this.commentRepository = repository;
            this.stockRepository = stRepository;
            this.userManager = manager;
            this.fmpService = fmp;
        }

        [HttpGet]
        public async Task<IActionResult?> GetAllComments()
        {

            //abstraction for validating the incoming data structure , just like zod in javascript/typescript
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var comments = await commentRepository.GetAllAsync();
            if (comments == null) return null;
            var commentsDTO = comments.Select(s => s.ToCommentDTO());
            return Ok(comments);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult?> GetCommentByID([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await commentRepository.GetByID(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDTO());
        }

        [HttpPost]
        [Route("{symbol:alpha}")]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromRoute] string symbol, [FromBody] CreateCommentDTO createCommentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //if stock exists
            var stock = await stockRepository.GetBySymbolAsync(symbol);
            if (stock == null)
            {
                //populate the stock from FMP service
                stock=await fmpService.FindStockByName(symbol);
                if(stock==null){
                    return BadRequest("Stock doesn't exists");
                }else{
                    await stockRepository.CreateAsync(stock);
                }
            }

            var username = User.GetUserName();
            var appUser = await userManager.FindByNameAsync(username);
            if (appUser == null) return Unauthorized("You are not authorized to create comments");
            //create comment from DTO
            var comment = createCommentDTO.ToCommentFromDTO(stock.ID);
            comment.AppUserId = appUser.Id;
            await commentRepository.CreateComment(comment);

            //created at action with 201 response

            return CreatedAtAction(nameof(GetCommentByID), new { id = comment.ID }, comment.ToCommentDTO());
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await commentRepository.UpdateAsync(id, updateDto.ToCommentFromUpdate(id));

            if (comment == null)
            {
                return NotFound("Comment not found");
            }
            return Ok(comment.ToCommentDTO());
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deletedComment = await commentRepository.DeleteAsync(id);
            if (deletedComment == null) return NotFound();

            return Ok(new
            {
                message = $"Deleted comment with {id}",
                commentDeleted = deletedComment
            });
        }
    }
}