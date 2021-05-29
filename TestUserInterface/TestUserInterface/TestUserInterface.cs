using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


namespace TestUserInterface
{
    class TestUserInterface
    {
        // private scope object hold vars 
        Menu Menu;
        Menu SubMenu;
        List<User> Users;


        //This User var oldes the current user 
        User CurrentUser = null;


        // In TestUserInterface we create the objects we need for this menu. 
        public TestUserInterface()
        {
            Menu = new Menu(); // Main Menu
            SubMenu = new Menu(); //Sub Menu
            Users = new List<User>();
        }

        /// <summary> ******************************************************************************************************************
        ///                    AddCustomer              AddCustomer             AddCustomer             AddCustomer
        ///                   
        ///                             AddCustomer is where we add a new Customer too the User DataBase.
        /// </summary> ******************************************************************************************************************

        private void Register()
        {
            var user = GetNewUserInfo();
            if (user != null)
            {
                Users.Add(user);
            }
        }

        private User GetNewUserInfo()
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
            var userEmailExists = Users.Any(u => u.Email == CustomerEmail);

            if (userEmailExists)
            {
                UserInterface.Error("User is already registered");
                return null;
            }

            UserInterface.Message("System - " + CustomerName + " registered successfully");

            return new User() { Name = CustomerName, Email = CustomerEmail, Password = Customerpassword };
        }

        /// <summary> ******************************************************************************************************************
        ///                  LoginCustomer              LoginCustomer             LoginCustomer             LoginCustomer
        ///                  
        ///                                      LoginCustomer is where we login a user
        /// </summary> ******************************************************************************************************************

        private void Login()
        {
            var loginUser = GetUserLogin();
            if (loginUser != null)
            {
                Greetings.UserMenuGreeting(loginUser.Name);
                CurrentUser = loginUser;
            }
        }

        private User GetUserLogin()
        {
            string TestEmail = UserInterface.GetInput("Email");
            string TestPassword = UserInterface.GetPassword("Password");

            //Cross checking the input data with the user data base, output is the user data's position 
            int CheckEmail = Users.FindIndex(ent => ent.Email == TestEmail);

            // if email dose not exits in database
            if (CheckEmail == -1)
            {
                UserInterface.Error("Email or Password wrong");
                return null;
            }

            // does the password match the email
            var user = Users[CheckEmail];
            if (user.Password != TestPassword)
            {
                UserInterface.Error("Email or Password wrong");
                return null;
            }

            //good;
            UserInterface.Message("System - Welcome " + user.Name + " (" + user.Email + ") ");
            return user;
        }

        /// <summary> ******************************************************************************************************************
        ///                 OPTION 1        Register new land                Register new land 
        ///                 
        ///            NewLandRegister and NewHouseRegister is where we add a house or Land too the house database 
        /// </summary> ******************************************************************************************************************

        public void RegisterLand()
        {
            var Land = GetNewLandInfo();
            if (Land != null)
            {
                CurrentUser.Properties.Add(Land);
            }
        }

        private Land GetNewLandInfo()
        {
            //use PropertyAddress of the Get class, too get an input and check it with database
            string PropertyAddress = Get.PropertyAddress(Users);

            if (PropertyAddress == null) // PropertyAddress already exits, then return null
            {
                return null;
            }

            //now use PropertyPostcode of Get too Postcode and check if 4 char long
            string PropertyPostcode = Get.PropertyPostcode();

            if (PropertyPostcode == null) // if it is not 4 long, return null
            {
                return null;
            }

            int Property_Area = UserInterface.GetInteger("Area");

            // print it all out
            UserInterface.Message("System - Land at " + PropertyAddress + ", " + PropertyPostcode + " " + Property_Area + "m² Registered successfully");
            return new Land() { Address = PropertyAddress, Postcode = PropertyPostcode, AreaInSquareMetres = Property_Area, Owner = CurrentUser };
        }

        /// <summary> ******************************************************************************************************************
        ///                 OPTION 2        Register new House                Register new House 
        ///                 
        ///            NewLandRegister and NewHouseRegister is where we add a house or Land too the house database 
        /// </summary> ******************************************************************************************************************

        public void RegisterHouse()
        {
            var House = GetNewHouseInfo();
            if (House != null)
            {
                CurrentUser.Properties.Add(House);
            }
        }
        private House GetNewHouseInfo()
        {
            //use PropertyAddress of the Get class, too get an input and check it with database
            string PropertyAddress = Get.PropertyAddress(Users);

            if (PropertyAddress == null)
            {
                return null;
            }

            //now use PropertyPostcode of Get too Postcode and check if 4 char long
            string PropertyPostcode = Get.PropertyPostcode();

            if (PropertyPostcode == null)
            {
                return null;
            }

            string House_desc = UserInterface.GetInput("Enter description of house (list of rooms etc)");

            UserInterface.Message("system - House at " + PropertyAddress + ", " + PropertyPostcode + " " + House_desc + " Registered successfully");
            return new House() { Address = PropertyAddress, Postcode = PropertyPostcode, Info = House_desc, Owner = CurrentUser };
        }


        /// <summary> ******************************************************************************************************************
        ///                 OPTION 3 -           ListProperties                    ListProperties 
        ///                 
        ///                         ListProperties is where we list all the users current properties     
        /// </summary> ******************************************************************************************************************

        public void ListProperties()
        {
            // call DisplayList, and get it too list Customers Property;
            UserInterface.DisplayList("Your Current Properties For Sale", CurrentUser.Properties);
        }


        /// <summary> ******************************************************************************************************************
        ///                 OPTION 4 -           List bids                    List bids 
        ///                                 ListBids is where we list bids on a Property
        /// </summary> ******************************************************************************************************************

        public void ListBids()
        {
            //call ChooseFromList, where we list all houses the user owns.
            int HouseNum = UserInterface.ChooseFromList(CurrentUser.Properties);

            // use HouseNum too list all the current bids for that Property;
            UserInterface.DisplayList("Bids received: ", CurrentUser.Properties[HouseNum].Bids);

        }


        /// <summary> ******************************************************************************************************************
        ///                 OPTION 5 -           Sell to highest                  Sell too Highest
        ///                 
        ///                        SellHouse is where we sell a house too the highest bidder 
        /// </summary> ******************************************************************************************************************


        public void SellHouse()
        {

            var Bidinfo = SellHouseinfo();
            UserInterface.Message(Bidinfo);
        }

        private string SellHouseinfo()
        {

            //call ChooseFromList, where we list all houses the user owns.
            int HouseNum = UserInterface.ChooseFromList(CurrentUser.Properties);

            if (CurrentUser.Properties[HouseNum].Owner == CurrentUser)
            {
                return "Can't Sell House Too Self";
            }

            //Here we call HighestBid, HighestBid will order the Bidlist in ascending order
            DataCalculation.HighestBid(CurrentUser.Properties[HouseNum].Bids);

            bool countcheck = DataCalculation.CheckCount(CurrentUser.Properties[HouseNum].Bids);

            if (countcheck == false)
            {
                return "Currently no Bids";
            }

            //we grab the first entry in the Bid Database, now that it's in order, This will be the biggest BID.
            int WinNum = CurrentUser.Properties[HouseNum].Bids[0].UserNum;
            int SoldPrice = CurrentUser.Properties[HouseNum].Bids[0].BidPrice;

            // we send the final sold price off to the database (this is for future expandability) 
            CurrentUser.Properties[HouseNum].SalePrice = SoldPrice;

            // call CalculateSalesTax too work out the tax that is payable 
            double TAX = CurrentUser.Properties[HouseNum].CalculateSalesTax();

            // print all the details out
            UserInterface.Message($"System - " + CurrentUser.Properties[HouseNum].ToString() + " SOLD too " + CurrentUser.Name + " (" + CurrentUser.Email + ") FOR $" + SoldPrice + "");

            // once we have all the details in localdata, we then remove the house from the sellers databse
            CurrentUser.Properties.RemoveAt(0);

            return "Tax payable $" + TAX.ToString() + "";
        }

        /// <summary> ******************************************************************************************************************
        ///                                             OPTION 6 - FOR SALE    
        ///                             
        ///                                  Below ListForSale will list the houses at a postcode
        /// </summary> ******************************************************************************************************************
              
        public void ListForSale()
        {
            var Land = Get.PropertyPostcode(); /// PropertyPostcode gets a postcode
            if (Land != null)
            {
                List<ListDisplay> ListMade = DataCalculation.GenerateDisplayList(Land, Users); //GenerateDisplayList makes a list of houses at that postcode

                UserInterface.DisplayList("Current Properties On Market", ListMade); // displays them 
            }
        }

        /// <summary> ******************************************************************************************************************
        ///                                         OPTION 7 - Place a bid   
        ///                             
        ///                                      Will allow us too bid on a place
        /// </summary> ******************************************************************************************************************

        public void BidOnHouse()
        {
            var Land = Get.PropertyPostcode(); /// PropertyPostcode gets a postcode
            if (Land != null)
            {
                List<ListDisplay> ListMade = DataCalculation.GenerateDisplayList(Land, Users); //GenerateDisplayList makes a list of houses at that postcode

                var num = UserInterface.ChooseFromList(ListMade); // choose a house from that list

                if (CurrentUser.Properties[num].Owner == CurrentUser)
                {
                    UserInterface.Message("Can't Sell House Too Self");
                }
                else
                {
                    Bidding.BidProperty(num, ListMade, Users); // BidProperty allows us to place a bid
                }
            }
        }


        /// <summary> ******************************************************************************************************************
        ///               OPTION 8 -            User menu Logout                   User  menu Logout
        ///               
        ///                                                   Logout user
        /// </summary> ******************************************************************************************************************

        public void LogoutCustomer()
        {
            // send message saying user is logged out 
            UserInterface.Message($"System -  " + CurrentUser.Name + " Successfully logged out");
            CurrentUser = null; // by setting CurrentloggedInUser to null, the user is no longer logged in;        
        }


        /// <summary> ******************************************************************************************************************
        ///                  Run program               Run program              Run program              Run program
        /// </summary> ******************************************************************************************************************

        //Here we add our menus too the menu and submenu objects
        public void Run()
        {
            // add main menu
            Menu.Add("Register as new Customer", Register);
            Menu.Add("Login as existing Custiner", Login);

            //add Submenu
            SubMenu.Add("Register new land for sale", RegisterLand);
            SubMenu.Add("Register a new house for sale", RegisterHouse);
            SubMenu.Add("List my properties ", ListProperties);
            SubMenu.Add("List bids received for a property", ListBids);
            SubMenu.Add("Sell one of my properties to highest bidder", SellHouse);
            SubMenu.Add("Search for a property for sale", ListForSale);
            SubMenu.Add("Place a bid on a property", BidOnHouse);
            SubMenu.Add("Logout", LogoutCustomer);

            DisplayMainMenu();
        }

        // Here we control what menu is auto displayed based on the CurrentloggedInUser.  
        public void DisplayMainMenu()
        {
            Greetings.FrontGreeting();

            while (true)
            {
                if (CurrentUser == null)
                {
                    Menu.Display();
                }
                else
                {
                    SubMenu.Display();
                }
            }
        }

        // MAIN RUN FUNCTION!
        static void Main(string[] args)
        {
            var ui = new TestUserInterface();
            ui.Run();
        }
    }
}
