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
        //List<User> Users;
        Users Users;

        //This User var holds the current user 
        User CurrentUser = null;

        // In TestUserInterface we create the objects we need for this menu. 
        public TestUserInterface()
        {
            Menu = new Menu(); // Main Menu
            SubMenu = new Menu(); //Sub Menu
            Users = new Users();
        }


        /// <summary> ******************************************************************************************************************
        ///                  LoginCustomer              LoginCustomer             LoginCustomer             LoginCustomer
        ///                  
        ///                                      LoginCustomer is where we login a user
        /// </summary> ******************************************************************************************************************

        private void Login()
        {
            string email = UserInterface.GetInput("Email");
            string password = UserInterface.GetPassword("Password");

            var loginUser = Users.Login(email, password);

            if (loginUser == null)
            {
                UserInterface.Error("Email or Password wrong");
                return;
            }

            UserInterface.Message("System - Welcome " + loginUser.Name + " (" + loginUser.Email + ") ");
            Greetings.UserMenuGreeting(loginUser.Name);
            CurrentUser = loginUser;
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
            // Make sure we are getting a new property address
            string PropertyAddress = RealEstateUserInterface.GetNewPropertyAddress(Users.UserList);

            if (PropertyAddress == null) // PropertyAddress already exits, then return null
            {
                return null;
            }

            //now use PropertyPostcode of Get too Postcode and check if 4 char long
            string PropertyPostcode = RealEstateUserInterface.GetPostcode();

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
            string PropertyAddress = RealEstateUserInterface.GetNewPropertyAddress(Users.UserList);

            if (PropertyAddress == null)
            {
                return null;
            }

            //now use PropertyPostcode of Get too Postcode and check if 4 char long
            string PropertyPostcode = RealEstateUserInterface.GetPostcode();

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

        private void ListProperties()
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
            if (CurrentUser.Properties == null || CurrentUser.Properties.Count() == 0)
            {
                UserInterface.Error("You have no properties, therefore no bids have been received.");
                return;
            }
            //call ChooseFromList, where we list all houses the user owns.
            int HouseNum = UserInterface.ChooseFromList(CurrentUser.Properties);

            // use HouseNum too list all the current bids for that Property;
            UserInterface.DisplayList("Bids received: ", CurrentUser.Properties[HouseNum].Bids.BidList);

        }


        /// <summary> ******************************************************************************************************************
        ///                 OPTION 5 -           Sell to highest                  Sell too Highest
        ///                 
        ///                        SellHouse is where we sell a house too the highest bidder 
        /// </summary> ******************************************************************************************************************


        private void SellHouse()
        {

            var Bidinfo = SellHouseinfo();
            UserInterface.Message(Bidinfo);
        }

        private string SellHouseinfo()
        {
            //call ChooseFromList, where we list all houses the user owns.
            int HouseNum = UserInterface.ChooseFromList(CurrentUser.Properties);

            CurrentUser.Properties[HouseNum].Bids.SortBids();

            bool countcheck = DataCalculation.CheckCount(CurrentUser.Properties[HouseNum].Bids.BidList);

            if (countcheck == false)
            {
                return "Currently no Bids";
            }

            //we grab the first entry in the Bid Database, now that it's in order, This will be the biggest BID.
            var highestBid = CurrentUser.Properties[HouseNum].Bids.GetHighestBid();
            var purchaser = highestBid.Owner;
            int soldPrice = highestBid.BidPrice;


            // we send the final sold price off to the database (this is for future expandability) 
            CurrentUser.Properties[HouseNum].SalePrice = soldPrice;

            // call CalculateSalesTax too work out the tax that is payable 
            double TAX = CurrentUser.Properties[HouseNum].CalculateSalesTax();

            // print all the details out
            UserInterface.Message($"System - " + CurrentUser.Properties[HouseNum].ToString() + " SOLD too " + purchaser.Name + " (" + CurrentUser.Email + ") FOR $" + soldPrice + "");

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
            var Land = RealEstateUserInterface.GetPostcode(); /// PropertyPostcode gets a postcode
            if (Land != null)
            {
                List<PropertyViewModel> ListMade = DataCalculation.GenerateDisplayList(Land, Users.UserList); //GenerateDisplayList makes a list of houses at that postcode

                UserInterface.DisplayList("Current Properties On Market", ListMade); // displays them 
            }
        }

        /// <summary> ******************************************************************************************************************
        ///                                         OPTION 7 - Place a bid   
        ///                             
        ///                                      Will allow us too bid on a place
        /// </summary> ******************************************************************************************************************

        public void BidOnProperty()
        {
            var chosenPostcode = RealEstateUserInterface.GetPostcode();
            if (chosenPostcode != null)
            {
                List<PropertyViewModel> chosenProperties = DataCalculation.GenerateDisplayList(chosenPostcode, Users.UserList); //GenerateDisplayList makes a list of houses at that postcode

                if (chosenProperties.Count() == 0)
                {
                    UserInterface.Error("There are no properties at that post code");
                    return;
                }

                PropertyViewModel chosenPropertyViewModel = UserInterface.ChooseItemFromList(chosenProperties);

                if (chosenPropertyViewModel.Owner == CurrentUser)
                {
                    UserInterface.Error("Can't Sell Your Own Property To Yourself");
                }
                else
                {
                    var chosenProperty = chosenPropertyViewModel.Owner.Properties[chosenPropertyViewModel.HouseNum];
                    chosenProperty.PlaceBid();// PlaceBid allows us to place a bid
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
            Menu.Add("Register as new Customer", Users.RegisterUser);
            Menu.Add("Login as existing Custiner", Login);

            //add Submenu
            SubMenu.Add("Register new land for sale", RegisterLand);
            SubMenu.Add("Register a new house for sale", RegisterHouse);
            SubMenu.Add("List my properties ", ListProperties);
            SubMenu.Add("List bids received for a property", ListBids);
            SubMenu.Add("Sell one of my properties to highest bidder", SellHouse);
            SubMenu.Add("Search for a property for sale", ListForSale);
            SubMenu.Add("Place a bid on a property", BidOnProperty);
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
