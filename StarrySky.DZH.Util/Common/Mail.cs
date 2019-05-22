using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

/// <summary>
/// 发邮件
/// <para>Author:丁振华</para>
/// </summary>
public class Mail
{
    public Mail()
    {
        //
        // TODO
        //
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="to">收件人 逗号隔开</param>
    /// <param name="subject">主题</param>
    /// <param name="body">正文</param>
    /// <param name="isBodyHtml">是否html展现正文</param>
    /// <param name="receipt">被阅读是否回执发件人</param>
    /// <param name="from">发件人</param>
    /// <param name="cc">抄送</param>
    /// <param name="bcc">密送</param>
    public static void SendMail(string to, string subject, string body, bool isBodyHtml = true, bool receipt = false, string from = "", string cc = "", string bcc = "")
    {
        try
        {
            from = string.IsNullOrWhiteSpace(from) ? "dingzhenhua@xxx.com" : from;
            MailAddress fromMail = new MailAddress(from, from);//发送者邮箱   

            MailMessage mm = new MailMessage(fromMail.Address, to);
            mm.Priority = MailPriority.Normal;

            if (!string.IsNullOrWhiteSpace(cc))
            {
                mm.CC.Add(cc);
            }
            if (!string.IsNullOrWhiteSpace(bcc))
            {
                mm.Bcc.Add(bcc);
            }

            mm.Subject = subject;
            mm.SubjectEncoding = Encoding.UTF8;
            mm.IsBodyHtml = isBodyHtml;
            mm.BodyEncoding = Encoding.UTF8;
            mm.Body = body;

            //信息处理通知（MDN）是一条信息返回给 e-mail 信息发送者，指示这条 e-mail 信息已经被打开
            if (receipt == true)
            { mm.Headers.Add("Disposition-Notification-To", from); }
            //附件
            //mm.Attachments.Add(new Attachment("c:/a.docx", System.Net.Mime.MediaTypeNames.Application.Rtf));

            SmtpClient smtp = new SmtpClient();
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("xxxxx", "xxxx");
            smtp.Host = "smtp.exmail.qq.com";//设置发送者邮箱对应的smtpserver  smtp.exmail.qq.com(使用SSL，端口号465)
            //smtp.Port = 25;
            smtp.Timeout = 10000;

            smtp.Send(mm);
        }
        catch (System.Net.Mail.SmtpException ex)
        {
            
        }


    }
}