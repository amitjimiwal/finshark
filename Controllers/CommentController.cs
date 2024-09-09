using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using stockapi.Data;
using stockapi.DTO.Comments;
using stockapi.Interface;
using stockapi.Mapper;

namespace stockapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {   
        private readonly ICommentRepository commentRepository;
        private readonly IStockRepository stockRepository;
        public CommentController(ICommentRepository repository,IStockRepository stRepository)
        {   
            this.commentRepository=repository;
            this.stockRepository=stRepository;
        }

        [HttpGet]
        public async Task<IActionResult?> GetAllComments(){

            //abstraction for validating the incoming data structure , just like zod in javascript/typescript
            if(!ModelState.IsValid)
                return BadRequest(ModelState);


            var comments=await commentRepository.GetAllAsync();
            if(comments==null) return null;
            var commentsDTO=comments.Select(s => s.ToCommentDTO());
            return Ok(comments);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult?> GetCommentByID([FromRoute]int id){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment=await commentRepository.GetByID(id);
            if(comment==null){
                return NotFound();
            }
            return Ok(comment.ToCommentDTO());
        }

        [HttpPost]
        [Route("{stockID:int}")]
        public async Task<IActionResult> CreateComment([FromRoute] int stockID,[FromBody] CreateCommentDTO createCommentDTO){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            //if stock exists
            var StockExists=await stockRepository.StockExists(stockID);
            if(!StockExists) return BadRequest("Stock Does not Exists");

            //create comment from DTO
            var comment=createCommentDTO.ToCommentFromDTO(stockID);
            await commentRepository.CreateComment(comment);

            //created at action with 201 response

            return CreatedAtAction(nameof(GetCommentByID),new{id=comment},comment.ToCommentDTO());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto)
        {
             if(!ModelState.IsValid)
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
        public async Task<IActionResult> DeleteComment([FromRoute] int id){

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var deletedComment=await commentRepository.DeleteAsync(id);
            if(deletedComment==null) return NotFound();

            return Ok(new {
                message=$"Deleted comment with {id}",
                commentDeleted=deletedComment
            });
        }
    }
}