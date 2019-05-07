using System;
using System.Collections.Generic;
using System.Linq;
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
		// TODO: 在這裡新增建構函式邏輯
		//
	}
    public static void SendMail(string From, string To, string CC, string Bcc, string Subject, string Body, string[] File, bool IsBodyHtml, bool Receipt)
    {
        SmtpClient smtp = new SmtpClient();
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtp.EnableSsl = false;
        smtp.Host = "10.2.90.2";
        smtp.Port = 25;
        MailMessage mm = new MailMessage(From, To);
        mm.Priority = MailPriority.Normal;
        mm.CC.Add(CC);
        Bcc = Bcc == "" ? "AVSReportS@avision.com.cn" : Bcc;
        mm.Bcc.Add(Bcc);
        mm.Subject = Subject;
        mm.SubjectEncoding = Encoding.UTF8;
        mm.IsBodyHtml = IsBodyHtml;
        mm.BodyEncoding = Encoding.UTF8;
        mm.Body = Body;
        if (Receipt == true)
        { mm.Headers.Add("Disposition-Notification-To", From); }
        //mm.Attachments.Add(new Attachment("c:/a.docx", System.Net.Mime.MediaTypeNames.Application.Rtf));
        try
        {
            smtp.Send(mm);
        }
        catch (System.Net.Mail.SmtpException ex)
        {
            //return ""+ex.Message;
        }

    }
}