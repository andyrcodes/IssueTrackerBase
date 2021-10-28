namespace IssueTrackerBase.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public int TicketId { get; set; }

        public string RecipientId { get; set; } = string.Empty ;

        public string SenderId { get; set; } = string.Empty;

        public bool Viewed { get; set; }

        public virtual Ticket? Ticket { get; set; }

        public virtual AppUser? Recipient { get; set; }

        public virtual AppUser? Sender { get; set; }

    }
}
