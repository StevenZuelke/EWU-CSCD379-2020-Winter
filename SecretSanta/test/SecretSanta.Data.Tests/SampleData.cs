using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SecretSanta.Data;

namespace SecretSanta.Data.Tests
{
    static public class SampleData
    {
        //Create 2 Users
        //User InigoMontoya
        public const string Inigo = "Inigo";
        public const string Montoya = "Montoya";
        //User PrincessButtercup
        public const string Princess = "Princess";
        public const string Buttercup = "Buttercup";
        //Methods to Create Users
        static public User CreateInigoMontoya() => new User(Inigo, Montoya);
        static public User CreatePrincessButtercup() => new User(Princess, Buttercup);
        //Create 2 Gifts
        //Gift 1
        public const string Title1 = "Title One";
        public const string Desc1 = "Description One";
        public const string Url1 = "Url One";
        //Gift 2
        public const string Title2 = "Title Two";
        public const string Desc2 = "Description Two";
        public const string Url2 = "Url Two";
        //Methods to Create Gifts
        static public Gift CreateGift1() => new Gift(Title1, Desc1, Url1, CreateInigoMontoya());
        static public Gift CreateGift2() => new Gift(Title2, Desc2, Url2, CreatePrincessButtercup());
        //Create 2 Groups
        //Group 1
        public const string GroupTitle1 = "Title 1";
        //Group 2
        public const string GroupTitle2 = "Title 2";
        //Methods to Create Groups
        static public Group CreateGroup1() => new Group(GroupTitle1);
        static public Group CreateGroup2() => new Group(GroupTitle2);
    }
}