namespace ComicTracker.Web.Infrastructure
{
    using System.Security.Claims;

    using static ComicTracker.Common.GlobalConstants;

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

        public static bool IsAdmin(this ClaimsPrincipal user)
            => user.IsInRole(AdministratorRoleName);
    }
}
