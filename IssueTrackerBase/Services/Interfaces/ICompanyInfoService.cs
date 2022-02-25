using IssueTrackerBase.Models;

namespace IssueTrackerBase.Services.Interfaces
{
    public interface ICompanyInfoService
    {
        Task<Company> GetCompanyInfoByIdAsync(int? companyId);

        Task<List<AppUser>> GetAllMembersAsync(int companyId);

        Task<List<Project>> GetAllProjectsAsync(int companyId);

        Task<List<Project>> GetArchivedProjectsAsync(int companyId);

        Task<List<Ticket>> GetAllTicketAsync(int companyId);

        Task<List<AppUser>> GetAllMembersByRoleAsync(int? companyId, string role);
    }
}
