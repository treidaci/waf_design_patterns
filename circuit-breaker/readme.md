# Circuit Breaker Demo with Polly in ASP.NET Core

This project demonstrates how to use the [Polly](https://github.com/App-vNext/Polly) library's Circuit Breaker pattern in an ASP.NET Core application. It uses a custom HTTP client pipeline to handle transient failures when calling an external API.

## Features

- **Polly Circuit Breaker**: Automatically opens the circuit after a configurable failure ratio, preventing further requests for a set duration.
- **Dynamic Endpoint Switching**: The app alternates between simulating failed (`/503`) and successful (`/200`) external responses every 2 minutes.
- **Resilience Pipeline**: Uses Polly's new `AddResilienceHandler` API for configuring the circuit breaker.

## How It Works

- The api exposes a single endpoint: `/external-data`.
- When called, it uses an `HttpClient` to request either `/503` (simulate failure) or `/200` (simulate success) from [https://mock.httpstatus.io](https://mock.httpstatus.io), switching every 2 minutes.
- The circuit breaker is configured to:
  - Open if 50% or more of the last 10 requests in a 30-second window fail.
  - Stay open for 15 seconds before transitioning to half-open.
  - Log state transitions to the console.
- The console app is a simple client that calls the api and prints the result to the console.

## Running the Project

1. **Clone the repository** and navigate to the project folder.

2. **Run the application**:

- Run both applications in parallel
- See the output in the console of both applications