using IssueTrackerBase.Models;

namespace IssueTrackerBase.Services.Interfaces
{
    public interface IHistoryService
    {
        Task AddHistoryAsync(Ticket oldTicket, Ticket newTicket, string userId);

        Task<List<TicketHistory>> GetProjectTicketHistoriesAsync(int projectId, int companyId);

        Task<List<TicketHistory>> GetCompanyTicketHistoriesAsync(int companyId);
    }
}
