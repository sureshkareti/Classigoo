using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Classigoo.Models
{
    public class Library
    {
        public static void WriteLog(Exception ex)
        {
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\" + Constants.ErrorLogFileName, true);
                sw.WriteLine(DateTime.Now.ToString() + " : " + ex.Source.ToString().Trim() + " ; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();

            }
            catch
            {

            }
        }
        public static void WriteLog(String Message, Exception ex)
        {
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\" + Constants.ErrorLogFileName, true);

                sw.WriteLine(DateTime.Now.ToString() + " : " + Message);
                sw.WriteLine(ex.Source.ToString().Trim() + " ; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();

            }
            catch
            {

            }
        }
        public static void WriteLog(String Message)
        {
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\" + Constants.ErrorLogFileName, true);

                if (Message != "")
                {
                    sw.WriteLine(DateTime.Now.ToString() + " : " + Message);
                    sw.WriteLine("");
                }
                else
                {
                    sw.WriteLine("");
                }
                sw.Flush();
                sw.Close();

            }
            catch
            {

            }
        }

        private static void EmailErrors(string emailErrorString)
        {
            try
            {
              
                string fromAddress = ConfigurationManager.AppSettings["FromAddress"];
                string fromAddressPassword = ConfigurationManager.AppSettings["frmAddressPassword"];

                string toAddress = ConfigurationManager.AppSettings["ToAddress"];
                string smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
                string subject = ConfigurationManager.AppSettings["Subject"];
                string port = ConfigurationManager.AppSettings["Port"];

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                message.From = new MailAddress(fromAddress);
                message.To.Add(new MailAddress(toAddress));
                message.Subject = subject;
                message.Body = emailErrorString;

                smtp.Port = Convert.ToInt32(port);
                smtp.Host = smtpServer;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(fromAddress, fromAddressPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                Models.Library.WriteLog("undergoes into exception while Sending email of Errors ;below is exception  ", ex);
            }
        }
    }
}