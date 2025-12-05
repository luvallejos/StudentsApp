using System;

namespace StudentsApp
{
    public class User
    {
        #region Properties
        public int Id { get; private set; }
        public string Name { get; set; }
        #endregion

        #region Constructors
        public User(string name) 
        { 
            Name = name;
        }
        #endregion

        #region Methods
        public void SetId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("User id must be a positive number.");

            Id = id;
        }

        public override bool Equals(object obj)
        {
            return obj is User other && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        #endregion
    }
}
