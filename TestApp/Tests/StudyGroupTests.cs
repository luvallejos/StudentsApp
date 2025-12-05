using NUnit.Framework;
using StudentsApp;
using System;
using System.Collections.Generic;

namespace TestApp
{
    [TestFixture]
    public class StudyGroupTests
    {
        [Test]
        public void StudyGroup_ShouldThrow_WhenNameIsTooShort()
        {
            string shortName = "Abcd";

            Assert.Throws<ArgumentException>(() =>
                new StudyGroup(shortName, Subject.Math,1, new HashSet<User>())
            );
        }

        [Test]
        public void StudyGroup_ShouldThrow_WhenNameIsTooLarge()
        {
            string LargeName = "Loremipsumdolorsitametconsectet";

            Assert.Throws<ArgumentException>(() =>
                new StudyGroup(LargeName, Subject.Math,1, new HashSet<User>())
            );
        }

        [Test]
        public void StudyGroup_ShouldThrow_WhenSubjectIsInvalid()
        {
            Subject invalidSubject = (Subject)999;

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
            var studyGroup = new StudyGroup(
                "MathGroup2025",
                Subject.Math,
                1,
                new HashSet<User>()
            );

            Assert.That(studyGroup.Name, Is.EqualTo("MathGroup2025"));
            Assert.That(studyGroup.Subject, Is.EqualTo(Subject.Math));
            Assert.That(studyGroup.CreateDate, Is.EqualTo(DateTime.Today));
            Assert.That(0, Is.EqualTo(studyGroup.Users.Count));
        }

        [Test]
        public void StudyGroup_AddUser_ShouldAddUser_WhenUserValid()
        {
            var user = new User(
                "John"
            );

            var studyGroup = new StudyGroup(
               "MathGroup2025",
               Subject.Math,
               1,
               new HashSet<User>()
           );

            studyGroup.AddUser(user);

            Assert.That(studyGroup.Users.Count, Is.EqualTo(1));
        }

        [Test]
        public void StudyGroup_AddUser_ShouldThrow_WhenUserIsNull()
        {
            var studyGroup = new StudyGroup(
               "MathGroup2025",
               Subject.Math,
               1,
               new HashSet<User>()
            );

            Assert.Throws<ArgumentNullException>(() => studyGroup.AddUser(null));
            Assert.That(studyGroup.Users.Count, Is.EqualTo(0));
        }

        [Test]
        public void StudyGroup_AddUser_ShouldThrow_WhenUserAlreadyExists()
        {
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

            Assert.Throws<InvalidOperationException>(() => studyGroup.AddUser(user));
            Assert.That(studyGroup.Users.Count, Is.EqualTo(1));
        }

        [Test]
        public void StudyGroup_RemoveUser_ShouldRemoveUser_WhenUserExists()
        {
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

            studyGroup.RemoveUser(user);

            Assert.That(studyGroup.Users.Count, Is.EqualTo(0));
        }

        [Test]
        public void StudyGroup_RemoveUser_ShouldThrow_WhenUserDoesNotExist()
        {
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

            Assert.Throws<InvalidOperationException>(() => studyGroup.RemoveUser(user));
            Assert.That(studyGroup.Users.Count, Is.EqualTo(0));
        }

        [Test]
        public void StudyGroup_RemoveUser_ShouldThrow_WhenUserIsNull()
        {
            var studyGroup = new StudyGroup(
               "MathGroup2025",
               Subject.Math,
               1,
               new HashSet<User>()
            );

            Assert.Throws<ArgumentNullException>(() => studyGroup.RemoveUser(null));
            Assert.That(studyGroup.Users.Count, Is.EqualTo(0));
        }
    }
}
