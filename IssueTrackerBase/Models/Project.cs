using System.ComponentModel.DataAnnotations.Schema;

namespace IssueTrackerBase.Models
{
    public class Project
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int ProjectPriorityId { get; set; }

        [NotMapped]
        public IFormFile? FormFile { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string FileContentType { get; set; } = string.Empty;

        public byte[]? FileData { get; set; }

        public bool Archived { get; set; }

        public virtual Company? Company { get; set; }

        public virtual ProjectPriority? ProjectPriority { get; set; }

        public virtual ICollection<AppUser> Members { get; set; } = new HashSet<AppUser>();

        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();

    }
}
