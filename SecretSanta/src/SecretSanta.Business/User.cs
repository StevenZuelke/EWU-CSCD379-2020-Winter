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
#pragma warning disable INTL0001 // Fields _PascalCase ### current Naming is fine
#pragma warning disable CA1051 // Do not declare visible instance fields ###can be public
        public IList<Gift> Gifts = new List<Gift>();
#pragma warning restore CA1051 // Do not declare visible instance fields
#pragma warning restore INTL0001 // Fields _PascalCase

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
