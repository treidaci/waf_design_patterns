namespace CompensationTransactionDemo;

public class FinancialTransactionWorkflow
{
    private readonly Stack<Func<Task>> _compensationSteps = new();

    public async Task<bool> ExecuteAsync(string fromAccount, string toAccount, decimal amount)
    {
        try
        {
            await RunStepAsync(
                action: () => RetryAsync(() => DebitAsync(fromAccount, amount), 3),
                compensation: () => RetryAsync(() => RefundAsync(fromAccount, amount), 3)
            );

            await RunStepAsync(
                action: () => RetryAsync(() => CreditAsync(toAccount, amount), 3),
                compensation: () => RetryAsync(() => DebitAsync(toAccount, amount), 3)
            );

            await RunStepAsync(
                action: () => RetryAsync(() => LogAsync(fromAccount, toAccount, amount), 2)
            );

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Workflow failed: {ex.Message}");
            await CompensateAsync();
            return false;
        }
    }

    private async Task RunStepAsync(Func<Task> action, Func<Task>? compensation = null)
    {
        await action();
        if (compensation != null)
        {
            _compensationSteps.Push(compensation);
        }
    }

    private async Task RetryAsync(Func<Task> action, int maxAttempts)
    {
        int attempt = 0;
        while (true)
        {
            try
            {
                attempt++;
                Console.WriteLine($"🔁 Attempt {attempt} of {maxAttempts}");
                await action();
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Retry {attempt} failed: {ex.Message}");
                if (attempt >= maxAttempts)
                    throw;
                await Task.Delay(500);
            }
        }
    }

    private Task DebitAsync(string accountId, decimal amount)
    {
        Console.WriteLine($"💳 Debiting {amount} from {accountId}");
        return Task.Delay(400);
    }

    private Task CreditAsync(string accountId, decimal amount)
    {
        Console.WriteLine($"💰 Crediting {amount} to {accountId}");
        // throw new Exception($"❌  Crediting {amount} to {accountId}"); // un-comment to see compensation
        return Task.Delay(400);
    }

    private Task RefundAsync(string accountId, decimal amount)
    {
        Console.WriteLine($"↩️ Refunding {amount} to {accountId}");
        return Task.Delay(400);
    }

    private Task LogAsync(string from, string to, decimal amount)
    {
        Console.WriteLine($"📝 Logging transfer of {amount} from {from} to {to}");
        return Task.Delay(300);
    }

    private async Task CompensateAsync()
    {
        Console.WriteLine("🔁 Starting compensation...");
        while (_compensationSteps.Count > 0)
        {
            var step = _compensationSteps.Pop();
            try
            {
                await step();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Compensation step failed: {ex.Message}");
            }
        }
    }
}