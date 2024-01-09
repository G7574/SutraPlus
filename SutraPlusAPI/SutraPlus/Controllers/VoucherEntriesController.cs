using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SutraPlus_BAL.Service;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;
using SutraPlus_DAL.Repository;
using System.Diagnostics;

namespace SutraPlus.Controllers
{
    [Route("VoucherEntries")]
    [ApiController]
    public class VoucherEntriesController : CommonTenantController
    {
        private VoucherEntriesService _VoucherEntriesService;
        private MasterDBContext _masterDBContext;
        public IConfiguration _configuration;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public VoucherEntriesController(MasterDBContext MasterDBContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<SalesController> logger) : base(httpContextAccessor)
        {
            _masterDBContext = MasterDBContext;
            _configuration = configuration;
            _logger = logger;
            _VoucherEntriesService = new VoucherEntriesService(Convert.ToInt32(base.tenantId), _masterDBContext, _configuration, _logger);
        }


        /// <summary>
        /// Add Bank/Journal Entries
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost("AddBankJournalEntries")]
        [AllowAnonymous]
        public IActionResult AddBankJournalEntries([FromBody] JObject Data)
        {
            try
            {
                var result = _VoucherEntriesService.AddBankJournalEntries(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }

        }


        /// <summary>
        /// Add Bank/Journal Entries
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost("AddCashEntries")]
        [AllowAnonymous]
        public IActionResult AddCashEntries([FromBody] JObject Data)
        {
            try
            {
                var result = _VoucherEntriesService.AddCashEntries(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }

        }





        [HttpPost("Get")]
        [AllowAnonymous]
        public async Task<ActionResult> Get([FromBody] JObject Data)
        {
            try
            {
                var result = _VoucherEntriesService.GetVoucherTypeList();
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
