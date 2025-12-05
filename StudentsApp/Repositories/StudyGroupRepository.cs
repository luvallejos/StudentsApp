using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp;

namespace StudentsApp.Repositories
{
    internal class StudyGroupRepository : IStudyGroupRepository
    {
        public Task<StudyGroup> GetStudyGroupByIdAsync(int studyGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<StudyGroup> UpdateStudyGroupAsync(int studyGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<List<StudyGroup>> GetAllStudyGroupsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<StudyGroup> CreateStudyGroupAsync(StudyGroup studyGroup, int ownerUserId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsForUserAndSubjectAsync(int ownerUserId, string subject)
        {
            throw new NotImplementedException();
        }

        public Task<List<StudyGroup>> GetStudyGroupsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> JoinStudyGroupAsync(int studyGroupId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> LeaveStudyGroupAsync(int studyGroupId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<StudyGroup>> SearchStudyGroupsAsync(string subject)
        {
            throw new NotImplementedException();
        }

    }
}
