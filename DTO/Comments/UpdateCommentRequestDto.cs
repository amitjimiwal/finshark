using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stockapi.DTO.Comments
{
    public class UpdateCommentRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}