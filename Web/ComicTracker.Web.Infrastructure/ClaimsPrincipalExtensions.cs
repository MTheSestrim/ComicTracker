namespace ComicTracker.Web.Infrastructure
{
    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            var currentClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (currentClaim != null)
            {
                return currentClaim.Value;
            }

            return null;
        }
    }
}
