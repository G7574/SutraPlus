using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SutraPlus.Utilities;
using SutraPlus_BAL.Service;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace SutraPlus.Controllers
{
    public class CommonTenantController : ControllerBase
    {
        //IMP : this controller is only to set the TenantId for all controller which is accessing tenantDB
        public string tenantId { get; set; }
        public CommonTenantController(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue("tenantId", out var headerValue))
            {
                tenantId = headerValue;
            }
            //tenantId = _httpContext.Request.Headers["tenantId"];

            //TODO: here .. desearialize the request hearder and extract the tenantID from header
            // Teanant ID =  Financialyear table ID 
        }
    }
}