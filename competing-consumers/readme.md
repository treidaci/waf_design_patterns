# CompetingConsumersAdvanced

This project demonstrates the **Competing Consumers Pattern** using a local in-memory queue with advanced features like **backpressure**, **retry logic**, and **poison message detection**. It's implemented in C# using the .NET 9 framework and runs entirely as a local console application.

---

## 📦 What It Does

- Spawns a random number of **producer services** that generate messages.
- Messages are pushed into a **bounded in-memory channel** (queue) which simulates **backpressure**.
- Spawns a random number of **consumer services** that pull messages and process them.
- Simulates **message processing failures** and retries.
- Detects and logs **poison messages** (i.e., messages that exceed the retry limit).

---

## 🚀 How to Run

### Requirements
- [.NET 9 SDK](https://dotnet.microsoft.com/download)

### Steps

```bash
dotnet restore
dotnet run
```

You'll see console output indicating:
- Producers generating and sending messages.
- Consumers processing those messages.
- Retry attempts on failure.
- Poison message detection after exceeding max retry count.

---

## 🔁 Features

### ✅ Bounded Queue with Backpressure
Uses `Channel<T>` with bounded capacity to throttle message flow and prevent producer overload.

```csharp
Channel.CreateBounded<string>(new BoundedChannelOptions(50)
{
    FullMode = BoundedChannelFullMode.Wait
});
```

### 🔁 Retry Logic
Each message is retried up to **3 times** on failure before being considered poison.

### 💥 Poison Message Detection
After 3 failed processing attempts, a message is flagged as "poison" and logged but not retried further.

### 🧪 Failure Simulation
Random failures simulate transient errors (e.g., network issues or bad data).

---

## 📂 Project Structure

| File                         | Purpose                                                       |
|------------------------------|---------------------------------------------------------------|
| `Program.cs`                 | Sets up DI and the main host                                  |
| `SimulationService.cs`       | Orchestrates producers and consumers, retry & poison logic    |
| `CompetingConsumersAdvanced.csproj` | .NET project file                                      |

---

## 📚 Concepts Demonstrated

- Competing Consumers Pattern
- Backpressure with Bounded Channel
- Retry + poison message handling
- Randomized parallel workloads with async execution

---

## 🧩 Related Patterns

- Queue-Based Load Leveling
- Circuit Breaker (to enhance poison logic)
- Bulkhead Pattern (limit consumer concurrency)

---

## ✅ Use Cases

- Distributed worker queues
- Task dispatch systems
- Message-processing services with fault isolation

---

Feel free to extend it by:
- Logging to file or dashboard
- Using real messaging infrastructure (e.g., RabbitMQ, Azure Service Bus)
- Adding metrics or observability with Prometheus or OpenTelemetry
