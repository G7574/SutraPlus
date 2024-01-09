using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment; // this is added
using Newtonsoft.Json.Linq;
using SutraPlus_DAL.Models;

namespace SutraPlus_DAL.Common
{
    public class EmailSender
    {
        private static ILoggerFactory logger = new LoggerFactory();
        private static ILogger _logger = logger.CreateLogger(nameof(EmailSender));
        public static IConfiguration Configuration { get; set; }
        public static IHostingEnvironment _hostingEnvironment { get; set; }

        //static class cunstructor
        private static readonly IConfiguration Root;
        private static readonly ConfigurationBuilder ConfigurationBuilder;

        public EmailSender(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        public static string LoadTemplate(string Template="")
        {
            string input = "";
            try
            {
                var filePath = "";
                if (Template == "")
                {
                    //filePath = "D:\\Templates\\CustomerCreate.htm";// System.Configuration.ConfigurationManager.AppSettings[""];
                    filePath = Configuration.GetSection("Template:TemplateFolderPath").Value.ToString()+ "\\CustomerCreate.htm";
                }
                else
                {
                    filePath = Template;
                }
                if (File.Exists(filePath))
                {
                    input = File.ReadAllText(filePath);
                }
                //Configuration.GetValue<string>("Templates");
                //var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Templates", name);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, nameof(LoadTemplate));
                throw ex;
            }
            return input;
        }

       
        public bool SendMailMessage(JObject emailConfig,string to, string subject, string body,string pass)
        {
            if (emailConfig == null || !emailConfig.Value<bool>("IsSuccess"))
            {
                _logger.LogError("No email configuration found", nameof(SendMailMessage));
                return false;
            }

            string _fromemail = emailConfig.Value<string>("FromEmail");
            string _password = emailConfig.Value<string>("Password");
            string emailServerHost = emailConfig.Value<string>("EmailServerHost");
            string emailServerPort = emailConfig.Value<string>("EmailServerPort");

            MailMessage mail = new MailMessage();
            
            // previous method to get email config
            //string _fromemail = Configuration.GetSection("EmailSenderConfig:FromMail").Value;
            //string _password = Configuration.GetSection("EmailSenderConfig:Password").Value; 
            //string emailServerHost = Configuration.GetSection("EmailSenderConfig:ServerHost").Value; 
            //string emailServerPort = Configuration.GetSection("EmailSenderConfig:ServerPort").Value; 
            
            body = LoadTemplate(Configuration.GetSection("EmailTemplates:ForgotPassword").Value).ToString();// System.Configuration.ConfigurationManager.AppSettings[""];
            body = body.Replace("{0}", to);
            body = body.Replace("{1}", pass);
            int intEmailServerPort = 0;

            try
            {
                if (int.TryParse(emailServerPort, out intEmailServerPort))
                {
                    mail.From = new MailAddress(_fromemail);
                    mail.To.Add(to);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    smtp.Host = emailServerHost;
                    smtp.Port = intEmailServerPort;
                    smtp.Credentials = new System.Net.NetworkCredential(_fromemail, _password);

                    smtp.Send(mail);
                    
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, nameof(SendMailMessage));
                return false;
            }
        }

        public bool SendMailMessage(JObject emailConfig, string to, string subject,string body, Customer customer,string WebUrl,String userName="", string password="")
        {

            if (emailConfig == null || !emailConfig.Value<bool>("IsSuccess"))
            {
                _logger.LogError("No email configuration found", nameof(SendMailMessage));
                return false;
            }

            string _fromemail = emailConfig.Value<string>("FromEmail");
            string _password = emailConfig.Value<string>("Password");
            string emailServerHost = emailConfig.Value<string>("EmailServerHost");
            string emailServerPort = emailConfig.Value<string>("EmailServerPort");

            MailMessage mail = new MailMessage();

            body = LoadTemplate(body);
            body = body.Replace("{0}", customer.ContactPerson);
            body = body.Replace("{1}", customer.Name);
            body = body.Replace("{2}", customer.Code);
            body = body.Replace("{3}", customer.Address);
            body = body.Replace("{4}", customer.Mobile);
            body = body.Replace("{5}", customer.Email);
            body = body.Replace("{6}", WebUrl);
            body = body.Replace("{7}", userName);
            body = body.Replace("{8}", password);



            int intEmailServerPort = 0;

            try
            {
                if (int.TryParse(emailServerPort, out intEmailServerPort))
                {
                    mail.From = new MailAddress(_fromemail);
                    mail.To.Add(to);
                    //mail.CC.Add(cc);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    smtp.Host = emailServerHost;
                    smtp.Port = intEmailServerPort;
                    smtp.Credentials = new System.Net.NetworkCredential(_fromemail, _password);
                    
                    smtp.Send(mail);

                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, nameof(SendMailMessage));
                return false;
            }
        }

        /// <summary>
        /// when new mail is going to update use this method to send otp on new mail id of customer
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static bool SendMailMessage_Mail_OTP(JObject emailConfig, string to, string subject, string body,string otp)
        {

            if (emailConfig == null || !emailConfig.Value<bool>("IsSuccess"))
            {
                _logger.LogError("No email configuration found", nameof(SendMailMessage_Mail_OTP));
                return false;
            }

            string _fromemail = emailConfig.Value<string>("FromEmail");
            string _password = emailConfig.Value<string>("Password");
            string emailServerHost = emailConfig.Value<string>("EmailServerHost");
            string emailServerPort = emailConfig.Value<string>("EmailServerPort");

            MailMessage mail = new MailMessage();

            // previous method to get email config
            //string _fromemail = Configuration.GetSection("EmailSenderConfig:FromMail").Value;
            //string _password = Configuration.GetSection("EmailSenderConfig:Password").Value; 
            //string emailServerHost = Configuration.GetSection("EmailSenderConfig:ServerHost").Value; 
            //string emailServerPort = Configuration.GetSection("EmailSenderConfig:ServerPort").Value; 

            body = LoadTemplate("D:\\Templates\\ChangeCustomerEmail.htm");// System.Configuration.ConfigurationManager.AppSettings[""];
            body = body.Replace("{0}", to);
            body = body.Replace("{1}", otp);
            int intEmailServerPort = 0;

            try
            {
                if (int.TryParse(emailServerPort, out intEmailServerPort))
                {
                    mail.From = new MailAddress(_fromemail);
                    mail.To.Add(to);
                    //mail.CC.Add(cc);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    smtp.Host = emailServerHost;
                    smtp.Port = intEmailServerPort;
                    smtp.Credentials = new System.Net.NetworkCredential(_fromemail, _password);

                    smtp.Send(mail);

                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, nameof(SendMailMessage));
                return false;
            }
        }
        /// <summary>
        /// When Admin wants to forgot & reset password along with OTP use following method
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="otp"></param>
        /// <returns></returns>
        public static bool SendMailMessage_Admi_ForgotPass_OTP(JObject emailConfig, string to, string subject, string body, string otp)
        {

            if (emailConfig == null || !emailConfig.Value<bool>("IsSuccess"))
            {
                _logger.LogError("No email configuration found", nameof(SendMailMessage_Admi_ForgotPass_OTP));
                return false;
            }

            string _fromemail = emailConfig.Value<string>("FromEmail");
            string _password = emailConfig.Value<string>("Password");
            string emailServerHost = emailConfig.Value<string>("EmailServerHost");
            string emailServerPort = emailConfig.Value<string>("EmailServerPort");

            MailMessage mail = new MailMessage();

            body = LoadTemplate("D:\\Templates\\Admin_Forgot_Email_OTP.htm");// System.Configuration.ConfigurationManager.AppSettings[""];
            body = body.Replace("{0}", otp);
           
            int intEmailServerPort = 0;

            try
            {
                if (int.TryParse(emailServerPort, out intEmailServerPort))
                {
                    mail.From = new MailAddress(_fromemail);
                    mail.To.Add(to);
                    //mail.CC.Add(cc);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    smtp.Host = emailServerHost;
                    smtp.Port = intEmailServerPort;
                    smtp.Credentials = new System.Net.NetworkCredential(_fromemail, _password);

                    smtp.Send(mail);

                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, nameof(SendMailMessage));
                return false;
            }
        }
        //Information mail to all admin about creation of User or Admin for same customer
        //add new template 
        public static bool SendMailMessageAdmin(JObject emailConfig, string usermail, string Adminmail, string subject, string body, string pass, string FLName, string date)
        {

            if (emailConfig == null || !emailConfig.Value<bool>("IsSuccess"))
            {
                _logger.LogError("No email configuration found", nameof(SendMailMessage));
                return false;
            }

            string _fromemail = emailConfig.Value<string>("FromEmail");
            string _password = emailConfig.Value<string>("Password");
            string emailServerHost = emailConfig.Value<string>("EmailServerHost");
            string emailServerPort = emailConfig.Value<string>("EmailServerPort");

            MailMessage mail = new MailMessage();

            body = LoadTemplate("D:\\Templates\\AdminSendMail.htm");// System.Configuration.ConfigurationManager.AppSettings[""];
            body = body.Replace("{0}", FLName);
            body = body.Replace("{1}", date);
            body = body.Replace("{2}", usermail);
            body = body.Replace("{3}", pass);
            int intEmailServerPort = 0;

            try
            {
                if (int.TryParse(emailServerPort, out intEmailServerPort))
                {
                    mail.From = new MailAddress(_fromemail);
                    mail.To.Add(Adminmail);
                    //mail.CC.Add(cc);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    smtp.Host = emailServerHost;
                    smtp.Port = intEmailServerPort;
                    smtp.Credentials = new System.Net.NetworkCredential(_fromemail, _password);

                    smtp.Send(mail);

                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, nameof(SendMailMessage));
                return false;
            }
        }
    }
}
