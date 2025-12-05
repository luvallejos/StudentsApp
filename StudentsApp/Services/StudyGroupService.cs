using StudentsApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp;

namespace StudentsApp.Services
{
    public class StudyGroupService : IStudyGroupService
    {
        private readonly IStudyGroupRepository _studyGroupRepository;
        private readonly IUserRepository _userRepository;

        public StudyGroupService(IStudyGroupRepository studyGroupRepository, IUserRepository userRepository)
        {
            _studyGroupRepository = studyGroupRepository;
            _userRepository = userRepository;
        }

        public async Task<StudyGroup> CreateStudyGroupAsync(StudyGroup studyGroup, int ownerUserId)
        {
            bool alreadyExists = await _studyGroupRepository.ExistsForUserAndSubjectAsync(ownerUserId, studyGroup.Subject.ToString());

            if (alreadyExists)
                throw new InvalidOperationException("User already has a study group for this subject.");

            var studyGroupCreated = await _studyGroupRepository.CreateStudyGroupAsync(studyGroup, ownerUserId);

            return studyGroupCreated;
        }

        public async Task<List<StudyGroup>> GetStudyGroupsAsync()
        {
            var studyGroupList = await _studyGroupRepository.GetAllStudyGroupsAsync();

            if (studyGroupList.Count == 0)
                throw new ArgumentException("No Study groups found.");

            return studyGroupList;
        }

        public async Task<List<StudyGroup>> GetSortedStudyGroupsAsync(string sortedCriteria)
        {
            var studyGroupList = await _studyGroupRepository.GetAllStudyGroupsAsync();

            if (studyGroupList.Count == 0)
                throw new ArgumentException("Study groups not found.");

            if (sortedCriteria == "NewestFirst")
            {
                return studyGroupList
                    .OrderByDescending(g => g.CreateDate)
                    .ToList();
            }
            else
            {
                return studyGroupList
                    .OrderBy(g => g.CreateDate)
                    .ToList();
            }
        }

        public async Task<List<StudyGroup>> SearchStudyGroupsAsync(string subject)
        {
            if (!Enum.IsDefined(typeof(Subject), subject))
                throw new ArgumentException("Subject is not valid");

            var studyGroupList = await _studyGroupRepository.SearchStudyGroupsAsync(subject);
            if (studyGroupList.Count == 0)
                throw new ArgumentException($"Study groups not found for the subject: {subject}.");

            return studyGroupList;
        }

        public async Task JoinStudyGroupAsync(int studyGroupId, int userId)
        {
            var studyGroup = await _studyGroupRepository.GetStudyGroupByIdAsync(studyGroupId);
            if (studyGroup == null)
                throw new ArgumentException("Study group not found.");

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found.");

            studyGroup.AddUser(user);

            await _studyGroupRepository.UpdateStudyGroupAsync(studyGroupId);
        }

        public async Task LeaveStudyGroupAsync(int studyGroupId, int userId)
        {
            var studyGroup = await _studyGroupRepository.GetStudyGroupByIdAsync(studyGroupId);
            if (studyGroup == null)
                throw new ArgumentException("Study group not found.");

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found.");

            if (!studyGroup.Users.Any(u => u.Id == userId))
                throw new InvalidOperationException("User does not exist in the study group.");

            studyGroup.RemoveUser(user);

            await _studyGroupRepository.UpdateStudyGroupAsync(studyGroupId);

        }
    }
}
