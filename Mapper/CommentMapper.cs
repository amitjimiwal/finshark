using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using stockapi.DTO.Comments;
using stockapi.Models;

namespace stockapi.Mapper
{
    public static class CommentMapper
    {
        public static CommentDTO ToCommentDTO(this Comment comment){
            return new CommentDTO(){
                ID=comment.ID,
                Title=comment.Title,
                Description=comment.Description,
                CreatedOn=comment.CreatedOn,
                StockID=comment.StockID
            };
        }
        public static Comment ToCommentFromDTO(this CreateCommentDTO commentDTO,int stockID){
            return new Comment(){
                Title=commentDTO.Title,
                Description=commentDTO.Description,
                StockID=stockID
            };
        }
        public static Comment ToCommentFromUpdate(this UpdateCommentRequestDto commentDto, int stockId){
            return new Comment
            {
                Title = commentDto.Title,
                Description = commentDto.Description,
                StockID = stockId
            };
        }
    }
}