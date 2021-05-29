using System;
using System.Collections.Generic;

//TestUserInterface is the script where we call and run everything from

namespace TestUserInterface
{
    public class UserInterface // class us used too display infomation / get inputs 
    {
        public static int ChooseFromList<T>(IList<T> list)
        {
            System.Diagnostics.Debug.Assert(list.Count > 0);
            DisplayList("Please choose one of the following:", list);
            int option = UserInterface.getOption(1, list.Count);
            return option;
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

        public static int getOption(int min, int max)
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
            Console.WriteLine($"{msg}, please try again");
            Console.WriteLine();
        }

        public static void Message(object msg)
        {
            Console.WriteLine(msg);
            Console.WriteLine();
        }
    }


    // the menu class, used too create and display a list of menu options  
    public class Menu
    {
        class MenuItem
        {
            private string item;
            private Action selected;

            public MenuItem(string item, Action eventHandler)
            {
                this.item = item;
                selected = eventHandler;
            }

            public void select()
            {
                selected();
            }

            public override string ToString()
            {
                return item;
            }
        }

        private List<MenuItem> items = new List<MenuItem>();

        public void Add(string menuItem, Action eventHandler)
        {
            items.Add(new MenuItem(menuItem, eventHandler));
        }

        public void Display()
        {
            UserInterface.DisplayList("Please select one of the following:", items);
            var option = UserInterface.getOption(1, items.Count);
            items[option].select();
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
        public User Owner { get; internal set; }      
        public double SalePrice { get; set; }

        public List<Bid> Bids = new List<Bid>();
        public abstract double CalculateSalesTax();
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
        public int UserNum { get; set; }
        public int BidPrice { get; set; }
    }

    //ListDisplay is a simple class we call, when we genrate a display list for serching 
    public class ListDisplay
    {
        public int UserNum { get; set; }
        public int HouseNum { get; set; }
        public string Houseinfo { get; set; }

        public override string ToString() // The ToString override allows us too return Property details in the DisplayList function. 
        {
            return Houseinfo;
        }
    }


    public class DataCalculation // the DataCalculation Class, is how any functions needed for Calculations needed to run the program 
    {
        // A little function too check the length of an string. then return a bool 
        public static bool CheckLength(string input, int length)
        {
            bool IsOver = false;

            if ((input.Length) == length)
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

        //A little function too sort a list in ascending order
        public static void HighestBid(List<Bid> BidList)
        {
            BidList.Sort((x, y) => y.BidPrice.CompareTo(x.BidPrice));

        }

        /// This Function Genrates a new LIST of all the houses in the registry based on a postcode
        public static List<ListDisplay> GenerateDisplayList(string PostCode, List<User> UserList)
        {
            List<ListDisplay> NewDisplayList = new List<ListDisplay>(); //the new list of houses 

            for (int i = 0; i < UserList.Count; i++) // for loop runs through all registered users   
            {
                for (int ii = 0; ii < UserList[i].Properties.Count; ii++) // for loop runs through all Properties  
                {
                    if (UserList[i].Properties[ii].Postcode == PostCode) // check if the current propertie has the wanted postcode 
                    {
                        //if true, then add the needed info too a new object called SaveList, and add that too the NewDisplayList
                        NewDisplayList.Add(new ListDisplay { UserNum = i, HouseNum = ii, Houseinfo = UserList[i].Properties[ii].ToString()});
                    }
                }
            }
            return NewDisplayList;
        }

    }

    // the bidding class, is for Calculations based around bidding 
    public class Bidding : DataCalculation
    {
        public static void BidProperty(int ChoosenProperties, List<ListDisplay> DisplayList, List<User> userList)
        {
            int UserNum = DisplayList[ChoosenProperties].UserNum;
            int HouseNum = DisplayList[ChoosenProperties].HouseNum;

            int BidNum = Get.PropertyBid(userList[UserNum].Properties[HouseNum].Bids);

            if(BidNum >= 0)
            {
                userList[UserNum].Properties[HouseNum].Bids.Add(new Bid { UserNum = UserNum, BidPrice = BidNum });
                UserInterface.Message($"System -  Bid of $" + BidNum + " for " + userList[UserNum].Properties[HouseNum].ToString() + " Successfully Placed");
            }
              
        }
    }


    // the Get class, is for anything where we need to Get data 
    public class Get : DataCalculation
    {
        public static string PropertyAddress(List<User> User) // get PropertyAddress
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

        public static string PropertyPostcode() // get Postcode
        {
            string Property_Postcode = UserInterface.GetInput("Postcode");

            bool PostcodeLegth = CheckLength(Property_Postcode, 4);

            if (PostcodeLegth == false)
            {
                UserInterface.Error("PostCode is not 4 characters long");
                return null;
            }

            return Property_Postcode;
        }

        public static int PropertyBid(List<Bid> Bidlist) // Get Bids
        {
            DataCalculation.HighestBid(Bidlist);

            string textprint;
            int vaule = 0;

            if (Bidlist.Count == 0)
            {
                textprint = "System - There are no bids on this Property, What do you place?";
                vaule = 0;
            }
            else
            {
                textprint = "System - Cuurent Bid is $" + Bidlist[0].BidPrice + ", What do you place?";
                vaule = Bidlist[0].BidPrice;
            }

            int Property_Bid = UserInterface.GetInteger(textprint);

            if (Property_Bid <= vaule)
            {
                UserInterface.Error("Bid too low");
                return -1;
            }

            return Property_Bid;
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


