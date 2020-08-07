using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace BlazorAuth0Demo.Shared
{
    /// <summary>
    /// Class to holds and define the policies used by app
    /// </summary>
    public static class PolicyTypes
    {
        public const string READ_WEATHER = "read:weather";
        public static AuthorizationOptions AddAppPolicies(this AuthorizationOptions options, string issuer)
        {
            options.AddPolicy(READ_WEATHER, policy => policy.Requirements.Add(new HasScopeRequirement(READ_WEATHER, issuer)));
            return options;
        }
    }
}
