using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SutraPlus.Utilities;
using SutraPlus_BAL.Service;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;

namespace SutraPlus.Controllers
{
    [Route("Customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private CustomerService _customerService;
        private MasterDBContext _masterDBContext;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public CustomerController(MasterDBContext MasterDBContext, ILogger<CustomerController> logger, IConfiguration _configuration)
        {
            _masterDBContext = MasterDBContext;
            _logger = logger;
            _customerService = new CustomerService(_masterDBContext, _logger, _configuration);
        }
        /// <summary>
        /// Get Financial Year (Last 3 (N) year) Ordered value
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        [HttpGet("Get/{CustomerId}")]
        [AllowAnonymous]
        public async Task<ActionResult> Get(string CustomerId)
        {
            try
            {
                var result = _customerService.GetYearList(CustomerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Add new customer 
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        [AllowAnonymous]
        public async Task<ActionResult> Add([FromBody] JObject Data)
        {
            try
            {
                var result = _customerService.Add(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get all customer list in active state
        /// </summary>
        /// <returns></returns>
        [HttpPost("List")]
        [AllowAnonymous]
        public async Task<ActionResult> List([FromBody] JObject Data)
        {
            try
            {
                var result = _customerService.List(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetSingle/{Id}")]
        [AllowAnonymous]
        public async Task<ActionResult> Get(int Id)
        {
            try
            {
                var result = _customerService.Get(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Update a single customer with all fields based on Id
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        [AllowAnonymous]
        public async Task<ActionResult> Update([FromBody] JObject Data)
        {
            try
            {
                var result = _customerService.Update(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Set IsActive to 0 (soft delete)
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        [AllowAnonymous]
        public async Task<ActionResult> Delete([FromBody] JObject Data)
        {
            try
            {
                var result = _customerService.Delete(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Set IsActive to 1 i.e. activate the customer which is de-activated
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("Activate")]
        [AllowAnonymous]
        public async Task<ActionResult> Activate([FromBody] JObject Data)
        {
            try
            {
                var result = _customerService.Activate(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Get Financial Year list for last 10 years in Jobject format
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get")]
        [AllowAnonymous]
        public async Task<ActionResult> Get()
        {
            try
            {
                var result = _customerService.Get();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Generate OTP for change in customer data. Accessible for SuperAdmin
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("GenerateOTP")]
        [AllowAnonymous]
        public async Task<ActionResult> GenerateOTP([FromBody] JObject Data)
        {
            try
            {
                var result = _customerService.GenerateOTP(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Validate OTP & update email id of customer
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("ValidateOTP")]
        [AllowAnonymous]
        public async Task<ActionResult> ValidateOTP([FromBody] JObject Data)
        {
            try
            {
                var result = _customerService.ValidateOTP(Data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// search customer based on name, city, pin, mobile, email, first name, last name, gst no in single text field value
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        [HttpPost("Search")] // not in use as used in list
        [AllowAnonymous]
        public async Task<ActionResult> SearchCustomer([FromBody] JObject Data)
        {
            try
            {
                var result = _customerService.SearchCustomer(Data);
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
