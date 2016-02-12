using System.Net.Mail;
using System.Net;
using System;

namespace Services
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Services" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Services.svc o Services.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class Services : IServices
    {
        //Metodo Protocolo permite comunicacion a traves de correso electronico
        public string sendEmail(string emaildes,string subject,string mensaje)
        {
            string strDestinatario = emaildes;
            System.Net.Mail.SmtpClient clienteSMTP = new System.Net.Mail.SmtpClient();
            clienteSMTP.Host = "smtp.live.com";
            clienteSMTP.Port = 25;
            //clienteSMTP.Host = "smtp.gmail.com";
            //clienteSMTP.Port = 587;

            //clienteSMTP.Credentials = new NetworkCredential("knowerconocimiento@gmail.com", "@gestorc");
            clienteSMTP.Credentials = new NetworkCredential("thcf963@hotmail.com", "1234561");

            MailMessage correo = new MailMessage();
            correo.IsBodyHtml = true;

            correo.To.Add(strDestinatario);
            correo.From = new MailAddress("123@123.123");
            correo.Subject = subject;
            string pag = mensaje;
            correo.Body += pag;
            correo.BodyEncoding = System.Text.Encoding.Unicode;
            correo.SubjectEncoding = System.Text.Encoding.Unicode;

            clienteSMTP.EnableSsl = true;
            clienteSMTP.Send(correo);
            return "ok";
        }  
        
        
            
    }
}
