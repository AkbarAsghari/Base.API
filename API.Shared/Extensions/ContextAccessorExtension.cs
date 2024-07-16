using Microsoft.AspNetCore.Http;

namespace API.Shared.Extensions
{
    public static class ContextAccessorExtension
    {
        public static string GetClaimsIP(this HttpContext _accessor)
        {
            var claim = _accessor.User.Claims.FirstOrDefault(it => it.Type == "IP");
            return claim == null ? string.Empty : claim.Value;
        }

        public static Guid GetClaimsUserID(this HttpContext _accessor)
        {
            var claim = _accessor.User.Claims.FirstOrDefault(it => it.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            return claim != null ? Guid.Parse(claim.Value) : Guid.Empty;
        }
    }
}
