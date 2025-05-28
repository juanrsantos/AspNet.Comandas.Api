using Comandas.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Comandas.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration iConfiguration)
        {
            _configuration = iConfiguration;
        }

        public bool EnviarEmail(string endereco, string assunto, string mensagem)
        {
            var emailOrigem = _configuration.GetSection("ConfigEmail:EmailOrigem").Value;
            var host = _configuration.GetSection("ConfigEmail:Host").Value;
            var senha = _configuration.GetSection("ConfigEmail:Senha").Value;
            var porta = _configuration.GetSection("ConfigEmail:Porta").Value;

            try
            {
                using (var mensagemEmail = new MailMessage())
                {
                    mensagemEmail.From = new MailAddress(emailOrigem);
                    mensagemEmail.To.Add(new MailAddress(emailOrigem));
                    mensagemEmail.Body = mensagem;
                    mensagemEmail.Subject = assunto;
                    mensagemEmail.BodyEncoding = Encoding.UTF8;
                    mensagemEmail.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");
                    mensagemEmail.Priority = MailPriority.Normal;

                    using (var clientSmtp = new SmtpClient())
                    {
                        clientSmtp.Host = host;
                        clientSmtp.Port = int.Parse(porta);
                        clientSmtp.EnableSsl = true;
                        clientSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        clientSmtp.Credentials = new NetworkCredential(emailOrigem, senha);
                        clientSmtp.UseDefaultCredentials = false;
                        clientSmtp.Send(mensagemEmail);
                        return true;
                    }
                }

            }
            catch(SmtpFailedRecipientException smtp)
            {
                return false;
            }
            catch(SmtpException smtp)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
