using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsumidorConsole
{
    public class ConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ParametersModel _parameters;
        private readonly ILogger<ConsumerService> _logger;

        public ConsumerService(ILogger<ConsumerService> logger)
        {
            _logger = logger;
            _parameters = new ParametersModel(); 

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _parameters.BootstrapServer,
                GroupId = _parameters.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Aguardando mensagens");
            _consumer.Subscribe(_parameters.TopicName);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(stoppingToken);
                    if (result != null)
                    {
                        var mensagem = result.Message.Value;
                        _logger.LogInformation($"Mensagem recebida: {mensagem}");

                        // Processamento da mensagem e envio de resposta
                        var responseMessage = $"Ordem {mensagem} processada com sucesso.";
                        _logger.LogInformation(responseMessage);

                        // Enviar a resposta para o tópico de respostas
                        var producerConfig = new ProducerConfig { BootstrapServers = _parameters.BootstrapServer };
                        using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
                        await producer.ProduceAsync("respostas-topic", new Message<Null, string> { Value = responseMessage });
                        _logger.LogInformation($"Resposta enviada: {responseMessage}");
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError($"Erro ao consumir mensagem: {ex.Message}");
                }

            }
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            _consumer.Close();
            _logger.LogInformation("Conexão fechada");
            return Task.CompletedTask;
        }
    }
}
