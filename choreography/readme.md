## In this pattern:

1. Each component listens for and reacts to domain events.
2. There is no central controller dictating the order of execution.
3. Coordination is done implicitly via events.

## How It Works
1. OrderService triggers a domain event (OrderCreated).
2. PaymentService reacts to it via subscription and emits a new event (PaymentProcessed).
3. ShippingService listens for PaymentProcessed and reacts accordingly.
4. Each service is independent and only responds to events it cares about.
   
## To extend this demo:

1. Add error handling and retry logic.
2. Add logging/tracing to visualize the choreography.
3. Move from in-process to message brokers like Azure Service Bus, RabbitMQ, etc. for real-world usage.