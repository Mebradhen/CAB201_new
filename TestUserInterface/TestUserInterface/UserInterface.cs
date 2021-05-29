using System;
using System.Collections.Generic;
using System.Linq;

//TestUserInterface is the script where we call and run everything from

namespace TestUserInterface
{
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


    /// <summary>
    /// UserInterface methods that are useful for the Real Estate Industry
    /// </summary>
    public class RealEstateUserInterface
    {
        public static string GetPostcode()
        {
            string Property_Postcode = UserInterface.GetInput("Postcode");

            bool PostcodeLegth = DataCalculation.CheckLength(Property_Postcode, 4);

            if (PostcodeLegth == false)
            {
                UserInterface.Error("PostCode is not 4 characters long");
                return null;
            }

            return Property_Postcode;
        }

        public static int GetPropertyBid(Bids bids) // Get Bids
        {
            string textprint;

            int highestBidPrice = bids.GetHighestBidPrice();

            if (bids.Count == 0)
            {
                textprint = "System - There are no bids on this Property, What do you place?";
            }
            else
            {
                textprint = "System - Current Highest Bid is $" + highestBidPrice + ", What do you place?";
            }

            int Property_Bid = UserInterface.GetInteger(textprint);

            if (Property_Bid <= highestBidPrice)
            {
                UserInterface.Error("Bid too low");
                return -1;
            }

            return Property_Bid;
        }

        public static string GetNewPropertyAddress(List<User> User)
        {
            string Property_Address = UserInterface.GetInput("Address");

            int Checkhouse = -1;

            for (int i = 0; i < User.Count; i++) // for loop runs through all registered users
            {
                Checkhouse = User[i].Properties.FindIndex(ent => ent.Address == Property_Address);
            }

            if (Checkhouse != -1)
            {
                UserInterface.Error("Property is already Registered");
                return null;
            }
            return Property_Address;
        }
    }


   

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



    //User class is for storing user info
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<Property> Properties = new List<Property>();
    }




    //Property class is for storing info on Properties 
    public abstract class Property
    {
        public string Address { get; set; }
        public string Postcode { get; set; }
        public User Owner { get; set; }
        public double SalePrice { get; set; }

        public Bids Bids = new Bids();
        public abstract double CalculateSalesTax();

        public void PlaceBid()
        {
            User owner = Owner;

            double BidNum = RealEstateUserInterface.GetPropertyBid(Bids);

            BidNum = Math.Round(BidNum);

            if (BidNum >= 0)
            {
                Bids.Add(new Bid { Owner = owner, BidPrice = (int)BidNum });
                UserInterface.Message($"System -  Bid of $" + (int)BidNum + " for " + this.ToString() + " Successfully Placed");
            }
        }
    }

    public class Land : Property // using polymorphism and inheritance, the land class is an child of Property. Storing info for land
    {
        private const double LAND_TAX_PER_SQUARE_METRE = 5.50;
        public double AreaInSquareMetres { get; set; }
        public override double CalculateSalesTax()
        {
            return AreaInSquareMetres * LAND_TAX_PER_SQUARE_METRE;
        }

        public override string ToString() // The ToString override allows us too return Property details in the DisplayList function. 
        {
            return $" Land at - {Address}, {Postcode}, with: {AreaInSquareMetres} m²";
        }
    }

    public class House : Property// using polymorphism and inheritance, the House class is an child of Property. Storing info for House
    {
        private const double SALES_TAX_ON_HOUSING = 0.10f;

        public string Info { get; set; }

        public override double CalculateSalesTax()
        {
            return this.SalePrice * SALES_TAX_ON_HOUSING;
        }

        public override string ToString() // The ToString override allows us too return Property details in the DisplayList function. 
        {
            return " A House at - " + Address + ", " + Postcode + ", with: " + Info + " ";
        }
    }




    // NewBidRegister is yet another class, where other scripts can call it to make an object that stores a Properties bids
    public class Bid
    {
        public User Owner { get; set; }
        public int BidPrice { get; set; }
        public override string ToString()
        {
            return "$" + BidPrice.ToString();
        }
    }

    public class Bids
    {
        public List<Bid> BidList { get; private set; } = new List<Bid>();

        public int Count { get { return BidList.Count; } }

        public void SortBids()
        {
            BidList.Sort((x, y) => y.BidPrice.CompareTo(x.BidPrice));
        }

        public int GetHighestBidPrice()
        {
            return GetHighestBid()?.BidPrice ?? 0;
        }
        public Bid GetHighestBid()
        {
            if (BidList.Count == 0) return null;

            SortBids();

            return BidList[0];
        }

        public void Add(Bid bid)
        {
            BidList.Add(bid);
        }
    }

    //ListDisplay is a simple class we call, when we genrate a display list for serching 
    public class PropertyViewModel
    {
        public User Owner { get; set; }
        public int HouseNum { get; set; }
        public string Houseinfo { get; set; }

        public override string ToString() // The ToString override allows us too return Property details in the DisplayList function. 
        {
            return Houseinfo;
        }
    }


    public class DataCalculation // the DataCalculation Class, is how any functions needed for Calculations needed to run the program 
    {
        // A little function to check the length of an string. then return a bool 
        public static bool CheckLength(string input, int length)
        {
            return input.Length == length;
        }


        public static bool CheckCount<T>(IList<T> input)
        {
            bool IsOver = false;

            if ((input.Count) > 0)
            {
                IsOver = true;
            }

            return IsOver;
        }

        // A little function too check FOR something in a string. then return a bool 
        public static bool CheckFor(string input, string Char)
        {
            bool IsOver = false;

            if (input.IndexOf(Char) >= 0)
            {
                IsOver = true;
            }

            return IsOver;
        }


        /// This Function Genrates a new LIST of all the houses in the registry based on a postcode
        public static List<PropertyViewModel> GenerateDisplayList(string PostCode, List<User> UserList)
        {
            List<PropertyViewModel> NewDisplayList = new List<PropertyViewModel>(); //the new list of houses 

            for (int i = 0; i < UserList.Count; i++) // for loop runs through all registered users   
            {
                for (int ii = 0; ii < UserList[i].Properties.Count; ii++) // for loop runs through all Properties
                {
                    if (UserList[i].Properties[ii].Postcode == PostCode) // check if the current property has the wanted postcode
                    {
                        //if true, then add the needed info to a new object called SaveList, and add that to the NewDisplayList
                        NewDisplayList.Add(new PropertyViewModel { Owner = UserList[i].Properties[ii].Owner, HouseNum = ii, Houseinfo = UserList[i].Properties[ii].ToString() });
                    }
                }
            }
            return NewDisplayList;
        }

    }

    public class Greetings //IconsNfun is a class that holes just fun little bits for the interface
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
