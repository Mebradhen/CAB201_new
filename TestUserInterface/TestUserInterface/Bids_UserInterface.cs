using System;
using System.Collections.Generic;
using System.Linq;

namespace TestUserInterface
{

    /// <summary> ******************************************************************************************************************
    ///     
    ///                                             Welcome to Bids_UserInterface.cs
    ///                                                             
    ///                                   This File holes majority of classes / methords too do with Bids
    ///                                                                               
    /// </summary>******************************************************************************************************************

    // NewBidRegister is yet another class, where other scripts can call it to make an object that stores a Properties bids
    public class Bid
    {
        public User Owner { get; set; }
        public int BidPrice { get; set; }
        public override string ToString()
        {
            return "$" + BidPrice.ToString();
        }
    }

    // The bids class holes all our methords for Bids 
    public class Bids
    {
        public List<Bid> BidList { get; private set; } = new List<Bid>();

        public int Count { get { return BidList.Count; } }

        public void SortBids()
        {
            BidList.Sort((x, y) => y.BidPrice.CompareTo(x.BidPrice));
        }

        public int GetHighestBidPrice()
        {
            return GetHighestBid()?.BidPrice ?? 0;
        }
        public Bid GetHighestBid()
        {
            if (BidList.Count == 0) return null;

            SortBids();

            return BidList[0];
        }

        public void Add(Bid bid)
        {
            BidList.Add(bid);
        }
    }
}