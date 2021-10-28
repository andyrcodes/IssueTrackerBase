namespace IssueTrackerBase.Models
{
    public class TicketComment
    {
        public int Id { get; set; }

        public string Comment { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public int TicketId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public virtual Ticket? Ticket { get; set; }

        public virtual AppUser? User { get; set; }

    }
}
