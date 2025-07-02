using ChoreographyDemo.Events;

namespace ChoreographyDemo.Services;

public class OrderService
{
    public void CreateOrder()
    {
        var orderId = Guid.NewGuid();
        Console.WriteLine($"[OrderService] Order {orderId} created.");
        EventBus.Publish(new OrderCreated(orderId));
    }
}