
using Comandas.Services.Interfaces;
using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;
using Newtonsoft.Json;
using System.Text;

namespace Comandas.Api
{
    public class PulsarConsumer : BackgroundService
    {
        private readonly ILogger<PulsarConsumer> _logger;
        private readonly IPulsarClient _pulsarClient;
        private readonly IConsumer<string> _consumer;
        private readonly IServiceProvider _serviceProvider;

        public PulsarConsumer(ILogger<PulsarConsumer> logger, IPulsarClient pulsarClient, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _pulsarClient = pulsarClient;
            // Criando um consumer do Pulsar.
            _consumer = _pulsarClient.NewConsumer(Schema.String)
                           .Topic("Email")
                           .SubscriptionName("Enviar_email")
                           .SubscriptionType(SubscriptionType.Shared)
                           .InitialPosition(SubscriptionInitialPosition.Earliest)
                           .Create();
            _serviceProvider = serviceProvider;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SERVIÇO EM SEGUNDO PLANO INICIADO...");
            while (true) 
            {
                _logger.LogInformation("EXECUTANDO TAREFA EM BACKGROUND {0}", DateTimeOffset.Now);
                var mensagem = await _consumer.Receive(stoppingToken);
                var json = Encoding.UTF8.GetString(mensagem.Data);
                var mensagemconvertida = JsonConvert.DeserializeObject<EventoUsuario>(json);

                var escopo = _serviceProvider.CreateScope();
                var emailServices = escopo.ServiceProvider.GetRequiredService<IEmailService>();
                emailServices.EnviarEmail(mensagemconvertida.Email, mensagemconvertida.Assunto, "Logado com sucesso");
                _logger.LogInformation("MENSAGEM EXIBIDA PROCESSANDO {0}", json);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
            _logger.LogInformation("SERVIÇO EM SEGUNDO PLANO FINALIZADO !");
        }
    }
}
