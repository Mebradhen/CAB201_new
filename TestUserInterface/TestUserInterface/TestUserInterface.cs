using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


namespace TestUserInterface
{
    /// <summary> ******************************************************************************************************************
    ///     
    ///                                             Welcome to TestUserInterface.cs
    ///                                                             
    ///                              This File holes is the Master Mail where everything is called from!!!
    ///                              
    ///                                                        ▐███████▌
    ///                                                        ▐░▀░▀░▀░▌    
    ///                                                        ▐▄▄▄▄▄▄▄▌
    ///                                                  ▄▀▀▀█▒▐░▀▀▄▀▀░▌▒█▀▀▀▄
    ///                                                  ▌▌▌▌▐▒▄▌░▄▄▄░▐▄▒▌▐▐▐▐
    /// </summary>******************************************************************************************************************

    class TestUserInterface
    {
        // private scope object hold vars 
        Menu_UserInterface Menu;
        Menu_UserInterface SubMenu;
        Users Users;

        //This User var holds the current user 
        User CurrentUser = null;

        // In TestUserInterface we create the objects we need for this menu. 
        private TestUserInterface()
        {
            Menu = new Menu_UserInterface(); // Main Menu
            SubMenu = new Menu_UserInterface(); //Sub Menu
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

        private void RegisterLand()
        {
            var Land = RealEstateUserInterface.GetNewLandInfo(CurrentUser , Users);
            if (Land != null)
            {
                CurrentUser.Properties.Add(Land);
            }
        }



        /// <summary> ******************************************************************************************************************
        ///                 OPTION 2        Register new House                Register new House 
        ///                 
        ///            NewLandRegister and NewHouseRegister is where we add a house or Land too the house database 
        /// </summary> ******************************************************************************************************************

        private void RegisterHouse()
        {
            var House = RealEstateUserInterface.GetNewHouseInfo(CurrentUser, Users);
            if (House != null)
            {
                CurrentUser.Properties.Add(House);
            }
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

        private void ListBids()
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
            if (CurrentUser.Properties == null || CurrentUser.Properties.Count() == 0)
            {
                UserInterface.Error("You have no properties, therefore no bids have been received.");
                return;
            }

            RealEstateUserInterface.SellHouseinfo(CurrentUser);
        }

        /// <summary> ******************************************************************************************************************
        ///                                             OPTION 6 - FOR SALE    
        ///                             
        ///                                  Below ListForSale will list the houses at a postcode
        /// </summary> ******************************************************************************************************************

        private void ListForSale()
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

        private void BidOnProperty()
        {
            var chosenPostcode = RealEstateUserInterface.GetPostcode();

            if (chosenPostcode == null)
            {
                return;
            }

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
                chosenProperty.PlaceBid(CurrentUser);// PlaceBid allows us to place a bid
            }

        }


        /// <summary> ******************************************************************************************************************
        ///               OPTION 8 -            User menu Logout                   User  menu Logout
        ///               
        ///                                                   Logout user
        /// </summary> ******************************************************************************************************************

        private void LogoutCustomer()
        {
            // send message saying user is logged out 
            UserInterface.Message($"System -  " + CurrentUser.Name + " Successfully logged out");
            CurrentUser = null; // by setting CurrentloggedInUser to null, the user is no longer logged in;        
        }


        /// <summary> ******************************************************************************************************************
        ///                  Run program               Run program              Run program              Run program
        /// </summary> ******************************************************************************************************************

        //Here we add our menus too the menu and submenu objects
        private void Run()
        {

            // add main menu
            Menu.Add("Register as new Customer", Users.RegisterUser);
            Menu.Add("Login as existing Customer", Login);

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
        private void DisplayMainMenu()
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
            TestUserInterface ui = new TestUserInterface();
            ui.Run();
        }
    }
}
