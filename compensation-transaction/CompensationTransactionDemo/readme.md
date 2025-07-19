# CompensatingTransactionDemo

This project demonstrates the **Compensating Transaction Pattern** using **asynchronous workflows**, **retry logic**, and **logging** in a .NET 9 console application. It simulates a distributed financial operation with reversible steps and robust error handling.

---

## 📦 What It Does

- Simulates a multi-step financial transfer:
  - Debits an account
  - Credits another account
  - Logs the transaction
- Applies **retries** with backoff for each step
- Defines and executes **compensation steps** if any part fails
- Uses async/await and structured logs to track workflow execution

---

## 🚀 How to Run

### Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/download)

### Steps

```bash
dotnet restore
dotnet run
```

You will see output in the terminal showing:
- Attempted actions with retries
- Any failures and compensation steps
- Final status of the workflow

---

## 🔁 Retry Logic

Each step is retried up to a configurable number of attempts (default: 2–3), with a delay between retries. If retries are exhausted, the workflow triggers compensation logic.

---

## ⚖️ Compensation Strategy

For each successful step, a corresponding undo (compensation) step is registered in a stack. If a later step fails, compensation steps are invoked in reverse order to undo prior effects.

---

## 🧱 Project Structure

| File                                 | Purpose                                         |
|--------------------------------------|-------------------------------------------------|
| `Program.cs`                         | Entry point for the app                         |
| `FinancialTransactionWorkflow.cs`    | Defines the async workflow with compensation    |
| `CompensatingTransactionDemo.csproj` | Project config for .NET 9              |

---

## 📚 Concepts Demonstrated

- Compensating Transaction Pattern
- Async workflows with failure recovery
- Retry mechanism per operation
- Transaction simulation without external dependencies

---

## 🧪 Simulating Failure

To test compensation, you can introduce `throw new Exception("fail")` in any method like `CreditAsync` or `LogAsync`.

---

## ✅ Use Cases

- Distributed systems without atomic transactions
- Financial systems, booking/reservation platforms
- Event-driven architectures with eventual consistency

---

Feel free to extend this with:
- `ILogger` integration
- Polly retry policies
- External storage or message brokers

