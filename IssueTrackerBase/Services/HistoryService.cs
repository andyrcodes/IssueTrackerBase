using IssueTrackerBase.Data;
using IssueTrackerBase.Models;
using IssueTrackerBase.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IssueTrackerBase.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly ApplicationDbContext _context;

        public HistoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddHistoryAsync(Ticket oldTicket, Ticket newTicket, string userId)
        {
            try
            {
                if (oldTicket is null && newTicket is not null)
                {
                    TicketHistory history = new()
                    {
                        TicketId = newTicket.Id,
                        Property = "",
                        OldValue = "",
                        NewValue = "",
                        Created = DateTime.Now,
                        UserId = userId,
                        Description = "Ticket Created"
                    };

                    await _context.AddAsync(history);
                }
                else
                {
                    if (oldTicket.Title != newTicket.Title)
                    {
                        TicketHistory history = new()
                        {
                            TicketId = newTicket.Id,
                            Property = "Title",
                            OldValue = oldTicket.Title,
                            NewValue = newTicket.Title,
                            Created = DateTime.Now,
                            UserId = userId,
                            Description = "Title Updated"
                        };

                        await _context.AddAsync(history);

                    }
                    if (oldTicket.Description != newTicket.Description)
                    {
                        TicketHistory history = new()
                        {
                            TicketId = newTicket.Id,
                            Property = "Description",
                            OldValue = oldTicket.Description,
                            NewValue = newTicket.Description,
                            Created = DateTime.Now,
                            UserId = userId,
                            Description = "Description Updated"
                        };

                        await _context.AddAsync(history);
                    }
                    if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
                    {
                        TicketHistory history = new()
                        {
                            TicketId = newTicket.Id,
                            Property = "Ticket Priority",
                            OldValue = oldTicket.TicketPriority.Name,
                            NewValue = newTicket.TicketPriority.Name,
                            Created = DateTime.Now,
                            UserId = userId,
                            Description = $"Priority Updated: {newTicket.TicketPriority.Name}"
                        };

                        await _context.AddAsync(history);
                    }
                    if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
                    {
                        TicketHistory history = new()
                        {
                            TicketId = newTicket.Id,
                            Property = "Ticket Status",
                            OldValue = oldTicket.TicketStatus.Name,
                            NewValue = newTicket.TicketStatus.Name,
                            Created = DateTime.Now,
                            UserId = userId,
                            Description = $"Status Updated: {newTicket.TicketStatus.Name}"
                        };

                        await _context.AddAsync(history);
                    }
                    if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
                    {
                        TicketHistory history = new()
                        {
                            TicketId = newTicket.Id,
                            Property = "Ticket Type",
                            OldValue = oldTicket.TicketType.Name,
                            NewValue = newTicket.TicketType.Name,
                            Created = DateTime.Now,
                            UserId = userId,
                            Description = $"Type Updated: {newTicket.TicketType.Name}"
                        };

                        await _context.AddAsync(history);
                    }
                    if (oldTicket.DeveloperUserId != newTicket.DeveloperUserId)
                    {
                        TicketHistory history = new()
                        {
                            TicketId = newTicket.Id,
                            Property = "Developer",
                            OldValue = oldTicket.DeveloperUser?.FullName ?? "Not Assigned",
                            NewValue = newTicket.DeveloperUser.FullName,
                            Created = DateTime.Now,
                            UserId = userId,
                            Description = $"Developer Assigned: {newTicket.DeveloperUser.FullName}"
                        };

                        await _context.AddAsync(history);
                    }
                    if (oldTicket.Comments.Count != newTicket.Comments.Count)
                    {
                        TicketHistory history = new()
                        {
                            TicketId = newTicket.Id,
                            Property = "Comments",
                            OldValue = "",
                            NewValue = "",
                            Created = DateTime.Now,
                            UserId = userId,
                            Description = "Comment Added"
                        };

                        await _context.AddAsync(history);
                    }
                    if (oldTicket.Attachments.Count != newTicket.Attachments.Count)
                    {
                        TicketHistory history = new()
                        {
                            TicketId = newTicket.Id,
                            Property = "Attachments",
                            OldValue = "",
                            NewValue = "",
                            Created = DateTime.Now,
                            UserId = userId,
                            Description = "Attachment Added"
                        };

                        await _context.AddAsync(history);
                    }
                }
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TicketHistory>> GetCompanyTicketHistoriesAsync(int companyId)
        {
            try
            {
                List<Project> projects = (await _context.Companies.Include(c => c.Projects)
                                                                    .ThenInclude(p => p.Tickets)
                                                                        .ThenInclude(t => t.Histories)
                                                                            .ThenInclude(h => h.User)
                                                                    .FirstOrDefaultAsync(c => c.Id == companyId)).Projects.ToList();

                List<Ticket> tickets = projects.SelectMany(p => p.Tickets).ToList();

                List<TicketHistory> histories = tickets.SelectMany(t => t.Histories).ToList();

                return histories;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TicketHistory>> GetProjectTicketHistoriesAsync(int projectId, int companyId)
        {
            Project project = await _context.Projects.Include(p => p.Tickets)
                                                        .ThenInclude(t => t.Histories)
                                                        .ThenInclude(h => h.User)
                                                        .FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == companyId);

            List<TicketHistory> histories = project.Tickets.SelectMany(t => t.Histories).ToList();

            return histories;
        }
    }
}
