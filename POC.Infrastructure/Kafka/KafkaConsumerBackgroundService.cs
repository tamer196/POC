using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using POC.Application.Common.Presence;
using POC.Application.Events.UserActivity;
using POC.Application.Notification.UserActivity;
using POC.Infrastructure.Presence;
using System.Text.Json;

namespace POC.Infrastructure.Kafka
{
    public class KafkaConsumerBackgroundService : BackgroundService
    {
        private readonly ILogger<KafkaConsumerBackgroundService> _logger;
        private readonly KafkaOptions _options;
        private readonly IOnlineUserStore _onlineUserStore;
        private readonly IUserActivityNotifier _notifier;

        public KafkaConsumerBackgroundService(
            ILogger<KafkaConsumerBackgroundService> logger,
            IOptions<KafkaOptions> options,
            IOnlineUserStore onlineUserStore,
            IUserActivityNotifier notifier)
        {
            _logger = logger;
            _options = options.Value;
            _onlineUserStore = onlineUserStore;
            _notifier = notifier;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(5000, stoppingToken); // ✅ allow app to start first

            var config = new ConsumerConfig
            {
                BootstrapServers = _options.BootstrapServers,
                GroupId = _options.ConsumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe(_options.Topic);

            _logger.LogInformation("Kafka consumer CONNECTED to {Server}", _options.BootstrapServers);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(stoppingToken);

                        if (result?.Message?.Value != null)
                        {
                            _logger.LogInformation("Raw message: {Value}", result.Message.Value);

                            var userEvent = JsonSerializer.Deserialize<UserActivityEvent>(result.Message.Value);

                            if (userEvent != null)
                            {
                                ProcessEventAsync(userEvent);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Kafka consume error");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Kafka consumer error");
            }
            finally
            {
                consumer.Close();
            }
        }

        private async Task ProcessEventAsync(UserActivityEvent userEvent)
        {
            try
            {
                if (userEvent.EventType == UserEventType.Login)
                {
                    var onlineUser = new OnlineUserDto
                    {
                        UserId = userEvent.UserId,
                        UserName = userEvent.UserName,
                        LastActivityUtc = userEvent.TimestampUtc
                    };

                    _onlineUserStore.AddOrUpdate(onlineUser);
                    _logger.LogWarning("User added to online store: {UserName}", userEvent.UserName);
                    await _notifier.UserLoggedIn(onlineUser);
                }
                else if (userEvent.EventType == UserEventType.Logout)
                {
                    _onlineUserStore.Remove(userEvent.UserId);
                    _logger.LogWarning("User removed from online store: {UserName}", userEvent.UserName);
                    await _notifier.UserLoggedOut(userEvent.UserId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ProcessEvent");
            }
        }
    }
}