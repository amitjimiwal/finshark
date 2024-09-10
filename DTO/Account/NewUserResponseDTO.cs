using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stockapi.DTO.Account
{
    public class NewUserResponseDTO
    {
        public string? UserName { get; set; } = string.Empty;
        public string? EmailAddress { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}