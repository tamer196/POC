using Microsoft.Extensions.Configuration;
using POC.KafkaProducer;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var kafkaOptions = config.GetSection("Kafka").Get<KafkaOptions>()!;

var producer = new UserActivityProducer(kafkaOptions);
var simulator = new UserActivitySimulator();

Console.WriteLine("Producer started...");

while (true)
{
    try
    {
        var userEvent = simulator.Generate();

        await producer.PublishAsync(userEvent);

        Console.WriteLine(
            $"{userEvent.UserName} - {userEvent.EventType}");

        await Task.Delay(2000);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);

        continue;
    }
}