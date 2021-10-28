namespace IssueTrackerBase.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public bool Archived { get; set; }

        public int ProjectId { get; set; }

        public int TicketPriorityId { get; set; }

        public int TicketStatusId { get; set; }

        public int TicketTypeId { get; set; }

        public string OwnerUserId { get; set; } = string.Empty ;

        public string DeveloperUserId { get; set; } = string.Empty;

        public virtual Project? Project { get; set; }

        public virtual TicketPriority? TicketPriority { get; set; }

        public virtual TicketStatus? TicketStatus { get; set; }

        public virtual TicketType? TicketType { get; set; }

        public virtual AppUser? OwnerUser { get; set; }

        public virtual AppUser? DeveloperUser { get; set; }

        public virtual ICollection<TicketAttachment> Attachments { get; set; } = new HashSet<TicketAttachment>();

        public virtual ICollection<TicketComment> Comments { get; set; } = new HashSet<TicketComment>();

        public virtual ICollection<TicketHistory> Histories { get; set; } = new HashSet<TicketHistory>();

        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();

    }
}
