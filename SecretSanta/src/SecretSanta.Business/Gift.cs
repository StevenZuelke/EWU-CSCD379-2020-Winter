using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Business
{
    class Gift
    {
        int Id { get; }
        string Title;
        string Description;
        string Url;
        User User;

        public Gift(int id, string title, string description, string url, User user)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.Url = url;
            this.User = user;
        }
    }
}
