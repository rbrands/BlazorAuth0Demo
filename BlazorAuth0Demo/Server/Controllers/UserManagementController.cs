using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlazorAuth0Demo.Shared;
using Microsoft.Extensions.Logging;
using BlazorAuth0Demo.Server.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlazorAuth0Demo.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserManagementController : ControllerBase
    {
        private readonly ILogger<UserManagementController> _logger;
        private Auth0Repository _auth0Repository;
        public UserManagementController(ILogger<UserManagementController> logger, Auth0Repository auth0Repository)
        {
            _logger = logger;
            _auth0Repository = auth0Repository;
        }
        private async Task<IActionResult> GetAuth0ManagementToken()
        {
            string accessToken = await _auth0Repository.GetAccessToken();
            return Ok(accessToken);
        }

        [HttpPost("assignpermission")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AssignPermission([FromQuery] string userId, [FromQuery] string registrationCode)
        {
            UserManagmentResult result = new UserManagmentResult { Status = false, Message = String.Empty };
            if (registrationCode.Equals("4711"))
            {
                await _auth0Repository.AssignPermission("https://generic-api", userId, PolicyTypes.READ_WEATHER);
                result.Status = true;
                result.Message = "Assigned";
            }
            else
            {
                result.Status = false;
                result.Message = "Wrong registration code";
            }
            return Ok(result);
        }
    }
}
