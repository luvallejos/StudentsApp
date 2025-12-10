using Moq;
using NUnit.Framework;
using StudentsApp;
using StudentsApp.Repositories;
using StudentsApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestApp.Tests
{
    [TestFixture]
    public class StudyGroupServiceTest
    {
        private Mock<IStudyGroupRepository> _studyGroupRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private StudyGroupService _studyGroupService;

        [SetUp]
        public void SetUp()
        {
            _studyGroupRepositoryMock = new Mock<IStudyGroupRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();

            _studyGroupService = new StudyGroupService(_studyGroupRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Test]
        public async Task CreateStudyGroup_ShouldReturnGroup_WhenValidGroupAndDoesNotExistsForUser() 
        {
            // Arrange
            int ownerUserId = 1;

            var studyGroup = new StudyGroup(
                "MathGroup2025",
                Subject.Math,
                1,
                new HashSet<User>()
            );

            _studyGroupRepositoryMock.Setup(r => r.ExistsForUserAndSubjectAsync(ownerUserId, studyGroup.Subject.ToString()))
               .ReturnsAsync(false);

            _studyGroupRepositoryMock.Setup(r => r.CreateStudyGroupAsync(studyGroup, ownerUserId))
               .ReturnsAsync(studyGroup);

            // Act
            var result = await _studyGroupService.CreateStudyGroupAsync(studyGroup, ownerUserId);

            // Assert
            _studyGroupRepositoryMock.Verify(r => r.CreateStudyGroupAsync(studyGroup, ownerUserId), Times.Once);
            Assert.That(result, Is.InstanceOf<StudyGroup>());

        }

        [Test]
        public void CreateStudyGroup_ShouldThrow_WhenGroupAlreadyExistsForUser()
        {
            // Arrange
            int ownerUserId = 1;

            var studyGroup = new StudyGroup(
                "MathGroup2025",
                Subject.Math,
                1,
                new HashSet<User>()
            );

            _studyGroupRepositoryMock.Setup(r => r.ExistsForUserAndSubjectAsync(ownerUserId, studyGroup.Subject.ToString()))
               .ReturnsAsync(true);

            // Act + Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                    await _studyGroupService.CreateStudyGroupAsync(studyGroup, ownerUserId)
                );

            _studyGroupRepositoryMock.Verify(r => r.ExistsForUserAndSubjectAsync(ownerUserId, studyGroup.Subject.ToString()), Times.Once);

            _studyGroupRepositoryMock.Verify(r => r.CreateStudyGroupAsync(studyGroup, ownerUserId), Times.Never);
        }

        [Test]
        public async Task GetStudyGroups_ShouldReturnGroups_WhenThereAreResults()
        {
            // Arrange
            List<StudyGroup> studyGroups = new List<StudyGroup>()
            {
                new StudyGroup("MathGroup2025", Subject.Math, 1, new HashSet<User>()),
                new StudyGroup("ChemistryGroup2025",Subject.Chemistry,1, new HashSet<User>()),
                new StudyGroup("PhysicsGroup2025",Subject.Physics,1, new HashSet<User>())
            };

            _studyGroupRepositoryMock.Setup(r => r.GetAllStudyGroupsAsync())
               .ReturnsAsync(studyGroups);

            // Act
            var result = await _studyGroupService.GetStudyGroupsAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<List<StudyGroup>>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0].Name, Is.EqualTo("MathGroup2025"));
            Assert.That(result[0].Subject, Is.EqualTo(Subject.Math));
            Assert.That(result[1].Name, Is.EqualTo("ChemistryGroup2025"));
            Assert.That(result[1].Subject, Is.EqualTo(Subject.Chemistry));
            Assert.That(result[2].Name, Is.EqualTo("PhysicsGroup2025"));
            Assert.That(result[2].Subject, Is.EqualTo(Subject.Physics));
        }

        [Test]
        public void GetStudyGroups_ShouldThrow_WhenThereAreNoResults()
        {
            //Arrange
            List<StudyGroup> studyGroups = new List<StudyGroup>();

            _studyGroupRepositoryMock.Setup(r => r.GetAllStudyGroupsAsync())
               .ReturnsAsync(studyGroups);

            //Act + Assert
            Assert.ThrowsAsync<ArgumentException>(
                () => _studyGroupService.GetStudyGroupsAsync()
             );
        }

        [Test]
        public async Task GetSortedStudyGroups_ShouldReturnSortedGroups_WhenSortingNewestFirst()
        {
            //Arrange
            List<StudyGroup> studyGroups = new List<StudyGroup>()
            {
                new StudyGroup("MathGroup2025", Subject.Math, 1,new HashSet<User>(), new DateTime(2025, 08, 10)),
                new StudyGroup("ChemistryGroup2025",Subject.Chemistry,1, new HashSet<User>(), new DateTime(2025, 03, 12)),
                new StudyGroup("PhysicsGroup2025",Subject.Physics,1, new HashSet<User>(), new DateTime(2025, 05, 21))
            };

            string sortedCriteria = "NewestFirst";

            _studyGroupRepositoryMock.Setup(r => r.GetAllStudyGroupsAsync())
               .ReturnsAsync(studyGroups);

            //Act
            var result = await _studyGroupService.GetSortedStudyGroupsAsync(sortedCriteria);

            //Assert
            Assert.That(result, Is.InstanceOf<List<StudyGroup>>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0].Name, Is.EqualTo("MathGroup2025"));
            Assert.That(result[0].Subject, Is.EqualTo(Subject.Math));
            Assert.That(result[1].Name, Is.EqualTo("PhysicsGroup2025"));
            Assert.That(result[1].Subject, Is.EqualTo(Subject.Physics));
            Assert.That(result[2].Name, Is.EqualTo("ChemistryGroup2025"));
            Assert.That(result[2].Subject, Is.EqualTo(Subject.Chemistry));
        }

        [Test]
        public async Task GetSortedStudyGroups_ShouldReturnSortedGroups_WhenSortingOldestFirst()
        {
            //Arrange
            List<StudyGroup> studyGroups = new List<StudyGroup>()
            {
                new StudyGroup("MathGroup2025", Subject.Math,1, new HashSet<User>(), new DateTime(2025, 08, 10)),
                new StudyGroup("ChemistryGroup2025",Subject.Chemistry,1, new HashSet<User>(), new DateTime(2025, 03, 12)),
                new StudyGroup("PhysicsGroup2025",Subject.Physics,1, new HashSet<User>(), new DateTime(2025, 05, 21))
            };

            string sortedCriteria = "OldestFirst";

            _studyGroupRepositoryMock.Setup(r => r.GetAllStudyGroupsAsync())
               .ReturnsAsync(studyGroups);

            //Act
            var result = await _studyGroupService.GetSortedStudyGroupsAsync(sortedCriteria);

            //Assert
            Assert.That(result, Is.InstanceOf<List<StudyGroup>>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0].Name, Is.EqualTo("ChemistryGroup2025"));
            Assert.That(result[0].Subject, Is.EqualTo(Subject.Chemistry));
            Assert.That(result[1].Name, Is.EqualTo("PhysicsGroup2025"));
            Assert.That(result[1].Subject, Is.EqualTo(Subject.Physics));
            Assert.That(result[2].Name, Is.EqualTo("MathGroup2025"));
            Assert.That(result[2].Subject, Is.EqualTo(Subject.Math));
        }

        [Test]
        public void GetSortedStudyGroups_ShouldThrow_WhenThereAreNoResults()
        {
            //Arrange
            List<StudyGroup> studyGroups = new List<StudyGroup>();
            string sortedCriteria = "";

            _studyGroupRepositoryMock.Setup(r => r.GetAllStudyGroupsAsync())
               .ReturnsAsync(studyGroups);

            //Act + Assert
            Assert.ThrowsAsync<ArgumentException>(
                () => _studyGroupService.GetSortedStudyGroupsAsync(sortedCriteria)
             );
        }


        [Test]
        public async Task SearchStudyGroups_ShouldReturnGroups_WhenValidSubjectAndThereAreResults()
        {
            //Arrange
            List<StudyGroup> studyGroup = new List<StudyGroup>()
            {
                new StudyGroup("MathGroup2025", Subject.Math,1, new HashSet<User>()),
            };

            string validSubject = Subject.Math.ToString();

            _studyGroupRepositoryMock.Setup(r => r.SearchStudyGroupsAsync(validSubject))
               .ReturnsAsync(studyGroup);

            //Act
            var result = await _studyGroupService.SearchStudyGroupsAsync(validSubject);

            //Assert
            Assert.That(result, Is.InstanceOf<List<StudyGroup>>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("MathGroup2025"));
            Assert.That(result[0].Subject, Is.EqualTo(Subject.Math));
        }

        [Test]
        public void SearchStudyGroups_ShouldThrow_WhenValidSubjectAndThereAreNoResults()
        {
            //Arrange
            List<StudyGroup> studyGroups = new List<StudyGroup>();

            string validSubject = Subject.Math.ToString();

            _studyGroupRepositoryMock.Setup(r => r.SearchStudyGroupsAsync(validSubject))
               .ReturnsAsync(studyGroups);

            //Act + Assert
            Assert.ThrowsAsync<ArgumentException>(
                () => _studyGroupService.SearchStudyGroupsAsync(validSubject)
             );
        }

        [Test]
        public void SearchStudyGroups_ShouldThrow_WhenInvalidSubject()
        {
            //Arrange
            string invalidSubject = "";

            //Act + Assert
            Assert.ThrowsAsync<ArgumentException>(
                () => _studyGroupService.SearchStudyGroupsAsync(invalidSubject)
             );
        }

        [Test]
        public async Task JoinStudyGroup_ShouldAddUser_WhenGroupExistsAndUserDoesNotExistInGroup()
        {
            //Arrange
            StudyGroup studyGroup = new StudyGroup("MathGroup2025", Subject.Math, 1,new HashSet<User>());
            int studyGroupId = 10;

            _studyGroupRepositoryMock.Setup(r => r.GetStudyGroupByIdAsync(studyGroupId))
               .ReturnsAsync(studyGroup);

            User user = new User("John");
            int userId = 2;

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(userId))
               .ReturnsAsync(user);

            StudyGroup studyGroupUpdated = new StudyGroup("MathGroup2025", Subject.Math,1, new HashSet<User>() {user});

            _studyGroupRepositoryMock.Setup(r => r.UpdateStudyGroupAsync(studyGroupId))
               .ReturnsAsync(studyGroup);

            //Act
            await _studyGroupService.JoinStudyGroupAsync(studyGroupId, userId);

            //Assert
            _userRepositoryMock.Verify(r => r.GetUserByIdAsync(userId), Times.Once);
            _studyGroupRepositoryMock.Verify(r => r.GetStudyGroupByIdAsync(studyGroupId), Times.Once);
            _studyGroupRepositoryMock.Verify(r => r.UpdateStudyGroupAsync(studyGroupId), Times.Once);
            Assert.That(studyGroup.Users.Count, Is.EqualTo(1));
        }

        [Test]
        public void JoinStudyGroup_ShoulThrow_WhenGroupExistsAndUserAlreadyExistsInGroup()
        {
            //Arrange
            User user = new User("John");
            int userId = 2;

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(userId))
               .ReturnsAsync(user);

            StudyGroup studyGroup = new StudyGroup("MathGroup2025", Subject.Math, 1, new HashSet<User>() {user});
            int studyGroupId = 10;

            _studyGroupRepositoryMock.Setup(r => r.GetStudyGroupByIdAsync(studyGroupId))
               .ReturnsAsync(studyGroup);

            //Act
            Assert.ThrowsAsync<InvalidOperationException>(
                () => _studyGroupService.JoinStudyGroupAsync(studyGroupId, userId)
             );

            //Assert
            _userRepositoryMock.Verify(r => r.GetUserByIdAsync(userId), Times.Once);
            _studyGroupRepositoryMock.Verify(r => r.GetStudyGroupByIdAsync(studyGroupId), Times.Once);
            _studyGroupRepositoryMock.Verify(r => r.UpdateStudyGroupAsync(studyGroupId), Times.Never);
            Assert.That(studyGroup.Users.Count, Is.EqualTo(1));
        }

        [Test]
        public void JoinStudyGroup_ShoulThrow_WhenInvalidGroup()
        {
            //Arrange
            int studyGroupId = 10;
            int userId = 2;

            _studyGroupRepositoryMock.Setup(r => r.GetStudyGroupByIdAsync(studyGroupId))
               .ReturnsAsync((StudyGroup)null);

            //Act
            Assert.ThrowsAsync<ArgumentException>(
                () => _studyGroupService.JoinStudyGroupAsync(studyGroupId, userId)
             );

            //Assert
            _studyGroupRepositoryMock.Verify(r => r.GetStudyGroupByIdAsync(studyGroupId), Times.Once);
            _studyGroupRepositoryMock.Verify(r => r.UpdateStudyGroupAsync(studyGroupId), Times.Never);
        }

        [Test]
        public void JoinStudyGroup_ShoulThrow_WhenUserNotFound()
        {
            //Arrange
            StudyGroup studyGroup = new StudyGroup("MathGroup2025", Subject.Math,1, new HashSet<User>());
            int studyGroupId = 10;

            _studyGroupRepositoryMock.Setup(r => r.GetStudyGroupByIdAsync(studyGroupId))
               .ReturnsAsync(studyGroup);

            int userId = 2;

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(userId))
               .ReturnsAsync((User)null);

            //Act
            Assert.ThrowsAsync<ArgumentException>(
                () => _studyGroupService.JoinStudyGroupAsync(studyGroupId, userId)
             );

            //Assert
            _studyGroupRepositoryMock.Verify(r => r.GetStudyGroupByIdAsync(studyGroupId), Times.Once);
            _userRepositoryMock.Verify(r => r.GetUserByIdAsync(userId), Times.Once);
            _studyGroupRepositoryMock.Verify(r => r.UpdateStudyGroupAsync(studyGroupId), Times.Never);
            Assert.That(studyGroup.Users.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task LeaveStudyGroup_ShouldRemoveUser_WhenGroupExistsAndUserAlreadyExistsInGroup()
        {
            //Arrange
            User user = new User("John");
            int userId = 2;
            user.SetId(userId);

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(userId))
               .ReturnsAsync(user);

            StudyGroup studyGroup = new StudyGroup("MathGroup2025", Subject.Math,1, new HashSet<User>() { user });
            int studyGroupId = 10;

            _studyGroupRepositoryMock.Setup(r => r.GetStudyGroupByIdAsync(studyGroupId))
               .ReturnsAsync(studyGroup);

            //Act
            await _studyGroupService.LeaveStudyGroupAsync(studyGroupId, userId);

            //Assert
            _studyGroupRepositoryMock.Verify(r => r.GetStudyGroupByIdAsync(studyGroupId), Times.Once);
            _userRepositoryMock.Verify(r => r.GetUserByIdAsync(userId), Times.Once);
            _studyGroupRepositoryMock.Verify(r => r.UpdateStudyGroupAsync(studyGroupId), Times.Once);
            Assert.That(studyGroup.Users.Count, Is.EqualTo(0));
        }

        [Test]
        public void LeaveStudyGroup_ShoulThrow_WhenValidGroupAndUserDoesNotExistInGroup()
        {
            //Arrange
            User user = new User("John");
            int userIdDoesNotExistInGroup = 2;
            user.SetId(userIdDoesNotExistInGroup);

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(userIdDoesNotExistInGroup))
               .ReturnsAsync(user);

            User userInGroup = new User("Charly");
            int userIdInGroup = 3;
            userInGroup.SetId(userIdInGroup);

            StudyGroup studyGroup = new StudyGroup("MathGroup2025", Subject.Math,1, new HashSet<User>() { userInGroup });
            int studyGroupId = 10;

            _studyGroupRepositoryMock.Setup(r => r.GetStudyGroupByIdAsync(studyGroupId))
               .ReturnsAsync(studyGroup);

            //Act
            Assert.ThrowsAsync<InvalidOperationException>(
                () => _studyGroupService.LeaveStudyGroupAsync(studyGroupId, userIdDoesNotExistInGroup)
             );

            //Assert
            _studyGroupRepositoryMock.Verify(r => r.GetStudyGroupByIdAsync(studyGroupId), Times.Once);
            _userRepositoryMock.Verify(r => r.GetUserByIdAsync(userIdDoesNotExistInGroup), Times.Once);
            _studyGroupRepositoryMock.Verify(r => r.UpdateStudyGroupAsync(studyGroupId), Times.Never);
            Assert.That(studyGroup.Users.Count, Is.EqualTo(1));
        }

        [Test]
        public void LeaveStudyGroup_ShoulThrow_WhenInvalidGroup()
        {
            //Arrange
            int studyGroupId = 10;
            int userId = 2;

            _studyGroupRepositoryMock.Setup(r => r.GetStudyGroupByIdAsync(studyGroupId))
               .ReturnsAsync((StudyGroup)null);

            //Act
            Assert.ThrowsAsync<ArgumentException>(
                () => _studyGroupService.LeaveStudyGroupAsync(studyGroupId, userId)
             );

            //Assert
            _studyGroupRepositoryMock.Verify(r => r.GetStudyGroupByIdAsync(studyGroupId), Times.Once);
            _studyGroupRepositoryMock.Verify(r => r.UpdateStudyGroupAsync(studyGroupId), Times.Never);
        }

        [Test]
        public void LeaveStudyGroup_ShoulThrow_WhenUserNotFound()
        {
            //Arrange
            StudyGroup studyGroup = new StudyGroup("MathGroup2025", Subject.Math, 1,new HashSet<User>());
            int studyGroupId = 10;

            _studyGroupRepositoryMock.Setup(r => r.GetStudyGroupByIdAsync(studyGroupId))
               .ReturnsAsync(studyGroup);

            int userId = 2;

            _userRepositoryMock.Setup(r => r.GetUserByIdAsync(userId))
               .ReturnsAsync((User)null);

            //Act
            Assert.ThrowsAsync<ArgumentException>(
                () => _studyGroupService.LeaveStudyGroupAsync(studyGroupId, userId)
             );

            //Assert
            _studyGroupRepositoryMock.Verify(r => r.GetStudyGroupByIdAsync(studyGroupId), Times.Once);
            _studyGroupRepositoryMock.Verify(r => r.UpdateStudyGroupAsync(studyGroupId), Times.Never);
            Assert.That(studyGroup.Users.Count, Is.EqualTo(0));
        }

    }
}
