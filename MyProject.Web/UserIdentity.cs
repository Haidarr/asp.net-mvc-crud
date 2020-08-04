using System;
using System.Security.Claims;
using System.Security.Principal;

namespace MyProject.Web
{
    public static class UserIdentity
    {

        public static string GetUserFullName(this IIdentity identity)
        {
            var claimIdentity = (ClaimsIdentity)identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.GivenName);

            return claim.Value;
        }

        public static string GetUserCurrentRole(this IIdentity identity)
        {
            var claimIdentity = (ClaimsIdentity)identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.Role);

            return claim?.Value;
        }


        public static long GetUserID(this IIdentity identity)
        {
            var claimIdentity = (ClaimsIdentity)identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.SerialNumber);

            return long.Parse(claim.Value);
        }


    }
}
