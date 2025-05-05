using Comandas.Services.Interfaces;
using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Comandas.Services
{
    public class PulsarProduceService : IAsyncDisposable, IPulsarProduceService
    {
        private readonly IPulsarClient pulsarClient;
        private readonly IProducer<string> producer;
        private readonly ILogger<PulsarProduceService> logger;
        public PulsarProduceService(ILogger<PulsarProduceService> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            var pulsarUrl = "pulsar://pulsar:6650";
            var topico = "Email";
            var urlPulsar = new Uri(pulsarUrl);

            pulsarClient = PulsarClient.Builder().ServiceUrl(urlPulsar).Build();
            producer = pulsarClient.NewProducer(Schema.String).Topic(topico).Create();
        }
        public async Task EnviarMensagemAsync(EventoUsuario eventoUsuario)
        {
            try
            {
                var mensagem = JsonConvert.SerializeObject(eventoUsuario);
                await producer.Send(mensagem);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Erro");
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (producer != null)
            {
                await producer.DisposeAsync();  
            }
            if(pulsarClient != null)
            {
                await pulsarClient.DisposeAsync();
            }
        }
    }
}
