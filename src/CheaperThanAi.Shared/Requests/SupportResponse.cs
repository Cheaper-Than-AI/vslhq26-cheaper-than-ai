namespace CheaperThanAi.Shared.Requests;

public sealed class SupportResponse
{
    public string Message { get; set; } = string.Empty;

    // Optional server-generated easter-egg message for fun
    public string? EasterEgg { get; set; }
}
