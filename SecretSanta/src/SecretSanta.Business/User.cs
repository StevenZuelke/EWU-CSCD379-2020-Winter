using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Business
{
    public class User
    {
        public int Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<Gift> Gifts { get; }

    public User(int id, string fname, string lname, IList<Gift> list)
        {
            this.Id = id;
            this.FirstName = fname;
            this.LastName = lname;
            this.Gifts = list;
        }
        public User(int id, string fname, string lname)
        {
            this.Id = id;
            this.FirstName = fname;
            this.LastName = lname;
            
        }
    }
}
