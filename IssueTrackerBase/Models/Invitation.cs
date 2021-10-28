namespace IssueTrackerBase.Models
{
    public class Invitation
    {
        public int Id { get; set; }

        public DateTime InviteDate { get; set; }

        public DateTime JoinDate { get; set; }

        public Guid CompanyToken { get; set; }

        public int CompanyId { get; set; }

        public int ProjectId { get; set; }

        public string InviteeId { get; set; } = string.Empty;

        public string InvitorId { get; set; } = string.Empty ;

        public string InviteeEmail { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public bool IsValid { get; set; }

        public virtual Company? Company { get; set; }

        public virtual Project? Project { get; set; }

        public virtual AppUser? Invitee { get; set; }

        public virtual AppUser? Invitor { get; set; }

    }
}
