using StudentsApp;
using System;
using System.Collections.Generic;

namespace TestApp
{
    public class StudyGroup
    {
        #region Private Fields
        private string _name;
        private Subject _subject;
        #endregion

        #region Constructors
        public StudyGroup(string name, Subject subject, int ownerUserId, HashSet<User> users) : this(name, subject, ownerUserId, users, DateTime.Today)
        {
        }

        public StudyGroup(string name, Subject subject, int ownerUserId, HashSet<User> users, DateTime createDate)
        {
            Name = name;
            Subject = subject;
            OwnerUserId = ownerUserId;
            Users = users;
            CreateDate = createDate;
        }
        #endregion

        #region Properties
        public int StudyGroupId { get; private set; }   

        public string Name {
            get => _name;
            set 
            {
                if (value.Length < 5 || value.Length > 30)
                    throw new ArgumentException("Name does not comply with accepted numbers of characteres");
                _name = value;
            } }

        public Subject Subject {
            get => _subject;
            set {
                    if(!Enum.IsDefined(typeof(Subject), value))
                        throw new ArgumentException("Subject is not valid");
                    _subject = value;
            } 
        }

        public DateTime CreateDate { get; private set;}

        public int OwnerUserId { get; private set; }

        public HashSet<User> Users { get; private set; }
        #endregion

        #region Methods

        public void SetId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("StudyGroupId must be a positive number.");

            StudyGroupId = id;
        }

        public void AddUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!Users.Add(user))
                throw new InvalidOperationException("User already exist in the group.");
        }

        public void RemoveUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!Users.Contains(user))
                throw new InvalidOperationException("User does not exist in the group.");

            Users.Remove(user);
        }
        #endregion
    }

}
