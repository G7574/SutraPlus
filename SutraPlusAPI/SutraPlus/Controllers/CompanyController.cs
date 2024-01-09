using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SutraPlus_BAL.Service;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SutraPlus.Controllers
{
    [Route("Company")]
    [ApiController]
    public class CompanyController : CommonTenantController
    {

        private MasterDBContext _masterDBContext;
        private CompanyService _companyService;
        public IConfiguration _configuration;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public CompanyController(MasterDBContext MasterDBContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<UserSecurityController> logger) : base(httpContextAccessor)
        {
            _masterDBContext = MasterDBContext;
            _configuration = configuration;
            _logger = logger;
            _companyService = new CompanyService(Convert.ToInt32(base.tenantId), _masterDBContext, _configuration, _logger);
        }
        /// <summary>
        /// Get All company list based on user mailid, financial year & Company Code
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("List")] //changed from Get to List
        [AllowAnonymous]
        public IActionResult List([FromHeader] int tenantId) //check with piyush list or search api
        {
            try
            {
                var result = _companyService.List();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Add")]
        [AllowAnonymous]
        public IActionResult Add([FromHeader] int tenantId, [FromBody] JObject Data)
        {
            try
            {
                var result = _companyService.Add(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get E Invoice Key based on companyId
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("GetInvoiceKey/{companyId}")] //changed from GetSingle to Get
        [AllowAnonymous]
        public IActionResult GetInv([FromHeader] int tenantId, int companyId)
        {
            try
            {
                var result = _companyService.GetInvoiceKey(companyId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get single company data based on companyId
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("Get/{companyId}")] //changed from GetSingle to Get
        [AllowAnonymous]
        public IActionResult Get([FromHeader] int tenantId, int companyId)
        {
            try
            {
                var result = _companyService.Get(companyId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get single company data based on companyId
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        [AllowAnonymous]
        public IActionResult Update([FromHeader] int tenantId, [FromBody] JObject Data)
        {
            try
            {
                var result = _companyService.Update(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetCompanyEInvoice")]
        [AllowAnonymous]
        public IActionResult GetCompany([FromBody] JObject data)
        {
            int id = data["CompanyId"].Value<int>();
            var result = _companyService.GetCompanyEInvoice(id);

            //var result = _companyService.GetCompanyEInvoice(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("SaveEInvoice")]
        [AllowAnonymous]
        public IActionResult SaveEInvoice([FromBody] JObject Data)
        {
            try
            {
                var result = _companyService.SaveEInvoice(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("SaveAkadaEntry")]
        [AllowAnonymous]
        public IActionResult SaveAkadaEntry([FromBody] JObject Data)
        {
            try
            {
                var result = _companyService.SaveAkadaEntry(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("SaveOptionSettings")]
        [AllowAnonymous]
        public IActionResult SaveOptionSettings([FromBody] JObject Data)
        {
            try
            {
                var result = _companyService.SaveOptionSettings(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetOptionSettings")]
        [AllowAnonymous]
        public IActionResult GetOptionSettings([FromBody] JObject Data)
        {
            try
            {
                var result = _companyService.GetOptionSettings(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// search company based on name, place, pan, email, tan, celphone, gst, 
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("Search")]
        [AllowAnonymous]
        public async Task<ActionResult> Search([FromHeader] int tenantId, [FromBody] JObject Data)//TODO : Pascal casing for method
        {
            try
            {
                var result = _companyService.Search(Data);
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
