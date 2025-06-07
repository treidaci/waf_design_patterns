To implement the Bulkhead Pattern in C#, you can use resource isolation strategies—typically through thread pools, semaphore limits, or dedicated service clients per workload. 
The goal is to prevent the failure or saturation of one component from cascading across the entire system.

The solution includes a .NET background worker service with three implementations of the Bulkhead pattern:
- SemaphoreSlim-based bulkhead isolation for concurrent task limits.
- HttpClient-based isolation via named clients and max connection limits.
- Polly-based BulkheadPolicy for service call resilience.

The worker service calls the Simulated API.

To run the project:
```bash
docker compose up --build
```
Then analyse the logs from the BulkheadWorker container.