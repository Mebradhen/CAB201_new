using System;
using System.Collections.Generic;
using System.Linq;

namespace TestUserInterface
{
    /// <summary> ******************************************************************************************************************
    ///     
    ///                                             Welcome to User_UserInterface.cs
    ///                                                             
    ///                                   This File holes majority of classes / methords too do with Users
    ///                                                                               
    /// </summary>******************************************************************************************************************


    //User class is for storing user info
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<Property> Properties = new List<Property>();

    }


    // The bids class holes all our methords for User 
    public class Users
    {
        public List<User> UserList { get; private set; } = new List<User>();

        public bool Exists(string customerEmail)
        {
            return UserList.Any(u => u.Email == customerEmail);
        }

        internal void RegisterUser()
        {
            var user = UserInterface.GetNewUserInfo(this);
            if (user != null)
            {
                UserList.Add(user);
            }
        }

        public User Login(string email, string password)
        {

            //Cross checking the input data with the user data base, output is the user data's position 
            int CheckEmail = UserList.FindIndex(ent => ent.Email == email);

            // if email dose not exits in database
            if (CheckEmail == -1)
            {
                return null;
            }

            // does the password match the email
            var user = UserList[CheckEmail];
            if (user.Password != password)
            {
                return null;
            }

            //good;
            return user;
        }
    }

}