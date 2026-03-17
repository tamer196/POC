using Confluent.Kafka;
using POC.Application.Events.UserActivity;
using System.Text.Json;

namespace POC.KafkaProducer
{
    public class UserActivityProducer
    {
        private readonly IProducer<string, string> _producer;
        private readonly KafkaOptions _options;

        public UserActivityProducer(KafkaOptions options)
        {
            _options = options;

            var config = new ProducerConfig
            {
                BootstrapServers = options.BootstrapServers
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishAsync(UserActivityEvent userEvent)
        {
            var json = JsonSerializer.Serialize(userEvent);

            Console.WriteLine($"Sending to Kafka: {json}");

            var message = new Message<string, string>
            {
                Key = userEvent.UserId.ToString(),
                Value = json
            };

            await _producer.ProduceAsync(_options.Topic, message);

            _producer.Flush(TimeSpan.FromSeconds(5));   // ⭐ IMPORTANT

            Console.WriteLine("Message delivered.");
        }
    }
}
