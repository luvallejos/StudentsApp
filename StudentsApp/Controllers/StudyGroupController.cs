using Microsoft.AspNetCore.Mvc;
using StudentsApp.Services;
using System.Threading.Tasks;
using TestApp;

namespace StudentsApp.Controllers
{
    public class StudyGroupController
    {
        private readonly IStudyGroupService _studyGroupService;

        public StudyGroupController(IStudyGroupService studyGroupService)
        {
            _studyGroupService = studyGroupService;
        }

        public async Task<IActionResult> CreateStudyGroup(StudyGroup studyGroup, int ownerUserId)
        {
            await _studyGroupService.CreateStudyGroupAsync(studyGroup, ownerUserId);
            return new OkResult();
        }

        public async Task<IActionResult> GetStudyGroups()
        {
            var studyGroups = await _studyGroupService.GetStudyGroupsAsync();
            return new OkObjectResult(studyGroups);
        }

        public async Task<IActionResult> GetSortedStudyGroups(string sortedCriteria)
        {
            var studyGroups = await _studyGroupService.GetSortedStudyGroupsAsync(sortedCriteria);
            return new OkObjectResult(studyGroups);
        }

        public async Task<IActionResult> SearchStudyGroups(string subject)
        {
            var studyGroups = await _studyGroupService.SearchStudyGroupsAsync(subject);
            return new OkObjectResult(studyGroups);
        }

        public async Task<IActionResult> JoinStudyGroup(int studyGroupId, int userId)
        {
            await _studyGroupService.JoinStudyGroupAsync(studyGroupId, userId);
            return new OkResult();
        }

        public async Task<IActionResult> LeaveStudyGroup(int studyGroupId, int userId)
        {
            await _studyGroupService.LeaveStudyGroupAsync(studyGroupId, userId);
            return new OkResult();
        }
    }
} 
