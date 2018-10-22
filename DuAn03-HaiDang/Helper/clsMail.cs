using System;
using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
//using Outlook = Microsoft.Office.Interop.Outlook;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DuAn03_HaiDang
{
    class clsMail
    {
        private string strType = "Google"; //1: Google, 2 Yahoo, 3 Hotmail, 4 : AOL, 5: Outlook, 6 Others
        private string strHost = "smtp.gmail.com";
        private int intPort = 25;
        private string strFrom = "vthphong@gmail.com";
        private string strDisplayName = "vthphong";
        private string strPassword = "123456";
        private string strTo = "vtphong@yahoo.com,vqubao@gmail.com";
        private string strSubject = "Test mail";
        private string strBody = "Hi All";
        private ArrayList alAttachments;

        public clsMail()
        {
            alAttachments = new ArrayList();
        }

        public string Type
        {
            get
            {
                return ((string)(strType));
            }
            set
            {
                strType = value;
            }
        }

        public string Host
        {
            get
            {
                return ((string)(strHost));
            }
            set
            {
                strHost = value;
            }
        }

        public int Port
        {
            get
            {
                return ((int)(intPort));
            }
            set
            {
                intPort = value;
            }
        }

        public string From
        {
            get
            {
                return ((string)(strFrom));
            }
            set
            {
                strFrom = value;
            }
        }

        public string DisplayName
        {
            get
            {
                return ((string)(strDisplayName));
            }
            set
            {
                strDisplayName = value;
            }
        }

        public string Password
        {
            get
            {
                return ((string)(strPassword));
            }
            set
            {
                strPassword = value;
            }
        }

        public string To
        {
            get
            {
                return ((string)(strTo));
            }
            set
            {
                strTo = value;
            }
        }

        public string Subject
        {
            get
            {
                return ((string)(strSubject));
            }
            set
            {
                strSubject = value;
            }
        }

        public string Body
        {
            get
            {
                return ((string)(strBody));
            }
            set
            {
                strBody = value;
            }
        }

        public void AddAttachment(string fName)
        {
            alAttachments.Add(fName);
        }

        //public void SendMail()
        //{
        //    try
        //    {
        //        switch (Type)
        //        {
        //            case "Google":
        //                {
        //                    break;
        //                }
        //            case "Yahoo":
        //                {
        //                    break;
        //                }
        //            case "Outlook":
        //                {
        //                    break;
        //                }
        //            case "Hotmail":
        //                {
        //                    break;
        //                }
        //            case "AOL":
        //                {
        //                    break;
        //                }
        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        public bool SendMail()
        {
            try
            {
                if (strType == "Outlook")
                {
                    //// Create the Outlook application.
                    //Outlook.Application oApp = new Outlook.Application();

                    //// Create a new mail item.
                    //Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                    //// Set HTMLBody.
                    ////add the body of the email
                    //oMsg.HTMLBody = strBody;

                    ////Add an attachment.
                    //String sDisplayName = strDisplayName;
                    //int iPosition = (int)oMsg.Body.Length + 1;
                    //int iAttachType = (int)Outlook.OlAttachmentType.olByValue;

                    ////now attached the file
                    //for (int j = 0; j <= alAttachments.Count - 1; j++)
                    //{
                    //    Outlook.Attachment oAttach = oMsg.Attachments.Add(alAttachments[j].ToString(), iAttachType, iPosition, sDisplayName);
                    //}

                    ////Subject line
                    //oMsg.Subject = strSubject;

                    //// Add a recipient.
                    //Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;

                    //// Change the recipient in the next line if necessary.

                    ////Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(this.To);
                    //string pattern = ", ";
                    //Regex rg = new Regex(pattern);
                    //string[] arResult = rg.Split(this.To);
                    //foreach (string subString in arResult)
                    //{
                    //    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(subString);
                    //    oRecip.Resolve();
                    //}
                    ////Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add("vthphong@gmail.com");
                    ////oRecip = (Outlook.Recipient)oRecips.Add("vtphong@yahoo.com");
                    ////oRecip.Resolve();

                    //// Send.
                    //oMsg.Send();

                    //// Clean up.
                    ////oRecip = null;
                    //oRecips = null;
                    //oMsg = null;
                    //oApp = null;
                    ////end of try block
                }
                else
                {
                    // Building the Message
                    MailMessage msg = new MailMessage();
                    msg.To.Add(this.To);
                    msg.From = new MailAddress(strFrom, strDisplayName, System.Text.Encoding.UTF8);
                    msg.Subject = strSubject;
                    msg.SubjectEncoding = System.Text.Encoding.UTF8;
                    msg.Body = strBody;
                    msg.BodyEncoding = System.Text.Encoding.UTF8;
                    msg.IsBodyHtml = true;
                    msg.Priority = MailPriority.High;
                    

                    for (int j = 0; j <= alAttachments.Count - 1; j++)
                    {
                        msg.Attachments.Add(new Attachment(alAttachments[j].ToString()));
                    }

                    if (strType == "Yahoo" || strType == "AOL")
                    {
                        SmtpClient client = new SmtpClient();
                        client.Credentials = new System.Net.NetworkCredential(strFrom, strPassword);
                        client.Port = intPort;
                        client.Host = strHost;
                        client.EnableSsl = false;
                        client.Send(msg);
                    }
                    else if (strType == "Google" || strType == "Hotmail")
                    {
                        SmtpClient client = new SmtpClient();
                        client.Credentials = new System.Net.NetworkCredential(strFrom, strPassword);
                        client.Port = intPort;
                        client.Host = strHost;
                        client.EnableSsl = true;
                        client.Send(msg);
                    }
                    else
                    {
                        SmtpClient client = new SmtpClient();
                        client.Credentials = new System.Net.NetworkCredential(strFrom, strPassword);
                        client.Port = intPort;
                        client.Host = strHost;
                        client.EnableSsl = true;
                        client.EnableSsl = true;
                        ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                        {
                            return true;
                        };
                        client.Send(msg);
                    }

                    //client.Send(msg);

                    //Close
                    msg.Dispose();
                }

                return true;
            }
            catch (SmtpException ex)
            {
                MessageBox.Show(ex.Message, "SendMail");
                return false;
            }
        }

        /*
      OutLook.Application mailApp = new OutLook.Application();
      OutLook.NameSpace myNam = mailApp.GetNamespace("MAPI");

      myNam.Logon(null, null, true, true);


      OutLook.MAPIFolder ofold = myNam.GetDefaultFolder(OutLook.OlDefaultFolders.olFolderSentMail);
      OutLook._MailItem mi = (OutLook._MailItem)mailApp.CreateItem(OutLook.OlItemType.olMailItem);
      mi.To = mailto.Text;
      mi.CC = emailcc.Text;//here is where i added additional CC's (that was easy) lol
      mi.CC = emailcc.Text;
      mi.CC = emailcc.Text;
      mi.CC = emailcc.Text;
      mi.CC = emailcc.Text;
      mi.CC = emailcc.Text;
      mi.CC = emailcc.Text;
      mi.CC = emailcc.Text;
      mi.Subject = subject.Text;
      mi.HTMLBody = body.Text;// i'm building the body in a sepparate call. It can be empty as well
      mi.SaveSentMessageFolder = ofold;
      mi.Send();
         */

    }
}
