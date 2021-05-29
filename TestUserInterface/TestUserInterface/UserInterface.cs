using System;
using System.Collections.Generic;
using System.Linq;

//TestUserInterface is the script where we call and run everything from

namespace TestUserInterface
{
    /// <summary> ******************************************************************************************************************
    ///     
    ///                                             Welcome to User_UserInterface.cs
    ///                                                             
    ///                              This File holes methords to do with displaying and inputting data
    ///                              
    ///                                                   For UserInterface
    ///                                                                               
    /// </summary>******************************************************************************************************************

    public class UserInterface // class us used too display infomation / get inputs 
    {
        public static int ChooseFromList<T>(IList<T> list)
        {
            System.Diagnostics.Debug.Assert(list.Count > 0);
            DisplayList("Please choose one of the following:", list);
            int option = UserInterface.GetOption(1, list.Count);
            return option;
        }

        // Given a list -- get user to choose an item (starting at 1...)
        // and return the actual the relevant item (not the number)
        public static T ChooseItemFromList<T>(IList<T> list)
        {
            System.Diagnostics.Debug.Assert(list.Count > 0);
            DisplayList("Please choose one of the following:", list);
            int option = UserInterface.GetOption(1, list.Count);
            return list[option];
        }

        public static void DisplayList<T>(string title, IList<T> list)
        {
            Console.WriteLine(title);
            if (list.Count == 0)
                Console.WriteLine("  None");
            else
                for (int i = 0; i < list.Count; i++)
                    Console.WriteLine("  {0}) {1}", i + 1, list[i].ToString());

            Console.WriteLine();
        }

        public static int GetOption(int min, int max)
        {
            while (true)
            {
                var key = Console.ReadKey(true);
                var option = key.KeyChar - '0';
                if (min <= option && option <= max)
                    return option - 1;
                else
                    UserInterface.Error("Invalid option");
            }
        }

        public static string GetInput(string prompt)
        {
            Console.Write("{0}: ", prompt);
            return Console.ReadLine();
        }


        public static int GetInteger(string prompt)
        {
            while (true)
            {
                var response = UserInterface.GetInput(prompt);
                int integer;
                if (int.TryParse(response, out integer))
                    return integer;
                else
                    Error("Invalid number");
            }
        }

        public static string GetPassword(string prompt)
        {
            Console.Write("{0}: ", prompt);
            var password = new System.Text.StringBuilder();
            while (true)
            {
                var keyInfo = Console.ReadKey(intercept: true);
                var key = keyInfo.Key;

                if (key == ConsoleKey.Enter)
                    break;
                else if (key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        Console.Write("\b \b");
                        password.Remove(password.Length - 1, 1);
                    }
                }
                else
                {
                    Console.Write("*");
                    password.Append(keyInfo.KeyChar);
                }
            }
            Console.WriteLine();
            return password.ToString();
        }

        public static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{msg}, please try again");
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void Message(object msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.WriteLine();
            Console.ResetColor();
        }

        // GetNewUserInfo Get New info for Users
        public static User GetNewUserInfo(Users users)
        {
            var CustomerName = UserInterface.GetInput("Full name");
            var CustomerEmail = UserInterface.GetInput("Email");

            // Here we are checking if the Email has a legit @ in it. 
            bool AtCheck = DataCalculation.CheckFor(CustomerEmail, "@");

            if (!AtCheck)
            {
                UserInterface.Error("Email is missing an @");
                return null;
            }

            var Customerpassword = UserInterface.GetPassword("Password");

            // here we check if there is any other users with the same email 
            var userEmailExists = users.Exists(CustomerEmail);

            if (userEmailExists)
            {
                UserInterface.Error("User is already registered");
                return null;
            }

            UserInterface.Message("System - " + CustomerName + " registered successfully");

            return new User() { Name = CustomerName, Email = CustomerEmail, Password = Customerpassword };
        }
    }


    public class Greetings //Greetings is a class that holes just fun little bits for the interface
    {
        public static void FrontGreeting()
        {
            UserInterface.Message("------------------------------------------------");
            UserInterface.Message("                   WELCOME TO ");
            UserInterface.Message("              VECTOR REALESTATE.COM ");
            UserInterface.Message("          :.:.:.:.:.: ˁ˚ᴥ˚ˀ :.:.:.:.:.:");
            UserInterface.Message("------------------------------------------------");
        }

        public static void UserMenuGreeting(string username)
        {
            UserInterface.Message("------------------------------------------------");
            UserInterface.Message("       CURRENT USER: " + username + "  ");
            UserInterface.Message("------------------------------------------------");
        }
    }
}
