using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ProdutorAPI
{
    public class ProdutorService
    {
        private readonly IConfiguration _configuration;
        private readonly ProducerConfig _producerConfig;
        private readonly ILogger<ProdutorService> _logger;

        public ProdutorService(IConfiguration configuration, ILogger<ProdutorService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var bootstrap = _configuration.GetSection("KafkaConfig").GetValue<string>("BootstrapServer");

            _producerConfig = new ProducerConfig()
            {
                BootstrapServers = bootstrap
            };
        }

        public async Task<string> SendMessage(string message)
        {
            var topic = _configuration.GetSection("KafkaConfig").GetValue<string>("OrdemTopic");

            try
            {
                // Envia a mensagem JSON para o Kafka
                using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
                var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
                _logger.LogInformation($"Mensagem enviada com sucesso: {result.Status}");

                // Consumir a resposta da Aplicação 2
                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = _producerConfig.BootstrapServers,
                    GroupId = "respostas-group",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
                consumer.Subscribe("respostas-topic");

                try
                {
                    var consumeResult = consumer.Consume(TimeSpan.FromSeconds(10));
                    if (consumeResult != null)
                    {
                        var responseMessage = consumeResult.Message.Value;

                        // Não tente desserializar o responseMessage como JSON
                        // Apenas retorne a string diretamente
                        return $"Ordem processada: {responseMessage}";
                    }
                    else
                    {
                        return "Sem resposta da aplicação 2";
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError($"Erro ao consumir mensagem: {ex.Message}");
                    return "Erro ao processar a ordem";
                }
            }
            catch (ProduceException<Null, string> ex)
            {
                _logger.LogError($"Erro ao enviar mensagem: {ex.Message}");
                return "Erro ao enviar a ordem";
            }
        }

    }
}