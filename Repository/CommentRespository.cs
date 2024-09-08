using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stockapi.Data;
using stockapi.DTO.Comments;
using stockapi.Interface;
using stockapi.Models;

namespace stockapi.Repository
{
    public class CommentRespository:ICommentRepository
    {
        private readonly AppDbContext dbContext;
         public CommentRespository(AppDbContext context)
         {  
            this.dbContext=context;
         }

        public async Task<Comment> CreateComment(Comment commentModel)
        {
            await dbContext.Comments.AddAsync(commentModel);
            await dbContext.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<Comment>> GetAllAsync(){
            return await dbContext.Comments.ToListAsync();
         }

         public async Task<Comment?> GetByID(int id){
            var comment=await dbContext.Comments.FindAsync(id);
            if(comment==null) return null;
            return comment;
         }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
               var existingComment = await dbContext.Comments.FindAsync(id);

            if (existingComment == null)
            {
                return null;
            }

            existingComment.Title = commentModel.Title;
            existingComment.Description = commentModel.Description;

            await dbContext.SaveChangesAsync();
            return existingComment;
        }
    }
}