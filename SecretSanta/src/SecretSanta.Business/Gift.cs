﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Business
{
    public class Gift
    {
        public int Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public User User { get; set; }

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
