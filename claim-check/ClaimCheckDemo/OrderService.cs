using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace ClaimCheckDemo;

public class OrderService(IMessageSession messageSession, IClaimCheckStore claimCheckStore)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var orderId = Guid.NewGuid().ToString();
        var payloadData = "This is a large simulated payload..."u8.ToArray();

        var reference = await claimCheckStore.StoreAsync(orderId, payloadData);

        var message = new OrderSubmitted
        {
            OrderId = orderId,
            PayloadReference = reference
        };

        await messageSession.SendLocal(message, cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}