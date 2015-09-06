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

        /// <summary>
        /// Genera el código SHA256 de la cadena que pasamos como parametro
        /// </summary>
        /// <param name="input">contraseña a encriptar</param>
        /// <returns>Devuelve el codigo SHA256 del parametro</returns>
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

        /// <summary>
        /// Envía un email
        /// </summary>
        /// <param name="socio">Instancia de socio al que enviar el email</param>
        /// <param name="subject">Asunto del email</param>
        /// <param name="body">Cuerpo en html del email</param>
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

        /// <summary>
        /// Genera una contraseña aleatoria de la longitud que pasemos como parametro
        /// </summary>
        /// <param name="length">int que sera la longitud de la contraseña</param>
        /// <returns>Contraseña aleatoria de la longitud deseada</returns>
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

        /// <summary>
        /// Genera el xml con los datos para rellenar la factura
        /// </summary>
        /// <param name="factura">Instancia de facturas</param>
        /// <param name="reserva">Instancia de reservas</param>
        /// <returns>xml codificado en UTF8 para rellenar formulario en pdf</returns>
        public static string generarXmlFactura(facturas factura, reservas reserva)
        {
            // Transformamos los campos que pueden dar problemas de codificacion a utf8 para no tener problemas con el pdf
            byte[] bytes = Encoding.Default.GetBytes(reserva.socios.nombre);
            string nombre = Encoding.UTF8.GetString(bytes);

            bytes = Encoding.Default.GetBytes(reserva.socios.apellidos);
            string apellidos = Encoding.UTF8.GetString(bytes);

            bytes = Encoding.Default.GetBytes(reserva.socios.direccion1);
            string direccion1 = Encoding.UTF8.GetString(bytes);

            bytes = Encoding.Default.GetBytes(reserva.socios.direccion2);
            string direccion2 = Encoding.UTF8.GetString(bytes);

            bytes = Encoding.Default.GetBytes(reserva.recursos.nombre_rec);
            string nombre_rec = Encoding.UTF8.GetString(bytes);

            // Generamos el xml con formato UTF8
            string ret = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                        + "<xfdf xmlns=\"http://ns.adobe.com/xfdf/\" xml:space=\"preserve\">"
                            + "<f href=\"c:/Google Drive/PFC/factura.pdf\" />"
                            + "<fields>"
                                + "<field name = \"n_factura\">"
                                + "<value>" + factura.id.ToString() + "</value>"
                                + "</field>"
                                + "<field name = \"f_factura\">"
                                + "<value>" + DateTime.Today.ToString("dd/MM/yyyy") + "</value>"
                                + "</field>"
                                + "<field name = \"id_socio\">"
                                + "<value>" + reserva.socios.id + "</value>"
                                + "</field>"
                                + "<field name = \"nombre\">"
                                + "<value>" + nombre + apellidos + "</value>"
                                + "</field>"
                                + "<field name = \"nif\">"
                                + "<value>" + reserva.socios.nif + "</value>"
                                + "</field>"
                                + "<field name = \"direccion1\">"
                                + "<value>" + direccion1 + "</value>"
                                + "</field>"
                                + "<field name = \"direccion2\">"
                                + "<value>" + direccion2 + "</value>"
                                + "</field>"
                                + "<field name = \"nombre_recurso\">"
                                + "<value>" + nombre_rec + "</value>"
                                + "</field>"
                                + "<field name = \"fecha\">"
                                + "<value>" + reserva.fecha.ToString("dd/MM/yyyy") + "</value>"
                                + "</field>"
                                + "<field name = \"hora\">"
                                + "<value>" + reserva.hora.Hour + "</value>"
                                + "</field>"
                                + "<field name = \"precio\">"
                                + "<value>" + reserva.precio.ToString() + "</value>"
                                + "</field>"
                                + "<field name = \"precio_total\">"
                                + "<value>" + reserva.precio.ToString() + "</value>"
                                + "</field>"
                            + "</fields>"
                        + "</xfdf>";

            return ret;
        }


    }
}