using System.ComponentModel.DataAnnotations.Schema;

namespace IssueTrackerBase.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty ;

        [NotMapped]
        public IFormFile? FormFile { get; set; }

        public string FileName { get; set; } = string.Empty;

        public byte[]? FileData { get; set; }

        public string FileContentType { get; set; } = string.Empty;

        public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();

        public virtual ICollection<AppUser> Members { get; set; } = new HashSet<AppUser>();

        public virtual ICollection<Invitation> Invitations { get; set; } = new HashSet<Invitation>();

    }
}
