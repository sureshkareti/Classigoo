using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

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
            //message += otp;
           
            StringBuilder sbPostData = new StringBuilder();
            sbPostData.AppendFormat("authkey={0}", authKey);
            sbPostData.AppendFormat("&mobile={0}", mobileNumber);
            sbPostData.AppendFormat("&message={0}", message);
            sbPostData.AppendFormat("&sender={0}", senderId);
            sbPostData.AppendFormat("&otp={0}", otp);

             string sendSMSUri = "http://control.msg91.com/api/sendotp.php";
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendSMSUri);
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbPostData.ToString());
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                HttpStatusCode statusCode = response.StatusCode;
                if (statusCode != HttpStatusCode.OK)
                {
                    isOTPSent = false;
                    string status = response.StatusDescription;
                    Library.WriteLog("Unable to send OTP, error msg - " + status);
                }
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                response.Close();
            }
            catch (SystemException ex)
            {
                isOTPSent = false;
                Library.WriteLog("At sending OTP", ex);
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
        
            StringBuilder sbPostData = new StringBuilder();
            sbPostData.AppendFormat("authkey={0}", authKey);
            sbPostData.AppendFormat("&mobile={0}", mobileNumber);
            sbPostData.AppendFormat("&otp={0}", otp);

       
                string verifySMSUri = "https://control.msg91.com/api/verifyRequestOTP.php";
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(verifySMSUri);
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbPostData.ToString());
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
               
                HttpStatusCode statusCode = response.StatusCode;
                if(statusCode!=HttpStatusCode.OK)
                {
                    isVerified = false;
                    string status = response.StatusDescription;
                    Library.WriteLog("Unable to send OTP, error msg - "+status);
                }
                
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();

           
                reader.Close();
                response.Close();
            }
            catch (SystemException ex)
            {
                isVerified = false;
                Library.WriteLog("At Verify OTP", ex);
            }

            return isVerified;
        }

        public bool ResendOTP(string phoneNumber)
        {
            bool isVerified = true;
            try
            {
                string authKey = "222262AHv0m83QXj5b2fa36c";
                string mobileNumber = "91" + phoneNumber;

                StringBuilder sbPostData = new StringBuilder();
                sbPostData.AppendFormat("authkey={0}", authKey);
                sbPostData.AppendFormat("&mobile={0}", mobileNumber);
                sbPostData.AppendFormat("&retrytype={0}", "text");


                string verifySMSUri = "http://control.msg91.com/api/retryotp.php";
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(verifySMSUri);
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbPostData.ToString());
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

                HttpStatusCode statusCode = response.StatusCode;
                if (statusCode != HttpStatusCode.OK)
                {
                    isVerified = false;
                    string status = response.StatusDescription;
                    Library.WriteLog("Unable to send OTP, error msg - " + status);
                }

                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();


                reader.Close();
                response.Close();
            }
            catch (SystemException ex)
            {
                isVerified = false;
                Library.WriteLog("At Verify OTP", ex);
            }

            return isVerified;
        }

        public int GenarateOTP()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }
}