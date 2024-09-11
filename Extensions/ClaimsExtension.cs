using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace stockapi.Extensions
{
    public static class ClaimsExtension
    {
        public static string GetUserName(this ClaimsPrincipal user){
            return user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")).Value;
        }
        public static string GetEmail(this ClaimsPrincipal user){
            return user.Claims.SingleOrDefault(x => x.Type.Equals("https://schemas.xmlsoap.org/ws/2005/05/identity/claims/email")).Value;
        }
    }
}