using IssueTrackerBase.Data;
using IssueTrackerBase.Models;
using IssueTrackerBase.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IssueTrackerBase.Services
{
    public class RolesService : IRolesService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RolesService(
            ApplicationDbContext context, 
            RoleManager<IdentityRole> roleManager, 
            UserManager<AppUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<bool> AddUserToRoleAsync(AppUser user, string roleName)
        {
            try
            {
                bool result = (await _userManager.AddToRoleAsync(user, roleName)).Succeeded;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GetRoleNameByIdAsync(string roleId)
        {
            try
            {
                IdentityRole role = _context.Roles.Find(roleId);

                string result = await _roleManager.GetRoleNameAsync(role);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<IdentityRole>> GetRolesAsync()
        {
            try
            {
                List<IdentityRole> result = await _context.Roles.ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(AppUser user)
        {
            try
            {
                IEnumerable<string> result = await _userManager.GetRolesAsync(user);
                
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AppUser>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            try
            {
                List<AppUser> users = (await _userManager.GetUsersInRoleAsync(roleName)).ToList();
                List<AppUser> result = users.Where(u => u.CompanyId == companyId).ToList();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AppUser>> GetUsersNotInRoleAsync(string roleName, int companyId)
        {
            try
            {
                List<string> userIds = (await _userManager.GetUsersInRoleAsync(roleName)).Select(u => u.Id).ToList();
                List<AppUser> roleUsers = await _context.Users.Where(u => !userIds.Contains(u.Id)).ToListAsync();

                List<AppUser> result = roleUsers.Where(u => u.CompanyId == companyId).ToList();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsUserInRoleAsync(AppUser user, string roleName)
        {
            try
            {
                bool result = await _userManager.IsInRoleAsync(user, roleName);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveUserFromRoleAsync(AppUser appUser, string roleName)
        {
            try
            {
                bool result = (await _userManager.RemoveFromRoleAsync(appUser, roleName)).Succeeded;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveUserFromRolesAsync(AppUser user, IEnumerable<string> roles)
        {
            try
            {
                bool result = (await _userManager.RemoveFromRolesAsync(user, roles)).Succeeded;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
