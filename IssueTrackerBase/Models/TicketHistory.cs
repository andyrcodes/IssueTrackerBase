namespace IssueTrackerBase.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }

        public int TicketId { get; set; }

        public string Property { get; set; } = string.Empty;

        public string OldValue { get; set; } = string.Empty;

        public string NewValue { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public virtual Ticket? Ticket { get; set; }

        public virtual AppUser? User { get; set; }
    }
}
