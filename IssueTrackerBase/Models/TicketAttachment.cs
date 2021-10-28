using System.ComponentModel.DataAnnotations.Schema;

namespace IssueTrackerBase.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public int TicketId { get; set; }

        public string UserId { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile? FormFile { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string FileContentType { get; set; } = string.Empty;

        public byte[]? FileData { get; set; }

        public virtual Ticket? Ticket { get; set; }

        public virtual AppUser? User { get; set; }
    }
}
