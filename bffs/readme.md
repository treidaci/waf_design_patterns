A basic solution for a Backends for Frontends (BFF) architecture using a ClientOnboarding domain model. It includes:

- A shared ClientOnboarding model.
- A Web BFF that filters for verified clients.
- A Mobile BFF that returns all clients.
- A mock Onboarding service that returns test client data.
- Common Program.cs setup for each API.

The APIs are minimal APIs that mimic the structure below in terms of urls. Uisng this structure for clarity:

```markdown
BffClientOnboarding.sln
|
├── Shared/
│   ├── Shared.csproj
│   └── Models/
│       └── ClientOnboarding.cs
|
├── WebBffApi/
│   ├── WebBffApi.csproj
│   ├── Controllers/
│   │   └── ClientController.cs
│   └── Program.cs
|
├── MobileBffApi/
│   ├── MobileBffApi.csproj
│   ├── Controllers/
│   │   └── ClientController.cs
│   └── Program.cs
|
├── OnboardingService/
│   ├── OnboardingService.csproj
│   ├── Controllers/
│   │   └── ClientsController.cs
│   └── Program.cs
```
Run the project through docker compose:
```
docker compose build
docker compose up
```

APIs accessible at
```
http://localhost:5001/swagger
http://localhost:5002/swagger
http://localhost:5005/swagger
```

If you want to run the apis outside docker, replace `"http://onboarding-service"` with `http://localhost:5005/`
