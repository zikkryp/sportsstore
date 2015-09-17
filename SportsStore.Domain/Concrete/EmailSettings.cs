using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "";
        public string MailFromAddress = "";
        public bool UseSsl = true;
        public string Username = "";
        public string Password = "";
        public string ServerName = "";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"D:\\sports_store_emails";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings settings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            this.settings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using (var client = new SmtpClient())
            {
                client.EnableSsl = settings.UseSsl;
                client.Host = settings.ServerName;
                client.Port = settings.ServerPort;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(settings.Username, settings.Password);

                if (settings.WriteAsFile)
                {
                    client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    client.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder().AppendLine("A new order has been submitted").AppendLine("---").AppendLine("Items:");

                foreach (var line in cart.Lines)
                {
                    var subTotal = line.Product.Price * line.Quantity;

                    body.AppendFormat("{0} x {1}", line.Quantity, line.Product.Name);
                }

                body.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Ship to:")
                    .AppendLine(shippingInfo.Name)
                    .AppendLine(shippingInfo.Line1)
                    .AppendLine(shippingInfo.Line2 ?? "")
                    .AppendLine(shippingInfo.Line3 ?? "")
                    .AppendLine(shippingInfo.City)
                    .AppendLine(shippingInfo.State ?? "")
                    .AppendLine(shippingInfo.Country)
                    .AppendLine(shippingInfo.Zip)
                    .AppendLine("---")
                    .AppendFormat("Gift wrap: {0}",
                    shippingInfo.GiftWrap ? "Yes" : "No");

                MailMessage message = new MailMessage(settings.MailFromAddress, settings.MailToAddress, "New order submitted!", body.ToString());
                
                if (settings.WriteAsFile)
                {
                    message.BodyEncoding = Encoding.ASCII;
                }

                client.Send(message);
            }
        }
    }
}
