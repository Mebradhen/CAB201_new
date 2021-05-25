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



    //NewUserRegister class, This is our class where we can create list objects. The aim being to store user details.
    public class User
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }

        public List<Property> Properties = new List<Property>();
    }


    //Property Register, This class here is where we can create the Property Objects, too save Property Details
    public class Property
    {         
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Details { get; set; }

        public List<Bid> Bids = new List<Bid>();
    }

    public class House : Property
    {
        public string Info { get; set; }

    }

    public class Land : Property
    {
        public string Info { get; set; }

    }



    // NewBidRegister is yet another class, where other scripts can call it to make an object that stores a Properties bids
    public class Bid
    { 
          public int UserNum { get; set; }
          public int BidPrice { get; set; }
    }

   
    public class DataCalculation // the DataCalculation Class, is how any functions needed for Calculations needed to run the program 
    {

        // This function Is used too Display a Property
        public static void DisplayProperty(string PostCode, string Titl, NewUserRegister consumer)
        {
            List<SaveList> NewDisplayList = GenerateProperty(PostCode, consumer);
            
            UserInterface.DisplayList(Titl, NewDisplayList);               
        }

        // This function Is used too Bid on a Property, BidProperty fields has the postcode and consumer list 
        public static void BidProperty(string PostCode, NewUserRegister consumer)
        {
            // we call the GenerateProperty function, this function grabs all the properties from all registered consumers based on an input postcode   
            List<SaveList> NewDisplayList = GenerateProperty(PostCode, consumer);

            // just a string too hold message strings
            string Message_bid;

            // send the new NewDisplayList off too ChooseFromList, the selected house returns a int
            int ChoosenHouse = UserInterface.ChooseFromList(NewDisplayList);

            // get stored house num based on ChoosenHouse int  
            int UserNum = NewDisplayList[ChoosenHouse].UserNum;
            int HouseNum = NewDisplayList[ChoosenHouse].HouseNum;

            //call HighestBid too sort the Bid list into ascending order
            HighestBid(consumer.NewUser[UserNum].NewProperty[HouseNum].NewBid);

            //check the bid list has any entries 
            if (consumer.NewUser[UserNum].NewProperty[HouseNum].NewBid.Count <= 0) // if not throw an error 
            {
                Message_bid = "No Bids: Enter Starting Bid";
            }
            else // if yes, display the current highest bid 
            {
                Message_bid = "Current Highest Bid is $" + consumer.NewUser[UserNum].NewProperty[HouseNum].NewBid[0] + " - What will you place?";
            }

            UserInterface.Message(Message_bid); // display Message_bid

            int bid_amount = int.Parse(UserInterface.GetInput("Enter Bid ($)"));

            UserInterface.Message("Your Bid of $" + bid_amount + " Has been Placed");

            // add the Bid too the Property bid list 
            consumer.NewUser[UserNum].NewProperty[HouseNum].AddBid(UserNum, consumer, bid_amount);
        }
          
        /// This Function Genrates a new LIST of all the houses in the registry based on a postcode
        public static List<SaveList> GenerateProperty(string PostCode, NewUserRegister consumer)
        {
            List<SaveList> NewDisplayList = new List<SaveList>(); //the new list of houses 

            for (int i = 0; i < consumer.NewUser.Count; i++) // for loop runs through all registered users   
            {
                for (int ii = 0; ii < consumer.NewUser[i].NewProperty.Count; ii++) // for loop runs through all Properties  
                {
                    if (consumer.NewUser[i].NewProperty[ii].PropertyPostcode == PostCode) // check if the current propertie has the wanted postcode 
                    {
                        //if true, then add the needed info too a new object called SaveList, and add that too the NewDisplayList
                        NewDisplayList.Add(new SaveList(consumer.NewUser[i].NewProperty[ii].ToString(), i,ii)); 
                    }
                }
            }

            return NewDisplayList;
        }

        // class too store text and user and house position for listing all houses at a postcode. 
        public class SaveList
        {
            public string HouseDetails;
            public int UserNum;
            public int HouseNum;

            public SaveList(string GetText, int userNumber, int HouseNumber)
            {
                this.HouseDetails = GetText;
                this.UserNum = userNumber;
                this.HouseNum = HouseNumber;
            }
            public override string ToString()
            {
                return HouseDetails;
            }
        }

        // this function is where we work out the TAX for the sale of properties 
        public static double SaleTax(double SALEPrice, int Type, String Info)
        {
            double tax = 0.0f;

            if(Type == 0)
            {
                tax = double.Parse(Info) * 5.50;
            }
            else
            {
                tax = (0.10 * SALEPrice);
            }

            tax = Math.Round(tax);

            return tax;
        }

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

        // A little function too sort a list in ascending order
        public static void HighestBid(List<NewBidRegister.BidInfo> BidList)
        {
            BidList.Sort((x, y) => y.BidPrice.CompareTo(x.BidPrice));

        }
    }

    public class IconsNfun //IconsNfun is a class that holes just fun little bits for the interface
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


