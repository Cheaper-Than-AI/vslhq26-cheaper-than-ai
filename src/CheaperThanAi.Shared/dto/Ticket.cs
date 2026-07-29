namespace CheaperThanAi.Shared.dto
{
    public class Ticket
    {
        public string Id { get; set; } = default!;

        public DateTime DateTime { get; set; } = default!;

        public string UserName { get; set; } = default!;

        public string IssueDescription { get; set; } = default!;

        public PriorityLevel PriorityLevel { get; set; } = default!;

        public string Category { get; set; } = default!;
    }

    public enum PriorityLevel
    {
        Low,
        Medium,
        High,
    }
}
