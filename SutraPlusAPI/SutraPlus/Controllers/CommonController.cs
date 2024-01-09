using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NLog;
using SutraPlus.Utilities;
using SutraPlus_BAL.Service;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;

namespace SutraPlus.Controllers
{
    [Route("Common")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        //shivaji update : this controller points to MasterDatabase (SutraPlus)
        private CommonService _commonService;
        private MasterDBContext _masterDBContext;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public CommonController(MasterDBContext MasterDBContext, ILogger<CommonController> logger)
        {
            _masterDBContext = MasterDBContext;
            _logger = logger;
            _commonService = new CommonService(_masterDBContext,_logger);
        }

        [HttpGet("Get")]
        [AllowAnonymous]
        public async Task<ActionResult> GetStates()
        {
            try
            {
                _logger.LogDebug("Inside getstatus method");
                var result = _commonService.GetStates();
                return Ok(result);
            }
            catch (Exception ex)
            {
               _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetFinancialYear")]
        [AllowAnonymous]
        public async Task<ActionResult> GetFinancialYears()
        {
            try
            { 
                var result = _commonService.GetFinancialYears();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        //Sudhir 20-4-23
        [HttpGet("GetCountry")]
        [AllowAnonymous]
        public async Task<ActionResult> GetCounties()
        {
            try
            { 
                var result = _commonService.GetCounties();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
    }
}
