namespace POC.Infrastructure.Kafka
{
    public sealed class KafkaOptions
    {
        public const string SectionName = "Kafka";

        public string BootstrapServers { get; init; } = string.Empty;

        public string Topic { get; init; } = string.Empty;

        public string ConsumerGroupId { get; init; } = string.Empty;
    }
}
