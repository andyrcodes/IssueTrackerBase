using IssueTrackerBase.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IssueTrackerBase.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; } = default!;
        public DbSet<Project> Projects { get; set; } = default!;
        public DbSet<Company> Companies { get; set; } = default!;
        public DbSet<Invitation> Invitations { get; set; } = default!;
        public DbSet<Notification> Notifications { get; set; } = default!;
        public DbSet<ProjectPriority> ProjectPriorities { get; set; } = default!;
        public DbSet<TicketAttachment> Attachments { get; set; } = default!;
        public DbSet<TicketComment> Comments { get; set; } = default!;
        public DbSet<TicketHistory> Histories { get; set; } = default!;
        public DbSet<TicketPriority> TicketPriorities { get; set; } = default!;
        public DbSet<TicketStatus> TicketStatuses { get; set; } = default!;
        public DbSet<TicketType> TicketTypes { get; set; } = default!;

    }
}