namespace Async_Request_Reply;

public record StatusResponse(
    string Status,
    DateTime EstimatedCompletion,
    List<Link> Links);

// Link.cs
public record Link(string Href, string Rel);