using IssueTrackerBase.Data;
using IssueTrackerBase.Enums;
using IssueTrackerBase.Models;
using IssueTrackerBase.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IssueTrackerBase.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRolesService _rolesService;

        public ProjectService(
            ApplicationDbContext context,
            IRolesService rolesService)
        {
            _context = context;
            _rolesService = rolesService;
        }

        public async Task AddNewProjectAsync(Project project)
        {
            try
            {
                await _context.AddAsync(project);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddProjectManagerAsync(string userId, int projectId)
        {
            try
            {
                AppUser currentPM = await GetProjectManagerAsync(projectId);

                if (currentPM is not null)
                {
                    try
                    {
                        await RemoveProjectManagerAsync(projectId);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                try
                {
                    await AddUserToProjectAsync(userId, projectId);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddUserToProjectAsync(string userId, int projectId)
        {
            try
            {
                AppUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

                if (user is not null)
                {
                    if (!await IsUserOnProjectAsync(userId, projectId))
                    {
                        try
                        {
                            Project project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
                            project.Members.Add(user);
                            await UpdateProjectAsync(project);
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
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ArchiveProjectAsync(Project project)
        {
            try
            {
                project.Archived = true;
                _context.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AppUser>> GetAllProjectMembersExceptPMAsync(int projectId)
        {
            try
            {
                List<AppUser> developers = await GetProjectMembersByRoleAsync(projectId, nameof(Roles.Developer));
                List<AppUser> submitters = await GetProjectMembersByRoleAsync(projectId, nameof(Roles.Submitter));
                List<AppUser> admins = await GetProjectMembersByRoleAsync(projectId, nameof(Roles.Admin));

                List<AppUser> members = developers.Concat(admins).Concat(submitters).ToList();

                return members;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Project>> GetAllProjectsByCompanyAsync(int companyId)
        {
            List<Project> projects = new();
            try
            {
                projects = await _context.Projects.Where(p => p.CompanyId == companyId && !p.Archived)
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
                                                        .ThenInclude(t => t.TicketStatus)
                                                    .Include(p => p.Tickets)
                                                        .ThenInclude(t => t.TicketPriority)
                                                    .Include(p => p.Tickets)
                                                        .ThenInclude(t => t.TicketType)
                                                    .Include(p => p.ProjectPriority)
                                                    .ToListAsync();

                return projects;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Project>> GetAllProjectsByPriorityAsync(int companyId, string priorityName)
        {
            try
            {
                List<Project> projects = await GetAllProjectsByCompanyAsync(companyId);
                int priorityId = await LookupProjectPriorityId(priorityName);

                return projects.Where(p => p.ProjectPriorityId == priorityId).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Project>> GetArchivedProjectsByCompany(int companyId)
        {
            List<Project> projects = new();
            try
            {
                projects = await _context.Projects.Where(p => p.CompanyId == companyId && p.Archived)
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
                                                        .ThenInclude(t => t.TicketStatus)
                                                    .Include(p => p.Tickets)
                                                        .ThenInclude(t => t.TicketPriority)
                                                    .Include(p => p.Tickets)
                                                        .ThenInclude(t => t.TicketType)
                                                    .Include(p => p.ProjectPriority)
                                                    .ToListAsync();

                return projects;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Project> GetProjectByIdAsync(int projectId, int companyId)
        {
            try
            {
                Project project = await _context.Projects
                                                    .Include(p => p.Tickets)
                                                    .Include(p => p.Members)
                                                    .Include(p => p.ProjectPriority)
                                                    .FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == companyId);

                return project;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AppUser> GetProjectManagerAsync(int projectId)
        {
            try
            {
                Project project = await _context.Projects.Include(p => p.Members).FirstOrDefaultAsync(p => p.Id == projectId);

                foreach(var user in project.Members)
                {
                    if (await _rolesService.IsUserInRoleAsync(user, nameof(Roles.ProjectManager)))
                    {
                        return user;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AppUser>> GetProjectMembersByRoleAsync(int projectId, string role)
        {
            try
            {
                Project project = await _context.Projects
                                                    .Include(p => p.Members)
                                                    .FirstOrDefaultAsync(p => p.Id == projectId);

                List<AppUser> members = new();

                foreach(var user in project.Members)
                {
                    if(await _rolesService.IsUserInRoleAsync(user, role))
                    {
                        members.Add(user);
                    }
                }

                return members;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Project>> GetUnassignedProjectsAsync(int companyId)
        {
            List<Project> result = new();
            List<Project> projects = new();

            try
            {
                projects = await _context.Projects.Where(p => p.CompanyId == companyId).ToListAsync();

                foreach(Project project in projects)
                {
                    if((await GetProjectMembersByRoleAsync(project.Id, nameof(Roles.ProjectManager))).Count == 0)
                    {
                        result.Add(project);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public async Task<List<Project>> GetUserProjectsAsync(string userId)
        {
            try
            {
                List<Project> userProjects = (await _context.Users
                                                                .Include(u => u.Projects)
                                                                    .ThenInclude(p => p.Company)
                                                                .Include(u => u.Projects)
                                                                    .ThenInclude(p => p.Members)
                                                                .Include(u => u.Projects)
                                                                    .ThenInclude(p => p.Tickets)
                                                                .Include(u => u.Projects)
                                                                    .ThenInclude(p => p.Tickets)
                                                                        .ThenInclude(t => t.DeveloperUser)
                                                                .Include(u => u.Projects)
                                                                    .ThenInclude(p => p.Tickets)
                                                                         .ThenInclude(t => t.OwnerUser)
                                                                .Include(u => u.Projects)
                                                                    .ThenInclude(p => p.Tickets)
                                                                        .ThenInclude(t => t.TicketPriority)
                                                                .Include(u => u.Projects)
                                                                    .ThenInclude(p => p.Tickets)
                                                                        .ThenInclude(t => t.TicketType)
                                                                .Include(u => u.Projects)
                                                                    .ThenInclude(p => p.Tickets)
                                                                        .ThenInclude(t => t.TicketStatus)
                                                                .FirstOrDefaultAsync(u => u.Id == userId)).Projects.ToList();

                return userProjects;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"*** ERROR *** - Error retrieving user projects list. --> {ex.Message}");
                throw;
            }
        }

        public async Task<List<AppUser>> GetUsersNotOnProjectAsync(int projectId, int companyId)
        {
            try
            {
                List<AppUser> users = await _context.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToListAsync();
                return users.Where(u => u.CompanyId == companyId).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsUserOnProjectAsync(string userId, int projectId)
        {
            try
            {
                Project project = await _context.Projects.Include(p => p.Members).FirstOrDefaultAsync(p => p.Id == projectId);
                bool result = false;

                if(project is not null)
                {
                    result = project.Members.Any(m => m.Id == userId);
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> LookupProjectPriorityId(string priorityName)
        {
            try
            {
                int priorityId = (await _context.ProjectPriorities.FirstOrDefaultAsync(p => p.Name == priorityName)).Id;
                return priorityId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RemoveProjectManagerAsync(int projectId)
        {
            try
            {
                Project project = await _context.Projects.Include(p => p.Members).FirstOrDefaultAsync(p => p.Id == projectId);

                try
                {
                    foreach(AppUser user in project.Members)
                    {
                        if(await _rolesService.IsUserInRoleAsync(user, nameof(Roles.ProjectManager)))
                        {
                            await RemoveUserFromProjectAsync(user.Id, projectId);
                        }
                    }
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

        public async Task RemoveUserFromProjectAsync(string userId, int projectId)
        {
            try
            {
                AppUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                Project project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

                try
                {
                    if(await IsUserOnProjectAsync(userId, projectId))
                    {
                        project.Members.Remove(user);
                        await UpdateProjectAsync(project);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"*** ERROR *** - Error removing user from project -> {ex.Message}");
                throw;
            }
        }

        public async Task RemoveUsersFromProjectByRoleAsync(string role, int projectId)
        {
            try
            {
                List<AppUser> members = await GetProjectMembersByRoleAsync(projectId, role);
                Project project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

                foreach(AppUser user in members)
                {
                    try
                    {
                        project.Members.Remove(user);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                await UpdateProjectAsync(project);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateProjectAsync(Project project)
        {
            try
            {
                _context.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
