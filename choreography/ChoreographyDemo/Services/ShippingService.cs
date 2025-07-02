using ChoreographyDemo.Events;

namespace ChoreographyDemo.Services;

public class ShippingService
{
    public ShippingService()
    {
        EventBus.Subscribe<PaymentProcessed>(HandlePaymentProcessed);
    }

    private void HandlePaymentProcessed(PaymentProcessed evt)
    {
        Console.WriteLine($"[ShippingService] Shipping order {evt.OrderId}");
    }
}