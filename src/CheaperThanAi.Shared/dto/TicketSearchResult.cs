namespace CheaperThanAi.Shared.dto
{
    public class TicketSearchResult
    {
        public string Id { get; set; } = default!;

        public DateTime DateTime { get; set; } = default!;

        public string UserName { get; set; } = default!;

        public string Category { get; set; } = default!;

        public PriorityLevel PriorityLevel { get; set; } = default!;

        public string IssueDescription { get; set; } = default!;

        public double Score { get; set; }
    }
}