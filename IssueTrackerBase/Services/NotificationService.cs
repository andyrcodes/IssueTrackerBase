using IssueTrackerBase.Data;
using IssueTrackerBase.Models;
using IssueTrackerBase.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace IssueTrackerBase.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IRolesService _rolesService;

        public NotificationService(
            ApplicationDbContext context, 
            IEmailSender emailSender, 
            IRolesService rolesService)
        {
            _context = context;
            _emailSender = emailSender;
            _rolesService = rolesService;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetReceivedNotificationsAsync(string userId)
        {
            List<Notification> notifications = new();
            try
            {
                notifications = await _context.Notifications.Where(n => n.RecipientId == userId)
                                                .Include(n => n.Recipient)
                                                .Include(n => n.Sender)
                                                .Include(n => n.Ticket)
                                                    .ThenInclude(t => t.Project)
                                                .ToListAsync();

                return notifications;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Notification>> GetSentNotificationsAsync(string userId)
        {
            List<Notification> notifications = new();
            try
            {
                notifications = await _context.Notifications.Where(n => n.SenderId == userId)
                                                .Include(n => n.Recipient)
                                                .Include(n => n.Sender)
                                                .Include(n => n.Ticket)
                                                    .ThenInclude(t => t.Project)
                                                .ToListAsync();

                return notifications;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SendEmailNotificationAsync(Notification notification, string emailSubject)
        {
            try
            {
                AppUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == notification.RecipientId);

                string userEmail = user.Email;
                string message = notification.Message;

                try
                {
                    await _emailSender.SendEmailAsync(userEmail, emailSubject, message);
                    return true;
                }
                catch(Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendEmailNotificationsByRoleAsync(Notification notification, int companyId, string role)
        {
            try
            {
                List<AppUser> members = await _rolesService.GetUsersInRoleAsync(role, companyId);

                foreach(var user in members)
                {
                    notification.RecipientId = user.Id;
                    await SendEmailNotificationAsync(notification, notification.Title);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendMembersEmailNotificationAsync(Notification notification, List<AppUser> members)
        {
            try
            {
                foreach (var user in members)
                {
                    notification.RecipientId = user.Id;
                    await SendEmailNotificationAsync(notification, notification.Title);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
