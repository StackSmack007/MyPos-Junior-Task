namespace CommonLibrary.Extensions
{
    using System.Linq;
    using System.Security.Claims;

    public static class UserExtensions
    {
        public static string Id(this ClaimsPrincipal user)=>
             user.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;
    }
}
