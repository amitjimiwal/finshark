using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace stockapi.DTO.Comments
{
    public class CreateCommentDTO
    {
        [Required]
        [MinLength(5,ErrorMessage ="Title must be atleast 5 characters")]
        [MaxLength(280,ErrorMessage ="Title can't be over 280 characters")]
        public string Title { get; set; }  =string.Empty;

        [Required]
        [MinLength(5,ErrorMessage ="Description must be atleast 5 characters")]
        [MaxLength(280,ErrorMessage ="Description can't be over 280 characters")]
        public string Description { get; set; }=string.Empty;
    }
}