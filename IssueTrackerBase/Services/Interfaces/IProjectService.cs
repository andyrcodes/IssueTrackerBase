using IssueTrackerBase.Models;

namespace IssueTrackerBase.Services.Interfaces
{
    public interface IProjectService
    {
        Task AddNewProjectAsync(Project project);

        Task<bool> AddProjectManagerAsync(string userId, int projectId);

        Task<bool> AddUserToProjectAsync(string userId, int projectId);

        Task ArchiveProjectAsync(Project project);

        Task<List<Project>> GetAllProjectsByCompanyAsync(int companyId);

        Task<List<Project>> GetAllProjectsByPriorityAsync(int companyId, string priorityName);

        Task<List<AppUser>> GetAllProjectMembersExceptPMAsync(int projectId);

        Task<List<Project>> GetArchivedProjectsByCompany(int companyId);

        Task<AppUser> GetProjectManagerAsync(int projectId);

        Task<List<AppUser>> GetProjectMembersByRoleAsync(int projectId, string role);

        Task<Project> GetProjectByIdAsync(int projectId, int companyId);

        Task<List<AppUser>> GetUsersNotOnProjectAsync(int projectId, int companyId);

        Task<List<Project>> GetUnassignedProjectsAsync(int companyId);

        Task<List<Project>> GetUserProjectsAsync(string userId);

        Task<bool> IsUserOnProjectAsync(string userId, int projectId);

        Task<int> LookupProjectPriorityId(string priorityName);

        Task RemoveProjectManagerAsync(int projectId);

        Task RemoveUsersFromProjectByRoleAsync(string role, int projectId);

        Task RemoveUserFromProjectAsync(string userId, int projectId);

        Task UpdateProjectAsync(Project project);


    }
}
