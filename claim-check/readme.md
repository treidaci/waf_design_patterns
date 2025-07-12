# ClaimCheckDemo

This project demonstrates the **Claim-Check Pattern** using [NServiceBus](https://docs.particular.net/nservicebus/) in a .NET console application. The Claim-Check Pattern is useful for handling **large payloads** by offloading the data to external storage and passing only a reference (a "claim check") through the message pipeline.

---

## 📦 What It Does

- Simulates sending a message (`OrderSubmitted`) with a large payload.
- Stores the payload in the local file system (`claim-check-storage/`).
- Sends only a reference (claim check) to the message handler.
- The handler retrieves the payload using the reference and processes it.

---

## 🚀 How to Run

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- No external transport or broker needed — uses NServiceBus's `LearningTransport`.

### Steps

1. **Restore dependencies**
   ```bash
   dotnet restore
   ```

2. **Run the project**
   ```bash
   dotnet run
   ```

3. **Observe console output**

   You’ll see:
    - A message being sent (`OrderSubmitted`)
    - The payload written to disk
    - The handler reading and displaying the payload

---

## 📂 File Structure

- `Program.cs` – Sets up the host and configures NServiceBus.
- `OrderService.cs` – Creates and sends the message with a claim check.
- `OrderSubmitted.cs` – The message contract.
- `OrderSubmittedHandler.cs` – Retrieves and handles the message.
- `FileClaimCheckStore.cs` – Saves and retrieves payloads to/from disk.
- `claim-check-storage/` – Directory where payload files are stored.

---

## 🧪 What to Expect

Upon running:
- A simulated payload (binary blob) is stored to `claim-check-storage/{OrderId}.bin`
- A message is sent referencing the file path
- The handler retrieves the file, reads the content, and logs it

---

## 📚 Technologies Used

- .NET 8 Console App
- NServiceBus
- NServiceBus.Extensions.Hosting
- LearningTransport (no external broker required)

---

## ✅ Use Cases

- Offloading large binary files from messages
- Decoupling message transport from payload handling
- Simulating real-world scenarios like image uploads, documents, etc.

---

Feel free to modify the storage provider (e.g., switch to Azure Blob Storage) or extend with additional message types.
