using Async_Request_Reply.Services;
using Microsoft.AspNetCore.Mvc;

namespace Async_Request_Reply.Controllers;

[ApiController]
[Route("api/people")]
public class AsyncPersonController(IBackgroundTaskQueue taskQueue) : ControllerBase
{
    private static readonly Dictionary<Guid, Person> PendingRequests = new();

    [HttpPost]
    public IActionResult CreatePerson([FromBody] string name)
    {
        var id = Guid.NewGuid();
        var person = new Person(id, name, "Pending");
        
        PendingRequests[id] = person;
        
        taskQueue.QueueBackgroundWorkItem(async token =>
        {
            await Task.Delay(5000, token); // Simulate long-running process
            PendingRequests[id] = person with { Status = "Completed" };
        });

        var statusUrl = Url.ActionLink("GetStatus", values: new { id })!;
        var links = new List<Link>
        {
            new(statusUrl, "self"),
            new($"/api/people/{id}", "person")
        };

        return Accepted(statusUrl, new StatusResponse(
            "Processing",
            DateTime.UtcNow.AddSeconds(5),
            links));
    }

    [HttpGet("status/{id}", Name = "GetStatus")]
    public IActionResult GetStatus(Guid id)
    {
        if (!PendingRequests.TryGetValue(id, out var person))
            return NotFound();

        var statusUrl = Url.ActionLink("GetStatus", values: new { id })!;
        var links = new List<Link>
        {
            new(statusUrl, "self")
        };

        if (person.Status == "Completed")
        {
            links.Add(new($"/api/people/{id}", "person"));
            return Ok(new StatusResponse(
                person.Status,
                DateTime.UtcNow,
                links));
        }

        links.Add(new(statusUrl, "poll"));
        return Accepted(new StatusResponse(
            person.Status,
            DateTime.UtcNow.AddSeconds(1),
            links));
    }

    [HttpGet("{id}")]
    public IActionResult GetPerson(Guid id)
    {
        return PendingRequests.TryGetValue(id, out var person) && person.Status == "Completed"
            ? Ok(person)
            : NotFound();
    }
}