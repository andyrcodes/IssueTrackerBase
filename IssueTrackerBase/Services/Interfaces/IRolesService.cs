using IssueTrackerBase.Models;
using Microsoft.AspNetCore.Identity;

namespace IssueTrackerBase.Services.Interfaces
{
    public interface IRolesService
    {
        Task<bool> AddUserToRoleAsync(AppUser user, string roleName);

        Task<List<IdentityRole>> GetRolesAsync();

        Task<string> GetRoleNameByIdAsync(string roleId);

        Task<List<AppUser>> GetUsersInRoleAsync(string roleName, int companyId);

        Task<List<AppUser>> GetUsersNotInRoleAsync(string roleName, int companyId);

        Task<IEnumerable<string>> GetUserRolesAsync(AppUser user);

        Task<bool> IsUserInRoleAsync(AppUser user, string roleName);

        Task<bool> RemoveUserFromRoleAsync(AppUser appUser, string roleName);

        Task<bool> RemoveUserFromRolesAsync(AppUser user, IEnumerable<string> roles);
    }
}
