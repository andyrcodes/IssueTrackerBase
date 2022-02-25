using IssueTrackerBase.Models;

namespace IssueTrackerBase.Services.Interfaces
{
    public interface IInviteService
    {
        Task<bool> AcceptInviteAsync(Guid? token, string userId, int companyId);

        Task AddNewInviteAsync(Invitation invitation);

        Task<bool> AnyInviteAsync(Guid token, string email, int companyId);

        Task<Invitation> GetInvitationAsync(int invitationId, int companyId);

        Task<Invitation> GetInvitationAsync(Guid token, string email, int companyId);

        Task<bool> ValidateInviteCodeAsync(Guid? token);
    }
}
