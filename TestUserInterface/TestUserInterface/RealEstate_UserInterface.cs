using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


namespace TestUserInterface
{
    /// <summary> ******************************************************************************************************************
    ///     
    ///                                             Welcome to User_UserInterface.cs
    ///                                                             
    ///                         This File UserInterface methods that are useful for the Real Estate Industry
    ///                                                                               
    /// </summary>******************************************************************************************************************

    public class RealEstateUserInterface
    {
        // Get a GetPostcode and validate the number 
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

        // get a Bid and validate the number has a Correct input
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

        // get a new Address input and validate its a new address 
        public static string GetNewPropertyAddress(List<User> user)
        {
            string Property_Address = UserInterface.GetInput("Address");

            int Checkhouse = -1;

            for (int i = 0; i < user.Count; i++) // for loop runs through all registered users
            {
                Checkhouse = user[i].Properties.FindIndex(ent => ent.Address == Property_Address);
            }

            if (Checkhouse != -1)
            {
                UserInterface.Error("Property is already Registered");
                return null;
            }
            return Property_Address;
        }




        // Get New info about Land
        public static Land GetNewLandInfo(User user, Users users)
        {
            // Make sure we are getting a new property address
            string PropertyAddress = RealEstateUserInterface.GetNewPropertyAddress(users.UserList);

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
            return new Land() { Address = PropertyAddress, Postcode = PropertyPostcode, AreaInSquareMetres = Property_Area, Owner = user};
        }



        // get new infomaton about house
        public static House GetNewHouseInfo(User user, Users users)
        {
            //use PropertyAddress of the Get class, too get an input and check it with database
            string PropertyAddress = RealEstateUserInterface.GetNewPropertyAddress(users.UserList);

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
            return new House() { Address = PropertyAddress, Postcode = PropertyPostcode, Info = House_desc, Owner = user};
        }

    }
}