using IssueTrackerBase.Data;
using IssueTrackerBase.Models;
using IssueTrackerBase.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IssueTrackerBase.Services
{
    public class CompanyInfoService : ICompanyInfoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRolesService _rolesService;


        public CompanyInfoService(
            ApplicationDbContext context, 
            IRolesService rolesService)
        {
            _context = context;
            _rolesService = rolesService;
        }



        public async Task<List<AppUser>> GetAllMembersAsync(int companyId)
        {
            List<AppUser> result = new();

            try
            {
                result = await _context.Users.Where(u => u.CompanyId == companyId).ToListAsync();

                return result;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<List<AppUser>> GetAllMembersByRoleAsync(int? companyId, string role)
        {
            List<AppUser> result = new();

            try
            {
                var members = await _context.Users.Where(u => u.CompanyId == companyId).ToListAsync();
                foreach (var member in members)
                {
                    if(await _rolesService.IsUserInRoleAsync(member, role))
                    {
                        result.Add(member);
                    }
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Project>> GetAllProjectsAsync(int companyId)
        {
            List<Project> result = new();

            try
            {
                result = await _context.Projects.Where(p => p.CompanyId == companyId && !p.Archived)
                    .Include(p => p.Members)
                    .Include(p => p.Tickets)
                        .ThenInclude(t => t.Comments)
                    .Include(p => p.Tickets)
                        .ThenInclude(t => t.Attachments)
                    .Include(p => p.Tickets)
                        .ThenInclude(t => t.Histories)
                    .Include(p => p.Tickets)
                        .ThenInclude(t => t.Notifications)
                    .Include(p => p.Tickets)
                        .ThenInclude(t => t.DeveloperUser)
                    .Include(p => p.Tickets)
                        .ThenInclude(t => t.OwnerUser)
                    .Include(p => p.Tickets)
                        .ThenInclude(t => t.TicketPriority)
                    .Include(p => p.Tickets)
                        .ThenInclude(t => t.TicketStatus)
                    .Include(p => p.Tickets)
                        .ThenInclude(t => t.TicketType)
                    .Include(p => p.ProjectPriority)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketAsync(int companyId)
        {
            List<Ticket> result = new();
            List<Project> projects = new();

            try
            {
                projects = await GetAllProjectsAsync(companyId);
                result = projects.SelectMany(p => p.Tickets).ToList();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Project>> GetArchivedProjectsAsync(int companyId)
        {
            List<Project> result = new();

            try
            {
                result = await _context.Projects.Where(p => p.CompanyId == companyId && p.Archived)
                    .Include(p => p.Members)
                    .Include(p => p.Tickets)
                        .ThenInclude(t => t.Comments)
                    .Include(p => p.Tickets)
                        .ThenInclude(t => t.Attachments)
                    .Include(p => p.Tickets)
                        .ThenInclude(t => t.Histories)
                    .Include(p => p.Tickets)
                        .ThenInclude(t => t.Notifications)
                    .Include(p => p.Tickets)
                        .ThenInclude(t => t.DeveloperUser)
                    .Include(p => p.Tickets)
                        .ThenInclude(t => t.OwnerUser)
                    .Include(p => p.Tickets)
                        .ThenInclude(t => t.TicketPriority)
                    .Include(p => p.Tickets)
                        .ThenInclude(t => t.TicketStatus)
                    .Include(p => p.Tickets)
                        .ThenInclude(t => t.TicketType)
                    .Include(p => p.ProjectPriority)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Company> GetCompanyInfoByIdAsync(int? companyId)
        {
            Company result = new();

            try
            {
                if (companyId is not null)
                {
                    result = await _context.Companies.Include(c => c.Members).Include(c => c.Projects).Include(c => c.Invitations).FirstOrDefaultAsync(c => c.Id == companyId);
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
