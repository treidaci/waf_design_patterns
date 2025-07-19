using CompensationTransactionDemo;

var workflow = new FinancialTransactionWorkflow();
var success = await workflow.ExecuteAsync("AccountA", "AccountB", 100m);

Console.WriteLine(success ? "✅ Async workflow with retry completed" : "💥 Async workflow failed and compensated");