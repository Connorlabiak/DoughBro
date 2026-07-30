using System.Security.Claims;

namespace DoughBro.src.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        /// <summary>
        /// Retrieves the Firebase User ID (UID) from the authenticated principal's claims.
        /// </summary>
        public static string? GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            return principal.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? principal.FindFirstValue("sub");
        }
    }
}