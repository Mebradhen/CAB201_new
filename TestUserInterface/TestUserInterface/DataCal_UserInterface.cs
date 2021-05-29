using System;
using System.Collections.Generic;
using System.Linq;

namespace TestUserInterface
{
    /// <summary> ******************************************************************************************************************
    /// 
    ///             DataCalculation is a class used for methods that relate to mutilating and validating data  
    ///          
    ///                                                     A true classic lol                                                     
    ///                            But yes, maybe not the best class for OOP but a needed one for neatness
    ///
    /// </summary>******************************************************************************************************************

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
        public static List<PropertyViewModel> GenerateDisplayList(string postCode, List<User> userList)
        {
            List<PropertyViewModel> NewDisplayList = new List<PropertyViewModel>(); //the new list of houses 

            for (int i = 0; i < userList.Count; i++) // for loop runs through all registered users   
            {
                for (int ii = 0; ii < userList[i].Properties.Count; ii++) // for loop runs through all Properties
                {
                    if (userList[i].Properties[ii].Postcode == postCode) // check if the current property has the wanted postcode
                    {
                        //if true, then add the needed info to a new object called SaveList, and add that to the NewDisplayList
                        NewDisplayList.Add(new PropertyViewModel { Owner = userList[i].Properties[ii].Owner, HouseNum = ii, Houseinfo = userList[i].Properties[ii].ToString() });
                    }
                }
            }
            return NewDisplayList;
        }

    }
}