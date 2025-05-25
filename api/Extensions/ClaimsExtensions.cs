using System.Security.Claims;

namespace api.Extensions;

public static class ClaimsExtensions {
    public static string GetUsername(this ClaimsPrincipal user) {
        return user.Claims
            .SingleOrDefault(x => x.Type.Equals(ClaimTypes.Name))
            ?.Value // Null-conditional operator to avoid NullReferenceException
               ?? throw new InvalidOperationException("Username claim not found.");
    }
}