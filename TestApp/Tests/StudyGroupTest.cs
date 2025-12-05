using NUnit.Framework;
using StudentsApp;
using System;
using System.Collections.Generic;

namespace TestApp
{
    [TestFixture]
    public class StudyGroupTest
    {
        [Test]
        public void StudyGroup_ShouldThrow_WhenNameIsTooShort()
        {
            //Arrange
            string shortName = "Abcd";

            //Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new StudyGroup(shortName, Subject.Math,1, new HashSet<User>())
            );
        }

        [Test]
        public void StudyGroup_ShouldThrow_WhenNameIsTooLarge()
        {
            //Arrange
            string LargeName = "Loremipsumdolorsitametconsectet";

            //Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new StudyGroup(LargeName, Subject.Math,1, new HashSet<User>())
            );
        }

        [Test]
        public void StudyGroup_ShouldThrow_WhenSubjectIsInvalid()
        {
            //Arrange
            Subject invalidSubject = (Subject)999;

            //Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new StudyGroup(
                    "GroupName",
                    invalidSubject,
                    1,
                    new HashSet<User>()
                )
            );
        }

        [Test]
        public void StudyGroup_ShouldCreateStudyGroup_WhenDataIsValid()
        {
            //Arrange & Act
            var studyGroup = new StudyGroup(
                "MathGroup2025",
                Subject.Math,
                1,
                new HashSet<User>()
            );

            //Assert
            Assert.That(studyGroup.Name, Is.EqualTo("MathGroup2025"));
            Assert.That(studyGroup.Subject, Is.EqualTo(Subject.Math));
            Assert.That(studyGroup.CreateDate, Is.EqualTo(DateTime.Today));
            Assert.That(0, Is.EqualTo(studyGroup.Users.Count));
        }

        [Test]
        public void StudyGroup_AddUser_ShouldAddUser_WhenUserValid()
        {
            //Arrange
            var user = new User(
                "John"
            );

            var studyGroup = new StudyGroup(
               "MathGroup2025",
               Subject.Math,
               1,
               new HashSet<User>()
           );

            //Act
            studyGroup.AddUser(user);

            //Assert
            Assert.That(studyGroup.Users.Count, Is.EqualTo(1));
        }

        [Test]
        public void StudyGroup_AddUser_ShouldThrow_WhenUserIsNull()
        {
            //Arrange
            var studyGroup = new StudyGroup(
               "MathGroup2025",
               Subject.Math,
               1,
               new HashSet<User>()
            );

            //Act & Assert
            Assert.Throws<ArgumentNullException>(() => studyGroup.AddUser(null));
            Assert.That(studyGroup.Users.Count, Is.EqualTo(0));
        }

        [Test]
        public void StudyGroup_AddUser_ShouldThrow_WhenUserAlreadyExists()
        {
            //Arrange
            var user = new User(
                "John"
            );

            user.SetId(1);

            var studyGroup = new StudyGroup(
              "MathGroup2025",
              Subject.Math,
              1,
              new HashSet<User>() {user}
            );

            //Act & Assert
            Assert.Throws<InvalidOperationException>(() => studyGroup.AddUser(user));
            Assert.That(studyGroup.Users.Count, Is.EqualTo(1));
        }

        [Test]
        public void StudyGroup_RemoveUser_ShouldRemoveUser_WhenUserExists()
        {
            //Arrange
            var user = new User(
                "John"
            );

            user.SetId(1);

            var studyGroup = new StudyGroup(
               "MathGroup2025",
               Subject.Math,
               1,
               new HashSet<User>() {user}
            );

            //Act
            studyGroup.RemoveUser(user);

            //Assert
            Assert.That(studyGroup.Users.Count, Is.EqualTo(0));
        }

        [Test]
        public void StudyGroup_RemoveUser_ShouldThrow_WhenUserDoesNotExist()
        {
            //Arrange
            var user = new User(
                "John"
            );

            var studyGroup = new StudyGroup(
               "MathGroup2025",
               Subject.Math,
               1,
               new HashSet<User>()
            );

            user.SetId(1);

            //Act & Assert
            Assert.Throws<InvalidOperationException>(() => studyGroup.RemoveUser(user));
            Assert.That(studyGroup.Users.Count, Is.EqualTo(0));
        }

        [Test]
        public void StudyGroup_RemoveUser_ShouldThrow_WhenUserIsNull()
        {
            //Arrange
            var studyGroup = new StudyGroup(
               "MathGroup2025",
               Subject.Math,
               1,
               new HashSet<User>()
            );

            //Act & Assert
            Assert.Throws<ArgumentNullException>(() => studyGroup.RemoveUser(null));
            Assert.That(studyGroup.Users.Count, Is.EqualTo(0));
        }
    }
}
