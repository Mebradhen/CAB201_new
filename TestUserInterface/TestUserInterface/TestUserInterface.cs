using System;
using System.Collections.Generic;
using System.Collections;


namespace TestUserInterface
{
    class TestUserInterface
    {
        // public scope object hold vars 
        Menu Menu;
        Menu SubMenu;

        List<User> Users;

        //This Global Score int, is how we check if someone is logged in or not 
        int CurrentloggedInUser = -2;

        // A simple string var we throw console messages into
        string Message_text = "";

       

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
    private void AddCustomer()
        {
            // Use GetInput too get all the information we need add a new user. 

            var CustomerName = UserInterface.GetInput("Full name");
            var CustomerEmail = UserInterface.GetInput("Email");
            var Customerpassword = UserInterface.GetPassword("Password");

            // we send off the Customer Email too the CheckUserDataBase function, as too see if the email is new or old.
            // Returns an Int of current users position in database. Otherwise = -1; 

            // int CheckEmail = Customer.CheckUserDataBase(CustomerEmail,1);

            int CheckEmail = 1;

            // CheckFor for a checker function, it can be customised too search for anything in a string in the second input.
            // Here we are checking if the Email has a legit @ in it. 

            bool @Check = DataCalculation.CheckFor(CustomerEmail, "@");

            //if We have an @
            if (@Check == true)
            {
                // If the email is new
                if (CheckEmail == -1)
                {
                    //then simply add the new user too the Database object. 
                    Users.Add(CustomerName, CustomerEmail, Customerpassword);
                    Message_text = "System - " + CustomerName + " registered successfully";
                }
                else // otherwise throw an error 
                {
                    Message_text = "System - User is already registered";
                }
            }
            else // otherwise throw an error 
            {
                Message_text = "System - Email is missing an @";
            }
        }

        /// <summary> ******************************************************************************************************************
        ///                  LoginCustomer              LoginCustomer             LoginCustomer             LoginCustomer
        ///                  
        ///                                      LoginCustomer is where we login a user
        /// </summary> ******************************************************************************************************************

        private void LoginCustomer()
        {
            //one again we use GetInput too get an input for email and password. 
            string TestEmail = UserInterface.GetInput("Email");
            string TestPassword = UserInterface.GetPassword("Password");

            //Cross checking the input data with the user data base, output is the user data's position 
            int CheckEmail = Customer.CheckUserDataBase(TestEmail,1);
            int CheckPassword = Customer.CheckUserDataBase(TestPassword,2);

            // if email exits and password exits
            if (CheckEmail > -1 && CheckPassword > -1) // then login the user 
            {
                CurrentloggedInUser = CheckEmail;
            
                Message_text = "System - Welcome " + Customer.NewUser[CurrentloggedInUser].UserName + " (" + Customer.NewUser[CurrentloggedInUser].UserEmail + ") ";
            }
            else // otherwise, throw and error 
            {
                Message_text = "System - Error Try Email or Password again";
            }
        }

        /// <summary> ******************************************************************************************************************
        ///                 OPTION 1 and 2 -           Register new land / House                   Register new land / House
        ///                 
        ///            NewLandRegister and NewHouseRegister is where we add a house or Land too the house database 
        /// </summary> ******************************************************************************************************************

        public void NewLandRegister()
        {
            // Again use GetInput to get the Address, Postcode and area for a new property  

            string House_Address = UserInterface.GetInput("Address");
            string House_Postcode = UserInterface.GetInput("Postcode");
            string House_Area = UserInterface.GetInput("Area");

            // call the RegisterHouseArea and input the above strings, Set the forth input too 0
            RegisterHouseArea(House_Address, House_Postcode, House_Area, 0);
        }

        public void NewHouseRegister()
        {
            // Again use GetInput to get the Address, Postcode and House_desc for a new property  

            string House_Address = UserInterface.GetInput("Address");
            string House_Postcode = UserInterface.GetInput("Postcode");
            string House_desc = UserInterface.GetInput("Enter description of house (list of rooms etc)");

            // call the RegisterHouseArea and input the above strings Set the forth input too 1
            RegisterHouseArea(House_Address, House_Postcode, House_desc, 1);
        }

        // RegisterHouseArea helps to reduce duplicate code, given both use this code. 
        public void RegisterHouseArea(string HAddress, string HPostcode, string HInfo , int HType )
        {
            // Here we use CheckPropertyDataBase too check if the house is new or already for sale;
            int Checkhouse = Customer.NewUser[CurrentloggedInUser].CheckPropertyDataBase(HAddress);

            // We then use CheckLength again too see how long the HPostcode var is.
            bool CheckPostcode = DataCalculation.CheckLength(HPostcode, 4);

            if (CheckPostcode == true) // IF Postcode is not over 4
            {
                if (Checkhouse == -1) // house is new 
                {
                    // then add Property too Users House objects DataBase
                    Customer.NewUser[CurrentloggedInUser].AddProperty(HAddress, HPostcode, HInfo, HType);
                    Message_text = "System - " + HAddress + ", " + HPostcode + " " + HInfo + " Registered successfully";
                }
                else // Throw error 
                {
                    Message_text = "System - ERROR: Property is already Registered";
                }
            }
            else // Throw error 
            {
                Message_text = "System - ERROR: PostCode is not 4 characters long ";
            }
        }

        /// <summary> ******************************************************************************************************************
        ///                 OPTION 3 -           ListProperties                    ListProperties 
        ///                 
        ///                         ListProperties is where we list all the users current properties     
        /// </summary> ******************************************************************************************************************

        public void ListProperties()
        {
            Message_text = ""; // just stops Message_text printing anything here 

            // call DisplayList, and get it too list Customers Property;
            UserInterface.DisplayList("Your Current Properties For Sale", Customer.NewUser[CurrentloggedInUser].NewProperty);
        }

        /// <summary> ******************************************************************************************************************
        ///                 OPTION 4 -           List bids                    List bids 
        ///                                 ListBids is where we list bids on a Property
        /// </summary> ******************************************************************************************************************

        public void ListBids() 
        {
            //call ChooseFromList, where we list all houses the user owns.
            int HouseNum = UserInterface.ChooseFromList(Customer.NewUser[CurrentloggedInUser].NewProperty);

            // use HouseNum too list all the current bids for that Property;
            UserInterface.DisplayList("Bids received: ", Customer.NewUser[CurrentloggedInUser].NewProperty[HouseNum].NewBid);
            
        }

        /// <summary> ******************************************************************************************************************
        ///                 OPTION 5 -           Sell to highest                  Sell too Highest
        ///                 
        ///                        SellHouse is where we sell a house too the highest bidder 
        /// </summary> ******************************************************************************************************************

        public void SellHouse() 
        {
            //call ChooseFromList, where we list all houses the user owns.
            int HouseNum = UserInterface.ChooseFromList(Customer.NewUser[CurrentloggedInUser].NewProperty);

            //UserHold holes the Userinfo class, just too keep the code a bit cleaner
            NewUserRegister.UserInfo UserHold = Customer.NewUser[CurrentloggedInUser];

            //Here we call HighestBid, HighestBid will order the Bidlist in ascending order
            DataCalculation.HighestBid(UserHold.NewProperty[HouseNum].NewBid);

            //we grab the first entry in the Bid Database, now that it's in order, This will be the biggest BID.
            //WINNERBID_NUM gets the user number 
            int WINNERBID_NUM = UserHold.NewProperty[HouseNum].NewBid[0].UserNum;

            // Here we grav all the data we need on the winning bidder and house
            string H_Address = UserHold.NewProperty[HouseNum].PropertyAddress;
            string H_PostCode = UserHold.NewProperty[HouseNum].PropertyPostcode;
            string H_Info = UserHold.NewProperty[HouseNum].PropertyDetails;
            int H_Type = UserHold.NewProperty[HouseNum].PropertyType;
            
            string OtherUserName = Customer.NewUser[WINNERBID_NUM].UserName;
            string OtherUserEmail = Customer.NewUser[WINNERBID_NUM].UserEmail;

            int SoldPrice = UserHold.NewProperty[HouseNum].NewBid[0].BidPrice;

            // This below is something we don't talk about
           // Customer.NewUser[WINNERBID_NUM].AddProperty(H_Address, H_PostCode, H_Info, H_Type);

            // once we have all the details in localdata, we then remove the house from the sellers databse
            UserHold.NewProperty[HouseNum].NewBid.RemoveAt(0);

            // call SaleTax too work out the tax that is payable 
            double TAX = DataCalculation.SaleTax(SoldPrice, H_Type, H_Info);

            // print all the details out
            UserInterface.Message($"System - " + H_Address + ", " + H_PostCode + ", " + H_Info + " SOLD too " + OtherUserName + " (" + OtherUserEmail + ") FOR $" + SoldPrice + "");
            UserInterface.Message($"Tax payable $"+ TAX.ToString());
        }

        /// <summary> ******************************************************************************************************************
        ///                             OPTION 6 - FOR SALE   AND     OPTION 7 -Place a bid   
        ///                             
        ///                         Below ListForSale and SearchHouses call the SearchHouses function
        /// </summary> ******************************************************************************************************************

        public void ListForSale()
        {
            SearchHouses(0);
        }

        public void BidOnHouse()
        {
            SearchHouses(1);
        }

        public void SearchHouses(int function) // SearchHouses if used in the two functions above, as too reduce duplicate code 
        {
            Message_text = "";

            // use GetInput too get the users current postcode they want too search for:
            string House_Postcode = UserInterface.GetInput("Postcode: ");
            
            // check if postcode is 4 long 
            bool CheckPostcode = DataCalculation.CheckLength(House_Postcode, 4);

            if (CheckPostcode == true) // if it is 4 char long
            {
                if (function == 0) // if we want too just list houses for sale 
                {
                    DataCalculation.DisplayProperty(House_Postcode, "Current Properties On Market", Customer);
                }
                else // for we want to bid on a Property;
                {
                    DataCalculation.BidProperty(House_Postcode, Customer);
                }
            }
            else // throw an error
            {
                Message_text = "System - ERROR: PostCode is not 4 characters long ";
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
            UserInterface.Message($"System -  " + Customer.NewUser[CurrentloggedInUser].UserName + " Successfully logged out");
            CurrentloggedInUser = -2; // by setting CurrentloggedInUser to -2, the user is no long logged in ;
          
        }


        /// <summary> ******************************************************************************************************************
        ///                  Run program               Run program              Run program              Run program
        /// </summary> ******************************************************************************************************************

        //Here we add our menus too the menu and submenu objects
        public void Run()
        {
            // add main menu
            Menu.Add("Register as new Customer", AddCustomer);
            Menu.Add("Login as existing Custiner", LoginCustomer);

            // add Submenu
            SubMenu.Add("Register new land for sale", NewLandRegister);
            SubMenu.Add("Register a new house for sale", NewHouseRegister);
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
            while (true)
            {
                if (CurrentloggedInUser == -2) // it's coded this way, so the WELCOME text is only shown once
                {
                    IconsNfun.FrontGreeting();
                    
                    CurrentloggedInUser = -1;
                }
                else if (CurrentloggedInUser == -1) // Display frount page 
                {
                    Menu.Display();
                }
                else // Display the User Sub Menu
                {
                   // IconsNfun.UserMenuGreeting(Customer.NewUser[CurrentloggedInUser].UserName);
                    SubMenu.Display();
                }

                UserInterface.Message(Message_text); // print Message_text strings
            }
         }

        // MAIN RUN FUNCTION!
        static void Main(string[] args)
        {
            TestUserInterface test = new TestUserInterface();
            test.Run();           
        }
     }

   
}

