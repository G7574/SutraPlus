using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SutraPlus.Utilities;
using SutraPlus_BAL.Service;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SutraPlus.Controllers
{
    [Route("Security")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private SecurityService _securityService;
        private DBContext _DBContext;
        private MasterDBContext _masterDBContext;
        public SecurityController(DBContext DBContext, MasterDBContext MasterDBContext)
        {
            _masterDBContext = MasterDBContext;
            _DBContext = DBContext;
            _securityService = new SecurityService(_DBContext, _masterDBContext);
        }

        [HttpGet("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] JObject responseData)
        {
            var result = _securityService.Authenticate(responseData);
            return Ok(result);
        }       
    }
}
