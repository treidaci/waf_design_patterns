using ChoreographyDemo.Events;

namespace ChoreographyDemo.Services;

public class PaymentService
{
    public PaymentService()
    {
        EventBus.Subscribe<OrderCreated>(HandleOrderCreated);
    }

    private void HandleOrderCreated(OrderCreated evt)
    {
        Console.WriteLine($"[PaymentService] Processing payment for Order {evt.OrderId}");
        EventBus.Publish(new PaymentProcessed(evt.OrderId));
    }
}