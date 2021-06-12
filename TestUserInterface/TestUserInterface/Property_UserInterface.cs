using System;
using System.Collections.Generic;
using System.Linq;

namespace TestUserInterface
{
    /// <summary> ******************************************************************************************************************
    ///     
    ///                                             Welcome to Property_UserInterface.cs
    ///                                                             
    ///                              This File holes majority of classes / methords too do with Properties
    ///                                                                               
    /// </summary>******************************************************************************************************************



    //ListDisplay is a View Model class we call, when we generate a display list for serching 
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

    //Property class is for storing info on Properties 
    public abstract class Property
    {
        public string Address { get; set; }
        public string Postcode { get; set; }
        public User Owner { get; set; }
        public double SalePrice { get; set; }

        public Bids Bids = new Bids();
        public abstract double CalculateSalesTax();
        // place bid
        public void PlaceBid(User user)
        {
            User Bidder = user;

            double BidNum = RealEstateUserInterface.GetPropertyBid(Bids);

            BidNum = Math.Round(BidNum);

            if (BidNum >= 0)
            {
                Bids.Add(new Bid { Bidder = Bidder, BidPrice = (int)BidNum });
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


 

}