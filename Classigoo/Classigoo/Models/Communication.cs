using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Classigoo.Models
{
    public class Communication
    {
        public bool SendOTP(string phoneNumber)
        {
            bool isOTPSent = true;
            try
            {
            string authKey = "222262AHv0m83QXj5b2fa36c";
            string mobileNumber = "91" + phoneNumber;
            string senderId = "ClassigooOTP";
            string message = HttpUtility.UrlEncode("Dear Classigoo User, The OTP for login on Classigoo is ");
            int otp = GenarateOTP();
            message += otp;
           
                var client = new RestSharp.RestClient("http://control.msg91.com/api/sendotp.php");
                var request = new RestSharp.RestRequest(RestSharp.Method.POST);
                request.AddParameter("authkey", authKey);
                request.AddParameter("mobile", mobileNumber);
                request.AddParameter("message", message);
                request.AddParameter("sender", senderId);
                request.AddParameter("otp", otp);
                RestSharp.IRestResponse response = client.Execute(request);
                Status status = JsonConvert.DeserializeObject<Status>(response.Content);
                if (status.type=="error")
                {
                   isOTPSent = false;
                   Library.WriteLog("Unable to send OTP, error msg - " + status.message);
                }
             
            }
            catch (Exception ex)
            {
                isOTPSent = false;
                Library.WriteLog("At sending OTP API", ex);
            }
            return isOTPSent;
        }

        public bool VerifyOTP(string phoneNumber,string otp)
        {
            bool isVerified = true;
            try
            {
                string authKey = "222262AHv0m83QXj5b2fa36c";
                string mobileNumber = "91" + phoneNumber;
                var client = new RestSharp.RestClient("https://control.msg91.com/api/verifyRequestOTP.php");
                var request = new RestSharp.RestRequest(RestSharp.Method.POST);
                request.AddParameter("authkey", authKey);
                request.AddParameter("mobile", mobileNumber);
                request.AddParameter("otp", otp);
                RestSharp.IRestResponse response = client.Execute(request);
                Status status = JsonConvert.DeserializeObject<Status>(response.Content);
                if (status.type == "error")
                {
                    isVerified = false;
                    Library.WriteLog("Unable to verify OTP, error msg - " + status.message);
                }

            }
            catch (Exception ex)
            {
                isVerified = false;
                Library.WriteLog("At Verify OTP Api", ex);
            }

            return isVerified;
        }

        public bool ResendOTP(string phoneNumber)
        {
            bool isOTPSent = true;
            try
            {
                string authKey = "222262AHv0m83QXj5b2fa36c";
                string mobileNumber = "91" + phoneNumber;
                var client = new RestSharp.RestClient("http://control.msg91.com/api/retryotp.php");
                var request = new RestSharp.RestRequest(RestSharp.Method.POST);
                request.AddParameter("authkey", authKey);
                request.AddParameter("mobile", mobileNumber);
                request.AddParameter("retrytype", "text");
                RestSharp.IRestResponse response = client.Execute(request);
                Status status = JsonConvert.DeserializeObject<Status>(response.Content);
                if (status.type == "error")
                {
                    isOTPSent = false;
                    Library.WriteLog("Unable to send OTP, error msg - " + status.message);
                }
            }
            catch (Exception ex)
            {
                isOTPSent = false;
                Library.WriteLog("At Verify OTP", ex);
            }

            return isOTPSent;
        }

        public int GenarateOTP()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public void SendMessage(string phoneNumber,string userName)
        {
            try
            {
                string authKey = "222262AHv0m83QXj5b2fa36c";
                string mobileNumber = "91" + phoneNumber;
                string senderId = "MSGCLG";
                string homePageUrl = Constants.DomainName + "/User/Home";
                var message = new StringBuilder();
                message.AppendLine("Congracts," + userName+"!");
                message.AppendLine("Your Ad published successfully. ");
                message.AppendLine("View and manage adds here: ");
                message.AppendLine(homePageUrl);
             
                var client = new RestSharp.RestClient("http://control.msg91.com/api/sendhttp.php");
                var request = new RestSharp.RestRequest(RestSharp.Method.POST);
                request.AddParameter("authkey", authKey);
                request.AddParameter("mobiles", mobileNumber);
                request.AddParameter("message", message);
                request.AddParameter("sender", senderId);
                request.AddParameter("route", "4");
                request.AddParameter("country", "91");
                RestSharp.IRestResponse response = client.Execute(request);
                //Status status = JsonConvert.DeserializeObject<Status>(response.Content);
                //if (status.type == "error")
                //{
                //    isMsgSent = false;
                //    Library.WriteLog("Unable to send Message, error msg - " + status.message);
                //}

            }
            catch (Exception ex)
            {
                Library.WriteLog("At sending Message API", ex);
            }
        }
    }
}