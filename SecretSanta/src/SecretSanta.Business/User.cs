using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Business
{
    class User
    {
        int Id { get; }
        string FirstName;
        string LastName;
        IList<Gift> Gifts = new List<Gift>();

        public User(int id, string fname, string lname)
        {
            this.Id = id;
            this.FirstName = fname;
            this.LastName = lname;
        }
    }
}
