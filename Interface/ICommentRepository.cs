using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using stockapi.DTO.Comments;
using stockapi.Models;

namespace stockapi.Interface
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByID(int id);

        Task<Comment> CreateComment(Comment commentModel);

        Task<Comment?> UpdateAsync(int id,Comment updateCommentRequestDto);
    }
}