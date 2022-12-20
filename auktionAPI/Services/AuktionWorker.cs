using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace auktionAPI.Services;
public class AuktionWorker : BackgroundService
{
    private readonly ILogger<AuktionWorker> _logger;
    private readonly IConnection _connection;
    private readonly IAuktionService _auktionService;
    private readonly IBudHandler _budHandler;

    public AuktionWorker(ILogger<AuktionWorker> logger, IConfiguration configuration, 
        IAuktionService auktionService, IBudHandler budHandler)
    {
        _logger = logger;
        _auktionService = auktionService;
        _budHandler = budHandler;
        
        var mqhostname = configuration["BudBrokerHost"];
        _logger.LogInformation($"AuktionWorker listening on host at {mqhostname}");
        
        var factory = new ConnectionFactory() { HostName = mqhostname };
        _connection = factory.CreateConnection();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = _connection.CreateModel();
        channel.QueueDeclare(queue: "bud",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
        
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            
            Bud bud = JsonSerializer.Deserialize<Bud>(message)!;

            if (bud is not null)
            {
                UpdateAuction(bud);

                _logger.LogInformation(" [x] Received {0}", message);
            }
        };

        channel.BasicConsume(queue: "bud",
                                autoAck: true,
                                consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async void UpdateAuction(Bud bud)
    {
        _budHandler.AuktionList = await _auktionService.GetAsync();

        Auktion? auktion = _budHandler.UpdateAuctionIfBidIsValid(bud);

        if (auktion is not null)
        {
            await _auktionService.UpdateAsync(auktion.Id!, auktion);

            _logger.LogInformation("Bid inserted in auction");
        }    
    }
}
