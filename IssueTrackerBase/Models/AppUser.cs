using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssueTrackerBase.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName { get { return $"{FirstName} {LastName}"; }}

        [NotMapped]
        [Display(Name = "Select Image")]
        [DataType(DataType.Upload)]
        public IFormFile? AvatarFormFile { get; set; }

        public string AvatarFileName { get; set; } = string.Empty;

        public byte[]? AvatarFileData { get; set; }

        public int CompanyId { get; set; }

        public virtual Company? Company { get; set; }

        public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();

    }
}
