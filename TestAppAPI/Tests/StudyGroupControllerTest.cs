using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using StudentsApp;
using StudentsApp.Controllers;
using StudentsApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp;

namespace TestAppAPI.Tests
{
    [TestFixture]
    public class StudyGroupControllerTest
    {
        private Mock<IStudyGroupService> _studyGroupServiceMock;
        private StudyGroupController _studyGroupController;

        [SetUp]
        public void SetUp()
        {
            _studyGroupServiceMock = new Mock<IStudyGroupService>();
            _studyGroupController = new StudyGroupController(_studyGroupServiceMock.Object);
        }

        [Test]
        public async Task CreateStudyGroup_ShouldReturnOkResult_WhenGroupCreated()
        {
            //Arrange
            int ownerUserId = 1;

            var studyGroup = new StudyGroup(
                "MathGroup2025",
                Subject.Math,
                1,
                new HashSet<User>()
            );

            _studyGroupServiceMock.Setup(s => s.CreateStudyGroupAsync(studyGroup, ownerUserId))
               .ReturnsAsync(studyGroup);

            //Act
            var result = await _studyGroupController.CreateStudyGroup(studyGroup, ownerUserId);

            //Assert
            _studyGroupServiceMock.Verify(s => s.CreateStudyGroupAsync(studyGroup, ownerUserId), Times.Once);
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public void CreateStudyGroup_ShouldThrow_WhenGroupCreationFails()
        {
            //Arrange
            _studyGroupServiceMock.Setup(s => s.CreateStudyGroupAsync((StudyGroup)null, 0))
               .ThrowsAsync(new Exception());

            //Act + Assert
            Assert.ThrowsAsync<Exception>(async () =>
                    await _studyGroupController.CreateStudyGroup((StudyGroup)null, 0)
                );

            _studyGroupServiceMock.Verify(s => s.CreateStudyGroupAsync((StudyGroup)null, 0), Times.Once);
        }

        [Test]
        public async Task GetStudyGroups_ShouldReturnOkAndGroups_WhenRequestSuccesfull()
        {
            //Arrange
            List<StudyGroup> studyGroups = new List<StudyGroup>()
            {
                new StudyGroup("MathGroup2025", Subject.Math, 1, new HashSet<User>()),
                new StudyGroup("ChemistryGroup2025",Subject.Chemistry, 1, new HashSet<User>()),
                new StudyGroup("PhysicsGroup2025",Subject.Physics, 1, new HashSet<User>())
            };

            _studyGroupServiceMock.Setup(s => s.GetStudyGroupsAsync())
               .ReturnsAsync(studyGroups);

            //Act
            var result = await _studyGroupController.GetStudyGroups() as OkObjectResult;


            //Assert
            _studyGroupServiceMock.Verify(s => s.GetStudyGroupsAsync(), Times.Once);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var returned = result.Value as List<StudyGroup>;

            Assert.That(returned, Is.Not.Null);
            Assert.That(returned.Count, Is.EqualTo(3));
            Assert.That(returned[0].Name, Is.EqualTo("MathGroup2025"));
            Assert.That(returned[0].Subject, Is.EqualTo(Subject.Math));
            Assert.That(returned[1].Name, Is.EqualTo("ChemistryGroup2025"));
            Assert.That(returned[1].Subject, Is.EqualTo(Subject.Chemistry));
            Assert.That(returned[2].Name, Is.EqualTo("PhysicsGroup2025"));
            Assert.That(returned[2].Subject, Is.EqualTo(Subject.Physics));
        }

        [Test]
        public void GetStudyGroups_ShouldThrow_WhenRequestFails()
        {
            //Arrange
            _studyGroupServiceMock.Setup(s => s.GetStudyGroupsAsync())
                .ThrowsAsync(new Exception());

            //Act + Assert
            Assert.ThrowsAsync<Exception>(async () =>
                    await _studyGroupController.GetStudyGroups()
                );

            _studyGroupServiceMock.Verify(s => s.GetStudyGroupsAsync(), Times.Once);
        }

        [Test]
        public async Task GetSortedStudyGroups_ShouldReturnSortedGroups_WhenResultsFound()
        {
            //Arrange
            List<StudyGroup> studyGroups = new List<StudyGroup>()
            {
                new StudyGroup("MathGroup2025", Subject.Math, 1, new HashSet<User>(), new DateTime(2025, 08, 10)),
                new StudyGroup("PhysicsGroup2025",Subject.Physics, 1, new HashSet<User>(), new DateTime(2025, 05, 21)),
                new StudyGroup("ChemistryGroup2025",Subject.Chemistry,1, new HashSet<User>(), new DateTime(2025, 03, 12))
            };

            string sortedCriteria = "NewestFirst";

            _studyGroupServiceMock.Setup(s => s.GetSortedStudyGroupsAsync(sortedCriteria))
               .ReturnsAsync(studyGroups);

            //Act
            var result = await _studyGroupController.GetSortedStudyGroups(sortedCriteria) as OkObjectResult;

            //Assert
            _studyGroupServiceMock.Verify(s => s.GetSortedStudyGroupsAsync(sortedCriteria), Times.Once);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var returned = result.Value as List<StudyGroup>;

            Assert.That(result, Is.Not.Null);
            Assert.That(returned.Count, Is.EqualTo(3));
            Assert.That(returned[0].Name, Is.EqualTo("MathGroup2025"));
            Assert.That(returned[0].Subject, Is.EqualTo(Subject.Math));
            Assert.That(returned[1].Name, Is.EqualTo("PhysicsGroup2025"));
            Assert.That(returned[1].Subject, Is.EqualTo(Subject.Physics));
            Assert.That(returned[2].Name, Is.EqualTo("ChemistryGroup2025"));
            Assert.That(returned[2].Subject, Is.EqualTo(Subject.Chemistry));
        }

        [Test]
        public void GetSortedStudyGroups_ShouldThrow_WhenRequestFails()
        {
            //Arrange
            string sortedCriteria = "";

            _studyGroupServiceMock.Setup(s => s.GetSortedStudyGroupsAsync(sortedCriteria))
                      .ThrowsAsync(new Exception());

            //Act + Assert
            Assert.ThrowsAsync<Exception>(async () =>
                    await _studyGroupController.GetSortedStudyGroups(sortedCriteria)
                );

            _studyGroupServiceMock.Verify(s => s.GetSortedStudyGroupsAsync(sortedCriteria), Times.Once);
        }

        [Test]
        public async Task SearchStudyGroups_ShouldReturnGroupsBySubject_WhenResultsFound()
        {
            //Arrange
            string subject = Subject.Math.ToString();

            List<StudyGroup> studyGroups = new List<StudyGroup>()
            {
                new StudyGroup("MathGroup2025", Subject.Math, 1, new HashSet<User>()),
            };

            _studyGroupServiceMock.Setup(s => s.SearchStudyGroupsAsync(subject))
               .ReturnsAsync(studyGroups);

            //Act
            var result = await _studyGroupController.SearchStudyGroups(subject) as OkObjectResult;

            //Assert
            _studyGroupServiceMock.Verify(s => s.SearchStudyGroupsAsync(subject), Times.Once);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var returned = result.Value as List<StudyGroup>;

            Assert.That(returned, Is.Not.Null);
            Assert.That(returned.Count, Is.EqualTo(1));
            Assert.That(returned[0].Name, Is.EqualTo("MathGroup2025"));
            Assert.That(returned[0].Subject, Is.EqualTo(Subject.Math));
        }

        [Test]
        public void SearchStudyGroups_ShouldThrow_WhenRequestFails()
        {
            //Arrange
            string subject = "";

            _studyGroupServiceMock.Setup(s => s.SearchStudyGroupsAsync(subject))
                      .ThrowsAsync(new Exception());

            //Act + Assert
            Assert.ThrowsAsync<Exception>(async () =>
                    await _studyGroupController.SearchStudyGroups(subject)
                );

            _studyGroupServiceMock.Verify(s => s.SearchStudyGroupsAsync(subject), Times.Once);
        }

        [Test]
        public async Task JoinStudyGroup_ShouldReturnOkResult_WhenRequestSuccesfull()
        {
            //Arrange
            int studyGroupId = 10;
            int userId = 5;

            //Act
            var result = await _studyGroupController.JoinStudyGroup(studyGroupId, userId);

            //Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
            _studyGroupServiceMock.Verify(s => s.JoinStudyGroupAsync(studyGroupId, userId), Times.Once);
        }

        [Test]
        public void JoinStudyGroup_ShouldThrow_WhenRequestFails()
        {
            //Arrange
            int studyGroupId = 1;
            int userId = 1;

            _studyGroupServiceMock.Setup(s => s.JoinStudyGroupAsync(studyGroupId, userId))
                      .ThrowsAsync(new Exception());

            //Act + Assert
            Assert.ThrowsAsync<Exception>(async () =>
                    await _studyGroupController.JoinStudyGroup(studyGroupId, userId)
                );

            _studyGroupServiceMock.Verify(s => s.JoinStudyGroupAsync(studyGroupId, userId), Times.Once);
        }

        [Test]
        public async Task LeaveStudyGroup_ShouldReturnOkResult_WhenRequestSuccesfull()
        {
            //Arrange
            int studyGroupId = 10;
            int userId = 5;

            //Act
            var result = await _studyGroupController.LeaveStudyGroup(studyGroupId, userId);

            //Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
            _studyGroupServiceMock.Verify(s => s.LeaveStudyGroupAsync(studyGroupId, userId), Times.Once);
        }

        [Test]
        public void LeaveStudyGroup_ShouldThrow_WhenRequestFails()
        {
            //Arrange
            int studyGroupId = 1;
            int userId = 2;

            _studyGroupServiceMock.Setup(s => s.LeaveStudyGroupAsync(studyGroupId, userId))
                      .ThrowsAsync(new Exception());

            //Act + Assert
            Assert.ThrowsAsync<Exception>(async () =>
                    await _studyGroupController.LeaveStudyGroup(studyGroupId, userId)
                );

            _studyGroupServiceMock.Verify(s => s.LeaveStudyGroupAsync(studyGroupId, userId), Times.Once);
        }

    }
}
