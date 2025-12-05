using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp;

namespace StudentsApp.Services
{
    public interface IStudyGroupService
    {
        Task<StudyGroup> CreateStudyGroupAsync(StudyGroup studyGroup, int ownerUserId);

        Task<List<StudyGroup>> GetStudyGroupsAsync();

        Task<List<StudyGroup>> GetSortedStudyGroupsAsync(string sortedCriteria);

        Task<List<StudyGroup>> SearchStudyGroupsAsync(string subject);

        Task JoinStudyGroupAsync(int studyGroupId, int userId);

        Task LeaveStudyGroupAsync(int studyGroupId, int userId);
    }
}