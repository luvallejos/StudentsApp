using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp;

namespace StudentsApp.Repositories
{
    public interface IStudyGroupRepository
    {
        Task<StudyGroup> GetStudyGroupByIdAsync(int studyGroupId);

        Task<StudyGroup> UpdateStudyGroupAsync(int studyGroupId);

        Task<StudyGroup> CreateStudyGroupAsync(StudyGroup studyGroup, int ownerUserId);

        Task<bool> ExistsForUserAndSubjectAsync(int ownerUserId, string subject);

        Task<List<StudyGroup>> GetAllStudyGroupsAsync();

        Task<List<StudyGroup>> SearchStudyGroupsAsync(string subject);

        Task<IActionResult> JoinStudyGroupAsync(int studyGroupId, int userId);

        Task<IActionResult> LeaveStudyGroupAsync(int studyGroupId, int userId);

    }
}