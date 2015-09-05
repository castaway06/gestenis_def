using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;

namespace GesTenis.tools
{
    public class Tools
    {
        public static string SHA256Encrypt(string input)
        {
            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedBytes = provider.ComputeHash(inputBytes);

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
                output.Append(hashedBytes[i].ToString("x2").ToLower());

            return output.ToString();
        }

        public static void sendEmail(socios socio, string subject, string body)
        {

            var fromAddress = new MailAddress("proyecto.gestenis@gmail.com", "Proyecto Gestenis");
            string fromPassword = "adminsocio12";
            var toAddress = new MailAddress(socio.email, socio.nombre);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                try // TODO: Mirar que hacer con esta excepción
                {
                    smtp.Send(message);
                }
                catch (Exception e)
                {
                    // ´TODO: mirar que hacer con esta excepción
                    Debug.WriteLine(e);
                }
            }

        }

        public static string randomPassword(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        public static string generarXmlFactura(facturas factura, reservas reserva)
        {
            string ret = "xml para factura " + factura.id + ". Socio: " + reserva.socios.nombre + " " + reserva.socios.apellidos
                + ". Ha reservado el recurso " + reserva.recursos.nombre_rec + " el dia " + reserva.fecha + " a las " + reserva.hora
                + ". El precio es de " + reserva.precio + " Euros.";
            return ret;
        }

    }
}