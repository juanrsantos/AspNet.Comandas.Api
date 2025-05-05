
namespace Comandas.Api
{
    public class PulsarConsumer : BackgroundService
    {
        private readonly ILogger<PulsarConsumer> _logger;

        public PulsarConsumer(ILogger<PulsarConsumer> logger)
        {
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SERVIÇO EM SEGUNDO PLANO INICIADO...");
            while (true) 
            {
                _logger.LogInformation("EXECUTANDO TAREFA EM BACKGROUND {0}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
            _logger.LogInformation("SERVIÇO EM SEGUNDO PLANO FINALIZADO !");
        }
    }
}
