using ChoreographyDemo.Services;

namespace ChoreographyDemo;

class Program
{
    static void Main()
    {
        var paymentService = new PaymentService();
        var shippingService = new ShippingService();
        var orderService = new OrderService();

        orderService.CreateOrder();

        Console.ReadLine();
    }
}