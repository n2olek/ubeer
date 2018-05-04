using S9.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBeer.Services
{
    public class SendEmailService
    {
        public void SendErrorNotification(string emailTo, string subject, string body)
        {
            string[] emailTos = emailTo.Split(',');
            EmailUtil emailUtil = new EmailUtil();
            bool isSent = emailUtil.SendEmail(emailTos, null, null, subject, body);
        }
    }
}
