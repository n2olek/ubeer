using System;
using System.Configuration;
using System.Net.Configuration;
using System.Text;
using System.Web;
//using System.Web.Configuration;

using System.Net;
using System.Net.Mail;

namespace S9.Utility
{
    public class EmailUtil
    {
        public string LastError = "";
        public bool IsSSLEnabled = false;

        public bool SendEmail(string to, string subject, string message)
        {
            // Prepare a message
            MailMessage msgMail = new MailMessage();

            try
            {
                msgMail.To.Add(new MailAddress(to));
                msgMail.Subject = subject;

                msgMail.IsBodyHtml = true;
                msgMail.Body = message;

                // Sending out an email
                return Send(msgMail);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return false;
            }
        }

        public bool SendEmail(string to, string cc, string bcc, string subject, string message)
        {
            // Prepare a message
            MailMessage msgMail = new MailMessage();

            try
            {
                msgMail.To.Add(new MailAddress(to));
                if( !string.IsNullOrEmpty(cc) )
                    msgMail.CC.Add(cc);
                
                if( !string.IsNullOrEmpty(bcc) )
                    msgMail.Bcc.Add(bcc);

                msgMail.Subject = subject;
                msgMail.IsBodyHtml = true;
                msgMail.Body = message;

                // Sending out an email
                return Send(msgMail);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return false;
            }
        }

        public bool SendEmail(string[] to, string[] cc, string[] bcc, string subject, string message)
        {
            // Prepare a message
            MailMessage msgMail = new MailMessage();

            try
            {
                if (to != null)
                {
                    for (int iCount = 0; iCount < to.Length; iCount++)
                    {
                        msgMail.To.Add(new MailAddress(to[iCount]));
                    }
                }

                if (cc != null)
                {
                    for (int iCount = 0; iCount < cc.Length; iCount++)
                    {
                        msgMail.CC.Add(new MailAddress(cc[iCount]));
                    }
                }

                if (bcc != null)
                {
                    for (int iCount = 0; iCount < bcc.Length; iCount++)
                    {
                        msgMail.Bcc.Add(new MailAddress(bcc[iCount]));
                    }
                }


                msgMail.Subject = subject;
                msgMail.IsBodyHtml = true;
                msgMail.Body = message;

                // Sending out an email
                return Send(msgMail);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return false;
            }
        }

        // internal method
        private bool Send(MailMessage msgMail)
        {
            try
            {
                /*/
                var smtpcl = new SmtpClient(ConfigurationManager.AppSettings["MailServer"], int.Parse(ConfigurationManager.AppSettings["MailServerPort"]))
                {
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailUserName"], ConfigurationManager.AppSettings["MailPassword"]),
                    EnableSsl = true
                };
                //msgMail.From = new MailAddress(ConfigurationManager.AppSettings["MailUserName"]);                
                /*/

                
                SmtpClient smtpcl = new SmtpClient();
                //smtpcl.EnableSsl = IsSSLEnabled;
                msgMail.BodyEncoding = Encoding.UTF8;
                //smtpcl.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
                
                smtpcl.Send(msgMail);

                return true;
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return false;
            }
        }
    }
}