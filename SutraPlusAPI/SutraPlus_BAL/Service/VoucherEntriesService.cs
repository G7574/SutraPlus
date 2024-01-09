using Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SutraPlus.Models;
using SutraPlus_DAL.Common;
using SutraPlus_DAL.Models;
using SutraPlus_DAL.Models.DTO;
using SutraPlus_DAL.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutraPlus_BAL.Service
{
    public class VoucherEntriesService
    {
        public VoucherEntriesRepository _VoucherEntriesRepository = null;
        private MasterDBContext _masterDBContext;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public IConfiguration _configuration { get; }
        private EmailSender _emailSender = null;

        public VoucherEntriesService(int tenantID, MasterDBContext masterDBContext, IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _masterDBContext = masterDBContext;
            _logger = logger;
            _configuration = configuration;
            _emailSender = new EmailSender(_configuration);
            _VoucherEntriesRepository = new VoucherEntriesRepository(tenantID, masterDBContext, _configuration, _logger);
        }

        public string AddBankJournalEntries(JObject Data)
        {
            try
            {
                return _VoucherEntriesRepository.AddBankJournalEntries(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }

        public string AddCashEntries(JObject Data)
        {
            try
            {
                return _VoucherEntriesRepository.AddCashEntries(Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }



        public JObject GetVoucherTypeList()
        {
            try
            {
                var response = new JObject();                
                response = _VoucherEntriesRepository.GetVoucherTypeList();
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }



    }
}
