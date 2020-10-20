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
                //string authKey = "343590AuEEci4h5f804b3dP1";
                //string mobileNumber = "91" + phoneNumber;
                //string senderId = "CLSIGO";
                //string message = HttpUtility.UrlEncode("Dear Classigoo User, The OTP for login on Classigoo is ");
                //int otp = GenarateOTP();
                //message += otp;

                //var client = new RestSharp.RestClient("http://control.msg91.com/api/sendotp.php");
                //var request = new RestSharp.RestRequest(RestSharp.Method.POST);
                //request.AddParameter("authkey", authKey);
                //request.AddParameter("mobile", mobileNumber);
                //request.AddParameter("message", message);
                //request.AddParameter("sender", senderId);
                //request.AddParameter("otp", otp);
                //RestSharp.IRestResponse response = client.Execute(request);
                //Status status = JsonConvert.DeserializeObject<Status>(response.Content);
                //if (status.type == "error")
                //{
                //    isOTPSent = false;
                //    Library.WriteLog("Unable to send OTP, error msg - " + status.message);
                //}





                string authKey = "343590AuEEci4h5f804b3dP1";
                string mobileNumber = "91" + phoneNumber;
                //string senderId = "CLSIGO";
                string messageTemplate = "5f81d9d233de063d4a41076c";
                string messageVariables = "{COMPANY_NAME" + ":" + "Classigoo}";
                int otp = GenarateOTP();

                var client = new RestSharp.RestClient("https://api.msg91.com/api/v5/otp");
                var request = new RestSharp.RestRequest(RestSharp.Method.GET);
                request.AddParameter("authkey", authKey);
                request.AddParameter("mobile", mobileNumber);
                request.AddParameter("template_id", messageTemplate);
                request.AddParameter("invisible", 1);
                request.AddParameter("otp", otp);
                //request.AddParameter("extra_param", messageVariables);

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
                Library.WriteLog("At sending OTP API", ex);
            }
            return isOTPSent;
        }

        public bool VerifyOTP(string phoneNumber, string otp)
        {
            bool isVerified = true;
            try
            {
                //string authKey = "343590AuEEci4h5f804b3dP1";
                //string mobileNumber = "91" + phoneNumber;
                //var client = new RestSharp.RestClient("https://control.msg91.com/api/verifyRequestOTP.php");
                //var request = new RestSharp.RestRequest(RestSharp.Method.POST);
                //request.AddParameter("authkey", authKey);
                //request.AddParameter("mobile", mobileNumber);
                //request.AddParameter("otp", otp);
                //RestSharp.IRestResponse response = client.Execute(request);
                //Status status = JsonConvert.DeserializeObject<Status>(response.Content);
                //if (status.type == "error")
                //{
                //    isVerified = false;
                //    Library.WriteLog("Unable to verify OTP, error msg - " + status.message);
                //}


                string authKey = "343590AuEEci4h5f804b3dP1";
                string mobileNumber = "91" + phoneNumber;
                var client = new RestSharp.RestClient("https://api.msg91.com/api/v5/otp/verify");
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
                //string authKey = "222262AHv0m83QXj5b2fa36c";
                //string mobileNumber = "91" + phoneNumber;
                //var client = new RestSharp.RestClient("http://control.msg91.com/api/retryotp.php");
                //var request = new RestSharp.RestRequest(RestSharp.Method.POST);
                //request.AddParameter("authkey", authKey);
                //request.AddParameter("mobile", mobileNumber);
                //request.AddParameter("retrytype", "text");
                //RestSharp.IRestResponse response = client.Execute(request);
                //Status status = JsonConvert.DeserializeObject<Status>(response.Content);
                //if (status.type == "error")
                //{
                //    isOTPSent = false;
                //    Library.WriteLog("Unable to send OTP, error msg - " + status.message);
                //}


                string authKey = "343590AuEEci4h5f804b3dP1";
                string mobileNumber = "91" + phoneNumber;
                var client = new RestSharp.RestClient("https://api.msg91.com/api/v5/otp/retry");
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

        public void SendMessage(string phoneNumber, string message)
        {

            try
            {
                if (phoneNumber != null && phoneNumber != string.Empty)
                {

                    //string authKey = "222262AHv0m83QXj5b2fa36c";
                    //string mobileNumber = "91" + phoneNumber;
                    //string senderId = "CLSIGO";
                    ////string homePageUrl = Constants.DomainName + "/User/Home";
                    //// var message = new StringBuilder();
                    ////message.AppendLine("Congracts," + userName+"!");
                    ////message.AppendLine("Your Ad published successfully. ");
                    ////message.AppendLine("View and manage adds here: ");
                    ////message.AppendLine(homePageUrl);

                    //var client = new RestSharp.RestClient("http://control.msg91.com/api/sendhttp.php");
                    //var request = new RestSharp.RestRequest(RestSharp.Method.POST);
                    //request.AddParameter("authkey", authKey);
                    //request.AddParameter("mobiles", mobileNumber);
                    //request.AddParameter("message", message);
                    //request.AddParameter("sender", senderId);
                    //request.AddParameter("route", "4");
                    //request.AddParameter("country", "91");
                    //RestSharp.IRestResponse response = client.Execute(request);
                    ////Status status = JsonConvert.DeserializeObject<Status>(response.Content);
                    ////if (status.type == "error")
                    ////{
                    ////    isMsgSent = false;
                    ////    Library.WriteLog("Unable to send Message, error msg - " + status.message);
                    ////}




                    string authKey = "343590AuEEci4h5f804b3dP1";
                    string mobileNumber = "91" + phoneNumber;
                    string senderId = "CLSIGO";
                    string flowId = "5f848639560738296369d4e9";


                    var client = new RestSharp.RestClient("https://api.msg91.com/api/v5/flow/");
                    var request = new RestSharp.RestRequest(RestSharp.Method.POST);
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("authkey", authKey);

                    request.AddParameter("flow_id", flowId);
                    request.AddParameter("sender", senderId);
                                  
                    request.AddParameter("recipients", JsonConvert.SerializeObject(new Recipients() { mobiles = mobileNumber, message = message }));
                   
                    RestSharp.IRestResponse response = client.Execute(request);

                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At sending Message API", ex);
            }
        }

        public void SendMessages(List<string> phoneNumbers, string message)
        {

            try
            {
                if (phoneNumbers.Count > 0)
                {

                    List<Recipients> messages = new List<Recipients>();
                    foreach(var x in phoneNumbers)
                    {
                        messages.Add(new Recipients() { mobiles = "91" + x, message = message });
                    }

                    string authKey = "343590AuEEci4h5f804b3dP1";
                    string senderId = "CLSIGO";
                    string flowId = "5f848639560738296369d4e9";


                    var client = new RestSharp.RestClient("https://api.msg91.com/api/v5/flow/");
                    var request = new RestSharp.RestRequest(RestSharp.Method.POST);
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("authkey", "343590AuEEci4h5f804b3dP1");
                    //request.AddJsonBody(new { flow_id = "5f848639560738296369d4e9", sender= "CLSIGO", recipients= new { mobiles ="919014454730", message ="hello ... world"} });
                    
                    request.AddParameter("flow_id", "5f848639560738296369d4e9");
                    request.AddParameter("sender", "CLSIGO");
                    request.AddParameter("recipients", JsonConvert.SerializeObject(messages));
                    RestSharp.IRestResponse response = client.Execute(request);
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("At sending Message API", ex);
            }
        }
    }

    public class Recipients
    {
        public string mobiles { set; get; }
        public string message { set; get; }
    }

    
}