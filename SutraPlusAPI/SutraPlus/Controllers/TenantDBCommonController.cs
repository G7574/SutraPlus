using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SutraPlus_BAL.Service;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;

//whole controlled, service & repository developed by Sudhir

namespace SutraPlus.Controllers
{
    [Route("TenantDBCommon")]
    [ApiController]
    public class TenantDBCommonController : CommonTenantController
    {
        private TenantDBCommonService _tenantDBCommonService;
        private MasterDBContext _masterDBContext;
        public IConfiguration _configuration;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public TenantDBCommonController(MasterDBContext MasterDBContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<UserSecurityController> logger) : base(httpContextAccessor)
        {
            _masterDBContext = MasterDBContext;
            _configuration = configuration;
            _logger = logger;
            _tenantDBCommonService = new TenantDBCommonService(Convert.ToInt32(base.tenantId), _masterDBContext, _configuration, _logger);

        }
        //Sudhir Development 18-4-23
        /// <summary>
        /// Get UOM Master from teanantDBCOntext (Dropdown)
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost("GetUnit")]
        [AllowAnonymous]
        public async Task<ActionResult> UnitsDropDown()
        {
            try
            {
                var result = _tenantDBCommonService.UnitsDropDown();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get Ledger Type from teanantDBCOntext (Dropdown)
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        /// 
        [HttpPost("GetLedger")]
        [AllowAnonymous]
        public async Task<ActionResult> LedgerTypeDropDown()
        {
            try
            {
                var result = _tenantDBCommonService.LedgerTypeDropDown();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// GetDealer Type from teanantDBCOntext (Dropdown)
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost("GetDealerType")]
        [AllowAnonymous]
        public async Task<ActionResult> DealerTypeDropDown()
        {
            try
            {
                var result = _tenantDBCommonService.DealerTypeDropDown();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get Voucher Type from teanantDBCOntext (Dropdown)
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost("GetVoucherType")]
        [AllowAnonymous]
        public async Task<ActionResult> GetVoucherType()
        {
            try
            {
                var result = _tenantDBCommonService.GetVoucherTypeList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }



        /// <summary>
        /// Get Accounting Groups from teanantDBCOntext (Dropdown)
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost("GetAccGroup")]
        [AllowAnonymous]
        public async Task<ActionResult> AccounitngGroupsDropDown()
        {
            try
            {
                var result = _tenantDBCommonService.AccounitngGroupsDropDown();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetOtherAccGroup")]
        [AllowAnonymous]
        public async Task<ActionResult> OtherAccounitngGroupsDropDown()
        {
            try
            {
                var result = _tenantDBCommonService.OtherAccounitngGroupsDropDown();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// not required this api from teanantDBCOntext (Dropdown)
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost("GetUserType")]
        [AllowAnonymous]
        public async Task<ActionResult> UserTypeDropDown()
        {
            try
            {
                var result = _tenantDBCommonService.UserTypeDropDown();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Add Party (Ledger which are used in 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("AddLedger")]
        [AllowAnonymous]
        public async Task<ActionResult> AddLedger([FromBody] JObject data)
        {
            try
            {

                var result = _tenantDBCommonService.LedgerAdd(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddOtherLedger")]
        [AllowAnonymous]
        public async Task<ActionResult> AddOtherLedger([FromBody] JObject data)
        {
            try
            {
                var result = _tenantDBCommonService.LedgerOtherAdd(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get List of ledger based on company id  with serach & paginations
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("GetLedgerList")]
        [AllowAnonymous]
        public async Task<ActionResult> GetLedgers([FromBody] JObject data)
        {
            try
            {
                var result = _tenantDBCommonService.GetLedgerList(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get List of ledger based on company id  
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("Get_Ledger")]
        [AllowAnonymous]
        public async Task<ActionResult> GetLedger([FromBody] JObject data)
        {
            try
            {
                var result = _tenantDBCommonService.GetLedger(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Update single ledger based on company id & ledger id
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("UpdateLedger")]
        [AllowAnonymous]
        public async Task<ActionResult> UpdateLedger([FromBody] JObject data)
        {
            try
            {
                var result = _tenantDBCommonService.UptLedger(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update single ledger based on company id & ledger id
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("SetLedger")]
        [AllowAnonymous]
        public async Task<ActionResult> SetLedger([FromBody] JObject data)
        {
            try
            {
                var result = _tenantDBCommonService.SetLedger(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Update other ledger
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("SetOtherLedger")]
        [AllowAnonymous]
        public async Task<ActionResult> UpdateOtherLedger([FromBody] JObject data)
        {
            try
            {
                var result = _tenantDBCommonService.UpdateOtherLedger(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        //
        //Sudhir Development 19-4-23
        /// <summary>
        /// Add Commodity / Product in master table
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost("AddProduct")]
        [AllowAnonymous]
        public IActionResult Add([FromBody] JObject Data)
        {
            try
            {
                var result = _tenantDBCommonService.Add(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get single product based on Id (PK)
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost("GetProduct")]
        [AllowAnonymous]
        public IActionResult GetSingle([FromBody] JObject Data)
        {
            try
            {
                var result = _tenantDBCommonService.GetSingle(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("GetProductList")]
        [AllowAnonymous]
        public IActionResult Get([FromBody] JObject Data)
        {
            try
            {
                var result = _tenantDBCommonService.Get(Data);
                return Ok(result);
            }
            catch (Exception es)
            {
                return BadRequest(es.Message);
            }
        }
        [HttpPost("UpdateProduct")]
        [AllowAnonymous]
        public IActionResult Update([FromBody] JObject Data)
        {
            try
            {
                var result = _tenantDBCommonService.Update(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("DeleteProduct")]
        [AllowAnonymous]
        public async Task<ActionResult> Delete([FromBody] JObject Data)
        {
            try
            {
                var result = _tenantDBCommonService.Delete(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        //Sudhir 20-4-23
        /// <summary>
        /// Add User In Master/Create USer Page
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost("AddUser")]
        [AllowAnonymous]
        public IActionResult AddUser([FromBody] JObject Data)
        {
            try
            {
                var result = _tenantDBCommonService.AddUser(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get single User In  Master /Cerate User 
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost("GetUser")]
        [AllowAnonymous]
        public IActionResult GetUser([FromBody] JObject Data) //TODO check with piyush can he able to pass Id for tenant & id for this 
        {
            try
            {
                var result = _tenantDBCommonService.GetUserSingle(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get User all List in Master Pagein User Creates
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost("GetUserList")]
        [AllowAnonymous]
        public IActionResult GetUserList([FromBody] JObject Data)
        {
            try
            {
                var result = _tenantDBCommonService.GetUserList(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Update single User  based on Id  
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost("UpdateUser")]
        [AllowAnonymous]
        public IActionResult UpdateUser([FromBody] JObject Data)
        {
            try
            {
                var result = _tenantDBCommonService.UpdateUser(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("DeleteUser")]
        [AllowAnonymous]
        public async Task<ActionResult> DeleteUser([FromBody] JObject Data)
        {
            try
            {
                var result = _tenantDBCommonService.DeleteUser(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get All Form Details for Access module in user creation form
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost("GetFormList")] //changed from Post to Get
        [AllowAnonymous]
        public IActionResult GetFormList()
        {
            try
            {
                var result = _tenantDBCommonService.GetFormList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetUserFormList")]
        [AllowAnonymous]
        public IActionResult GetUserFormList()
        {
            try
            {
                var result = _tenantDBCommonService.GetUsrFormList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddAccesss")]
        [AllowAnonymous]
        public IActionResult AddUserNew([FromBody] JObject Data)
        {
            try
            {
                var result = _tenantDBCommonService.UserAccess(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("GetUserAccesss")]
        [AllowAnonymous]
        public IActionResult GetUserAccesss([FromBody] JObject Data)
        {
            try
            {
                var result = _tenantDBCommonService.GetUserAccess(Data);
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
