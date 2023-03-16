using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run([BlobTrigger("files/{name}", Connection = "DefaultEndpointsProtocol=https;AccountName=reenbitcampteststorage;AccountKey=V8L4BGxEGM8H8jUxGoelNkfP+NffrymeyIlDOSJ1A+e2VTV20GAB/56CGWAHiOuWlRrPhAy/rsOu+AStwkH/+g==;EndpointSuffix=core.windows.net")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            
            // Send email notification to user
            SendEmailNotification(name);
        }
        private static void SendEmailNotification(string filename)
        {
            // Set up email message
            string subject = "File uploaded successfully";
            string body = $"The file '{filename}' was successfully uploaded to your Blob storage account.";
            string recipient = "roman.lesheha@lnu.edu.ua"; 

            // Send email using SMTP client
            using (var client = new SmtpClient())
            {
                // Configure SMTP client settings
                client.Host = "smtp.example.com";
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("roman.leshega2003@gmail.com", "romanlesh123"); 

                // Create email message
                var message = new MailMessage(
                    new MailAddress("roman.leshega2003@gmail.com", "Your Name"),
                    new MailAddress(recipient))
                {
                    Subject = subject,
                    Body = body
                };

                // Send email message
                client.Send(message);
            }
        }
    }
}
