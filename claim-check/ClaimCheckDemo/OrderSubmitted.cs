using NServiceBus;

namespace ClaimCheckDemo;

public class OrderSubmitted : IMessage
{
    public string OrderId { get; set; }
    public string PayloadReference { get; set; }
}