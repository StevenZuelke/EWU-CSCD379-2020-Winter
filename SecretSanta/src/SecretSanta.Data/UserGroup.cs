using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Data
{
    //Class to hold the many to many relationship between User and Group
    public class UserGroup
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public User User { get; set; }
        public Group Group { get; set; }
    }
}
