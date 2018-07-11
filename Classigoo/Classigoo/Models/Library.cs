using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
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
                sw.WriteLine(CustomActions.GetCurrentISTTime().ToString() + " : " + ex.Source.ToString().Trim() + " ; " + ex.Message.ToString().Trim());
                if(ex.InnerException != null)
                {
                    sw.WriteLine(CustomActions.GetCurrentISTTime().ToString() + " : " + ex.Source.ToString().Trim() + " ; " + Convert.ToString( ex.InnerException.Message));
                }


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

                sw.WriteLine(CustomActions.GetCurrentISTTime().ToString() + " : " + Message);
                sw.WriteLine(ex.Source.ToString().Trim() + " ; " + ex.Message.ToString().Trim());
                if (ex.InnerException != null)
                {
                    sw.WriteLine(CustomActions.GetCurrentISTTime().ToString() + " : " + ex.Source.ToString().Trim() + " ; " + Convert.ToString(ex.InnerException.Message));
                }
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
                    sw.WriteLine(CustomActions.GetCurrentISTTime().ToString() + " : " + Message);
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

        public static void EmailErrors(string emailErrorString)
        {
            try
            {

                // string fromAddress = ConfigurationManager.AppSettings["FromAddress"];smtp.gmail.com
                string fromAddress = "classigoo2018@gmail.com";
              //string fromAddressPassword = ConfigurationManager.AppSettings["frmAddressPassword"];
                string fromAddressPassword = "19052018";
              //  string toAddress = ConfigurationManager.AppSettings["ToAddress"];
                string toAddress = "classigoo2018@gmail.com";
                // string smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
                string smtpServer = "smtp.gmail.com";
                // string subject = ConfigurationManager.AppSettings["Subject"];
                string subject = "Erro occured in Classigoo";
                //string port = ConfigurationManager.AppSettings["Port"];
                string port = "587";

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

        public static void SendEmail(string addId)
        {
            try
            {
                string fromAddress = "classigoo2018@gmail.com";
                string fromAddressPassword = "19052018";
                string toAddress = "classigoo2018@gmail.com";
                string smtpServer = "smtp.gmail.com";
                string subject = "New Ad Published";
                string port = "587";

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                message.From = new MailAddress(fromAddress);
                message.To.Add(new MailAddress(toAddress));
                message.Subject = subject;
                message.IsBodyHtml = true;
                string addUrl = Constants.DomainName + "/List/PreviewAdd?addId="+addId+"";
               // addUrl = "< a href =\"" + addUrl + "\">" + "here" + "</a>";
                // string Body= "<html><body> <span> Hello Admin,</span></br><span> New Ad was published AdId: "+addId+"</span></br><span>Preview Add: "+addUrl+"</ body ></ html > ";
                // message.Body = Body;
                var body = new StringBuilder();
                body.AppendLine("Hello Admin,");
                body.AppendLine();
                body.AppendLine("New Ad was published.");
                body.AppendLine();
                body.AppendLine("AdId: "+addId+"");
                body.AppendLine();
                body.AppendLine("Preview Add: <a href=\""+ addUrl + "\">"+addUrl+"</a>");
                body.AppendLine();
                message.Body = body.ToString();

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

        public static void SendEmailGodaddy(string addId)
        {
            try
            {
                //Create the msg object to be sent
                MailMessage msg = new MailMessage();
                //Add your email address to the recipients
                msg.To.Add("classigoo2018@gmail.com");
                //Configure the address we are sending the mail from
                MailAddress address = new MailAddress("classigoo2018@gmail.com");
                msg.From = address;
                msg.Subject = "New Ad Published";
                msg.IsBodyHtml = true;
                string addUrl = Constants.DomainName + "/List/PreviewAdd?addId=" + addId + "";
                // addUrl = "< a href =\"" + addUrl + "\">" + "here" + "</a>";
                // string Body= "<html><body> <span> Hello Admin,</span></br><span> New Ad was published AdId: "+addId+"</span></br><span>Preview Add: "+addUrl+"</ body ></ html > ";
                // message.Body = Body;
                var body = new StringBuilder();
                body.AppendLine("Hello Admin,");
                body.AppendLine();
                body.AppendLine("New Ad was published.");
                body.AppendLine();
                body.AppendLine("AdId: " + addId + "");
                body.AppendLine();
                body.AppendLine("Preview Add: <a href=\"" + addUrl + "\">" + addUrl + "</a>");
                body.AppendLine();
                msg.Body = body.ToString();
               // msg.Body = txtName.Text + "n" + txtEmail.Text + "n" + txtMessage.Text;

                //Configure an SmtpClient to send the mail.            
                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Host = "smtp.gmail.com";
                client.Port = 25;

                //Setup credentials to login to our sender email address ("UserName", "Password")
                NetworkCredential credentials = new NetworkCredential("classigoo2018@gmail.com", "19052018");
                client.UseDefaultCredentials = true;
                client.Credentials = credentials;

                //Send the msg
                client.Send(msg);

                //Display some feedback to the user to let them know it was sent
                //lblResult.Text = "Your message was sent!";
            }
            catch (Exception ex)
            {
                Models.Library.WriteLog("undergoes into exception while Sending email of Errors ;below is exception  ", ex);
                //If the message failed at some point, let the user know
                // lblResult.Text = ex.ToString();
                //"Your message failed to send, please try again."
            }
        }
    }
}