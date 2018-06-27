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
    public class CustomActions
    {
        public void SendOTP(string phoneNumber)
        {
            //Your authentication key
            string authKey = "222262AHv0m83QXj5b2fa36c";
            //Multiple mobiles numbers separated by comma
            string mobileNumber = "91" + phoneNumber;
            //Sender ID,While using route4 sender id should be 6 characters long.
            string senderId = "ClassigooOTP";
            //Your message to send, Add URL encoding here.
            string message = HttpUtility.UrlEncode("Dear Classigoo User, The OTP for login on Classigoo is ");
            int otp = 5634;
            message += otp;
            //Prepare you post parameters
            StringBuilder sbPostData = new StringBuilder();
            sbPostData.AppendFormat("authkey={0}", authKey);
            sbPostData.AppendFormat("&mobile={0}", mobileNumber);
            sbPostData.AppendFormat("&message={0}", message);
             sbPostData.AppendFormat("&sender={0}", senderId);
            //sbPostData.AppendFormat("&route={0}", "default");
            sbPostData.AppendFormat("&otp={0}", otp);

            try
            {
                //Call Send SMS API
                string sendSMSUri = "http://control.msg91.com/api/sendotp.php";
                //Create HTTPWebrequest
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendSMSUri);
                //Prepare and Add URL Encoded data
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbPostData.ToString());
                //Specify post method
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                //Get the response
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();

                //Close the response
                reader.Close();
                response.Close();
            }
            catch (SystemException ex)
            {
                //MessageBox.Show(ex.Message.ToString());
            }

        }

        public void VerifyOTP(string phoneNumber,string otp)
        {
            //Your authentication key
            string authKey = "222262AHv0m83QXj5b2fa36c";
            //Multiple mobiles numbers separated by comma
            string mobileNumber = "91" + phoneNumber;
        
            //Prepare you post parameters
            StringBuilder sbPostData = new StringBuilder();
            sbPostData.AppendFormat("authkey={0}", authKey);
            sbPostData.AppendFormat("&mobile={0}", mobileNumber);
            sbPostData.AppendFormat("&otp={0}", otp);

            try
            {
                //Call Send SMS API
                string verifySMSUri = "https://control.msg91.com/api/verifyRequestOTP.php";
                //Create HTTPWebrequest
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(verifySMSUri);
                //Prepare and Add URL Encoded data
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbPostData.ToString());
                //Specify post method
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                //Get the response
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();

                //Close the response
                reader.Close();
                response.Close();
            }
            catch (SystemException ex)
            {
                //MessageBox.Show(ex.Message.ToString());
            }

        }
    }
}