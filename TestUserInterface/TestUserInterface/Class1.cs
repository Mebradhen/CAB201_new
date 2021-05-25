using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUserInterface
{
    class Class1
    {


    }
}

/*
 * 
 *         // create a new list
        public List<UserInfo> NewUser = new List<UserInfo>();

        public void AddUser(string New_name, string New_email, string New_Password)
        {
            NewUser.Add(new UserInfo(New_name, New_email, New_Password));
        }

        // CheckUserDataBase
        // This here is what we call when we want to check info in the database. 
        public int CheckUserDataBase(string InputData, int checks)
        {
            int exist = -1;

            switch (checks) // checks will have an int that user inputs when called. 
            {
                case 1: exist = NewUser.FindIndex(ent => ent.UserEmail == InputData); break; // if 1, check if emails match
                case 2: exist = NewUser.FindIndex(ent => ent.UserPassword == InputData); break; // if 2, check if passowrds match
            }      
            return exist; // return the number of where there matche... aka the usuer number 
        }








            // Add a new Property Too The List
            public void AddProperty(string New_Address, string New_Postcode, string New_Details, int New_Type)
            {
                NewProperty.Add(new PropertyInfo(New_Address, New_Postcode, New_Details, New_Type));
            }

            // a simple return function, this is used to check info into the data base 
            public int CheckPropertyDataBase(string InputData)
            {
                int exist = -1;
                exist = NewProperty.FindIndex(ent => ent.PropertyAddress == InputData); // check if InputData is the same as PropertyAddress
                return exist; // return postiton of found data in list 
            }



  public override string ToString() // The ToString override allows us too return Property details in the DisplayList function. 
                {
                    string info =  ""; 

                    if(PropertyType == 0)
                    {
                        info = "m²";
                    }

                    return "" + PropertyAddress + ", " + PropertyPostcode + ", " + PropertyDetails + " " + info;
                }





      public override string ToString() // Once again call ToString too display Name, email and bid from 
            {
                    string Name = UserObject.NewUser[UserNum].UserName.ToString();
                    string email = UserObject.NewUser[UserNum].UserEmail.ToString();

                return "" + Name + " (" + email + ") With $" + BidPrice.ToString() + " ";
                }
            }

            // Add a new Bid Too The List
            public void AddBid(int New_Num, NewUserRegister New_Object, int New_Bid)
            {
                NewBid.Add(new BidInfo(New_Num, New_Object, New_Bid));
            }  

 * 
 * */ 