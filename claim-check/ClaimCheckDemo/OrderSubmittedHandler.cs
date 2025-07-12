using System;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace ClaimCheckDemo;

public class OrderSubmittedHandler(IClaimCheckStore claimCheckStore) : IHandleMessages<OrderSubmitted>
{
    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        var data = await claimCheckStore.RetrieveAsync(message.PayloadReference);
        var content = Encoding.UTF8.GetString(data);

        Console.WriteLine($"[Handler] Order ID: {message.OrderId}");
        Console.WriteLine($"[Handler] Retrieved Payload: {content}");
    }
}