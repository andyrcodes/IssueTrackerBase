using IssueTrackerBase.Data;
using IssueTrackerBase.Models;
using IssueTrackerBase.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace IssueTrackerBase.Services
{
    public class InviteService : IInviteService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IProjectService _projectService;

        public InviteService(
            ApplicationDbContext context, 
            IEmailSender emailSender, 
            IProjectService projectService)
        {
            _context = context;
            _emailSender = emailSender;
            _projectService = projectService;
        }

        public async Task<bool> AcceptInviteAsync(Guid? token, string userId, int companyId)
        {
            try
            {
                Invitation invite = await _context.Invitations.FirstOrDefaultAsync(i => i.CompanyToken == token);

                if(invite is not null)
                {
                    try
                    {
                        invite.IsValid = false;
                        invite.InviteeId = userId;
                        await _context.SaveChangesAsync();

                        return true;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task AddNewInviteAsync(Invitation invitation)
        {
            await _context.AddAsync(invitation);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyInviteAsync(Guid token, string email, int companyId)
        {
            try
            {
                bool result = await _context.Invitations.Where(i => i.CompanyId == companyId).AnyAsync(i => i.CompanyToken == token && i.InviteeEmail == email);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Invitation> GetInvitationAsync(int invitationId, int companyId)
        {
            try
            {
                Invitation invite = await _context.Invitations.Where(i => i.CompanyId == companyId)
                                                                .Include(i => i.Company)
                                                                .Include(i => i.Project)
                                                                .Include(i => i.Invitor)
                                                                .FirstOrDefaultAsync(i => i.Id == invitationId);

                return invite;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<Invitation> GetInvitationAsync(Guid token, string email, int companyId)
        {
            try
            {
                Invitation invite = await _context.Invitations.Where(i => i.CompanyId == companyId)
                                                                .Include(i => i.Company)
                                                                .Include(i => i.Project)
                                                                .Include(i => i.Invitor)
                                                                .FirstOrDefaultAsync(i => i.CompanyToken == token && i.InviteeEmail == email);

                return invite;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ValidateInviteCodeAsync(Guid? token)
        {
            try
            {
                if(token is null)
                {
                    return false;
                }

                bool result = false;
                Invitation invite = await _context.Invitations.FirstOrDefaultAsync(i => i.CompanyToken == token);

                if(invite is not null)
                {
                    DateTime inviteDate = invite.InviteDate;

                    bool validDate = (DateTime.Now - inviteDate).TotalDays <= 7;

                    if (validDate)
                    {
                        result = invite.IsValid;
                    }
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
