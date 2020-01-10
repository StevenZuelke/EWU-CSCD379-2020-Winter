using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Business
{
    public class Gift
    {
        public int Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        //This url should be a string i know better
#pragma warning disable CA1056 // Uri properties should not be strings
        public string Url { get; set; }
#pragma warning restore CA1056 // Uri properties should not be strings
        public User User { get; set; }

#pragma warning disable CA1054 // Uri parameters should not be strings ### URL is not uri
        public Gift(int id, string title, string description, string url, User user)
#pragma warning restore CA1054 // Uri parameters should not be strings
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.Url = url;
            this.User = user;
        }
    }
}
